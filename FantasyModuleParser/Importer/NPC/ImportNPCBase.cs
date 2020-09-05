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
        private ImportCommonUtils importCommonUtils = new ImportCommonUtils();
        public abstract NPCModel ImportTextToNPCModel(string importTextContent);

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

            if (npcCharacteristics[2].Contains("("))
            {
                // includes removing the comma character at the end
                npcModel.Tag = npcCharacteristics[2].ToLower().Substring(0, npcCharacteristics[2].Length - 1);
            }

            if (npcModel.Tag != null && npcModel.Tag.Length > 0)
                if (npcCharacteristics.Length > 4)
                    npcModel.Alignment = npcCharacteristics[3] + " " + npcCharacteristics[4];
                else
                    npcModel.Alignment = npcCharacteristics[3];
            else
                if (npcCharacteristics.Length > 3)
                npcModel.Alignment = npcCharacteristics[2] + " " + npcCharacteristics[3];
            else
                npcModel.Alignment = npcCharacteristics[2];
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
            {
                npcModel.HP = hitPoints.Substring(11);
            }
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
                {
                    npcModel.Speed = int.Parse(trimmedSpeedAttribute.Split(' ')[1], CultureInfo.CurrentCulture);
                }
                if (trimmedSpeedAttribute.StartsWith("burrow ", StringComparison.Ordinal))
                {
                    npcModel.Burrow = int.Parse(trimmedSpeedAttribute.Split(' ')[1], CultureInfo.CurrentCulture);
                }
                if (trimmedSpeedAttribute.StartsWith("climb ", StringComparison.Ordinal))
                {
                    npcModel.Climb = int.Parse(trimmedSpeedAttribute.Split(' ')[1], CultureInfo.CurrentCulture);
                }
                if (trimmedSpeedAttribute.StartsWith("fly ", StringComparison.Ordinal))
                {
                    npcModel.Fly = int.Parse(trimmedSpeedAttribute.Split(' ')[1], CultureInfo.CurrentCulture);
                }
                if (trimmedSpeedAttribute.Contains("(hover)"))
                {
                    npcModel.Hover = true;
                }
                if (trimmedSpeedAttribute.StartsWith("swim ", StringComparison.Ordinal))
                {
                    npcModel.Swim = int.Parse(trimmedSpeedAttribute.Split(' ')[1], CultureInfo.CurrentCulture);
                }
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
            {
                npcModel.DamageVulnerabilityModelList = parseDamageTypeStringToList(damageVulnerabilites);
            }
            else
            {
                // Populate with all options deselected
                npcModel.DamageVulnerabilityModelList = parseDamageTypeStringToList("");
            }
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
                SelectableActionModel damageTypeModel = selectableActionModels.FirstOrDefault(item => item.ActionDescription.Equals(damageTypeValueTrimmed));
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
                    {
                        npcModel.Blindsight = int.Parse(visionAttributeArray[arrayIndex + 1], CultureInfo.CurrentCulture);
                    }
                    if (attribute.ToLower(CultureInfo.CurrentCulture).Equals("darkvision", StringComparison.Ordinal))
                    {
                        npcModel.Darkvision = int.Parse(visionAttributeArray[arrayIndex + 1], CultureInfo.CurrentCulture);
                    }
                    if (attribute.ToLower(CultureInfo.CurrentCulture).Equals("tremorsense", StringComparison.Ordinal))
                    {
                        npcModel.Tremorsense = int.Parse(visionAttributeArray[arrayIndex + 1], CultureInfo.CurrentCulture);
                    }
                    if (attribute.ToLower(CultureInfo.CurrentCulture).Equals("truesight", StringComparison.Ordinal))
                    {
                        npcModel.Truesight = int.Parse(visionAttributeArray[arrayIndex + 1], CultureInfo.CurrentCulture);
                    }
                    if (attribute.ToLower(CultureInfo.CurrentCulture).Equals("perception", StringComparison.Ordinal))
                    {
                        npcModel.PassivePerception = int.Parse(visionAttributeArray[arrayIndex + 1], CultureInfo.CurrentCulture);
                    }
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
                string xpString = new string(splitArray[2].Where(c => !Char.IsWhiteSpace(c) && c != '(').ToArray());
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
            for (int idx = 0; idx < 5; idx++)
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
                {
                    weaponAttackModel.ToHit = parseAttributeStringToInt(weaponDescriptionData.Split(' ')[0]);
                }
                if (weaponDescriptionData.Contains("reach"))
                {
                    weaponAttackModel.Reach = parseAttributeStringToInt(weaponDescriptionData.Split(' ')[2]);
                }
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
                {
                    weaponAttackModel.TargetType = TargetType.target;
                }
                if (weaponDescriptionData.Contains("one creature"))
                {
                    weaponAttackModel.TargetType = TargetType.creature;
                }
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
            // This is the versatile weapon check
            //if (PrimaryWithVersatileRegex.IsMatch(damagePropertyData))
            //{
            //    string[] damagePropertyDataSplit = damagePropertyData.Split(new string[] { " plus " }, StringSplitOptions.None);
            //    weaponAttackModel.PrimaryDamage = importCommonUtils.ParseDamageProperty(damagePropertyDataSplit[0]);
            //    weaponAttackModel.SecondaryDamage = importCommonUtils.ParseDamageProperty(damagePropertyDataSplit[1]);
            //    weaponAttackModel.IsVersatile = true;

            //    // Parse out any flavor text
            //    ParseWeaponAttackFlavorText(weaponAttackModel, damagePropertyData, PrimaryWithVersatileRegex);
            //}
            //// Check for a secondary damage type
            //else 
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
                //weaponAttackModel.IsVersatile = false;
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
            {
                regexMatchLength = PrimaryWithVersatileRegex.Match(damagePropertyData).Value.Length;
            }
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
            if(damageTypes.Contains("nonmagical weapons") || damageTypes.Contains("nonmagical attacks"))
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
