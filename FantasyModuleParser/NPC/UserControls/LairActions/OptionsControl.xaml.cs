using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.NPC.UserControls.LairActions
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class OptionsControl : UserControl
    {
        public LairAction LairAction { get; set; } = new LairAction();

        public OptionsControl()
        {
            InitializeComponent();
            LairAction.ActionName = ActionModelBase.OptionsNameID;
            DataContext = LairAction;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ActionController actionController = new ActionController();
            var thisDataContext = (sender as Button).DataContext;
            if (thisDataContext is LairAction)
            {
                actionController.UpdateLairAction(CommonMethod.CloneJson(thisDataContext as LairAction));
            }
        }
    }
}
