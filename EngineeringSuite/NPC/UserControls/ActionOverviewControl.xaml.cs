using EngineeringSuite.NPC.Controller;
using EngineeringSuite.NPC.Models.NPCAction;
using EngineeringSuite.NPC.ViewModel;
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

namespace EngineeringSuite.NPC.UserControls
{
    /// <summary>
    /// Interaction logic for ActionOverviewControl.xaml
    /// </summary>
    public partial class ActionOverviewControl : UserControl
    {

        public ActionViewModel actionViewModel { get; set; }

        public ActionOverviewControl()
        {
            InitializeComponent();
        }

        private void btn_Up_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Up Button Pressed for Action Overview Control");
        }

        private void btn_Down_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Down Button Pressed for Action Overview Control ");
        }

        private void btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Edit Button Pressed for Action Overview Control ");
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Cancel Button Pressed for Action Overview Control ");
            OnRemoveAction();
        }

        private ActionViewModel GetParentActionViewModel()
        {
            if (Parent is NPCEActions)
            {
                var _parentDataContext = ((NPCEActions)Parent).DataContext;

                // Validate and make sure the parent DataContext is ActionViewModel
                if (_parentDataContext is ActionViewModel)
                {
                    ActionViewModel _actionViewModel = (ActionViewModel)_parentDataContext;
                    return _actionViewModel;
                }
            }
            return null;
        }

        #region Button Events
        public event EventHandler RemoveAction;
        protected virtual void OnRemoveAction()
        {
            if (RemoveAction != null) RemoveAction(this, EventArgs.Empty);
        }
        #endregion
    }
}
