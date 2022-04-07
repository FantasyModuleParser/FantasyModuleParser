using FantasyModuleParser.Main.Models;
using log4net;
using log4net.Core;
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
            ChangeLogLevel(configurationModel);
        }
        public static void Save(SettingsModel configurationModel, string filePath)
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
        public static SettingsModel Load(string filePath)
        {
            SettingsModel settingsModel = null;
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(@filePath);
               settingsModel = JsonConvert.DeserializeObject<SettingsModel>(jsonData);
            }
            else
            {
                settingsModel = new SettingsModel();
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
                settingsModel.LoadLastProject = false;
                settingsModel.LastProject = null;
            }

            _createDirectoryStructure(settingsModel);

            return settingsModel;
        }

        /// <summary>
        /// Based on the Settings Model object, creates a directory (or attempts it) for every module folder location
        /// defined.
        /// </summary>
        private static void _createDirectoryStructure(SettingsModel settingsModel)
        {
            Directory.CreateDirectory(settingsModel.MainFolderLocation);
            Directory.CreateDirectory(settingsModel.ProjectFolderLocation );
            Directory.CreateDirectory(settingsModel.NPCFolderLocation );
            Directory.CreateDirectory(settingsModel.SpellFolderLocation );
            Directory.CreateDirectory(settingsModel.EquipmentFolderLocation );
            Directory.CreateDirectory(settingsModel.ArtifactFolderLocation );
            Directory.CreateDirectory(settingsModel.TableFolderLocation );
            Directory.CreateDirectory(settingsModel.ParcelFolderLocation );
            Directory.CreateDirectory(settingsModel.FGModuleFolderLocation );
            Directory.CreateDirectory(settingsModel.FGCampaignFolderLocation);
        }

        public void ChangeLogLevel()
        {
            ChangeLogLevel(settingsModel);
        }

        public void ChangeLogLevel(SettingsModel settingsModel)
        {
            switch (settingsModel.LogLevel)
            {
                case "INFO":
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = Level.Info;
                    break;
                case "DEBUG":
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = Level.Debug;
                    break;
                default:
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = Level.Info;
                    break;
            }
    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).RaiseConfigurationChanged(EventArgs.Empty);
        }
    }
}
