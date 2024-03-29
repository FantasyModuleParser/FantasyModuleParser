﻿using FantasyModuleParser.Importer.Utils;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FantasyModuleParser.Importer.NPC
{
    public class ImportDnDBeyondNPC : ImportNPCBase
    {
        public ImportDnDBeyondNPC()
        {
            importCommonUtils = new ImportCommonUtils();
        }

        /// <summary>
        /// Parses and Imports data from DnD Beyond website
        /// </summary>
        /// <param name="importTextContent">The file content of an *.npc file created by the NPC Engineer module in Engineer Suite</param>
        /// <returns></returns>
        public override NPCModel ImportTextToNPCModel(string importTextContent)
        {
            NPCModel parsedNPCModel = new NPCController().InitializeNPCModel();
            StringReader stringReader = new StringReader(importTextContent);
            resetContinueFlags();

            // The first line indicates the NPC name
            string line = stringReader.ReadLine();
			if (!string.IsNullOrWhiteSpace(line))
			{
				// Line number one indicates the NPC name
				parsedNPCModel.NPCName = line;
            }

            while ((line = stringReader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (sizeList.Any(s => line.StartsWith(s, StringComparison.OrdinalIgnoreCase)))
                {
                    // Line 2 indicates Size, Type, (tag), Alignment
                    ParseSizeAndAlignment(parsedNPCModel, line);
                }

                if (line.StartsWith("Armor Class", StringComparison.OrdinalIgnoreCase) || line.StartsWith("Armour Class", StringComparison.OrdinalIgnoreCase))
                {
                    ParseArmorClass(parsedNPCModel, line);
                }

                if (line.StartsWith("Hit Points", StringComparison.Ordinal))
                {
                    ParseHitPoints(parsedNPCModel, line);
                }

                if (line.StartsWith("Speed", StringComparison.Ordinal))
                {
                    ParseSpeedAttributes(parsedNPCModel, line);
                }

                switch (line)
                {
                    case "STR":
                        continueStrengthFlag = true;
                        break;
                    case "DEX":
                        continueDexterityFlag = true;
                        break;
                    case "CON":
                        continueConstitutionFlag = true;
                        break;
                    case "INT":
                        continueIntelligenceFlag = true;
                        break;
                    case "WIS":
                        continueWisdomFlag = true;
                        break;
                    case "CHA":
                        continueCharismaFlag = true;
                        break;
                }

                while (continueStrengthFlag == true && !line.Equals("STR", System.StringComparison.OrdinalIgnoreCase))
                {
                    ParseStatAttributeStrength(parsedNPCModel, line);
                    resetContinueFlags();
                }

                while (continueDexterityFlag == true && !line.Equals("DEX", System.StringComparison.OrdinalIgnoreCase))
                {
                    ParseStatAttributeDexterity(parsedNPCModel, line);
                    resetContinueFlags();
                }

                while (continueConstitutionFlag == true && !line.Equals("CON", System.StringComparison.OrdinalIgnoreCase))
                {
                    ParseStatAttributeConstitution(parsedNPCModel, line);
                    resetContinueFlags();
                }

                while (continueIntelligenceFlag == true && !line.Equals("INT", System.StringComparison.OrdinalIgnoreCase))
                {
                    ParseStatAttributeIntelligence(parsedNPCModel, line);
                    resetContinueFlags();
                }

                while (continueWisdomFlag == true && !line.Equals("WIS", System.StringComparison.OrdinalIgnoreCase))
                {
                    ParseStatAttributeWisdom(parsedNPCModel, line);
                    resetContinueFlags();
                }

                while (continueCharismaFlag == true && !line.Equals("CHA", System.StringComparison.OrdinalIgnoreCase))
                {
                    ParseStatAttributeCharisma(parsedNPCModel, line);
                    resetContinueFlags();
                }

                if (line.StartsWith("Saving Throws", StringComparison.Ordinal))
                {
                    ParseSavingThrows(parsedNPCModel, line);
                }

                if (line.StartsWith("Skills", StringComparison.Ordinal))
                {
                    ParseSkillAttributes(parsedNPCModel, line);
                }

                if (line.StartsWith("Damage Resistances", StringComparison.Ordinal))
                {
                    ParseDamageResistances(parsedNPCModel, line);
                }

                if (line.StartsWith("Damage Vulnerabilities", StringComparison.Ordinal))
                {
                    ParseDamageVulnerabilities(parsedNPCModel, line);
                }

                if (line.StartsWith("Damage Immunities", StringComparison.Ordinal))
                {
                    ParseDamageImmunities(parsedNPCModel, line);
                }

                if (line.StartsWith("Condition Immunities", StringComparison.Ordinal))
                {
                    ParseConditionImmunities(parsedNPCModel, line);
                }
                
                if (line.StartsWith("Senses", StringComparison.Ordinal))
                {
                    ParseVisionAttributes(parsedNPCModel, line);
                }

                if (line.StartsWith("Languages", StringComparison.Ordinal))
                {
                    ParseLanguages(parsedNPCModel, line);
                }

                if (line.StartsWith("Challenge", StringComparison.Ordinal))
                {
                    ParseChallengeRatingAndXP(parsedNPCModel, line);
                    continueTraitsFlag = true;
                    continue;
                }

                if (line.StartsWith("Proficiency Bonus", StringComparison.Ordinal))
                {
                    continue;
                }

                if (continueTraitsFlag)
                {
                    if (line.Equals("Actions", System.StringComparison.OrdinalIgnoreCase))
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
                    if (line.Equals("Actions", System.StringComparison.OrdinalIgnoreCase))
                    {
                        resetContinueFlags();
                        continueActionsFlag = true;
                        continue;
                    }
                    ParseSpellCastingAttributes(parsedNPCModel, line);
                }

                if (continueInnateSpellcastingFlag)
                {
                    if (line.Equals("Actions", System.StringComparison.OrdinalIgnoreCase))
                    {
                        resetContinueFlags();
                        continueActionsFlag = true;
                        continue;
                    }
                    // If the line contains a period, then it means there are additional traits to be included
                    if (line.Contains("."))
                    {
                        resetContinueFlags();
                        continueTraitsFlag = true;
                        ParseTraits(parsedNPCModel, line);
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
                    if (line.Equals("Reactions", System.StringComparison.OrdinalIgnoreCase))
                    {
                        continueReactionsFlag = true;
                        continue;
                    }
                    if (line.Equals("Legendary Actions", System.StringComparison.OrdinalIgnoreCase))
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
                    if (line.Equals("Legendary Actions", System.StringComparison.OrdinalIgnoreCase))
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
            }

            return parsedNPCModel;
        }
    }
}