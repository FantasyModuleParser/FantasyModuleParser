using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using System;
using System.Collections.Generic;
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
