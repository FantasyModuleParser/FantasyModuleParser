using FantasyModuleParser.Importer.NPC;
using FantasyModuleParser.NPC.Controllers;
using System.Windows;

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
