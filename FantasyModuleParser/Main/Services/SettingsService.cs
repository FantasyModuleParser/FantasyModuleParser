using FantasyModuleParser.Main.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace FantasyModuleParser.Main.Services
{
    class SettingsService
    {
        private string settingsConfigurationFilePath;
        private const string settingsConfigurationFileName = "fmp.config";

        public SettingsService()
        {
            settingsConfigurationFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP");
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
                return new SettingsModel();
            }
        }
    }
}
