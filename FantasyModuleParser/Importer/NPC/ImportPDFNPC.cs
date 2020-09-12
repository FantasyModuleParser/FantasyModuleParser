using FantasyModuleParser.Importer.Utils;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FantasyModuleParser.Importer.NPC
{
    public class ImportPDFNPC : ImportESNPCBase
    {
        #region Format NPC Text Data Flags
        private bool afterChallengeLine = false;
        private bool afterInnateSpellcastingLine = false;
        private bool afterSpellcastingLine = false;
        private bool afterActionsLine = false;
        #endregion
        public ImportPDFNPC()
        {
            importCommonUtils = new ImportCommonUtils();
        }

        // This method should be run first to format the incoming NPC data
        // to be usable by the importer
        private string FormatNPCTextData(string importTextContent)
        {
            StringBuilder formattedTextContent = new StringBuilder();
            StringReader stringReader = new StringReader(importTextContent);
            string line = "";
            ResetFormatNPCTextDataFlags();
            while ((line = stringReader.ReadLine()) != null)
            {
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
                        formattedTextContent.Append(" ");
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
                else if (afterActionsLine)
                {
                    formattedTextContent.Append(line);
                    if (line.EndsWith("one target.") || line.EndsWith("one creature."))
                        continue;
                    if (line.EndsWith("."))
                        formattedTextContent.Append(" \n");
                    else if (line.EndsWith("prepared:"))
                        formattedTextContent.Append("\\r");
                    //else if (line.Equals("Actions"))
                    //    formattedTextContent.Append("\n");
                    else
                        formattedTextContent.Append(" ");
                }
                else
                {
                    if (line.StartsWith("Challenge "))
                        afterChallengeLine = true;

                    formattedTextContent.Append(line).Append("\n");
                }

            }
            return formattedTextContent.ToString();
        }

        private void ResetFormatNPCTextDataFlags()
        {
            afterChallengeLine = false;
            afterInnateSpellcastingLine = false;
            afterSpellcastingLine = false;
            afterActionsLine = false;
        }

        public override NPCModel ImportTextToNPCModel(string importTextContent)
        {
            NPCModel parsedNPCModel = new NPCController().InitializeNPCModel();
            string formattedNPCTextData = FormatNPCTextData(importTextContent);
            StringReader stringReader = new StringReader(formattedNPCTextData);
            string line = "";
            int lineNumber = 1;
            resetContinueFlags();
            while ((line = stringReader.ReadLine()) != null)
            {
                if (lineNumber == 1)
                    // Line number one indicates the NPC name
                    parsedNPCModel.NPCName = line;
                if (line.StartsWith("Tiny") || line.StartsWith("Small") || line.StartsWith("Medium") || line.StartsWith("Large") || line.StartsWith("Huge") || line.StartsWith("Gargantuan"))
                    // Line 2 indicates Size, Type, (tag), Alignment
                    ParseSizeAndAlignment(parsedNPCModel, line);
                if (line.StartsWith("Armor Class", StringComparison.Ordinal))
                    ParseArmorClass(parsedNPCModel, line);
                if (line.StartsWith("Hit Points", StringComparison.Ordinal))
                    ParseHitPoints(parsedNPCModel, line);
                if (line.StartsWith("Speed", StringComparison.Ordinal))
                    ParseSpeedAttributes(parsedNPCModel, line);
                if (line.Equals("STR DEX CON INT WIS CHA", StringComparison.Ordinal))
                {
                    continueBaseStatsFlag = true;
                    continue;
                }
                if (continueBaseStatsFlag)
                {
                    ParseStatAttributes(parsedNPCModel, line);
                    resetContinueFlags();
                }
                if (line.StartsWith("Saving Throws", StringComparison.Ordinal))
                    ParseSavingThrows(parsedNPCModel, line);
                if (line.StartsWith("Skills", StringComparison.Ordinal))
                    ParseSkillAttributes(parsedNPCModel, line);
                if (line.StartsWith("Damage Resistances", StringComparison.Ordinal))
                    ParseDamageResistances(parsedNPCModel, line);
                if (line.StartsWith("Damage Vulnerabilities", StringComparison.Ordinal))
                    ParseDamageVulnerabilities(parsedNPCModel, line);
                if (line.StartsWith("Damage Immunities", StringComparison.Ordinal))
                    ParseDamageImmunities(parsedNPCModel, line);
                if (line.StartsWith("Condition Immunities", StringComparison.Ordinal))
                    ParseConditionImmunities(parsedNPCModel, line);
                if (line.StartsWith("Senses", StringComparison.Ordinal))
                    ParseVisionAttributes(parsedNPCModel, line);
                if (line.StartsWith("Languages", StringComparison.Ordinal))
                    ParseLanguages(parsedNPCModel, line);
                if (line.StartsWith("Challenge", StringComparison.Ordinal))
                {
                    ParseChallengeRatingAndXP(parsedNPCModel, line);
                    continueTraitsFlag = true;
                    continue;
                }
                if (continueTraitsFlag)
                {
                    if (line.Equals("Actions"))
                    {
                        resetContinueFlags();
                        continueActionsFlag = true;
                        continue;
                    }
                    if (line.StartsWith("Innate Spellcasting"))
                    {
                        resetContinueFlags();
                        continueInnateSpellcastingFlag = true;
                        ParseInnateSpellCastingAttributes(parsedNPCModel, line);
                        continue;
                    }
                    if (line.StartsWith("Spellcasting"))
                    {
                        resetContinueFlags();
                        continueSpellcastingFlag = true;
                        ParseSpellCastingAttributes(parsedNPCModel, line);
                        continue;
                    }
                    ParseTraits(parsedNPCModel, line);
                    continue;
                }

                if (continueSpellcastingFlag)
                {
                    if (line.Equals("Actions"))
                    {
                        resetContinueFlags();
                        continueActionsFlag = true;
                        continue;
                    }
                    ParseSpellCastingAttributes(parsedNPCModel, line);
                }

                if (continueInnateSpellcastingFlag)
                {
                    if (line.Equals("Actions"))
                    {
                        resetContinueFlags();
                        continueActionsFlag = true;
                        continue;
                    }
                    ParseInnateSpellCastingAttributes(parsedNPCModel, line);
                }

                if (line.StartsWith("Innate Spellcasting"))
                {
                    resetContinueFlags();
                    continueInnateSpellcastingFlag = true;
                    continue;
                }
                if (continueActionsFlag)
                {
                    resetContinueFlags();
                    if (line.Equals("Reactions"))
                    {
                        continueReactionsFlag = true;
                        continue;
                    }
                    if (line.Equals("Legendary Actions"))
                    {
                        continueLegendaryActionsFlag = true;
                        continue;
                    }
                    continueActionsFlag = true;
                    ParseStandardAction(parsedNPCModel, line);
                }
                if (continueReactionsFlag)
                {
                    resetContinueFlags();
                    if (line.Equals("Legendary Actions"))
                    {
                        continueLegendaryActionsFlag = true;
                        continue;
                    }
                    continueReactionsFlag = true;
                    ParseReaction(parsedNPCModel, line);
                }
                if (continueLegendaryActionsFlag)
                {
                    ParseLegendaryAction(parsedNPCModel, line);
                    continue;
                }
                lineNumber++;
            }
            return parsedNPCModel;
        }

        /// <summary>
        /// 'STR DEX CON INT WIS CHA 
        /// 10 (+0) 11 (+0) 12 (+1) 13 (+1) 14 (+2) 15 (+2)'
        /// </summary>
        private void ParseStatAttributes(NPCModel npcModel, string statAttributes)
        {
            string[] splitAttributes = statAttributes.Split(' ');
            npcModel.AttributeStr = int.Parse(splitAttributes[0], CultureInfo.CurrentCulture);
            npcModel.AttributeDex = int.Parse(splitAttributes[2], CultureInfo.CurrentCulture);
            npcModel.AttributeCon = int.Parse(splitAttributes[4], CultureInfo.CurrentCulture);
            npcModel.AttributeInt = int.Parse(splitAttributes[6], CultureInfo.CurrentCulture);
            npcModel.AttributeWis = int.Parse(splitAttributes[8], CultureInfo.CurrentCulture);
            npcModel.AttributeCha = int.Parse(splitAttributes[10], CultureInfo.CurrentCulture);
        }

        public void ParseTraits(NPCModel npcModel, string traits)
        {
            ActionModelBase traitModel = null;
            if (npcModel.Traits == null)
                npcModel.Traits = new System.Collections.ObjectModel.ObservableCollection<ActionModelBase>();

            if (string.IsNullOrEmpty(traits))
                return;

            string[] traitArrayBySpace = traits.Split(' ');
            for (int idx = 0; idx < 5; idx++)
            {
                if (traitArrayBySpace[idx].Contains("."))
                {
                    string[] traitArray = traits.Split('.');
                    traitModel = new ActionModelBase();
                    traitModel.ActionName = traitArray[0];
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int idy = 1; idy < traitArray.Length; idy++)
                    {
                        stringBuilder.Append(traitArray[idy]).Append(".");
                    }
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                    traitModel.ActionDescription = stringBuilder.ToString().Trim();
                    npcModel.Traits.Add(traitModel);
                    return;
                }
            }
            // If no period was detected in the first 5 words,
            traitModel = npcModel.Traits.Last();
            traitModel.ActionDescription = traitModel.ActionDescription + "\n\n" + traits;
        }
    }
}
