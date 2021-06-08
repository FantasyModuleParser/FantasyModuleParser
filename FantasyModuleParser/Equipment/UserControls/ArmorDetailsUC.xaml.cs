using FantasyModuleParser.Equipment.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FantasyModuleParser.Equipment.UserControls
{
    /// <summary>
    /// Interaction logic for ArmorDetailsUC.xaml
    /// </summary>
    public partial class ArmorDetailsUC : UserControl
    {
        #region Selected Armor Enum Option
        public static readonly DependencyProperty SelectedArmorEnumProperty =
            DependencyProperty.Register
            (
                "selectedArmorEnum",
                typeof(int),
                typeof(ArmorDetailsUC),
                new FrameworkPropertyMetadata(0)
            );

        public int selectedArmorEnum
        {
            get { 
                return (int)GetValue(SelectedArmorEnumProperty);
            }
            set { 
                SetValue(SelectedArmorEnumProperty, value); 
            }
        }
        #endregion Selected Armor Enum Option
        public ArmorDetailsUC()
        {
            InitializeComponent();
        }

        private void PositiveNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text) || e.Text.Contains("-");
        }
    }
}
