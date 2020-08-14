using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.NPC.ViewModels
{
    public class DescriptionUCViewModel : ViewModelBase
    {
        private string _description;
        private NPCModel _npcModel;
        public string Description { 
            get 
            { return this._npcModel.Description; } 
            set 
            {
                this._npcModel.Description = value;
                RaisePropertyChanged(nameof(Description)); 
            } 
        }
        private NPCController npcController;
        public DescriptionUCViewModel()
        {
            npcController = new NPCController();
            _npcModel = npcController.GetNPCModel();
        }

        public void ValidateXML()
        {

        }
        public void Refresh()
        {
            _npcModel = npcController.GetNPCModel();
            RaisePropertyChanged(nameof(Description));
        }
    }
}
