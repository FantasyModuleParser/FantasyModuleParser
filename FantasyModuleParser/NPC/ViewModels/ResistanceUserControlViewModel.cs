using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.ViewModel;

namespace FantasyModuleParser.NPC.ViewModels
{
    public class ResistanceUserControlViewModel : ViewModelBase
    {
        public NPCModel NPCModel { get; set; }

        private NPCController npcController;
        public ResistanceUserControlViewModel()
        {
            npcController = new NPCController();
            NPCModel = npcController.GetNPCModel();
            RaisePropertyChanged("NPCModel");
        }

        public void Refresh()
        {
            NPCModel = npcController.GetNPCModel();
            RaisePropertyChanged("NPCModel");
        }
    }
}
