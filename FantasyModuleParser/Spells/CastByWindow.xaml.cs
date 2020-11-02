using System.Text;
using System.Windows;

namespace FantasyModuleParser.Spells
{
    /// <summary>
    /// Interaction logic for CastByWindow.xaml
    /// </summary>
    public partial class CastByWindow : Window
    {
        public CastByWindow()
        {
            InitializeComponent();
        }
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
        }
    }
}
