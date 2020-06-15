using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.UserControls.NPCTabs;
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
        public BaseStatsUC baseStatsUC { get; set; }
        public NPCModel npcModel { get; set; }
        #endregion
        public PreviewNPC()
        {
            InitializeComponent();
            npcController = new NPCController();
            npcModel = new NPCModel();
            DataContext = npcController.GetNPCModel();
        }

        public void UpdateMovementDescriptionInPreview(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(npcModel.Speed);
            Console.WriteLine(npcModel.Burrow);
        }
        public void WindowClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
