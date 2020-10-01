using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.ViewModel;
using FantasyModuleParser.Spells.Enums;
using FantasyModuleParser.Spells.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace FantasyModuleParser.Spells.ViewModels
{
    public class SpellViewModel : ViewModelBase
    {
        // TODO: Include SettingsModel & Service when finished
        private SettingsModel _settingsModel;
        private SettingsService _settingsService;
        private SpellModel _spellModel;
        private ModuleService _moduleService;
        private ModuleModel _moduleModel;
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

        public ModuleModel ModuleModel
        {
            get
            {
                return this._moduleModel;
            }
            set
            {
                this._moduleModel = value;
                RaisePropertyChanged(nameof(ModuleModel));
            }
        }
        public SpellViewModel()
        {
            SpellModel = new SpellModel();
            _settingsService = new SettingsService();
            _settingsModel = _settingsService.Load();

            _moduleService = new ModuleService();
            ModuleModel = _moduleService.GetModuleModel();
        }

        public void Save()
        {
            if (!string.IsNullOrWhiteSpace(SpellModel.SpellName)) { 
                using (StreamWriter file = File.CreateText(_settingsModel.SpellFolderLocation + @"\" + SpellModel.SpellName + ".spl"))
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
