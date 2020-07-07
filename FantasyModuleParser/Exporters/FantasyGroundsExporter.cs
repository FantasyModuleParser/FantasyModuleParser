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
                foreach (NPCModel npcModel in moduleModel.NPCModels)
                {
                    xmlWriter.WriteStartElement("npc");
                    xmlWriter.WriteStartElement(npcModel.NPCName.ToLower());
                    
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

                    xmlWriter.WriteEndElement(); // Closes </npcModel.NPCName>
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
            int ChaBonus = -5 + (npcModel.AttributeCha / 2);
            int ConBonus = -5 + (npcModel.AttributeCon / 2);
            int DexBonus = -5 + (npcModel.AttributeDex / 2);
            int IntBonus = -5 + (npcModel.AttributeInt / 2);
            int StrBonus = -5 + (npcModel.AttributeStr / 2);
            int WisBonus = -5 + (npcModel.AttributeWis / 2);

            string ChaModifier;
            string ConModifier;
            string DexModifier;
            string IntModifier;
            string StrModifier;
            string WisModifier;

            if (npcModel.AttributeCha >= 10)
                ChaModifier = "+";
            else
                ChaModifier = "-";

            if (npcModel.AttributeCon >= 10)
                ConModifier = "+";
            else
                ConModifier = "-";

            if (npcModel.AttributeDex >= 10)
                DexModifier = "+";
            else
                DexModifier = "-";

            if (npcModel.AttributeInt >= 10)
                IntModifier = "+";
            else
                IntModifier = "-";

            if (npcModel.AttributeStr >= 10)
                StrModifier = "+";
            else
                StrModifier = "-";

            if (npcModel.AttributeWis >= 10)
                WisModifier = "+";
            else
                WisModifier = "-";

            xmlWriter.WriteStartElement("abilities");
            xmlWriter.WriteStartElement("charisma");
            xmlWriter.WriteStartElement("bonus");
            xmlWriter.WriteAttributeString("type", "number");
            xmlWriter.WriteValue(ChaBonus);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("modifier");
            xmlWriter.WriteAttributeString("type", "string");
            xmlWriter.WriteValue(ChaModifier + ChaBonus);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("score");
            xmlWriter.WriteAttributeString("type", "number");
            xmlWriter.WriteValue(npcModel.AttributeCha);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("constitution");
            xmlWriter.WriteStartElement("bonus");
            xmlWriter.WriteAttributeString("type", "number");
            xmlWriter.WriteValue(ConBonus);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("modifier");
            xmlWriter.WriteAttributeString("type", "string");
            xmlWriter.WriteValue(ConModifier + ConBonus);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("score");
            xmlWriter.WriteAttributeString("type", "number");
            xmlWriter.WriteValue(npcModel.AttributeCon);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("dexterity");
            xmlWriter.WriteStartElement("bonus");
            xmlWriter.WriteAttributeString("type", "number");
            xmlWriter.WriteValue(DexBonus);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("modifier");
            xmlWriter.WriteAttributeString("type", "string");
            xmlWriter.WriteValue(DexModifier + DexBonus);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("score");
            xmlWriter.WriteAttributeString("type", "number");
            xmlWriter.WriteValue(npcModel.AttributeDex);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("intelligence");
            xmlWriter.WriteStartElement("bonus");
            xmlWriter.WriteAttributeString("type", "number");
            xmlWriter.WriteValue(IntBonus);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("modifier");
            xmlWriter.WriteAttributeString("type", "string");
            xmlWriter.WriteValue(IntModifier + IntBonus);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("score");
            xmlWriter.WriteAttributeString("type", "number");
            xmlWriter.WriteValue(npcModel.AttributeInt);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("strength");
            xmlWriter.WriteStartElement("bonus");
            xmlWriter.WriteAttributeString("type", "number");
            xmlWriter.WriteValue(StrBonus);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("modifier");
            xmlWriter.WriteAttributeString("type", "string");
            xmlWriter.WriteValue(StrModifier + StrBonus);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("score");
            xmlWriter.WriteAttributeString("type", "number");
            xmlWriter.WriteValue(npcModel.AttributeStr);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("wisdom");
            xmlWriter.WriteStartElement("bonus");
            xmlWriter.WriteAttributeString("type", "number");
            xmlWriter.WriteValue(WisBonus);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("modifier");
            xmlWriter.WriteAttributeString("type", "string");
            xmlWriter.WriteValue(WisModifier + WisBonus);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("score");
            xmlWriter.WriteAttributeString("type", "number");
            xmlWriter.WriteValue(npcModel.AttributeWis);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
        }

        
        private void WriteAC(XmlWriter xmlWriter, NPCModel npcModel)
        {
            xmlWriter.WriteStartElement("ac");
            xmlWriter.WriteAttributeString("type", "number");
            xmlWriter.WriteValue(npcModel.AC);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("actext");
            xmlWriter.WriteAttributeString("type", "string");
            xmlWriter.WriteValue(npcModel.ACText);
            xmlWriter.WriteEndElement();
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
