using FantasyModuleParser.Equipment.UserControls.Models;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            ArmorUCLayout.DataContext = this;
        }

        private void PositiveNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text) || e.Text.Contains("-");
        }

        public static readonly DependencyProperty ArmorModelProperty =
            DependencyProperty.Register("ArmorModelValue", typeof(ArmorModel), typeof(ArmorDetailsUC));

        public ArmorModel ArmorModelValue
        {
            get { return (ArmorModel)GetValue(ArmorModelProperty); }
            set { SetValue(ArmorModelProperty, value); }
        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
