
using FantasyModuleParser.Equipment.Enums;
using FantasyModuleParser.Equipment.Models;
using FantasyModuleParser.NPC.ViewModel;
using System.ComponentModel;

namespace FantasyModuleParser.Equipment.ViewModels
{
    public class EquipmentOptionControlViewModel : ViewModelBase
    {
        private EquipmentModel dataModel;

        [DefaultValue(PrimaryEquipmentEnum.AdventuringGear)]
        public PrimaryEquipmentEnum PrimaryEquipmentType
        {
            get { return dataModel.PrimaryEquipmentEnumType; }
            set { Set(ref dataModel.PrimaryEquipmentEnumType, value); }
        }


        #region Secondary Enum Type Selection Options
        [DefaultValue(ArmorEnum.LightArmor)]
        public ArmorEnum ArmorEnumType
        {
            get { return dataModel.ArmorEnumType; }
            set { Set(ref dataModel.ArmorEnumType, value); }
        }
        #endregion Secondary Enum Type Selection Options

        public int ArmorValue
        {
            get { return dataModel.Armor.ArmorValue; }
            set { Set(ref dataModel.Armor.ArmorValue, value); }
        }
        public int StrengthRequirement
        {
            get { return dataModel.Armor.StrengthRequirement; }
            set { Set(ref dataModel.Armor.StrengthRequirement, value); }
        }
        public bool IsStealthDisadvantage
        {
            get { return dataModel.Armor.IsStealthDisadvantage; }
            set { Set(ref dataModel.Armor.IsStealthDisadvantage, value); }
        }

        public EquipmentOptionControlViewModel()
        {
            dataModel = new EquipmentModel();
        }
    }
}
