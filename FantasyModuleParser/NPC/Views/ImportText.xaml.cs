using FantasyModuleParser.Importer.NPC;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.ViewModels;
using System;
using System.IO;
using System.Text.RegularExpressions;
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
            catch (Exception ex )
            {
                if (ex is ApplicationException || ex is FormatException)
                {
                    ImportErrorFeedbackTB.Text = attemptToFindParseMethodFromException(ex);
                    ImportErrorFeedbackTB.Text += Environment.NewLine;
                    ImportErrorFeedbackTB.Text += ex.Message;
                }   
                else
                    throw;
            }
            NPCStatBlockUC.RefreshDataContext(sender, null);
        }

        private string attemptToFindParseMethodFromException(Exception ex)
        {
            StringReader stringReader = new StringReader(ex.StackTrace);
            string line = "";
            string result = "";
            while ((line = stringReader.ReadLine()) != null)
            {
                if (line.Contains("FantasyModuleParser") && line.Contains(".Parse"))
                {
                    int preSubstringLength = line.IndexOf(".Parse");
                    int postSubstring = line.Substring(preSubstringLength).IndexOf("(");
                    result = line.Substring(preSubstringLength + 1, postSubstring - 1);

                    // We want to ignore basic methods named "Parse" and only target our custom ones
                    if(!result.Contains("Int32"))
                        break;
                }                   
            }
            stringReader.Dispose();
            return result;
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
