using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.NPC.UserControls.Action
{
    /// <summary>
    /// Interaction logic for ActionOtherActionControl.xaml
    /// </summary>
    public partial class OtherActionControl : UserControl
    {
        private OtherAction _otherAction;

        public OtherAction OtherAction
        {
            get
            {
                if (_otherAction == null)
                    _otherAction = new OtherAction();
                return _otherAction;
            }
            set
            {
                _otherAction = value;
            }
        }

        public OtherActionControl()
        {
            InitializeComponent();
            DataContext = OtherAction;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ActionController actionController = new ActionController();
            var thisDataContext = (sender as Button).DataContext;
            if (thisDataContext is OtherAction)
            {
                actionController.UpdateOtherAction((OtherAction)thisDataContext);
            }
        }
    }
}
