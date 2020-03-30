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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for NPCEImportText.xaml
    /// </summary>
    public partial class NPCEImportText : Window
    {
        public NPCEImportText()
        {
            InitializeComponent();
        }
        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
