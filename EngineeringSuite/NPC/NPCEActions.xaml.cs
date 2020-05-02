using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using EngineeringSuite.NPC.Controller;
using EngineeringSuite.NPC.Models.NPCAction;
using EngineeringSuite.NPC.UserControls;
using EngineeringSuite.NPC.ViewModel;

namespace EngineeringSuite.NPC
{
    /// <summary>
    /// Interaction logic for NPCEActions.xaml
    /// </summary>
    public partial class NPCEActions : Window
    {

        private NPCModel _npcModel;
        private NPCController _npcController;

        public NPCEActions()
        {
            InitializeComponent();
            DataContext = new ActionViewModel();

            _npcController = new NPCController();
            _npcModel = _npcController.GetNPCModel();
        }

        private void OnInit(object sender, RoutedEventArgs e)
        {
            _npcController = new NPCController();
            _npcModel = _npcController.GetNPCModel();

            foreach(ActionModelBase item in _npcModel.NPCActions)
            {
                ((ActionViewModel)DataContext).NPCActions.Add(item);
            }

            //NpcActionList.ItemsSource = _npcModel.NPCActions;
        }


        #region NPCE_Up
        private void Up2_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up3_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up4_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up5_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up6_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up7_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up8_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up9_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up10_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        #endregion
        #region NPCE_Down
        private void Down1_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down2_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down3_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down4_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down5_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down6_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down7_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down8_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down9_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        #endregion
        #region NPCE_Edit
        private void Edit1_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit2_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit3_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit4_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit5_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit6_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit7_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit8_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit9_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit10_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        #endregion
        #region NPCE_Cancel
        private void Cancel1_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel2_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel3_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel4_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel5_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel6_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel7_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel8_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel9_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel10_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        #endregion
        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            _npcModel.NPCActions.Clear();
            _npcModel.NPCActions.AddRange(((ActionViewModel)DataContext).NPCActions);
            this.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void updateMultiAttack(object sender, RoutedEventArgs e)
        {
            if ((bool)multiAttackCB.IsChecked)
            {
                //_npcModel.npcActions.MultiAttack = new Multiattack(multiAttackTextArea.Text);
                ((ActionViewModel)DataContext).updateMultiAttack(new Multiattack(multiAttackTextArea.Text));
            } else
            {
                ((ActionViewModel)DataContext).removeMultiAttack();
            }

        }

        private void WA_Update_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ActionOverviewControl_RemoveAction(object sender, EventArgs e)
        {
            if(sender is ActionOverviewControl)
            {
                var userControl_ActionOverviewControl = (ActionOverviewControl)sender;
                if(userControl_ActionOverviewControl.DataContext is Multiattack)
                    ((ActionViewModel)DataContext).removeMultiAttack();
            }
        }

        private void Update_OtherAction(object sender, RoutedEventArgs e)
        {
            ((ActionViewModel)DataContext).updateOtherAction();
        }

        private void WeaponAttack_Awesome_UpdateWeaponAttackAction(object sender, EventArgs e)
        {
            if(sender is ActionWeaponAttackControl)
            {
                ActionWeaponAttackControl _actionWeaponAttackControl = (ActionWeaponAttackControl)sender;
                if(_actionWeaponAttackControl.DataContext is WeaponAttack)
                {
                    ((ActionViewModel)DataContext).updateWeaponAttack((WeaponAttack)_actionWeaponAttackControl.DataContext);
                } 
            }
        }
    }
}
