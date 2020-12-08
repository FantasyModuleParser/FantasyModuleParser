using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.UserControls.Action;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.NPC.UserControls.NPCTabs
{
    /// <summary>
    /// Interaction logic for ActionOverviewUC.xaml
    /// </summary>
    public partial class ActionOverviewUC : UserControl
    {
		private ActionController actionController = new ActionController();
		public ActionOverviewUC()
        {
            InitializeComponent();
			DataContext = actionController.GetNPCModel();
        }
		public void Refresh()
        {
			DataContext = actionController.GetNPCModel();
		}

		private void LairActions_Click(object sender, RoutedEventArgs e)
		{
			new ViewLairActions().Show();
		}
		private void Actions_Click(object sender, RoutedEventArgs e)
		{
			new Actions().Show();
		}
		private void Reactions_Click(object sender, RoutedEventArgs e)
		{
			new Reactions().Show();
		}
		private void LegAction_Click(object sender, RoutedEventArgs e)
		{
			new LegendaryActions().Show();
		}

        private void OverviewControl_RemoveAction(object sender, EventArgs e)
        {
            if (sender is OverviewControl)
            {
                var action = (sender as OverviewControl).DataContext;
                if (action is ActionModelBase)
                {
                    actionController.RemoveActionFromNPC(action as ActionModelBase);
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
    }
}
