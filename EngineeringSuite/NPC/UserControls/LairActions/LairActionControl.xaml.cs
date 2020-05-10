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

namespace EngineeringSuite.NPC.UserControls.LairActions
{
    /// <summary>
    /// Interaction logic for LairActionControl.xaml
    /// </summary>
    public partial class LairActionControl : UserControl
    {
        public Models.Action.LairActions LairAction { get; set; } = new Models.Action.LairActions();
        public LairActionControl()
        {
            InitializeComponent();
            DataContext = LairAction;
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ActionController actionController = new ActionController();
            var thisDataContext = (sender as Button).DataContext;
            if (thisDataContext is Models.Action.LairActions)
            {
                actionController.UpdateLairAction(CommonMethod.CloneJson((Models.Action.LairActions)thisDataContext));
            }
        }
    }
}
