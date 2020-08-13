using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.NPC.ViewModels
{
    public class ActionOverviewUserControlViewModel : ViewModelBase
    {
        public ObservableCollection<ActionModelBase> NPCActions { get; set; }

        private ActionController actionController;
        public ActionOverviewUserControlViewModel()
        {
            actionController = new ActionController();
            NPCActions = actionController.GetNPCModel().NPCActions;
        }
        public void Refresh()
        {
            NPCActions = actionController.GetNPCModel().NPCActions;
            RaisePropertyChanged("NPCModel");
        }
    }
}
