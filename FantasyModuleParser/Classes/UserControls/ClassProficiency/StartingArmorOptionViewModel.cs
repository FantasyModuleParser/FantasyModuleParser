using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.Equipment.Enums;
using FantasyModuleParser.NPC.ViewModel;

namespace FantasyModuleParser.Classes.UserControls.ClassProficiency
{
    public class StartingArmorOptionViewModel: ClassProficiencyViewModelBase
    {
        public StartingArmorOptionViewModel() : base()
        {
        }

        public StartingArmorOptionViewModel(ProficiencyModel proficiencyModel) : base()
        {
            this.proficiencyModel = proficiencyModel;
        }

        public bool HasLightArmor
        {
            get { return _checkContainsArmorEnum(Equipment.Enums.ArmorEnum.LightArmor); }
            set
            {
                _setArmorProf(ArmorEnum.LightArmor, value);
                RaisePropertyChanged(nameof(HasLightArmor));
            }
        }

        public bool HasMediumArmor
        {
            get { return _checkContainsArmorEnum(Equipment.Enums.ArmorEnum.MediumArmor); }
            set
            {
                _setArmorProf(ArmorEnum.MediumArmor, value);
                RaisePropertyChanged(nameof(HasMediumArmor));
            }
        }

        public bool HasHeavyArmor
        {
            get { return _checkContainsArmorEnum(Equipment.Enums.ArmorEnum.HeavyArmor); }
            set
            {
                _setArmorProf(ArmorEnum.HeavyArmor, value);
                RaisePropertyChanged(nameof(HasHeavyArmor));
            }
        }

        public bool HasShield
        {
            get { return _checkContainsArmorEnum(Equipment.Enums.ArmorEnum.Shield); }
            set
            {
                _setArmorProf(ArmorEnum.Shield, value);
                RaisePropertyChanged(nameof(HasShield));
            }
        }

        public string UniqueArmorProperty
        {
            get { return proficiencyModel?.UniqueArmorProficencies; }
            set
            {
                proficiencyModel.UniqueArmorProficencies = value;
                RaisePropertyChanged(nameof(UniqueArmorProperty));
            }
        }

        private bool _checkContainsArmorEnum(ArmorEnum armorEnum)
        {
            if (proficiencyModel == null)
                return false;
            if (proficiencyModel.ArmorProficiencies == null)
                return false;

            return proficiencyModel.ArmorProficiencies.Contains(armorEnum);
        }

        private void _setArmorProf(ArmorEnum armorEnum, bool isSet)
        {
            if (proficiencyModel.ArmorProficiencies == null)
            {
                proficiencyModel.ArmorProficiencies = new System.Collections.ObjectModel.ObservableCollection<ArmorEnum>();
            }

            if (isSet)
                proficiencyModel.ArmorProficiencies.Add(armorEnum);
            else
                proficiencyModel.ArmorProficiencies.Remove(armorEnum);
        }
    }
}
