using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.Main.Models
{
    public class FMPConfigurationModel
    {
        public ObservableCollection<UserLanguageModel> UserLanguages { get; set; } = new ObservableCollection<UserLanguageModel>();
        public FMPConfigurationModel()
        {
        }
    }
}
