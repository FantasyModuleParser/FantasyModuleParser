using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Data;
using FantasyModuleParser.Tables.ViewModels;
using FantasyModuleParser.Tables.ViewModels.Enums;
using System.Data;
using FantasyModuleParser.Main.Services;
using System;

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

            TableOptionViewModel tableOptionViewModel = DataContext as TableOptionViewModel;

            // Create and add the columns to the collection.
            TableExampleDataGrid.Columns.Clear();
            TableExampleDataGrid.Columns.Add(CreateBoundColumn("From", "From"));
            TableExampleDataGrid.Columns.Add(CreateBoundColumn("To", "To"));

            for(int colIdx = 2; colIdx < tableOptionViewModel.TableModel.ColumnHeaderLabels.Count; colIdx++)
            {
                TableExampleDataGrid.Columns.Add(CreateBoundColumn($"Col{colIdx}", tableOptionViewModel.TableModel.ColumnHeaderLabels[colIdx]));
            }




            //DataView dv = new DataView(tableOptionViewModel.Data);

            //TableExampleDataGrid.ItemsSource = dv;

            // Generate the Context Menu used by the grid
            generateContextMenu();
        }

        private void generateContextMenu()
        {
            TableOptionViewModel tableOptionViewModel = DataContext as TableOptionViewModel;
            TableExampleDataGrid.ContextMenu.Items.Add(buildLinkWithinThisProject());
            TableExampleDataGrid.ContextMenu.Items.Add(new Separator());

            TableExampleDataGrid.ContextMenu.Items.Add(buildInternalLink());
            TableExampleDataGrid.ContextMenu.Items.Add(buildExternalLink());

            TableExampleDataGrid.ContextMenu.Items.Add(new Separator());
            MenuItem insertRowMenuItem = new MenuItem();
            insertRowMenuItem.Header = "Insert Row";
            insertRowMenuItem.Command = tableOptionViewModel.InsertRowCommand;
            TableExampleDataGrid.ContextMenu.Items.Add(insertRowMenuItem);

            MenuItem deleteRowMenuItem = new MenuItem();
            deleteRowMenuItem.Header = "Delete Row";
            deleteRowMenuItem.Command = tableOptionViewModel.DeleteRowCommand;
            deleteRowMenuItem.CommandParameter = TableExampleDataGrid.SelectedItem;
            TableExampleDataGrid.ContextMenu.Items.Add(deleteRowMenuItem);

            MenuItem clearRowMenuItem = new MenuItem();
            clearRowMenuItem.Header = "Clear Row";
            clearRowMenuItem.Command = tableOptionViewModel.ClearRowCommand;
            TableExampleDataGrid.ContextMenu.Items.Add(clearRowMenuItem);

        }

        private void ClearRowMenuItem_Click(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void DeleteRowMenuItem_Click(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void InsertRow_Click(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private MenuItem buildLinkWithinThisProject()
        {
            MenuItem miLinkWithinProject = new MenuItem();
            miLinkWithinProject.Header = "Select link within this Project";

            MenuItem imageMenuItem = new MenuItem();
            imageMenuItem.Header = "Image";

            // TODO:  Build out a reference of all images within 
            // this FMP module, if applicable

            MenuItem npcMenuItem = new MenuItem();
            npcMenuItem.Header = "NPC";

            //TODO:  Build out a reference of all NPCs between categories
            // with this FMP module

            MenuItem spellMenuItem = new MenuItem();
            npcMenuItem.Header = "Spell";

            //TODO:  Build out a reference of all NPCs between categories
            // with this FMP module

            MenuItem tableMenuItem = new MenuItem();
            npcMenuItem.Header = "Table";

            //TODO:  Build out a reference of all NPCs between categories
            // with this FMP module

            miLinkWithinProject.Items.Add(imageMenuItem);
            miLinkWithinProject.Items.Add(npcMenuItem);
            miLinkWithinProject.Items.Add(spellMenuItem);
            miLinkWithinProject.Items.Add(tableMenuItem);

            return miLinkWithinProject;
        }

        private MenuItem buildInternalLink()
        {
            MenuItem menuItemBuildInternalLink = new MenuItem();
            menuItemBuildInternalLink.Header = "Build Internal Link";

            menuItemBuildInternalLink.Items.Add(buildInternalLinkSubMenuItem("Image"));
            menuItemBuildInternalLink.Items.Add(buildInternalLinkSubMenuItem("NPC"));
            menuItemBuildInternalLink.Items.Add(buildInternalLinkSubMenuItem("Spell"));
            menuItemBuildInternalLink.Items.Add(buildInternalLinkSubMenuItem("Table"));
            menuItemBuildInternalLink.Items.Add(buildInternalLinkSubMenuItem("Parcel"));
            menuItemBuildInternalLink.Items.Add(buildInternalLinkSubMenuItem("Item"));

            return menuItemBuildInternalLink;
        }

        private MenuItem buildInternalLinkSubMenuItem(string headerValue)
        {
            MenuItem subMenuItem = new MenuItem();
            subMenuItem.Header = headerValue;
            subMenuItem.Click += BuildInternalLink_MenuItem_ClickEvent;

            return subMenuItem;
        }

        private void BuildInternalLink_MenuItem_ClickEvent(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private MenuItem buildExternalLink()
        {
            MenuItem menuItemBuildExternalLink = new MenuItem();
            menuItemBuildExternalLink.Header = "Build External Link";

            menuItemBuildExternalLink.Items.Add(buildExternalLinkSubMenuItem("Image"));
            menuItemBuildExternalLink.Items.Add(buildExternalLinkSubMenuItem("NPC"));
            menuItemBuildExternalLink.Items.Add(buildExternalLinkSubMenuItem("Spell"));
            menuItemBuildExternalLink.Items.Add(buildExternalLinkSubMenuItem("Table"));
            menuItemBuildExternalLink.Items.Add(buildExternalLinkSubMenuItem("Parcel"));
            menuItemBuildExternalLink.Items.Add(buildExternalLinkSubMenuItem("Item"));

            return menuItemBuildExternalLink;
        }

        private MenuItem buildExternalLinkSubMenuItem(string headerValue)
        {
            MenuItem subMenuItem = new MenuItem();
            subMenuItem.Header = headerValue;
            subMenuItem.Click += BuildExternalLink_MenuItem_ClickEvent;

            return subMenuItem;
        }

        private void BuildExternalLink_MenuItem_ClickEvent(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
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

        private void LoadTableModule_Click(object sender, RoutedEventArgs e)
        {
            SettingsService settingsService = new SettingsService();
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            
            openFileDlg.InitialDirectory = settingsService.Load().TableFolderLocation;
            openFileDlg.Filter = "Table files (*.tbl)|*.tbl|All files (*.*)|*.*";
            openFileDlg.RestoreDirectory = true;

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                TableOptionViewModel tableOptionViewModel = DataContext as TableOptionViewModel;
                tableOptionViewModel.TableModel = tableOptionViewModel.TableModel.Load(openFileDlg.FileName);
               

                tableOptionViewModel.TableDataView = new DataView(tableOptionViewModel.TableModel.tableDataTable);

                InitializeTableDataGrid();
            }
        }

        private void InsertColumn_Click(object sender, RoutedEventArgs e)
        {
            TableOptionViewModel tableOptionViewModel = DataContext as TableOptionViewModel;
            int currentColumnCount = TableExampleDataGrid.Columns.Count;
            //TableExampleDataGrid.Columns.Add(CreateBoundColumn("Col2", tableOptionViewModel.TableModel.ColumnHeaderLabels[2]));
            TableExampleDataGrid.Columns.Add(CreateBoundColumn($"Col{currentColumnCount}", ""));
        }

        private void TableExampleDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TableExampleDataGrid.SelectedIndex == -1) //if column selected, cant use .CurrentColumn property
            {
                MessageBox.Show("Please double click on a row");
            }
            else
            {
                DataRowView dataRowView = TableExampleDataGrid.SelectedItem as DataRowView;

                dataRowView.Delete();

                //DataGridColumn columnHeader = TableExampleDataGrid.CurrentColumn;

                //TableExampleDataGrid.Columns.Remove(columnHeader);
                //if (columnHeader != null)
                //{
                //    string input = Interaction.InputBox("Title", "Prompt", "Default", 0, 0);
                //    columnHeader.Header = input;
                //}
            }
        }
    }
}
