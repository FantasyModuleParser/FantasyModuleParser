using FantasyModuleParser.Equipment.Enums;
using FantasyModuleParser.Equipment.UserControls.Models;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using Newtonsoft.Json;
using System.IO;

namespace FantasyModuleParser.Equipment.Models
{
    public class EquipmentModel : ModelBase
    {

        public string Name { get; set; }
        public string NonIdName;
        public string NonIdDescription;

        public bool IsLocked;
        public bool IsIdentified;

        public string Description;
        public string ImageFilePath;

        public int CostValue;
        public CurrencyEnum CostDenomination;
        public double Weight;
        public RarityEnum RarityEnumType;

        public PrimaryEquipmentEnum PrimaryEquipmentEnumType;

        #region Secondary Panel Options
        public AdventuringGearEnum AdventuringGearEnumType;
        public ArmorEnum ArmorEnumType;
        public WeaponEnum WeaponEnumType;
        public AnimalsEnum AnimalsEnumType;
        public VehiclesEnum VehiclesEnumType;
        public ToolsEnum ToolsEnumType;
        public TreasureEnum TreasureEnumType;
        #endregion Secondary Panel Options

        public ArmorModel Armor { get; set; } = new ArmorModel();
        public WeaponModel Weapon { get; set; } = new WeaponModel();

        public EquipmentModel()
        {
        }
        public void Save()
        {
            SettingsService settingsService = new SettingsService();
            string folderPath = settingsService.Load().EquipmentFolderLocation;

            Save(folderPath);
        }
        public void Save(string folderPath)
        {
            Directory.CreateDirectory(folderPath);
            using (StreamWriter file = File.CreateText(Path.Combine(folderPath, this.Name + ".json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, this);
            }
        }

        public EquipmentModel Load(string @filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(@filePath);
                return JsonConvert.DeserializeObject<EquipmentModel>(jsonData);
            }
            return null;
        }

        public EquipmentModel ShallowCopy()
        {
            return (EquipmentModel)this.MemberwiseClone();
        }
    }
}
