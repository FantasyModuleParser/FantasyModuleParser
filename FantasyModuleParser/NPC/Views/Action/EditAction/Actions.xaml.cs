using FantasyModuleParser.Importer.NPC;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.UserControls.Action;
using System;
using System.Collections.ObjectModel;
using System.Windows;

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
                    // Darkpool:  This statement is to patch older beta saves, where NewsoftJson wrote the class type
                    // for specific actions as an ActionModelBase class instead of the intended class (e.g. Multiattack, WeaponAction, OtherAction).
                    // The workaround is to append the known ActionName. ActionDescription and use our importers to generate the correct class
                    if(!(action is Multiattack) && !(action is WeaponAttack) && !(action is OtherAction))
                    {
                        ImportNPCBase importNPCBase = new ImportPDFNPC();
                        action = importNPCBase.ParseStandardAction((action as ActionModelBase).ActionName + ". " + (action as ActionModelBase).ActionDescription);
                    }

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
