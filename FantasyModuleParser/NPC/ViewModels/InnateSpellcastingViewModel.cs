﻿using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.ViewModel;

namespace FantasyModuleParser.NPC.ViewModels
{
    public class InnateSpellcastingViewModel : ViewModelBase
    {
        public NPCModel NPCModel { get; set; }
        private NPCController npcController;

        public InnateSpellcastingViewModel()
        {
            npcController = new NPCController();
            NPCModel = npcController.GetNPCModel();
        }

        public void Refresh()
        {
            npcController = new NPCController();
            NPCModel = npcController.GetNPCModel();
            RaisePropertyChanged("Refresh");
        }
    }
}
