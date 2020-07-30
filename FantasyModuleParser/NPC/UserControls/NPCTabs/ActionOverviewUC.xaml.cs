using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.NPC.UserControls.NPCTabs
{
    /// <summary>
    /// Interaction logic for ActionOverviewUC.xaml
    /// </summary>
    public partial class ActionOverviewUC : UserControl
    {
		private ActionController actionController;
		private ActionOverviewUCViewModel viewModel = new ActionOverviewUCViewModel();
		public ActionOverviewUC()
        {
            InitializeComponent();
			DataContext = viewModel;
        }
		public void Refresh()
        {
			//NPCActions = actionController.GetNPCModel().NPCActions;
			(DataContext as ActionOverviewUCViewModel).Refresh();
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
	}
}
