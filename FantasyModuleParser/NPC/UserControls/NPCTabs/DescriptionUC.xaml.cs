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
            //var npcModel = ((App)Application.Current).NpcModelObject;
            //DataContext = new DescriptionUCViewModel();
        }

        private void ValidateXML(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine(textEditor.Text);
            //TextRange range;

            //range = new TextRange(descriptionRTB.Document.ContentStart, descriptionRTB.Document.ContentEnd);


            //var finalString = "";

            //using (var rtfMemoryStream = new MemoryStream())
            //{
            //    range.Save(rtfMemoryStream, DataFormats.Xaml);
            //    rtfMemoryStream.Seek(0, SeekOrigin.Begin);
            //    using (var rtfStreamReader = new StreamReader(rtfMemoryStream))
            //    {
            //        finalString = rtfStreamReader.ReadToEnd();
            //        Console.WriteLine(finalString);
            //    }
            //}
        }
        public void Refresh()
        {
            //descriptionRTB.Document.Blocks.Clear();

            //descriptionRTB.AppendText(npcController.GetNPCModel().Description);
            (DataContext as DescriptionUCViewModel).Refresh();
        }

        private void descriptionRTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            //TextRange range = new TextRange(descriptionRTB.Document.ContentStart, descriptionRTB.Document.ContentEnd);
            //npcController.GetNPCModel().Description = textEditor.Text;
            
            (DataContext as DescriptionUCViewModel).UpdateNPCDescription(textEditor.Text);
        }

        private void textEditor_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
