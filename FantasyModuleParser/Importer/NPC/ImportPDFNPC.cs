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
            resetContinueFlags();

            // The first line indicates the NPC name
            string line = stringReader.ReadLine();
            if (line != null)
			{
                parsedNPCModel.NPCName = line;
            }

            // TODO: When a line.StartsWith() if statement is parsed, it should call continue
            // so as not to check other if statements that are known to not be valid at the moment
            while ((line = stringReader.ReadLine()) != null)
            {
				if (line.StartsWith("Tiny") || line.StartsWith("Small") || line.StartsWith("Medium") || line.StartsWith("Large") || line.StartsWith("Huge") || line.StartsWith("Gargantuan"))
				{
					// Line 2 indicates Size, Type, (tag), Alignment
					ParseSizeAndAlignment(parsedNPCModel, line);
					continue;
				}

				if (line.StartsWith("Armor Class", StringComparison.OrdinalIgnoreCase) || line.StartsWith("Armour Class", StringComparison.OrdinalIgnoreCase))
				{
					ParseArmorClass(parsedNPCModel, line);
					continue;
				}

				if (line.StartsWith("Hit Points", StringComparison.OrdinalIgnoreCase))
				{
					ParseHitPoints(parsedNPCModel, line);
					continue;
				}

				if (line.StartsWith("Speed", StringComparison.OrdinalIgnoreCase))
				{
					ParseSpeedAttributes(parsedNPCModel, line);
					continue;
				}

				if (line.Equals("STR DEX CON INT WIS CHA", StringComparison.OrdinalIgnoreCase))
				{
					continueBaseStatsFlag = true;
					continue;
				}

				if (continueBaseStatsFlag)
				{
					ParseStatAttributes(parsedNPCModel, line);
					// Why not simply set continueBaseStatsFlag to false here?
					resetContinueFlags();
					continue;
				}

				if (line.StartsWith("Saving Throws", StringComparison.OrdinalIgnoreCase))
				{
					ParseSavingThrows(parsedNPCModel, line);
					continue;
				}

				if (line.StartsWith("Skills", StringComparison.OrdinalIgnoreCase))
				{
					ParseSkillAttributes(parsedNPCModel, line);
					continue;
				}

				if (line.StartsWith("Damage Resistances", StringComparison.OrdinalIgnoreCase))
				{
					ParseDamageResistances(parsedNPCModel, line);
					continue;
				}

				if (line.StartsWith("Damage Vulnerabilities", StringComparison.OrdinalIgnoreCase))
				{
					ParseDamageVulnerabilities(parsedNPCModel, line);
					continue;
				}

				if (line.StartsWith("Damage Immunities", StringComparison.OrdinalIgnoreCase))
				{
					ParseDamageImmunities(parsedNPCModel, line);
					continue;
				}

				if (line.StartsWith("Condition Immunities", StringComparison.OrdinalIgnoreCase))
				{
					ParseConditionImmunities(parsedNPCModel, line);
					continue;
				}

				if (line.StartsWith("Senses", StringComparison.OrdinalIgnoreCase))
				{
					ParseVisionAttributes(parsedNPCModel, line);
					continue;
				}

				if (line.StartsWith("Languages", StringComparison.OrdinalIgnoreCase))
				{
					ParseLanguages(parsedNPCModel, line);
					continue;
				}

				if (line.StartsWith("Challenge", StringComparison.OrdinalIgnoreCase))
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
            }
            return parsedNPCModel;
        }

        /// <summary>
        /// 'STR DEX CON INT WIS CHA 
        /// 10 (+0) 11 (+0) 12 (+1) 13 (+1) 14 (+2) 15 (+2)'
        /// </summary>
        private new static void ParseStatAttributes(NPCModel npcModel, string statAttributes)
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
