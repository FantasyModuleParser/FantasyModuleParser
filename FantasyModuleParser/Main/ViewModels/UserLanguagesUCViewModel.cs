using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.Commands;
using FantasyModuleParser.NPC.Models.Skills;
using System.Linq;
using System.Windows.Input;

namespace FantasyModuleParser.Main.ViewModels
{
    public class UserLanguagesUCViewModel
    {
        private FMPConfigurationService fmpConfigurationService;
        public FMPConfigurationModel FMPConfigurationModel { get; set; }
        public UserLanguagesUCViewModel()
        {
            fmpConfigurationService = new FMPConfigurationService();
            FMPConfigurationModel = fmpConfigurationService.Load();
            OnRemoveCategoryCommand = new ActionCommand(x => RemoveUserLanguage(x.ToString()));
        }
        public ICommand OnRemoveCategoryCommand { get; set; }
        public void AddUserLanguage(string newUserLanguage)
        {
            //validate that the language is not null AND is populated
            if (newUserLanguage != null && newUserLanguage.Length > 0) {
                // Check for any duplicates
                LanguageModel userLanguageModel = 
                    FMPConfigurationModel.UserLanguages.FirstOrDefault(item => item.Language.ToLower().Equals(newUserLanguage.ToLower()));
                if(userLanguageModel == null) { 
                    FMPConfigurationModel.UserLanguages.Add(new LanguageModel() { Language = newUserLanguage });
                    fmpConfigurationService.Save(FMPConfigurationModel);
                }
            }
        }
        public void RemoveUserLanguage(string userLanguage)
        {
            FMPConfigurationModel.UserLanguages.Remove(new LanguageModel() { Language = userLanguage });
            fmpConfigurationService.Save(FMPConfigurationModel);
        }
        public void RemoveUserLanguage(LanguageModel userLanguageModel)
        {
            FMPConfigurationModel.UserLanguages.Remove(userLanguageModel);
            fmpConfigurationService.Save(FMPConfigurationModel);
        }
    }
}
