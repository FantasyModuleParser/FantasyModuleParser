using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Skills;
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

                // For the Blank DB XML unit test, need to check if any NPCs exist
                if (moduleModel.Categories != null && moduleModel.Categories.Count > 0
                    && moduleModel.Categories[0].NPCModels.Count > 0)
                {
                    xmlWriter.WriteStartElement("npc");

                    //Category section in the XML generation
                    foreach (CategoryModel categoryModel in moduleModel.Categories)
                    {
                        xmlWriter.WriteStartElement("category");

                        //Now, write out each NPC with id-####
                        foreach (NPCModel npcModel in categoryModel.NPCModels)
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
                            WriteDamageResistances(xmlWriter, npcModel);
                            WriteDamageVulnerabilities(xmlWriter, npcModel);
                            WriteHP(xmlWriter, npcModel);
                            WriteLairActions(xmlWriter, npcModel);
                            WriteLanguages(xmlWriter, npcModel);
                            WriteLegendaryActions(xmlWriter, npcModel);
                            WriteName(xmlWriter, npcModel);
                            WriteReactions(xmlWriter, npcModel);
                            WriteSavingThrows(xmlWriter, npcModel);
                            WriteSenses(xmlWriter, npcModel);
                            WriteSkills(xmlWriter, npcModel);
                            WriteText(xmlWriter, npcModel);
                            WriteToken(xmlWriter, npcModel);
                            WriteTraits(xmlWriter, npcModel);
                            WriteXP(xmlWriter, npcModel);

                            xmlWriter.WriteEndElement(); // Closes </npcModel.NPCName>

                        }

                        xmlWriter.WriteEndElement(); // Close </category>


                    }
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

            // By default, Damage Immunities & Special Weapon Immunities are separated by a ';' symbol.
            // If no SpecialWeaponImmunities are selected, the ';' character is removed at the end of this method.
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
            xmlWriter.WriteStartElement("damageresistances"); // Open <damageresistances>
            xmlWriter.WriteAttributeString("type", "string"); // Add type=string

            foreach (SelectableActionModel damageResistances in npcModel.DamageResistanceModelList)
            {
                if (damageResistances.Selected)
                    stringBuilder.Append(damageResistances.ActionDescription.ToLower()).Append(", ");
            }
            if (stringBuilder.Length >= 2)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }

            // By default, Damage Resistances & Special Weapon Resistances are separated by a ';' symbol.
            // If no SpecialWeaponResistances are selected, the ';' character is removed at the end of this method.
            stringBuilder.Append("; ");

            foreach (SelectableActionModel specialWeaponResistance in npcModel.SpecialWeaponResistanceModelList)
            {
                if (specialWeaponResistance.Selected == true && specialWeaponResistance.ActionName != "NoSpecial")
                {

                    switch (specialWeaponResistance.ActionName)
                    {
                        case "Nonmagical":
                            Resistance = " from nonmagical attacks";
                            break;
                        case "NonmagicalSilvered":
                            Resistance = " from nonmagical attacks that aren't silvered";
                            break;
                        case "NonmagicalAdamantine":
                            Resistance = " from nonmagical attacks that aren't adamantine";
                            break;
                        case "NonmagicalColdForgedIron":
                            Resistance = " from nonmagical attacks that aren't cold-forged iron";
                            break;
                    }

                    foreach (SelectableActionModel specialWeaponDmgResistance in npcModel.SpecialWeaponDmgResistanceModelList)
                    {
                        if (specialWeaponDmgResistance.Selected)
                            stringBuilder.Append(specialWeaponDmgResistance.ActionDescription).Append(", ");
                    }
                    if (stringBuilder.Length >= 2)
                    {
                        stringBuilder.Remove(stringBuilder.Length - 2, 2);
                    }
                    stringBuilder.Append(Resistance);
                }
            }

            string weaponDamageResistanceString = stringBuilder.ToString().Trim();
            if (weaponDamageResistanceString.EndsWith(";", true, CultureInfo.CurrentCulture))
                weaponDamageResistanceString = weaponDamageResistanceString.Substring(0, weaponDamageResistanceString.Length - 1);

            xmlWriter.WriteString(weaponDamageResistanceString);
            xmlWriter.WriteEndElement(); // Close </damageresistances>
        }

        private void WriteDamageVulnerabilities(XmlWriter xmlWriter, NPCModel npcModel)
        {
            StringBuilder stringBuilder = new StringBuilder();
            xmlWriter.WriteStartElement("damagevulnerabilities"); // Open <damagevulnerabilities>
            xmlWriter.WriteAttributeString("type", "string"); // Add type=string

            foreach (SelectableActionModel damageVulnerabilities in npcModel.DamageVulnerabilityModelList)
            {
                if (damageVulnerabilities.Selected == true)
                    stringBuilder.Append(damageVulnerabilities.ActionDescription.ToLower()).Append(", ");
            }
            if (stringBuilder.Length >= 2)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }
            string weaponDamageVulnerabilityString = stringBuilder.ToString().Trim();

            xmlWriter.WriteValue(weaponDamageVulnerabilityString);
            xmlWriter.WriteEndElement(); // Close </damagevulnerabilities>
        }
        private void WriteHP(XmlWriter xmlWriter, NPCModel npcModel)
        {
            string[] hpArray = npcModel.HP.Split('(');
            string hpValue = hpArray[0].Trim(); // Removes any whitespace
            string hpDieBreakdown = "(" + hpArray[1];

            xmlWriter.WriteStartElement("hd"); // Open <hd>
            xmlWriter.WriteAttributeString("type", "string"); // Add type=string
            xmlWriter.WriteString(hpDieBreakdown); // Write HP formula
            xmlWriter.WriteEndElement(); // Close </hd>
            xmlWriter.WriteStartElement("hp"); // Open <hp>
            xmlWriter.WriteAttributeString("type", "number"); // Add type=number
            xmlWriter.WriteString(hpValue); // Write HP value
            xmlWriter.WriteEndElement(); // Close </hp>
        }

        private void WriteLairActions(XmlWriter xmlWriter, NPCModel npcModel)
        {
            xmlWriter.WriteStartElement("lairactions"); // Open <actions>
            int actionID = 1;
            foreach (LairAction lairaction in npcModel.LairActions)
            {
                xmlWriter.WriteStartElement("id-" + actionID.ToString("D4")); // Open <id-####>
                xmlWriter.WriteStartElement("desc"); // Open <desc>
                xmlWriter.WriteAttributeString("type", "string"); // Add type=string
                xmlWriter.WriteString(lairaction.ActionDescription); // Add Action Description
                xmlWriter.WriteEndElement(); // Close </desc>
                xmlWriter.WriteStartElement("name"); // Open <name>
                xmlWriter.WriteAttributeString("type", "string"); // Add type=string
                xmlWriter.WriteString(lairaction.ActionName); // Add Action Name
                xmlWriter.WriteEndElement(); // Close </name>
                xmlWriter.WriteEndElement(); // Close </id-####>
                actionID = ++actionID;
            }
            xmlWriter.WriteEndElement(); // Close </lairactions>
        }

        private void WriteLanguages(XmlWriter xmlWriter, NPCModel npcModel)
        {
            StringBuilder stringBuilder = new StringBuilder();
            xmlWriter.WriteStartElement("languages"); // Open <languages>
            xmlWriter.WriteAttributeString("type", "string"); // Add type=string

            foreach (LanguageModel standardLanguage in npcModel.StandardLanguages)
            {
                if (standardLanguage.Selected == true)
                    stringBuilder.Append(standardLanguage.Language.ToLower()).Append(", ");
            }
            foreach (LanguageModel monstrousLanguage in npcModel.MonstrousLanguages)
            {
                if (monstrousLanguage.Selected == true)
                    stringBuilder.Append(monstrousLanguage.Language.ToLower()).Append(", ");
            }
            foreach (LanguageModel exoticLanguage in npcModel.ExoticLanguages)
            {
                if (exoticLanguage.Selected == true)
                    stringBuilder.Append(exoticLanguage.Language.ToLower()).Append(", ");
            }
            if (npcModel.Telepathy == true)
            {
                stringBuilder.Append("telepathy " + npcModel.TelepathyRange).Append(", ");
            }
            if (stringBuilder.Length >= 2)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }
            string languageString = stringBuilder.ToString().Trim();

            xmlWriter.WriteValue(languageString);
            xmlWriter.WriteEndElement(); // Close </languages>
        }

        private void WriteLegendaryActions(XmlWriter xmlWriter, NPCModel npcModel)
        {
            xmlWriter.WriteStartElement("legendaryactions"); // Open <legendaryactions>
            int actionID = 1;
            foreach (LegendaryActionModel legendaryaction in npcModel.LegendaryActions)
            {
                xmlWriter.WriteStartElement("id-" + actionID.ToString("D4")); // Open <id-####>
                xmlWriter.WriteStartElement("desc"); // Open <desc>
                xmlWriter.WriteAttributeString("type", "string"); // Add type=string
                xmlWriter.WriteString(legendaryaction.ActionDescription); // Add Action Description
                xmlWriter.WriteEndElement(); // Close </desc>
                xmlWriter.WriteStartElement("name"); // Open <name>
                xmlWriter.WriteAttributeString("type", "string"); // Add type=string
                xmlWriter.WriteString(legendaryaction.ActionName); // Add Action Name
                xmlWriter.WriteEndElement(); // Close </name>
                xmlWriter.WriteEndElement(); // Close </id-####>
                actionID = ++actionID;
            }
            xmlWriter.WriteEndElement(); // Close </legendaryactions>
        }
        
        private void WriteName(XmlWriter xmlWriter, NPCModel npcModel)
        {
            xmlWriter.WriteStartElement("name"); // Open <name>
            xmlWriter.WriteAttributeString("type", "string"); // Add type=string
            xmlWriter.WriteString(npcModel.NPCName); // Add NPC name
            xmlWriter.WriteEndElement(); // Close </name>
        }
        private void WriteReactions(XmlWriter xmlWriter, NPCModel npcModel)
        {
            xmlWriter.WriteStartElement("reactions"); // Open <reactions>
            int actionID = 1;
            foreach (ActionModelBase reaction in npcModel.Reactions)
            {
                xmlWriter.WriteStartElement("id-" + actionID.ToString("D4")); // Open <id-####>
                xmlWriter.WriteStartElement("desc"); // Open <desc>
                xmlWriter.WriteString(reaction.ActionDescription); // Add Action Description
                xmlWriter.WriteEndElement(); // Close </desc>
                xmlWriter.WriteStartElement("name"); // Open <name>
                xmlWriter.WriteAttributeString("type", "string"); // Add type=string
                xmlWriter.WriteString(reaction.ActionName); // Add Action Name
                xmlWriter.WriteEndElement(); // Close </name>
                xmlWriter.WriteEndElement(); // Close </id-####>
                actionID = ++actionID;
            }
            xmlWriter.WriteEndElement(); // Close </reactions>
        }

        private void WriteSavingThrows(XmlWriter xmlWriter, NPCModel npcModel)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (npcModel.SavingThrowStr != 0 || npcModel.SavingThrowStrBool)
                stringBuilder.Append("Str ").Append(npcModel.SavingThrowStr >= 0 ? "+" : "").Append(npcModel.SavingThrowStr).Append(", ");
            if (npcModel.SavingThrowDex != 0 || npcModel.SavingThrowDexBool)
                stringBuilder.Append("Dex ").Append(npcModel.SavingThrowDex >= 0 ? "+" : "").Append(npcModel.SavingThrowDex).Append(", ");
            if (npcModel.SavingThrowCon != 0 || npcModel.SavingThrowConBool)
                stringBuilder.Append("Con ").Append(npcModel.SavingThrowCon >= 0 ? "+" : "").Append(npcModel.SavingThrowCon).Append(", ");
            if (npcModel.SavingThrowInt != 0 || npcModel.SavingThrowIntBool)
                stringBuilder.Append("Int ").Append(npcModel.SavingThrowInt >= 0 ? "+" : "").Append(npcModel.SavingThrowInt).Append(", ");
            if (npcModel.SavingThrowWis != 0 || npcModel.SavingThrowWisBool)
                stringBuilder.Append("Wis ").Append(npcModel.SavingThrowWis >= 0 ? "+" : "").Append(npcModel.SavingThrowWis).Append(", ");
            if (npcModel.SavingThrowCha != 0 || npcModel.SavingThrowChaBool)
                stringBuilder.Append("Cha ").Append(npcModel.SavingThrowCha >= 0 ? "+" : "").Append(npcModel.SavingThrowCha).Append(", ");

            if (stringBuilder.Length >= 2)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }
            string savingThrowString = stringBuilder.ToString().Trim();

            xmlWriter.WriteStartElement("savingthrows"); // Open <savingthrows>
            xmlWriter.WriteAttributeString("type", "string"); // Add type=string
            xmlWriter.WriteValue(savingThrowString);
            xmlWriter.WriteEndElement(); // Close </savingthrows>
        }

        private void WriteSenses(XmlWriter xmlWriter, NPCModel npcModel)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (npcModel.Speed > 0)
                stringBuilder.Append(npcModel.Speed + " ft.").Append(", ");
            if (npcModel.BlindBeyond)
                stringBuilder.Append("blindsight " + npcModel.Blindsight + " ft. (blind beyond this radius)").Append(", ");
            if (npcModel.Blindsight > 0 && !npcModel.BlindBeyond)
                stringBuilder.Append("blindsight " + npcModel.Blindsight + " ft.").Append(", ");
            if (npcModel.Burrow > 0)
                stringBuilder.Append("burrow " + npcModel.Burrow + " ft.").Append(", ");
            if (npcModel.Climb > 0)
                stringBuilder.Append("climb " + npcModel.Climb + " ft.").Append(", ");
            if (npcModel.Hover)
                stringBuilder.Append("fly " + npcModel.Fly + " ft. (hover)").Append(", ");
            if (npcModel.Fly > 0 && !npcModel.Hover)
                stringBuilder.Append("fly " + npcModel.Fly + " ft.").Append(", ");
            if (npcModel.Swim > 0)
                stringBuilder.Append("swim " + npcModel.Swim + " ft.").Append(", ");
            stringBuilder.Append("passive Perception " + npcModel.PassivePerception);
            string sensesString = stringBuilder.ToString().Trim();

            xmlWriter.WriteStartElement("senses"); // Open <senses>
            xmlWriter.WriteAttributeString("type", "string"); // Add type=string
            xmlWriter.WriteValue(sensesString);
            xmlWriter.WriteEndElement(); // Close </senses>
        }

        private void WriteSkills(XmlWriter xmlWriter, NPCModel npcModel)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (npcModel.Acrobatics != 0)
                stringBuilder.Append("Acrobatics ").Append(npcModel.Acrobatics >= 0 ? "+" : "").Append(npcModel.Acrobatics).Append(", ");
            if (npcModel.AnimalHandling != 0)
                stringBuilder.Append("Animal Handling ").Append(npcModel.AnimalHandling >= 0 ? "+" : "").Append(npcModel.AnimalHandling).Append(", ");
            if (npcModel.Arcana != 0)
                stringBuilder.Append("Arcana ").Append(npcModel.Arcana >= 0 ? "+" : "").Append(npcModel.Arcana).Append(", ");
            if (npcModel.Athletics != 0)
                stringBuilder.Append("Athletics ").Append(npcModel.Athletics >= 0 ? "+" : "").Append(npcModel.Athletics).Append(", ");
            if (npcModel.Deception != 0)
                stringBuilder.Append("Deception ").Append(npcModel.Deception >= 0 ? "+" : "").Append(npcModel.Deception).Append(", ");
            if (npcModel.History != 0)
                stringBuilder.Append("History ").Append(npcModel.History >= 0 ? "+" : "").Append(npcModel.History).Append(", ");
            if (npcModel.Insight != 0)
                stringBuilder.Append("Insight ").Append(npcModel.Insight >= 0 ? "+" : "").Append(npcModel.Insight).Append(", ");
            if (npcModel.Intimidation != 0)
                stringBuilder.Append("Intimidation ").Append(npcModel.Intimidation >= 0 ? "+" : "").Append(npcModel.Intimidation).Append(", ");
            if (npcModel.Investigation != 0)
                stringBuilder.Append("Investigation ").Append(npcModel.Investigation >= 0 ? "+" : "").Append(npcModel.Investigation).Append(", ");
            if (npcModel.Medicine != 0)
                stringBuilder.Append("Medicine ").Append(npcModel.Medicine >= 0 ? "+" : "").Append(npcModel.Medicine).Append(", ");
            if (npcModel.Nature != 0)
                stringBuilder.Append("Nature ").Append(npcModel.Nature >= 0 ? "+" : "").Append(npcModel.Nature).Append(", ");
            if (npcModel.Perception != 0)
                stringBuilder.Append("Perception ").Append(npcModel.Perception >= 0 ? "+" : "").Append(npcModel.Perception).Append(", ");
            if (npcModel.Performance != 0)
                stringBuilder.Append("Performance ").Append(npcModel.Performance >= 0 ? "+" : "").Append(npcModel.Performance).Append(", ");
            if (npcModel.Persuasion != 0)
                stringBuilder.Append("Persuasion ").Append(npcModel.Persuasion >= 0 ? "+" : "").Append(npcModel.Persuasion).Append(", ");
            if (npcModel.Religion != 0)
                stringBuilder.Append("Religion ").Append(npcModel.Religion >= 0 ? "+" : "").Append(npcModel.Religion).Append(", ");
            if (npcModel.SleightOfHand != 0)
                stringBuilder.Append("SleightOfHand ").Append(npcModel.SleightOfHand >= 0 ? "+" : "").Append(npcModel.SleightOfHand).Append(", ");
            if (npcModel.Stealth != 0)
                stringBuilder.Append("Stealth ").Append(npcModel.Stealth >= 0 ? "+" : "").Append(npcModel.Stealth).Append(", ");
            if (npcModel.Survival != 0)
                stringBuilder.Append("Survival ").Append(npcModel.Survival >= 0 ? "+" : "").Append(npcModel.Survival).Append(", ");

            if (stringBuilder.Length >= 2)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }
            string skillsString = stringBuilder.ToString().Trim();

            xmlWriter.WriteStartElement("skills"); // Open <skills>
            xmlWriter.WriteAttributeString("type", "string"); // Add type=string
            xmlWriter.WriteValue(skillsString);
            xmlWriter.WriteEndElement(); // Close </skills>
        }

        private void WriteText(XmlWriter xmlWriter, NPCModel npcModel)
        {
            
        }
        private void WriteToken(XmlWriter xmlWriter, NPCModel npcModel)
        {
            xmlWriter.WriteStartElement("token"); // Open <token>
            xmlWriter.WriteAttributeString("type", "token"); // Add type=token
            xmlWriter.WriteValue(npcModel.NPCToken);
            xmlWriter.WriteEndElement(); // Close </token>
        }
        private void WriteTraits(XmlWriter xmlWriter, NPCModel npcModel)
        {
            xmlWriter.WriteStartElement("traits"); // Open <actions>
            int actionID = 1;
            string actionName = "";

            foreach (ActionModelBase traits in npcModel.Traits)
            {
                xmlWriter.WriteStartElement("id-" + actionID.ToString("D4")); // Open <id-####>
                xmlWriter.WriteStartElement("desc"); // Open <desc>
                xmlWriter.WriteAttributeString("type", "string"); // Add type=string
                xmlWriter.WriteString(traits.ActionDescription); // Add Action Description
                xmlWriter.WriteEndElement(); // Close </desc>
                xmlWriter.WriteStartElement("name"); // Open <name>
                xmlWriter.WriteAttributeString("type", "string"); // Add type=string
                xmlWriter.WriteString(traits.ActionName); // Add Action Name
                xmlWriter.WriteEndElement(); // Close </name>
                xmlWriter.WriteEndElement(); // Close </id-####>
                actionID = ++actionID;
            }
            if (npcModel.Psionics)
            {
                actionName = "Innate Spellcasting (Psionics)";
            }
            else if (npcModel.InnateSpellcastingSection && !npcModel.Psionics)
            {
                actionName = "Innate Spellcasting";
            }
            if (actionName.Length > 0)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("The " + npcModel.NPCName + "'s innate spellcasting ability is " + npcModel.InnateSpellcastingAbility + ". ");
                stringBuilder.Append("It can innately cast the following spells, " + npcModel.ComponentText + ":");
                if (npcModel.InnateAtWill != null)
                    stringBuilder.Append("\rAt will: " + npcModel.InnateAtWill);
                if (npcModel.FivePerDay != null)
                    stringBuilder.Append("\r5/day each: " + npcModel.FivePerDay);
                if (npcModel.FourPerDay != null)
                    stringBuilder.Append("\r4/day each: " + npcModel.FourPerDay);
                if (npcModel.ThreePerDay != null)
                    stringBuilder.Append("\r3/day each: " + npcModel.ThreePerDay);
                if (npcModel.TwoPerDay != null)
                    stringBuilder.Append("\r2/day each: " + npcModel.TwoPerDay);
                if (npcModel.OnePerDay != null)
                    stringBuilder.Append("\r1/day each: " + npcModel.OnePerDay);
                string innateCastingDescription = stringBuilder.ToString();

                xmlWriter.WriteStartElement("id-" + actionID.ToString("D4")); // Open <id-####>
                xmlWriter.WriteStartElement("desc"); // Open <desc>
                xmlWriter.WriteAttributeString("type", "string"); // Add type=string
                xmlWriter.WriteString(innateCastingDescription); // Add Action Description
                xmlWriter.WriteEndElement(); // Close </desc>
                xmlWriter.WriteStartElement("name"); // Open <name>
                xmlWriter.WriteAttributeString("type", "string"); // Add type=string
                xmlWriter.WriteString(actionName); // Add Action Name
                xmlWriter.WriteEndElement(); // Close </name>
                xmlWriter.WriteEndElement(); // Close </id-####>
                actionID = ++actionID;
            }
            
            xmlWriter.WriteEndElement(); // Close </traits>
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
