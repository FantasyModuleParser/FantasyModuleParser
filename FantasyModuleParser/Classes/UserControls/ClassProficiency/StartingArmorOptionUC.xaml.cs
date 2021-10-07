using FantasyModuleParser.Classes.Model;
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

        public static readonly DependencyProperty ClassModelProperty =
            DependencyProperty.Register("ClassModelValue", typeof(ClassModel), typeof(StartingArmorOptionUC));

        public ClassModel ClassModelValue
        {
            get { return (ClassModel)GetValue(ClassModelProperty); }
            set { SetValue(ClassModelProperty, value); }
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
            get { return ClassModelValue?.UniqueArmorProficencies;}
            set { ClassModelValue.UniqueArmorProficencies = value; 
                RaisePropertyChanged(nameof(UniqueArmorProperty)); }
        }

        private bool _checkContainsArmorEnum(ArmorEnum armorEnum)
        {
            if (ClassModelValue == null)
                return false;
            if (ClassModelValue.ArmorProficiencies == null)
                return false;

            return ClassModelValue.ArmorProficiencies.Contains(armorEnum);
        }

        private void _setArmorProf(ArmorEnum armorEnum, bool isSet)
        {
            if(ClassModelValue.ArmorProficiencies == null)
            {
                ClassModelValue.ArmorProficiencies = new System.Collections.ObjectModel.ObservableCollection<ArmorEnum>();
            }

            if (isSet)
                ClassModelValue.ArmorProficiencies.Add(armorEnum);
            else
                ClassModelValue.ArmorProficiencies.Remove(armorEnum);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
