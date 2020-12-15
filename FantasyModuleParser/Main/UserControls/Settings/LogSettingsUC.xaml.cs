using FantasyModuleParser.Main.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace FantasyModuleParser.Main.UserControls.Settings
{
    /// <summary>
    /// Interaction logic for LogSettingsUC.xaml
    /// </summary>
    public partial class LogSettingsUC : UserControl
    {
        public LogSettingsUC()
        {
            InitializeComponent();
        }

        private void ChangeDefaultLogFolder_Click(object sender, MouseButtonEventArgs e)
        {
            SettingsViewModel viewModel = DataContext as SettingsViewModel;
            viewModel.ChangeDefaultLogFolder();
        }
    }
}
