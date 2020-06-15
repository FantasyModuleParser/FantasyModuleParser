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

        public string UpdateSpeedDescription()
        {
            return "Speed: " + NPCModel.Speed + ", Fly: " + NPCModel.Fly + ", Burrow: " + NPCModel.Burrow;
        }
        private string _updateSkillsDescription()
        {
            StringBuilder sb = new StringBuilder();
            string delimiter = ", ";
            sb.Append(appendSkill("Acrobatics", NPCModel.Acrobatics)).Append(delimiter);
            sb.Append(appendSkill("Animal Handling", NPCModel.AnimalHandling)).Append(delimiter);
            sb.Append(appendSkill("Arcana", NPCModel.Arcana)).Append(delimiter);
            sb.Append(appendSkill("Athletics", NPCModel.Athletics)).Append(delimiter);
            sb.Append(appendSkill("Deception", NPCModel.Deception)).Append(delimiter);
            sb.Append(appendSkill("History", NPCModel.History)).Append(delimiter);
            sb.Append(appendSkill("Insight", NPCModel.Insight)).Append(delimiter);
            sb.Append(appendSkill("Intimidation", NPCModel.Intimidation)).Append(delimiter);
            sb.Append(appendSkill("Investigation", NPCModel.Investigation)).Append(delimiter);
            sb.Append(appendSkill("Medicine", NPCModel.Medicine)).Append(delimiter);
            sb.Append(appendSkill("Nature", NPCModel.Nature)).Append(delimiter);
            sb.Append(appendSkill("Perception", NPCModel.Perception)).Append(delimiter);
            sb.Append(appendSkill("Performance", NPCModel.Performance)).Append(delimiter);
            sb.Append(appendSkill("Persuasion", NPCModel.Persuasion)).Append(delimiter);
            sb.Append(appendSkill("Religion", NPCModel.Religion)).Append(delimiter);
            sb.Append(appendSkill("Sleight of Hand", NPCModel.SleightOfHand)).Append(delimiter);
            sb.Append(appendSkill("Stealth", NPCModel.Stealth)).Append(delimiter);
            sb.Append(appendSkill("Survival", NPCModel.Survival)).Append(delimiter);

            // Remove the last comma
            sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }

        private string appendSkill(string skillName, int skillValue)
        {
            if(skillValue != 0)
                return skillName + ((skillValue < 0) ? " -" : " +") + skillValue;
            return "";
        }
    }
}
