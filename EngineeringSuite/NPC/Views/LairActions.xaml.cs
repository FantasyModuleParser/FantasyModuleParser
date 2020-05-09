using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EngineeringSuite;
using EngineeringSuite.NPC.Controllers;
using EngineeringSuite.NPC.Models.Action;
using EngineeringSuite.NPC.UserControls.LairActions;

namespace EngineeringSuite.NPC
{
    /// <summary>
    /// Interaction logic for LairActions.xaml
    /// </summary>
    public partial class LairActions : Window
    {
        private ActionController actionController;
        public ObservableCollection<LairAction> NpcLairActions { get; set; }
        public LairActions()
        {
            InitializeComponent();
            actionController = new ActionController();
            NpcLairActions = actionController.GetNPCModel().LairActions;

            DataContext = this;
        }
        private void action_Checked(object sender, RoutedEventArgs e)
        {
            if (lairOptions.IsChecked == true)
            {
                stackOptions.Visibility = Visibility.Visible;
                stackActions.Visibility = Visibility.Hidden;
            }
            if (lairActions.IsChecked == true)
            {
                stackOptions.Visibility = Visibility.Hidden;
                stackActions.Visibility = Visibility.Visible;
            }

            //throw new NotImplementedException();
        }
        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OverviewControl_RaiseActionInList(object sender, EventArgs e)
        {
            if (sender is OverviewControl)
            {
                var action = (sender as OverviewControl).DataContext;
                if (action is LairAction)
                {
                    actionController.RaiseActionInLairActionList(action as LairAction);
                }
            }
        }

        private void OverviewControl_LowerActionInList(object sender, EventArgs e)
        {
            if (sender is OverviewControl)
            {
                var action = (sender as OverviewControl).DataContext;
                if (action is LairAction)
                {
                    actionController.LowerActionInLairActionsList(action as LairAction);
                }
            }
        }

        private void OverviewControl_RemoveAction(object sender, EventArgs e)
        {
            if (sender is OverviewControl)
            {
                var action = (sender as OverviewControl).DataContext;
                if (action is LairAction)
                {
                    actionController.RemoveLairAction(action as LairAction);
                }
            }
        }

        private void OverviewControl_EditAction(object sender, EventArgs e)
        {
            if (sender is OverviewControl)
            {
                var action = (sender as OverviewControl).DataContext;
                if (action is LairAction)
                {
                    var lairAction = (action as LairAction);
                    if (lairAction.ActionName.Equals(OptionsControl.ActionName))
                    {
                        lairOptions.IsChecked = true;
                        stackOptions.Visibility = Visibility.Visible;
                        stackActions.Visibility = Visibility.Hidden;
                        lairOptionsControl.DataContext = lairAction;
                    } else
                    {
                        lairActions.IsChecked = true;
                        stackOptions.Visibility = Visibility.Hidden;
                        stackActions.Visibility = Visibility.Visible;
                        lairActionControl.DataContext = lairAction;
                    }
                }
            }
        }
    }
}
