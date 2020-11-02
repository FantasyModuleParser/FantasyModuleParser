using FantasyModuleParser.Main.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace FantasyModuleParser.Main.Views
{
    /// <summary>
    /// Interaction logic for Changelog.xaml
    /// </summary>
    public partial class Changelog : Window
    {
        private ChangelogViewModel viewModel;
        public Changelog()
        {
            InitializeComponent();
            viewModel = new ChangelogViewModel();
            DataContext = viewModel;

            // Enable it so the popup window can close on the Escape key
            PreviewKeyDown += (sender, eventArgs) => { if (eventArgs.Key == Key.Escape) Close(); };
        }
        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
