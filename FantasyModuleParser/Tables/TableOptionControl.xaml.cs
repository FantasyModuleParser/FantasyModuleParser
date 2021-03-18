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
using System.Web.UI.WebControls;
using System.Data;

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
            InitializeTableDataGrid();
            //TableOptionViewModel tableOptionViewModel = DataContext as TableOptionViewModel;
            //TableExampleDataGrid.ItemsSource = tableOptionViewModel.Data.DefaultView;

        }

        private void InitializeTableDataGrid()
        {
            //TableExampleDataGrid.BorderColor = System.Drawing.Color.Black;
            //TableExampleDataGrid.CellPadding = 3;
            TableExampleDataGrid.AutoGenerateColumns = false;
            TableExampleDataGrid.CanUserSortColumns = false;
            TableExampleDataGrid.CanUserReorderColumns = false;

            // Set the styles for the DataGrid.
            //TableExampleDataGrid.HeaderStyle.BackColor =
            //    System.Drawing.Color.FromArgb(0x0000aaaa);

            // Create the columns for the DataGrid control. The DataGrid
            // columns are dynamically generated. Therefore, the columns   
            // must be re-created each time the page is refreshed.

            // Create and add the columns to the collection.
            TableExampleDataGrid.Columns.Add(CreateBoundColumn("From", "From"));
            TableExampleDataGrid.Columns.Add(CreateBoundColumn("To", "To"));
            TableExampleDataGrid.Columns.Add(CreateBoundColumn("1", ""));

            TableOptionViewModel tableOptionViewModel = DataContext as TableOptionViewModel;

            DataView dv = new DataView(tableOptionViewModel.Data);

            TableExampleDataGrid.ItemsSource = dv;




        }

        private DataGridBoundColumn CreateBoundColumn(string DataFieldValue, string HeaderTextValue)
        {

            // This version of the CreateBoundColumn method sets only the
            // DataField and HeaderText properties.

            // Create a BoundColumn.
            DataGridBoundColumn column = new DataGridTextColumn
            {
                // Set the properties of the BoundColumn.
                Binding = new Binding(DataFieldValue),
                Header = HeaderTextValue,
                Width = 120
            };

            return column;

        }

        private static int columnNumber = 100;
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TableOptionViewModel tableOptionViewModel = DataContext as TableOptionViewModel;
            //tableOptionViewModel.ChangeGridDimesions();
            //TableExampleDataGrid?.Items.Refresh();
            //tableOptionViewModel.AddColumn($"Column {columnNumber}");
            //columnNumber++;
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

        private void TableExampleDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var columnIndex = e.Column.DisplayIndex;
            var rowIndex = e.Row.GetIndex();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
