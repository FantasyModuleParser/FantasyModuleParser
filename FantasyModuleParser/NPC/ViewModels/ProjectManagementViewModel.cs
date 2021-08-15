using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.Commands;
using FantasyModuleParser.NPC.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FantasyModuleParser.NPC.ViewModels
{
    public class ProjectManagementViewModel : ViewModelBase
    {
        private ModuleService moduleService;
        private SettingsService settingsService;
        private string _fullModulePath;
        // This boolean is used when a new project is invoked.  The purpose is
        // that if the user cancels a new project (currently via the "Close" action), then
        // the original Module data should repopulate the fields.
        private bool _isNewProjectInvoked;
        private ModuleModel _previousModuleModel;
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
            _previousModuleModel = ModuleModel.ShallowCopy() ;
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


        #region Commands
        #region New Project Command
        private ICommand _newProjectCommand;
        public ICommand NewProjectCommand
        {
            get
            {
                if(_newProjectCommand == null)
                {
                    _newProjectCommand = new ActionCommand(param => NewModuleSetup());
                }
                return _newProjectCommand;
            }
        }
        public void NewModuleSetup()
        {
            // If a new project has been invoked, keep track of the original ModuleModel data
            // in case the user changes their mind
            if (!_isNewProjectInvoked)
            {
                _isNewProjectInvoked = true;
                _previousModuleModel = moduleService.GetModuleModel();
            }

            moduleService.UpdateModuleModel(new ModuleModel());
            ModuleModel = moduleService.GetModuleModel();
            RaisePropertyChanged(nameof(ModuleModel));
            
        }
        #endregion

        #region Close and Revert Command (Cancel)
        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if(_cancelCommand == null)
                {
                    _cancelCommand = new ActionCommand(param => RevertToSavedModuleModel());
                }
                return _cancelCommand;
            }
        }
        public void RevertToSavedModuleModel()
        {
            moduleService.UpdateModuleModel(_previousModuleModel);
            ModuleModel = moduleService.GetModuleModel();
            RaisePropertyChanged(nameof(ModuleModel));
        }
        #endregion
        #endregion


        public void SaveModule(string folderPath, ModuleModel moduleModel)
        {
            // If the module has no Categories set, then default the first one to the Module Title
            if(moduleModel.Categories == null || moduleModel.Categories.Count == 0)
            {
                moduleModel.Categories = new ObservableCollection<CategoryModel>();
                moduleModel.Categories.Add(new CategoryModel() { Name = moduleModel.Name });
            }

            moduleService.Save(moduleModel);
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
