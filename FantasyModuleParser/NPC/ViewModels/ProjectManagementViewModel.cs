using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.NPC.ViewModels
{
    public class ProjectManagementViewModel : ViewModelBase
    {
        private ModuleService moduleService;
        public ModuleModel ModuleModel { get; set; }
        public ProjectManagementViewModel()
        {
            moduleService = new ModuleService();
            ModuleModel = moduleService.GetModuleModel();
        }

        public void UpdateModule()
        {
            moduleService.UpdateModuleModel(ModuleModel);
        }

        public void SaveModule(string folderPath, ModuleModel moduleModel)
        {
            // If the module has no Categories set, then default the first one to the Module Title
            if(moduleModel.Categories == null || moduleModel.Categories.Count == 0)
            {
                moduleModel.Categories = new ObservableCollection<CategoryModel>();
                moduleModel.Categories.Add(new CategoryModel() { Name = moduleModel.Name });
            }

            string appendedFileName = folderPath + "\\" + moduleModel.ModFilename + ".fmp";
            moduleModel.SaveFilePath = folderPath;
            moduleService.Save(appendedFileName, moduleModel);
        }

        public void LoadModule(string filePath)
        {
            moduleService.Load(filePath);
            ModuleModel = moduleService.GetModuleModel();
            RaisePropertyChanged("ModuleModel");
        }
    }
}
