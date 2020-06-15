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

        public PreviewNPCViewModel()
        {
            npcController = new NPCController();
            NPCModel = npcController.GetNPCModel();
            SpeedDescription = UpdateSpeedDescription();
        }

        public string UpdateSpeedDescription()
        {
            return NPCModel.Speed + " ft., climb " + NPCModel.Climb + " ft., fly " + NPCModel.Fly + " ft., burrow " + NPCModel.Burrow + " ft., swim " + NPCModel.Swim + " ft.";
        }
    }
}
