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
            LegendaryActionModel lam = new LegendaryActionModel();
            lam.ActionName = ActionModelBase.OptionsNameID;
            lam.ActionDescription = OptionDescriptionTB.Text;
            actionController.UpdateLegendaryAction(CommonMethod.CloneJson(lam));
        }

        private void UpdateAction_Click(object sender, RoutedEventArgs e)
        {
            LegendaryActionModel lam = new LegendaryActionModel();
            lam.ActionName = ActionNameTB.Text;
            lam.ActionDescription = ActionDescTB.Text;
            actionController.UpdateLegendaryAction(CommonMethod.CloneJson(lam));
        }

        private void OverviewControl_RemoveAction(object sender, EventArgs e)
        {
            actionController.RemoveLegendaryAction((sender as OverviewControl).DataContext as LegendaryActionModel);
        }

        private void OverviewControl_EditAction(object sender, EventArgs e)
        {
            LegendaryActionModel temp = (sender as OverviewControl).DataContext as LegendaryActionModel;
            if (temp.ActionName.Equals(ActionModelBase.OptionsNameID))
            {
                OptionDescriptionTB.Text = temp.ActionDescription;
            }
            else
            {
                ActionNameTB.Text = temp.ActionName;
                ActionDescTB.Text = temp.ActionDescription;
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
