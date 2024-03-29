﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.UserControls.Action;

namespace FantasyModuleParser.NPC
{
    /// <summary>
    /// Interaction logic for new_NPCActions.xaml
    /// </summary>
    public partial class Actions : Window
    {
        private ActionController actionController;
        //TODO: Because I'm not sure of a better way to bind this..
        public ObservableCollection<ActionModelBase> NPCActions { get; set; }

        public Actions()
        {
            InitializeComponent();
            // Enable it so the popup window can close on the Escape key
            PreviewKeyDown += (sender, eventArgs) => { if (eventArgs.Key == Key.Escape) Close(); };
            actionController = new ActionController();
            NPCActions = actionController.GetNPCModel().NPCActions;
            DataContext = this;
        }

        private void action_Checked(object sender, RoutedEventArgs e)
        {
            if (multiAttack.IsChecked == true)
            {
                multiAttackControl.Visibility = Visibility.Visible;
                otherActionControl.Visibility = Visibility.Hidden;
                weaponAttackControl.Visibility = Visibility.Hidden;
            }
            if (weaponAttack.IsChecked == true)
            {
                multiAttackControl.Visibility = Visibility.Hidden;
                otherActionControl.Visibility = Visibility.Hidden;
                weaponAttackControl.Visibility = Visibility.Visible;
            }
            if (otherAttack.IsChecked == true)
            {
                multiAttackControl.Visibility = Visibility.Hidden;
                otherActionControl.Visibility = Visibility.Visible;
                weaponAttackControl.Visibility = Visibility.Hidden;
            }
        }

        private void OverviewControl_RemoveAction(object sender, EventArgs e)
        {
            if(sender is OverviewControl)
            {
                var action = (sender as OverviewControl).DataContext;
                if(action is ActionModelBase)
                {
                    actionController.RemoveActionFromNPC(action as ActionModelBase);
                }
            }
        }

        private void OverviewControl_EditAction(object sender, EventArgs e)
        {
            if (sender is OverviewControl)
            {
                var action = (sender as OverviewControl).DataContext;
                if (action is ActionModelBase)
                {
                    // The curse of the Pass-by-reference strikes again.. hence need for deep clone :(
                    if (action is Multiattack)
                    {
                        multiAttack.IsChecked = true;
                        multiAttackControl.DataContext = CommonMethod.CloneJson(action as Multiattack);
                    }
                    if (action is WeaponAttack)
                    {
                        weaponAttack.IsChecked = true;
                        weaponAttackControl.DataContext = CommonMethod.CloneJson(action as WeaponAttack);
                    }
                    if (action is OtherAction)
                    {
                        otherAttack.IsChecked = true;
                        otherActionControl.DataContext = CommonMethod.CloneJson(action as OtherAction);
                    }
                }
            }

        }

        private void OverviewControl_RaiseActionInList(object sender, EventArgs e)
        {
            if (sender is OverviewControl)
            {
                var action = (sender as OverviewControl).DataContext;
                if (action is ActionModelBase)
                {
                    actionController.RaiseActionInNPCActionList(action as ActionModelBase);
                }
            }
        }

        private void OverviewControl_LowerActionInList(object sender, EventArgs e)
        {
            if (sender is OverviewControl)
            {
                var action = (sender as OverviewControl).DataContext;
                if (action is ActionModelBase)
                {
                    actionController.LowerActionInNPCActionsList(action as ActionModelBase);
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
   
}
