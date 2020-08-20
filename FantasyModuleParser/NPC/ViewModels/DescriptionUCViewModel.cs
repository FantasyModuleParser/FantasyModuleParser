using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.ViewModel;
using Markdig;
using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

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

        private FlowDocument _markdownDocument;
        public FlowDocument MarkdownDocument
        {
            get
            {
                return this._markdownDocument;
            }
            set
            {
                this._markdownDocument = value;
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
            //MarkdownDocument markdownDocument = new MarkdownDocument();
            //markdownDocument.Parse(inputMarkdownText);


            //StringBuilder output = new StringBuilder();
            //foreach (var element in markdownDocument.Blocks)
            //{
            //    if (element is HeaderBlock header)
            //    {
            //        //Console.WriteLine($"Header: {header.ToString()}");
            //        if (header.HeaderLevel == 1)
            //            output.Append("<h>").Append(header.ToString().Trim()).Append("</h>");
            //    }

            //    if (element is ParagraphBlock paragraph)
            //    {
            //        if(paragraph.Type == MarkdownBlockType.Paragraph)
            //        {
            //            foreach(MarkdownInline inlineElements in paragraph.Inlines)
            //            {
            //            }
            //        }
            //        Console.WriteLine($"Paragraph: {paragraph.ToString()}");
            //    }
            //}

            var result = Markdown.ToHtml(inputMarkdownText);

            StringBuilder output = new StringBuilder();
            output.Append(_replaceHtmlTagsToFGCompliance(result).Trim());
            return output.ToString();
        }

        private string _replaceHtmlTagsToFGCompliance(string input)
        {
            input = tagReplace(input, "strong", "b");
            input = tagReplace(input, "em", "i");
            input = tagReplace(input, "h1", "h");
            input = tagReplace(input, "code", "frame");
            input = tagReplace(input, "ul", "list");

            return input;
        }

        private string tagReplace(string input, string existingTag, string newTag)
        {
            input = input.Replace("<" + existingTag + ">", "<" + newTag + ">");
            input = input.Replace("</" + existingTag + ">", "</" + newTag + ">");
            return input;
        }
        public void Refresh()
        {
            _npcModel = npcController.GetNPCModel();
            RaisePropertyChanged(nameof(Description));
        }
    }
}
