using FantasyModuleParser.Classes.Model;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.Classes.UserControls.ClassProficiency
{
    /// <summary>
    /// Interaction logic for StartingWeaponOptionUC.xaml
    /// </summary>
    public partial class StartingWeaponOptionUC : UserControl, INotifyPropertyChanged
    {
        public StartingWeaponOptionUC()
        {
            InitializeComponent();
            StartingWeaponOptionLayout.DataContext = this;
        }

        public static readonly DependencyProperty ProficiencyModelProperty =
            DependencyProperty.Register("ProficiencyModelValue", typeof(ProficiencyModel), typeof(StartingWeaponOptionUC));

        public ProficiencyModel ProficiencyModelValue
        {
            get { return (ProficiencyModel)GetValue(ProficiencyModelProperty); }
            set { SetValue(ProficiencyModelProperty, value); }
        }

        public string WeaponProficiencies
        {
            get => ProficiencyModelValue != null ? ProficiencyModelValue.WeaponProficiencies : string.Empty;
            set
            {
                ProficiencyModelValue.WeaponProficiencies = value;
                RaisePropertyChanged(nameof(WeaponProficiencies));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
