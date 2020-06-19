﻿using FantasyModuleParser.NPC.Controllers;
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
        public string Challenge { get; set; }

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
            Challenge = UpdateChallengeRating();
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
            if (value != 0)
                return name + " " + value + " ft." + (hover ? " (hover), ": ", ") + delimiter;
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
        #region UpdateSenses
        private string UpdateSenses()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(appendSenses("darkvision ", NPCModel.Darkvision, " ft."));
            stringBuilder.Append(appendSenses("blindsight ", NPCModel.Blindsight, " ft."));
            stringBuilder.Append(appendSenses("tremorsense ", NPCModel.Tremorsense, " ft."));
            stringBuilder.Append(appendSenses("truesight ", NPCModel.Truesight, " ft."));
            stringBuilder.Append(appendSenses("passive perception ", NPCModel.PassivePerception, " ft."));
            if (stringBuilder.Length >= 2)
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.ToString();
        }
        private string appendSenses(string senseName, int senseValue, string senseRange)
        {
            string delimiter = ", ";
            if (senseValue != 0)
                return senseName + senseValue + senseRange + delimiter;
            return "";
        }
        #endregion
        #region UpdateChallengeRating
        private string UpdateChallengeRating()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(NPCModel.ChallengeRating + " (" + NPCModel.XP + ") XP");
            return stringBuilder.ToString();
        }
        #endregion
    }
}
