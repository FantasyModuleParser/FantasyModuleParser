using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.NPC.ViewModels
{
    public class NPCOptionControlViewModel : ViewModelBase
    {
        private ModuleService _moduleService;
        public ModuleModel ModuleModel { get; set; }
        public ObservableCollection<CategoryModel> ViewModelCategories { get; set; }
        public NPCOptionControlViewModel()
        {
            _moduleService = new ModuleService();
            ModuleModel = _moduleService.GetModuleModel();
            ViewModelCategories = ModuleModel.Categories;
        }
        public void Refresh()
        {
            ModuleModel = _moduleService.GetModuleModel();
            ViewModelCategories = ModuleModel.Categories;
            RaisePropertyChanged("NPCOptionControlViewModel");
        }

        public void AddNPCToCategory(NPCModel npcModel, string categoryValue)
        {
            if (categoryValue == null || categoryValue.Length == 0)
                throw new InvalidDataException("Category value is null;  Cannot save NPC");
            if (npcModel == null)
                throw new InvalidDataException("NPC Model data object is null");

            ModuleModel _moduleModel = _moduleService.GetModuleModel();
            CategoryModel categoryModel = _moduleModel.Categories.FirstOrDefault(item => item.Name.Equals(categoryValue));
            if (categoryModel == null)
                throw new InvalidDataException("Category Value is not in the Module Model data object!");
            else
                categoryModel.NPCModels.Add(npcModel);  // The real magic is here
        }
    }
}
