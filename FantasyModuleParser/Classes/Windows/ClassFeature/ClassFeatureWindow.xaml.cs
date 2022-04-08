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

namespace FantasyModuleParser.Classes.Windows.ClassFeature
{
    /// <summary>
    /// Interaction logic for ClassFeatureWindow.xaml
    /// </summary>
    public partial class ClassFeatureWindow : Window
    {
        public ClassFeatureWindow()
        {
            InitializeComponent();
        }

        private void Button_CloseWindowAction(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
