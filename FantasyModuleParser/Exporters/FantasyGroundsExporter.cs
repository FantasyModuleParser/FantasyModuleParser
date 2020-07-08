using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Models;
using FantasyModuleParser.NPC.Models.Action;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace FantasyModuleParser.Exporters
{
    public class FantasyGroundsExporter : IExporter
    {
        string Immunity;
        string Resistance;

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
                xmlWriter.WriteStartElement("npc");
                //Now, write out each NPC with id-####
                foreach (NPCModel npcModel in moduleModel.NPCModels)
                {
                    
                    xmlWriter.WriteStartElement(npcModel.NPCName.ToLower()); // Open <npcModel.NPCName>
                    //Put together all the innards of the NPC to XML
                    WriteAbilities(xmlWriter, npcModel);
                    WriteAC(xmlWriter, npcModel);
                    WriteActions(xmlWriter, npcModel);
                    WriteAlignment(xmlWriter, npcModel);
                    WriteConditionImmunities(xmlWriter, npcModel);
                    WriteCR(xmlWriter, npcModel);
                    WriteDamageImmunities(xmlWriter, npcModel);
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
                    
                }
                xmlWriter.WriteEndElement(); // Closes </npc>
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

            xmlWriter.WriteStartElement("abilities"); // Open <abilities>
            xmlWriter.WriteStartElement("charisma"); // Open <charisma>
            xmlWriter.WriteStartElement("bonus"); // Open <bonus>
            xmlWriter.WriteAttributeString("type", "number"); // Add type="number"
            xmlWriter.WriteValue(ChaBonus); // Add bonus value
            xmlWriter.WriteEndElement(); // Close </bonus>
            xmlWriter.WriteStartElement("modifier"); // Open <modifier>
            xmlWriter.WriteAttributeString("type", "string"); // Add type="string"
            xmlWriter.WriteValue(ChaModifier + ChaBonus); // Add bonus value with + or -
            xmlWriter.WriteEndElement(); // Close </modifier>
            xmlWriter.WriteStartElement("score"); // Open <score>
            xmlWriter.WriteAttributeString("type", "number"); // Add type="number"
            xmlWriter.WriteValue(npcModel.AttributeCha); // Add Attibute value
            xmlWriter.WriteEndElement(); // Close </score>
            xmlWriter.WriteEndElement(); // Close </charisma>
            xmlWriter.WriteStartElement("constitution"); // Open <constitution>
            xmlWriter.WriteStartElement("bonus"); // Open <bonus>
            xmlWriter.WriteAttributeString("type", "number"); // Add type="number"
            xmlWriter.WriteValue(ConBonus); // Add bonus value
            xmlWriter.WriteEndElement(); // Close </bonus>
            xmlWriter.WriteStartElement("modifier"); // Open <modifier>
            xmlWriter.WriteAttributeString("type", "string"); // Add type="string"
            xmlWriter.WriteValue(ConModifier + ConBonus); // Add bonus value with + or minus
            xmlWriter.WriteEndElement(); // Close </modifier>
            xmlWriter.WriteStartElement("score"); // Open <score>
            xmlWriter.WriteAttributeString("type", "number"); // Add type="number"
            xmlWriter.WriteValue(npcModel.AttributeCon); // Add Attibute value
            xmlWriter.WriteEndElement(); // Close </score>
            xmlWriter.WriteEndElement(); // Close </constitution>
            xmlWriter.WriteStartElement("dexterity"); // Open <dexterity>
            xmlWriter.WriteStartElement("bonus"); // Open <bonus>
            xmlWriter.WriteAttributeString("type", "number"); // Add type="number"
            xmlWriter.WriteValue(DexBonus); // Add bonus value
            xmlWriter.WriteEndElement(); // Close </bonus>
            xmlWriter.WriteStartElement("modifier"); // Open <modifier>
            xmlWriter.WriteAttributeString("type", "string"); // Add type="string"
            xmlWriter.WriteValue(DexModifier + DexBonus); // Add bonus value with + or minus
            xmlWriter.WriteEndElement(); // Close </modifier>
            xmlWriter.WriteStartElement("score"); // Open <score>
            xmlWriter.WriteAttributeString("type", "number"); // Add type="number"
            xmlWriter.WriteValue(npcModel.AttributeDex); // Add Attibute value
            xmlWriter.WriteEndElement(); // Close </score>
            xmlWriter.WriteEndElement(); // Close </dexterity>
            xmlWriter.WriteStartElement("intelligence"); // Open <intelligence>
            xmlWriter.WriteStartElement("bonus"); // Open <bonus>
            xmlWriter.WriteAttributeString("type", "number"); // Add type="number"
            xmlWriter.WriteValue(IntBonus); // Add bonus value
            xmlWriter.WriteEndElement(); // Close </bonus>
            xmlWriter.WriteStartElement("modifier"); // Open <modifier>
            xmlWriter.WriteAttributeString("type", "string"); // Add type="string"
            xmlWriter.WriteValue(IntModifier + IntBonus); // Add bonus value with + or minus
            xmlWriter.WriteEndElement(); // Close </modifier>
            xmlWriter.WriteStartElement("score"); // Open <score>
            xmlWriter.WriteAttributeString("type", "number"); // Add type="number"
            xmlWriter.WriteValue(npcModel.AttributeInt); // Add Attibute value
            xmlWriter.WriteEndElement(); // Close </score>
            xmlWriter.WriteEndElement(); // Close </intelligence>
            xmlWriter.WriteStartElement("strength"); // Open <strength>
            xmlWriter.WriteStartElement("bonus"); // Open <bonus>
            xmlWriter.WriteAttributeString("type", "number"); // Add type="number"
            xmlWriter.WriteValue(StrBonus); // Add bonus value
            xmlWriter.WriteEndElement(); // Close </bonus>
            xmlWriter.WriteStartElement("modifier"); // Open <modifier>
            xmlWriter.WriteAttributeString("type", "string"); // Add type="string"
            xmlWriter.WriteValue(StrModifier + StrBonus); // Add bonus value with + or minus
            xmlWriter.WriteEndElement(); // Close </modifier>
            xmlWriter.WriteStartElement("score"); // Open <score>
            xmlWriter.WriteAttributeString("type", "number"); // Add type="number"
            xmlWriter.WriteValue(npcModel.AttributeStr); // Add Attibute value
            xmlWriter.WriteEndElement(); // Close </score>
            xmlWriter.WriteEndElement(); // Close </strength>
            xmlWriter.WriteStartElement("wisdom"); // Open <wisdom>
            xmlWriter.WriteStartElement("bonus"); // Open <bonus>
            xmlWriter.WriteAttributeString("type", "number"); // Add type="number"
            xmlWriter.WriteValue(WisBonus); // Add bonus value
            xmlWriter.WriteEndElement(); // Close </bonus>
            xmlWriter.WriteStartElement("modifier"); // Open <modifier>
            xmlWriter.WriteAttributeString("type", "string"); // Add type="string"
            xmlWriter.WriteValue(WisModifier + WisBonus); // Add bonus value with + or minus
            xmlWriter.WriteEndElement(); // Close </modifier>
            xmlWriter.WriteStartElement("score"); // Open <score>
            xmlWriter.WriteAttributeString("type", "number"); // Add type="number"
            xmlWriter.WriteValue(npcModel.AttributeWis); // Add Attibute value
            xmlWriter.WriteEndElement(); // Close </score>
            xmlWriter.WriteEndElement(); // Close </intelligence>
            xmlWriter.WriteEndElement(); // Close </abilities>
        }

        
        private void WriteAC(XmlWriter xmlWriter, NPCModel npcModel)
        {
            xmlWriter.WriteStartElement("ac"); // Open <ac>
            xmlWriter.WriteAttributeString("type", "number"); // Add type=number
            xmlWriter.WriteValue(npcModel.AC); // Add AC value
            xmlWriter.WriteEndElement(); // Close </ac>
            xmlWriter.WriteStartElement("actext"); // Open <actext>
            xmlWriter.WriteAttributeString("type", "string"); // Add type=string
            xmlWriter.WriteValue(npcModel.ACText); // Add AC Text string
            xmlWriter.WriteEndElement(); // Close </actext>
        }

        private void WriteActions(XmlWriter xmlWriter, NPCModel npcModel)
        {
            xmlWriter.WriteStartElement("actions"); // Open <actions>
            int actionID = 1;
            foreach (ActionModelBase action in npcModel.NPCActions)
            {
                xmlWriter.WriteStartElement("id-" + actionID.ToString("D4")); // Open <id-####>
                xmlWriter.WriteStartElement("desc"); // Open <desc>
                xmlWriter.WriteAttributeString("type", "string"); // Add type=string
                xmlWriter.WriteString(action.ActionDescription); // Add Action Description
                xmlWriter.WriteEndElement(); // Close </desc>
                xmlWriter.WriteStartElement("name"); // Open <name>
                xmlWriter.WriteAttributeString("type", "string"); // Add type=string
                xmlWriter.WriteString(action.ActionName); // Add Action Name
                xmlWriter.WriteEndElement(); // Close </name>
                xmlWriter.WriteEndElement(); // Close </id-####>
                actionID = ++ actionID;
            }
                xmlWriter.WriteEndElement(); // Close </actions>
        }

        private void WriteAlignment(XmlWriter xmlWriter, NPCModel npcModel)
        {
            xmlWriter.WriteStartElement("alignment"); // Open <alignment>
            xmlWriter.WriteAttributeString("type", "string"); // Add type=string
            xmlWriter.WriteString(npcModel.Alignment); // Add alignment string
            xmlWriter.WriteEndElement(); // Close </alignment>
        }

        private void WriteConditionImmunities(XmlWriter xmlWriter, NPCModel npcModel)
        {
            StringBuilder stringBuilder = new StringBuilder();
            xmlWriter.WriteStartElement("conditionimmunities"); // Open <conditionimmunities>
            xmlWriter.WriteAttributeString("type", "string"); // Add type=string
            
            foreach (SelectableActionModel condition in npcModel.ConditionImmunityModelList)
            {
                if (condition.Selected)
                    stringBuilder.Append(condition.ActionDescription.ToLower()).Append(", ");
            }
            if (npcModel.ConditionOther)
            {
                stringBuilder.Append(npcModel.ConditionOtherText.ToLower() + ", ");
            }
            if (stringBuilder.Length >= 2)
                stringBuilder.Remove(stringBuilder.Length - 2, 2);

            xmlWriter.WriteValue(stringBuilder.ToString().Trim());
            xmlWriter.WriteEndElement(); // Close </conditionimmunities>
        }
        
        private void WriteCR(XmlWriter xmlWriter, NPCModel npcModel)
        {
            xmlWriter.WriteStartElement("cr"); // Open <cr>
            xmlWriter.WriteAttributeString("type", "string"); // Add type=string
            xmlWriter.WriteString(npcModel.ChallengeRating); // Add CR string
            xmlWriter.WriteEndElement(); // Close </cr>
        }

        private void WriteDamageImmunities(XmlWriter xmlWriter, NPCModel npcModel)
        {
            StringBuilder stringBuilder = new StringBuilder();
            xmlWriter.WriteStartElement("damageimmunities"); // Open <damageimmunities>
            xmlWriter.WriteAttributeString("type", "string"); // Add type=string

            foreach (SelectableActionModel damageImmunities in npcModel.DamageImmunityModelList)
            {
                if (damageImmunities.Selected)
                    stringBuilder.Append(damageImmunities.ActionDescription.ToLower()).Append(", ");
            }
            if (stringBuilder.Length >= 2)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }

            stringBuilder.Append("; ");

            foreach (SelectableActionModel specialWeaponImmunity in npcModel.SpecialWeaponImmunityModelList)
            {
                if (specialWeaponImmunity.Selected == true && specialWeaponImmunity.ActionName != "NoSpecial")
                {

                    switch (specialWeaponImmunity.ActionName)
                    {
                        case "Nonmagical":
                            Immunity = " from nonmagical attacks";
                            break;
                        case "NonmagicalSilvered":
                            Immunity = " from nonmagical attacks that aren't silvered";
                            break;
                        case "NonmagicalAdamantine":
                            Immunity = " from nonmagical attacks that aren't adamantine";
                            break;
                        case "NonmagicalColdForgedIron":
                            Immunity = " from nonmagical attacks that aren't cold-forged iron";
                            break;
                    }

                    foreach (SelectableActionModel specialWeaponDmgImmunity in npcModel.SpecialWeaponDmgImmunityModelList)
                    {
                        if (specialWeaponDmgImmunity.Selected)
                            stringBuilder.Append(specialWeaponDmgImmunity.ActionDescription).Append(", ");
                    }
                    if (stringBuilder.Length >= 2)
                    {
                        stringBuilder.Remove(stringBuilder.Length - 2, 2);
                    }
                    stringBuilder.Append(Immunity);
                }
            }

            string weaponDamageImmunityString = stringBuilder.ToString().Trim();
            if (weaponDamageImmunityString.EndsWith(";", true, CultureInfo.CurrentCulture))
                weaponDamageImmunityString = weaponDamageImmunityString.Substring(0, weaponDamageImmunityString.Length - 1);

            xmlWriter.WriteString(weaponDamageImmunityString);
            xmlWriter.WriteEndElement(); // Close </damageimmunities>
        }

        private void WriteDamageResistances(XmlWriter xmlWriter, NPCModel npcModel)
        {
            StringBuilder stringBuilder = new StringBuilder();
            xmlWriter.WriteStartElement("damageresistances"); // Open <damageimmunities>
            xmlWriter.WriteAttributeString("type", "string"); // Add type=string

            foreach (SelectableActionModel damageResistances in npcModel.DamageResistanceModelList)
            {
                if (damageResistances.Selected == true)
                    stringBuilder.Append(damageResistances.ActionDescription.ToLower()).Append(", ");
            }
            if (stringBuilder.Length >= 2)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }
            stringBuilder.Append("; ");
            if (stringBuilder.Length == 2)
            {
                stringBuilder.Remove(0, 2);
            }
            foreach (SelectableActionModel specialWeaponResistances in npcModel.SpecialWeaponResistanceModelList)
            {
                if (specialWeaponResistances.Selected == true && specialWeaponResistances.ActionName != "NoSpecial")
                {
                    if (specialWeaponResistances.ActionName == "Nonmagical")
                    {
                        Resistance = " from nonmagical attacks";
                    }
                    else if (specialWeaponResistances.ActionName == "NonmagicalSilvered")
                    {
                        Resistance = " from nonmagical attacks that aren't silvered";
                    }
                    else if (specialWeaponResistances.ActionName == "NonmagicalAdamantine")
                    {
                        Resistance = " from nonmagical attacks that aren't adamantine";
                    }
                    else if (specialWeaponResistances.ActionName == "NonmagicalColdForgedIron")
                    {
                        Resistance = " from nonmagical attacks that aren't cold-forged iron";
                    }
                    else if (specialWeaponResistances.ActionName == "Magical")
                    {
                        Resistance = " from magic weapons";
                    }
                    foreach (SelectableActionModel specialWeaponDmgResistance in npcModel.SpecialWeaponDmgResistanceModelList)
                    {
                        if (specialWeaponDmgResistance.Selected == true)
                            stringBuilder.Append(specialWeaponDmgResistance.ActionDescription).Append(", ");
                    }
                    if (stringBuilder.Length >= 2)
                    {
                        stringBuilder.Remove(stringBuilder.Length - 2, 2);
                    }
                    stringBuilder.Append(Resistance);
                }
            }

            xmlWriter.WriteValue(stringBuilder.ToString().Trim());
            xmlWriter.WriteEndElement(); // Close </damageresistances>
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
