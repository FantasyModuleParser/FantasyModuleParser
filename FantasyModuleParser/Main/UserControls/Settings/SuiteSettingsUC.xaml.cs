using FantasyModuleParser.Main.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace FantasyModuleParser.Main.UserControls.Settings
{
    /// <summary>
    /// Interaction logic for SuiteSettingsUC.xaml
    /// </summary>
    public partial class SuiteSettingsUC : UserControl
    {
        public SuiteSettingsUC()
        {
            InitializeComponent(); 
        }

        private void MainDefaultFolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SettingsViewModel viewModel = DataContext as SettingsViewModel;
            viewModel.ChangeDefaultMainFolder();
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

        private void SpellDefaultFolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SettingsViewModel viewModel = DataContext as SettingsViewModel;
            viewModel.ChangeDefaultSpellFolder(); // <-- Needs some input here
        }

        private void EquipmentDefaultFolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SettingsViewModel viewModel = DataContext as SettingsViewModel;
            viewModel.ChangeDefaultEquipmentFolder(); // <-- Needs some input here
        }

        private void ArtifactDefaultFolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SettingsViewModel viewModel = DataContext as SettingsViewModel;
            viewModel.ChangeDefaultArtifactsFolder(); // <-- Needs some input here
        }

        private void TablesDefaultFolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SettingsViewModel viewModel = DataContext as SettingsViewModel;
            viewModel.ChangeDefaultTablesFolder(); // <-- Needs some input here
        }

        private void ParcelsDefaultFolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SettingsViewModel viewModel = DataContext as SettingsViewModel;
            viewModel.ChangeDefaultParcelsFolder(); // <-- Needs some input here
        }

        private void FGModuleDefaultFolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SettingsViewModel viewModel = DataContext as SettingsViewModel;
            viewModel.ChangeDefaultFGModuleFolder(); // <-- Needs some input here
        }
    }
}
