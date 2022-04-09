using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.Equipment.Models;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Commands;
using FantasyModuleParser.NPC.ViewModel;
using FantasyModuleParser.Spells.Models;
using FantasyModuleParser.Tables.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FantasyModuleParser.Main.ViewModels
{
    public class CategoriesUCViewModel : ViewModelBase
    {
        public ModuleModel ModuleModel { get; set; }
        private ModuleService _moduleService;

        private CategoryModel _categoryModel;
        public new CategoryModel SelectedCategoryModel
        {
            get { return _categoryModel; }
            set { SetProperty(ref _categoryModel, value);
                UpdatePopulatedModuleList();
                
            }
        }

        //TODO:  This would need to be updated every time a new Module was entered into FMP
        //          I don't like manual, but hard to think of a workaround at this time...
        private const string NPC_MODULE = "NPCs",
            SPELL_MODULE = "Spells",
            TABLE_MODULE = "Tables",
            EQUIPMENT_MODULE = "Equipment",
            ClASSES_MODULE = "Classes";

        private ObservableCollection<string> _populatedModuleList;
        public ObservableCollection<string> PopulatedModuleList
        {
            get { return _populatedModuleList;}
            set { SetProperty(ref _populatedModuleList, value); }
        }

        private string _selectedPopulatedModuleList;
        public string SelectedPopulatedModuleList 
        {
            get { return _selectedPopulatedModuleList;}
            set { Set(ref _selectedPopulatedModuleList, value);
                UpdateSelectedCategoryModuleRecords();
                    }
        }

        private ObservableCollection<ModelBase> _selectedCategoryModuleRecords;
        public ObservableCollection<ModelBase> SelectedCategoryModuleRecords
        {
            get { return _selectedCategoryModuleRecords;}
            set { Set(ref _selectedCategoryModuleRecords, value);
            }
        }

        public CategoriesUCViewModel()
        {
            _moduleService = new ModuleService();
            ModuleModel = _moduleService.GetModuleModel();
            //ModuleModel.Categories = new System.Collections.ObjectModel.ObservableCollection<CategoryModel>();
            //ModuleModel.Categories.Add(new CategoryModel(){ Name = "TEESTS" });
            PopulatedModuleList = new ObservableCollection<string>();
            //PopulatedModuleList.Add("NPC Models");
            SelectedCategoryModuleRecords = new ObservableCollection<ModelBase>();
        }

        public void AddCategoryToModule(string categoryName)
        {
            if (categoryName.Length != 0)
            {
                CategoryModel categoryModel = ModuleModel.Categories.FirstOrDefault(item => item.Name.Equals(categoryName));
                if (categoryModel == null)
                {
                    ModuleModel.Categories.Add(new CategoryModel() { Name = categoryName });
                }
            }

            RaisePropertyChanged("Categories");
        }

        private ICommand _removeCategoryCommand;
        public ICommand RemoveCategoryCommand
        {
            get
            {
                if (_removeCategoryCommand == null)
                {
                    _removeCategoryCommand = new ActionCommand(param => OnRemoveCategoryCommand(param),
                        param => ModuleModel.Categories.Count > 1);
                }
                return _removeCategoryCommand;
            }
        }

        protected virtual void OnRemoveCategoryCommand(object param)
        {
            CategoryModel selectedCategory = (CategoryModel)param;
            string confirmMessage = string.Format("Are you sure you want to remove the category {0}?\n\nTHIS ACTION IS NOT REVERSABLE!!!", selectedCategory.Name);
            MessageBoxResult result = MessageBox.Show(confirmMessage, "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ModuleModel.Categories.Remove(selectedCategory);
            }

        }

        private void UpdatePopulatedModuleList()
        {
            if(this.SelectedCategoryModel != null)
            {
                if (this.SelectedCategoryModel.NPCModels?.Count > 0)
                    this.PopulatedModuleList.Add(NPC_MODULE);
                if (this.SelectedCategoryModel.SpellModels?.Count > 0)
                    this.PopulatedModuleList.Add(SPELL_MODULE);
                if (this.SelectedCategoryModel.TableModels?.Count > 0)
                    this.PopulatedModuleList.Add(TABLE_MODULE);
                if (this.SelectedCategoryModel.EquipmentModels?.Count > 0)
                    this.PopulatedModuleList.Add(EQUIPMENT_MODULE);
                if (this.SelectedCategoryModel.ClassModels?.Count > 0)
                    this.PopulatedModuleList.Add(ClASSES_MODULE);
            } else
            {
                this.PopulatedModuleList.Clear();
            }

            RaisePropertyChanged(nameof(PopulatedModuleList));
        }
        private void UpdateSelectedCategoryModuleRecords()
        {

            SelectedCategoryModuleRecords.Clear();
            switch (SelectedPopulatedModuleList)
            {
                case NPC_MODULE:
                    foreach (ModelBase modelBase in this.SelectedCategoryModel.NPCModels)
                        this.SelectedCategoryModuleRecords.Add(modelBase);
                    break;
                case SPELL_MODULE:
                    foreach (ModelBase modelBase in this.SelectedCategoryModel.SpellModels)
                        this.SelectedCategoryModuleRecords.Add(modelBase);
                    break;
                case TABLE_MODULE:
                    foreach (ModelBase modelBase in this.SelectedCategoryModel.TableModels)
                        this.SelectedCategoryModuleRecords.Add(modelBase);
                    break;
                case EQUIPMENT_MODULE:
                    foreach (ModelBase modelBase in this.SelectedCategoryModel.EquipmentModels)
                        this.SelectedCategoryModuleRecords.Add(modelBase);
                    break;
                case ClASSES_MODULE:
                    foreach (ModelBase modelBase in this.SelectedCategoryModel.ClassModels)
                        this.SelectedCategoryModuleRecords.Add(modelBase);
                    break;
            }
        }


        private ICommand _removeModelBaseFromSelectedCategoryCommand;
        public ICommand RemoveModelBaseFromSelectedCategoryCommand
        {
            get
            {
                if (_removeModelBaseFromSelectedCategoryCommand == null)
                {
                    _removeModelBaseFromSelectedCategoryCommand = 
                        new ActionCommand(param => OnRemoveModelBaseFromSelectedCategoryCommand(param));
                }
                return _removeModelBaseFromSelectedCategoryCommand;
            }
        }

        protected virtual void OnRemoveModelBaseFromSelectedCategoryCommand(object param)
        {
            switch (SelectedPopulatedModuleList)
            {
                case NPC_MODULE:
                    this.SelectedCategoryModel.NPCModels.Remove(param as NPCModel);
                    break;
                case SPELL_MODULE:
                    this.SelectedCategoryModel.SpellModels.Remove(param as SpellModel);
                    break;
                case TABLE_MODULE:
                    this.SelectedCategoryModel.TableModels.Remove(param as TableModel);
                    break;
                case EQUIPMENT_MODULE:
                    this.SelectedCategoryModel.EquipmentModels.Remove(param as EquipmentModel);
                    break;
                case ClASSES_MODULE:
                    this.SelectedCategoryModel.ClassModels.Remove(param as ClassModel);
                    break;
            }

            _moduleService.Save(ModuleModel);
            UpdateSelectedCategoryModuleRecords();
        }

    }
}
