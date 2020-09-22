using FantasyModuleParser.Main.ViewModels;
using System.Windows.Forms;
using System.Windows.Input;

namespace FantasyModuleParser.Main.UserControls.Settings
{
    /// <summary>
    /// Interaction logic for SuiteSettingsUC.xaml
    /// </summary>
    public partial class SuiteSettingsUC : System.Windows.Controls.UserControl
    {
        public SuiteSettingsUC()
        {
            InitializeComponent();
        }

        private void MainDefaultFolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the default Main Folder";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string sSelectedPath = folderBrowserDialog.SelectedPath;
                SettingsViewModel viewModel = DataContext as SettingsViewModel;
                viewModel.SettingsModel.MainFolderLocation = sSelectedPath;
                MainDefaultFolder.Text = sSelectedPath;
            }
        }

        private void ProjectDefaultFolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SettingsViewModel viewModel = DataContext as SettingsViewModel;
            viewModel.ChangeDefaultProjectFolder(); // <-- Needs some input here
        }

        private void NPCDefaultFolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SettingsViewModel viewModel = DataContext as SettingsViewModel;
            viewModel.ChangeDefaultNPCFolder(); // <-- Needs some input here
        }
    }
}
