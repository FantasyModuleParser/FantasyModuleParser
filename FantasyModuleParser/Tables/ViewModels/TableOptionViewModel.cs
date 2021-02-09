using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC.ViewModel;
using FantasyModuleParser.Tables.Models;
using FantasyModuleParser.Tables.Services;
using System.Collections.ObjectModel;

namespace FantasyModuleParser.Tables.ViewModels
{
    public class TableOptionViewModel : ViewModelBase
    {
        private ITableService _tableService;
        private TableModel _tableModel;
        private ModuleModel _moduleModel;
        private CategoryModel _selectedCategoryModel;
        private ObservableCollection<ObservableCollection<CellModel>> _cells;
        private int _gridHeight = 10;
        private int _gridWidth = 10;
        public TableModel TableModel
        {
            get
            {
                return this._tableModel;
            }
            set
            {
                this._tableModel = value;
                RaisePropertyChanged(nameof(TableModel));
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
        public int GridHeight
        {
            get { return this._gridHeight; }
            set
            {
                this._gridHeight = value;
                RaisePropertyChanged(nameof(GridHeight));
            }
        }
        private int GridWidth
        {
            get { return this._gridWidth; }
            set
            {
                this._gridWidth = value;
                RaisePropertyChanged(nameof(GridWidth));
            }
        }
        public TableOptionViewModel()
        {
            _tableService = new TableService();
        }

        private ObservableCollection<ObservableCollection<CellModel>> CreateCells()
        {
            var cells = new ObservableCollection<ObservableCollection<ICellViewModel>>();
            for (var posRow = 0; posRow < GridHeight; posRow++)
            {
                var row = new ObservableCollection<ICellViewModel>();
                for (var posCol = 0; posCol < GridWidth; posCol++)
                {
                    //var cellViewModel = new CellViewModel(Cell.Empty);
                    //row.Add(cellViewModel);
                }
                cells.Add(row);
            }
            //return cells;
            return null;
        }
    }
}
