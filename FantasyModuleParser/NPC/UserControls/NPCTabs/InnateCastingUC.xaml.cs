using FantasyModuleParser.Main;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action.Enums;
using FantasyModuleParser.NPC.Views;
using FantasyModuleParser.NPC.UserControls.NPCTabs;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FantasyModuleParser.NPC.UserControls.NPCTabs
{
    /// <summary>
    /// Interaction logic for InnateCastingUC.xaml
    /// </summary>
    public partial class InnateCastingUC : UserControl
    {
        #region Controllers
        public NPCController npcController { get; set; }
        #endregion
        #region Methods
        private void InnateSpellcasting_Click(object sender, RoutedEventArgs e)
        {
            strComponentText.Text = "requiring no material components";
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9-]+"); ;
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion
        public InnateCastingUC()
        {
            InitializeComponent();
            npcController = new NPCController();
            DataContext = npcController.GetNPCModel();
        }
    }
}
