using FantasyModuleParser.Equipment.UserControls.Models;
using System.ComponentModel;
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


            //if(DataContext is EquipmentOptionControlViewModel)
            //{
            //    EquipmentOptionControlViewModel viewModel = DataContext as EquipmentOptionControlViewModel;
            //}
            //var weaponModel = (WeaponModel)GetValue(WeaponModelProperty);
        }

        private void WeaponPropertyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var weaponModel = (WeaponModel)GetValue(WeaponModelProperty);
        }

        public static readonly DependencyProperty WeaponModelProperty =
            DependencyProperty.Register("WeaponModel", typeof(WeaponModel), typeof(WeaponUC)
                , new PropertyMetadata(OnWeaponModelPropertyChanged));
                //new FrameworkPropertyMetadata(false, 
                //    new PropertyChangedCallback(OnWeaponModelPropertyChanged)));

        public WeaponModel WeaponModel
        {
            get { return (WeaponModel)GetValue(WeaponModelProperty); }
            set { SetValue(WeaponModelProperty, value); }
        }

        private static void OnWeaponModelPropertyChanged(
            DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //PropertyChangedEventHandler h = PropertyChanged;
            //if (h != null)
            //{
            //    h(sender, new PropertyChangedEventArgs("Second"));
            //}
            WeaponUC c = sender as WeaponUC;
            if(c != null)
            {
                c.OnWeaponModelChanged();
            }
        }

        protected virtual void OnWeaponModelChanged ()
        {
            WeaponModel weaponModel = WeaponModel;

            // Set the 'Bonus Damage" box checked IF the bonus die value > 0 OR bonus damange 'bonus' value > 0
            if(weaponModel.BonusDamage != null)
            {
                SecondaryDamageCB.IsChecked = weaponModel.BonusDamage.NumOfDice > 0 || weaponModel.BonusDamage.Bonus > 0;
            }
        }
    }
}
