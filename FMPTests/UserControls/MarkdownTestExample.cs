using System;
using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMPTests.UserControls
{
    [TestClass]
    public class MarkdownTestExample
    {
        [TestMethod]
        public void TestMethod1()
        {
            string md = "### Some Header 3\r\nThis is **Markdown**";
            MarkdownDocument document = new MarkdownDocument();
            document.Parse(md);

            // Takes note of all of the Top Level Headers.
            foreach (var element in document.Blocks)
            {
                if (element is HeaderBlock header)
                {
                    Console.WriteLine($"Header: {header.ToString()}");
                }

                if (element is ParagraphBlock paragraph)
                {
                    Console.WriteLine($"Paragraph: {paragraph.ToString()}");
                }
            }
        }
    }
}
