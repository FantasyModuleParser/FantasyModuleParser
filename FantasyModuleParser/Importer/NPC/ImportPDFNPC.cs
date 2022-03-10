using FantasyModuleParser.Importer.Utils;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FantasyModuleParser.Importer.NPC
{
    public class ImportPDFNPC : ImportESNPCBase
    {
        // private static readonly char[] spaceSeparator = new char[] { ' ' };

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
                line = line.Trim();
                if (string.IsNullOrEmpty(line)) continue;

                // if (line.StartsWith("Tiny") || line.StartsWith("Small") || line.StartsWith("Medium") || line.StartsWith("Large") || line.StartsWith("Huge") || line.StartsWith("Gargantuan"))
                if (sizeList.Any(s => line.StartsWith(s, StringComparison.OrdinalIgnoreCase)))
				{
					// Line 2 indicates Size, Type, (tag), Alignment
					ParseSizeAndAlignment(parsedNPCModel, line);
					//continue;
				}

				if (line.StartsWith("Armor Class", StringComparison.OrdinalIgnoreCase) || line.StartsWith("Armour Class", StringComparison.OrdinalIgnoreCase))
				{
					ParseArmorClass(parsedNPCModel, line);
					//continue;
				}

				if (line.StartsWith("Hit Points", StringComparison.OrdinalIgnoreCase))
				{
					ParseHitPoints(parsedNPCModel, line);
					//continue;
				}

				if (line.StartsWith("Speed", StringComparison.OrdinalIgnoreCase))
				{
					ParseSpeedAttributes(parsedNPCModel, line);
					//continue;
				}

                // if (line.Equals("STR DEX CON INT WIS CHA", StringComparison.OrdinalIgnoreCase)) - entire line matched
                if (rgxCharacteristics.IsMatch(line))
				{
					continueBaseStatsFlag = true;
					continue;
				}

				if (continueBaseStatsFlag)
				{
					ParseStatAttributes(parsedNPCModel, line);
                    //TODO: Why not simply set continueBaseStatsFlag to false here?
                    resetContinueFlags();
					//continue;
				}

                // because of the two step method above for reading the line with "STR DEX CON INT WIS CHA" and then reading
                // the next line to get the stats, when all the stats are on one line, it has to be done differently, in a single step
                // match if the stats are embedded in the line with the characteristics
                //     STR 15 (+2); DEX 10 (+0); CON 13 (+1); INT 2 (−4); WIS 11 (+0); CHA 4 (−3)
                if (allCharacteristics.IsMatch(line))
                {
                    ParseStatAttributes(parsedNPCModel, line);
                    continue;
                }

                if (line.StartsWith("Saving Throws", StringComparison.OrdinalIgnoreCase))
				{
					ParseSavingThrows(parsedNPCModel, line);
					//continue;
				}

				if (line.StartsWith("Skills", StringComparison.OrdinalIgnoreCase))
				{
					ParseSkillAttributes(parsedNPCModel, line);
					//continue;
				}

				if (line.StartsWith("Damage Resistances", StringComparison.OrdinalIgnoreCase))
				{
					ParseDamageResistances(parsedNPCModel, line);
					//continue;
				}

				if (line.StartsWith("Damage Vulnerabilities", StringComparison.OrdinalIgnoreCase))
				{
					ParseDamageVulnerabilities(parsedNPCModel, line);
					//continue;
				}

				if (line.StartsWith("Damage Immunities", StringComparison.OrdinalIgnoreCase))
				{
					ParseDamageImmunities(parsedNPCModel, line);
					//continue;
				}

				if (line.StartsWith("Condition Immunities", StringComparison.OrdinalIgnoreCase))
				{
					ParseConditionImmunities(parsedNPCModel, line);
					//continue;
				}

				if (line.StartsWith("Senses", StringComparison.OrdinalIgnoreCase))
				{
					ParseVisionAttributes(parsedNPCModel, line);
					//continue;
				}

				if (line.StartsWith("Languages", StringComparison.OrdinalIgnoreCase))
				{
					ParseLanguages(parsedNPCModel, line);
					//continue;
				}

				if (line.StartsWith("Challenge", StringComparison.OrdinalIgnoreCase))
                {
                    ParseChallengeRatingAndXP(parsedNPCModel, line);
                    continueTraitsFlag = true;
                    continue;
                }

                if (continueTraitsFlag)
                {
                    if (line.Equals("actions", StringComparison.OrdinalIgnoreCase))
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
    }
}
