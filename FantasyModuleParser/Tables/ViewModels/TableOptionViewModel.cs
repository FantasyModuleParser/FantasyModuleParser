using FantasyModuleParser.Main.Models;
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
            get { return _dataTable; }
            set { this._dataTable = value; RaisePropertyChanged(nameof(Data)); }
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

        // Preset Range Modifiers
        public int PresetRangeMinimum
        {
            get => _tableModel.presetRollMinimum;
            set { Set(ref _tableModel.presetRollMinimum, value); }
        }
        public int PresetRangeMaximum
        {
            get => _tableModel.presetRollMaximum;
            set { Set(ref _tableModel.presetRollMaximum, value); }
        }

        public ICommand OnDataGridSizeChangeCommand { get; set; }

        public TableOptionViewModel()
        {
            _tableService = new TableService();
            _tableModel = new TableModel();
            CreateDefaultDataTable();
            _dataGridColumns = new ObservableCollection<DataGridColumn>();
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
                Data.Columns.Add(new DataColumn($"[{idx}]", typeof(int)));
            }

            //3.  With the columns defined, now move the gridData into the DataTable
            // NOTE:  Even if the GridData in TableModel has more columns of data, it will only read upto the 
            // number of defined columns above.  *** This will result in data loss if not careful!!! ***

            foreach(List<string> rowData in TableModel.BasicStringGridData)
            {
                DataRow dr = Data.NewRow();
                for(int rowIdx = 0; rowIdx < TableModel.ColumnHeaderLabels.Count; rowIdx++)
                {
                    // Safety check that the list of strings in rowData does not throw an ArrayOutOfBoundsException
                    if (rowData.Count < rowIdx)
                        dr[rowIdx] = rowData[rowIdx];
                }

                Data.Rows.Add(dr);
            }
        }

        public void ChangeGridDimesions()
        {

            // 1. Because DataTable requires to clear out 
            Data = new DataTable();
            ResetColumns();
            DataGridColumns.Clear();
            
            if (_tableModel != null)
            {
                for (var posCol = 0; posCol < _tableModel.ColumnCount; posCol++)
                {
                    //if (!Data.Columns.Contains($"Column { posCol }"))
                    //    Data.Columns.Add($"Column { posCol }");
                    //var row = new ObservableCollection<ICellViewModel>();

                    List<string> recordInfo = new List<string>();
                    for (var posRow = 0; posRow < _tableModel.RowCount; posRow++)
                    {
                        // If this loop is on the first outer iteration, then it's the "header" row, where all
                        // columns are defined
                        if (posCol == 0)
                        {
                            //string columnName = posCol >= row.Count ? "" + posCol : row[posCol].Content;
                            //if (!Data.Columns.Contains(columnName))
                            //{ Data.Columns.Add(columnName); }
                            //DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
                            //dataGridTextColumn.Header = columnName;
                            //DataGridColumns.Add(dataGridTextColumn);
                            //Data.Rows.Add($"Row { posCol }");
                            DataRow dataRow = Data.NewRow();
                            dataRow[posCol] = $"Row {posRow}";
                            Data.Rows.Add(dataRow);
                        }
                        else
                        {
                            //recordInfo.Add(posCol >= row.Count ? "" + posCol : row[posCol].Content);
                            DataRow dataRow = Data.Rows[posRow];

                            if (dataRow != null)
                            {
                                dataRow[$"Column { posCol }"] = $"Row {posRow}";
                            }
                            else
                            {
                                dataRow = Data.NewRow();
                                dataRow[$"Column { posCol }"] = $"Row {posRow}";
                                Data.Rows.Add(dataRow);
                            }


                        }
                    }
                    //Data.Rows.Add(recordInfo.ToArray());
                }
                //Data.AcceptChanges();
                
            }

            OnPropertyChanged(nameof(Data));
        }

        private static readonly DataTable _dt = new DataTable();
        public void AddColumn(string columnName)
        {
            var temp = this.Data;
            this.Data = _dt;
            temp.Columns.Clear();
            temp.Columns.Add(columnName);
            this.Data = temp;

        }

        private void ResetColumns()
        {
            var temp = this.Data;
            this.Data = _dt;
            temp.Columns.Clear();
            for (var posCol = 0; posCol < _tableModel.ColumnCount; posCol++)
            {
                if (!temp.Columns.Contains($"Column { posCol }"))
                {
                    DataColumn dataColumn = new DataColumn($"Column { posCol }");
                    dataColumn.Caption = "";
                    temp.Columns.Add(dataColumn);
                }
            }
            this.Data = temp;
        }

        private ObservableCollection<ObservableCollection<ICellViewModel>> CreateCells()
        {
            var cells = new ObservableCollection<ObservableCollection<ICellViewModel>>();
            for (var posRow = 0; posRow < RowCount; posRow++)
            {
                var row = new ObservableCollection<ICellViewModel>();
                for (var posCol = 0; posCol < ColumnCount; posCol++)
                {
                    var cellViewModel = new CellViewModel("");
                    row.Add(cellViewModel);
                }
                cells.Add(row);
            }
            return cells;
        }


        private DataMatrix generateData()
        {
            List<object[]> rows = new List<object[]>();
            for (int i = 0; i < RowCount; i++)
            {
                var line = new object[ColumnCount];
                for (int j = 0; j < ColumnCount; j++)
                {
                    line[j] = ($"Data Entry Line: {i + 1} Entry: {j + 1}");
                }
                rows.Add(line);
            }

            List<string> columns = new List<string>();
            for (int i = 0; i < ColumnCount; i++)
            {
                //columns.Add(new MatrixColumn() { Name = $"Column {i + 1}" });
                columns.Add($"Column {i + 1}");
            }
            return new DataMatrix() { Columns = columns, Rows = rows };
        }

        public void CreateTable()
        {
            SourceCollection = generateData();
        }

    }
}
