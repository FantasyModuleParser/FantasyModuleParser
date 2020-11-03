using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.Commands;
using FantasyModuleParser.Spells.Models;
using System.Linq;
using System.Windows.Input;

namespace FantasyModuleParser.Main.ViewModels
{
    public class CustomCastersUCViewModel
    {
        private FMPConfigurationService fmpConfigurationService;
        public FMPConfigurationModel FMPConfigurationModel { get; set; }
        
        public CustomCastersUCViewModel()
        {
            fmpConfigurationService = new FMPConfigurationService();
            FMPConfigurationModel = fmpConfigurationService.Load();
            OnRemoveCustomCasterCommand = new ActionCommand(x => RemoveCustomCaster(x.ToString()));
        }
        public ICommand OnRemoveCustomCasterCommand { get; set; }
        public void AddCustomCaster(string newCustomCaster)
        {
            //validate that the language is not null AND is populated
            if (newCustomCaster != null && newCustomCaster.Length > 0)
            {
                // Check for any duplicates
                CustomCastersModel customCastersModel =
                    FMPConfigurationModel.CustomCasters.FirstOrDefault(item => item.Caster.Equals(newCustomCaster));
                if (customCastersModel == null)
                {
                    FMPConfigurationModel.CustomCasters.Add(new CustomCastersModel() { Caster = newCustomCaster });
                    fmpConfigurationService.Save(FMPConfigurationModel);
                }
            }
        }
        public void RemoveCustomCaster(string customCaster)
        {
            FMPConfigurationModel.CustomCasters.Remove(new CustomCastersModel() { Caster = customCaster });
            fmpConfigurationService.Save(FMPConfigurationModel);
        }
        public void RemoveCustomCaster(CustomCastersModel customCastersModel)
        {
            FMPConfigurationModel.CustomCasters.Remove(customCastersModel);
            fmpConfigurationService.Save(FMPConfigurationModel);
        }
    }
}
