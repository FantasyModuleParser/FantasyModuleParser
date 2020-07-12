using FantasyModuleParser.Main.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.Main.Services
{
    // In effect, this should be used as an injected service via Dependency Injection
    // Purpose of the class is to provide a central way to access the ModuleModel object
    // used throughout the application (hence why ModuleModel is static)
    public class ModuleService
    {
        // Static module to be shared for any class using ModuleService
        private static ModuleModel moduleModel;
        public ModuleService()
        {
            if(moduleModel == null)
            {
                moduleModel = new ModuleModel();
            }
        }

        public ModuleModel GetModuleModel()
        {
            return ModuleService.moduleModel;
        }

        public void UpdateModuleModel(ModuleModel moduleModel)
        {
            ModuleService.moduleModel = moduleModel;
        }

        public void Save(string filePath, ModuleModel moduleModel)
        {
            using (StreamWriter file = File.CreateText(@filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, moduleModel);
            }
        }

        // Loads a FPM Module json file based on a given filePath
        // Must call GetModuleModel() to get the loaded module (overkill bloat happening here)
        public void Load(string @filePath)
        {
            string jsonData = File.ReadAllText(@filePath);
            ModuleModel moduleModel = JsonConvert.DeserializeObject<ModuleModel>(jsonData);
            ModuleService.moduleModel = moduleModel;
        }
    }
}
