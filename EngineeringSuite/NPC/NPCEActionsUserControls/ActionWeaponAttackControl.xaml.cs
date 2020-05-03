using EngineeringSuite.NPC.Models.NPCAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EngineeringSuite.NPC.UserControls
{
    /// <summary>
    /// Interaction logic for ActionWeaponAttackControl.xaml
    /// </summary>
    public partial class ActionWeaponAttackControl : UserControl
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

        public ActionWeaponAttackControl()
        {
            InitializeComponent();
            DataContext = WeaponAttack;
        }

        public event EventHandler UpdateWeaponAttackAction;
        protected virtual void OnUpdateWeaponAttackAction()
        {
            if (UpdateWeaponAttackAction != null) UpdateWeaponAttackAction(this, EventArgs.Empty);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            WeaponAttack thisWeaponAttack = (WeaponAttack)((Button)sender).DataContext;
            if(thisWeaponAttack.ActionName != null && thisWeaponAttack.ActionName.Length > 0)
                OnUpdateWeaponAttackAction();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9-]+"); ;
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
