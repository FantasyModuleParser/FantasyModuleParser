using FantasyModuleParser.Importer.Enums;
using System;
using System.IO;
using System.Text;

namespace FantasyModuleParser.Importer.Spells
{
    public class FormatSpellPDFService
    {
        public FormatSpellPDFService()
        {

        }

        public static string FormatData(string importData)
        {
            StringBuilder resultStringBuilder = new StringBuilder();
            ImportSpellState importStatEnum = ImportSpellState.INITIAL;

            StringReader stringReader = new StringReader(importData);
            string line = "";
            while ((line = stringReader.ReadLine()) != null)
            {
                //importStatEnum = getImportStateEnum(importStatEnum, importData);
                switch (importStatEnum)
                {
                    case ImportSpellState.INITIAL:
                        resultStringBuilder.Append(line).Append(Environment.NewLine);
                        importStatEnum = ImportSpellState.LEVEL_SCHOOL;
                        break;
                    case ImportSpellState.LEVEL_SCHOOL:
                        resultStringBuilder.Append(line).Append(Environment.NewLine);
                        importStatEnum = ImportSpellState.CASTING_TIME;
                        break;
                    case ImportSpellState.CASTING_TIME:
                        resultStringBuilder.Append(line).Append(Environment.NewLine);
                        importStatEnum = ImportSpellState.RANGE;
                        break;
                    case ImportSpellState.RANGE:
                        resultStringBuilder.Append(line).Append(Environment.NewLine);
                        importStatEnum = ImportSpellState.COMPONENTS;
                        break;
                    case ImportSpellState.COMPONENTS:
                        if (line.StartsWith("Duration:"))
                        {
                            importStatEnum = ImportSpellState.DURATION;
                            resultStringBuilder.Append(Environment.NewLine).Append(line);
                        }
                        else
                            resultStringBuilder.Append(line).Append(" ");
                        break;
                    case ImportSpellState.DURATION:
                        resultStringBuilder.Append(Environment.NewLine).Append(line);
                        importStatEnum = ImportSpellState.CAST_BY;
                        break;
                    case ImportSpellState.CAST_BY:
                        //if (line.StartsWith("Classes:"))
                        //    ParseCastByClasses(line, resultSpellModel);
                        //else
                        //    resultSpellModel.Description = line; // No character class to associate with
                        resultStringBuilder.Append(Environment.NewLine).Append(line);
                        importStatEnum = ImportSpellState.DESCRIPTION;
                        break;
                    case ImportSpellState.DESCRIPTION:
                        resultStringBuilder.Append(' ').Append(line);
                        break;
                    default:
                        //no-op
                        break;
                }
            }
            return resultStringBuilder.ToString().TrimEnd();
        }
    }
}
