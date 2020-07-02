using FantasyModuleParser.NPC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
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

            if (moduleModel.Name == null || moduleModel.Name.Length == 0)
            {
                throw new ApplicationException("No Module Name has been set");
            }

            string moduleFolderPath = Path.Combine(moduleModel.ModulePath, moduleModel.Name);

            // Create the folder all content will go into based on the Module name
            System.IO.Directory.CreateDirectory(moduleFolderPath);

            // A blank module consists of two files;  db.xml & definition.xml
            string dbXmlFileContent = GenerateDBXmlFile(moduleModel);
            string definitionXmlFileContent = GenerateDefinitionXmlContent(moduleModel);

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(moduleFolderPath, "db.xml")))
            {
                outputFile.WriteLine(dbXmlFileContent);
            }

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(moduleFolderPath, "definition.xml")))
            {
                outputFile.WriteLine(definitionXmlFileContent);
            }

            //TODO:  Write out any other file content below!

            // ============================================

            // Zipping up the folder contents and naming to *.mod

            // First need to check if the file exists;  If so, delete it
            if (File.Exists(@Path.Combine(moduleModel.ModulePath, moduleModel.Name + ".mod")))
            {
                File.Delete(@Path.Combine(moduleModel.ModulePath, moduleModel.Name + ".mod"));
            }
            ZipFile.CreateFromDirectory(moduleFolderPath, Path.Combine(moduleModel.ModulePath, moduleModel.Name + ".mod"));
        }

        public string GenerateDBXmlFile(ModuleModel moduleModel)
        {
            //Blank Mod:
            /*
             *  <?xml version="1.0" encoding="utf-8"?>
             *  <root version="4" dataversion="20200528" release="8|CoreRPG:4" />
             */

            XmlDocument xml = new XmlDocument();

            XmlElement rootElement = xml.CreateElement("root");
            rootElement.SetAttribute("version", "4"); // Not sure how relevant this is
            rootElement.SetAttribute("dataversion", DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
            rootElement.SetAttribute("release", "8|CoreRPG:4");
            xml.AppendChild(rootElement);

            return writeXmlDocumentToString(xml);

        }

        public string GenerateDefinitionXmlContent(ModuleModel moduleModel)
        {
            XmlDocument xml = new XmlDocument();

            XmlElement rootElement = xml.CreateElement("root");
            rootElement.SetAttribute("version", "4"); // Not sure how relevant this is

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
            using (StringWriter sw = new StringWriterWithEncoding())
            using (XmlWriter writer = XmlWriter.Create(sw, settings))
            {
                xmlDocument.WriteContentTo(writer);
                writer.Close();
                document = sw.ToString();
            }

            return document;
        }

        public sealed class StringWriterWithEncoding : StringWriter
        {
            private readonly Encoding encoding;

            public StringWriterWithEncoding() { }

            public StringWriterWithEncoding(Encoding encoding)
            {
                this.encoding = encoding;
            }

            public override Encoding Encoding
            {
                get { return encoding; }
            }
        }
    }
}
