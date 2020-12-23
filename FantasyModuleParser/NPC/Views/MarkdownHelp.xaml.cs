using System.Windows;
using System.Windows.Input;

namespace FantasyModuleParser.NPC.Views
{
    /// <summary>
    /// Interaction logic for MarkdownHelp.xaml
    /// </summary>
    public partial class MarkdownHelp : Window
    {
        public MarkdownHelp()
        {
            InitializeComponent();
            // Enable it so the popup window can close on the Escape key
            PreviewKeyDown += (sender, eventArgs) => { if (eventArgs.Key == Key.Escape) Close(); };
        }
       
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
