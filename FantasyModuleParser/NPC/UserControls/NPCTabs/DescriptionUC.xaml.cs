using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.ViewModels;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace FantasyModuleParser.NPC.UserControls.NPCTabs
{
    /// <summary>
    /// Interaction logic for DescriptionUC.xaml
    /// </summary>
    public partial class DescriptionUC : UserControl
    {
        #region Controllers
        public NPCController npcController { get; set; }
        #endregion
        public DescriptionUC()
        {
            InitializeComponent();
        }

        private void ValidateXML(object sender, RoutedEventArgs e)
        {

        }
        public void Refresh()
        {
            (DataContext as DescriptionUCViewModel).Refresh();
        }
    }
}
