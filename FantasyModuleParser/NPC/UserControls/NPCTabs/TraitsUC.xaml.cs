using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.UserControls.Action;
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
    /// Interaction logic for TraitsUC.xaml
    /// </summary>
    public partial class TraitsUC : UserControl
    {
        #region Controllers
        public NPCController npcController { get; set; }
		#endregion
		#region Methods
		private void SwapTextBoxes(TextBox input, TextBox output)
		{
			string onHold = input.Text;
			input.Text = output.Text;
			output.Text = onHold;
		}
		#endregion

		public TraitsUC()
        {
            InitializeComponent();
            npcController = new NPCController();
            //var npcModel = ((App)Application.Current).NpcModelObject;

            NPCModel npcModel = npcController.GetNPCModel();
            if(npcModel.Traits == null)
            {
                npcModel.Traits = new ObservableCollection<ActionModelBase>();
            }

            DataContext = npcModel;
        }

        public void Refresh()
        {
            NPCModel npcModel = npcController.GetNPCModel();
            if (npcModel.Traits == null)
            {
                npcModel.Traits = new ObservableCollection<ActionModelBase>();
            }
            DataContext = npcController.GetNPCModel();
        }
        private void AddTraitButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as NPCModel).Traits.Add(new ActionModelBase());
        }

        private void OverviewControl_RaiseActionInList(object sender, EventArgs e)
        {
            NPCModel npcModel = DataContext as NPCModel;
            ActionModelBase action = getActionModelBaseFromLocal(sender);
            ObservableCollection<ActionModelBase> traitsList = npcModel.Traits;
            int actionIndex = traitsList.IndexOf(action);

            if (actionIndex > 0)
            {
                traitsList.Move(actionIndex, actionIndex - 1);
            }
        }

        private void OverviewControl_LowerActionInList(object sender, EventArgs e)
        {
            NPCModel npcModel = DataContext as NPCModel;
            ActionModelBase action = getActionModelBaseFromLocal(sender);
            ObservableCollection<ActionModelBase> traitsList = npcModel.Traits;
            int actionIndex = traitsList.IndexOf(action);

            //If the action is at the bottom of the list, do nothing
            if ((actionIndex + 1) < traitsList.Count)
            {
                traitsList.Move(actionIndex, actionIndex + 1);
            }
        }

        private void OverviewControl_RemoveAction(object sender, EventArgs e)
        {
            NPCModel npcModel = DataContext as NPCModel;
            ActionModelBase action = getActionModelBaseFromLocal(sender);

            npcModel.Traits.Remove(action);
        }

        private ActionModelBase getActionModelBaseFromLocal(object sender)
        {
            if (sender is OverviewControl)
            {
                var action = (sender as OverviewControl).DataContext;
                return action as ActionModelBase;
            }
            return null;
        }
    }
}
