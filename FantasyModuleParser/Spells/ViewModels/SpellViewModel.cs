using FantasyModuleParser.NPC.ViewModel;
using FantasyModuleParser.Spells.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.Spells.ViewModels
{
    public class SpellViewModel : ViewModelBase
    {
        // TODO: Include SettingsModel & Service when finished

        private SpellModel _spellModel;
        public SpellModel SpellModel
        {
            get
            {
                return this._spellModel;
            }
            set
            {
                this._spellModel = value;
                RaisePropertyChanged(nameof(SpellModel));
            }
        }
        public SpellViewModel()
        {
            SpellModel = new SpellModel();
        }

        public void Save()
        {
            if (!string.IsNullOrWhiteSpace(SpellModel.SpellName)) { 
                using (StreamWriter file = File.CreateText(@"C:\Users\darkp\AppData\Roaming\FMP\Spells\" + SpellModel.SpellName + ".spl"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(file, SpellModel);
                }
            }
        }

        public void Save(string filePath)
        {
            using (StreamWriter file = File.CreateText(@filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, SpellModel);
            }
        }
    }
}
