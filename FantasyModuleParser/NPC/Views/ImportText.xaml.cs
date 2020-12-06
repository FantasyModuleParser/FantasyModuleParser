using FantasyModuleParser.Importer.NPC;
using FantasyModuleParser.NPC.Controllers;
using System.Windows;
using System.Windows.Input;

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
        public ImportText()
        {
            InitializeComponent();
            // Enable it so the popup window can close on the Escape key
            PreviewKeyDown += (sender, eventArgs) => { if (eventArgs.Key == Key.Escape) Close(); };
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
            }
            this.Visibility = Visibility.Hidden;
        }
    }
}
