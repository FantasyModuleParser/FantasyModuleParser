﻿using FantasyModuleParser.Importer.Utils;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using System;
using System.Globalization;
using System.IO;

namespace FantasyModuleParser.Importer.NPC
{
    public class Import5eToolsNPC : ImportNPCBase
    {
        public Import5eToolsNPC()
        {
            importCommonUtils = new ImportCommonUtils();
        }

        /// <summary>
        /// Parses NPC data from 5e.tools website
        /// </summary>

        public override NPCModel ImportTextToNPCModel(string importTextContent)
        {
            NPCModel parsedNPCModel = new NPCController().InitializeNPCModel();
            StringReader stringReader = new StringReader(importTextContent);
            string line = "";
            int lineNumber = 1;
            resetContinueFlags();
            while ((line = stringReader.ReadLine()) != null)
            {
                if (lineNumber == 1)
                {
                    // Line number one indicates the NPC name
                    parsedNPCModel.NPCName = line;
                }
                if (line.StartsWith("Tiny") || line.StartsWith("Small") || line.StartsWith("Medium") || line.StartsWith("Large") || line.StartsWith("Huge") || line.StartsWith("Gargantuan"))
                {
                    // Line 2 indicates Size, Type, (tag), Alignment
                    ParseSizeAndAlignment(parsedNPCModel, line);
                }

                if (line.StartsWith("Armor Class", StringComparison.Ordinal))
                    ParseArmorClass(parsedNPCModel, line);
                if (line.StartsWith("Hit Points", StringComparison.Ordinal))
                    ParseHitPoints(parsedNPCModel, line);
                if (line.StartsWith("Speed", StringComparison.Ordinal))
                    ParseSpeedAttributes(parsedNPCModel, line);
                if (line.StartsWith("STR DEX CON INT WIS CHA"))
                {
                    continueBaseStatsFlag = true;
                    continue;
                }
                while (continueBaseStatsFlag == true)
                {
                    string[] splitAttributes = line.Split(' ');
                    parsedNPCModel.AttributeStr = int.Parse(splitAttributes[0], CultureInfo.CurrentCulture);
                    parsedNPCModel.AttributeDex = int.Parse(splitAttributes[2], CultureInfo.CurrentCulture);
                    parsedNPCModel.AttributeCon = int.Parse(splitAttributes[4], CultureInfo.CurrentCulture);
                    parsedNPCModel.AttributeInt = int.Parse(splitAttributes[6], CultureInfo.CurrentCulture);
                    parsedNPCModel.AttributeWis = int.Parse(splitAttributes[8], CultureInfo.CurrentCulture);
                    parsedNPCModel.AttributeCha = int.Parse(splitAttributes[10], CultureInfo.CurrentCulture);
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
    }
}