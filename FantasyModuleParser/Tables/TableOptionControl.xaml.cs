using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Data;
using FantasyModuleParser.Tables.ViewModels;
using FantasyModuleParser.Tables.ViewModels.Enums;
using System.Collections.ObjectModel;
using FantasyModuleParser.Tables.Models;
using System.Text;

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
            TableOptionViewModel tableOptionViewModel = DataContext as TableOptionViewModel;
            tableOptionViewModel.UpdateDataGrid();
            TableExampleDataGrid?.Items.Refresh();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numericValidationRegex.IsMatch(e.Text);
        }

        private void RoleMethodCombobox_Changed(object sender, SelectionChangedEventArgs e)
        {
            // Validate that the grids to be hidden / visible have been initialized
            if (CustomDiceRollGrid == null || PresetRangeGrid == null)
                return;

            // Validate that the viewModel exists and is tied to the TableOptionViewModel for correct reference
            TableOptionViewModel tableOptionViewModel = DataContext as TableOptionViewModel;
            if (tableOptionViewModel != null && tableOptionViewModel.TableModel != null)
            {
                // Reset the grid to default both grids to hidden
                CustomDiceRollGrid.Visibility = Visibility.Hidden;
                PresetRangeGrid.Visibility = Visibility.Hidden;
                // Switch between the RollMethodEnum options, setting the appropriate grid to be visible
                switch (tableOptionViewModel.TableModel.RollMethod)
                {
                    case RollMethodEnum.CustomDiceRoll:
                        CustomDiceRollGrid.Visibility = Visibility.Visible;
                        break;
                    case RollMethodEnum.PresetRange:
                        PresetRangeGrid.Visibility = Visibility.Visible;
                        break;
                }
            }
        }
    }
}
