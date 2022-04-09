using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Commands;
using FantasyModuleParser.NPC.ViewModel;
using FantasyModuleParser.Tables.Models;
using FantasyModuleParser.Tables.Services;
using FantasyModuleParser.Tables.ViewModels.Enums;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Controls;
using System.Windows.Input;

namespace FantasyModuleParser.Tables.ViewModels
{
    public class TableOptionViewModel : ViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TableOptionViewModel));

        private ITableService _tableService;
        private TableModel _tableModel;
        private TableModel _selectedTableModel;
        private ModuleModel _moduleModel;
        private CategoryModel _selectedCategoryModel;
        private DataTable _dataTable;
        private ObservableCollection<DataGridColumn> _dataGridColumns;
        private ModuleService moduleService;

        public TableModel TableModel
        {
            get => this._tableModel;
            //set { Set(ref _tableModel, value); }
            set
            {
                this._tableModel = value;
                RaisePropertyChanged(nameof(TableModel));
            }
        }
        public TableModel SelectedTableModel
        {
            get => this._selectedTableModel;
            //set { Set(ref _tableModel, value); }
            set
            {
                if(value != null)
                { 
                    this._selectedTableModel = value;
                
                    // This enforces a new copy of the selected table from the project
                    TableModel = CommonMethod.CloneJson(value);
                    TableDataView = new DataView(TableModel.tableDataTable);
                    RaisePropertyChanged(nameof(SelectedTableModel));
                }
            }
        }
        public ModuleModel ModuleModel
        {
            get
            {
                return this._moduleModel;
            }
            set
            {
                this._moduleModel = value;
                RaisePropertyChanged(nameof(ModuleModel));
            }
        }
        public CategoryModel SelectedCategoryModel
        {
            get
            {
                return this._selectedCategoryModel;
            }
            set
            {
                this._selectedCategoryModel = value;
                RaisePropertyChanged(nameof(SelectedCategoryModel));
            }
        }

        public DataTable Data
        {
            get { return _tableModel.tableDataTable; }
            set { this._tableModel.tableDataTable = value; RaisePropertyChanged(nameof(Data)); }
        }

        //public int RowCount
        //{
        //    get { return _tableModel.RowCount; }
        //    set { Set(ref _tableModel.RowCount, value); }
        //}
        //public int ColumnCount
        //{
        //    get { return _tableModel.ColumnCount; }
        //    set { Set(ref _tableModel.ColumnCount, value); }
        //}

        public ObservableCollection<DataGridColumn> DataGridColumns
        {
            get { return _dataGridColumns; }
            set { Set(ref _dataGridColumns, value); }
        }

        private DataView _dataView;
        public DataView TableDataView
        {
            get => _dataView;
            set { Set(ref _dataView, value); }
        }

        // This is an attempt at binding the selected item from the DataGrid on the view
        // as to be used to manipulate the DataView object inside this ViewModel
        private object _selectedItem;
        public object SelectedItem
        {
            get => _selectedItem;
            set { Set(ref _selectedItem, value); }
        }

        public ICommand OnDataGridSizeChangeCommand { get; set; }

        public TableOptionViewModel()
        {
            _tableService = new TableService();
            _tableModel = new TableModel();

            // Load the ModuleModel data
            moduleService = new ModuleService();
            ModuleModel = moduleService.GetModuleModel();

            CreateNewTable();
            //_dataGridColumns = new ObservableCollection<DataGridColumn>();
            //ChangeGridDimesions();
            //CreateTable();
        }

        public void Refresh()
        {
            ModuleModel = moduleService.GetModuleModel();
        }

        public void CreateDefaultDataTable()
        {
            Data = new DataTable();

            // The Default Grid consists of three columns and one row:

            // | From | To | <blank but editable> |
            // | Row1 | -- | -------------------- |

            // First, Load the TableModel contents into the TableModel object
            TableModel.Load();

            // 2. Create the DataColumns based on the number of entries into the TableModel.ColumnHeaderLabel list
            // 2a.  There are always two columns;  From & To  (indicating dice roll range)

            Data.Columns.Add(new DataColumn("From", typeof(int)));
            Data.Columns.Add(new DataColumn("To", typeof(int)));

            //for(int idx = 2; idx < TableModel.tableDataTable.Columns.Count; idx++)
            //{
                Data.Columns.Add(new DataColumn($"Right Click to Change", typeof(string)));
            //}

            //3.  With the columns defined, now move the gridData into the DataTable
            // NOTE:  Even if the GridData in TableModel has more columns of data, it will only read upto the 
            // number of defined columns above in ColumnHeaderLabels.
            //
            // *** This will result in data loss if not careful when removing columns!!! ***

            //foreach(List<string> rowData in TableModel.BasicStringGridData)
            //{
            //    DataRow dr = Data.NewRow();
            //    for(int rowIdx = 0; rowIdx < TableModel.tableDataTable.Columns.Count; rowIdx++)
            //    {
            //        // Safety check that the list of strings in rowData does not throw an ArrayOutOfBoundsException
                    
            //        if (rowIdx < rowData.Count) 
            //        {
            //            switch (rowIdx)
            //            {
            //                case 0:
            //                    dr["From"] = rowData[rowIdx];
            //                    break;
            //                case 1:
            //                    dr["To"] = rowData[rowIdx];
            //                    break;
            //                default:
            //                    dr[$"Col{rowIdx}"] = rowData[rowIdx];
            //                    break;
            //            }
            //        }
            //    }

            //    Data.Rows.Add(dr);
            //}
        }

        #region Commands
        ActionCommand _newTableCommand;
        public ICommand NewTableCommand
        {
            get
            {
                if (_newTableCommand == null)
                {
                    _newTableCommand = new ActionCommand(param => CreateNewTable());
                }
                return _newTableCommand;
            }
        }
        private void CreateNewTable()
        {
            SelectedTableModel = new TableModel();
            //TableModel = new TableModel();
            CreateDefaultDataTable();
            TableDataView = new DataView(Data);
        }

        // Experimenting with the ActionCommand as a delegate for the button clicks from the View
        ActionCommand _insertRowCommand;
        public ICommand InsertRowCommand
        {
            get
            {
                if(_insertRowCommand == null)
                {
                    _insertRowCommand = new ActionCommand(param => TableDataView.AddNew());
                }
                return _insertRowCommand;
            }
        }

        ActionCommand _deleteRowCommand;
        public ICommand DeleteRowCommand
        {
            get
            {
                if (_deleteRowCommand == null)
                {
                    _deleteRowCommand = new ActionCommand(param => attemptToDeleteLastRow(param),
                        param => TableDataView.Count > 0);
                    //TODO:  Can add a Predicate command to the ActionCommand, but not sure how that works in practice.....
                }
                return _deleteRowCommand;
            }
        }

        ActionCommand _addColumnCommand;
        public ICommand AddColumnCommand
        {
            get
            {
                if (_addColumnCommand == null)
                {
                    _addColumnCommand = new ActionCommand(param => AddColumnToDataTable());
                }
                return _addColumnCommand;
            }
        }
        ActionCommand _addToProjectCommand;
        public ICommand AddToProjectCommand
        {
            get
            {
                if(_addToProjectCommand == null)
                {
                    _addToProjectCommand = new ActionCommand(param => AddTableToProject());
                }
                return _addToProjectCommand;
            }
        }

        private void AddTableToProject()
        {

            if (SelectedCategoryModel != null) 
            {
                if(SelectedCategoryModel.TableModels == null)
                {
                    SelectedCategoryModel.TableModels = new ObservableCollection<TableModel>();
                }

                moduleService.AddTableToCategory(TableModel, SelectedCategoryModel.Name);
                SelectedTableModel = SelectedCategoryModel.TableModels[SelectedCategoryModel.TableModels.IndexOf(TableModel)];
            }
        }

        ActionCommand _prevTableCommand;
        public ICommand PrevTableCommand
        {
            get
            {
                if (_prevTableCommand == null)
                {
                    _prevTableCommand = new ActionCommand(param => SelectPreviousTable(), 
                        param => SelectedCategoryModel != null && SelectedCategoryModel.TableModels != null && SelectedCategoryModel.TableModels.Count > 0);
                }
                return _prevTableCommand;
            }
        }
        private void SelectPreviousTable()
        {
            if(SelectedCategoryModel != null && SelectedCategoryModel.TableModels != null && SelectedCategoryModel.TableModels.Count > 0)
            {
                // Get index count from the current table
                int tableModelIndex = SelectedCategoryModel.TableModels.IndexOf(SelectedTableModel);
                // Either the index is 0 or -1, in which case grab the last TableModel in the collection
                if(tableModelIndex < 1)
                {
                    SelectedTableModel = SelectedCategoryModel.TableModels[SelectedCategoryModel.TableModels.Count - 1];
                }
                else
                {
                    SelectedTableModel = SelectedCategoryModel.TableModels[tableModelIndex - 1];
                }
            }
        }

        ActionCommand _nextTableCommand;
        public ICommand NextTableCommand
        {
            get
            {
                if (_nextTableCommand == null)
                {
                    _nextTableCommand = new ActionCommand(param => SelectNextTable(),
                        param => SelectedCategoryModel != null && SelectedCategoryModel.TableModels != null && SelectedCategoryModel.TableModels.Count > 0);
                }
                return _nextTableCommand;
            }
        }
        private void SelectNextTable()
        {
            if (SelectedCategoryModel != null && SelectedCategoryModel.TableModels != null && SelectedCategoryModel.TableModels.Count > 0)
            {
                // Get index count from the current table
                int tableModelIndex = SelectedCategoryModel.TableModels.IndexOf(SelectedTableModel);
                // Either the index is 0 or -1, in which case grab the last TableModel in the collection
                if (tableModelIndex >= (SelectedCategoryModel.TableModels.Count - 1))
                {
                    SelectedTableModel = SelectedCategoryModel.TableModels[0];
                }
                else
                {
                    SelectedTableModel = SelectedCategoryModel.TableModels[tableModelIndex + 1];
                }
            }
        }

        #endregion

        private void attemptToDeleteLastRow(object param)
        {
                TableDataView.Delete(TableDataView.Count - 1);
        }

        public void DeleteRow(DataRow dataRow)
        {
            TableDataView.Table.Rows.Remove(dataRow);
        }

        private ActionCommand _insertColumnCommand;
        public ActionCommand InsertColumnCommand
        {
            get
            {
                if(_insertColumnCommand == null)
                {
                    _insertColumnCommand = new ActionCommand(param => AddColumnToDataTable());
                }
                return _insertColumnCommand;
            }
        }
        private void AddColumnToDataTable()
        {
            Data.Columns.Add(new DataColumn("Col" + TableModel.tableDataTable.Columns.Count, typeof(string)));
            //TableModel.tableDataTable.Columns.Add("");

            TableDataView = new DataView(Data);
        }

        private ActionCommand _removeColumnCommand;
        public ActionCommand RemoveColumnCommand
        {
            get
            {
                if(_removeColumnCommand == null)
                {
                    _removeColumnCommand = new ActionCommand(param => RemoveColumnFromDataTable(),
                        param => PredicateRemoveColumnFromDataTable());
                }
                return _removeColumnCommand;
            }
        }

        private bool PredicateRemoveColumnFromDataTable()
        {
            return Data.Columns.Count > 3;
        }
        private void RemoveColumnFromDataTable(DataGridColumn dataColumn)
        {
            if (dataColumn == null)
            {
                log.Info(string.Format("Cannot delete the column as the param is not a dataColumn type :: {0}", dataColumn));
                return;
            }

            if (dataColumn.DisplayIndex <= 1)
            {
                log.Info(string.Format("Cannot delete column {0} at the index {1}; Continuing on..", dataColumn.Header, dataColumn.DisplayIndex));
                return;
            }
            Data.Columns.RemoveAt(dataColumn.DisplayIndex);
        }

        private void RemoveColumnFromDataTable()
        {
            if(Data.Columns.Count > 3) {

                Data.Columns.RemoveAt(Data.Columns.Count - 1);
                //TableModel.tableDataTable.Columns.RemoveAt(Data.Columns.Count - 1);

                TableDataView = new DataView(Data);
                

                //TableDataView.Dispose();


                RaisePropertyChanged(nameof(TableDataView));
               
            }
        }

        ActionCommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                { 
                    _saveCommand = new ActionCommand(param => _tableModel.Save());
                }
                return _saveCommand;
            }
        }

        private ActionCommand _loadTableCommand;
        public ActionCommand LoadTableCommand
        {
            get
            {
                if (_loadTableCommand == null)
                {
                    _loadTableCommand = new ActionCommand(param => LoadTableAction());
                }
                return _loadTableCommand;
            }
        }
        private void LoadTableAction()
        {
            //Data.Columns.Add(new DataColumn("Col" + TableModel.ColumnHeaderLabels.Count, typeof(string)));
            //TableModel.ColumnHeaderLabels.Add("");

            //TableDataView = new DataView(Data);

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
                TableModel = TableModel.Load(openFileDlg.FileName);
                TableDataView = new DataView(TableModel.tableDataTable);
            }
        }

        public void UpdateColumnHeader(int columnIndex, String newColumnValue)
        {
            TableDataView.Table.Columns[columnIndex].ColumnName = newColumnValue;
        }
    }
}
