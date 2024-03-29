﻿using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xaml;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.ViewModels;
using FantasyModuleParser.NPC.Views;
using Markdig;
using Markdig.Wpf;
using XamlReader = System.Windows.Markup.XamlReader;

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
        #region Variables
        private bool _isMarkdownHelpWindowOpen = false;
        #endregion

        public DescriptionUC()
        {
            InitializeComponent();
            PreviewKeyDown += DescriptionUC_PreviewKeyDown;
        }

        private void DescriptionUC_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(Keyboard.Modifiers == ModifierKeys.Control)
            {
                if(e.Key == Key.B)
                    btn_bold_Click(sender, e);
                if (e.Key == Key.I)
                    btn_italics_Click(sender, e);
                if (e.Key == Key.U)
                    btn_underline_Click(sender, e);
            }

            // If the return key is pressed AND the 
        }

        private void ValidateXML(object sender, RoutedEventArgs e)
        {

        }
        public void Refresh()
        {
            if (DataContext != null)
            {
                (DataContext as DescriptionUCViewModel).Refresh();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var markdown = MarkdownTextBox.Text;
            var xaml = Markdig.Wpf.Markdown.ToXaml(markdown, BuildPipeline());
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xaml)))
            {
                using (var reader = new XamlXmlReader(stream, new MyXamlSchemaContext()))
                {
                    if (XamlReader.Load(reader) is FlowDocument document)
                    {
                        MarkdownViewer.Document = document;
                    }
                }
            }
        }
        private void MarkdownHelp_Click(object sender, RoutedEventArgs e)
        {
            if (!_isMarkdownHelpWindowOpen)
            {
                _isMarkdownHelpWindowOpen = true;
                MarkdownHelp markdownHelp = new MarkdownHelp();
                markdownHelp.Closing += MarkdownHelp_Closing;
                markdownHelp.Show();
            }
        }

        private void MarkdownHelp_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _isMarkdownHelpWindowOpen = false;
        }

        private static MarkdownPipeline BuildPipeline()
        {
            return new MarkdownPipelineBuilder()
                .UseSupportedExtensions()
                .Build();
        }

        class MyXamlSchemaContext : XamlSchemaContext
        {
            public override bool TryGetCompatibleXamlNamespace(string xamlNamespace, out string compatibleNamespace)
            {
                if (xamlNamespace.Equals("clr-namespace:Markdig.Wpf", StringComparison.Ordinal))
                {
                    compatibleNamespace = $"clr-namespace:Markdig.Wpf;assembly={Assembly.GetAssembly(typeof(Markdig.Wpf.Styles)).FullName}";
                    return true;
                }
                return base.TryGetCompatibleXamlNamespace(xamlNamespace, out compatibleNamespace);
            }
        }

        private void btn_bold_Click(object sender, RoutedEventArgs e)
        {
            applySelectedTextMarkdownMod("**");
        }

        private void btn_italics_Click(object sender, RoutedEventArgs e)
        {
            applySelectedTextMarkdownMod("*");
        }

        private void btn_underline_Click(object sender, RoutedEventArgs e)
        {
            applySelectedTextMarkdownMod("++");
        }

        private void btn_header_Click(object sender, RoutedEventArgs e)
        {
            applySelectedTextMarkdownMod("# ", " #");
        }

        private void btn_text_Click(object sender, RoutedEventArgs e)
        {
           // I think this is supposed to reset all mods?
        }

        private void btn_chatbox_Click(object sender, RoutedEventArgs e)
        {
            applySelectedTextMarkdownMod("`");
        }

        private void applySelectedTextMarkdownMod(String markdownMod)
        {
            applySelectedTextMarkdownMod(markdownMod, markdownMod);
        }
        private void applySelectedTextMarkdownMod(String markdownMod, String markdownModSuffix)
        {
            String selectedText = MarkdownTextBox.SelectedText;
            if (!String.IsNullOrEmpty(selectedText))
            { 
                MarkdownTextBox.SelectedText = markdownMod + selectedText + markdownModSuffix;
            }
        }

        private void btn_bullet_Click(object sender, RoutedEventArgs e)
        {
            applySelectedTextMarkdownMod("* ", "");
        }

        private void btn_ClearText_Click(object sender, RoutedEventArgs e)
        {
            MarkdownTextBox.Text = "";
        }
    }
}
