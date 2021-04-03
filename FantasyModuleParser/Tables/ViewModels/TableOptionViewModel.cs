using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC.Commands;
using FantasyModuleParser.NPC.ViewModel;
using FantasyModuleParser.Tables.Models;
using FantasyModuleParser.Tables.Services;
using FantasyModuleParser.Tables.ViewModels.Enums;
using log4net;
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
        private ModuleModel _moduleModel;
        private CategoryModel _selectedCategoryModel;
        private DataTable _dataTable;
        private ObservableCollection<DataGridColumn> _dataGridColumns;

        public TableModel TableModel
        {
            get { return this._tableModel; }
            set { Set(ref _tableModel, value); }
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

        public int RowCount
        {
            get { return _tableModel.RowCount; }
            set { Set(ref _tableModel.RowCount, value); }
        }
        public int ColumnCount
        {
            get { return _tableModel.ColumnCount; }
            set { Set(ref _tableModel.ColumnCount, value); }
        }

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
            CreateDefaultDataTable();

            TableDataView = new DataView(Data);
            //_dataGridColumns = new ObservableCollection<DataGridColumn>();
            //ChangeGridDimesions();
            //CreateTable();
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

            for(int idx = 2; idx < TableModel.ColumnHeaderLabels.Count; idx++)
            {
                Data.Columns.Add(new DataColumn($"Col{idx}", typeof(string)));
            }

            //3.  With the columns defined, now move the gridData into the DataTable
            // NOTE:  Even if the GridData in TableModel has more columns of data, it will only read upto the 
            // number of defined columns above in ColumnHeaderLabels.
            //
            // *** This will result in data loss if not careful when removing columns!!! ***

            foreach(List<string> rowData in TableModel.BasicStringGridData)
            {
                DataRow dr = Data.NewRow();
                for(int rowIdx = 0; rowIdx < TableModel.ColumnHeaderLabels.Count; rowIdx++)
                {
                    // Safety check that the list of strings in rowData does not throw an ArrayOutOfBoundsException
                    
                    if (rowIdx < rowData.Count) 
                    {
                        switch (rowIdx)
                        {
                            case 0:
                                dr["From"] = rowData[rowIdx];
                                break;
                            case 1:
                                dr["To"] = rowData[rowIdx];
                                break;
                            default:
                                dr[$"Col{rowIdx}"] = rowData[rowIdx];
                                break;
                        }
                    }
                }

                Data.Rows.Add(dr);
            }
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
            Data.Columns.Add(new DataColumn("Col" + TableModel.ColumnHeaderLabels.Count, typeof(string)));
            TableModel.ColumnHeaderLabels.Add("");

            TableDataView = new DataView(Data);
        }

        private ActionCommand _removeColumnCommand;
        public ActionCommand RemoveColumnCommand
        {
            get
            {
                if(_removeColumnCommand == null)
                {
                    _removeColumnCommand = new ActionCommand(param => RemoveColumnFromDataTable(param as DataGridColumn));
                }
                return _removeColumnCommand;
            }
        }

        private bool PredicateRemoveColumnFromDataTable(object dataColumn)
        {
            log.Info(dataColumn);
            if(dataColumn == null)
            {
                return false;
            }
            return (dataColumn as DataGridColumn).DisplayIndex> 1;
        }
        private void RemoveColumnFromDataTable(DataGridColumn dataColumn)
        {
            if(dataColumn == null)
            {
                log.Info(string.Format("Cannot delete the column as the param is not a dataColumn type :: {0}", dataColumn));
                return;
            }

            if(dataColumn.DisplayIndex <= 1)
            {
                log.Info(string.Format("Cannot delete column {0} at the index {1}; Continuing on..", dataColumn.Header, dataColumn.DisplayIndex));
                return;
            }
            Data.Columns.RemoveAt(dataColumn.DisplayIndex);
        }


        private void RemoveColumnFromDataTable()
        {

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
    }
}
