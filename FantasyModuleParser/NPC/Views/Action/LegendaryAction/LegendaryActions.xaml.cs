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
using FantasyModuleParser;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.UserControls.Action;

namespace FantasyModuleParser.NPC
{
    /// <summary>
    /// Interaction logic for LegendaryActions.xaml
    /// </summary>
    public partial class LegendaryActions : Window
    {
        private ActionController actionController;
        public ObservableCollection<LegendaryActionModel> NpcLegendaryActions { get; set; }
        public LegendaryActions()
        {
            InitializeComponent();
            actionController = new ActionController();
            NpcLegendaryActions = actionController.GetNPCModel().LegendaryActions;

            DataContext = this;
        }

        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateOption_Click(object sender, RoutedEventArgs e)
        {
            LegendaryActionModel legendaryActionModel = new LegendaryActionModel();
            legendaryActionModel.ActionName = ActionModelBase.OptionsNameID;
            legendaryActionModel.ActionDescription = OptionDescriptionTB.Text;
            actionController.UpdateLegendaryAction(CommonMethod.CloneJson(legendaryActionModel));
        }

        private void UpdateAction_Click(object sender, RoutedEventArgs e)
        {
            LegendaryActionModel updateAction = new LegendaryActionModel();
            updateAction.ActionName = ActionNameTB.Text;
            updateAction.ActionDescription = ActionDescTB.Text;
            actionController.UpdateLegendaryAction(CommonMethod.CloneJson(updateAction));
        }

        private void OverviewControl_RemoveAction(object sender, EventArgs e)
        {
            actionController.RemoveLegendaryAction((sender as OverviewControl).DataContext as LegendaryActionModel);
        }

        private void OverviewControl_EditAction(object sender, EventArgs e)
        {
            LegendaryActionModel editAction = (sender as OverviewControl).DataContext as LegendaryActionModel;
            if (editAction.ActionName.Equals(ActionModelBase.OptionsNameID))
            {
                OptionDescriptionTB.Text = editAction.ActionDescription;
            }
            else
            {
                ActionNameTB.Text = editAction.ActionName;
                ActionDescTB.Text = editAction.ActionDescription;
            }
        }

        private void OverviewControl_RaiseActionInList(object sender, EventArgs e)
        {
            actionController.RaiseLegendaryActionInList((sender as OverviewControl).DataContext as LegendaryActionModel);
        }

        private void OverviewControl_LowerActionInList(object sender, EventArgs e)
        {
            actionController.LowerLegendaryActionInList((sender as OverviewControl).DataContext as LegendaryActionModel);
        }
    }
}
