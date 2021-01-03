using FantasyModuleParser.Main.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace FantasyModuleParser.Main
{
    /// <summary>
    /// Interaction logic for FMPConfigurationView.xaml
    /// </summary>
    public partial class FMPConfigurationView : Window
    {
        private FMPConfigurationViewModel configurationViewModel;
        public FMPConfigurationView()
        {
            InitializeComponent();

            // Enable it so the popup window can close on the Escape key
            PreviewKeyDown += (sender, eventArgs) => { if (eventArgs.Key == Key.Escape) Close(); };

            configurationViewModel = new FMPConfigurationViewModel();
            DataContext = configurationViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            configurationViewModel.Refresh();
            DataContext = configurationViewModel;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
