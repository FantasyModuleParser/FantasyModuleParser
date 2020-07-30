using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FantasyModuleParser.NPC.UserControls.NPCTabs
{
    /// <summary>
    /// Interaction logic for ActionOverviewUC.xaml
    /// </summary>
    public partial class ActionOverviewUC : UserControl
    {
		private ActionController actionController;
		public ObservableCollection<ActionModelBase> NPCActions { get; set; }
		public ActionOverviewUC()
        {
            InitializeComponent();
			DataContext = new ActionOverviewUserControlViewModel();
		}

		public void Refresh()
        {
			//NPCActions = actionController.GetNPCModel().NPCActions;
			(DataContext as ActionOverviewUserControlViewModel).Refresh();
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
