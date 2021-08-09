
using FantasyModuleParser.Equipment.Enums;
using FantasyModuleParser.Equipment.Enums.Weapon;
using FantasyModuleParser.Equipment.Models;
using FantasyModuleParser.Equipment.UserControls.Models;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.Models.Action.Enums;
using FantasyModuleParser.NPC.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FantasyModuleParser.Equipment.ViewModels
{
    public class EquipmentOptionControlViewModel : ViewModelBase
    {
        private ModuleService _moduleService;

        private EquipmentModel dataModel;
        private ModuleModel _moduleModel;
        private CategoryModel _categoryModel;
        private ModelBase _footerSelectedModel;
        public EquipmentModel EquipmentDataModel
        {
            get { return dataModel; }
            set { Set(ref dataModel, value); }
        }
        public string Name
        {
            get { return dataModel.Name; }
            set { dataModel.Name = value; RaisePropertyChanged(nameof(Name)); }
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
        public bool IsLocked
        {
            get { return dataModel.IsLocked; }
            set { Set(ref dataModel.IsLocked, value); }
        }
        public bool IsIdentified
        {
            get { return dataModel.IsIdentified; }
            set { Set(ref dataModel.IsIdentified, value); }
        }

        public string CostValue
        {
            get { return dataModel.CostValue; }
            set { Set(ref dataModel.CostValue, value); }
        }

        public CurrencyEnum CostDenomination
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
        [DefaultValue(AdventuringGearEnum.AdventuringGear)]
        public AdventuringGearEnum AdventuringGearEnumType
        {
            get { return dataModel.AdventuringGearEnumType; }
            set { Set(ref dataModel.AdventuringGearEnumType, value); }
        }
        [DefaultValue(ArmorEnum.LightArmor)]
        public ArmorEnum ArmorEnumType
        {
            get { return dataModel.ArmorEnumType; }
            set { dataModel.ArmorEnumType = value; RaisePropertyChanged(nameof(ArmorEnumType)); }
        }

        [DefaultValue(AnimalsEnum.Mounts)]
        public AnimalsEnum AnimalsEnumType
        {
            get { return dataModel.AnimalsEnumType; }
            set { Set(ref dataModel.AnimalsEnumType, value); }
        }
        [DefaultValue(ToolsEnum.ArtisanTools)]
        public ToolsEnum ToolsEnumType
        {
            get { return dataModel.ToolsEnumType; }
            set { Set(ref dataModel.ToolsEnumType, value); }
        }
        [DefaultValue(VehiclesEnum.TackAndHarness)]
        public VehiclesEnum VehiclesEnumType
        {
            get { return dataModel.VehiclesEnumType; }
            set { Set(ref dataModel.VehiclesEnumType, value); }
        }
        [DefaultValue(TreasureEnum.Art)]
        public TreasureEnum TreasureEnumType
        {
            get { return dataModel.TreasureEnumType; }
            set { Set(ref dataModel.TreasureEnumType, value); }
        }

        [DefaultValue(WeaponEnum.MMW)]
        public WeaponEnum WeaponEnumType
        {
            get { return dataModel.WeaponEnumType; }
            set { Set(ref dataModel.WeaponEnumType, value); }
        }
        #endregion Secondary Enum Type Selection Options

        #region Armor Bindings
        public ArmorModel ArmorModelValue
        {
            get { return dataModel.Armor; }
            set { dataModel.Armor =  value; RaisePropertyChanged(nameof(ArmorModelValue)); }
        }
        //public int ArmorValue
        //{
        //    get { return dataModel.Armor.ArmorValue; }
        //    set { Set(ref dataModel.Armor.ArmorValue, value); }
        //}
        //public int StrengthRequirement
        //{
        //    get { return dataModel.Armor.StrengthRequirement; }
        //    set { Set(ref dataModel.Armor.StrengthRequirement, value); }
        //}
        //public bool IsStealthDisadvantage
        //{
        //    get { return dataModel.Armor.IsStealthDisadvantage; }
        //    set { Set(ref dataModel.Armor.IsStealthDisadvantage, value); }
        //}

        private void _raiseArmorBindingProperties()
        {
            RaisePropertyChanged(nameof(ArmorModelValue));
            //RaisePropertyChanged(nameof(ArmorValue));
            //RaisePropertyChanged(nameof(StrengthRequirement));
            //RaisePropertyChanged(nameof(IsStealthDisadvantage));
        }

        #endregion Armor Bindings

        #region Weapon Bindings

        public WeaponModel Weapon
        {
            get { return dataModel.Weapon; }
            set { dataModel.Weapon = value; RaisePropertyChanged(nameof(Weapon));}
        }

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

        private void _raisePrimaryDamageAttributes()
        {
            RaisePropertyChanged(nameof(PrimaryDamageDieCount));
            RaisePropertyChanged(nameof(PrimaryDieType));
            RaisePropertyChanged(nameof(PrimaryDamageBonus));
            RaisePropertyChanged(nameof(PrimaryDamageType));
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

        private void _raiseSecondaryDamageAttributes()
        {
            RaisePropertyChanged(nameof(SecondaryDamageDieCount));
            RaisePropertyChanged(nameof(SecondaryDieType));
            RaisePropertyChanged(nameof(SecondaryDamageBonus));
            RaisePropertyChanged(nameof(SecondaryDamageType));
        }

        #endregion Secondary Damage Attributes
        public HashSet<WeaponPropertyEnum> SelectedWeaponProperties
        {
            get { return dataModel.Weapon.WeaponProperties; }
        }

        public HashSet<WeaponMaterialEnum> SelectedWeaponMaterials
        {
            get { return dataModel.Weapon.MaterialProperties;  }
        }

        private void _raiseOtherWeaponBindingProperties()
        {
            RaisePropertyChanged(nameof(SelectedWeaponProperties));
            RaisePropertyChanged(nameof(SelectedWeaponMaterials));
        }

        private void _raisePropertyChangeOnWeaponBindings()
        {
            RaisePropertyChanged(nameof(Weapon));
            _raisePrimaryDamageAttributes();
            _raiseSecondaryDamageAttributes();
            _raiseOtherWeaponBindingProperties();
        }

        #endregion Weapon Bindings

        public string EquipmentDescription
        {
            get { return dataModel.Description; }
            set { Set(ref dataModel.Description, value); }
        }

        public string EquipmentImageFilePath
        {
            get { return dataModel.ImageFilePath; }
            set { Set(ref dataModel.ImageFilePath, value); }
        }

        public ObservableCollection<CategoryModel> ModuleCategoriesSource
        {
            get { return _moduleModel?.Categories; }
            private set { _moduleModel.Categories = value; }
        }

        public CategoryModel SelectedCategoryModel
        {
            get { return _categoryModel; }
            set { Set(ref _categoryModel, value); }
        }

        public ModelBase SelectedFooterItemModel
        {
            get { return _footerSelectedModel; }
            set 
            { 
                if(value is EquipmentModel)
                {
                    EquipmentDataModel = (value as EquipmentModel).ShallowCopy();
                    raiseAllUIProperties();
                }
                Set(ref _footerSelectedModel, value);
            }
        }

        public void NewEquipmentModel()
        {
            EquipmentDataModel = new EquipmentModel();
            raiseAllUIProperties();
        }

        public void SaveEquipmentModel()
        {
            EquipmentDataModel.Save();
        }

        public void LoadEquipmentModel(string @filePath)
        {
            dataModel = dataModel.Load(filePath);

            raiseAllUIProperties();
        }

        private void raiseAllUIProperties()
        {
            RaisePropertyChanged(nameof(Name));
            RaisePropertyChanged(nameof(NonIdName));
            RaisePropertyChanged(nameof(NonIdDescription));
            RaisePropertyChanged(nameof(IsLocked));
            RaisePropertyChanged(nameof(IsIdentified));
            RaisePropertyChanged(nameof(CostValue));
            RaisePropertyChanged(nameof(CostDenomination));
            RaisePropertyChanged(nameof(Weight));
            RaisePropertyChanged(nameof(PrimaryEquipmentType));
            RaisePropertyChanged(nameof(EquipmentDescription));
            RaisePropertyChanged(nameof(EquipmentImageFilePath));
            RaisePropertyChanged(nameof(ArmorEnumType));
            RaisePropertyChanged(nameof(AnimalsEnumType));
            RaisePropertyChanged(nameof(VehiclesEnumType));
            RaisePropertyChanged(nameof(WeaponEnumType));

            _raiseArmorBindingProperties();
            _raisePropertyChangeOnWeaponBindings();
        }

        public EquipmentOptionControlViewModel()
        {
            EquipmentDataModel = new EquipmentModel();
            _moduleService = new ModuleService();
        }

        public void Refresh()
        {
            _moduleModel = _moduleService.GetModuleModel();
            SelectedCategoryModel = _moduleModel?.Categories.Count > 0? _moduleModel?.Categories[0]: null;
            RaisePropertyChanged(nameof(ModuleCategoriesSource));
        }

        public void AddEquipmentToCategory()
        {
            _moduleService.AddEquipmentToCategory(EquipmentDataModel, SelectedCategoryModel.Name);
            RaisePropertyChanged(nameof(ModuleCategoriesSource));
            RaisePropertyChanged(nameof(SelectedCategoryModel));
        }
    }
}
