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
                this.SettingsModel.MainFolderLocation = @sSelectedPath;

                // Need to call this on the Model to tell the UserControl using this as a DataContext
                // to say "Hey, UserControl!  The 'SettingsModel' object has changed, and you should refresh yourself!
                RaisePropertyChanged("SettingsModel"); 
            }
        }
        public void ChangeDefaultProjectFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the default Main Folder";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string sSelectedPath = folderBrowserDialog.SelectedPath;
                this.SettingsModel.ProjectFolderLocation = @sSelectedPath;

                // Need to call this on the Model to tell the UserControl using this as a DataContext
                // to say "Hey, UserControl!  The 'SettingsModel' object has changed, and you should refresh yourself!
                RaisePropertyChanged("SettingsModel");
            }
        }
        public void ChangeDefaultNPCFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the default Main Folder";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string sSelectedPath = folderBrowserDialog.SelectedPath;
                this.SettingsModel.NPCFolderLocation = @sSelectedPath;

                // Need to call this on the Model to tell the UserControl using this as a DataContext
                // to say "Hey, UserControl!  The 'SettingsModel' object has changed, and you should refresh yourself!
                RaisePropertyChanged("SettingsModel");
            }
        }
    }
}
