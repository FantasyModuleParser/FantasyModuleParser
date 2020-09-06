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
        private IImportNPC importDnDBeyondNPC;
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
            importDnDBeyondNPC = new ImportDnDBeyondNPC();
            NPCController npcController = new NPCController();

            if(ImportSelector.SelectedIndex == 0)
                npcController.LoadNPCModel(importNPCService.ImportTextToNPCModel(ImportTextBox.Text));
            if (ImportSelector.SelectedIndex == 1)
                npcController.LoadNPCModel(importDnDBeyondNPC.ImportTextToNPCModel(ImportTextBox.Text));
            this.Visibility = Visibility.Hidden;
        }
    }
}
