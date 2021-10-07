using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.Commands;
using FantasyModuleParser.NPC.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FantasyModuleParser.Main.ViewModels
{
    public class CategoriesUCViewModel : ViewModelBase
    {
        public ModuleModel ModuleModel { get; set; }
        private ModuleService _moduleService;
        public CategoriesUCViewModel()
        {
            _moduleService = new ModuleService();
            ModuleModel = _moduleService.GetModuleModel();
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
    }
}
