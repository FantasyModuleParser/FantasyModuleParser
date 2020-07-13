using FantasyModuleParser.Commands;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                UserLanguageModel userLanguageModel = 
                    FMPConfigurationModel.UserLanguages.FirstOrDefault(item => item.UserLanguageValue.Equals(newUserLanguage));
                if(userLanguageModel == null) { 
                    FMPConfigurationModel.UserLanguages.Add(new UserLanguageModel() { UserLanguageValue = newUserLanguage });
                    fmpConfigurationService.Save(FMPConfigurationModel);
                }
            }
        }
        public void RemoveUserLanguage(string userLanguage)
        {
            FMPConfigurationModel.UserLanguages.Remove(new UserLanguageModel() { UserLanguageValue = userLanguage });
            fmpConfigurationService.Save(FMPConfigurationModel);
        }
        public void RemoveUserLanguage(UserLanguageModel userLanguageModel)
        {
            FMPConfigurationModel.UserLanguages.Remove(userLanguageModel);
            fmpConfigurationService.Save(FMPConfigurationModel);
        }
    }
}
