using FantasyModuleParser.NPC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FantasyModuleParser.Exporters
{
    public class FantasyGroundsExporter : IExporter
    {
        public void CreateModule(ModuleModel moduleModel)
        {
            if (moduleModel.ModulePath == null || moduleModel.ModulePath.Length == 0)
            {
                throw new ApplicationException("No Module Path has been set");
            }

            // A blank module consists of two files;  db.xml & definition.xml
        }

        private void GenerateDBXmlFile(ModuleModel moduleModel)
        {

        }

        public string GenerateDefinitionXmlContent(ModuleModel moduleModel)
        {
            XmlDocument xml = new XmlDocument();

            XmlElement rootElement = xml.CreateElement("root");
            rootElement.SetAttribute("version", "3.3"); // Not sure how relevant this is

            xml.AppendChild(rootElement);

            XmlElement nameElement = xml.CreateElement("name");
            nameElement.InnerText = moduleModel.Name;

            XmlElement categoryElement = xml.CreateElement("category");
            categoryElement.InnerText = moduleModel.Category;

            XmlElement authorElement = xml.CreateElement("author");
            authorElement.InnerText = moduleModel.Author;

            XmlElement ruleSetElement = xml.CreateElement("ruleset");
            ruleSetElement.InnerText = "5E";

            rootElement.AppendChild(nameElement);
            rootElement.AppendChild(categoryElement);
            rootElement.AppendChild(authorElement);
            rootElement.AppendChild(ruleSetElement);

            xml.AppendChild(rootElement);

            return writeXmlDocumentToString(xml);
        }

        private string writeXmlDocumentToString(XmlDocument xmlDocument)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                ConformanceLevel = ConformanceLevel.Document,
                OmitXmlDeclaration = false,
                CloseOutput = true,
                Indent = true,
                IndentChars = "  ",
                NewLineHandling = NewLineHandling.Replace
            };

            string document = "";
            using (StringWriter sw = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sw, settings))
            {
                xmlDocument.WriteContentTo(writer);
                writer.Close();
                document = sw.ToString();
            }

            return document;
        }
    }
}
