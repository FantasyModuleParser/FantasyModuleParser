using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.ViewModel;
using System.Collections.ObjectModel;

namespace FantasyModuleParser.NPC.ViewModels
{
    public class ProjectManagementViewModel : ViewModelBase
    {
        private ModuleService moduleService;
        private SettingsService settingsService;
        private string _fullModulePath;
        public string FullModulePath { 
            get
            {
                return _fullModulePath;
            }
            set
            {
                _fullModulePath = value;
                RaisePropertyChanged(nameof(FullModulePath));
            }
        }
        public SettingsModel SettingsModel { get; set; }
        public ModuleModel ModuleModel { get; set; }
        public ProjectManagementViewModel()
        {
            moduleService = new ModuleService();
            settingsService = new SettingsService();
            ModuleModel = moduleService.GetModuleModel();
            SettingsModel = settingsService.Load();

            UpdateFullModulePath();

            RaisePropertyChanged(nameof(ModuleModel));
            RaisePropertyChanged(nameof(SettingsModel));
        }

        public void UpdateModule()
        {
            moduleService.UpdateModuleModel(ModuleModel);
            ModuleModel = moduleService.GetModuleModel();
        }
        public void NewModuleSetup()
        {
            moduleService.UpdateModuleModel(new ModuleModel());
            ModuleModel = moduleService.GetModuleModel();
            RaisePropertyChanged(nameof(ModuleModel));
        }

        public void SaveModule(string folderPath, ModuleModel moduleModel)
        {
            // If the module has no Categories set, then default the first one to the Module Title
            if(moduleModel.Categories == null || moduleModel.Categories.Count == 0)
            {
                moduleModel.Categories = new ObservableCollection<CategoryModel>();
                moduleModel.Categories.Add(new CategoryModel() { Name = moduleModel.Name });
            }

            string appendedFileName = settingsService.Load().ProjectFolderLocation + "\\" + moduleModel.ModFilename + ".fmp";
            moduleService.Save(appendedFileName, moduleModel);
        }

        public void LoadModule(string filePath)
        {
            moduleService.Load(filePath);
            ModuleModel = moduleService.GetModuleModel();
            RaisePropertyChanged(nameof(ModuleModel));
        }

        public void UpdateFullModulePath()
        {
            if(!string.IsNullOrEmpty(ModuleModel.ModFilename))
                FullModulePath = SettingsModel.FGModuleFolderLocation + "\\" + ModuleModel.ModFilename + ".mod";
            else
                FullModulePath = SettingsModel.FGModuleFolderLocation;
        }

    }
}
