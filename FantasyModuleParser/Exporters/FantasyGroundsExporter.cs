using FantasyModuleParser.NPC;
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
            using (StringWriter sw = new StringWriterWithEncoding(Encoding.UTF8))
            using (XmlWriter xmlWriter = XmlWriter.Create(sw, GetXmlWriterSettings()))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("root");
                xmlWriter.WriteAttributeString("version", "4");
                xmlWriter.WriteAttributeString("dataversion", "20200528");
                xmlWriter.WriteAttributeString("release", "8|CoreRPG:4");

                //Now, write out each NPC with id-####
                int _idNumber = 1;
                foreach (NPCModel npcModel in moduleModel.NPCModels)
                {
                    xmlWriter.WriteStartElement("npc");
                    xmlWriter.WriteStartElement("id-" + _idNumber.ToString("D4"));
                    
                    //Put together all the innards of the NPC to XML
                    
                    WriteAbilities(xmlWriter, npcModel);
                    WriteAC(xmlWriter, npcModel);
                    WriteActions(xmlWriter, npcModel);
                    WriteHP(xmlWriter, npcModel);
                    WriteInnateSpells(xmlWriter, npcModel);
                    WriteLairActions(xmlWriter, npcModel);
                    WriteLegendaryActions(xmlWriter, npcModel);
                    WriteName(xmlWriter, npcModel);
                    WriteReactions(xmlWriter, npcModel);
                    WriteSpells(xmlWriter, npcModel);
                    WriteSpellSlots(xmlWriter, npcModel);
                    WriteText(xmlWriter, npcModel);
                    WriteToken(xmlWriter, npcModel);
                    WriteTraits(xmlWriter, npcModel);
                    WriteXP(xmlWriter, npcModel);

                    xmlWriter.WriteEndElement(); // Closes </id-####>
                    xmlWriter.WriteEndElement(); // Closes </npc>
                }

                xmlWriter.WriteEndElement(); // Closes </root>
                xmlWriter.WriteEndDocument();
                xmlWriter.Close();

                return sw.ToString();
            }
        }

        private void WriteAbilities(XmlWriter xmlWriter, NPCModel npcModel)
        {

        }

        private void WriteAC(XmlWriter xmlWriter, NPCModel npcModel)
        {

        }

        private void WriteActions(XmlWriter xmlWriter, NPCModel npcModel)
        {

        }

        private void WriteHP(XmlWriter xmlWriter, NPCModel npcModel)
        {

        }

        private void WriteInnateSpells(XmlWriter xmlWriter, NPCModel npcModel)
        {

        }

        private void WriteLairActions(XmlWriter xmlWriter, NPCModel npcModel)
        {

        }
        private void WriteLegendaryActions(XmlWriter xmlWriter, NPCModel npcModel)
        {

        }
        private void WriteName(XmlWriter xmlWriter, NPCModel npcModel)
        {

        }
        private void WriteReactions(XmlWriter xmlWriter, NPCModel npcModel)
        {

        }
        private void WriteSpells(XmlWriter xmlWriter, NPCModel npcModel)
        {

        }
        private void WriteSpellSlots(XmlWriter xmlWriter, NPCModel npcModel)
        {

        }
        private void WriteText(XmlWriter xmlWriter, NPCModel npcModel)
        {

        }
        private void WriteToken(XmlWriter xmlWriter, NPCModel npcModel)
        {

        }
        private void WriteTraits(XmlWriter xmlWriter, NPCModel npcModel)
        {

        }
        private void WriteXP(XmlWriter xmlWriter, NPCModel npcModel)
        {

        }

        public string GenerateDefinitionXmlContent(ModuleModel moduleModel)
        {
            using (StringWriter sw = new StringWriterWithEncoding(Encoding.UTF8))
            using (XmlWriter xmlWriter = XmlWriter.Create(sw, GetXmlWriterSettings()))
            {
               

                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("root");
                xmlWriter.WriteAttributeString("version", "4");

                // <name>
                xmlWriter.WriteStartElement("name");
                xmlWriter.WriteString(moduleModel.Name);
                xmlWriter.WriteEndElement();

                // <category>
                xmlWriter.WriteStartElement("category");
                xmlWriter.WriteString(moduleModel.Category);
                xmlWriter.WriteEndElement();

                // <author>
                xmlWriter.WriteStartElement("author");
                xmlWriter.WriteString(moduleModel.Author);
                xmlWriter.WriteEndElement();

                // <ruleset>
                xmlWriter.WriteStartElement("ruleset");
                xmlWriter.WriteString("5E");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndDocument();
                xmlWriter.Close();

                return sw.ToString();
            }
        }

        private XmlWriterSettings GetXmlWriterSettings()
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

            return settings;
        }
        private string writeXmlDocumentToString(XmlDocument xmlDocument)
        {
            XmlWriterSettings settings = GetXmlWriterSettings();

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
