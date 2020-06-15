using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.ViewModels;
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
using System.Windows.Shapes;

namespace FantasyModuleParser.NPC.Views
{
    /// <summary>
    /// Interaction logic for PreviewNPC.xaml
    /// </summary>
    public partial class PreviewNPC : Window
    {
        #region Controllers
        public NPCController npcController { get; set; }
        private PreviewNPCViewModel viewModel;
        #endregion
        public PreviewNPC()
        {
            InitializeComponent();
            //npcController = new NPCController();
            //DataContext = npcController.GetNPCModel();
            viewModel = new PreviewNPCViewModel();
            DataContext = viewModel;
        }
        public void WindowClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
