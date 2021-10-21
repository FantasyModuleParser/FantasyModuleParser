﻿using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.Equipment.Enums;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.Classes.UserControls.ClassProficiency
{
    /// <summary>
    /// Interaction logic for StartingArmorOptionUC.xaml
    /// </summary>
    public partial class StartingArmorOptionUC : UserControl, INotifyPropertyChanged
    {
        public StartingArmorOptionUC()
        {
            InitializeComponent();
            StartingArmorOptionLayout.DataContext = this;
        }

        public static readonly DependencyProperty ProficiencyModelProperty =
            DependencyProperty.Register("ProficiencyModelValue", typeof(ProficiencyModel), typeof(StartingArmorOptionUC));

        public ProficiencyModel ProficiencyModelValue
        {
            get { return (ProficiencyModel)GetValue(ProficiencyModelProperty); }
            set { SetValue(ProficiencyModelProperty, value); }
        }

        public bool HasLightArmor
        {
            get { return _checkContainsArmorEnum(Equipment.Enums.ArmorEnum.LightArmor); }
            set { _setArmorProf(ArmorEnum.LightArmor, value);
                RaisePropertyChanged(nameof(HasLightArmor));
            }
        }

        public bool HasMediumArmor
        {
            get { return _checkContainsArmorEnum(Equipment.Enums.ArmorEnum.MediumArmor); }
            set { _setArmorProf(ArmorEnum.MediumArmor, value);
                RaisePropertyChanged(nameof(HasMediumArmor));
            }
        }

        public bool HasHeavyArmor
        {
            get { return _checkContainsArmorEnum(Equipment.Enums.ArmorEnum.HeavyArmor); }
            set { _setArmorProf(ArmorEnum.HeavyArmor, value);
                RaisePropertyChanged(nameof(HasHeavyArmor));
            }
        }

        public bool HasShield
        {
            get { return _checkContainsArmorEnum(Equipment.Enums.ArmorEnum.Shield); }
            set { 
                _setArmorProf(ArmorEnum.Shield, value);
                RaisePropertyChanged(nameof(HasShield));
            }
        }

        public string UniqueArmorProperty
        {
            get { return ProficiencyModelValue?.UniqueArmorProficencies;}
            set { ProficiencyModelValue.UniqueArmorProficencies = value; 
                RaisePropertyChanged(nameof(UniqueArmorProperty)); }
        }

        private bool _checkContainsArmorEnum(ArmorEnum armorEnum)
        {
            if (ProficiencyModelValue == null)
                return false;
            if (ProficiencyModelValue.ArmorProficiencies == null)
                return false;

            return ProficiencyModelValue.ArmorProficiencies.Contains(armorEnum);
        }

        private void _setArmorProf(ArmorEnum armorEnum, bool isSet)
        {
            if(ProficiencyModelValue.ArmorProficiencies == null)
            {
                ProficiencyModelValue.ArmorProficiencies = new System.Collections.ObjectModel.ObservableCollection<ArmorEnum>();
            }

            if (isSet)
                ProficiencyModelValue.ArmorProficiencies.Add(armorEnum);
            else
                ProficiencyModelValue.ArmorProficiencies.Remove(armorEnum);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
