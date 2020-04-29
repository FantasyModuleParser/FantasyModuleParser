using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.ViewModel
{
    public class NPCViewModel : ViewModelBase
    {
        private NPCModel _npcModel;
        public NPCModel npcModel
        {
            get => _npcModel;
            set => SetProperty(ref _npcModel, value);
        }
    }
}
