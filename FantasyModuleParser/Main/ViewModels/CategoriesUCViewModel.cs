using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    }
}
