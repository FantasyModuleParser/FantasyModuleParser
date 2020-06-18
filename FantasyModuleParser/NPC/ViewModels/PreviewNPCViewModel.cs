using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public PreviewNPCViewModel()
        {
            npcController = new NPCController();
            NPCModel = npcController.GetNPCModel();
            SpeedDescription = UpdateSpeedDescription();
            SkillsDescription = _updateSkillsDescription();
            StrengthAttribute = UpdateStrengthAttribute();
            DexterityAttribute = UpdateDexterityAttribute();
            ConstitutionAttribute = UpdateConstitutionAttribute();
            IntelligenceAttribute = UpdateIntelligenceAttribute();
            WisdomAttribute = UpdateWisdomAttribute();
            CharismaAttribute = UpdateCharismaAttribute();
        }

        public PreviewNPCViewModel(NPCModel nPCModel)
        {
            NPCModel = nPCModel;
            SpeedDescription = UpdateSpeedDescription();
            SkillsDescription = _updateSkillsDescription();
        }

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
        public string UpdateSpeedDescription()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (NPCModel.Speed == 0)
                stringBuilder.Append("0 ft., ");
            else 
                stringBuilder.Append(_appendSpeedAttribute("", NPCModel.Speed, false));
                stringBuilder.Append(_appendSpeedAttribute("climb", NPCModel.Climb, false));
                stringBuilder.Append(_appendSpeedAttribute("fly", NPCModel.Fly, NPCModel.Hover));
                stringBuilder.Append(_appendSpeedAttribute("burrow", NPCModel.Burrow, false));
                stringBuilder.Append(_appendSpeedAttribute("swim", NPCModel.Swim, false));

            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.ToString().Trim();
        }

        private string _appendSpeedAttribute(string name, int value, bool hover)
        {
            if (value != 0)
                return name + " " + value + " ft." + (hover ? " (hover), ": ", ");
            return "";
        }

        private string _updateSkillsDescription()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string delimiter = ", ";
            stringBuilder.Append(appendSkill("Acrobatics", NPCModel.Acrobatics)).Append(delimiter);
            stringBuilder.Append(appendSkill("Animal Handling", NPCModel.AnimalHandling)).Append(delimiter);
            stringBuilder.Append(appendSkill("Arcana", NPCModel.Arcana)).Append(delimiter);
            stringBuilder.Append(appendSkill("Athletics", NPCModel.Athletics)).Append(delimiter);
            stringBuilder.Append(appendSkill("Deception", NPCModel.Deception)).Append(delimiter);
            stringBuilder.Append(appendSkill("History", NPCModel.History)).Append(delimiter);
            stringBuilder.Append(appendSkill("Insight", NPCModel.Insight)).Append(delimiter);
            stringBuilder.Append(appendSkill("Intimidation", NPCModel.Intimidation)).Append(delimiter);
            stringBuilder.Append(appendSkill("Investigation", NPCModel.Investigation)).Append(delimiter);
            stringBuilder.Append(appendSkill("Medicine", NPCModel.Medicine)).Append(delimiter);
            stringBuilder.Append(appendSkill("Nature", NPCModel.Nature)).Append(delimiter);
            stringBuilder.Append(appendSkill("Perception", NPCModel.Perception)).Append(delimiter);
            stringBuilder.Append(appendSkill("Performance", NPCModel.Performance)).Append(delimiter);
            stringBuilder.Append(appendSkill("Persuasion", NPCModel.Persuasion)).Append(delimiter);
            stringBuilder.Append(appendSkill("Religion", NPCModel.Religion)).Append(delimiter);
            stringBuilder.Append(appendSkill("Sleight of Hand", NPCModel.SleightOfHand)).Append(delimiter);
            stringBuilder.Append(appendSkill("Stealth", NPCModel.Stealth)).Append(delimiter);
            stringBuilder.Append(appendSkill("Survival", NPCModel.Survival)).Append(delimiter);

            // Remove the last comma
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.ToString();
        }

        private string appendSkill(string skillName, int skillValue)
        {
            if(skillValue != 0)
                return skillName + ((skillValue < 0) ? " " : " +") + skillValue;
            return "";
        }
    }
}
