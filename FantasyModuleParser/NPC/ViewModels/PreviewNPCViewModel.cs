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

        public PreviewNPCViewModel()
        {
            npcController = new NPCController();
            NPCModel = npcController.GetNPCModel();
            SpeedDescription = UpdateSpeedDescription();
            SkillsDescription = _updateSkillsDescription();
        }

        public PreviewNPCViewModel(NPCModel nPCModel)
        {
            NPCModel = nPCModel;
            SpeedDescription = UpdateSpeedDescription();
            SkillsDescription = _updateSkillsDescription();
        }

        public string UpdateSpeedDescription()
        {
            StringBuilder stringBuilder
                = new StringBuilder();

            if(NPCModel.Speed == 0)
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

            // Remove the last comma
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.ToString();
        }

        private string appendSkill(string skillName, int skillValue)
        {
            string delimiter = ", ";
            if (skillValue != 0)
                return skillName + ((skillValue < 0) ? " -" : " +") + skillValue + delimiter;
            return "";
        }
    }
}
