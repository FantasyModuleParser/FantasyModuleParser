using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.ViewModel;
using FantasyModuleParser.Spells.Models;
using FantasyModuleParser.Spells.Services;
using log4net;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Windows;

namespace FantasyModuleParser.Spells.ViewModels
{
    public class SpellViewModel : ViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SpellViewModel));
        // TODO: Include SettingsModel & Service when finished
        private SettingsModel _settingsModel;
        private SettingsService _settingsService;
        private SpellModel _spellModel;
        private ModuleService _moduleService;
        private ModuleModel _moduleModel;
        private CategoryModel _selectedCategoryModel;
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
        public CategoryModel SelectedCategoryModel
        {
            get
            {
                return this._selectedCategoryModel;
            }
            set
            {
                this._selectedCategoryModel = value;
                RaisePropertyChanged(nameof(SelectedCategoryModel));
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
            log.Debug("Spell View Model constructed");
        }
        public void Save()
        {
            Directory.CreateDirectory(_settingsModel.SpellFolderLocation);
            if (!string.IsNullOrWhiteSpace(SpellModel.SpellName))
            {
                if (string.IsNullOrEmpty(SpellModel.CastBy))
                {
                    log.Warn("No CastBy classes selected for spell " + SpellModel.SpellName);
                    MessageBox.Show("Please select which class can cast spell " + SpellModel.SpellName + " and try again.");
                    return;
                }
                try
                {
                    Save(_settingsModel.SpellFolderLocation + @"\" + SpellModel.SpellName + ".spl");
                }
                catch (InvalidDataException exception)
                {
                    MessageBox.Show(" ==== Error detected while saving Spell :: " + exception.Message + " ======== ");
                    log.Error(" ==== Error detected while saving Spell :: " + exception.Message + " ======== ");
                }
            }
        }
        public void Save(string filePath)
        {
            log.Debug("Beginning to save Spell...");
            using (StreamWriter file = File.CreateText(@filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, SpellModel);
                log.Info("Spell " + SpellModel.SpellName + " has successfully been saved to " + filePath);
                MessageBox.Show("Spell " + SpellModel.SpellName + " has been saved successfully");
            }
            log.Debug("Finished saving Spell...");
        }

        public void LoadSpell()
        {
            log.Debug("Loading spell started...");
            SpellModel = _spellService.Load(_settingsModel.SpellFolderLocation);
            log.Debug("Loading spell completed...");
            log.Info("Spell " + SpellModel.SpellName + " has been successfully loaded");
        }

        public void Refresh()
        {
            ModuleModel = _moduleService.GetModuleModel();
        }

        public void AddSpellToModule(string categoryValue)
        {
            if (ModuleModel == null || ModuleModel.Categories == null || ModuleModel.Categories.Count == 0 || categoryValue == null)
            {
                MessageBox.Show("No Module Project loaded!\nPlease create / load a Module through Options -> Manage Project");
                log.Warn("No Module Project loaded!\nPlease create / load a Module through Options -> Manage Project");
                return;
            }
            else if (string.IsNullOrEmpty(SpellModel.CastBy))
            {
                MessageBox.Show("Please select which class can cast spell " + SpellModel.SpellName + " and try again.");
                log.Warn("No CastBy classes selected for spell " + SpellModel.SpellName);
                return;
            }
            try
            {
                log.Debug("Adding spell " + SpellModel.SpellName + " to project started...");
                _moduleService.AddSpellToCategory(SpellModel, categoryValue);
                log.Debug("Adding spell " + SpellModel.SpellName + " to project completed...");
                MessageBox.Show("Spell " + SpellModel.SpellName + " has been added to the project");
                log.Info("Spell " + SpellModel.SpellName + " has been added to the project");
            }
            catch (InvalidDataException exception)
            {
                MessageBox.Show("Error detected while adding Spell to Project :: " + exception.Message);
                log.Error(" ==== Error detected while adding Spell to Project:: " + exception.Message + " ======== ");
            }
        }
        public void UpdateCastBy(string classNames)
        {
            this.SpellModel.CastBy = classNames;
            RaisePropertyChanged(nameof(SpellModel));
            RaisePropertyChanged(nameof(SpellModel.CastBy));
        }

        public static string GenerateComponentDescription(SpellModel spellModel)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (spellModel.IsVerbalComponent)
            {
                stringBuilder.Append("V");
                if (spellModel.IsSomaticComponent)
                {
                    stringBuilder.Append(", S");
                    if (spellModel.IsMaterialComponent)
                        stringBuilder.Append(", M (" + spellModel.ComponentText + ")");
                }
                else if (spellModel.IsMaterialComponent)
                    stringBuilder.Append(", M (" + spellModel.ComponentText + ")");
            }
            else if (!spellModel.IsVerbalComponent && spellModel.IsSomaticComponent)
            {
                stringBuilder.Append("S");
                if (spellModel.IsMaterialComponent)
                    stringBuilder.Append(", M (" + spellModel.ComponentText + ")");
            }
            else if (!spellModel.IsVerbalComponent && !spellModel.IsSomaticComponent && spellModel.IsMaterialComponent)
                stringBuilder.Append("M (" + spellModel.ComponentText + ")");

            return stringBuilder.ToString();
        }
    }
}