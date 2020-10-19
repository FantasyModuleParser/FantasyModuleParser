using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.ViewModel;
using FantasyModuleParser.Spells.Models;
using FantasyModuleParser.Spells.Services;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

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
        private ISpellService _spellService;
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
            _spellService = new SpellService();
        }

        public void Save()
        {
            Directory.CreateDirectory(_settingsModel.SpellFolderLocation);

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

        public void LoadSpell()
        {
            SpellModel = _spellService.Load(_settingsModel.SpellFolderLocation);
        }

        public void Refresh()
        {
            ModuleModel = _moduleService.GetModuleModel();
        }

        public void AddSpellToModule(string categoryValue)
        {
            if(ModuleModel == null || ModuleModel.Categories == null || ModuleModel.Categories.Count == 0 || categoryValue == null)
            {
                MessageBox.Show("No Module Project loaded!\nPlease create / load a Module through Options -> Manage Project");
                return;
            }

            try
            {
                _moduleService.AddSpellToCategory(SpellModel, categoryValue);
                MessageBox.Show("Spell has been added to the project");
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error detected while adding NPC to button :: " + exception.Message);
            }
        }
    }
}
