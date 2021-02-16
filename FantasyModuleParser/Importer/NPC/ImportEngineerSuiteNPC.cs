﻿using FantasyModuleParser.Importer.Utils;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using System;
using System.Globalization;
using System.IO;

namespace FantasyModuleParser.Importer.NPC
{
    public class ImportEngineerSuiteNPC : ImportESNPCBase
    {
        public ImportEngineerSuiteNPC()
        {
            importCommonUtils = new ImportCommonUtils();
        }
        /// <summary>
        /// Parses & Imports data from .npc files generated by Engineer Suite - NPC Module by Maasq
        /// </summary>
        /// <param name="importTextContent">The file content of an *.npc file created by the NPC Engineer module in Engineer Suite</param>
        /// <returns></returns>
        public override NPCModel ImportTextToNPCModel(string importTextContent)
        {
            NPCModel parsedNPCModel = new NPCController().InitializeNPCModel();

            string line = "";
            StringReader stringReader = new StringReader(importTextContent);

            int lineNumber = 1;
            while((line = stringReader.ReadLine()) != null)
            {
                if (line.StartsWith("***Part 1***"))
                    continue;
                if (lineNumber == 1)
                {
                    // Line number one indicates the NPC name
                    parsedNPCModel.NPCName = line;
                }
                if (lineNumber == 2)
                {
                    // Line 2 indicates Size, Type, (tag), Alignment
                    ParseSizeAndAlignment(parsedNPCModel, line);
                }

                if (line.StartsWith("Armor Class", StringComparison.OrdinalIgnoreCase) || line.StartsWith("Armour Class", StringComparison.OrdinalIgnoreCase))
                    ParseArmorClass(parsedNPCModel, line);
                if (line.StartsWith("Hit Points", StringComparison.Ordinal))
                    ParseHitPoints(parsedNPCModel, line);
                if (line.StartsWith("Speed", StringComparison.Ordinal))
                    ParseSpeedAttributes(parsedNPCModel, line);
                if (line.StartsWith("STR DEX CON INT WIS CHA", StringComparison.Ordinal))
                    ParseStatAttributes(parsedNPCModel, line);
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
                if (line.StartsWith("Challenge", StringComparison.Ordinal)) { 
                    ParseChallengeRatingAndXP(parsedNPCModel, line);
                    continueTraitsFlag = true;
                    continue;
                }
                if (line.StartsWith("Innate Spellcasting"))
                {
                    resetContinueFlags();
                    ParseInnateSpellCastingAttributes(parsedNPCModel, line);
                }
                if (line.StartsWith("Spellcasting"))
                {
                    resetContinueFlags();
                    ParseSpellCastingAttributes(parsedNPCModel, line);
                }
                if (line.StartsWith("ACTIONS"))
                {
                    resetContinueFlags();
                    continueActionsFlag = true;
                }
                if (line.StartsWith("REACTIONS"))
                {
                    resetContinueFlags();
                    continueReactionsFlag = true;
                }
                if (line.StartsWith("LEGENDARY ACTIONS"))
                {
                    resetContinueFlags();
                    continueLegendaryActionsFlag = true;
                }
                if (line.StartsWith("LAIR ACTIONS"))
                {
                    resetContinueFlags();
                    continueLairActionsFlag = true;
                }
                if (line.StartsWith("***Part 2***"))
                {
                    resetContinueFlags();
                }

                // Parsing through ***Part 3***
                if (line.StartsWith("NPCgender:"))
                {
                    parsedNPCModel.NPCGender = line.Substring(11);
                }
                if (line.StartsWith("NPCunique:"))
                {
                    parsedNPCModel.Unique = line.Equals("NPCunique: 1", StringComparison.Ordinal);
                }
                if (line.StartsWith("NPCpropername:"))
                {
                    parsedNPCModel.NPCNamed = line.Equals("NPCpropername: 1", StringComparison.Ordinal);
                }
                if (line.StartsWith("NPCimagePath:"))
                {
                    parsedNPCModel.NPCImage = line.Substring(14);
                    if (parsedNPCModel.NPCImage.Equals(" "))
                        parsedNPCModel.NPCImage = "";
                }
                if (line.StartsWith("NPCTokenPath:"))
                {
                    parsedNPCModel.NPCToken = line.Substring(13);
                    if (parsedNPCModel.NPCToken.Equals(" ") || parsedNPCModel.NPCToken.Equals(null))
                        parsedNPCModel.NPCToken = "";
                }
                if (line.StartsWith("LAction"))
                {
                    // Get the lair action number
                    int lairActionIndex = int.Parse(line.Split(':')[0].Substring(7), CultureInfo.CurrentCulture);
                    
                    // Need to check to see if Lair Action is even populated (there is a chance data isn't saved from ES v1)
                    if (parsedNPCModel.LairActions.Count >= lairActionIndex)
                        parsedNPCModel.LairActions[lairActionIndex - 1].ActionName = line.Split(':')[1].Trim();
                }

                // Process any 'continue' flags accordingly here
                if (continueTraitsFlag)
                    ParseTraits(parsedNPCModel, line);
                
                if(continueActionsFlag && !line.Equals("ACTIONS", StringComparison.OrdinalIgnoreCase))
                    ParseStandardAction(parsedNPCModel, line);

                if (continueReactionsFlag && !line.Equals("REACTIONS", StringComparison.OrdinalIgnoreCase))
                    ParseReaction(parsedNPCModel, line);

                if (continueLegendaryActionsFlag && !line.Equals("LEGENDARY ACTIONS", StringComparison.OrdinalIgnoreCase))
                    ParseLegendaryAction(parsedNPCModel, line);

                if (continueLairActionsFlag && !line.Equals("LAIR ACTIONS", StringComparison.OrdinalIgnoreCase))
                    ParseLairAction(parsedNPCModel, line);

                lineNumber++;
            }
            return parsedNPCModel;
        }
    }
}