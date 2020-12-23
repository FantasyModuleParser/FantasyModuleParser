using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.NPC.UserControls.LegendaryAction
{
    /// <summary>
    /// Interaction logic for LegendaryActionOptionControl.xaml
    /// </summary>
    public partial class LegendaryActionOptionControl : UserControl
    {
        public LegendaryActionModel LegendaryActionModel { get; set; } = new LegendaryActionModel();

        public const string ActionName = "Options";

        public LegendaryActionOptionControl()
        {
            InitializeComponent();
            LegendaryActionModel.ActionName = ActionName;
            DataContext = LegendaryActionModel;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ActionController actionController = new ActionController();
            var thisDataContext = (sender as Button).DataContext;
            if (thisDataContext is LegendaryActionModel)
            {
                actionController.UpdateLegendaryAction(CommonMethod.CloneJson(thisDataContext as LegendaryActionModel));
            }
        }
    }
}
