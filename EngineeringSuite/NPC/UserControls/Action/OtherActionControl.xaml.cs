using EngineeringSuite.NPC.Controllers;
using EngineeringSuite.NPC.Models.Action;
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

namespace EngineeringSuite.NPC.UserControls.Action
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
