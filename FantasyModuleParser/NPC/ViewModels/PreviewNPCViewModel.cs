using FantasyModuleParser.NPC.Controllers;
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

        public PreviewNPCViewModel()
        {
            npcController = new NPCController();
            NPCModel = npcController.GetNPCModel();
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
        }

        public PreviewNPCViewModel(NPCModel nPCModel)
        {
            NPCModel = nPCModel;
            SpeedDescription = UpdateSpeedDescription();
            SkillsDescription = UpdateSkillsDescription();
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
            stringBuilder.Append(appendSavingThrow("Str", NPCModel.SavingThrowStr));
            stringBuilder.Append(appendSavingThrow("Dex", NPCModel.SavingThrowDex));
            stringBuilder.Append(appendSavingThrow("Con", NPCModel.SavingThrowCon));
            stringBuilder.Append(appendSavingThrow("Int", NPCModel.SavingThrowInt));
            stringBuilder.Append(appendSavingThrow("Wis", NPCModel.SavingThrowWis));
            stringBuilder.Append(appendSavingThrow("Cha", NPCModel.SavingThrowCha));
            if (stringBuilder.Length >= 2)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }
            return stringBuilder.ToString();
        }
        private string appendSavingThrow(string savingThrowName, int savingThrowValue)
        {
            string delimiter = ", ";
            if (savingThrowValue != 0)
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
                    stringBuilder.Append(selectableActionModel.ActionName).Append(", ");
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
                return senseName + senseValue + senseRange + " (blind beyond)" + delimiter;
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
    }
}
