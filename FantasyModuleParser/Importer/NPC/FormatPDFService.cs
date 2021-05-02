using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private const char spaceChar = ' ';
        private const string periodStr = ".";
        private const string colonStr = ":";
        private const string slashSlashRStr = "\\r";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="importTextContent"></param>
        /// <returns></returns>
        public string FormatImportContent(string importTextContent)
        {
            StringBuilder formattedTextContent = new StringBuilder();
            StringReader stringReader = new StringReader(importTextContent);
            string line = "";

            List<string> innateList = new List<string>() { "At will", "1/day each:", "2/day each:", "3/day each:", "4/day each:", "5/day each:" };
            List<string> innateSelectionList = new List<string>() { "At will", "1/day", "2/day", "3/day", "4/day", "5/day" };
            List<string> spellList = new List<string>() { "Cantrips", "1st level", "2nd level", "3rd level",
                "4th level", "5th level", "6th level", "7th level", "8th level", "9th level" };


			ImportNPCState importNPCState = ImportNPCState.NO_STATE;

            while ((line = stringReader.ReadLine()) != null)
            {
                line = line.Trim();
                if (string.IsNullOrEmpty(line)) { continue; }

                switch (importNPCState)
                {
                    case ImportNPCState.SENSES:
                        {
                            if (line.StartsWith("Languages", StringComparison.OrdinalIgnoreCase))
                            {
								_ = formattedTextContent.Append(Environment.NewLine).Append(line);
                                importNPCState = ImportNPCState.NO_STATE;
                            }
                            else
                            {
                                _ = formattedTextContent.Append(line).Append(spaceChar);
                            }
                        }
                        break;
                    case ImportNPCState.DAMAGE_RESIST:
                        {
                            if (line.StartsWith("Condition Immunities", StringComparison.OrdinalIgnoreCase))
                            {
                                importNPCState = ImportNPCState.NO_STATE;
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line);
                            }
                            else if (line.StartsWith("Damage Immunities", StringComparison.OrdinalIgnoreCase))
                            {
                                importNPCState = ImportNPCState.DAMAGE_IMMUNITY;
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line);
                            }
                            else if (line.StartsWith("Senses", StringComparison.OrdinalIgnoreCase))
                            {
                                importNPCState = ImportNPCState.SENSES;
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line);
                            }
                            else
                            {
                                _ = formattedTextContent.Append(line).Append(spaceChar);
                            }
                        }
                        break;
                    case ImportNPCState.DAMAGE_IMMUNITY:
                        { 
                            if (line.StartsWith("Condition Immunities", StringComparison.OrdinalIgnoreCase))
                            {
                                importNPCState = ImportNPCState.NO_STATE;
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line);
                            }
                            else if (line.StartsWith("Senses", StringComparison.OrdinalIgnoreCase))
                            {
                                importNPCState = ImportNPCState.NO_STATE;
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line).Append(Environment.NewLine);
                            }
                            else if (line.StartsWith("Languages", StringComparison.OrdinalIgnoreCase))
                            {
                                importNPCState = ImportNPCState.NO_STATE;
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line);
                            }
                            else
                            {
                                _ = formattedTextContent.Append(line).Append(spaceChar);
                            } 
                        }
                        break;
                    case ImportNPCState.CHALLENGE:
                        {
                            _ = formattedTextContent.Append(line);
                            if (line.EndsWith("one target.", StringComparison.OrdinalIgnoreCase) || line.EndsWith("one creature.", StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }
                            if (line.EndsWith(periodStr))
                            {
                                _ = formattedTextContent.Append(spaceChar + Environment.NewLine);
                            }
                            else if (line.EndsWith("prepared:", StringComparison.OrdinalIgnoreCase))
                            {
                                _ = formattedTextContent.Append(slashSlashRStr);
								if (line.StartsWith("Spellcasting. ", StringComparison.OrdinalIgnoreCase))
								{
									importNPCState = ImportNPCState.SPELLCASTING;
								}
							}
							else if (line.StartsWith("Spellcasting. ", StringComparison.OrdinalIgnoreCase))
                            {
                                importNPCState = ImportNPCState.SPELLCASTING;
                            }
                            else if (line.Equals("actions", StringComparison.OrdinalIgnoreCase))
                            {
                                _ = formattedTextContent.Append(Environment.NewLine);
                                importNPCState = ImportNPCState.ACTIONS;
                            }
                            else if (line.StartsWith("Innate Spellcasting", StringComparison.OrdinalIgnoreCase))
                            {
                                importNPCState = ImportNPCState.INNATE_SPELLCASTING;
                            }
                            else if(line.StartsWith("Proficiency", StringComparison.OrdinalIgnoreCase))
							{
                                _ = formattedTextContent.Append(Environment.NewLine);
                            }
                            else
                            {
                                _ = formattedTextContent.Append(spaceChar);
                            } 
                        }
                        break;
                    case ImportNPCState.INNATE_SPELLCASTING:
                        { 
                            if (line.StartsWith("Spellcasting. ", StringComparison.OrdinalIgnoreCase))
                            {
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line);
                                importNPCState = ImportNPCState.SPELLCASTING;
                            }
                            if (line.Equals("actions", StringComparison.OrdinalIgnoreCase))
                            {
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line).Append(Environment.NewLine);
                                importNPCState = ImportNPCState.ACTIONS;
                            }
                            else if (line.EndsWith(colonStr))
                            {
                                _ = formattedTextContent.Append(line);
                            }
                            else if (innateList.Any(s => line.StartsWith(s, StringComparison.OrdinalIgnoreCase)))
                             //if (
								//line.StartsWith("At will", StringComparison.OrdinalIgnoreCase) || line.StartsWith("5/day each:", StringComparison.OrdinalIgnoreCase) ||
								//line.StartsWith("4/day each:", StringComparison.OrdinalIgnoreCase) || line.StartsWith("3/day each:", StringComparison.OrdinalIgnoreCase) ||
								//line.StartsWith("2/day each:", StringComparison.OrdinalIgnoreCase) || line.StartsWith("1/day each:", StringComparison.OrdinalIgnoreCase))
							{
                                _ = formattedTextContent.Append(slashSlashRStr).Append(line);
                                importNPCState = ImportNPCState.INNATE_SPELLCASTING_SELECTION;
                            }
                            else
                            {
                                _ = formattedTextContent.Append(spaceChar).Append(line).Append(spaceChar);
                            }
                        }
                        break;
                    case ImportNPCState.INNATE_SPELLCASTING_SELECTION:
                        {
                            if (line.StartsWith("Spellcasting. ", StringComparison.OrdinalIgnoreCase))
                            {
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line);
                                importNPCState = ImportNPCState.SPELLCASTING;
                            }
                            else if (checkIfLineMaybeTrait(line))
                            {
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line);
                                importNPCState = ImportNPCState.TRAITS;
                            }
                            else if (innateSelectionList.Any(s => line.StartsWith(s, StringComparison.OrdinalIgnoreCase)))
							//if (
							//	line.StartsWith("At will", StringComparison.OrdinalIgnoreCase) || line.StartsWith("5/day", StringComparison.OrdinalIgnoreCase) ||
							//	line.StartsWith("4/day", StringComparison.OrdinalIgnoreCase) || line.StartsWith("3/day", StringComparison.OrdinalIgnoreCase) ||
							//	line.StartsWith("2/day", StringComparison.OrdinalIgnoreCase) || line.StartsWith("1/day", StringComparison.OrdinalIgnoreCase))
							{
                                _ = formattedTextContent.Append(slashSlashRStr).Append(line);
                            }
                            else if (line.Equals("actions", StringComparison.OrdinalIgnoreCase))
                            {
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line).Append(Environment.NewLine);
                                importNPCState = ImportNPCState.ACTIONS;
                            }
                            else
                            {
                                _ = formattedTextContent.Append(spaceChar).Append(line);
                            }
                        }
                        break;
                    case ImportNPCState.SPELLCASTING:
                        {
                            if (line.Equals("actions", StringComparison.OrdinalIgnoreCase))
                            {
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line).Append(Environment.NewLine);
                                importNPCState = ImportNPCState.ACTIONS;
                            }
							else if (spellList.Any(s => line.StartsWith(s, StringComparison.OrdinalIgnoreCase)))
							//if (
							//line.StartsWith("Cantrips", StringComparison.OrdinalIgnoreCase) || line.StartsWith("1st level", StringComparison.OrdinalIgnoreCase) ||
							//line.StartsWith("2nd level", StringComparison.OrdinalIgnoreCase) || line.StartsWith("3rd level", StringComparison.OrdinalIgnoreCase) ||
							//line.StartsWith("4th level", StringComparison.OrdinalIgnoreCase) || line.StartsWith("5th level", StringComparison.OrdinalIgnoreCase) ||
							//line.StartsWith("6th level", StringComparison.OrdinalIgnoreCase) || line.StartsWith("7th level", StringComparison.OrdinalIgnoreCase) ||
							//line.StartsWith("8th level", StringComparison.OrdinalIgnoreCase) || line.StartsWith("9th level", StringComparison.OrdinalIgnoreCase))
							{
								_ = formattedTextContent.Append(slashSlashRStr).Append(line);
							}
							else if (checkIfLineMaybeTrait(line))
                            {
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line);
                                importNPCState = ImportNPCState.TRAITS;
                            }
                            else
                            {
                                _ = formattedTextContent.Append(spaceChar).Append(line);
                            }
                        }
                        break;
                    case ImportNPCState.TRAITS:
                        {
                            if (line.Equals("actions", StringComparison.OrdinalIgnoreCase))
                            {
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line).Append(Environment.NewLine);
                                importNPCState = ImportNPCState.ACTIONS;
                            }
                            else if (checkIfLineMaybeTrait(line))
                            {
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line);
                            }
                            else
                            {
                                _ = formattedTextContent.Append(spaceChar).Append(line);
                            }
                        }
                        break;
                    case ImportNPCState.ACTIONS:
                        {
                            _ = formattedTextContent.Append(line);
                            if (line.EndsWith("one target.", StringComparison.OrdinalIgnoreCase) || line.EndsWith("one creature.", StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }
                            // The ':' character is a result from the ultimate_tyrant parsing
                            if (line.EndsWith(".") || (line.EndsWith(":") && !line.EndsWith("Hit:", StringComparison.OrdinalIgnoreCase)))
                            {
                                _ = formattedTextContent.Append(spaceChar + Environment.NewLine);
                            }
                            else if (line.EndsWith("prepared:", StringComparison.OrdinalIgnoreCase))
                            {
                                _ = formattedTextContent.Append(slashSlashRStr);
                            }
                            else if (line.Equals("legendary actions", StringComparison.OrdinalIgnoreCase))
                            {
                                _ = formattedTextContent.Append(Environment.NewLine);

                                // In order to use the Engineer Suite Parser, the first Legendary Actions line is expected to begin with Options: 
                                _ = formattedTextContent.Append("Options. ");
                            }
                            else if (line.Equals("reactions", StringComparison.OrdinalIgnoreCase))
                            {
                                _ = formattedTextContent.Append(Environment.NewLine);
                                importNPCState = ImportNPCState.REACTIONS;
                            }
                            else
                            {
                                _ = formattedTextContent.Append(spaceChar);
                            }
                        }
                        break;
                    case ImportNPCState.REACTIONS:
                        {
                            if (checkIfLineMaybeTrait(line))
                            {
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line).Append(spaceChar);
                            }
                            else if (line.Equals("legendary actions", StringComparison.OrdinalIgnoreCase))
                            {
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line).Append(Environment.NewLine);

                                // In order to use the Engineer Suite Parser, the first Legendary Actions line is expected to begin with Options: 
                                _ = formattedTextContent.Append("Options. ");
                            }
                            else
                            {
                                _ = formattedTextContent.Append(line).Append(spaceChar);
                            }
                        }
                        break;
                    case ImportNPCState.NO_STATE:
                        { 
                            if (line.StartsWith("Challenge ", StringComparison.OrdinalIgnoreCase))
                            {
                                importNPCState = ImportNPCState.CHALLENGE;
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line).Append(Environment.NewLine);
                                continue;
                            }

                            if (line.StartsWith("Senses", StringComparison.OrdinalIgnoreCase))
                            {
                                importNPCState = ImportNPCState.SENSES;
                                _ = formattedTextContent.Append(Environment.NewLine).Append(line).Append(spaceChar);
                                continue;
                            }

                            if (line.StartsWith("Damage Immunities", StringComparison.OrdinalIgnoreCase))
                            {
                                importNPCState = ImportNPCState.DAMAGE_IMMUNITY;
                                _ = formattedTextContent.Append(line).Append(spaceChar);
                                continue;
                            }

                            if (line.StartsWith("Damage Resistance", StringComparison.OrdinalIgnoreCase))
                            {
                                importNPCState = ImportNPCState.DAMAGE_RESIST;
                                _ = formattedTextContent.Append(line).Append(spaceChar);
                                continue;
                            }

                            _ = formattedTextContent.Append(line).Append(Environment.NewLine);
                        }
                        break;
                    default:
                        break;
                }
            }
            return formattedTextContent.ToString();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool checkIfLineMaybeTrait(string line)
        {
            string[] strArray = line.Split(' ');
            for (int idx = 0; idx < 5; idx++)
            {
				if (idx >= strArray.Length) { return false; }

				// The logic behind this is Trait Names are typically capitialized (e.g. Flyby., Shadow Step.)
				// If the string found contains a period AND is lowercase, then it's very likely it's not a new trait (e.g. lowered., location.)
				if (strArray[idx].EndsWith(periodStr)) { return !strArray[idx].ToLower().Equals(strArray[idx]); }
			}
			return false;
        }
    }
}
