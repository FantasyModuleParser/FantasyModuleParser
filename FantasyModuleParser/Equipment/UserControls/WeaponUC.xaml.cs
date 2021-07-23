using FantasyModuleParser.Equipment.UserControls.Models;
using FantasyModuleParser.Equipment.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.Equipment.UserControls
{
    /// <summary>
    /// Interaction logic for WeaponUC.xaml
    /// </summary>
    public partial class WeaponUC : UserControl
    {
        public WeaponUC()
        {
            InitializeComponent();
            WeaponUCLayout.DataContext = this;
            
        }

        public void Refresh()
        {
            // Check local DataContext that it's an instance of EquipmentOptionControlViewModel
            // and refresh the two list boxes accordingly
            if(DataContext is EquipmentOptionControlViewModel)
            {
                EquipmentOptionControlViewModel viewModel = DataContext as EquipmentOptionControlViewModel;
            }
            var weaponModel = (WeaponModel)GetValue(WeaponModelProperty);
        }

        private void WeaponPropertyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var weaponModel = (WeaponModel)GetValue(WeaponModelProperty);
        }

        public static readonly DependencyProperty WeaponModelProperty =
            DependencyProperty.Register("WeaponModel", typeof(WeaponModel), typeof(WeaponUC));

        public WeaponModel WeaponModel
        {
            get { return (WeaponModel)GetValue(WeaponModelProperty); }
            set { SetValue(WeaponModelProperty, value); }
        }

        public static readonly DependencyProperty PrimaryDamageDieCountProperty =
            DependencyProperty.Register("PrimaryDamageDieCount", typeof(WeaponModel), typeof(WeaponUC));

        public int PrimaryDamageDieCount
        {
            get { return (int)GetValue(PrimaryDamageDieCountProperty); }
            set { SetValue(PrimaryDamageDieCountProperty, value); }
        }
    }
}
