using FantasyModuleParser.Equipment.Enums.Weapon;
using FantasyModuleParser.Equipment.UserControls.Models;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using static FantasyModuleParser.Extensions.EnumerationExtension;

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

        private void WeaponPropertyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItems = WeaponPropertyListBox.SelectedItems;
            var weaponModel = (WeaponModel)GetValue(WeaponModelProperty);
            foreach(EnumerationMember enumProp in e.AddedItems)
            {
                WeaponPropertyEnum weaponPropertyEnum =
                    (WeaponPropertyEnum)Enum.Parse(typeof(WeaponPropertyEnum), enumProp.Value.ToString());
                weaponModel.WeaponProperties.Add(weaponPropertyEnum);
            }

            foreach(EnumerationMember enumProp in e.RemovedItems)
            {
                WeaponPropertyEnum weaponPropertyEnum =
                    (WeaponPropertyEnum)Enum.Parse(typeof(WeaponPropertyEnum), enumProp.Value.ToString());
                weaponModel.WeaponProperties.Remove(weaponPropertyEnum);
            }

        }

        public static readonly DependencyProperty WeaponModelProperty =
            DependencyProperty.Register("WeaponModel", typeof(WeaponModel), typeof(WeaponUC),
                new PropertyMetadata(OnWeaponModelPropertyChanged));

        public WeaponModel WeaponModel
        {
            get { return (WeaponModel)GetValue(WeaponModelProperty); }
            set { SetValue(WeaponModelProperty, value); }
        }

        //The purpose of this is that when a WeaponModel object is updated (based on the Parent binding), this 
        // will allow for fine-tuning of the UI without extensive bindings.  i.e. Add Bonus Damage CB, the list
        // of weapon attributes & properties

        // Works in combination with OnWeaponModelChanged
        private static void OnWeaponModelPropertyChanged(
            DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            WeaponUC c = sender as WeaponUC;
            if(c != null)
            {
                c.OnWeaponModelChanged();
            }
        }

        //
        protected virtual void OnWeaponModelChanged ()
        {
            WeaponModel weaponModel = WeaponModel;

            // Set the 'Bonus Damage" box checked IF the bonus die value > 0 OR bonus damange 'bonus' value > 0
            if(weaponModel.BonusDamage != null)
            {
                SecondaryDamageCB.IsChecked = weaponModel.BonusDamage.NumOfDice > 0 || weaponModel.BonusDamage.Bonus > 0;
            }

            // If there are Weapon Properties available, then configure the UI here with them
            if(weaponModel.WeaponProperties.Count > 0)
            {
                foreach(WeaponPropertyEnum weaponPropertyEnum in weaponModel.WeaponProperties)
                {
                    EnumerationMember enumerationMember = new EnumerationMember()
                    {
                        Description = "Reach",
                        Value = 5
                    };
                    WeaponPropertyListBox.SelectedItems.Add(enumerationMember);
                }
            }
        }
    }
}
