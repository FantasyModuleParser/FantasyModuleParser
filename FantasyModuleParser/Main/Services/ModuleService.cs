using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            return moduleModel;
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
        public bool IsModuleConfigured()
        {
            if (moduleModel == null)
                return false;

            if (moduleModel.Name == null || moduleModel.Name.Length == 0)
                return false;
            return true;
        }

        public void AddNPCToCategory(NPCModel npcModel, string categoryValue)
        {
            if (categoryValue == null || categoryValue.Length == 0)
                throw new InvalidDataException("Category value is null;  Cannot save NPC");
            if (npcModel == null)
                throw new InvalidDataException("NPC Model data object is null");
            if(npcModel.NPCName == null || npcModel.NPCName.Length == 0)
                throw new InvalidDataException("NPC name is empty!");
            if (npcModel.ChallengeRating == null || npcModel.ChallengeRating.Length == 0)
                throw new InvalidDataException("No Challenge Rating has been set for NPC.");
            if (npcModel.NPCType == null || npcModel.NPCType.Length == 0)
                throw new InvalidDataException("NPC must have a specified type.");
            CategoryModel categoryModel = moduleModel.Categories.FirstOrDefault(item => item.Name.Equals(categoryValue));
            if (categoryModel == null)
                throw new InvalidDataException("Category Value is not in the Module Model data object!");
            else
            { 
                if( categoryModel.NPCModels.FirstOrDefault(x => x.NPCName.Equals(npcModel.NPCName, StringComparison.Ordinal)) == null)
                    categoryModel.NPCModels.Add(npcModel);  // The real magic is here
            }

            string appendedFileName = moduleModel.SaveFilePath + "\\" + moduleModel.ModFilename + ".fmp";
            Save(appendedFileName, moduleModel);
        }
    }
}
