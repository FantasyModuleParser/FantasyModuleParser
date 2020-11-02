using FantasyModuleParser.NPC.Models.Skills;
using System.Collections.ObjectModel;

namespace FantasyModuleParser.Main.Models
{
    public class FMPConfigurationModel
    {
        public ObservableCollection<LanguageModel> UserLanguages { get; set; } = new ObservableCollection<LanguageModel>();
        public FMPConfigurationModel()
        {
        }
    }
}
