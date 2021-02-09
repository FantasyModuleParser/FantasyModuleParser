using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Data;

namespace FantasyModuleParser.Tables
{
    /// <summary>
    /// 
    /// 
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
            if (CustomDiceCBI != null & PresetRangeCBI != null)
            {
                CustomDiceRollGrid.Visibility = CustomDiceCBI.IsSelected ? Visibility.Visible : Visibility.Hidden;
                PresetRangeGrid.Visibility = PresetRangeCBI.IsSelected ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0)
                this.textBox.SetBinding(TextBox.TextProperty, (string)null);
            else
            {
                //var selectedCell = e.AddedCells.First();
                var selectedCell = e.AddedCells[0];

                // Assumes your header is the same name as the field it's bound to
                var binding = new Binding(selectedCell.Column.Header.ToString())
                {
                    Mode = BindingMode.TwoWay,
                    Source = selectedCell.Item,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                this.textBox.SetBinding(TextBox.TextProperty, binding);
            }
        }
    }
}
