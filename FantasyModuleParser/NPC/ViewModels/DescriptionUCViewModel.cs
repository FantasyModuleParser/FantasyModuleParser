using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.ViewModel;
using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.NPC.ViewModels
{
    public class DescriptionUCViewModel : ViewModelBase
    {
        private NPCModel _npcModel;
        public string Description { 
            get 
            { return this._npcModel.Description; } 
            set 
            {
                this._npcModel.Description = value;
                RaisePropertyChanged(nameof(Description)); 
            } 
        }
        private NPCController npcController;
        public DescriptionUCViewModel()
        {
            npcController = new NPCController();
            _npcModel = npcController.GetNPCModel();
        }
        /// <summary>
        /// Specifically used for Unit test integration of 
        /// </summary>
        /// <param name="npcModel"></param>
        public DescriptionUCViewModel(NPCModel npcModel)
        {
            this._npcModel = npcModel;
        }

        public void ValidateXML()
        {
            ValidateXML(Description);
        }
        public String ValidateXML(String inputMarkdownText)
        {
            MarkdownDocument markdownDocument = new MarkdownDocument();
            markdownDocument.Parse(inputMarkdownText);

            StringBuilder output = new StringBuilder();
            foreach (var element in markdownDocument.Blocks)
            {
                if (element is HeaderBlock header)
                {
                    //Console.WriteLine($"Header: {header.ToString()}");
                    if (header.HeaderLevel == 1)
                        output.Append("<h>").Append(header.ToString()).Append("</h>");
                }

                if (element is ParagraphBlock paragraph)
                {
                    Console.WriteLine($"Paragraph: {paragraph.ToString()}");
                }
            }

            return output.ToString();
        }
        public void Refresh()
        {
            _npcModel = npcController.GetNPCModel();
            RaisePropertyChanged(nameof(Description));
        }
    }
}
