using EngineeringSuite.NPC.Controller;
using EngineeringSuite.NPC.Models.NPCAction;
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

namespace EngineeringSuite.NPC.UserController
{
    /// <summary>
    /// Interaction logic for ActionOverviewControl.xaml
    /// </summary>
    public partial class ActionOverviewControl : UserControl
    {

        private NPCController _npcController;

        public ActionOverviewControl()
        {
            InitializeComponent();
            _npcController = new NPCController();
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
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //NPCActionController actionController = new NPCActionController();
            
            //// Having to refactor this so that the built-in previewer doesn't throw 
            //// a hissy-fit 

            //ActionModelBase actionOverviewModel = null;
            //NPCModel _npcModel = _npcController.GetNPCModel();
            //if(_npcModel != null)
            //{
            //    actionOverviewModel = actionController.GetActionOverviewModel(_npcController.GetNPCModel().npcActions, Id);
            //    if (actionOverviewModel != null)
            //    {
            //        actionName.Text = actionOverviewModel.ActionName;
            //        actionDescription.Text = actionOverviewModel.ActionDescription;
            //    }
            //}
        }
    }
}
