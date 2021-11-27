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
                settingsModel.ClassFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP", "Classes");
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
            Directory.CreateDirectory(_getDirectoryPath(settingsModel.MainFolderLocation, String.Empty));
            Directory.CreateDirectory(_getDirectoryPath(settingsModel.ProjectFolderLocation, "Projects"));
            Directory.CreateDirectory(_getDirectoryPath(settingsModel.NPCFolderLocation, "NPCs"));
            Directory.CreateDirectory(_getDirectoryPath(settingsModel.SpellFolderLocation, "Spells"));
            Directory.CreateDirectory(_getDirectoryPath(settingsModel.EquipmentFolderLocation, "Equipment"));
            Directory.CreateDirectory(_getDirectoryPath(settingsModel.ArtifactFolderLocation, "Artifacts"));
            Directory.CreateDirectory(_getDirectoryPath(settingsModel.ClassFolderLocation, "Classes"));
            Directory.CreateDirectory(_getDirectoryPath(settingsModel.TableFolderLocation, "Tables"));
            Directory.CreateDirectory(_getDirectoryPath(settingsModel.ParcelFolderLocation, "Parcels"));
            Directory.CreateDirectory(_getDirectoryPath(settingsModel.FGModuleFolderLocation, "modules"));
            Directory.CreateDirectory(_getDirectoryPath(settingsModel.FGCampaignFolderLocation, "campaigns"));
        }

        private static string _getDirectoryPath(string directoryPath, string subFolder)
        {
            if(settingsModel.MainFolderLocation == null)
            {
                settingsModel.MainFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP");
            }

            if(directoryPath == null)
            {
                return Path.Combine(settingsModel.MainFolderLocation, subFolder);
            }

            return directoryPath;
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
