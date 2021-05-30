using FantasyModuleParser.NPC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.Equipment.UserControls.ViewModels
{
    public class EquipmentNavigationUCViewModel : ViewModelBase
    {
        public EquipmentNavigationUCViewModel()
        {
            PreviousItemLabel = "Change Me";
        }

        public string PreviousItemLabel { get; set; }
    }
}
