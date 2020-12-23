using System.Windows;
using System.Windows.Input;

namespace FantasyModuleParser.NPC
{
    /// <summary>
    /// Interaction logic for FGListOptions.xaml
    /// </summary>
    public partial class FGListOptions : Window
    {
        public FGListOptions()
        {
            InitializeComponent();
            // Enable it so the popup window can close on the Escape key
            PreviewKeyDown += (sender, eventArgs) => { if (eventArgs.Key == Key.Escape) Close(); };
        }
        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
