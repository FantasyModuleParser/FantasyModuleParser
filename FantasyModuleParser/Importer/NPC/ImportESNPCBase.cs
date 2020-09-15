using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Models.Action;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FantasyModuleParser.Importer.NPC
{
    public abstract class ImportESNPCBase : ImportNPCBase
    {

        /// <summary>
        /// 'STR DEX CON INT WIS CHA 10 (+0) 11 (+0) 12 (+1) 13 (+1) 14 (+2) 15 (+2)'
        /// </summary>
        public void ParseStatAttributes(NPCModel npcModel, string statAttributes)
        {
            if (statAttributes.StartsWith("STR DEX CON INT WIS CHA"))
            {
                string[] splitAttributes = statAttributes.Split(' ');
                npcModel.AttributeStr = int.Parse(splitAttributes[6], CultureInfo.CurrentCulture);
                npcModel.AttributeDex = int.Parse(splitAttributes[8], CultureInfo.CurrentCulture);
                npcModel.AttributeCon = int.Parse(splitAttributes[10], CultureInfo.CurrentCulture);
                npcModel.AttributeInt = int.Parse(splitAttributes[12], CultureInfo.CurrentCulture);
                npcModel.AttributeWis = int.Parse(splitAttributes[14], CultureInfo.CurrentCulture);
                npcModel.AttributeCha = int.Parse(splitAttributes[16], CultureInfo.CurrentCulture);
            }
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
                if (innateSpellcastingAttributes.StartsWith("Innate Spellcasting (Psionics)"))
                    npcModel.Psionics = true;
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
                    string spellSaveAndAttackData = innateSpellcastingAttributes.Substring(spellSaveDCIndex, spellAttacksIndex - spellSaveDCIndex);
                    foreach (string subpart in spellSaveAndAttackData.Split(' '))
                    {
                        if (subpart.Contains(","))
                            npcModel.InnateSpellSaveDC = int.Parse(subpart.Replace(',', ' '), CultureInfo.CurrentCulture);
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
                npcModel.ComponentText = innateSpellcastingAttributes.Substring(preComponentText + 18, postComponentText - preComponentText - 18);

                string[] innateSpellcastingAttributesArray = innateSpellcastingAttributes.Split(new string[] { "\\r" }, StringSplitOptions.RemoveEmptyEntries);
                for (int arrayIndex = 1; arrayIndex < innateSpellcastingAttributesArray.Length; arrayIndex++)
                {
                    string innerData = innateSpellcastingAttributesArray[arrayIndex];
                    int eachIndexModifier = innerData.Contains(" each:") ? 6 : 1;
                    if (innerData.StartsWith("At will:", StringComparison.Ordinal))
                        npcModel.InnateAtWill = innerData.Substring(9).TrimEnd();
                    if (innerData.StartsWith("5/day", StringComparison.Ordinal))
                        npcModel.FivePerDay = innerData.Substring(6 + eachIndexModifier).TrimEnd();
                    if (innerData.StartsWith("4/day", StringComparison.Ordinal))
                        npcModel.FourPerDay = innerData.Substring(6 + eachIndexModifier).TrimEnd();
                    if (innerData.StartsWith("3/day", StringComparison.Ordinal))
                        npcModel.ThreePerDay = innerData.Substring(6 + eachIndexModifier).TrimEnd();
                    if (innerData.StartsWith("2/day", StringComparison.Ordinal))
                        npcModel.TwoPerDay = innerData.Substring(6 + eachIndexModifier).TrimEnd();
                    if (innerData.StartsWith("1/day", StringComparison.Ordinal))
                        npcModel.OnePerDay = innerData.Substring(6 + eachIndexModifier).TrimEnd();
                }
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
                int spellAttacksIndex = spellCastingAttributes.IndexOf(" to hit with spell attacks).", StringComparison.Ordinal);
                if (spellAttacksIndex != -1)
                {
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
                }
                else
                {
                    string spellCastingSaveDCString = spellCastingAttributes.Substring(spellSaveDCIndex);
                    spellCastingSaveDCString = spellCastingSaveDCString.Substring(0, spellCastingSaveDCString.IndexOf(").", StringComparison.Ordinal));
                    npcModel.SpellcastingSpellSaveDC = int.Parse(spellCastingSaveDCString.Substring("(spell save DC ".Length), CultureInfo.CurrentCulture);
                }

                // Spell Class
                int hasTheFollowingIndex = spellCastingAttributes.IndexOf("has the following ");
                int spellsPreparedIndex = spellCastingAttributes.IndexOf(" spells prepared:");
                npcModel.SpellcastingSpellClass = spellCastingAttributes.Substring(hasTheFollowingIndex + 18, spellsPreparedIndex - hasTheFollowingIndex - 18);
                if(npcModel.SpellcastingSpellClass.Length > 0)
                    npcModel.SpellcastingSpellClass = ("" + npcModel.SpellcastingSpellClass[0]).ToUpper() + npcModel.SpellcastingSpellClass.Substring(1);
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
                        npcModel.CantripSpells = (spellDataArray[1] + " " + spellDataArray[2]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').ToLower().Trim();
                        npcModel.CantripSpellList = appendSpellList(spellDataArray, 3);
                        break;
                    case "1st":
                        npcModel.FirstLevelSpells = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.FirstLevelSpellList = appendSpellList(spellDataArray, 4).TrimEnd();
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

        /// <summary>
        /// 'Parry. You know what it does.. NINJA DODGE.'
        /// </summary>
        public void ParseReaction(NPCModel npcModel, string reaction)
        {
            if (reaction.Length == 0 || reaction.Trim().Length == 0)
                return;
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
            if (legendaryAction.Length == 0 || legendaryAction.Trim().Length == 0)
                return;
            string[] legendaryActionArray = legendaryAction.Split('.');
            LegendaryActionModel legendaryActionModel = new LegendaryActionModel();
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

        /// <summary>
        /// This is an odd one, as the Lair description is laid out in ***Part 1***, but the 
        /// names are in ***Part 3***
        /// </summary>
        /// <example>
        /// All the options of the lair:
        /// </example>
        public void ParseLairAction(NPCModel npcModel, string lairAction)
        {
            if (lairAction.Trim().Length <= 0)
                return;
            LairAction lairActionModel = new LairAction();
            lairActionModel.ActionName = npcModel.LairActions.Count.ToString();
            lairActionModel.ActionDescription = lairAction;
            npcModel.LairActions.Add(lairActionModel);
        }
    }
}

