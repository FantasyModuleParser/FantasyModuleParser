using FantasyModuleParser.Main.ViewModels;
using System.Windows;
using System.Windows.Input;


namespace FantasyModuleParser.Main
{
    /// <summary>
    /// Interaction logic for Supporters.xaml
    /// </summary>
    public partial class Supporters : Window
    {
        private SupportersViewModel viewModel;
        
        public Supporters()
        {
            InitializeComponent();
            viewModel = new SupportersViewModel();
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
