using FantasyModuleParser.Importer.Utils;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Action.Enums;
using FantasyModuleParser.NPC.Models.Skills;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FantasyModuleParser.Importer.NPC
{
    public class ImportDnDBeyondNPC : ImportNPCBase
    {
        private ImportCommonUtils importCommonUtils;
        public ImportDnDBeyondNPC()
        {
            importCommonUtils = new ImportCommonUtils();
        }

        private bool continueStrengthFlag = false,
            continueDexterityFlag = false,
            continueConstitutionFlag = false,
            continueIntelligenceFlag = false,
            continueWisdomFlag = false,
            continueCharismaFlag = false,
            continueTraitsFlag = false,
            continueInnateSpellcastingFlag = false,
            continueSpellcastingFlag = false,
            continueActionsFlag = false,
            continueReactionsFlag = false,
            continueLegendaryActionsFlag = false;
        /// <summary>
        /// Parses & Imports data from DnD Beyond website
        /// </summary>
        /// <param name="importTextContent">The file content of an *.npc file created by the NPC Engineer module in Engineer Suite</param>
        /// <returns></returns>
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
                if (line.Equals("STR"))
                {
                    continueStrengthFlag = true;
                }
                if (continueStrengthFlag == true && !line.Equals("STR"))
                {
                    ParseStatAttributeStrength(parsedNPCModel, line);
                    resetContinueFlags();
                }
                if (line.Equals("DEX"))
                {
                    continueDexterityFlag = true;
                }
                if (continueDexterityFlag == true && !line.Equals("DEX"))
                {
                    ParseStatAttributeDexterity(parsedNPCModel, line);
                    resetContinueFlags();
                }
                if (line.Equals("CON"))
                {
                    continueConstitutionFlag = true;
                }
                if (continueConstitutionFlag == true && !line.Equals("CON"))
                {
                    ParseStatAttributeConstitution(parsedNPCModel, line);
                    resetContinueFlags();
                }
                if (line.Equals("INT"))
                {
                    continueIntelligenceFlag = true;
                }
                if (continueIntelligenceFlag == true && !line.Equals("INT"))
                {
                    ParseStatAttributeIntelligence(parsedNPCModel, line);
                    resetContinueFlags();
                }
                if (line.Equals("WIS"))
                {
                    continueWisdomFlag = true;
                }
                if (continueWisdomFlag == true && !line.Equals("WIS"))
                {
                    ParseStatAttributeWisdom(parsedNPCModel, line);
                    resetContinueFlags();
                }
                if (line.Equals("CHA"))
                {
                    continueCharismaFlag = true;
                }
                if (continueCharismaFlag == true && !line.Equals("CHA"))
                {
                    ParseStatAttributeCharisma(parsedNPCModel, line);
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
                if (line.StartsWith("Spellcasting"))
                {
                    resetContinueFlags();
                    ParseSpellCastingAttributes(parsedNPCModel, line);
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
        /// Resets all 'continue' flags to false;  Used in ImportTextToNPCModel for run-on lines (e.g. Traits, Actions, etc...)
        /// </summary>
        private void resetContinueFlags()
        {
            continueStrengthFlag = false;
            continueDexterityFlag = false;
            continueConstitutionFlag = false;
            continueIntelligenceFlag = false;
            continueWisdomFlag = false;
            continueCharismaFlag = false;
            continueTraitsFlag = false;
            continueInnateSpellcastingFlag = false;
            continueSpellcastingFlag = false;
            continueActionsFlag = false;
            continueReactionsFlag = false;
            continueLegendaryActionsFlag = false;
        }

        /// <summary>
        /// STR 
        /// 10 (+0)
        /// DEX
        /// 11 (+0)
        /// CON
        /// 12 (+1)
        /// INT 
        /// 13 (+1)
        /// WIS
        /// 14 (+2)
        /// CHA
        /// 15 (+2)'
        /// </summary>

        public void ParseStatAttributeStrength(NPCModel npcModel, string statAttributeStrength)
        {
            string[] splitAttributes = statAttributeStrength.Split('(');
            npcModel.AttributeStr = int.Parse(splitAttributes[0]);
        }
        public void ParseStatAttributeDexterity(NPCModel npcModel, string statAttributeDexterity)
        {
            string[] splitAttributes = statAttributeDexterity.Split('(');
            npcModel.AttributeDex = int.Parse(splitAttributes[0]);
        }
        public void ParseStatAttributeConstitution(NPCModel npcModel, string statAttributeConstitution)
        {
            string[] splitAttributes = statAttributeConstitution.Split('(');
            npcModel.AttributeCon = int.Parse(splitAttributes[0]);
        }
        public void ParseStatAttributeIntelligence(NPCModel npcModel, string statAttributeIntelligence)
        {
            string[] splitAttributes = statAttributeIntelligence.Split('(');
            npcModel.AttributeInt = int.Parse(splitAttributes[0]);
        }
        public void ParseStatAttributeWisdom(NPCModel npcModel, string statAttributeWisdom)
        {
            string[] splitAttributes = statAttributeWisdom.Split('(');
            npcModel.AttributeWis = int.Parse(splitAttributes[0]);
        }
        public void ParseStatAttributeCharisma(NPCModel npcModel, string statAttributeCharisma)
        {
            string[] splitAttributes = statAttributeCharisma.Split('(');
            npcModel.AttributeCha = int.Parse(splitAttributes[0]);
        }

        

        /// <summary>
        /// Innate Spellcasting. V1_npc_all's innate spellcasting ability is Wisdom (spell save DC 8, +30 to hit with spell attacks). He can innately cast the following spells, requiring no material components:\rAt will: Super Cantrips\r5/day each: Daylight\r4/day each: False Life\r3/day each: Hunger\r2/day each: Breakfast, Lunch, Dinner\r1/day each: Nom Noms
        /// </summary>
        /// <param name="npcModel"></param>
        /// <param name="innateSpellcastingAttributes"></param>
        public void ParseInnateSpellCastingAttributes(NPCModel npcModel, string innateSpellcastingAttributes)
        {
            if (innateSpellcastingAttributes.StartsWith("Innate Spellcasting"))
            {
                npcModel.InnateSpellcastingSection = true;
                // Innate Spellcasting Ability
                int abilityIsIndex = innateSpellcastingAttributes.IndexOf("spellcasting ability is ", StringComparison.Ordinal);
                int spellSaveDCIndex = innateSpellcastingAttributes.IndexOf("(spell save DC ", StringComparison.Ordinal);
                // 24 is the string length to "spellcasting ability is "
                npcModel.InnateSpellcastingAbility = innateSpellcastingAttributes.Substring(abilityIsIndex + 24, spellSaveDCIndex - abilityIsIndex - 25);

                // Spell Save DC & Attack Bonus
                int spellAttacksIndex = innateSpellcastingAttributes.IndexOf(" to hit with spell attacks).", StringComparison.Ordinal);

                // If no spell attack bonus is available, spellAttacksIndex equals -1
                if (spellAttacksIndex != -1)
                {
                    String spellSaveAndAttackData = innateSpellcastingAttributes.Substring(spellSaveDCIndex, spellAttacksIndex - spellSaveDCIndex);
                    foreach (String subpart in spellSaveAndAttackData.Split(' '))
                    {
                        if (subpart.Contains(","))
                        {
                            npcModel.InnateSpellSaveDC = int.Parse(subpart.Replace(',', ' '), CultureInfo.CurrentCulture);
                        }
                        if (subpart.Contains('+') || subpart.Contains('-'))
                            npcModel.InnateSpellHitBonus = parseAttributeStringToInt(subpart);
                    }
                }
                else
                {
                    // Process only the Save DC
                    string innateSpellcastingSaveDCString = innateSpellcastingAttributes.Substring(spellSaveDCIndex);
                    innateSpellcastingSaveDCString = innateSpellcastingSaveDCString.Substring(0, innateSpellcastingSaveDCString.IndexOf(").", StringComparison.Ordinal));
                    npcModel.InnateSpellSaveDC = int.Parse(innateSpellcastingSaveDCString.Substring("(spell save DC ".Length), CultureInfo.CurrentCulture);
                }

                // Component Text
                int preComponentText = innateSpellcastingAttributes.IndexOf("following spells,", StringComparison.Ordinal);
                int postComponentText = innateSpellcastingAttributes.IndexOf(":\\r", StringComparison.Ordinal);
                if(postComponentText == -1)
                    npcModel.ComponentText = innateSpellcastingAttributes.Substring(preComponentText + 18);
                else
                    npcModel.ComponentText = innateSpellcastingAttributes.Substring(preComponentText + 18, postComponentText - preComponentText - 18);

                string[] innateSpellcastingAttributesArray = innateSpellcastingAttributes.Split(new string[] { "\\r" }, StringSplitOptions.RemoveEmptyEntries);
                for (int arrayIndex = 1; arrayIndex < innateSpellcastingAttributesArray.Length; arrayIndex++)
                {
                    string innerData = innateSpellcastingAttributesArray[arrayIndex];
                    if (innerData.StartsWith("At will:", StringComparison.Ordinal))
                        npcModel.InnateAtWill = innerData.Substring(9);
                    if (innerData.StartsWith("5/day each:", StringComparison.Ordinal))
                        npcModel.FivePerDay = innerData.Substring(12);
                    if (innerData.StartsWith("4/day each:", StringComparison.Ordinal))
                        npcModel.FourPerDay = innerData.Substring(12);
                    if (innerData.StartsWith("3/day each:", StringComparison.Ordinal))
                        npcModel.ThreePerDay = innerData.Substring(12);
                    if (innerData.StartsWith("2/day each:", StringComparison.Ordinal))
                        npcModel.TwoPerDay = innerData.Substring(12);
                    if (innerData.StartsWith("1/day each:", StringComparison.Ordinal))
                        npcModel.OnePerDay = innerData.Substring(12);
                }
            }
            else
            {
                // For DnD Beyond, Innate spell castings are on separate lines (ES NPC was all on the same line)
                if (innateSpellcastingAttributes.StartsWith("At will:", StringComparison.Ordinal))
                    npcModel.InnateAtWill = innateSpellcastingAttributes.Substring(9);
                if (innateSpellcastingAttributes.StartsWith("5/day each:", StringComparison.Ordinal))
                    npcModel.FivePerDay = innateSpellcastingAttributes.Substring(12);
                if (innateSpellcastingAttributes.StartsWith("4/day each:", StringComparison.Ordinal))
                    npcModel.FourPerDay = innateSpellcastingAttributes.Substring(12);
                if (innateSpellcastingAttributes.StartsWith("3/day each:", StringComparison.Ordinal))
                    npcModel.ThreePerDay = innateSpellcastingAttributes.Substring(12);
                if (innateSpellcastingAttributes.StartsWith("2/day each:", StringComparison.Ordinal))
                    npcModel.TwoPerDay = innateSpellcastingAttributes.Substring(12);
                if (innateSpellcastingAttributes.StartsWith("1/day each:", StringComparison.Ordinal))
                    npcModel.OnePerDay = innateSpellcastingAttributes.Substring(12);
            }
        }

        /// <summary>
        /// 'Spellcasting. V1_npc_all is an 18th-level spellcaster. His spellcasting ability is Constitution (spell save DC 8, +12 to hit with spell attacks). V1_npc_all has the following Sorcerer spells prepared:\rCantrips (At will): Cantrips1\r1st level (9 slots): Spell 1st\r2nd level (8 slots): Spell 2nd\r3rd level (7 slots): Spell 3rd\r4th level (6 slots): Spell 4th\r5th level (5 slots): Spell 5th\r6th level (4 slots): Spell 6th\r7th level (3 slots): Spell 7th\r8th level (2 slots): Spell 8th\r9th level (1 slot): Spell 9th\r*Spell 2nd'
        /// </summary>
        public void ParseSpellCastingAttributes(NPCModel npcModel, string spellCastingAttributes)
        {
            if (spellCastingAttributes.StartsWith("Spellcasting"))
            {
                npcModel.SpellcastingSection = true;
                // Start with getting spellcaster level
                npcModel.SpellcastingCasterLevel = spellCastingAttributes.Substring(spellCastingAttributes.IndexOf("-level", StringComparison.Ordinal) - 4, 4).Trim();

                // Spellcasting Ability
                int abilityIsIndex = spellCastingAttributes.IndexOf("spellcasting ability is ", StringComparison.Ordinal);
                int spellSaveDCIndex = spellCastingAttributes.IndexOf("(spell save DC ", StringComparison.Ordinal);
                // 24 is the string length to "spellcasting ability is "
                npcModel.SCSpellcastingAbility = spellCastingAttributes.Substring(abilityIsIndex + 24, spellSaveDCIndex - abilityIsIndex - 25);

                // Spell Save DC & Attack Bonus
                int spellAttacksIndex = spellCastingAttributes.IndexOf(" to hit with spell attacks)", StringComparison.Ordinal);
                string spellSaveAndAttackData = spellCastingAttributes.Substring(spellSaveDCIndex, spellAttacksIndex - spellSaveDCIndex);
                foreach (string subpart in spellSaveAndAttackData.Split(' '))
                {
                    if (subpart.Contains(","))
                    {
                        npcModel.SpellcastingSpellSaveDC = int.Parse(subpart.Replace(',', ' '), CultureInfo.CurrentCulture);
                    }
                    if (subpart.Contains('+') || subpart.Contains('-'))
                        npcModel.SpellcastingSpellHitBonus = parseAttributeStringToInt(subpart);
                }

                // Spell Class
                int hasTheFollowingIndex = spellCastingAttributes.IndexOf("has the following ");
                int spellsPreparedIndex = spellCastingAttributes.IndexOf(" spells prepared:");
                npcModel.SpellcastingSpellClass = spellCastingAttributes.Substring(hasTheFollowingIndex + 18, spellsPreparedIndex - hasTheFollowingIndex - 18);
                npcModel.FlavorText = "";

                // Parse through all the spell slots, based on the phrase "\r"
                ParseSpellLevelAndList(spellCastingAttributes, npcModel);
            }
            //throw new NotImplementedException();
        }

        private void ParseSpellLevelAndList(string spellAttributes, NPCModel npcModel)
        {
            foreach (string spellData in spellAttributes.Split(new string[] { "\\r" }, StringSplitOptions.None))
            {
                string[] spellDataArray = spellData.Split(' ');
                switch (spellDataArray[0])
                {
                    case "Cantrips":
                        npcModel.CantripSpells = (spellDataArray[1] + " " + spellDataArray[2]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.CantripSpellList = appendSpellList(spellDataArray, 3);
                        break;
                    case "1st":
                        npcModel.FirstLevelSpells = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.FirstLevelSpellList = appendSpellList(spellDataArray, 4);
                        break;
                    case "2nd":
                        npcModel.SecondLevelSpells = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.SecondLevelSpellList = appendSpellList(spellDataArray, 4);
                        break;
                    case "3rd":
                        npcModel.ThirdLevelSpells = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.ThirdLevelSpellList = appendSpellList(spellDataArray, 4);
                        break;
                    case "4th":
                        npcModel.FourthLevelSpells = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.FourthLevelSpellList = appendSpellList(spellDataArray, 4);
                        break;
                    case "5th":
                        npcModel.FifthLevelSpells = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.FifthLevelSpellList = appendSpellList(spellDataArray, 4);
                        break;
                    case "6th":
                        npcModel.SixthLevelSpells = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.SixthLevelSpellList = appendSpellList(spellDataArray, 4);
                        break;
                    case "7th":
                        npcModel.SeventhLevelSpells = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.SeventhLevelSpellList = appendSpellList(spellDataArray, 4);
                        break;
                    case "8th":
                        npcModel.EighthLevelSpells = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.EighthLevelSpellList = appendSpellList(spellDataArray, 4);
                        break;
                    case "9th":
                        npcModel.NinthLevelSpells = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.NinthLevelSpellList = appendSpellList(spellDataArray, 4);
                        break;
                    default:
                        if (!spellData.Contains("spellcasting ability is"))
                            npcModel.MarkedSpells = appendSpellList(spellDataArray, 0);
                        break;
                }
            }
        }
        private string appendSpellList(string[] spellDataArray, int startIndex)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = startIndex; index < spellDataArray.Length; index++)
            {
                stringBuilder.Append(spellDataArray[index]).Append(" ");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 'Parry. You know what it does.. NINJA DODGE.'
        /// </summary>
        public void ParseReaction(NPCModel npcModel, string reaction)
        {
            string[] reactionArray = reaction.Split('.');
            ActionModelBase reactionModel = new ActionModelBase();
            reactionModel.ActionName = reactionArray[0];
            StringBuilder stringBuilder = new StringBuilder();
            for (int idx = 1; idx < reactionArray.Length; idx++)
            {
                stringBuilder.Append(reactionArray[idx]).Append(".");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            reactionModel.ActionDescription = stringBuilder.ToString().Trim();

            npcModel.Reactions.Add(reactionModel);
        }

        /// <summary>
        /// 'Options. This creature has 5 legendary actions.'
        /// </summary>
        public void ParseLegendaryAction(NPCModel npcModel, string legendaryAction)
        {
            if (String.IsNullOrEmpty(legendaryAction))
                return;
            LegendaryActionModel legendaryActionModel = new LegendaryActionModel();
            if (legendaryAction.Contains("choosing from the options below"))
            {
                legendaryActionModel.ActionName = "Options";
                legendaryActionModel.ActionDescription = legendaryAction;
                npcModel.LegendaryActions.Add(legendaryActionModel);
                return;
            }
            string[] legendaryActionArray = legendaryAction.Split('.');
            legendaryActionModel.ActionName = legendaryActionArray[0];
            StringBuilder stringBuilder = new StringBuilder();
            for (int idx = 1; idx < legendaryActionArray.Length; idx++)
            {
                stringBuilder.Append(legendaryActionArray[idx]).Append(".");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            legendaryActionModel.ActionDescription = stringBuilder.ToString().Trim();

            npcModel.LegendaryActions.Add(legendaryActionModel);
        }
    }
}
