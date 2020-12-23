using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.NPC.UserControls.LairActions
{
    /// <summary>
    /// Interaction logic for LairActionControl.xaml
    /// </summary>
    public partial class LairActionControl : UserControl
    {
        public LairAction LairAction { get; set; } = new LairAction();
        public LairActionControl()
        {
            InitializeComponent();
            DataContext = LairAction;
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ActionController actionController = new ActionController();
            var thisDataContext = (sender as Button).DataContext;
            if (thisDataContext is LairAction)
            {
                actionController.UpdateLairAction(CommonMethod.CloneJson((Models.Action.LairAction)thisDataContext));
            }
        }
    }
}
