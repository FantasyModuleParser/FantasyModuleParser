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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FantasyModuleParser.Importer.NPC
{
    public abstract class ImportNPCBase : IImportNPC
    {
        public ImportCommonUtils importCommonUtils = new ImportCommonUtils();
        public abstract NPCModel ImportTextToNPCModel(string importTextContent);

        /// <summary>
        /// Declares all the 'continue' flags used in Importers
        /// </summary>
        public bool continueStrengthFlag = false;
        public bool continueDexterityFlag = false;
        public bool continueConstitutionFlag = false;
        public bool continueIntelligenceFlag = false;
        public bool continueWisdomFlag = false;
        public bool continueCharismaFlag = false;
        public bool continueBaseStatsFlag = false;
        public bool continueTraitsFlag = false;
        public bool continueInnateSpellcastingFlag = false;
        public bool continueSpellcastingFlag = false;
        public bool continueActionsFlag = false;
        public bool continueReactionsFlag = false;
        public bool continueLegendaryActionsFlag = false;
        public bool continueLairActionsFlag = false;

        /// <summary>
        /// Resets all 'continue' flags to false;  Used in ImportTextToNPCModel for run-on lines (e.g. Traits, Actions, etc...)
        /// </summary>
        public void resetContinueFlags()
        {
            continueStrengthFlag = false;
            continueDexterityFlag = false;
            continueConstitutionFlag = false;
            continueIntelligenceFlag = false;
            continueWisdomFlag = false;
            continueCharismaFlag = false;
            continueBaseStatsFlag = false;
            continueTraitsFlag = false;
            continueInnateSpellcastingFlag = false;
            continueSpellcastingFlag = false;
            continueActionsFlag = false;
            continueReactionsFlag = false;
            continueLegendaryActionsFlag = false;
            continueLairActionsFlag = false;
        }

        /// <summary>
        /// 'Tiny beast (devil), lawful neutral'
        /// </summary>
        public void ParseSizeAndAlignment(NPCModel npcModel, string sizeAndAlignment)
        {
            string[] npcCharacteristics = sizeAndAlignment.Split(' ');
            npcModel.Size = npcCharacteristics[0];
            string tag = npcCharacteristics[1].ToLower();
            if (tag.EndsWith(","))
                npcModel.NPCType = tag.Substring(0, tag.Length - 1);
            else
                npcModel.NPCType = npcCharacteristics[1].ToLower();

            if (npcCharacteristics[2].Contains("(") && npcCharacteristics[2].EndsWith(","))
                // includes removing the comma character at the end
                npcModel.Tag = npcCharacteristics[2].ToLower().Substring(0, npcCharacteristics[2].Length - 1);
            else if (npcCharacteristics.Length > 3)
                npcModel.Alignment = npcCharacteristics[2] + " " + npcCharacteristics[3];
            else
                npcModel.Alignment = npcCharacteristics[2];

            if (npcCharacteristics.Length > 3 && npcCharacteristics[3].Contains(")"))
                npcModel.Tag = npcModel.Tag + ", " + npcCharacteristics[3].ToLower().Substring(0, npcCharacteristics[3].Length - 1);
            if (npcModel.Tag != null && npcModel.Tag.Length > 0)
                if (npcCharacteristics.Length > 4)
                    if (npcModel.Tag.Contains(","))
                        npcModel.Alignment = npcCharacteristics[4] + " " + npcCharacteristics[5];
                    else
                        npcModel.Alignment = npcCharacteristics[3] + " " + npcCharacteristics[4];
                else
                    npcModel.Alignment = npcCharacteristics[3];
        }

        /// <summary>
        /// 'Armor Class 16 (Natural Armor)'
        /// </summary>
        public void ParseArmorClass(NPCModel npcModel, string armorClass)
        {
            if (armorClass.StartsWith("Armor Class ", StringComparison.Ordinal))
            {
                npcModel.AC = armorClass.Substring(12);
            }
        }

        /// <summary>
        /// 'Hit Points 90 (10d8 + 44)'
        /// </summary>
        public void ParseHitPoints(NPCModel npcModel, string hitPoints)
        {
            if (hitPoints.StartsWith("Hit Points"))
                npcModel.HP = hitPoints.Substring(11);
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
        /// 'Speed 10 ft., burrow 20 ft., climb 30 ft., fly 40 ft. (hover), swim 50 ft.'
        /// </summary>S
        public void ParseSpeedAttributes(NPCModel npcModel, string speedAttributes)
        {
            if (speedAttributes == null || speedAttributes.Length == 0)
            {
                npcModel.Speed = 0;
                npcModel.Burrow = 0;
                npcModel.Climb = 0;
                npcModel.Fly = 0;
                npcModel.Hover = false;
                npcModel.Swim = 0;
                return;
            }
            foreach (string speedAttribute in speedAttributes.Split(','))
            {
                var trimmedSpeedAttribute = speedAttribute.Trim().ToLower(CultureInfo.CurrentCulture);
                if (trimmedSpeedAttribute.StartsWith("speed ", StringComparison.Ordinal))
                    npcModel.Speed = int.Parse(trimmedSpeedAttribute.Split(' ')[1], CultureInfo.CurrentCulture);
                if (trimmedSpeedAttribute.StartsWith("burrow ", StringComparison.Ordinal))
                    npcModel.Burrow = int.Parse(trimmedSpeedAttribute.Split(' ')[1], CultureInfo.CurrentCulture);
                if (trimmedSpeedAttribute.StartsWith("climb ", StringComparison.Ordinal))
                    npcModel.Climb = int.Parse(trimmedSpeedAttribute.Split(' ')[1], CultureInfo.CurrentCulture);
                if (trimmedSpeedAttribute.StartsWith("fly ", StringComparison.Ordinal))
                    npcModel.Fly = int.Parse(trimmedSpeedAttribute.Split(' ')[1], CultureInfo.CurrentCulture);
                if (trimmedSpeedAttribute.Contains("(hover)"))
                    npcModel.Hover = true;
                if (trimmedSpeedAttribute.StartsWith("swim ", StringComparison.Ordinal))
                    npcModel.Swim = int.Parse(trimmedSpeedAttribute.Split(' ')[1], CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// 'Saving Throws Str +1, Dex +2, Con +3, Int +0, Wis +5, Cha +6'
        /// </summary>
        public void ParseSavingThrows(NPCModel npcModel, string savingThrows)
        {
            if (savingThrows.StartsWith("Saving Throws"))
            {
                string[] splitSavingThrows = savingThrows.Split(' ');
                bool isStr = false, isDex = false, isCon = false, isInt = false, isWis = false, isCha = false;
                bool attributeIdentified = false;
                foreach (string savingThrowWord in splitSavingThrows)
                {
                    if (savingThrowWord.Equals("Saving", StringComparison.Ordinal) || savingThrowWord.Equals("Throws", StringComparison.Ordinal))
                        continue;

                    if (attributeIdentified)
                    {
                        if (isStr)
                        {
                            npcModel.SavingThrowStr = parseAttributeStringToInt(savingThrowWord);
                            npcModel.SavingThrowStrBool = npcModel.SavingThrowStr == 0;
                        }
                        if (isDex)
                        {
                            npcModel.SavingThrowDex = parseAttributeStringToInt(savingThrowWord);
                            npcModel.SavingThrowDexBool = npcModel.SavingThrowDex == 0;
                        }
                        if (isCon)
                        {
                            npcModel.SavingThrowCon = parseAttributeStringToInt(savingThrowWord);
                            npcModel.SavingThrowConBool = npcModel.SavingThrowCon == 0;
                        }
                        if (isInt)
                        {
                            npcModel.SavingThrowInt = parseAttributeStringToInt(savingThrowWord);
                            npcModel.SavingThrowIntBool = npcModel.SavingThrowInt == 0;
                        }
                        if (isWis)
                        {
                            npcModel.SavingThrowWis = parseAttributeStringToInt(savingThrowWord);
                            npcModel.SavingThrowWisBool = npcModel.SavingThrowWis == 0;
                        }
                        if (isCha)
                        {
                            npcModel.SavingThrowCha = parseAttributeStringToInt(savingThrowWord);
                            npcModel.SavingThrowChaBool = npcModel.SavingThrowCha == 0;
                        }

                        attributeIdentified = false; // Reset for next attribute check
                    }
                    else
                    {
                        isStr = savingThrowWord.ToUpper(CultureInfo.CurrentCulture).Equals("STR", StringComparison.Ordinal);
                        isDex = savingThrowWord.ToUpper(CultureInfo.CurrentCulture).Equals("DEX", StringComparison.Ordinal);
                        isCon = savingThrowWord.ToUpper(CultureInfo.CurrentCulture).Equals("CON", StringComparison.Ordinal);
                        isInt = savingThrowWord.ToUpper(CultureInfo.CurrentCulture).Equals("INT", StringComparison.Ordinal);
                        isWis = savingThrowWord.ToUpper(CultureInfo.CurrentCulture).Equals("WIS", StringComparison.Ordinal);
                        isCha = savingThrowWord.ToUpper(CultureInfo.CurrentCulture).Equals("CHA", StringComparison.Ordinal);
                        attributeIdentified = true;
                    }
                }

            }
        }

        public int parseAttributeStringToInt(string savingThrowValue)
        {
            if (savingThrowValue.Length == 0 || savingThrowValue.Trim().Length == 0)
                return 0;
            savingThrowValue = savingThrowValue.Replace('+', ' ');
            savingThrowValue = savingThrowValue.Replace(',', ' ');
            string savingThrowValueSubstring = savingThrowValue.Trim();
            return int.Parse(savingThrowValueSubstring, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// 'Skills Acrobatics +1, Animal Handling +2, Arcana +3, Athletics +4, Deception +5, History +6, Insight +7, Intimidation +8, Investigation +9,
        ///  Medicine +10, Nature +11, Perception +12, Performance +13, Persuasion +14, Religion +15, Sleight of Hand +16, Stealth +17, Survival +18'
        /// </summary>
        public void ParseSkillAttributes(NPCModel npcModel, string skillAttributes)
        {
            int columnIndex = 0;
            string[] skillAttributeArray = skillAttributes.Split(' ');
            foreach (string skillAttributeValue in skillAttributeArray)
            {
                switch (skillAttributeValue)
                {
                    case "Acrobatics":
                        npcModel.Acrobatics = parseAttributeStringToInt(skillAttributeArray[columnIndex + 1]);
                        break;
                    case "Animal":
                        npcModel.AnimalHandling = parseAttributeStringToInt(skillAttributeArray[columnIndex + 2]);
                        break;
                    case "Arcana":
                        npcModel.Arcana = parseAttributeStringToInt(skillAttributeArray[columnIndex + 1]);
                        break;
                    case "Athletics":
                        npcModel.Athletics = parseAttributeStringToInt(skillAttributeArray[columnIndex + 1]);
                        break;
                    case "Deception":
                        npcModel.Deception = parseAttributeStringToInt(skillAttributeArray[columnIndex + 1]);
                        break;
                    case "History":
                        npcModel.History = parseAttributeStringToInt(skillAttributeArray[columnIndex + 1]);
                        break;
                    case "Insight":
                        npcModel.Insight = parseAttributeStringToInt(skillAttributeArray[columnIndex + 1]);
                        break;
                    case "Intimidation":
                        npcModel.Intimidation = parseAttributeStringToInt(skillAttributeArray[columnIndex + 1]);
                        break;
                    case "Investigation":
                        npcModel.Investigation = parseAttributeStringToInt(skillAttributeArray[columnIndex + 1]);
                        break;
                    case "Medicine":
                        npcModel.Medicine = parseAttributeStringToInt(skillAttributeArray[columnIndex + 1]);
                        break;
                    case "Nature":
                        npcModel.Nature = parseAttributeStringToInt(skillAttributeArray[columnIndex + 1]);
                        break;
                    case "Perception":
                        npcModel.Perception = parseAttributeStringToInt(skillAttributeArray[columnIndex + 1]);
                        break;
                    case "Performance":
                        npcModel.Performance = parseAttributeStringToInt(skillAttributeArray[columnIndex + 1]);
                        break;
                    case "Persuasion":
                        npcModel.Persuasion = parseAttributeStringToInt(skillAttributeArray[columnIndex + 1]);
                        break;
                    case "Religion":
                        npcModel.Religion = parseAttributeStringToInt(skillAttributeArray[columnIndex + 1]);
                        break;
                    case "Sleight":
                        npcModel.SleightOfHand = parseAttributeStringToInt(skillAttributeArray[columnIndex + 3]);
                        break;
                    case "Stealth":
                        npcModel.Stealth = parseAttributeStringToInt(skillAttributeArray[columnIndex + 1]);
                        break;
                    case "Survival":
                        npcModel.Survival = parseAttributeStringToInt(skillAttributeArray[columnIndex + 1]);
                        break;
                    default:
                        break;
                }
                columnIndex++;
            }
        }

        /// <summary>
        /// 'Damage Vulnerabilities acid, fire, lightning, poison, radiant; bludgeoning and slashing'
        /// </summary>
        public void ParseDamageVulnerabilities(NPCModel npcModel, string damageVulnerabilites)
        {
            if (damageVulnerabilites.StartsWith("Damage Vulnerabilities", StringComparison.Ordinal))
                npcModel.DamageVulnerabilityModelList = parseDamageTypeStringToList(damageVulnerabilites);
            else
                // Populate with all options deselected
                npcModel.DamageVulnerabilityModelList = parseDamageTypeStringToList("");
        }

        private List<SelectableActionModel> parseDamageTypeStringToList(string damageTypes)
        {
            NPCController npcController = new NPCController();
            List<SelectableActionModel> selectableActionModels = npcController.GetSelectableActionModelList(typeof(DamageType));
            if (damageTypes.Length == 0)
                return selectableActionModels;

            foreach (string damageTypeValue in damageTypes.Split(' '))
            {
                string damageTypeValueTrimmed = damageTypeValue.Replace(',', ' ').Replace(';', ' ').Trim();
                SelectableActionModel damageTypeModel = selectableActionModels.FirstOrDefault(
                    item => item.ActionDescription.ToLower(CultureInfo.CurrentCulture).Equals(damageTypeValueTrimmed.ToLower(CultureInfo.CurrentCulture))
                    );
                if (damageTypeModel != null)
                    damageTypeModel.Selected = true;
            }
            return selectableActionModels;
        }

        /// <summary>
        /// 'Condition Immunities blinded, frightened, invisible, paralyzed, prone, restrained'
        /// </summary>
        public void ParseConditionImmunities(NPCModel npcModel, string conditionImmunities)
        {
            NPCController npcController = new NPCController();
            List<SelectableActionModel> selectableActionModels = npcController.GetSelectableActionModelList(typeof(ConditionType));

            if (conditionImmunities.Trim().Length != 0 && conditionImmunities.StartsWith("Condition Immunities", StringComparison.Ordinal))
                foreach (string conditionImmunityTypeValue in conditionImmunities.Split(' '))
                {
                    string conditionImmunityTypeValueTrimmed = conditionImmunityTypeValue.Replace(',', ' ').Replace(';', ' ').Trim();
                    SelectableActionModel conditionImmunityTypeModel = selectableActionModels.FirstOrDefault(item => item.ActionDescription.Equals(conditionImmunityTypeValueTrimmed));
                    if (conditionImmunityTypeModel != null)
                        conditionImmunityTypeModel.Selected = true;
                }
            npcModel.ConditionImmunityModelList = selectableActionModels;
        }

        /// <summary>
        /// 'Senses blindsight 60 ft. (blind beyond this radius), darkvision 70 ft., tremorsense 80 ft., truesight 90 ft., passive Perception 22'
        /// </summary>
        public void ParseVisionAttributes(NPCModel npcModel, string visionAttributes)
        {
            if (visionAttributes != null && visionAttributes.StartsWith("Senses", StringComparison.Ordinal))
            {
                if (visionAttributes.Contains("blind beyond this radius"))
                    npcModel.BlindBeyond = true;

                string[] visionAttributeArray = visionAttributes.Split(' ');
                int arrayIndex = 0;
                foreach (string attribute in visionAttributeArray)
                {
                    if (attribute.ToLower(CultureInfo.CurrentCulture).Equals("blindsight", StringComparison.Ordinal))
                        npcModel.Blindsight = int.Parse(visionAttributeArray[arrayIndex + 1], CultureInfo.CurrentCulture);
                    if (attribute.ToLower(CultureInfo.CurrentCulture).Equals("darkvision", StringComparison.Ordinal))
                        npcModel.Darkvision = int.Parse(visionAttributeArray[arrayIndex + 1], CultureInfo.CurrentCulture);
                    if (attribute.ToLower(CultureInfo.CurrentCulture).Equals("tremorsense", StringComparison.Ordinal))
                        npcModel.Tremorsense = int.Parse(visionAttributeArray[arrayIndex + 1], CultureInfo.CurrentCulture);
                    if (attribute.ToLower(CultureInfo.CurrentCulture).Equals("truesight", StringComparison.Ordinal))
                        npcModel.Truesight = int.Parse(visionAttributeArray[arrayIndex + 1], CultureInfo.CurrentCulture);
                    if (attribute.ToLower(CultureInfo.CurrentCulture).Equals("perception", StringComparison.Ordinal))
                        npcModel.PassivePerception = int.Parse(visionAttributeArray[arrayIndex + 1], CultureInfo.CurrentCulture);
                    arrayIndex++;
                }
            }
        }

        /// <summary>
        /// 'Languages Aarakocra, Bullywug, Celestial, Common, Draconic, Elvish, Gnomish, Grell, Halfling, Ice toad, Infernal, Modron, Slaad, Sylvan, Thieves' cant, Thri-kreen, Umber hulk, telepathy 90'
        /// </summary>
        public void ParseLanguages(NPCModel npcModel, string languages)
        {
            LanguageController languageController = new LanguageController();

            npcModel.StandardLanguages = languageController.GenerateStandardLanguages();
            npcModel.ExoticLanguages = languageController.GenerateExoticLanguages();
            npcModel.MonstrousLanguages = languageController.GenerateMonsterLanguages();
            npcModel.UserLanguages = new System.Collections.ObjectModel.ObservableCollection<LanguageModel>();

            string languageStringTrimmed = languages.Remove(0, 9); // Removes the 'Languages' word
            foreach (string language in languageStringTrimmed.Split(','))
            {
                string languageTrimmed = language.Trim().ToLower();
                LanguageModel standardLanguage = npcModel.StandardLanguages.FirstOrDefault(item => item.Language.ToLower().Equals(languageTrimmed));
                if (standardLanguage != null)
                {
                    standardLanguage.Selected = true;
                    continue;
                }

                LanguageModel exoticLanguage = npcModel.ExoticLanguages.FirstOrDefault(item => item.Language.ToLower().Equals(languageTrimmed));
                if (exoticLanguage != null)
                {
                    exoticLanguage.Selected = true;
                    continue;
                }
                LanguageModel monstrousLanguage = npcModel.MonstrousLanguages.FirstOrDefault(item => item.Language.ToLower().Equals(languageTrimmed));
                if (monstrousLanguage != null)
                {
                    monstrousLanguage.Selected = true;
                    continue;
                }

                if (languageTrimmed.Contains("telepathy"))
                {
                    npcModel.Telepathy = true;
                    npcModel.TelepathyRange = languageTrimmed.Replace("telepathy ", "");
                    continue;
                }

                // At this point, any other hits would be considered an User Language

                npcModel.UserLanguages.Add(new LanguageModel()
                {
                    Language = language.Trim(),
                    Selected = true
                });
            }

        }

        /// <summary>
        /// 'Challenge 8 (3,900 XP)'
        /// </summary>
        public void ParseChallengeRatingAndXP(NPCModel npcModel, string challengeRatingAndXP)
        {
            if (challengeRatingAndXP != null && challengeRatingAndXP.StartsWith("Challenge"))
            {
                string[] splitArray = challengeRatingAndXP.Split(' ');
                npcModel.ChallengeRating = splitArray[1];
                string xpString = new string(splitArray[2].Where(c => !Char.IsWhiteSpace(c) && c != '(' && c != ')').ToArray());
                npcModel.XP = int.Parse(xpString, NumberStyles.AllowThousands, CultureInfo.CurrentCulture);
            }

        }

        /// <summary>
        /// 'Trait Number 1. Some trait goes here for flavor Anger. This NPC gets angry very, very easily Unit Test. Unit Test the third'
        /// </summary>
        public void ParseTraits(NPCModel npcModel, string traits)
        {
            if (npcModel.Traits == null)
                npcModel.Traits = new System.Collections.ObjectModel.ObservableCollection<ActionModelBase>();

            if (string.IsNullOrEmpty(traits))
                return;

            string[] traitArray = traits.Split('.');
            ActionModelBase traitModel = new ActionModelBase();
            traitModel.ActionName = traitArray[0];
            StringBuilder stringBuilder = new StringBuilder();
            for (int idx = 1; idx < traitArray.Length; idx++)
            {
                stringBuilder.Append(traitArray[idx]).Append(".");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            traitModel.ActionDescription = stringBuilder.ToString().Trim();

            npcModel.Traits.Add(traitModel);
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
                if (postComponentText == -1)
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
                // Fixes a scenario where the DnD Beyond description uses a full lowercase spell class (e.g. cleric).  Needs to be first letter uppercase
                // e.g. Cleric
                npcModel.SpellcastingSpellClass = ("" + npcModel.SpellcastingSpellClass[0]).ToUpper() + npcModel.SpellcastingSpellClass.Substring(1);
                npcModel.FlavorText = "";

            }
            else
                ParseSpellLevelAndList(spellCastingAttributes, npcModel);
            //throw new NotImplementedException();
        }

        private void ParseSpellLevelAndList(string spellAttributes, NPCModel npcModel)
        {
            string[] spellDataArray = spellAttributes.Split(' ');
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
                    if (!spellAttributes.Contains("spellcasting ability is"))
                        npcModel.MarkedSpells = appendSpellList(spellDataArray, 0);
                    break;
            }
        }

        public string appendSpellList(string[] spellDataArray, int startIndex)
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
        /// 'Multiattack. .This creature makes 3 attacks.'
        /// </summary>
        public void ParseStandardAction(NPCModel npcModel, string standardAction)
        {
            // Don't deal w/ an empty string.
            if (standardAction.Length == 0 || standardAction.Trim().Length == 0)
                return;


            if (standardAction.StartsWith(Multiattack.LocalActionName))
            {
                ParseMultiattackAction(npcModel, standardAction);
                return;
            }

            // For any standard action, it will contain one of the following from the WeaponType enum description
            if (standardAction.Contains(GetDescription(typeof(WeaponType), WeaponType.MSA)) ||
                standardAction.Contains(GetDescription(typeof(WeaponType), WeaponType.MWA)) ||
                standardAction.Contains(GetDescription(typeof(WeaponType), WeaponType.RSA)) ||
                standardAction.Contains(GetDescription(typeof(WeaponType), WeaponType.RWA)) ||
                standardAction.Contains(GetDescription(typeof(WeaponType), WeaponType.SA)) ||
                standardAction.Contains(GetDescription(typeof(WeaponType), WeaponType.WA)))
            {
                ParseWeaponAttackAction(npcModel, standardAction);
                return;
            }

            // Special Case;  If the string has more than 5 words, and none of those 5 words has a period character,
            //  then append the line to the previously created Action's Description as a new line
            string[] standardActionArray = standardAction.Split(' ');
            for (int idx = 0; idx < 9; idx++)
            {
                if (standardActionArray[idx].Contains("."))
                {
                    // if not Multiattack or standard action, then it's an OtherAction
                    ParseOtherAction(npcModel, standardAction);
                    return;
                }
            }

            ActionModelBase actionModelBase = npcModel.NPCActions.Last();
            actionModelBase.ActionDescription = actionModelBase.ActionDescription + "\n\n" + standardAction;
        }

        private static string[] ParseOtherAction(NPCModel npcModel, string standardAction)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string[] standardActionArray;
            OtherAction otherActionModel = new OtherAction();
            standardActionArray = standardAction.Split('.');
            for (int idx = 1; idx < standardActionArray.Length; idx++)
            {
                stringBuilder.Append(standardActionArray[idx].Trim()).Append(". ");
            }
            otherActionModel.ActionName = standardActionArray[0];
            otherActionModel.ActionDescription = stringBuilder.Remove(stringBuilder.Length - 2, 2).ToString().Trim();
            npcModel.NPCActions.Add(otherActionModel);
            return standardActionArray;
        }

        private void ParseWeaponAttackAction(NPCModel npcModel, string standardAction)
        {
            WeaponAttack weaponAttackModel = new WeaponAttack();

            weaponAttackModel.WeaponType = importCommonUtils.GetWeaponTypeFromString(standardAction);
            weaponAttackModel.ActionName = standardAction.Split('.')[0];

            int firstColonIndex = standardAction.IndexOf(':');
            string weaponDescription = standardAction.Substring(firstColonIndex + 2);

            string[] weaponDescriptionDataSplit = weaponDescription.Split(',');

            foreach (string weaponDescriptionData in weaponDescriptionDataSplit)
            {
                if (weaponDescriptionData.Contains("to hit"))
                    weaponAttackModel.ToHit = parseAttributeStringToInt(weaponDescriptionData.Split(' ')[0]);
                if (weaponDescriptionData.Contains("reach"))
                    weaponAttackModel.Reach = parseAttributeStringToInt(weaponDescriptionData.Split(' ')[2]);
                if (weaponDescriptionData.Contains("range"))
                {
                    int rangeIndex = weaponDescriptionData.IndexOf("range ", StringComparison.Ordinal);
                    string rangeStringValue = weaponDescriptionData.Substring(rangeIndex + 6).Split(' ')[0];

                    if (rangeStringValue.Contains("/"))
                    {
                        weaponAttackModel.WeaponRangeShort = int.Parse(rangeStringValue.Split('/')[0], CultureInfo.CurrentCulture);
                        weaponAttackModel.WeaponRangeLong = int.Parse(rangeStringValue.Split('/')[1], CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        weaponAttackModel.WeaponRangeShort = int.Parse(rangeStringValue, CultureInfo.CurrentCulture);

                    }
                }
                if (weaponDescriptionData.Contains("one target"))
                    weaponAttackModel.TargetType = TargetType.target;
                if (weaponDescriptionData.Contains("one creature"))
                    weaponAttackModel.TargetType = TargetType.creature;
            }

            ParseWeaponAttackDamageText(weaponAttackModel, weaponDescription);

            // Update the WA description
            ActionController actionController = new ActionController();
            actionController.GenerateWeaponDescription(weaponAttackModel);

            npcModel.NPCActions.Add(weaponAttackModel);
        }

        private void ParseWeaponAttackDamageText(WeaponAttack weaponAttackModel, string weaponDescription)
        {
            Regex PrimarySecondaryDamageRegex = new Regex(@".*damage.*plus.*damage\.");
            Regex PrimaryOnlyDamageRegex = new Regex(@".*?damage");
            Regex PrimaryWithVersatileRegex = new Regex(@".*?if used with two hands.*");
            string damagePropertyData = weaponDescription.Substring(weaponDescription.IndexOf("Hit: ", StringComparison.Ordinal) + 4);
            string flavorText = "";
            if (PrimarySecondaryDamageRegex.IsMatch(damagePropertyData))
            {
                string[] damagePropertyDataSplit = damagePropertyData.Split(new string[] { " plus " }, StringSplitOptions.None);
                weaponAttackModel.PrimaryDamage = importCommonUtils.ParseDamageProperty(damagePropertyDataSplit[0]);
                weaponAttackModel.SecondaryDamage = importCommonUtils.ParseDamageProperty(damagePropertyDataSplit[1]);


                // Parse out any flavor text
                ParseWeaponAttackFlavorText(weaponAttackModel, damagePropertyData, PrimarySecondaryDamageRegex);
            }
            else
            {
                weaponAttackModel.PrimaryDamage = importCommonUtils.ParseDamageProperty(damagePropertyData);
                weaponAttackModel.SecondaryDamage = null;
                // Parse out any flavor text
                ParseWeaponAttackFlavorText(weaponAttackModel, damagePropertyData, PrimaryOnlyDamageRegex);
            }

            weaponAttackModel.IsMagic = damagePropertyData.Contains("magic");
            weaponAttackModel.IsSilver = damagePropertyData.Contains("silver");
            weaponAttackModel.IsAdamantine = damagePropertyData.Contains("adamantine");
            weaponAttackModel.IsColdForgedIron = damagePropertyData.Contains("cold-forged iron");
            weaponAttackModel.IsVersatile = PrimaryWithVersatileRegex.IsMatch(damagePropertyData);
        }

        private void ParseWeaponAttackFlavorText(WeaponAttack weaponAttackModel, string damagePropertyData, Regex regex)
        {
            // Check for any flavor text
            int regexMatchLength = regex.Match(damagePropertyData).Value.Length;
            Regex PrimaryWithVersatileRegex = new Regex(@".*?if used with two hands.*");
            if (PrimaryWithVersatileRegex.IsMatch(damagePropertyData))
                regexMatchLength = PrimaryWithVersatileRegex.Match(damagePropertyData).Value.Length;
            if (damagePropertyData.Length != regexMatchLength)
            {
                // in the case that the last character is a period, just ignore flavor text
                if (damagePropertyData.Substring(regexMatchLength).Equals(".", StringComparison.Ordinal))
                {
                    weaponAttackModel.OtherTextCheck = false;
                    weaponAttackModel.OtherText = "";
                }
                else
                {
                    weaponAttackModel.OtherTextCheck = true;
                    weaponAttackModel.OtherText = damagePropertyData.Substring(regexMatchLength);
                    // Removes a use case where the above line returns ". <DESCRIPTION HERE>"
                    if (weaponAttackModel.OtherText[0] == '.')
                        weaponAttackModel.OtherText = weaponAttackModel.OtherText.Substring(2);
                }
            }
        }

        private static string[] ParseMultiattackAction(NPCModel npcModel, string standardAction)
        {
            const string delimiter = ".";
            StringBuilder stringBuilder = new StringBuilder();
            string[] standardActionArray;
            Multiattack multiattackModel = new Multiattack();
            standardActionArray = standardAction.Split('.');
            for (int idx = 1; idx < standardActionArray.Length; idx++)
            {
                stringBuilder.Append(standardActionArray[idx].Trim()).Append(delimiter);
            }
            multiattackModel.ActionDescription = stringBuilder.Remove(stringBuilder.Length - delimiter.Length, delimiter.Length).ToString();
            npcModel.NPCActions.Add(multiattackModel);
            return standardActionArray;
        }

        private string GetDescription(Type EnumType, object enumValue)
        {
            var descriptionAttribute = EnumType
                .GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;


            return descriptionAttribute != null
                ? descriptionAttribute.Description
                : enumValue.ToString();
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
            if (string.IsNullOrEmpty(legendaryAction))
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

        #region Damage Resistance Parsing
        /// <summary>
        /// 'Damage Resistances cold, force, necrotic, psychic, thunder from nonmagical weapons'
        /// </summary>
        public void ParseDamageResistances(NPCModel npcModel, string damageResistances)
        {
            if (damageResistances.StartsWith("Damage Resistances", StringComparison.Ordinal))
            {
                npcModel.DamageResistanceModelList = parseDamageTypeStringToList(damageResistances);
                npcModel.SpecialWeaponResistanceModelList = parseSpecialDamageResistanceStringToList(damageResistances);
            }
            else
            {
                // Populate with all options deselected
                npcModel.DamageResistanceModelList = parseDamageTypeStringToList("");
                npcModel.SpecialWeaponResistanceModelList = parseSpecialDamageResistanceStringToList("");
                npcModel.SpecialWeaponResistanceModelList.First().Selected = true;
            }
            npcModel.SpecialWeaponDmgResistanceModelList = new NPCController().GetSelectableActionModelList(typeof(DamageType));
        }
        private List<SelectableActionModel> parseSpecialDamageResistanceStringToList(string damageTypes)
        {
            NPCController npcController = new NPCController();
            List<SelectableActionModel> selectableActionModels = npcController.GetSelectableActionModelList(typeof(WeaponResistance));
            damageTypes = damageTypes.ToLower(CultureInfo.CurrentCulture);
            if (damageTypes.Contains("nonmagical weapons") || damageTypes.Contains("nonmagical attacks"))
            {
                if (damageTypes.Contains("that aren't silvered"))
                    selectableActionModels
                        .First(item => item.ActionName.Equals(WeaponResistance.NonmagicalSilvered.ToString(), StringComparison.Ordinal))
                        .Selected = true;
                else if (damageTypes.Contains("that aren't adamantine"))
                    selectableActionModels
                        .First(item => item.ActionName.Equals(WeaponResistance.NonmagicalAdamantine.ToString(), StringComparison.Ordinal))
                        .Selected = true;
                else if (damageTypes.Contains("that aren't cold-forged iron"))
                    selectableActionModels
                        .First(item => item.ActionName.Equals(WeaponResistance.NonmagicalColdForgedIron.ToString(), StringComparison.Ordinal))
                        .Selected = true;
                else
                    selectableActionModels
                        .First(item => item.ActionName.Equals(WeaponResistance.Nonmagical.ToString(), StringComparison.Ordinal))
                        .Selected = true;
            }
            else
                selectableActionModels
                    .First(item => item.ActionName.Equals(WeaponResistance.NoSpecial.ToString(), StringComparison.Ordinal))
                    .Selected = true;

            return selectableActionModels;
        }

        #endregion

        #region Damage Immunity Parsing
        /// <summary>
        /// 'Damage Immunities acid, force, poison, thunder; slashing from nonmagical weapons that aren't silvered'
        /// </summary>
        public void ParseDamageImmunities(NPCModel npcModel, string damageImmunities)
        {
            if (damageImmunities.StartsWith("Damage Immunities", StringComparison.Ordinal))
            {
                npcModel.DamageImmunityModelList = parseDamageTypeStringToList(damageImmunities);
                npcModel.SpecialWeaponImmunityModelList = parseSpecialDamageImmunityStringToList(damageImmunities);
            }
            else
            {
                // Populate with all options deselected
                npcModel.DamageImmunityModelList = parseDamageTypeStringToList("");
                npcModel.SpecialWeaponImmunityModelList = parseSpecialDamageImmunityStringToList("");
                npcModel.SpecialWeaponImmunityModelList.First().Selected = true;
            }

            npcModel.SpecialWeaponDmgImmunityModelList = new NPCController().GetSelectableActionModelList(typeof(DamageType));
        }

        private List<SelectableActionModel> parseSpecialDamageImmunityStringToList(string damageTypes)
        {
            NPCController npcController = new NPCController();
            List<SelectableActionModel> selectableActionModels = npcController.GetSelectableActionModelList(typeof(WeaponImmunity));
            damageTypes = damageTypes.ToLower(CultureInfo.CurrentCulture);
            if (damageTypes.Contains("nonmagical weapons") || damageTypes.Contains("nonmagical attacks"))
            {
                if (damageTypes.Contains("that aren't silvered"))
                    selectableActionModels
                        .First(item => item.ActionName.Equals(WeaponImmunity.NonmagicalSilvered.ToString(), StringComparison.Ordinal))
                        .Selected = true;
                else if (damageTypes.Contains("that aren't adamantine"))
                    selectableActionModels
                        .First(item => item.ActionName.Equals(WeaponImmunity.NonmagicalAdamantine.ToString(), StringComparison.Ordinal))
                        .Selected = true;
                else if (damageTypes.Contains("that aren't cold-forged iron"))
                    selectableActionModels
                        .First(item => item.ActionName.Equals(WeaponImmunity.NonmagicalColdForgedIron.ToString(), StringComparison.Ordinal))
                        .Selected = true;
                else
                    selectableActionModels
                    .First(item => item.ActionName.Equals(WeaponImmunity.Nonmagical.ToString(), StringComparison.Ordinal))
                    .Selected = true;
            }
            else
                selectableActionModels
                    .First(item => item.ActionName.Equals(WeaponImmunity.NoSpecial.ToString(), StringComparison.Ordinal))
                    .Selected = true;

            return selectableActionModels;
        }
        #endregion

    }
}
