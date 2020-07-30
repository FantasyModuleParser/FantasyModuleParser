using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.ViewModel;
using System;

namespace FantasyModuleParser.NPC.ViewModels
{
    public class ActionOverviewUCViewModel : ViewModelBase
    {
        //public ObservableCollection<ActionModelBase> NPCActions { get;}
        public NPCModel NPCModel { get; set; }
        private NPCController npcController = new NPCController();

        public ActionOverviewUCViewModel()
        {
            NPCModel = npcController.GetNPCModel();
            NPCModel.NPCActions.CollectionChanged += NPCActions_CollectionChanged;
        }

        private void NPCActions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("NPCActions_Changed");
        }

        public void Refresh()
        {
            NPCModel = npcController.GetNPCModel();
            RaisePropertyChanged("NPCActions_Changed");
        }
    }
}
