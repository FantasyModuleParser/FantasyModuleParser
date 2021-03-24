using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC.Commands;
using FantasyModuleParser.NPC.ViewModel;
using FantasyModuleParser.Tables.Models;
using FantasyModuleParser.Tables.Services;
using FantasyModuleParser.Tables.ViewModels.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Controls;
using System.Windows.Input;

namespace FantasyModuleParser.Tables.ViewModels
{
    public class TableOptionViewModel : ViewModelBase
    {
        private ITableService _tableService;
        private TableModel _tableModel;
        private ModuleModel _moduleModel;
        private CategoryModel _selectedCategoryModel;
        private ObservableCollection<ObservableCollection<ICellViewModel>> _cells;
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
        public ObservableCollection<ObservableCollection<ICellViewModel>> Cells
        {
            get { return this._cells; }
            set
            {
                this._cells = value;
            }
        }

        public DataTable Data
        {
            get { return _tableModel.tableDataTable; }
            set { this._tableModel.tableDataTable = value; RaisePropertyChanged(nameof(Data)); }
        }

        public string Name
        {
            get { return _tableModel.Name; }
            set { Set(ref _tableModel.Name, value); }
        }

        public string Description
        {
            get { return _tableModel.Description; }
            set { Set(ref _tableModel.Description, value); }
        }

        public OutputTypeEnum OutputType
        {
            get { return _tableModel.OutputType; }
            set { Set(ref _tableModel.OutputType, value); }
        }

        public RollMethodEnum RollMethod
        {
            get { return _tableModel.RollMethod; }
            set { Set(ref _tableModel.RollMethod, value); }
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

        private DataMatrix sourceCollection;
        public DataMatrix SourceCollection
        {
            get => sourceCollection;
            set { Set(ref sourceCollection, value); }
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

        ActionCommand _clearRowCommand;
        public ICommand ClearRowCommand
        {
            get
            {
                if (_clearRowCommand == null)
                {
                    _clearRowCommand = new ActionCommand(param => attemptToClearLastRow(),
                        param => TableDataView.Count > 0);
                    //TODO:  Can add a Predicate command to the ActionCommand, but not sure how that works in practice.....
                }
                return _clearRowCommand;
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
                    //TODO:  Can add a Predicate command to the ActionCommand, but not sure how that works in practice.....
                }
                return _addColumnCommand;
            }
        }

        private void attemptToDeleteLastRow(object param)
        {
                TableDataView.Delete(TableDataView.Count - 1);
        }

        private void attemptToClearLastRow()
        {
            if (TableDataView.Count > 0)
                TableDataView.Delete(TableDataView.Count - 1);
        }

        private void AddColumnToDataTable()
        {
            Data.Columns.Add(new DataColumn("Col" + TableModel.ColumnHeaderLabels.Count, typeof(string)));
            TableModel.ColumnHeaderLabels.Add("");

            TableDataView = new DataView(Data);

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
