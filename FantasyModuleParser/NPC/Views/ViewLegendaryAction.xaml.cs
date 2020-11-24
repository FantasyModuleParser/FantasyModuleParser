using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.UserControls.LegendaryAction;
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

namespace FantasyModuleParser.NPC.Views
{
    /// <summary>
    /// Interaction logic for ViewLegendaryAction.xaml
    /// </summary>
    public partial class ViewLegendaryAction : Window
    {

        private ActionController actionController;
        public ObservableCollection<LegendaryActionModel> actionList { get; set; }
        public ViewLegendaryAction()
        {
            InitializeComponent();
            actionController = new ActionController();
            actionList = actionController.GetNPCModel().LegendaryActions;

            DataContext = this;
            OptionsRadioButton.IsChecked = true;
        }

        private void action_Checked(object sender, RoutedEventArgs e)
        {
            if (OptionsRadioButton.IsChecked == true)
            {
                stackOptions.Visibility = Visibility.Visible;
                stackActions.Visibility = Visibility.Hidden;
            }
            if (ActionsRadioButton.IsChecked == true)
            {
                stackOptions.Visibility = Visibility.Hidden;
                stackActions.Visibility = Visibility.Visible;
            }

            //throw new NotImplementedException();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private LegendaryActionModel getModelFromDataContext(object sender)
        {
            if (sender is LegendaryActionOverviewControl)
            {
                var action = (sender as LegendaryActionOverviewControl).DataContext;
                if (action is LegendaryActionModel)
                {
                    return (action as LegendaryActionModel);
                }
            }
            return null;
        }

        private void LegendaryActionOverviewControl_RaiseActionInList(object sender, EventArgs e)
        {
            var model = getModelFromDataContext(sender);
            if(model != null)
                actionController.RaiseLegendaryActionInList(model);
                
        }

        private void LegendaryActionOverviewControl_LowerActionInList(object sender, EventArgs e)
        {
            var model = getModelFromDataContext(sender);
            if (model != null)
                actionController.LowerLegendaryActionInList(model);
        }

        private void LegendaryActionOverviewControl_RemoveAction(object sender, EventArgs e)
        {
            var model = getModelFromDataContext(sender);
            if (model != null)
                actionController.RemoveLegendaryAction(model);
        }

        private void LegendaryActionOverviewControl_EditAction(object sender, EventArgs e)
        {
            var model = getModelFromDataContext(sender);
            if (model.ActionName.Equals(LegendaryActionOptionControl.ActionName))
            {
                OptionsRadioButton.IsChecked = true;
                stackOptions.Visibility = Visibility.Visible;
                stackActions.Visibility = Visibility.Hidden;
                legendaryOptionsControl.DataContext = model;
            } else
            {
                ActionsRadioButton.IsChecked = true;
                stackOptions.Visibility = Visibility.Hidden;
                stackActions.Visibility = Visibility.Visible;
                legendaryActionControl.DataContext = model;
            }
        }
    }
}
