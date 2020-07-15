using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FantasyModuleParser.NPC.UserControls.Action
{
    /// <summary>
    /// Interaction logic for WeaponAttackControl.xaml
    /// </summary>
    public partial class WeaponAttackControl : UserControl
    {
        private WeaponAttack _weaponAttack;

        public WeaponAttack WeaponAttack { 
            get 
            {
                if (_weaponAttack == null)
                    _weaponAttack = new WeaponAttack();
                return _weaponAttack;
            }
            set
            {
                _weaponAttack = value;
            } 
        }

        public WeaponAttackControl()
        {
            InitializeComponent();
            DataContext = WeaponAttack;
        }
        
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            ActionController actionController = new ActionController();
            var thisDataContext = (sender as Button).DataContext;
            if (thisDataContext is WeaponAttack)
            {
                actionController.UpdateWeaponAttackAction((WeaponAttack)thisDataContext);
            }
        }
        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            ActionController actionController = new ActionController();

            var thisDataContext = (sender as Button).DataContext;
            if (thisDataContext is WeaponAttack)
            {
                actionController.GenerateWeaponDescription((WeaponAttack)thisDataContext);
            }
        }
        private void AddBonusDmg_Checked(object sender, RoutedEventArgs e)
        {
            SecondaryDamageNumOfDice.IsReadOnly = false;
            SecondaryDamageDieType.IsEnabled = true;
            SecondaryDamageBonus.IsReadOnly = false;
            SecondaryDamageType.IsEnabled = true;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9-]+"); ;
            e.Handled = regex.IsMatch(e.Text);
        }
        private void PositiveNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text) || e.Text.Contains("-");
        }
        private void OtherText_Checked(object sender, RoutedEventArgs e)
        {
            OtherText_Text.IsReadOnly = false;
        }
    }
}
