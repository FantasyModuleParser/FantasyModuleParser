using System.IO;
using System.Text;

namespace FantasyModuleParser.Importer.NPC
{
    public class FormatPDFService : IFormatContentService
    {
        private bool afterChallengeLine = false;
        private bool afterInnateSpellcastingLine = false;
        private bool afterSpellcastingLine = false;
        private bool afterActionsLine = false;
        private bool afterSensesLine = false;
        private bool afterDamageImmunityLine = false;
        private bool afterTraitLine = false;
        private bool afterReactionsLine = false;

        public string FormatImportContent(string importTextContent)
        {
            StringBuilder formattedTextContent = new StringBuilder();
            StringReader stringReader = new StringReader(importTextContent);
            string line = "";
            ResetFormatNPCTextDataFlags();
            while ((line = stringReader.ReadLine()) != null)
            {
                if (afterSensesLine)
                {
                    formattedTextContent.Append(line);
                    if (line.StartsWith("Languages"))
                    {
                        ResetFormatNPCTextDataFlags();
                        formattedTextContent.Append("\n");
                    }
                    else
                        formattedTextContent.Append(" ");
                }
                if (afterDamageImmunityLine)
                {
                    formattedTextContent.Append(line);
                    if (line.StartsWith("Condition Immunities"))
                    {
                        ResetFormatNPCTextDataFlags();
                        formattedTextContent.Append("\n");
                    }
                    else if (line.StartsWith("Senses"))
                    {
                        ResetFormatNPCTextDataFlags();
                        formattedTextContent.Append("\n");
                    }
                    else if (line.StartsWith("Languages"))
                    {
                        ResetFormatNPCTextDataFlags();
                        formattedTextContent.Append("\n");
                    }
                    else
                        formattedTextContent.Append(" ");

                }
                if (afterChallengeLine)
                {
                    formattedTextContent.Append(line);
                    if (line.EndsWith("one target.") || line.EndsWith("one creature."))
                        continue;
                    if (line.EndsWith("."))
                        formattedTextContent.Append(" \n");
                    else if (line.EndsWith("prepared:"))
                        formattedTextContent.Append("\\r");
                    else if (line.Equals("Actions"))
                    {
                        formattedTextContent.Append("\n");
                        ResetFormatNPCTextDataFlags();
                        afterActionsLine = true;
                    }
                    else if (line.StartsWith("Innate Spellcasting"))
                    {
                        //formattedTextContent.Append(" ");
                        ResetFormatNPCTextDataFlags();
                        afterInnateSpellcastingLine = true;
                    }
                    else if (line.StartsWith("Spellcasting. "))
                    {
                        formattedTextContent.Append(" ");
                        ResetFormatNPCTextDataFlags();
                        afterSpellcastingLine = true;
                    }
                    else
                        formattedTextContent.Append(" ");
                }
                else if (afterInnateSpellcastingLine)
                {
                    if (line.StartsWith("Spellcasting. "))
                    {
                        formattedTextContent.Append("\n").Append(line);
                        ResetFormatNPCTextDataFlags();
                        afterSpellcastingLine = true;
                    }
                    if (line.Equals("Actions"))
                    {
                        formattedTextContent.Append("\n").Append(line).Append("\n");
                        ResetFormatNPCTextDataFlags();
                        afterActionsLine = true;
                    }
                    else if (line.EndsWith(":"))
                        formattedTextContent.Append(line);
                    else if (line.StartsWith("At will") || line.StartsWith("5/day each:")
                        || line.StartsWith("4/day each:") || line.StartsWith("3/day each:")
                        || line.StartsWith("2/day each:") || line.StartsWith("1/day each:"))
                    {
                        formattedTextContent.Append("\\r").Append(line);
                    }
                    else if (checkIfLineMaybeTrait(line))
                    {

                    }
                    else
                        formattedTextContent.Append(" ").Append(line).Append(" ");
                }
                else if (afterSpellcastingLine)
                {
                    if (line.Equals("Actions"))
                    {
                        formattedTextContent.Append("\n").Append(line).Append("\n");
                        ResetFormatNPCTextDataFlags();
                        afterActionsLine = true;
                    }
                    else if (line.EndsWith("prepared:"))
                        formattedTextContent.Append(line);
                    else if (line.StartsWith("Cantrips") || line.StartsWith("1st level")
                        || line.StartsWith("2nd level") || line.StartsWith("3rd level")
                        || line.StartsWith("4th level") || line.StartsWith("5th level")
                        || line.StartsWith("6th level") || line.StartsWith("7th level")
                        || line.StartsWith("8th level") || line.StartsWith("9th level"))
                    {
                        formattedTextContent.Append("\\r").Append(line);
                    }
                    else
                        formattedTextContent.Append(" ").Append(line).Append(" ");
                }
                else if (afterTraitLine)
                {

                }
                else if (afterActionsLine)
                {
                    formattedTextContent.Append(line);
                    if (line.EndsWith("one target.") || line.EndsWith("one creature."))
                        continue;
                    if (line.EndsWith("."))
                        formattedTextContent.Append(" \n");
                    else if (line.EndsWith("prepared:"))
                        formattedTextContent.Append("\\r");
                    else if (line.Equals("Legendary Actions"))
                    {
                        formattedTextContent.Append("\n");

                        // In order to use the Engineer Suite Parser, the first Legendary Actions line is expected to begin with Options: 
                        formattedTextContent.Append("Options. ");
                    }
                    else if (line.Equals("Reactions"))
                    {
                        ResetFormatNPCTextDataFlags();
                        afterReactionsLine = true;
                        continue;
                    }
                    else
                        formattedTextContent.Append(" ");
                }
                else if (afterReactionsLine)
                {
                    if (checkIfLineMaybeTrait(line))
                        formattedTextContent.Append("\n").Append(line).Append(" ");
                    else
                        formattedTextContent.Append(line).Append(" ");
                }
                else
                {
                    if (line.StartsWith("Challenge "))
                        afterChallengeLine = true;
                    if (line.StartsWith("Senses"))
                    {
                        afterSensesLine = true;
                        formattedTextContent.Append(line).Append(" ");
                        continue;
                    }
                    if (line.StartsWith("Damage Immunities"))
                    {
                        afterDamageImmunityLine = true;
                        ResetFormatNPCTextDataFlags();
                        formattedTextContent.Append(line).Append(" ");
                        continue;
                    }


                    formattedTextContent.Append(line).Append("\n");
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
                if (strArray[idx].EndsWith("."))
                    return true;

            }
            return false;
        }

        private void ResetFormatNPCTextDataFlags()
        {
            afterSensesLine = false;
            afterDamageImmunityLine = false;
            afterChallengeLine = false;
            afterInnateSpellcastingLine = false;
            afterSpellcastingLine = false;
            afterActionsLine = false;
            afterTraitLine = false;
            afterReactionsLine = false;
        }
    }
}
