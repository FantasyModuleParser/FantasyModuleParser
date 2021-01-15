using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC.ViewModel;
using FantasyModuleParser.Tables.Models;
using FantasyModuleParser.Tables.Services;

namespace FantasyModuleParser.Tables.ViewModels
{
    public class TableOptionViewModel : ViewModelBase
    {
        private ITableService _tableService;
        private TableModel _tableModel;
        private ModuleModel _moduleModel;
        private CategoryModel _selectedCategoryModel;
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
        public TableOptionViewModel()
        {
            _tableService = new TableService();
        }
    }
}
