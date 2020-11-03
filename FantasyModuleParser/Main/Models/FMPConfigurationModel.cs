using FantasyModuleParser.NPC.Models.Skills;
using FantasyModuleParser.Spells.Models;
using System.Collections.ObjectModel;

namespace FantasyModuleParser.Main.Models
{
    public class FMPConfigurationModel
    {
        public ObservableCollection<LanguageModel> UserLanguages { get; set; } = new ObservableCollection<LanguageModel>();
        public ObservableCollection<CustomCastersModel> CustomCasters { get; set; } = new ObservableCollection<CustomCastersModel>();
        public FMPConfigurationModel()
        {
        }
    }
}
