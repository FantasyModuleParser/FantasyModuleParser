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
        public string Description { 
            get 
            { return this._description; } 
            set 
            { 
                RaisePropertyChanged(nameof(Description)); 
                this._description = value; } 
        }
        private NPCController npcController;
        public DescriptionUCViewModel()
        {
            npcController = new NPCController();
        }

        public void ValidateXML()
        {

        }
        public void Refresh()
        {
            Description = npcController.GetNPCModel().Description;
        }

        public void UpdateNPCDescription(string description)
        {
            npcController.GetNPCModel().Description = description;
        }
    }
}
