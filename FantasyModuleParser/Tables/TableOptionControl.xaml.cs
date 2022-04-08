using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Data;
using FantasyModuleParser.Tables.ViewModels;
using FantasyModuleParser.Tables.ViewModels.Enums;
using System.Data;
using System;
using FantasyModuleParser.Tables.Views;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace FantasyModuleParser.Tables
{
    /// <summary>
    /// Interaction logic for TableUserControl.xaml
    /// </summary>
    public partial class TableUserControl : UserControl
    {
        private Regex numericValidationRegex = new Regex("[^0-9]+");

        private TableOptionViewModel tableOptionViewModel;

        public TableUserControl()
        {
            InitializeComponent();
            tableOptionViewModel = DataContext as TableOptionViewModel;
            generateContextMenu();
        }

        private void InitializeTableDataGrid()
        {
            //TableExampleDataGrid.BorderColor = System.Drawing.Color.Black;
            //TableExampleDataGrid.CellPadding = 3;
            TableExampleDataGrid.AutoGenerateColumns = true;
            TableExampleDataGrid.CanUserSortColumns = false;
            TableExampleDataGrid.CanUserReorderColumns = false;

            // Set the styles for the DataGrid.
            //TableExampleDataGrid.HeaderStyle.BackColor =
            //    System.Drawing.Color.FromArgb(0x0000aaaa);

            // Create the columns for the DataGrid control. The DataGrid
            // columns are dynamically generated. Therefore, the columns   
            // must be re-created each time the page is refreshed.

            //TableOptionViewModel tableOptionViewModel = DataContext as TableOptionViewModel;

            // Create and add the columns to the collection.
            //TableExampleDataGrid.Columns.Clear();
            //TableExampleDataGrid.Columns.Add(CreateBoundColumn("From", "From"));
            //TableExampleDataGrid.Columns.Add(CreateBoundColumn("To", "To"));

            //for(int colIdx = 2; colIdx < tableOptionViewModel.TableModel.ColumnHeaderLabels.Count; colIdx++)
            //{
            //    TableExampleDataGrid.Columns.Add(CreateBoundColumn($"Col{colIdx}", tableOptionViewModel.TableModel.ColumnHeaderLabels[colIdx]));
            //}
        }

        private void generateContextMenu()
        {

            TableExampleDataGrid.ContextMenu = new ContextMenu();
            MenuItem changeColumnHeaderValueMenuItem = new MenuItem();
            changeColumnHeaderValueMenuItem.Header = "Change Column Header Value";
            changeColumnHeaderValueMenuItem.Click += ChangeColumnHeaderValueMenuItem_Click;
            TableExampleDataGrid.ContextMenu.Items.Add(changeColumnHeaderValueMenuItem);
            TableExampleDataGrid.ContextMenu.Items.Add(new Separator());


            //TableOptionViewModel tableOptionViewModel = DataContext as TableOptionViewModel;
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
            deleteRowMenuItem.Click += DeleteRowMenuItem_Click;
            // NOTE:  For some reason, CurrentItem is not updated when invoking the ICommand function
            //        But, it is available through the code-behind call to DeleteRowMenuItem_Click
            //deleteRowMenuItem.Command = tableOptionViewModel.DeleteRowCommand;
            //deleteRowMenuItem.CommandParameter = TableExampleDataGrid.CurrentItem;
            TableExampleDataGrid.ContextMenu.Items.Add(deleteRowMenuItem);

            //MenuItem clearRowMenuItem = new MenuItem();
            //clearRowMenuItem.Header = "Clear Row";
            //clearRowMenuItem.Command = tableOptionViewModel.ClearRowCommand;
            //TableExampleDataGrid.ContextMenu.Items.Add(clearRowMenuItem);

            TableExampleDataGrid.ContextMenu.Items.Add(new Separator());

            MenuItem insertColumnMenuItem = new MenuItem();
            insertColumnMenuItem.Header = "Insert Column";
            //insertColumnMenuItem.Click += InsertColumn_Click;
            insertColumnMenuItem.Command = tableOptionViewModel.InsertColumnCommand;
            TableExampleDataGrid.ContextMenu.Items.Add(insertColumnMenuItem);

            MenuItem deleteColumnMenuItem = new MenuItem();
            deleteColumnMenuItem.Header = "Delete Column";
            //deleteColumnMenuItem.Click += DeleteColumnMenuItem_Click;
            deleteColumnMenuItem.Command = tableOptionViewModel.RemoveColumnCommand;
            deleteColumnMenuItem.CommandParameter = TableExampleDataGrid.CurrentColumn;
            TableExampleDataGrid.ContextMenu.Items.Add(deleteColumnMenuItem);

            TableExampleDataGrid.ContextMenu.Items.Add(new Separator());

            MenuItem clearCellMenuItem = new MenuItem();
            clearCellMenuItem.Header = "Clear Cell";
            clearCellMenuItem.Click += ClearCellMenuItem_Click;
            TableExampleDataGrid.ContextMenu.Items.Add(clearCellMenuItem);

        }

        private void ChangeColumnHeaderValueMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ChangeColumnHeaderValueViewAction(TableExampleDataGrid.CurrentColumn);
        }

        private void ChangeColumnHeaderValueViewAction(DataGridColumn dataGridColumn)
        {
            // 1.  Check to see if the dataGridColumn has a display index of 0 or 1 and 
            //      return a message stating that these are static

            if(dataGridColumn.DisplayIndex < 2)
            {
                MessageBox.Show(String.Format("Cannot change the column header :: {0}", dataGridColumn.Header));
                return;
            }

            // 2. Open a popup window requesting to change the column header value
            ChangeColumnHeaderView changeColumnHeaderView = new ChangeColumnHeaderView(dataGridColumn.Header.ToString());
            changeColumnHeaderView.ShowDialog();
            // 3. Apply change to column header
            dataGridColumn.Header = changeColumnHeaderView.ColumnHeaderText;

            // 4. Because column header is not directly bound due to dynamic nature of List, 
            //      need to manually update the Model data
            //tableOptionViewModel.TableModel.ColumnHeaderLabels[dataGridColumn.DisplayIndex] = changeColumnHeaderView.ColumnHeaderText;
            tableOptionViewModel.UpdateColumnHeader(dataGridColumn.DisplayIndex, changeColumnHeaderView.ColumnHeaderText);
        }

        private void ClearCellMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = TableExampleDataGrid.CurrentItem;
            TableExampleDataGrid.SelectedValue = "";
        }

        private void DeleteRowMenuItem_Click(object sender, RoutedEventArgs e)
        {
            tableOptionViewModel.DeleteRow((TableExampleDataGrid.CurrentItem as DataRowView).Row);
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
            //TableOptionViewModel tableOptionViewModel = DataContext as TableOptionViewModel;
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

        private void TableExampleDataGrid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject DepObject = (DependencyObject)e.OriginalSource;

            while ((DepObject != null) && !(DepObject is DataGridColumnHeader)
                && !(DepObject is DataGridRow))
            {
                DepObject = VisualTreeHelper.GetParent(DepObject);
            }

            if (DepObject == null)
            {
                return;
            }

            if (DepObject is DataGridColumnHeader)
            {
                DataGridColumnHeader dataGridColumnHeader = (DataGridColumnHeader)DepObject;
                TableExampleDataGrid.ContextMenu.Visibility = Visibility.Collapsed;

                ChangeColumnHeaderValueViewAction(dataGridColumnHeader.Column);
            }
            else
            {
                TableExampleDataGrid.ContextMenu.Visibility = Visibility.Visible;
            }
        }
    }
}
