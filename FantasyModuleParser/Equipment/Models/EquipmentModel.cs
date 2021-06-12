using FantasyModuleParser.Equipment.Enums;
using FantasyModuleParser.Equipment.UserControls.Models;
using FantasyModuleParser.Main.Models;

namespace FantasyModuleParser.Equipment.Models
{
    public class EquipmentModel : ModelBase
    {

        public string Description;
        public string ImageFilePath;

        public int CostValue;
        public string CostDenomination;
        public double Weight;

        public PrimaryEquipmentEnum PrimaryEquipmentEnumType;

        #region Secondary Panel Options
        public ArmorEnum ArmorEnumType;
        public WeaponEnum WeaponEnumType;
        #endregion Secondary Panel Options

        public ArmorModel Armor = new ArmorModel();
        public WeaponModel Weapon = new WeaponModel();

        public EquipmentModel()
        {
        }

        public void Save()
        {

        }

        public void Load()
        {

        }
    }
}
