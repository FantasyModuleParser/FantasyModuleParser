using FantasyModuleParser.Main.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace FantasyModuleParser.Main.Services
{
    public class SettingsService
    {
        private string settingsConfigurationFilePath;
        private const string settingsConfigurationFileName = "fmp.config";
        private static SettingsModel settingsModel;

        public SettingsService()
        {
            settingsConfigurationFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP");
            if (settingsModel == null)
                settingsModel = new SettingsModel();
        }
        public SettingsModel GetSettingsModel()
        {
            return settingsModel;
        }
        public void Save(SettingsModel configurationModel)
        {
            Save(configurationModel, Path.Combine(settingsConfigurationFilePath, settingsConfigurationFileName));
        }
        public void Save(SettingsModel configurationModel, string filePath)
        {
            using (StreamWriter file = File.CreateText(@filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, configurationModel);
            }
        }
        public SettingsModel Load()
        {
            return Load(Path.Combine(settingsConfigurationFilePath, settingsConfigurationFileName));
        }
        public SettingsModel Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(@filePath);
                SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(jsonData);
                return settingsModel;
            }
            else
            {
                SettingsModel settingsModel = new SettingsModel();
                settingsModel.MainFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP");
                settingsModel.ProjectFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP", "Projects");
                settingsModel.NPCFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP", "NPCs");
                settingsModel.SpellFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP", "Spells");
                settingsModel.EquipmentFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP", "Equipment");
                settingsModel.ArtifactFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP", "Artifacts");
                settingsModel.TableFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP", "Tables");
                settingsModel.ParcelFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP", "Parcels");
                settingsModel.FGModuleFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Fantasy Grounds", "modules");
                settingsModel.FGCampaignFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Fantasy Grounds", "campaigns");
                settingsModel.PersistentWindow = true;
                settingsModel.DefaultGUISelection = "None";
                return settingsModel;
            }
        }
    }
}
