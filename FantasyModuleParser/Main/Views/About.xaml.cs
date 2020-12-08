using System.Windows;
using System.Windows.Input;

namespace FantasyModuleParser.Main
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
            // Enable it so the popup window can close on the Escape key
            PreviewKeyDown += (sender, eventArgs) => { if (eventArgs.Key == Key.Escape) Close(); };
            versionlabel.Content = "v" + System.Windows.Forms.Application.ProductVersion;
        }
        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
