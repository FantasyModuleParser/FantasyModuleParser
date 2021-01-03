using FantasyModuleParser.Main.Services;
using FantasyModuleParser.Main.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace FantasyModuleParser.Main
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private SettingsService settingsService;
        public Settings()
        {
            InitializeComponent();
            //DataContext = new SettingsViewModel();
            settingsService = new SettingsService();
            // Enable it so the popup window can close on the Escape key
            PreviewKeyDown += (sender, eventArgs) => { if (eventArgs.Key == Key.Escape) Close(); };
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            SettingsViewModel settingsViewModel = DataContext as SettingsViewModel;
            settingsService.Save(settingsViewModel.SettingsModel);
            Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SettingsViewModel settingsViewModel = DataContext as SettingsViewModel;
            settingsService.Save(settingsViewModel.SettingsModel);
        }
    }
}
