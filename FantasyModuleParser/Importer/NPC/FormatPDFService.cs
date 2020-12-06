using System.IO;
using System.Text;

namespace FantasyModuleParser.Importer.NPC
{
    enum ImportNPCState
    {
        NO_STATE,
        CHALLENGE,
        INNATE_SPELLCASTING,
        INNATE_SPELLCASTING_SELECTION,
        SPELLCASTING,
        ACTIONS,
        DAMAGE_RESIST,
        DAMAGE_IMMUNITY,
        SENSES,
        TRAITS,
        REACTIONS
    }
    public class FormatPDFService : IFormatContentService
    {
        
        public string FormatImportContent(string importTextContent)
        {
            StringBuilder formattedTextContent = new StringBuilder();
            StringReader stringReader = new StringReader(importTextContent);
            string line = "";

            ImportNPCState importNPCState = ImportNPCState.NO_STATE;

            while ((line = stringReader.ReadLine()) != null)
            {

                switch (importNPCState)
                {
                    case ImportNPCState.SENSES:
                        {
                            if (line.StartsWith("Languages"))
                            {
                                formattedTextContent.Append("\n").Append(line);
                                importNPCState = ImportNPCState.NO_STATE;
                            }
                            else
                            {
                                formattedTextContent.Append(line).Append(" ");

                            }
                        }
                        break;
                    case ImportNPCState.DAMAGE_RESIST:
                        {
                            if (line.StartsWith("Condition Immunities"))
                            {
                                importNPCState = ImportNPCState.NO_STATE;
                                formattedTextContent.Append("\n").Append(line);
                            }
                            else if (line.StartsWith("Damage Immunities"))
                            {
                                importNPCState = ImportNPCState.DAMAGE_IMMUNITY;
                                formattedTextContent.Append("\n").Append(line);
                            }
                            else if (line.StartsWith("Senses"))
                            {
                                importNPCState = ImportNPCState.SENSES;
                                formattedTextContent.Append("\n").Append(line);
                            }
                            else
                            {
                                formattedTextContent.Append(line).Append(" ");
                            }
                        }
                        break;
                    case ImportNPCState.DAMAGE_IMMUNITY:
                        { 
                            if (line.StartsWith("Condition Immunities"))
                            {
                                importNPCState = ImportNPCState.NO_STATE;
                                formattedTextContent.Append("\n").Append(line);
                            }
                            else if (line.StartsWith("Senses"))
                            {
                                importNPCState = ImportNPCState.NO_STATE;
                                formattedTextContent.Append("\n").Append(line).Append("\n");
                            }
                            else if (line.StartsWith("Languages"))
                            {
                                importNPCState = ImportNPCState.NO_STATE;
                                formattedTextContent.Append("\n").Append(line);
                            }
                            else
                            {
                                formattedTextContent.Append(line).Append(" ");
                            } 
                        }
                        break;
                    case ImportNPCState.CHALLENGE:
                        { 
                            formattedTextContent.Append(line);
                            if (line.EndsWith("one target.") || line.EndsWith("one creature."))
                            {
                                continue;
                            }
                            if (line.EndsWith("."))
                            {
                                formattedTextContent.Append(" \n");
                            }
                            else if (line.EndsWith("prepared:"))
                            {
                                formattedTextContent.Append("\\r");
                            }
                            else if (line.ToLower().Equals("actions"))
                            {
                                formattedTextContent.Append("\n");
                                importNPCState = ImportNPCState.ACTIONS;
                            }
                            else if (line.StartsWith("Innate Spellcasting"))
                            {
                                importNPCState = ImportNPCState.INNATE_SPELLCASTING;
                            }
                            else if (line.StartsWith("Spellcasting. "))
                            {
                                importNPCState = ImportNPCState.SPELLCASTING;
                            }
                            else
                            {
                                formattedTextContent.Append(" ");
                            } 
                        }
                        break;
                    case ImportNPCState.INNATE_SPELLCASTING:
                        { 
                            if (line.StartsWith("Spellcasting. "))
                            {
                                formattedTextContent.Append("\n").Append(line);
                                importNPCState = ImportNPCState.SPELLCASTING;
                            }
                            if (line.ToLower().Equals("actions"))
                            {
                                formattedTextContent.Append("\n").Append(line).Append("\n");
                                importNPCState = ImportNPCState.ACTIONS;
                            }
                            else if (line.StartsWith("Spellcasting. "))
                            {
                                importNPCState = ImportNPCState.SPELLCASTING;
                            }
                            else if (line.EndsWith(":"))
                            {
                                formattedTextContent.Append(line);
                            }
                            else if (line.StartsWith("At will") || line.StartsWith("5/day each:")
                                || line.StartsWith("4/day each:") || line.StartsWith("3/day each:")
                                || line.StartsWith("2/day each:") || line.StartsWith("1/day each:"))
                            {
                                formattedTextContent.Append("\\r").Append(line);
                                importNPCState = ImportNPCState.INNATE_SPELLCASTING_SELECTION;
                            }
                            else
                            {
                                formattedTextContent.Append(" ").Append(line).Append(" ");
                            }
                        }
                        break;
                    case ImportNPCState.INNATE_SPELLCASTING_SELECTION:
                        {
                            if (line.StartsWith("Spellcasting. "))
                            {
                                formattedTextContent.Append("\n").Append(line);
                                importNPCState = ImportNPCState.SPELLCASTING;
                            }
                            else if (checkIfLineMaybeTrait(line))
                            {
                                formattedTextContent.Append("\n").Append(line);
                                importNPCState = ImportNPCState.TRAITS;
                            }
                            else if (line.StartsWith("At will") || line.StartsWith("5/day")
                                || line.StartsWith("4/day") || line.StartsWith("3/day")
                                || line.StartsWith("2/day") || line.StartsWith("1/day"))
                                formattedTextContent.Append("\\r").Append(line);
                            else if (line.ToLower().Equals("actions"))
                            {
                                formattedTextContent.Append("\n").Append(line).Append("\n");
                                importNPCState = ImportNPCState.ACTIONS;
                            }
                            else
                            {
                                formattedTextContent.Append(" ").Append(line);
                            }
                        }
                        break;
                    case ImportNPCState.SPELLCASTING:
                        {
                            if (line.ToLower().Equals("actions"))
                            {
                                formattedTextContent.Append("\n").Append(line).Append("\n");
                                importNPCState = ImportNPCState.ACTIONS;
                            }
                            else if (checkIfLineMaybeTrait(line))
                            {
                                formattedTextContent.Append("\n").Append(line);
                                importNPCState = ImportNPCState.TRAITS;
                            }
                            else if (line.StartsWith("Cantrips") || line.StartsWith("1st level")
                                || line.StartsWith("2nd level") || line.StartsWith("3rd level")
                                || line.StartsWith("4th level") || line.StartsWith("5th level")
                                || line.StartsWith("6th level") || line.StartsWith("7th level")
                                || line.StartsWith("8th level") || line.StartsWith("9th level"))
                            {
                                formattedTextContent.Append("\\r").Append(line);
                            }
                            else
                            {
                                formattedTextContent.Append(" ").Append(line);
                            }
                        }
                        break;
                    case ImportNPCState.TRAITS:
                        {
                            if (line.ToLower().Equals("actions"))
                            {
                                formattedTextContent.Append("\n").Append(line).Append("\n");
                                importNPCState = ImportNPCState.ACTIONS;
                            }
                            else if (checkIfLineMaybeTrait(line))
                            {
                                formattedTextContent.Append("\n").Append(line);
                            }
                            else
                            {
                                formattedTextContent.Append(" ").Append(line);
                            }
                        }
                        break;
                    case ImportNPCState.ACTIONS:
                        {
                            formattedTextContent.Append(line);
                            if (line.EndsWith("one target.") || line.EndsWith("one creature."))
                            {
                                continue;
                            }
                            // The ':' character is a result from the ultimate_tyrant parsing
                            if (line.EndsWith(".") || (line.EndsWith(":") && !line.EndsWith("Hit:")))
                            {
                                formattedTextContent.Append(" \n");
                            }
                            else if (line.EndsWith("prepared:"))
                            {
                                formattedTextContent.Append("\\r");
                            }
                            else if (line.ToLower().Equals("legendary actions"))
                            {
                                formattedTextContent.Append("\n");

                                // In order to use the Engineer Suite Parser, the first Legendary Actions line is expected to begin with Options: 
                                formattedTextContent.Append("Options. ");
                            }
                            else if (line.ToLower().Equals("reactions"))
                            {
                                formattedTextContent.Append("\n");
                                importNPCState = ImportNPCState.REACTIONS;
                            }
                            else
                            {
                                formattedTextContent.Append(" ");
                            }
                        }
                        break;
                    case ImportNPCState.REACTIONS:
                        {
                            if (checkIfLineMaybeTrait(line))
                                formattedTextContent.Append("\n").Append(line).Append(" ");
                            else if (line.ToLower().Equals("legendary actions"))
                            {
                                formattedTextContent.Append("\n").Append(line).Append("\n");

                                // In order to use the Engineer Suite Parser, the first Legendary Actions line is expected to begin with Options: 
                                formattedTextContent.Append("Options. ");
                            }
                            else
                                formattedTextContent.Append(line).Append(" ");
                        }
                        break;
                    case ImportNPCState.NO_STATE:
                        { 
                            if (line.StartsWith("Challenge "))
                            {
                                importNPCState = ImportNPCState.CHALLENGE;
                                formattedTextContent.Append("\n").Append(line).Append("\n");
                                continue;
                            }
                            if (line.StartsWith("Senses"))
                            {
                                importNPCState = ImportNPCState.SENSES;
                                formattedTextContent.Append("\n").Append(line).Append(" ");
                                continue;
                            }
                            if (line.StartsWith("Damage Immunities"))
                            {
                                importNPCState = ImportNPCState.DAMAGE_IMMUNITY;
                                formattedTextContent.Append(line).Append(" ");
                                continue;
                            }
                            if (line.StartsWith("Damage Resistance"))
                            {
                                importNPCState = ImportNPCState.DAMAGE_RESIST;
                                formattedTextContent.Append(line).Append(" ");
                                continue;
                            }
                            formattedTextContent.Append(line).Append("\n");
                        }
                        break;
                    default:
                        break;
                }
            }
            return formattedTextContent.ToString();
        }
        private bool checkIfLineMaybeTrait(string line)
        {
            string[] strArray = line.Split(' ');
            for (int idx = 0; idx < 5; idx++)
            {
                if (idx >= strArray.Length)
                    return false;

                // The logic behind this is Trait Names are typically capitialized (e.g. Flyby., Shadow Step.)
                // If the string found contains a period AND is lowercase, then it's very likely it's not a new trait (e.g. lowered., location.)
                if (strArray[idx].EndsWith("."))
                    return !strArray[idx].ToLower().Equals(strArray[idx]);
            }
            return false;
        }
    }
}
