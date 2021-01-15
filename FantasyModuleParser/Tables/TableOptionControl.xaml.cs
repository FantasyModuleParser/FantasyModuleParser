using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace FantasyModuleParser.Tables
{
    /// <summary>
    /// Interaction logic for TableUserControl.xaml
    /// </summary>
    public partial class TableUserControl : UserControl
    {
        private Regex numericValidationRegex = new Regex("[^0-9]+");
        public TableUserControl()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numericValidationRegex.IsMatch(e.Text);
        }

        private void RoleMethodCombobox_Changed(object sender, SelectionChangedEventArgs e)
        {
            CustomDiceRollGrid.Visibility = CustomDiceCBI.IsSelected ? Visibility.Visible : Visibility.Hidden;
            PresetRangeGrid.Visibility = PresetRangeCBI.IsSelected ? Visibility.Visible : Visibility.Hidden; 
        }
    }
}
