
using FantasyModuleParser.Equipment.Enums;
using FantasyModuleParser.Equipment.Models;
using FantasyModuleParser.NPC.Models.Action.Enums;
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

        #region Armor Bindings
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

        #endregion Armor Bindings

        #region Weapon Bindings

        #region Primary Damage Attributes
        public int PrimaryDamageDieCount
        {
            get { return dataModel.Weapon.PrimaryDamage.NumOfDice; }
            set 
            { 
                dataModel.Weapon.PrimaryDamage.NumOfDice = value;
                RaisePropertyChanged(nameof(PrimaryDamageDieCount));
            }
        }

        public DieType PrimaryDieType
        {
            get { return dataModel.Weapon.PrimaryDamage.DieType; }
            set
            {
                dataModel.Weapon.PrimaryDamage.DieType = value;
                RaisePropertyChanged(nameof(PrimaryDieType));
            }
        }

        public int PrimaryDamageBonus
        {
            get { return dataModel.Weapon.PrimaryDamage.Bonus; }
            set
            {
                dataModel.Weapon.PrimaryDamage.Bonus = value;
                RaisePropertyChanged(nameof(PrimaryDamageBonus));
            }
        }

        public DamageType PrimaryDamageType
        {
            get { return dataModel.Weapon.PrimaryDamage.DamageType; }
            set
            {
                dataModel.Weapon.PrimaryDamage.DamageType = value;
                RaisePropertyChanged(nameof(PrimaryDamageType));
            }
        }
        #endregion Primary Damage Attributes
        #region Secondary Damage Attributes
        public int SecondaryDamageDieCount
        {
            get { return dataModel.Weapon.BonusDamage.NumOfDice; }
            set
            {
                dataModel.Weapon.BonusDamage.NumOfDice = value;
                RaisePropertyChanged(nameof(SecondaryDamageDieCount));
            }
        }

        public DieType SecondaryDieType
        {
            get { return dataModel.Weapon.BonusDamage.DieType; }
            set
            {
                dataModel.Weapon.BonusDamage.DieType = value;
                RaisePropertyChanged(nameof(SecondaryDieType));
            }
        }

        public int SecondaryDamageBonus
        {
            get { return dataModel.Weapon.BonusDamage.Bonus; }
            set
            {
                dataModel.Weapon.BonusDamage.Bonus = value;
                RaisePropertyChanged(nameof(SecondaryDamageBonus));
            }
        }

        public DamageType SecondaryDamageType
        {
            get { return dataModel.Weapon.BonusDamage.DamageType; }
            set
            {
                dataModel.Weapon.BonusDamage.DamageType = value;
                RaisePropertyChanged(nameof(SecondaryDamageType));
            }
        }
        #endregion Secondary Damage Attributes

        #endregion Weapon Bindings
        public EquipmentOptionControlViewModel()
        {
            dataModel = new EquipmentModel();
        }
    }
}
