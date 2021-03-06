using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC.ViewModel;
using FantasyModuleParser.Tables.Models;
using FantasyModuleParser.Tables.Services;
using FantasyModuleParser.Tables.ViewModels.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
        private int _gridHeight = 5;
        private int _gridWidth = 5;
        private DataTable _dataTable;
        private string _name;

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

        public int GridHeight
        {
            get { return _tableModel.GridHeight; }
            set { Set(ref _tableModel.GridHeight, value); }
        }
        public int GridWidth
        {
            get { return _tableModel.GridWidth; }
            set { Set(ref _tableModel.GridWidth, value); }
        }


        public ICommand OnDataGridSizeChangeCommand { get; set; }

        public TableOptionViewModel()
        {
            _tableService = new TableService();
            _tableModel = new TableModel();
            Data = new DataTable();
            UpdateDataGrid();
        }

        public void UpdateDataGrid()
        {
            Data.Clear();
            if (_tableModel != null)
            {
                for (var posRow = 0; posRow < _tableModel.GridHeight; posRow++)
                {
                    var row = new ObservableCollection<ICellViewModel>();

                    List<string> recordInfo = new List<string>();
                    for (var posCol = 0; posCol < _tableModel.GridWidth; posCol++)
                    {
                        // If this loop is on the first outer iteration, then it's the "header" row, where all
                        // columns are defined
                        if (posRow == 0)
                        {
                            string columnName = posCol >= row.Count ? "" + posCol : row[posCol].Content;
                            if (!Data.Columns.Contains(columnName))
                            { Data.Columns.Add(columnName); }
                        }
                        else
                        {
                            recordInfo.Add(posCol >= row.Count ? "" + posCol : row[posCol].Content);
                        }
                    }
                    Data.Rows.Add(recordInfo.ToArray());
                }
            }

        }

        private ObservableCollection<ObservableCollection<ICellViewModel>> CreateCells()
        {
            var cells = new ObservableCollection<ObservableCollection<ICellViewModel>>();
            for (var posRow = 0; posRow < GridHeight; posRow++)
            {
                var row = new ObservableCollection<ICellViewModel>();
                for (var posCol = 0; posCol < GridWidth; posCol++)
                {
                    var cellViewModel = new CellViewModel("");
                    row.Add(cellViewModel);
                }
                cells.Add(row);
            }
            return cells;
        }


    }
}
