using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private void ActionOverviewControl_RemoveAction(object sender, EventArgs e)
        {
            if(sender is ActionOverviewControl)
            {
                var userControl_ActionOverviewControl = (ActionOverviewControl)sender;
                if (userControl_ActionOverviewControl.DataContext is ActionModelBase)
                    ((ActionViewModel)DataContext).removeNPCAction((ActionModelBase)userControl_ActionOverviewControl.DataContext);
            }
        }

        private void Update_OtherAction(object sender, RoutedEventArgs e)
        {
            if(((ActionViewModel)DataContext).OtherActionName != null 
                && ((ActionViewModel)DataContext).OtherActionName.Length > 0)
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

        private void ActionOverviewControl_EditAction(object sender, EventArgs e)
        {
            ActionOverviewControl _actionWeaponAttackControl = (ActionOverviewControl)sender;
            if (_actionWeaponAttackControl.DataContext is Multiattack)
            {
                //((ActionViewModel)DataContext).updateWeaponAttack((WeaponAttack)_actionWeaponAttackControl.DataContext);
                Console.WriteLine("Starting Edit of a Multiattack");
            }
            if (_actionWeaponAttackControl.DataContext is WeaponAttack)
            {
                //((ActionViewModel)DataContext).updateWeaponAttack((WeaponAttack)_actionWeaponAttackControl.DataContext);
                Console.WriteLine("Starting Edit of a WeaponAttack");
                ((ActionViewModel)DataContext).weaponAttack = (WeaponAttack)_actionWeaponAttackControl.DataContext;
            }
            if (_actionWeaponAttackControl.DataContext is OtherAction)
            {
                //((ActionViewModel)DataContext).updateWeaponAttack((WeaponAttack)_actionWeaponAttackControl.DataContext);
                Console.WriteLine("Starting Edit of a OtherAction");
            }
        }

        private void ActionOverviewControl_RaiseActionInList(object sender, EventArgs e)
        {
            ActionOverviewControl _actionWeaponAttackControl = (ActionOverviewControl)sender;
            if (_actionWeaponAttackControl.DataContext is ActionModelBase)
            {
                ObservableCollection<ActionModelBase> npcActions = ((ActionViewModel)DataContext).NPCActions;
                int oldIndex = npcActions.IndexOf((ActionModelBase)_actionWeaponAttackControl.DataContext);
                if (oldIndex != 0)
                    npcActions.Move(oldIndex, oldIndex - 1);
            }

        }

        private void ActionOverviewControl_LowerActionInList(object sender, EventArgs e)
        {
            ActionOverviewControl _actionWeaponAttackControl = (ActionOverviewControl)sender;
            if (_actionWeaponAttackControl.DataContext is ActionModelBase)
            {
                ObservableCollection<ActionModelBase> npcActions = ((ActionViewModel)DataContext).NPCActions;
                int oldIndex = npcActions.IndexOf((ActionModelBase)_actionWeaponAttackControl.DataContext);
                if (oldIndex != npcActions.Count - 1)
                    npcActions.Move(oldIndex, oldIndex + 1);
            }
        }
    }
}
