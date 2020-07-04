﻿using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Skills;
using FantasyModuleParser.NPC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace FantasyModuleParser.NPC.ViewModels
{

    public class PreviewNPCViewModel : ViewModelBase
    {
        public NPCModel NPCModel { get; set; }
        private NPCController npcController;
        private string Resistance;
        private string Immunity;

        public string SpeedDescription { get; set; }
        public string SkillsDescription { get; set; }
        public string StrengthAttribute { get; set; }
        public string DexterityAttribute { get; set; }
        public string ConstitutionAttribute { get; set; }
        public string IntelligenceAttribute { get; set; }
        public string WisdomAttribute { get; set; }
        public string CharismaAttribute { get; set; }
        public string SavingThrows { get; set; }
        public string Senses { get; set; }
        public string DamageVulnerabilities { get; set; }
        public string DamageResistances { get; set; }
        public string DamageImmunities { get; set; }
        public string ConditionImmunities { get; set; }
        public string Languages { get; set; }
        public string Challenge { get; set; }
        public string TraitName1 { get; set; }
        public string TraitDesc1 { get; set; }
        public string TraitName2 { get; set; }
        public string TraitDesc2 { get; set; }
        public string TraitName3 { get; set; }
        public string TraitDesc3 { get; set; }
        public string TraitName4 { get; set; }
        public string TraitDesc4 { get; set; }
        public string TraitName5 { get; set; }
        public string TraitDesc5 { get; set; }
        public string TraitName6 { get; set; }
        public string TraitDesc6 { get; set; }
        public string TraitName7 { get; set; }
        public string TraitDesc7 { get; set; }
        public string TraitName8 { get; set; }
        public string TraitDesc8 { get; set; }
        public string TraitName9 { get; set; }
        public string TraitDesc9 { get; set; }
        public string TraitName10 { get; set; }
        public string TraitDesc10 { get; set; }
        public string InnateSpellcastingLabel { get; set; }
        public string InnateSpellcasting { get; set; }
        public string SpellcastingLabel { get; set; }
        public string Spellcasting { get; set; }
        public string SpellcastingCantripsLabel { get; set; }
        public string SpellcastingCantrips { get; set; }
        public string SpellcastingFirstLabel { get; set; }
        public string SpellcastingFirst { get; set; }
        public string SpellcastingSecondLabel { get; set; }
        public string SpellcastingSecond { get; set; }
        public string SpellcastingThirdLabel { get; set; }
        public string SpellcastingThird { get; set; }
        public string SpellcastingFourthLabel { get; set; }
        public string SpellcastingFourth { get; set; }
        public string SpellcastingFifthLabel { get; set; }
        public string SpellcastingFifth { get; set; }
        public string SpellcastingSixthLabel { get; set; }
        public string SpellcastingSixth { get; set; }
        public string SpellcastingSeventhLabel { get; set; }
        public string SpellcastingSeventh { get; set; }
        public string SpellcastingEighthLabel { get; set; }
        public string SpellcastingEighth { get; set; }
        public string SpellcastingNinthLabel { get; set; }
        public string SpellcastingNinth { get; set; }
        public string SpellcastingMarkedSpells { get; set; }
        public string WeaponName1 { get; set; }

        public PreviewNPCViewModel()
        {
            npcController = new NPCController();
            NPCModel = npcController.GetNPCModel();
            initalizeViewModel();
        }

        public PreviewNPCViewModel(NPCModel nPCModel)
        {
            NPCModel = nPCModel;
            initalizeViewModel();
        }

        private void initalizeViewModel()
        {
            SpeedDescription = UpdateSpeedDescription();
            SkillsDescription = UpdateSkillsDescription();
            StrengthAttribute = UpdateStrengthAttribute();
            DexterityAttribute = UpdateDexterityAttribute();
            ConstitutionAttribute = UpdateConstitutionAttribute();
            IntelligenceAttribute = UpdateIntelligenceAttribute();
            WisdomAttribute = UpdateWisdomAttribute();
            CharismaAttribute = UpdateCharismaAttribute();
            SavingThrows = UpdateSavingThrows();
            Senses = UpdateSenses();
            DamageVulnerabilities = UpdateDamageVulnerabilities();
            DamageResistances = UpdateDamageResistances();
            DamageImmunities = UpdateDamageImmunities();
            ConditionImmunities = UpdateConditionImmunities();
            Languages = UpdateLanguages();
            Challenge = UpdateChallengeRating();
        }

        public PreviewNPCViewModel(NPCModel nPCModel)
        {
            NPCModel = nPCModel;
            SpeedDescription = UpdateSpeedDescription();
            SkillsDescription = UpdateSkillsDescription();
            TraitName1 = UpdateTraitName1();
            TraitDesc1 = UpdateTraitDescription1();
            TraitName2 = UpdateTraitName2();
            TraitDesc2 = UpdateTraitDescription2();
            TraitName3 = UpdateTraitName3();
            TraitDesc3 = UpdateTraitDescription3();
            TraitName4 = UpdateTraitName4();
            TraitDesc4 = UpdateTraitDescription4();
            TraitName5 = UpdateTraitName5();
            TraitDesc5 = UpdateTraitDescription5();
            TraitName6 = UpdateTraitName6();
            TraitDesc6 = UpdateTraitDescription6();
            TraitName7 = UpdateTraitName7();
            TraitDesc7 = UpdateTraitDescription7();
            TraitName8 = UpdateTraitName8();
            TraitDesc8 = UpdateTraitDescription8();
            TraitName9 = UpdateTraitName9();
            TraitDesc9 = UpdateTraitDescription9();
            TraitName10 = UpdateTraitName10();
            TraitDesc10 = UpdateTraitDescription10();
            InnateSpellcastingLabel = UpdateInnateSpellcastingLabel();
            InnateSpellcasting = UpdateInnateSpellcasting();
            SpellcastingLabel = UpdateSpellcastingLabel();
            Spellcasting = UpdateSpellcasting();
            SpellcastingCantripsLabel = UpdateSpellcastingCantripsLabel();
            SpellcastingCantrips = UpdateSpellcastingCantrips();
            SpellcastingFirstLabel = UpdateSpellcastingFirstLabel();
            SpellcastingFirst = UpdateSpellcastingFirst();
            SpellcastingSecondLabel = UpdateSpellcastingSecondLabel();
            SpellcastingSecond = UpdateSpellcastingSecond();
            SpellcastingThirdLabel = UpdateSpellcastingThirdLabel();
            SpellcastingThird = UpdateSpellcastingThird();
            SpellcastingFourthLabel = UpdateSpellcastingFourthLabel();
            SpellcastingFourth = UpdateSpellcastingFourth();
            SpellcastingFifthLabel = UpdateSpellcastingFifthLabel();
            SpellcastingFifth = UpdateSpellcastingFifth();
            SpellcastingSixthLabel = UpdateSpellcastingSixthLabel();
            SpellcastingSixth = UpdateSpellcastingSixth();
            SpellcastingSeventhLabel = UpdateSpellcastingSeventhLabel();
            SpellcastingSeventh = UpdateSpellcastingSeventh();
            SpellcastingEighthLabel = UpdateSpellcastingEighthLabel();
            SpellcastingEighth = UpdateSpellcastingEighth();
            SpellcastingNinthLabel = UpdateSpellcastingNinthLabel();
            SpellcastingNinth = UpdateSpellcastingNinth();
            SpellcastingMarkedSpells = UpdateSpellcastingMarkedSpells();
            WeaponName1 = UpdateWeaponName1();
        }

        #region UpdateAbilityScores
        public string UpdateStrengthAttribute()
        {
            int num;
            StringBuilder stringBuilder = new StringBuilder();

            num = -5 + (NPCModel.AttributeStr / 2);

            if (NPCModel.AttributeStr >= 10)
                stringBuilder.Append(NPCModel.AttributeStr + " (+" + num + ")");
            else
                stringBuilder.Append(NPCModel.AttributeStr + " (" + num + ")");

            return stringBuilder.ToString();
        }
        public string UpdateDexterityAttribute()
        {
            int num;
            StringBuilder stringBuilder = new StringBuilder();

            num = -5 + (NPCModel.AttributeDex / 2);

            if (NPCModel.AttributeDex >= 10)
                stringBuilder.Append(NPCModel.AttributeDex + " (+" + num + ")");
            else
                stringBuilder.Append(NPCModel.AttributeDex + " (" + num + ")");

            return stringBuilder.ToString();
        }
        public string UpdateConstitutionAttribute()
        {
            int num;
            StringBuilder stringBuilder = new StringBuilder();

            num = -5 + (NPCModel.AttributeCon / 2);

            if (NPCModel.AttributeCon >= 10)
                stringBuilder.Append(NPCModel.AttributeCon + " (+" + num + ")");
            else
                stringBuilder.Append(NPCModel.AttributeCon + " (" + num + ")");

            return stringBuilder.ToString();
        }
        public string UpdateIntelligenceAttribute()
        {
            int num;
            StringBuilder stringBuilder = new StringBuilder();

            num = -5 + (NPCModel.AttributeInt / 2);

            if (NPCModel.AttributeInt >= 10)
                stringBuilder.Append(NPCModel.AttributeInt + " (+" + num + ")");
            else
                stringBuilder.Append(NPCModel.AttributeInt + " (" + num + ")");

            return stringBuilder.ToString();
        }
        public string UpdateWisdomAttribute()
        {
            int num;
            StringBuilder stringBuilder = new StringBuilder();

            num = -5 + (NPCModel.AttributeWis / 2);

            if (NPCModel.AttributeWis >= 10)
                stringBuilder.Append(NPCModel.AttributeWis + " (+" + num + ")");
            else
                stringBuilder.Append(NPCModel.AttributeWis + " (" + num + ")");

            return stringBuilder.ToString();
        }
        public string UpdateCharismaAttribute()
        {
            int num;
            StringBuilder stringBuilder = new StringBuilder();

            num = -5 + (NPCModel.AttributeCha / 2);

            if (NPCModel.AttributeCha >= 10)
                stringBuilder.Append(NPCModel.AttributeCha + " (+" + num + ")");
            else
                stringBuilder.Append(NPCModel.AttributeCha + " (" + num + ")");

            return stringBuilder.ToString();
        }
        #endregion
        #region UpdateSpeed
        public string UpdateSpeedDescription()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (NPCModel.Speed == 0)
                stringBuilder.Append("0 ft., ");
            else
                stringBuilder.Append(AppendSpeedAttribute("", NPCModel.Speed, false));
                stringBuilder.Append(AppendSpeedAttribute("climb", NPCModel.Climb, false));
                stringBuilder.Append(AppendSpeedAttribute("fly", NPCModel.Fly, NPCModel.Hover));
                stringBuilder.Append(AppendSpeedAttribute("burrow", NPCModel.Burrow, false));
                stringBuilder.Append(AppendSpeedAttribute("swim", NPCModel.Swim, false));

            if (stringBuilder.Length >= 2)
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.ToString().Trim();
        }

        private string AppendSpeedAttribute(string name, int value, bool hover)
        {
            string delimiter = ", ";
            if (value != 0 && hover == false)
                return name + " " + value + " ft." + delimiter;
            else if (value != 0 && hover == true)
                return name + " " + value + " ft." + " (hover)" + delimiter;
            return "";
        }
        #endregion
        #region UpdateSkills
        public Visibility ShowSkills
        {
            get
            {
                if (SkillsDescription.Length > 0)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        private string UpdateSkillsDescription()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(appendSkill("Acrobatics", NPCModel.Acrobatics));
            stringBuilder.Append(appendSkill("Animal Handling", NPCModel.AnimalHandling));
            stringBuilder.Append(appendSkill("Arcana", NPCModel.Arcana));
            stringBuilder.Append(appendSkill("Athletics", NPCModel.Athletics));
            stringBuilder.Append(appendSkill("Deception", NPCModel.Deception));
            stringBuilder.Append(appendSkill("History", NPCModel.History));
            stringBuilder.Append(appendSkill("Insight", NPCModel.Insight));
            stringBuilder.Append(appendSkill("Intimidation", NPCModel.Intimidation));
            stringBuilder.Append(appendSkill("Investigation", NPCModel.Investigation));
            stringBuilder.Append(appendSkill("Medicine", NPCModel.Medicine));
            stringBuilder.Append(appendSkill("Nature", NPCModel.Nature));
            stringBuilder.Append(appendSkill("Perception", NPCModel.Perception));
            stringBuilder.Append(appendSkill("Performance", NPCModel.Performance));
            stringBuilder.Append(appendSkill("Persuasion", NPCModel.Persuasion));
            stringBuilder.Append(appendSkill("Religion", NPCModel.Religion));
            stringBuilder.Append(appendSkill("Sleight of Hand", NPCModel.SleightOfHand));
            stringBuilder.Append(appendSkill("Stealth", NPCModel.Stealth));
            stringBuilder.Append(appendSkill("Survival", NPCModel.Survival));
            if (stringBuilder.Length >= 2)
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.ToString().Trim();
        } 
        private string appendSkill(string skillName, int skillValue)
        {
            string delimiter = ", ";
            if (skillValue != 0)
                return skillName + ((skillValue < 0) ? " " : " +") + skillValue + delimiter;
            return "";
        }
        #endregion
        #region UpdateSavingThrows
        public Visibility ShowSavingThrows
        {
            get
            {
                if (SavingThrows.Length > 0)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        private string UpdateSavingThrows()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(appendSavingThrowStr("Str", NPCModel.SavingThrowStr));
            stringBuilder.Append(appendSavingThrowDex("Dex", NPCModel.SavingThrowDex));
            stringBuilder.Append(appendSavingThrowCon("Con", NPCModel.SavingThrowCon));
            stringBuilder.Append(appendSavingThrowInt("Int", NPCModel.SavingThrowInt));
            stringBuilder.Append(appendSavingThrowWis("Wis", NPCModel.SavingThrowWis));
            stringBuilder.Append(appendSavingThrowCha("Cha", NPCModel.SavingThrowCha));
            if (stringBuilder.Length >= 2)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }
            return stringBuilder.ToString();
        }
        private string appendSavingThrowStr(string savingThrowName, int savingThrowValue)
        {
            string delimiter = ", ";
            if (NPCModel.SavingThrowStrBool == true && savingThrowValue == 0)
            {
                return savingThrowName + " +" + savingThrowValue + delimiter;
            }
            else if (NPCModel.SavingThrowStrBool == false && savingThrowValue != 0)
            {
                return savingThrowName + ((savingThrowValue < 0) ? " " : " +") + savingThrowValue + delimiter;
            }
            return "";
        }
        private string appendSavingThrowDex(string savingThrowName, int savingThrowValue)
        {
            string delimiter = ", ";
            if (NPCModel.SavingThrowDexBool == true && savingThrowValue == 0)
            {
                return savingThrowName + " +" + savingThrowValue + delimiter;
            }
            else if (NPCModel.SavingThrowDexBool == false && savingThrowValue != 0)
            {
                return savingThrowName + ((savingThrowValue < 0) ? " " : " +") + savingThrowValue + delimiter;
            }
            return "";
        }
        private string appendSavingThrowCon(string savingThrowName, int savingThrowValue)
        {
            string delimiter = ", ";
            if (NPCModel.SavingThrowConBool == true && savingThrowValue == 0)
            {
                return savingThrowName + " +" + savingThrowValue + delimiter;
            }
            else if (NPCModel.SavingThrowConBool == false && savingThrowValue != 0)
            {
                return savingThrowName + ((savingThrowValue < 0) ? " " : " +") + savingThrowValue + delimiter;
            }
            return "";
        }
        private string appendSavingThrowInt(string savingThrowName, int savingThrowValue)
        {
            string delimiter = ", ";
            if (NPCModel.SavingThrowIntBool == true && savingThrowValue == 0)
            {
                return savingThrowName + " +" + savingThrowValue + delimiter;
            }
            else if (NPCModel.SavingThrowIntBool == false && savingThrowValue != 0)
            {
                return savingThrowName + ((savingThrowValue < 0) ? " " : " +") + savingThrowValue + delimiter;
            }
            return "";
        }
        private string appendSavingThrowWis(string savingThrowName, int savingThrowValue)
        {
            string delimiter = ", ";
            if (NPCModel.SavingThrowWisBool == true && savingThrowValue == 0)
            {
                return savingThrowName + " +" + savingThrowValue + delimiter;
            }
            else if (NPCModel.SavingThrowWisBool == false && savingThrowValue != 0)
            {
                return savingThrowName + ((savingThrowValue < 0) ? " " : " +") + savingThrowValue + delimiter;
            }
            return "";
        }
        private string appendSavingThrowCha(string savingThrowName, int savingThrowValue)
        {
            string delimiter = ", ";
            if (NPCModel.SavingThrowChaBool == true && savingThrowValue == 0)
            {
                return savingThrowName + " +" + savingThrowValue + delimiter;
            }
            else if (NPCModel.SavingThrowChaBool == false && savingThrowValue != 0)
            {
                return savingThrowName + ((savingThrowValue < 0) ? " " : " +") + savingThrowValue + delimiter;
            }
            return "";
        }
        #endregion
        #region UpdateDamageVulnerabilities
        public Visibility ShowDamageVulnerabilities
        {
            get
            {
                if (DamageVulnerabilities.Length > 0)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        private string UpdateDamageVulnerabilities()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (SelectableActionModel selectableActionModel in NPCModel.DamageVulnerabilityModelList)
            {
                if (selectableActionModel.Selected == true)
                    stringBuilder.Append(selectableActionModel.ActionName).Append(", ");
            }
            if (stringBuilder.Length >= 2)
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.ToString().Trim();
        }
        #endregion
        #region UpdateDamageResistances
        public Visibility ShowDamageResistances
        {
            get
            {
                if (DamageResistances.Length > 0)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        private string UpdateDamageResistances()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (SelectableActionModel selectableActionModel in NPCModel.DamageResistanceModelList)
            {
                if (selectableActionModel.Selected == true)
                    stringBuilder.Append(selectableActionModel.ActionDescription).Append(", ");
            }
            if (stringBuilder.Length >= 2)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }
            stringBuilder.Append("; ");
            if (stringBuilder.Length == 2)
            {
                stringBuilder.Remove(0, 2);
            }
            foreach (SelectableActionModel selectableActionModel in NPCModel.SpecialWeaponResistanceModelList)
            {
                if (selectableActionModel.Selected == true && selectableActionModel.ActionName != "NoSpecial")
                {
                    if (selectableActionModel.ActionName == "Nonmagical")
                    {
                        Resistance = " from nonmagical attacks";
                    }
                    else if (selectableActionModel.ActionName == "NonmagicalSilvered")
                    {
                        Resistance = " from nonmagical attacks that aren't silvered";
                    }
                    else if (selectableActionModel.ActionName == "NonmagicalAdamantine")
                    {
                        Resistance = " from nonmagical attacks that aren't adamantine";
                    }
                    else if (selectableActionModel.ActionName == "NonmagicalColdForgedIron")
                    {
                        Resistance = " from nonmagical attacks that aren't cold-forged iron";
                    }
                    else if (selectableActionModel.ActionName == "Magical")
                    {
                        Resistance = " from magic weapons";
                    }
                    foreach (SelectableActionModel selectableActionModelDmg in NPCModel.SpecialWeaponDmgResistanceModelList)
                    {
                        if (selectableActionModelDmg.Selected == true)
                            stringBuilder.Append(selectableActionModelDmg.ActionDescription).Append(", ");
                    }
                    if (stringBuilder.Length >= 2)
                    {
                        stringBuilder.Remove(stringBuilder.Length - 2, 2);
                    }
                    stringBuilder.Append(Resistance);    
                }
            }
            return stringBuilder.ToString().Trim();
        }
        #endregion
        #region UpdateDamageImmunities
        public Visibility ShowDamageImmunities
        {
            get
            {
                if (DamageImmunities.Length > 0)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        private string UpdateDamageImmunities()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (SelectableActionModel selectableActionModel in NPCModel.DamageImmunityModelList)
            {
                if (selectableActionModel.Selected == true)
                    stringBuilder.Append(selectableActionModel.ActionDescription).Append(", ");
            }
            if (stringBuilder.Length >= 2)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }
            stringBuilder.Append("; ");
            if (stringBuilder.Length == 2)
            {
                stringBuilder.Remove(0, 2);
            }
            foreach (SelectableActionModel selectableActionModel in NPCModel.SpecialWeaponImmunityModelList)
            {
                if (selectableActionModel.Selected == true && selectableActionModel.ActionName != "NoSpecial")
                {
                    if (selectableActionModel.ActionName == "Nonmagical")
                    {
                        Immunity = " from nonmagical attacks";
                    }
                    else if (selectableActionModel.ActionName == "NonmagicalSilvered")
                    {
                        Immunity = " from nonmagical attacks that aren't silvered";
                    }
                    else if (selectableActionModel.ActionName == "NonmagicalAdamantine")
                    {
                        Immunity = " from nonmagical attacks that aren't adamantine";
                    }
                    else if (selectableActionModel.ActionName == "NonmagicalColdForgedIron")
                    {
                        Immunity = " from nonmagical attacks that aren't cold-forged iron";
                    }
                    foreach (SelectableActionModel selectableActionModelDmg in NPCModel.SpecialWeaponDmgImmunityModelList)
                    {
                        if (selectableActionModelDmg.Selected == true)
                            stringBuilder.Append(selectableActionModelDmg.ActionDescription).Append(", ");
                    }
                    if (stringBuilder.Length >= 2)
                    {
                        stringBuilder.Remove(stringBuilder.Length - 2, 2);
                    }
                    stringBuilder.Append(Immunity);
                }
            }
            return stringBuilder.ToString().Trim();
        }
        #endregion
        #region UpdateConditionImmunities
        public Visibility ShowConditionImmunities
        {
            get
            {
                if (ConditionImmunities.Length > 0)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        private string UpdateConditionImmunities()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (SelectableActionModel selectableActionModel in NPCModel.ConditionImmunityModelList)
            {
                if (selectableActionModel.Selected == true)
                    stringBuilder.Append(selectableActionModel.ActionDescription).Append(", ");
            }
            if (NPCModel.ConditionOther == true)
            {
                stringBuilder.Append(NPCModel.ConditionOtherText + ", ");
            }
            if (stringBuilder.Length >= 2)
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.ToString().Trim();
        }
        #endregion
        #region UpdateLanguages
        public Visibility ShowLanguages
        {
            get
            {
                if (Languages.Length > 0)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        private string UpdateLanguages()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (LanguageModel languageModel in NPCModel.StandardLanguages)
            {
                if (languageModel.Selected == true)
                    stringBuilder.Append(languageModel.Language).Append(", ");
            }
            foreach (LanguageModel languageModel in NPCModel.ExoticLanguages)
            {
                if (languageModel.Selected == true)
                    stringBuilder.Append(languageModel.Language).Append(", ");
            }
            foreach (LanguageModel languageModel in NPCModel.MonstrousLanguages)
            {
                if (languageModel.Selected == true)
                    stringBuilder.Append(languageModel.Language).Append(", ");
            }
            if (stringBuilder.Length >= 2)
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.ToString().Trim();
        }
        #endregion
        #region UpdateSenses
        private string UpdateSenses()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(appendSenses("darkvision ", NPCModel.Darkvision, " ft."));
            stringBuilder.Append(appendSenses("blindsight ", NPCModel.Blindsight, " ft."));
            stringBuilder.Append(appendSenses("tremorsense ", NPCModel.Tremorsense, " ft."));
            stringBuilder.Append(appendSenses("truesight ", NPCModel.Truesight, " ft."));
            stringBuilder.Append(appendSenses("passive perception ", NPCModel.PassivePerception, ""));
            if (stringBuilder.Length >= 2)
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.ToString();
        }
        private string appendSenses(string senseName, int senseValue, string senseRange)
        {
            string delimiter = ", ";
            if (senseValue != 0 && NPCModel.BlindBeyond == false)
                return senseName + senseValue + senseRange + delimiter;
            if (senseValue != 0 && NPCModel.BlindBeyond == true)
                return senseName + senseValue + senseRange + " (blind beyond this radius)" + delimiter;
            return "";
        }
        #endregion
        #region UpdateChallengeRating
        private string UpdateChallengeRating()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(NPCModel.ChallengeRating + " (" + NPCModel.XP + " XP)");
            return stringBuilder.ToString();
        }
        #endregion
        #region UpdateTrait1
        public Visibility ShowTraits1
        {
            get
            {
                if (NPCModel.Traits1 != null)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        private string UpdateTraitName1()
        {
            if (NPCModel.Traits1 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.Traits1 + ".");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateTraitDescription1()
        {
            if (NPCModel.TraitsDesc1 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.TraitsDesc1);
                return stringBuilder.ToString();
            }
            return "";
        }
        #endregion
        #region UpdateTrait2
        public Visibility ShowTraits2
        {
            get
            {
                if (NPCModel.Traits2 != null)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        private string UpdateTraitName2()
        {
            if (NPCModel.Traits2 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.Traits2 + ".");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateTraitDescription2()
        {
            if (NPCModel.TraitsDesc2 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.TraitsDesc2);
                return stringBuilder.ToString();
            }
            return "";
        }
        #endregion
        #region UpdateTrait3
        public Visibility ShowTraits3
        {
            get
            {
                if (NPCModel.Traits3 != null)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        private string UpdateTraitName3()
        {
            if (NPCModel.Traits3 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.Traits3 + ".");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateTraitDescription3()
        {
            if (NPCModel.TraitsDesc3 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.TraitsDesc3);
                return stringBuilder.ToString();
            }
            return "";
        }
        #endregion
        #region UpdateTrait4
        public Visibility ShowTraits4
        {
            get
            {
                if (NPCModel.Traits4 != null)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        private string UpdateTraitName4()
        {
            if (NPCModel.Traits4 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.Traits4 + ".");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateTraitDescription4()
        {
            if (NPCModel.TraitsDesc4 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.TraitsDesc4);
                return stringBuilder.ToString();
            }
            return "";
        }
        #endregion
        #region UpdateTrait5
        public Visibility ShowTraits5
        {
            get
            {
                if (NPCModel.Traits5 != null)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        private string UpdateTraitName5()
        {
            if (NPCModel.Traits5 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.Traits5 + ".");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateTraitDescription5()
        {
            if (NPCModel.TraitsDesc5 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.TraitsDesc5);
                return stringBuilder.ToString();
            }
            return "";
        }
        #endregion
        #region UpdateTrait6
        public Visibility ShowTraits6
        {
            get
            {
                if (NPCModel.Traits6 != null)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        private string UpdateTraitName6()
        {
            if (NPCModel.Traits6 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.Traits6 + ".");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateTraitDescription6()
        {
            if (NPCModel.TraitsDesc6 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.TraitsDesc6);
                return stringBuilder.ToString();
            }
            return "";
        }
        #endregion
        #region UpdateTrait7
        public Visibility ShowTraits7
        {
            get
            {
                if (NPCModel.Traits7 != null)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        private string UpdateTraitName7()
        {
            if (NPCModel.Traits7 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.Traits7 + ".");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateTraitDescription7()
        {
            if (NPCModel.TraitsDesc7 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.TraitsDesc7);
                return stringBuilder.ToString();
            }
            return "";
        }
        #endregion
        #region UpdateTrait8
        public Visibility ShowTraits8
        {
            get
            {
                if (NPCModel.Traits8 != null)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        private string UpdateTraitName8()
        {
            if (NPCModel.Traits8 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.Traits8 + ".");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateTraitDescription8()
        {
            if (NPCModel.TraitsDesc8 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.TraitsDesc8);
                return stringBuilder.ToString();
            }
            return "";
        }
        #endregion
        #region UpdateTrait9
        public Visibility ShowTraits9
        {
            get
            {
                if (NPCModel.Traits9 != null)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        private string UpdateTraitName9()
        {
            if (NPCModel.Traits9 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.Traits9 + ".");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateTraitDescription9()
        {
            if (NPCModel.TraitsDesc9 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.TraitsDesc9);
                return stringBuilder.ToString();
            }
            return "";
        }
        #endregion
        #region UpdateTrait10
        public Visibility ShowTraits10
        {
            get
            {
                if (NPCModel.Traits10 != null)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        private string UpdateTraitName10()
        {
            if (NPCModel.Traits10 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.Traits10 + ".");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateTraitDescription10()
        {
            if (NPCModel.TraitsDesc10 != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(NPCModel.TraitsDesc10);
                return stringBuilder.ToString();
            }
            return "";
        }
        #endregion
        #region UpdateInnateSpellcasting
        public Visibility ShowInnateSpellcasting
        {
            get
            {
                if (NPCModel.InnateSpellcastingSection == true)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility ShowInnateAtWill
        {
            get 
            {
                if (NPCModel.InnateAtWill != null)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility ShowInnateFivePerDay
        {
            get
            {
                if (NPCModel.FivePerDay != null)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility ShowInnateFourPerDay
        {
            get
            {
                if (NPCModel.FourPerDay != null)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility ShowInnateThreePerDay
        {
            get
            {
                if (NPCModel.ThreePerDay != null)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility ShowInnateTwoPerDay
        {
            get
            {
                if (NPCModel.TwoPerDay != null)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility ShowInnateOnePerDay
        {
            get
            {
                if (NPCModel.OnePerDay != null)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        private string UpdateInnateSpellcastingLabel()
        {
            if (NPCModel.InnateSpellcastingSection == true)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Innate Spellcasting");
                if (NPCModel.Psionics == true)
                {
                    stringBuilder.Append(" (Psionics)");
                }
                stringBuilder.Append(".");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateInnateSpellcasting()
        {
            if (NPCModel.InnateSpellcastingSection == true)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("The " + NPCModel.NPCName.ToLower() + "'s spellcasting ability is " + NPCModel.InnateSpellcastingAbility + " ("); 
                if (NPCModel.InnateSpellSaveDCCheck == true)
                    stringBuilder.Append("spell save DC " + NPCModel.InnateSpellSaveDC);
                if (NPCModel.InnateSpellSaveDCCheck == true && NPCModel.InnateSpellHitBonusCheck == true)
                    stringBuilder.Append(", ");
                if (NPCModel.InnateSpellHitBonusCheck == true && NPCModel.InnateSpellHitBonus >= 0)
                    stringBuilder.Append("+" + NPCModel.InnateSpellHitBonus + " to hit with spell attacks");
                else if (NPCModel.InnateSpellHitBonusCheck == true && NPCModel.InnateSpellHitBonus < 0)
                    stringBuilder.Append(NPCModel.InnateSpellHitBonus + " to hit with spell attacks");
                stringBuilder.Append("). The " + NPCModel.NPCName.ToLower() + " can innately cast the following spells, " + NPCModel.ComponentText + ":");
                return stringBuilder.ToString();
            }
            return "";
        }
        #endregion
        #region UpdateSpellcasting
        public Visibility ShowSpellcasting
        {
            get
            {
                if (NPCModel.SpellcastingSection == true)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility ShowCantrips
        {
            get
            {
                if (NPCModel.CantripSpellList != null)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }
        public Visibility ShowFirst
        {
            get
            {
                if (NPCModel.FirstLevelSpellList != null)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }
        public Visibility ShowSecond
        {
            get
            {
                if (NPCModel.SecondLevelSpellList != null)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }
        public Visibility ShowThird
        {
            get
            {
                if (NPCModel.ThirdLevelSpellList != null)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }
        public Visibility ShowFourth
        {
            get
            {
                if (NPCModel.FourthLevelSpellList != null)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }
        public Visibility ShowFifth
        {
            get
            {
                if (NPCModel.FifthLevelSpellList != null)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }
        public Visibility ShowSixth
        {
            get
            {
                if (NPCModel.SixthLevelSpellList != null)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }
        public Visibility ShowSeventh
        {
            get
            {
                if (NPCModel.SeventhLevelSpellList != null)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }
        public Visibility ShowEighth
        {
            get
            {
                if (NPCModel.EighthLevelSpellList != null)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }
        public Visibility ShowNinth
        {
            get
            {
                if (NPCModel.NinthLevelSpellList != null)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }
        public Visibility ShowMarked
        {
            get
            {
                if (NPCModel.MarkedSpells != null)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }
        private string UpdateSpellcastingLabel()
        {
            if (NPCModel.SpellcastingSection == true)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Spellcasting.");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateSpellcastingCantripsLabel()
        {
            if (NPCModel.CantripSpellList != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Cantrips (" + NPCModel.CantripSpells.ToLower() + "):");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateSpellcastingCantrips()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(NPCModel.CantripSpellList);
            return stringBuilder.ToString();
        }
        private string UpdateSpellcastingFirstLabel()
        {
            if (NPCModel.FirstLevelSpellList != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("1st level (" + NPCModel.FirstLevelSpells.ToLower() + "):");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateSpellcastingFirst()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(NPCModel.FirstLevelSpellList);
            return stringBuilder.ToString();
        }
        private string UpdateSpellcastingSecondLabel()
        {
            if (NPCModel.SecondLevelSpellList != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("2nd level (" + NPCModel.SecondLevelSpells.ToLower() + "):");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateSpellcastingSecond()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(NPCModel.SecondLevelSpellList);
            return stringBuilder.ToString();
        }
        private string UpdateSpellcastingThirdLabel()
        {
            if (NPCModel.ThirdLevelSpellList != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("3rd level (" + NPCModel.ThirdLevelSpells.ToLower() + "):");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateSpellcastingThird()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(NPCModel.ThirdLevelSpellList);
            return stringBuilder.ToString();
        }
        private string UpdateSpellcastingFourthLabel()
        {
            if (NPCModel.FourthLevelSpellList != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("4th level (" + NPCModel.FourthLevelSpells.ToLower() + "):");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateSpellcastingFourth()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(NPCModel.FourthLevelSpellList);
            return stringBuilder.ToString();
        }
        private string UpdateSpellcastingFifthLabel()
        {
            if (NPCModel.FifthLevelSpellList != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("5th level (" + NPCModel.FifthLevelSpells.ToLower() + "):");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateSpellcastingFifth()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(NPCModel.FifthLevelSpellList);
            return stringBuilder.ToString();
        }
        private string UpdateSpellcastingSixthLabel()
        {
            if (NPCModel.SixthLevelSpellList != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("6th level (" + NPCModel.SixthLevelSpells.ToLower() + "):");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateSpellcastingSixth()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(NPCModel.SixthLevelSpellList);
            return stringBuilder.ToString();
        }
        private string UpdateSpellcastingSeventhLabel()
        {
            if (NPCModel.SeventhLevelSpellList != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("7th level (" + NPCModel.SeventhLevelSpells.ToLower() + "):");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateSpellcastingSeventh()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(NPCModel.SeventhLevelSpellList);
            return stringBuilder.ToString();
        }
        private string UpdateSpellcastingEighthLabel()
        {
            if (NPCModel.EighthLevelSpellList != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("8th level (" + NPCModel.EighthLevelSpells.ToLower() + "):");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateSpellcastingEighth()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(NPCModel.EighthLevelSpellList);
            return stringBuilder.ToString();
        }
        private string UpdateSpellcastingNinthLabel()
        {
            if (NPCModel.NinthLevelSpellList != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("9th level (" + NPCModel.NinthLevelSpells.ToLower() + "):");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateSpellcastingNinth()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(NPCModel.NinthLevelSpellList);
            return stringBuilder.ToString();
        }
        private string UpdateSpellcastingMarkedSpells()
        {
            if (NPCModel.MarkedSpellsCheck == true)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("* " + NPCModel.MarkedSpells);
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateSpellcasting()
        {
            if (NPCModel.SpellcastingSection == true)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("The " + NPCModel.NPCName.ToLower() + " is a " + NPCModel.SpellcastingCasterLevel + " level spellcaster. Its spellcasting ability is ");
                stringBuilder.Append(NPCModel.SCSpellcastingAbility);
                if (NPCModel.SpellcastingSpellSaveDCCheck == true)
                {
                    stringBuilder.Append(" (spell save DC " + NPCModel.SpellcastingSpellSaveDC);
                    if (NPCModel.SpellcastingSpellHitBonusCheck == true)
                    {
                        stringBuilder.Append(", ");
                        if (NPCModel.SpellcastingSpellHitBonus >= 0)
                        {
                            stringBuilder.Append("+" + NPCModel.SpellcastingSpellHitBonus);
                        }
                        else
                        {
                            stringBuilder.Append(NPCModel.SpellcastingSpellHitBonus);
                        }
                        stringBuilder.Append(" to hit with spell attacks).");
                    }
                    else
                    {
                        stringBuilder.Append(").");
                    }
                }
                else if (NPCModel.SpellcastingSpellSaveDCCheck == false && NPCModel.SpellcastingSpellHitBonusCheck == true)
                {
                    if (NPCModel.SpellcastingSpellHitBonus >= 0)
                    {
                        stringBuilder.Append(" (+" + NPCModel.SpellcastingSpellHitBonus);
                    }
                    else
                    {
                        stringBuilder.Append(" (" + NPCModel.SpellcastingSpellHitBonus);
                    }
                    stringBuilder.Append(" to hit with spell attacks).");
                }
                if (NPCModel.FlavorText != null)
                {
                    stringBuilder.Append(" " + NPCModel.FlavorText);
                }
                stringBuilder.Append(" It has the following " + NPCModel.SpellcastingSpellClass.ToLower() + " spells prepared:");
                return stringBuilder.ToString();
            }
            return "";
        }
        private string UpdateWeaponName1()
        {
            return "";
        }
        #endregion
    }
}
