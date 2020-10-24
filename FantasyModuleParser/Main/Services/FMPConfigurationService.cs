using FantasyModuleParser.Main.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace FantasyModuleParser.Main.Services
{
    public class FMPConfigurationService
    {
        private string configurationFilePath;
        private const string configurationFileName = "languages.json";
        public FMPConfigurationService()
        {
            configurationFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP");
        }

        public void Save(FMPConfigurationModel configurationModel)
        {
            if (File.Exists(Path.Combine(configurationFilePath, "config.json")))
                File.Delete(Path.Combine(configurationFilePath, "config.json"));

            Save(configurationModel, Path.Combine(configurationFilePath, configurationFileName));
        }

        public void Save(FMPConfigurationModel configurationModel, string filePath)
        {
            using (StreamWriter file = File.CreateText(@filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, configurationModel);
            }
        }
        public FMPConfigurationModel Load()
        {
            return Load(Path.Combine(configurationFilePath, configurationFileName));
        }
        public FMPConfigurationModel Load(string filePath)
        {
            if (File.Exists(filePath)) { 
                string jsonData = File.ReadAllText(@filePath);
                FMPConfigurationModel fmpConfigurationModel = JsonConvert.DeserializeObject<FMPConfigurationModel>(jsonData);
                return fmpConfigurationModel;
            } else
            {
                return new FMPConfigurationModel();
            }
        }

    }
}
