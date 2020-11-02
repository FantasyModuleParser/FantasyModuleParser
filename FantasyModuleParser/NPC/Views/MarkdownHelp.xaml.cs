using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            PreviewKeyDown += (sender, eventArgs) => { if (eventArgs.Key == Key.Escape) Close(); };
        }
       
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
