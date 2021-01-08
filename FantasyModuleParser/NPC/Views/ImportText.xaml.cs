using FantasyModuleParser.Importer.NPC;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.ViewModels;
using System;
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

            importESNPC = new ImportEngineerSuiteNPC();
            importDnDBeyondNPC = new ImportDnDBeyondNPC();
            importPDFNPC = new ImportPDFNPC();
        }

        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void ImportTextAndReturnButton_Click(object sender, RoutedEventArgs e)
        {
            ImportNPCModelFromText(ImportTextBox.Text);
            this.Visibility = Visibility.Hidden;
        }

        private void ImportTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try 
            { 
                ImportNPCModelFromText(ImportTextBox.Text);
                ImportErrorFeedbackTB.Text = "";
            }
            catch (ApplicationException exception)
            {
                ImportErrorFeedbackTB.Text = exception.Message;
            }
            NPCStatBlockUC.RefreshDataContext(sender, null);
        }

        private void ImportNPCModelFromText(string inputData)
        {
            NPCController npcController = new NPCController();
            switch (ImportSelector.SelectedIndex)
            {
                case 0:
                    npcController.LoadNPCModel(importPDFNPC.ImportTextToNPCModel(inputData));
                    break;
                case 1:
                    npcController.LoadNPCModel(importESNPC.ImportTextToNPCModel(inputData));
                    break;
                case 2:
                    npcController.LoadNPCModel(importDnDBeyondNPC.ImportTextToNPCModel(inputData));
                    break;
            }
        }
    }
}
