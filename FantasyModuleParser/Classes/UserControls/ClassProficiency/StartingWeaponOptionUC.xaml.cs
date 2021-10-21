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

        public static readonly DependencyProperty ClassModelProperty =
            DependencyProperty.Register("ClassModelValue", typeof(ClassModel), typeof(StartingWeaponOptionUC));

        public ClassModel ClassModelValue
        {
            get { return (ClassModel)GetValue(ClassModelProperty); }
            set { SetValue(ClassModelProperty, value); }
        }

        public string WeaponProficiencies
        {
            get => ClassModelValue != null ? ClassModelValue.WeaponProficiencies : string.Empty;
            set
            {
                ClassModelValue.WeaponProficiencies = value;
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
