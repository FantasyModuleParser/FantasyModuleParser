
using FantasyModuleParser.Equipment.Enums;
using FantasyModuleParser.Equipment.Enums.Weapon;
using FantasyModuleParser.Equipment.Models;
using FantasyModuleParser.NPC.Models.Action.Enums;
using FantasyModuleParser.NPC.ViewModel;
using System.Collections.Generic;
using System.ComponentModel;

namespace FantasyModuleParser.Equipment.ViewModels
{
    public class EquipmentOptionControlViewModel : ViewModelBase
    {
        private EquipmentModel dataModel;

        public EquipmentModel EquipmentDataModel
        {
            get { return dataModel; }
            set { Set(ref dataModel, value); }
        }
        public string Name
        {
            get { return dataModel.Name; }
            set { Set(ref dataModel.Name, value); }
        }
        public string NonIdName
        {
            get { return dataModel.NonIdName; }
            set { Set(ref dataModel.NonIdName, value); }
        }
        public string NonIdDescription
        {
            get { return dataModel.NonIdDescription; }
            set { Set(ref dataModel.NonIdDescription, value); }
        }

        public int CostValue
        {
            get { return dataModel.CostValue; }
            set { Set(ref dataModel.CostValue, value); }
        }

        public string CostDenomination
        {
            get { return dataModel.CostDenomination; }
            set { Set(ref dataModel.CostDenomination, value); }
        }

        public double Weight
        {
            get { return dataModel.Weight; }
            set { Set(ref dataModel.Weight, value); }
        }

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

        [DefaultValue(AnimalsEnum.Mounts)]
        public AnimalsEnum AnimalsEnumType
        {
            get { return dataModel.AnimalsEnumType; }
            set { Set(ref dataModel.AnimalsEnumType, value); }
        }

        [DefaultValue(VehiclesEnum.TackAndHarness)]
        public VehiclesEnum VehiclesEnumType
        {
            get { return dataModel.VehiclesEnumType; }
            set { Set(ref dataModel.VehiclesEnumType, value); }
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

        public List<WeaponPropertyEnum> SelectedWeaponProperties
        {
            get { return dataModel.Weapon.WeaponProperties; }
        }

        public List<WeaponMaterialEnum> SelectedWeaponMaterials
        {
            get { return dataModel.Weapon.MaterialProperties;  }
        }

        public int ShortRange
        {
            get { return dataModel.Weapon.ShortRange; }
            set { Set(ref dataModel.Weapon.ShortRange, value); }
        }

        public int LongRange
        {
            get { return dataModel.Weapon.LongRange; }
            set { Set(ref dataModel.Weapon.LongRange, value); }
        }
        #endregion Secondary Damage Attributes

        #endregion Weapon Bindings

        public void SaveEquipmentModel()
        {
            EquipmentDataModel.Save();
        }

        public void LoadEquipmentModel(string @filePath)
        {
            dataModel = dataModel.Load(filePath);
            if(dataModel != null)
            {
                Name = dataModel.Name;
            }
        }
        public EquipmentOptionControlViewModel()
        {
            EquipmentDataModel = new EquipmentModel();
        }
    }
}
