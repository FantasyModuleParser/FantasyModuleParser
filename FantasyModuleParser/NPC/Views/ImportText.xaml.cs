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
        private IImportNPC importESNPC;
        private IImportNPC importDnDBeyondNPC;
        private IImportNPC importPDFNPC;
        private IImportNPC import5eToolsNPC;

        public ImportText()
        {
            InitializeComponent();
            ImportTextBox.Focus();
            ImportTextBox.Select(0,0);
        }

        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void ImportTextAndReturnButton_Click(object sender, RoutedEventArgs e)
        {
            importESNPC = new ImportEngineerSuiteNPC();
            importDnDBeyondNPC = new ImportDnDBeyondNPC();
            importPDFNPC = new ImportPDFNPC();
            import5eToolsNPC = new Import5eToolsNPC();
            NPCController npcController = new NPCController();

            switch(ImportSelector.SelectedIndex)
            {
                case 0:
                    npcController.LoadNPCModel(importPDFNPC.ImportTextToNPCModel(ImportTextBox.Text));
                    break;
                case 1:
                    npcController.LoadNPCModel(importESNPC.ImportTextToNPCModel(ImportTextBox.Text));
                    break;
                case 2:
                    npcController.LoadNPCModel(importDnDBeyondNPC.ImportTextToNPCModel(ImportTextBox.Text));
                    break;
                case 3:
                    npcController.LoadNPCModel(import5eToolsNPC.ImportTextToNPCModel(ImportTextBox.Text));
                    break;
            }
            this.Visibility = Visibility.Hidden;
        }
    }
}
