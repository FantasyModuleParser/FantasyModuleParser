using FantasyModuleParser.Importer.Enums;
using System;
using System.IO;
using System.Text;

namespace FantasyModuleParser.Importer.Spells
{
    public class FormatSpellDnDBeyondService
    {
        public FormatSpellDnDBeyondService()
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
                        importStatEnum = ImportSpellState.LEVEL;
                        break;
                    case ImportSpellState.LEVEL:
                        if (line.Equals("LEVEL"))
                        {
                            resultStringBuilder.Append(line).Append(Environment.NewLine);
                            break;
                        }
                        else resultStringBuilder.Append(line).Append(Environment.NewLine);
                        importStatEnum = ImportSpellState.CASTING_TIME;
                        break;
                    case ImportSpellState.CASTING_TIME:
                        if (line.Equals("CASTING TIME"))
                        {
                            resultStringBuilder.Append(line).Append(Environment.NewLine);
                            break;
                        }
                        else resultStringBuilder.Append(line).Append(Environment.NewLine);
                        importStatEnum = ImportSpellState.RANGE;
                        break;
                    case ImportSpellState.RANGE:
                        if (line.Equals("RANGE/AREA"))
                        {
                            resultStringBuilder.Append(line).Append(Environment.NewLine);
                            break;
                        }
                        else resultStringBuilder.Append(line).Append(Environment.NewLine);
                        importStatEnum = ImportSpellState.COMPONENTS;
                        break;
                    case ImportSpellState.COMPONENTS:
                        if (line.Equals("COMPONENTS"))
                        {
                            resultStringBuilder.Append(line).Append(Environment.NewLine);
                            break;
                        }
                        else resultStringBuilder.Append(line).Append(Environment.NewLine);
                        importStatEnum = ImportSpellState.DURATION;
                        break;
                    case ImportSpellState.DURATION:
                        if (line.Equals("DURATION"))
                        {
                            resultStringBuilder.Append(line).Append(Environment.NewLine);
                            break;
                        }
                        else resultStringBuilder.Append(line).Append(Environment.NewLine);
                        importStatEnum = ImportSpellState.SCHOOL;
                        break;
                    case ImportSpellState.SCHOOL:
                        if (line.Equals("SCHOOL"))
                        {
                            resultStringBuilder.Append(line).Append(Environment.NewLine);
                            break;
                        }
                        else resultStringBuilder.Append(line).Append(Environment.NewLine);
                        importStatEnum = ImportSpellState.ATTACK;
                        break;
                    case ImportSpellState.ATTACK:
                        if (line.Equals("ATTACK/SAVE"))
                        {
                            resultStringBuilder.Append(line).Append(Environment.NewLine);
                            break;
                        }
                        else resultStringBuilder.Append(line).Append(Environment.NewLine);
                        importStatEnum = ImportSpellState.DAMAGE;
                        break;
                    case ImportSpellState.DAMAGE:
                        if (line.Equals("DAMAGE/EFFECT"))
                        {
                            resultStringBuilder.Append(line).Append(Environment.NewLine);
                            break;
                        }
                        else resultStringBuilder.Append(line).Append(Environment.NewLine);
                        importStatEnum = ImportSpellState.DESCRIPTION;
                        break;
                    case ImportSpellState.DESCRIPTION:
                        if (string.IsNullOrEmpty(line))
                        {
                            break;
                        }
                        else if (line.StartsWith("*"))
                        {
                            resultStringBuilder.Append(line).Append(Environment.NewLine);
                            importStatEnum = ImportSpellState.MATERIAL;
                        }
                        else if (line.StartsWith("Spell Tags"))
                        {
                            importStatEnum = ImportSpellState.TAGS;
                        }
                        else resultStringBuilder.Append(line).Append(Environment.NewLine);
                        break;
                    case ImportSpellState.MATERIAL:
                        if (line.StartsWith("Spell Tags"))
                        {
                            importStatEnum = ImportSpellState.TAGS;
                            break;
                        }
                        else if (string.IsNullOrEmpty(line))
                        {
                            break;
                        }
                        else resultStringBuilder.Append(line).Append(Environment.NewLine);
                        break;
                    case ImportSpellState.TAGS:
                        if (string.IsNullOrEmpty(line))
                        {
                            importStatEnum = ImportSpellState.CAST_BY;
                            break;
                        }
                        else if (line.StartsWith("Spell Tags"))
                        {
                            resultStringBuilder.Append(line).Append(Environment.NewLine);
                            break;
                        }
                        break;
                    case ImportSpellState.CAST_BY:
                        if (line.StartsWith("Available For:"))
                        {
                            resultStringBuilder.Append(line);
                        }
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
