using FantasyModuleParser.Importer.Enums;
using FantasyModuleParser.Spells.Models;
using System;
using System.IO;

namespace FantasyModuleParser.Importer.Spells
{
    public class ImportSpellPDF : ImportSpellBase, IImportSpell
    {
        public SpellModel ImportTextToSpellModel(string importData)
        {
            SpellModel resultSpellModel = new SpellModel();

            string formattedImportData = FormatSpellPDFService.FormatData(importData);

            ImportSpellState importStatEnum = ImportSpellState.INITIAL;

            StringReader stringReader = new StringReader(formattedImportData);
            string line = "";
            while ((line = stringReader.ReadLine()) != null)
            {
                //importStatEnum = getImportStateEnum(importStatEnum, importData);
                switch (importStatEnum)
                {
                    case ImportSpellState.INITIAL:
                        resultSpellModel.SpellName = line;
                        importStatEnum = ImportSpellState.LEVEL_SCHOOL;
                        break;
                    case ImportSpellState.LEVEL_SCHOOL:
                        resultSpellModel.SpellLevel = ParseSpellLevel(line);
                        resultSpellModel.SpellSchool = ParseSpellSchool(line);
                        resultSpellModel.IsRitual = CheckIfRitual(line);
                        importStatEnum = ImportSpellState.CASTING_TIME;
                        break;
                    case ImportSpellState.CASTING_TIME:
                        ParseCastingTime(line, resultSpellModel);
                        importStatEnum = ImportSpellState.RANGE;
                        break;
                    case ImportSpellState.RANGE:
                        ParseRange(line, resultSpellModel);
                        importStatEnum = ImportSpellState.COMPONENTS;
                        break;
                    case ImportSpellState.COMPONENTS:
                        ParseComponents(line, resultSpellModel);
                        importStatEnum = ImportSpellState.DURATION;
                        break;
                    case ImportSpellState.DURATION:
                        ParseDuration(line, resultSpellModel);
                        importStatEnum = ImportSpellState.CAST_BY;
                        break;
                    case ImportSpellState.CAST_BY:
                        if (line.StartsWith("Classes:"))
                        {
                            ParseCastByClasses(line, resultSpellModel);
                        }
                        else
                        {
                            resultSpellModel.Description = line + " "; // No character class to associate with
                        }
                        importStatEnum = ImportSpellState.DESCRIPTION;
                        break;
                    case ImportSpellState.DESCRIPTION:
                        if (line.EndsWith(".", StringComparison.Ordinal))
                        {
                            resultSpellModel.Description += line;
                            resultSpellModel.Description += Environment.NewLine;
                        }
                        else if (line.StartsWith("At Higher Levels.", StringComparison.Ordinal))
                        {
                            resultSpellModel.Description += "**At Higher Levels.**";
                            resultSpellModel.Description += line.Substring(17);
                        }
                        else
                        {
                            resultSpellModel.Description += line;
                        }
                        break;
                    default:
                        break;
                }
            }

            // Trim off the Description, as it will have a leading newLine character
            resultSpellModel.Description = resultSpellModel.Description.TrimEnd();

            return resultSpellModel;
        }
    }
}
