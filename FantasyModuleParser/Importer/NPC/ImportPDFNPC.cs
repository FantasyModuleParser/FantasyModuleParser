using FantasyModuleParser.Importer.Utils;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using System;
using System.Globalization;
using System.IO;

namespace FantasyModuleParser.Importer.NPC
{
    public class ImportPDFNPC : ImportESNPCBase
    {

        IFormatContentService formatContentService = new FormatPDFService();
        public ImportPDFNPC()
        {
            importCommonUtils = new ImportCommonUtils();
        }

        public override NPCModel ImportTextToNPCModel(string importTextContent)
        {
            NPCModel parsedNPCModel = new NPCController().InitializeNPCModel();
            string formattedNPCTextData = formatContentService.FormatImportContent(importTextContent);
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
                    if (line.ToLower().Equals("actions"))
                    {
                        resetContinueFlags();
                        continueActionsFlag = true;
                        continue;
                    }
                    if (line.StartsWith("Innate Spellcasting"))
                    {
                        //resetContinueFlags();
                        //continueInnateSpellcastingFlag = true;
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
                    if (line.ToLower().Equals("actions"))
                    {
                        resetContinueFlags();
                        continueActionsFlag = true;
                        continue;
                    }
                    ParseSpellCastingAttributes(parsedNPCModel, line);
                }

                if (continueActionsFlag)
                {
                    resetContinueFlags();
                    if (line.ToLower().Equals("reactions"))
                    {
                        continueReactionsFlag = true;
                        continue;
                    }
                    if (line.ToLower().Equals("legendary actions"))
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
                    if (line.ToLower().Equals("legendary actions"))
                    {
                        continueLegendaryActionsFlag = true;
                        continue;
                    }
                    continueReactionsFlag = true;
                    ParseReaction(parsedNPCModel, line);
                }

                if (continueLegendaryActionsFlag)
                {
                    if (line.ToLower().Equals("reactions"))
                    {
                        resetContinueFlags();
                        continueReactionsFlag = true;
                        continue;
                    }
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
    }
}
