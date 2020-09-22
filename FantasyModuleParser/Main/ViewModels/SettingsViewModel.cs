using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC.ViewModel;

namespace FantasyModuleParser.Main.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsModel SettingsModel { get; set; }
        public SettingsViewModel()
        {
            SettingsModel = new SettingsModel();
        }

        public void ChangeDefaultProjectFolder()
        {
            //TODO: Include input parameters & change SettingsModel object accordingly
        }
        public void ChangeDefaultNPCFolder()
        {
            //TODO: Include input parameters & change SettingsModel object accordingly
        }
    }
}
