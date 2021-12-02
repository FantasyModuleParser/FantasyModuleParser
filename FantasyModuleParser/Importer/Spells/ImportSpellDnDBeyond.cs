using FantasyModuleParser.Importer.Enums;
using FantasyModuleParser.Spells.Models;
using System;
using System.Globalization;
using System.IO;

namespace FantasyModuleParser.Importer.Spells
{
    public class ImportSpellDnDBeyond : ImportSpellBase, IImportSpell
    {
        public SpellModel ImportTextToSpellModel(string importData)
        {
            SpellModel resultSpellModel = new SpellModel();

            string formattedImportData = FormatSpellDnDBeyondService.FormatData(importData);

            ImportSpellState importStatEnum = ImportSpellState.INITIAL;

            StringReader stringReader = new StringReader(formattedImportData);
            string line = "";
            while ((line = stringReader.ReadLine()) != null)
            {
                //importStatEnum = getImportStateEnum(importStatEnum, importData);
                switch (importStatEnum)
                {
                    case ImportSpellState.INITIAL:
                        if (line.ToLower().StartsWith("homebrew"))
                        {
                            resultSpellModel.SpellName = line.Remove(0, 9).Trim();
                        }
                        else if (line.ToLower().EndsWith("concentration"))
                        {
                            resultSpellModel.SpellName = line.Remove(line.Length - 14).Trim();
                        }
                        else if (line.ToLower().EndsWith("ritual"))
                        {
                            resultSpellModel.SpellName = line.Remove(line.Length - 7).Trim();
                        }
                        else resultSpellModel.SpellName = line.Trim();
                        importStatEnum = ImportSpellState.LEVEL;
                        break;
                    case ImportSpellState.LEVEL:
                        if (line.ToLower(CultureInfo.CurrentCulture).Contains("level"))
                        {
                            break;
                        }
                        else resultSpellModel.SpellLevel = ParseSpellLevel(line);
                        importStatEnum = ImportSpellState.CASTING_TIME;
                        break;
                    case ImportSpellState.CASTING_TIME:
                        if (line.ToLower(CultureInfo.CurrentCulture).Contains("casting time"))
                        {
                            break;
                        }
                        ParseDnDCastingTime(line, resultSpellModel);
                        importStatEnum = ImportSpellState.RANGE;
                        break;
                    case ImportSpellState.RANGE:
                        if (line.ToLower(CultureInfo.CurrentCulture).Contains("range/area"))
                        {
                            break;
                        }
                        ParseDnDRange(line, resultSpellModel);
                        importStatEnum = ImportSpellState.COMPONENTS;
                        break;
                    case ImportSpellState.COMPONENTS:
                        if (line.ToLower(CultureInfo.CurrentCulture).Contains("components"))
                        {
                            break;
                        }
                        ParseDnDComponents(line, resultSpellModel);
                        importStatEnum = ImportSpellState.DURATION;
                        break;
                    case ImportSpellState.DURATION:
                        if (line.ToLower(CultureInfo.CurrentCulture).Contains("duration"))
                        {
                            break;
                        }
                        ParseDnDDuration(line, resultSpellModel);
                        importStatEnum = ImportSpellState.SCHOOL;
                        break;
                    case ImportSpellState.SCHOOL:
                        if (line.ToLower(CultureInfo.CurrentCulture).Contains("school"))
                        {
                            break;
                        }
                        resultSpellModel.SpellSchool = ParseSpellSchool(line);
                        importStatEnum = ImportSpellState.ATTACK;
                        break;
                    case ImportSpellState.ATTACK:
                        if (line.ToLower(CultureInfo.CurrentCulture).Equals("attack/save"))
                        {
                            break;
                        }
                        if (!line.ToLower(CultureInfo.CurrentCulture).Equals("attack/save"))
                        {
                            importStatEnum = ImportSpellState.DAMAGE;
                            break;
                        }
                        break;
                    case ImportSpellState.DAMAGE:
                        if (line.ToLower(CultureInfo.CurrentCulture).Equals("damage/effect"))
                        {
                            break;
                        }
                        if (!line.ToLower(CultureInfo.CurrentCulture).Equals("damage/effect"))
                        {
                            importStatEnum = ImportSpellState.DESCRIPTION;
                            break;
                        }
                        break;
                    case ImportSpellState.DESCRIPTION:
                        if (line.StartsWith("At Higher Levels.", StringComparison.Ordinal))
                        {
                            resultSpellModel.Description += Environment.NewLine;
                            resultSpellModel.Description += "**At Higher Levels.**";
                            resultSpellModel.Description += line.Substring(17);
                        }
                        else if (line.StartsWith("Spell Tags"))
                        {
                            importStatEnum = ImportSpellState.TAGS;
                            break;
                        }
                        else if (line.StartsWith("Available For:"))
                        {
                            importStatEnum = ImportSpellState.CAST_BY;
                            break;
                        }
                        else if (line.EndsWith(".", StringComparison.Ordinal))
                        {
                            resultSpellModel.Description += line;
                            resultSpellModel.Description += Environment.NewLine;
                            break;
                        }
                        else if (line.StartsWith("*"))
                        {
                            resultSpellModel.ComponentText = line.Remove(0, 4);
                            importStatEnum = ImportSpellState.TAGS;
                            break;
                        }
                        else
                        {
                            resultSpellModel.Description += line;
                        }
                        resultSpellModel.Description = resultSpellModel.Description.Trim();
                        importStatEnum = ImportSpellState.TAGS;
                        break;
                    case ImportSpellState.TAGS:
                        if (line.StartsWith("Spell Tags"))
                        {
                            importStatEnum = ImportSpellState.CAST_BY;
                            break;
                        }
                        importStatEnum = ImportSpellState.CAST_BY;
                        break;
                    case ImportSpellState.CAST_BY:
                        if (line.StartsWith("Available For:"))
                        {
                            break;
                        }
                        break;
                    default:
                        break;
                }
            }

            return resultSpellModel;
        }
    }
}
