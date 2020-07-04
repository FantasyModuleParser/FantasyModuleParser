using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.Models;
using FantasyModuleParser.NPC.ViewModel;
using System;
using System.Collections.Generic;
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
            string appendedFileName = folderPath + "\\" + moduleModel.ModFilename + ".fpm";
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
