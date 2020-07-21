using FantasyModuleParser.Importer.NPC;
using FantasyModuleParser.NPC.Controllers;
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

namespace FantasyModuleParser.NPC
{
    /// <summary>
    /// Interaction logic for ImportText.xaml
    /// </summary>
    public partial class ImportText : Window
    {
        private IImportNPC importNPCService;
        public ImportText()
        {
            InitializeComponent();
        }
        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void ImportTextAndReturnButton_Click(object sender, RoutedEventArgs e)
        {
            importNPCService = new ImportEngineerSuiteNPC();
            NPCController npcController = new NPCController();

            npcController.LoadNPCModel(importNPCService.ImportTextToNPCModel(ImportTextBox.Text));
            this.Visibility = Visibility.Hidden;
        }
    }
}
