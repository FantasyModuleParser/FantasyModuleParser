using FantasyModuleParser.NPC.ViewModel;

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
