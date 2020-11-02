using System.Windows;

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
        }
        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
