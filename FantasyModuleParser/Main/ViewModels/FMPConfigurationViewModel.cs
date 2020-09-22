using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.ViewModel;
using System.Windows;

namespace FantasyModuleParser.Main.ViewModels
{
    public class FMPConfigurationViewModel : ViewModelBase
    {
        private ModuleService _moduleService;
        public Visibility ShowCategoryUserControl { get; set; }
        public Visibility ShowNoModuleLoaded { get; set; }
        public FMPConfigurationViewModel()
        {
            _moduleService = new ModuleService();
            ShowCategoryUserControl = _moduleService.IsModuleConfigured() ? Visibility.Visible : Visibility.Collapsed;
            // Would really like to have this working in XAML...
            ShowNoModuleLoaded = ShowCategoryUserControl.Equals(Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }

        public void Refresh()
        {
            ShowCategoryUserControl = _moduleService.IsModuleConfigured() ? Visibility.Visible : Visibility.Collapsed;
            ShowNoModuleLoaded = ShowCategoryUserControl.Equals(Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
