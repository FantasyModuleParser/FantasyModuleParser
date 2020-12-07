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
                SettingsModel.MainFolderLocation = @sSelectedPath;

                // Need to call this on the Model to tell the UserControl using this as a DataContext
                // to say "Hey, UserControl!  The 'SettingsModel' object has changed, and you should refresh yourself!
                RaisePropertyChanged("SettingsModel"); 
            }
        }
        public void ChangeDefaultProjectFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the default Project Folder";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string sSelectedPath = folderBrowserDialog.SelectedPath;
                SettingsModel.ProjectFolderLocation = @sSelectedPath;

                // Need to call this on the Model to tell the UserControl using this as a DataContext
                // to say "Hey, UserControl!  The 'SettingsModel' object has changed, and you should refresh yourself!
                RaisePropertyChanged("SettingsModel");
            }
        }
        public void ChangeDefaultNPCFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the default NPC Folder";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string sSelectedPath = folderBrowserDialog.SelectedPath;
                SettingsModel.NPCFolderLocation = @sSelectedPath;

                // Need to call this on the Model to tell the UserControl using this as a DataContext
                // to say "Hey, UserControl!  The 'SettingsModel' object has changed, and you should refresh yourself!
                RaisePropertyChanged("SettingsModel");
            }
        }
        public void ChangeDefaultSpellFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the default Spell Folder";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string sSelectedPath = folderBrowserDialog.SelectedPath;
                SettingsModel.SpellFolderLocation = @sSelectedPath;

                // Need to call this on the Model to tell the UserControl using this as a DataContext
                // to say "Hey, UserControl!  The 'SettingsModel' object has changed, and you should refresh yourself!
                RaisePropertyChanged("SettingsModel");
            }
        }
        public void ChangeDefaultEquipmentFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the default Equipment Folder";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string sSelectedPath = folderBrowserDialog.SelectedPath;
                SettingsModel.EquipmentFolderLocation = @sSelectedPath;

                // Need to call this on the Model to tell the UserControl using this as a DataContext
                // to say "Hey, UserControl!  The 'SettingsModel' object has changed, and you should refresh yourself!
                RaisePropertyChanged("SettingsModel");
            }
        }
        public void ChangeDefaultArtifactsFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the default Equipment Folder";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string sSelectedPath = folderBrowserDialog.SelectedPath;
                SettingsModel.ArtifactFolderLocation = @sSelectedPath;

                // Need to call this on the Model to tell the UserControl using this as a DataContext
                // to say "Hey, UserControl!  The 'SettingsModel' object has changed, and you should refresh yourself!
                RaisePropertyChanged("SettingsModel");
            }
        }
        public void ChangeDefaultTablesFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the default Equipment Folder";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string sSelectedPath = folderBrowserDialog.SelectedPath;
                SettingsModel.TableFolderLocation = @sSelectedPath;

                // Need to call this on the Model to tell the UserControl using this as a DataContext
                // to say "Hey, UserControl!  The 'SettingsModel' object has changed, and you should refresh yourself!
                RaisePropertyChanged("SettingsModel");
            }
        }
        public void ChangeDefaultParcelsFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the default Equipment Folder";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string sSelectedPath = folderBrowserDialog.SelectedPath;
                SettingsModel.ParcelFolderLocation = @sSelectedPath;

                // Need to call this on the Model to tell the UserControl using this as a DataContext
                // to say "Hey, UserControl!  The 'SettingsModel' object has changed, and you should refresh yourself!
                RaisePropertyChanged("SettingsModel");
            }
        }
        public void ChangeDefaultFGModuleFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the default Fantasy Grounds Module Folder";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string sSelectedPath = folderBrowserDialog.SelectedPath;
                SettingsModel.FGModuleFolderLocation = @sSelectedPath;

                // Need to call this on the Model to tell the UserControl using this as a DataContext
                // to say "Hey, UserControl!  The 'SettingsModel' object has changed, and you should refresh yourself!
                RaisePropertyChanged("SettingsModel");
            }
        }
        public void ChangeDefaultFGCampaignFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the default Fantasy Grounds Module Folder";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string sSelectedPath = folderBrowserDialog.SelectedPath;
                SettingsModel.FGCampaignFolderLocation = @sSelectedPath;

                // Need to call this on the Model to tell the UserControl using this as a DataContext
                // to say "Hey, UserControl!  The 'SettingsModel' object has changed, and you should refresh yourself!
                RaisePropertyChanged("SettingsModel");
            }
        }
    }
}
