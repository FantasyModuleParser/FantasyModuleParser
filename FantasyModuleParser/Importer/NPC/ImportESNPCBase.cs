using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Models.Action;
using log4net;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FantasyModuleParser.Importer.NPC
{
    public abstract class ImportESNPCBase : ImportNPCBase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Innate Spellcasting. V1_npc_all's innate spellcasting ability is Wisdom (spell save DC 8, +30 to hit with spell attacks). He can innately cast the following spells, requiring no material components:\rAt will: Super Cantrips\r5/day each: Daylight\r4/day each: False Life\r3/day each: Hunger\r2/day each: Breakfast, Lunch, Dinner\r1/day each: Nom Noms
        /// </summary>
        /// <param name="npcModel"></param>
        /// <param name="innateSpellcastingAttributes"></param>
        public new void ParseInnateSpellCastingAttributes(NPCModel npcModel, string innateSpellcastingAttributes)
        {
            if (innateSpellcastingAttributes.StartsWith("Innate Spellcasting"))
            {
                npcModel.InnateSpellcastingSection = true;
                if (innateSpellcastingAttributes.StartsWith("Innate Spellcasting (Psionics)"))
                {
                    npcModel.Psionics = true;
                }

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
                        {
                            npcModel.InnateSpellSaveDC = int.Parse(subpart.Replace(',', ' '), CultureInfo.CurrentCulture);
                        }

                        if (subpart.Contains('+') || subpart.Contains('-'))
                        {
                            npcModel.InnateSpellHitBonus = parseAttributeStringToInt(subpart);
                        }
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
                    {
                        npcModel.InnateAtWill = innerData.Substring(9).TrimEnd();
                    }

                    if (innerData.StartsWith("5/day", StringComparison.Ordinal))
                    {
                        npcModel.FivePerDay = innerData.Substring(6 + eachIndexModifier).TrimEnd();
                    }

                    if (innerData.StartsWith("4/day", StringComparison.Ordinal))
                    {
                        npcModel.FourPerDay = innerData.Substring(6 + eachIndexModifier).TrimEnd();
                    }

                    if (innerData.StartsWith("3/day", StringComparison.Ordinal))
                    {
                        npcModel.ThreePerDay = innerData.Substring(6 + eachIndexModifier).TrimEnd();
                    }

                    if (innerData.StartsWith("2/day", StringComparison.Ordinal))
                    {
                        npcModel.TwoPerDay = innerData.Substring(6 + eachIndexModifier).TrimEnd();
                    }

                    if (innerData.StartsWith("1/day", StringComparison.Ordinal))
                    {
                        npcModel.OnePerDay = innerData.Substring(6 + eachIndexModifier).TrimEnd();
                    }
                }

                // Per Anton & Trivishta unit tests, the gender is parsed here
                if (innateSpellcastingAttributes.Contains(" His "))
                {
                    npcModel.NPCGender = "male";
                }

                if (innateSpellcastingAttributes.Contains(" Her "))
                {
                    npcModel.NPCGender = "female";
                }

                // Per Anton & Trivishta unit tests, the Name boolean value is determined here
                // true == "Spellcasting. Trivishta ...."
                // false == "Spellcasting. The trivishta ..."

                npcModel.NPCNamed = !innateSpellcastingAttributes.Split(' ')[2].Equals("The");
            }
        }

        /// <summary>
        /// 'Spellcasting. V1_npc_all is an 18th-level spellcaster. His spellcasting ability is Constitution (spell save DC 8, +12 to hit with spell attacks). V1_npc_all has the following Sorcerer spells prepared:\rCantrips (At will): Cantrips1\r1st level (9 slots): Spell 1st\r2nd level (8 slots): Spell 2nd\r3rd level (7 slots): Spell 3rd\r4th level (6 slots): Spell 4th\r5th level (5 slots): Spell 5th\r6th level (4 slots): Spell 6th\r7th level (3 slots): Spell 7th\r8th level (2 slots): Spell 8th\r9th level (1 slot): Spell 9th\r*Spell 2nd'
        /// </summary>
        /// <param name="npcModel"></param>
        /// <param name="spellCastingAttributes"></param>
        public new void ParseSpellCastingAttributes(NPCModel npcModel, string spellCastingAttributes)
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
                        {
                            npcModel.SpellcastingSpellHitBonus = parseAttributeStringToInt(subpart);
                        }
                    }
                }
                else
                {
                    string spellCastingSaveDCString = spellCastingAttributes.Substring(spellSaveDCIndex);
                    spellCastingSaveDCString = spellCastingSaveDCString.Substring(0, spellCastingSaveDCString.IndexOf(").", StringComparison.Ordinal));
                    npcModel.SpellcastingSpellSaveDC = int.Parse(spellCastingSaveDCString.Substring("(spell save DC ".Length), CultureInfo.CurrentCulture);
                }

                // Spell Class
                int hasTheFollowingIndex = spellCastingAttributes.IndexOf("the following ");
                int spellsPreparedIndex = spellCastingAttributes.IndexOf(" spells ");
                if ((spellsPreparedIndex - hasTheFollowingIndex - 14) > 0)
                {
                    npcModel.SpellcastingSpellClass = spellCastingAttributes.Substring(hasTheFollowingIndex + 14, spellsPreparedIndex - hasTheFollowingIndex - 14);
                }

                if (npcModel.SpellcastingSpellClass != null && npcModel.SpellcastingSpellClass.Length > 0)
                {
                    npcModel.SpellcastingSpellClass = ("" + npcModel.SpellcastingSpellClass[0]).ToUpper() + npcModel.SpellcastingSpellClass.Substring(1);
                }

                if (spellCastingAttributes.IndexOf(" spells prepared:") == -1)
                {
                    npcModel.FlavorText = spellCastingAttributes.Substring(spellsPreparedIndex + 8, spellCastingAttributes.IndexOf(":\\r") - (spellsPreparedIndex + 8));
                }
                else
                {
                    npcModel.FlavorText = "";
                }

                // Parse through all the spell slots, based on the phrase "\r"
                ParseSpellLevelAndList(spellCastingAttributes, npcModel);

                // Per Anton & Trivishta unit tests, the gender is parsed here
                if (spellCastingAttributes.Contains(" His "))
                {
                    npcModel.NPCGender = "male";
                }
                if (spellCastingAttributes.Contains(" Her "))
                {
                    npcModel.NPCGender = "female";
                }

                // Per Anton & Trivishta unit tests, the Name boolean value is determined here
                // true == "Spellcasting. Trivishta ...."
                // false == "Spellcasting. The trivishta ..."

                npcModel.NPCNamed = !spellCastingAttributes.Split(' ')[1].Equals("The");

            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spellAttributes"></param>
        /// <param name="npcModel"></param>
        private void ParseSpellLevelAndList(string spellAttributes, NPCModel npcModel)
        {
            foreach (string spellData in spellAttributes.Split(new string[] { "\\r" }, StringSplitOptions.None))
            {
                string[] spellDataArray = spellData.Split(' ');
                switch (spellDataArray[0])
                {
                    case "Cantrips":
                        npcModel.CantripSpellSlots = (spellDataArray[1] + " " + spellDataArray[2]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').ToLower().Trim();
                        npcModel.CantripSpellList = appendSpellList(spellDataArray, 3);
                        break;
                    case "1st":
                        npcModel.FirstLevelSpellSlots = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.FirstLevelSpellList = appendSpellList(spellDataArray, 4).TrimEnd();
                        break;
                    case "2nd":
                        npcModel.SecondLevelSpellSlots = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.SecondLevelSpellList = appendSpellList(spellDataArray, 4);
                        break;
                    case "3rd":
                        npcModel.ThirdLevelSpellSlots = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.ThirdLevelSpellList = appendSpellList(spellDataArray, 4);
                        break;
                    case "4th":
                        npcModel.FourthLevelSpellSlots = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.FourthLevelSpellList = appendSpellList(spellDataArray, 4);
                        break;
                    case "5th":
                        npcModel.FifthLevelSpellSlots = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.FifthLevelSpellList = appendSpellList(spellDataArray, 4);
                        break;
                    case "6th":
                        npcModel.SixthLevelSpellSlots = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.SixthLevelSpellList = appendSpellList(spellDataArray, 4);
                        break;
                    case "7th":
                        npcModel.SeventhLevelSpellSlots = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.SeventhLevelSpellList = appendSpellList(spellDataArray, 4);
                        break;
                    case "8th":
                        npcModel.EighthLevelSpellSlots = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.EighthLevelSpellList = appendSpellList(spellDataArray, 4);
                        break;
                    case "9th":
                        npcModel.NinthLevelSpellSlots = (spellDataArray[2] + " " + spellDataArray[3]).Replace('(', ' ').Replace(')', ' ').Replace(':', ' ').Trim();
                        npcModel.NinthLevelSpellList = appendSpellList(spellDataArray, 4);
                        break;
                    default:
                        if (!spellData.Contains("spellcasting ability is"))
                        {
                            npcModel.MarkedSpells = appendSpellList(spellDataArray, 0);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 'Parry. You know what it does.. NINJA DODGE.'
        /// </summary>
        /// <param name="npcModel"></param>
        /// <param name="reaction"></param>
        public new void ParseReaction(NPCModel npcModel, string reaction)
        {
			if (reaction.Length == 0 || reaction.Trim().Length == 0) { return; }

			string[] reactionArray = reaction.Split('.');

            if (reactionArray.Length <= 1)
            {
                log.Error("Failed to parse the line in Reactions :: " + reaction + Environment.NewLine + "The Reaction description appears to be missing.");
                throw new ApplicationException(Environment.NewLine +
                    "Failed to parse the line in Reactions :: " + reaction +
                    Environment.NewLine + "The Rection description appears to be missing." +
                    Environment.NewLine + "An example would be \"Parry. The noble adds 2 to its AC against one melee attack that would hit it. To do so, the noble must see the attacker and be wielding a melee weapon.\" (without the double quotes)");
            }

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
        /// <param name="npcModel"></param>
        /// <param name="legendaryAction"></param>
        public new void ParseLegendaryAction(NPCModel npcModel, string legendaryAction)
        {
			if (string.IsNullOrWhiteSpace(legendaryAction)) { return; }

			string[] legendaryActionArray = legendaryAction.Split('.');

            if (legendaryActionArray.Length <= 1)
            {
                log.Error("Failed to parse the line in Legendary Actions :: " + legendaryAction + Environment.NewLine + "The Legendary Action description appears to be missing.");
                throw new ApplicationException(Environment.NewLine +
                    "Failed to parse the line in Legendary Actions :: " + legendaryAction +
                    Environment.NewLine + "The Legendary Action description appears to be missing." +
                    Environment.NewLine + "An example would be \"Detect. The golbin makes a Wisdom (Perception) check.\" (without the double quotes)");
            }

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
			if (string.IsNullOrWhiteSpace(lairAction)) { return; }

			LairAction lairActionModel = new LairAction();
            lairActionModel.ActionName = npcModel.LairActions.Count.ToString();
            lairActionModel.ActionDescription = lairAction;
            npcModel.LairActions.Add(lairActionModel);
        }
    }
}

