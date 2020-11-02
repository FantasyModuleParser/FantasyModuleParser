using System;
using System.Collections.ObjectModel;
using System.Windows;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.UserControls.Action;

namespace FantasyModuleParser.NPC
{
    /// <summary>
    /// Interaction logic for Reactions.xaml
    /// </summary>
    public partial class Reactions : Window
    {
        private ActionController actionController;
        private NPCController npcController;
        public ObservableCollection<ActionModelBase> NpcReactions{ get; set; }
        public Reactions()
        {
            InitializeComponent();
            actionController = new ActionController();
            npcController = new NPCController();

            NpcReactions = npcController.GetNPCModel().Reactions;

            DataContext = this;
        }

        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateReaction_Click(object sender, RoutedEventArgs e)
        {
            ActionModelBase actionModelBase = new ActionModelBase();
            actionModelBase.ActionName = ReactionNameTB.Text;
            actionModelBase.ActionDescription = ReactionDescTB.Text;
            actionController.UpdateReaction(CommonMethod.CloneJson(actionModelBase));
        }

        private void OverviewControl_RemoveAction(object sender, EventArgs e)
        {
            actionController.RemoveReaction((sender as OverviewControl).DataContext as ActionModelBase);
        }

        private void OverviewControl_EditAction(object sender, EventArgs e)
        {
            ActionModelBase editAction = (sender as OverviewControl).DataContext as ActionModelBase;
            ReactionNameTB.Text = editAction.ActionName;
            ReactionDescTB.Text = editAction.ActionDescription;
        }

        private void OverviewControl_RaiseActionInList(object sender, EventArgs e)
        {
            actionController.RaiseReactionInList((sender as OverviewControl).DataContext as ActionModelBase);
        }

        private void OverviewControl_LowerActionInList(object sender, EventArgs e)
        {
            actionController.LowerReactionInList((sender as OverviewControl).DataContext as ActionModelBase);
        }
    }
}
