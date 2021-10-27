using FantasyModuleParser.Converters;
using FantasyModuleParser.Equipment.Models;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC;
using FantasyModuleParser.Spells.Models;
using FantasyModuleParser.Tables.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using FantasyModuleParser.Classes.Model;

namespace FantasyModuleParser.Main.Services
{
    // In effect, this should be used as an injected service via Dependency Injection
    // Purpose of the class is to provide a central way to access the ModuleModel object
    // used throughout the application (hence why ModuleModel is static)
    public class ModuleService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ModuleService));
        // Static module to be shared for any class using ModuleService
        private static ModuleModel moduleModel;
        private SettingsService settingsService;
        public ModuleService()
        {
            if(moduleModel == null)
                moduleModel = new ModuleModel();
            settingsService = new SettingsService();
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
            Directory.CreateDirectory(settingsService.Load().ProjectFolderLocation);
            using (StreamWriter file = File.CreateText(@filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Converters.Add(new SelectableActionModelConverter());
                serializer.Converters.Add(new LanguageModelConverter());
                serializer.Serialize(file, moduleModel);
            }
        }

        public void Save(ModuleModel moduleModel)
        {
            Save(Path.Combine(settingsService.Load().ProjectFolderLocation, moduleModel.ModFilename + ".fmp"), moduleModel);
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
            string warningMessageDoNotAdd= "";

            if (categoryValue == null || categoryValue.Length == 0)
            {
                log.Error("Category value is null; Cannot save NPC.\nSelect a Category from the FG Category dropdown menu.");
                warningMessageDoNotAdd += "Cannot save " + npcModel.NPCName + " NPC. Select a Category from the FG Category dropdown menu. \n";
            }
            if (npcModel == null)
            {
                log.Error("NPC Model data object is null.");
                warningMessageDoNotAdd += "You must create an NPC first \n";
            }
            if (string.IsNullOrEmpty(npcModel.NPCName))
            {
                log.Warn("NPC name is empty!");
                warningMessageDoNotAdd += "Please give your NPC a name \n";
            }
            if (string.IsNullOrEmpty(npcModel.ChallengeRating))
            {
                log.Warn("No Challenge Rating has been set for " + npcModel.NPCName + ".");
                warningMessageDoNotAdd += "Challenge Rating is a required field for " + npcModel.NPCName + "\n";
            }
            if (string.IsNullOrEmpty(npcModel.NPCType))
            {
                log.Warn(npcModel.NPCName + " must have a specified type.");
                warningMessageDoNotAdd += npcModel.NPCName + " must have a specified type. \n";
            }
            CategoryModel categoryModel = moduleModel.Categories.FirstOrDefault(item => item.Name.Equals(categoryValue));
            if (categoryModel == null)
            {
                log.Error("Category Value is not in the Module Model data object!");
                warningMessageDoNotAdd += "No categories are available for selection \n";
            }
            if (!string.IsNullOrEmpty(npcModel.NPCImage) && npcModel.NPCImage.StartsWith("file:///"))
            {
                log.Warn("Remove the file:/// from the NPC Image path and Add to Project again.");
                warningMessageDoNotAdd += "Remove the file:/// from " + npcModel.NPCName + "'s Image path and Add to Project again. \n";
            }
            if (string.IsNullOrEmpty(npcModel.Alignment))
            {
                log.Warn("Select an alignment for the NPC and add to project again.");
                warningMessageDoNotAdd += "Select an alignment for " + npcModel.NPCName + " and add to project again.\n";
            }
            if (string.IsNullOrEmpty(npcModel.AC))
            {
                log.Warn("Please add an armor class and try again.");
                warningMessageDoNotAdd += "Please add an armor class for " + npcModel.NPCName + " and try again.\n";
            }
            if (string.IsNullOrEmpty(npcModel.NPCToken))
            {
                npcModel.NPCToken = "";
            }
            if (string.IsNullOrEmpty(npcModel.NPCImage))
            {
                npcModel.NPCImage = "";
            }

            if (!string.IsNullOrEmpty(warningMessageDoNotAdd))
			{
                MessageBox.Show(warningMessageDoNotAdd);
                return;
			}
            else
			{
                if (categoryModel.NPCModels.FirstOrDefault(x => x.NPCName.Equals(npcModel.NPCName, StringComparison.Ordinal)) == null)
				{
                    categoryModel.NPCModels.Add(npcModel);  // The real magic is here
                }

                Save(moduleModel);

                log.Info(npcModel.NPCName + " has successfully been added to project");
                MessageBox.Show(npcModel.NPCName + " has been added to the project");
            }
        }

        public void AddSpellToCategory(SpellModel spellModel, string categoryValue)
        {
            if (categoryValue == null || categoryValue.Length == 0)
            {
                log.Error("Category value is null;  Cannot save Spell");
                throw new InvalidDataException("Category value is null;  Cannot save Spell");
            }
            if (spellModel == null)
            {
                log.Error(nameof(SpellModel) + " data object is null");
                throw new InvalidDataException(nameof(SpellModel) + " data object is null");
            }
            if (spellModel.SpellName == null || spellModel.SpellName.Length == 0)
            {
                log.Error("Spell name is empty!");
                throw new InvalidDataException("Spell name is empty!");
            }
            CategoryModel categoryModel = moduleModel.Categories.FirstOrDefault(item => item.Name.Equals(categoryValue));
            if (categoryModel == null)
            {
                log.Error("Category Value is not in the Module Model data object!");
                throw new InvalidDataException("Category Value is not in the Module Model data object!");
            }
            if (string.IsNullOrEmpty(spellModel.CastBy))
            {
                log.Error("Spell " + spellModel.SpellName + " has a null value for CastBy");
                throw new InvalidDataException("Spell " + spellModel.SpellName + " has no casters listed.\n What good is a spell if you can't be cast by anyone?");
            }
            else
            {
                if (categoryModel.SpellModels.FirstOrDefault(x => x.SpellName.Equals(spellModel.SpellName, StringComparison.Ordinal)) == null)
                    categoryModel.SpellModels.Add(spellModel);  // The real magic is here
            }

            Save(moduleModel);
        }

        public void AddTableToCategory(TableModel tableModel, string categoryValue)
        {
            if (categoryValue == null || categoryValue.Length == 0)
            {
                log.Error("Category value is null;  Cannot save Table");
                throw new InvalidDataException("Category value is null;  Cannot save Table");
            }
            if (tableModel == null)
            {
                log.Error(nameof(TableModel) + " data object is null");
                throw new InvalidDataException(nameof(SpellModel) + " data object is null");
            }
            if (String.IsNullOrWhiteSpace(tableModel.Name))
            {
                log.Error("Table name is empty!");
                throw new InvalidDataException("Table name is empty!");
            }
            CategoryModel categoryModel = moduleModel.Categories.FirstOrDefault(item => item.Name.Equals(categoryValue));
            if (categoryModel == null)
            {
                log.Error("Category Value is not in the Module Model data object!");
                throw new InvalidDataException("Category Value is not in the Module Model data object!");
            }
            
            if (categoryModel.TableModels.FirstOrDefault(x => x.Name.Equals(tableModel.Name, StringComparison.Ordinal)) == null)
                categoryModel.TableModels.Add(tableModel);  // The real magic is here
            else
            {
                // Replace the TableModel object based on the name
                TableModel oldTableModel = categoryModel.TableModels.FirstOrDefault(x => x.Name.Equals(tableModel.Name, StringComparison.Ordinal));
                int oldTableModelIndex = categoryModel.TableModels.IndexOf(oldTableModel);

                if(oldTableModelIndex != -1)
                {
                    categoryModel.TableModels[oldTableModelIndex] = tableModel;
                }
            }

            Save(moduleModel);
        }

        public void AddEquipmentToCategory(EquipmentModel equipmentModel, string categoryValue)
        {
            if (categoryValue == null || categoryValue.Length == 0)
            {
                log.Error("Category value is null;  Cannot save Equipment");
                throw new InvalidDataException("Category value is null;  Cannot save Equipment");
            }
            if (equipmentModel == null)
            {
                log.Error(nameof(EquipmentModel) + " data object is null");
                throw new InvalidDataException(nameof(EquipmentModel) + " data object is null");
            }
            if (String.IsNullOrWhiteSpace(equipmentModel.Name))
            {
                log.Error("Equipment name is empty!");
                throw new InvalidDataException("Table name is empty!");
            }
            CategoryModel categoryModel = moduleModel.Categories.FirstOrDefault(item => item.Name.Equals(categoryValue));
            if (categoryModel == null)
            {
                log.Error("Category Value is not in the Module Model data object!");
                throw new InvalidDataException("Category Value is not in the Module Model data object!");
            }

            if (categoryModel.EquipmentModels.FirstOrDefault(x => x.Name.Equals(equipmentModel.Name, StringComparison.Ordinal)) == null)
                categoryModel.EquipmentModels.Add(equipmentModel);  // The real magic is here
            else
            {
                // Replace the EquipmentModel object based on the name
                EquipmentModel oldEquipmentModel = categoryModel.EquipmentModels.FirstOrDefault(x => x.Name.Equals(equipmentModel.Name, StringComparison.Ordinal));
                int oldEquipmentModelIdx = categoryModel.EquipmentModels.IndexOf(oldEquipmentModel);

                if (oldEquipmentModelIdx != -1)
                {
                    categoryModel.EquipmentModels[oldEquipmentModelIdx] = equipmentModel;
                }
            }

            Save(moduleModel);
        }

        public void AddClassToCategory(ClassModel classModel, string categoryValue)
        {
            if (categoryValue == null || categoryValue.Length == 0)
            {
                log.Error("Category value is null;  Cannot save Equipment");
                throw new InvalidDataException("Category value is null;  Cannot save Class");
            }
            if (classModel == null)
            {
                log.Error(nameof(EquipmentModel) + " data object is null");
                throw new InvalidDataException(nameof(EquipmentModel) + " data object is null");
            }
            if (String.IsNullOrWhiteSpace(classModel.Name))
            {
                log.Error("Class name is empty!");
                throw new InvalidDataException("Table name is empty!");
            }
            CategoryModel categoryModel = moduleModel.Categories.FirstOrDefault(item => item.Name.Equals(categoryValue));
            if (categoryModel == null)
            {
                log.Error("Category Value is not in the Module Model data object!");
                throw new InvalidDataException("Category Value is not in the Module Model data object!");
            }

            if (categoryModel.ClassModels.FirstOrDefault(x => x.Name.Equals(classModel.Name, StringComparison.Ordinal)) == null)
                categoryModel.ClassModels.Add(classModel);  // The real magic is here
            else
            {
                // Replace the EquipmentModel object based on the name
                ClassModel oldClassModel = categoryModel.ClassModels.FirstOrDefault(x => x.Name.Equals(classModel.Name, StringComparison.Ordinal));
                int oldClassModelIdx = categoryModel.ClassModels.IndexOf(oldClassModel);

                if (oldClassModelIdx != -1)
                {
                    categoryModel.ClassModels[oldClassModelIdx] = classModel;
                }
            }

            Save(moduleModel);
        }
    }
}
