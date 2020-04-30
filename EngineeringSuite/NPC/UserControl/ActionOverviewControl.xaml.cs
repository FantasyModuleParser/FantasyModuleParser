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

        public ActionOverviewControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string AOCId { get; set; }

        public int Id { get; set; }


        private void btn_Up_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Up Button Pressed for Action Overview Control :: ID = " + AOCId);
        }

        private void btn_Down_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Down Button Pressed for Action Overview Control :: ID = " + AOCId);
        }

        private void btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Edit Button Pressed for Action Overview Control :: ID = " + AOCId);
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Cancel Button Pressed for Action Overview Control :: ID = " + AOCId);
        }
    }
}
