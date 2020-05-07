using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EngineeringSuite.NPC.Controllers;
using EngineeringSuite.NPC.Models.Action;
using EngineeringSuite.NPC.UserControls.Action;
using EngineeringSuite.NPC.ViewModel;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace EngineeringSuite.NPC
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
            actionController = new ActionController();
            NPCActions = actionController.GetNPCModel().NPCActions;
            DataContext = this;
        }

        private void action_Checked(object sender, RoutedEventArgs e)
        {
            if (multiAttack.IsChecked == true)
            {
                stackMulti.Visibility = Visibility.Visible;
                stackOther.Visibility = Visibility.Hidden;
                stackWeapon.Visibility = Visibility.Hidden;
            }
            if (weaponAttack.IsChecked == true)
            {
                stackMulti.Visibility = Visibility.Hidden;
                stackOther.Visibility = Visibility.Hidden;
                stackWeapon.Visibility = Visibility.Visible;
            }
            if (otherAttack.IsChecked == true)
            {
                stackMulti.Visibility = Visibility.Hidden;
                stackOther.Visibility = Visibility.Visible;
                stackWeapon.Visibility = Visibility.Hidden;
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
