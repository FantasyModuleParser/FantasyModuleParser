using FantasyModuleParser.Equipment.Enums;
using FantasyModuleParser.NPC.ViewModel;
using System.ComponentModel;

namespace FantasyModuleParser.Equipment.Models
{
    public class EquipmentModel : ViewModelBase
    {
        private PrimaryEquipmentTypeEnum _primaryEquipmentType;
        [DefaultValue(PrimaryEquipmentTypeEnum.AdventuringGear)]
        public PrimaryEquipmentTypeEnum PrimaryEquipmentType
        {
            get { return _primaryEquipmentType; }
            set { Set(ref _primaryEquipmentType, value); }
        }

        #region Secondary Enum Type Selection Options
        private ArmorEnum _armorEnumType;
        [DefaultValue(ArmorEnum.LightArmor)]
        public ArmorEnum ArmorEnumType
        {
            get { return _armorEnumType; }
            set { Set(ref _armorEnumType, value); }
        }
        #endregion Secondary Enum Type Selection Options

        public EquipmentModel()
        {
        }
    }
}
