using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.ViewModel;
using System.Data.Linq;
using System.Windows.Forms;

namespace FantasyModuleParser.Main.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsModel SettingsModel { get; set; }

        private SettingsService _settingsService;
        public SettingsViewModel()
        {
            _settingsService = new SettingsService();
            SettingsModel = _settingsService.Load();
        }

        public void ChangeDefaultMainFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the default Main Folder";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string sSelectedPath = folderBrowserDialog.SelectedPath;
                SettingsModel.MainFolderLocation = sSelectedPath;
            }
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
