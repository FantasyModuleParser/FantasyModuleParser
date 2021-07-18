using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.Spells.Models;
using FantasyModuleParser.Tables.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
    public class FantasyGroundsCampaign : ICampaign
    {
        string Immunity;
        string Resistance;
        private readonly SettingsService settingsService;

		/// <summary>
		/// 
		/// </summary>
        public FantasyGroundsCampaign()
        {
            settingsService = new SettingsService();
        }
        public void CreateCampaign(ModuleModel moduleModel)
        {
            if (string.IsNullOrEmpty(settingsService.Load().FGCampaignFolderLocation))
            {
                throw new ApplicationException("No Campaign folder has been set");
            }

            if (string.IsNullOrEmpty(moduleModel.Name))
            {
                throw new ApplicationException("No Module Name has been set");
            }

            string campaignFolderPath = Path.Combine(settingsService.Load().FGCampaignFolderLocation, moduleModel.ModFilename);
            Directory.CreateDirectory(campaignFolderPath);

            string dbXmlFileContent = GenerateDBXmlFile(moduleModel);
            string campaignXmlFileContent = GenerateCampaignXmlContent(moduleModel);

			// TODO place these calls in a try catch block since they may easily throw any of 7 exceptions
			if (File.Exists(@Path.Combine(settingsService.Load().FGCampaignFolderLocation, moduleModel.Name, "campaign.xml")))
			{
				File.Delete(@Path.Combine(settingsService.Load().FGCampaignFolderLocation, moduleModel.Name, "campaign.xml"));
			}
			if (File.Exists(@Path.Combine(settingsService.Load().FGCampaignFolderLocation, moduleModel.Name, "db.xml")))
			{
				File.Delete(@Path.Combine(settingsService.Load().FGCampaignFolderLocation, moduleModel.Name, "db.xml"));
			}

			using (StreamWriter outputFile = new StreamWriter(Path.Combine(campaignFolderPath, "db.xml")))
            {
                outputFile.WriteLine(dbXmlFileContent);
            }

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(campaignFolderPath, "campaign.xml")))
            {
                outputFile.WriteLine(campaignXmlFileContent);
            }
        }
		public string GenerateDBXmlFile(ModuleModel moduleModel)
		{
			List<NPCModel> FatNPCList = CommonMethods.GenerateFatNPCList(moduleModel);
			List<SpellModel> FatSpellList = CommonMethods.GenerateFatSpellList(moduleModel);
			List<TableModel> FatTableList = CommonMethods.GenerateFatTableList(moduleModel);
			HashSet<string> UniqueCasterClass = new HashSet<string>();
			FatNPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
			FatSpellList.Sort((spellOne, spellTwo) => spellOne.SpellName.CompareTo(spellTwo.SpellName));
			FatTableList.Sort((tableOne, tableTwo) => tableOne.Name.CompareTo(tableTwo.Name));
			/// <summary>
			///  Names all token images to match the NPC name
			/// </summary>
			foreach (NPCModel npcModel in FatNPCList)
			{
				if (moduleModel.IncludeTokens)
				{
					if (!string.IsNullOrEmpty(npcModel.NPCToken))
					{
						string Filename = Path.GetFileName(npcModel.NPCToken);
						string NPCTokenFileName = Path.Combine(settingsService.Load().FGCampaignFolderLocation, moduleModel.Name, "tokens", Filename);
						string NPCTokenDirectory = Path.Combine(settingsService.Load().FGCampaignFolderLocation, moduleModel.Name, "tokens");
						// TODO place these calls in a try catch block since they may throw an exception
						if (Directory.Exists(NPCTokenDirectory))
						{
							if (File.Exists(NPCTokenFileName))	
							{ 
								File.Delete(NPCTokenFileName); 
							}
						}
						else 
						{ 
							Directory.CreateDirectory(NPCTokenDirectory); 
						}
						File.Copy(npcModel.NPCToken, NPCTokenFileName);
					}
				}
			}
			/// <summary>
			/// Names all images to match NPC name
			/// </summary>
			foreach (NPCModel npcModel in FatNPCList)
			{
				if (moduleModel.IncludeImages)
				{
					if (!string.IsNullOrEmpty(npcModel.NPCImage))
					{
						string Filename = Path.GetFileName(npcModel.NPCImage).Replace("-", "").Replace(" ", "");
						string NPCImageFileName = Path.Combine(settingsService.Load().FGCampaignFolderLocation, moduleModel.Name, "images", Filename);
						string NPCImageDirectory = Path.Combine(settingsService.Load().FGCampaignFolderLocation, moduleModel.Name, "images");

						// TODO place these calls in a try catch block since they may throw an exception
						if (Directory.Exists(NPCImageDirectory))
						{
							if (File.Exists(NPCImageFileName))
							{
								File.Delete(NPCImageFileName);
							}
						}
						else
						{
							Directory.CreateDirectory(NPCImageDirectory);
						}
						File.Copy(npcModel.NPCImage, NPCImageFileName);
					}
				}
			}

			using (StringWriter sw = new StringWriterWithEncoding(Encoding.UTF8))
			using (XmlWriter xmlWriter = XmlWriter.Create(sw, GetXmlWriterSettings()))
			{
				xmlWriter.WriteStartDocument();

				xmlWriter.WriteComment("Generated by Fantasy Module Parser");
				xmlWriter.WriteComment("Written by Theodore Story, Darkpool, and Battlemarch (c) 2021");

				xmlWriter.WriteStartElement("root");
				xmlWriter.WriteAttributeString("version", "4.0");
				if (moduleModel.IncludeImages)
				{
					#region Image XML
					xmlWriter.WriteStartElement("image");
					foreach (CategoryModel categoryModel in moduleModel.Categories)
					{
						xmlWriter.WriteStartElement("category");
						xmlWriter.WriteAttributeString("name", categoryModel.Name);
						xmlWriter.WriteAttributeString("baseicon", "0");
						xmlWriter.WriteAttributeString("decalicon", "0");
						foreach (NPCModel npcModel in categoryModel.NPCModels)
						{
							if (!string.IsNullOrEmpty(npcModel.NPCImage))
							{
								xmlWriter.WriteStartElement(Path.GetFileNameWithoutExtension(npcModel.NPCImage).Replace(" ", "").Replace("-", ""));
								xmlWriter.WriteStartElement("image");
								xmlWriter.WriteAttributeString("type", "image");
								xmlWriter.WriteStartElement("bitmap");
								xmlWriter.WriteAttributeString("type", "string");
								xmlWriter.WriteString("images" + "\\" + Path.GetFileName(npcModel.NPCImage).Replace(" ", "").Replace("-", ""));
								xmlWriter.WriteEndElement();
								xmlWriter.WriteEndElement();
								NPCExporter.WriteName(xmlWriter, npcModel);
								if (!string.IsNullOrEmpty(npcModel.NonID))
								{
									xmlWriter.WriteStartElement("nonid_name");
									xmlWriter.WriteAttributeString("type", "string");
									xmlWriter.WriteString(npcModel.NonID);
									xmlWriter.WriteEndElement();
								}
								if (!string.IsNullOrEmpty(npcModel.NonID))
								{
									xmlWriter.WriteStartElement("isidentified");
									xmlWriter.WriteAttributeString("type", "number");
									xmlWriter.WriteString("0");
									xmlWriter.WriteEndElement();
								}
								xmlWriter.WriteEndElement();
							}
						}
						xmlWriter.WriteEndElement();
					}
					xmlWriter.WriteEndElement();
					#endregion
				}
				#region Reference Section
				if (moduleModel.IncludeNPCs)
				{
					#region NPC Data
					xmlWriter.WriteStartElement("npc");

					//Category section in the XML generation
					foreach (CategoryModel categoryModel in moduleModel.Categories)
					{
						xmlWriter.WriteStartElement("category");
						xmlWriter.WriteAttributeString("name", categoryModel.Name);
						xmlWriter.WriteAttributeString("baseicon", "0");
						xmlWriter.WriteAttributeString("decalicon", "0");

						//Now, write out each NPC with NPC Name
						foreach (NPCModel npcModel in FatNPCList)
						{
							xmlWriter.WriteStartElement(NPCExporter.NPCNameToXMLFormat(npcModel)); // Open <npcModel.NPCName>
							NPCExporter.WriteAbilities(xmlWriter, npcModel);
							NPCExporter.WriteAC(xmlWriter, npcModel);
							NPCExporter.WriteActions(xmlWriter, npcModel);
							NPCExporter.WriteAlignment(xmlWriter, npcModel);
							NPCExporter.WriteConditionImmunities(xmlWriter, npcModel);
							NPCExporter.WriteCR(xmlWriter, npcModel);
							WriteDamageImmunities(xmlWriter, npcModel);
							WriteDamageResistances(xmlWriter, npcModel);
							WriteDamageVulnerabilities(xmlWriter, npcModel);
							NPCExporter.WriteHP(xmlWriter, npcModel);
							NPCExporter.WriteLairActions(xmlWriter, npcModel);
							NPCExporter.WriteLanguages(xmlWriter, npcModel);
							NPCExporter.WriteLegendaryActions(xmlWriter, npcModel);
							NPCExporter.WriteName(xmlWriter, npcModel);
							NPCExporter.WriteReactions(xmlWriter, npcModel);
							NPCExporter.WriteSavingThrows(xmlWriter, npcModel);
							NPCExporter.WriteSenses(xmlWriter, npcModel);
							NPCExporter.WriteSize(xmlWriter, npcModel);
							NPCExporter.WriteSkills(xmlWriter, npcModel);
							NPCExporter.WriteSpeed(xmlWriter, npcModel);
							NPCExporter.WriteText(xmlWriter, npcModel);
							NPCExporter.WriteToken(xmlWriter, npcModel, moduleModel);
							NPCExporter.WriteType(xmlWriter, npcModel);
							NPCExporter.WriteTraits(xmlWriter, npcModel);
							NPCExporter.WriteXP(xmlWriter, npcModel);
							xmlWriter.WriteEndElement(); // Closes </npcModel.NPCName>
						}
						xmlWriter.WriteEndElement(); // Close </category>
					}
					xmlWriter.WriteEndElement(); // Close </npcdata>
					#endregion
				}
				if (moduleModel.IncludeSpells)
				{
					#region Spell Data
					xmlWriter.WriteStartElement("spell");
					foreach (CategoryModel categoryModel in moduleModel.Categories)
					{
						xmlWriter.WriteStartElement("category");
						xmlWriter.WriteAttributeString("name", categoryModel.Name);
						xmlWriter.WriteAttributeString("baseicon", "0");
						xmlWriter.WriteAttributeString("decalicon", "0");
						xmlWriter.WriteEndElement();

						foreach (SpellModel spellModel in FatSpellList)
						{
							xmlWriter.WriteStartElement(SpellExporter.SpellNameToXMLFormat(spellModel));
							SpellExporter.WriteSpellName(xmlWriter, spellModel);
							SpellExporter.WriteSpellDescription(xmlWriter, spellModel);
							SpellExporter.WriteSpellLevel(xmlWriter, spellModel);
							SpellExporter.WriteSpellSchool(xmlWriter, spellModel);
							SpellExporter.WriteSpellRitual(xmlWriter, spellModel);
							SpellExporter.WriteSpellSource(xmlWriter, spellModel);
							SpellExporter.WriteCastingTime(xmlWriter, spellModel);
							SpellExporter.WriteSpellRange(xmlWriter, spellModel);
							SpellExporter.WriteSpellDuration(xmlWriter, spellModel);
							SpellExporter.WriteSpellComponents(xmlWriter, spellModel);
							xmlWriter.WriteEndElement();
						}
					}
					xmlWriter.WriteEndElement();
					#endregion
				}
				#region Equipment Data
				//	xmlWriter.WriteStartElement("equipmentdata");      
				//	xmlWriter.WriteAttributeString("static", "true"); 
				//	xmlWriter.WriteString(" ");                        
				//	xmlWriter.WriteEndElement();
				#endregion
				#region Equipment Lists
				//	xmlWriter.WriteStartElement("equipmentlists");   
				//	xmlWriter.WriteStartElement("equipment");           
				//	xmlWriter.WriteStartElement("name");              
				//	xmlWriter.WriteAttributeString("type", "string");   
				//	xmlWriter.WriteString("Equipment");                
				//	xmlWriter.WriteEndElement();                       
				//	xmlWriter.WriteEndElement();
				//	xmlWriter.WriteEndElement();
				#endregion
				if (moduleModel.IncludeTables)
				{
					#region Table Data
					xmlWriter.WriteStartElement("tables");
					foreach (CategoryModel categoryModel in moduleModel.Categories)
					{
						xmlWriter.WriteStartElement("category");
						xmlWriter.WriteAttributeString("name", categoryModel.Name);
						xmlWriter.WriteAttributeString("baseicon", "0");
						xmlWriter.WriteAttributeString("decalicon", "0");

						//Now, write out each NPC with NPC Name
						foreach (TableModel tableModel in FatTableList)
						{
							xmlWriter.WriteStartElement(TableExporter.TableNameToXMLFormat(tableModel)); // Open <tableModel.Name>
							TableExporter.WriteTableName(xmlWriter, tableModel);
							TableExporter.WriteTableDescription(xmlWriter, tableModel);
							TableExporter.WriteTableOutput(xmlWriter, tableModel);
							TableExporter.WriteTableNotes(xmlWriter, tableModel);
							TableExporter.WriteTableHideRolls(xmlWriter, tableModel);
							TableExporter.WriteTableRollModifier(xmlWriter, tableModel);
							TableExporter.WriteTableRollDice(xmlWriter, tableModel);
							TableExporter.WriteColumnLabels(xmlWriter, tableModel);
							TableExporter.WriteResultsColumn(xmlWriter, tableModel);
							xmlWriter.WriteEndElement(); // Closes </tableModel.Name>
						}
						xmlWriter.WriteEndElement(); // Close </category>
					}
					xmlWriter.WriteEndElement(); // Close </tables>
					#endregion
				}
				#endregion
				xmlWriter.WriteEndElement(); // Closes </root>
				xmlWriter.WriteEndDocument();
				xmlWriter.Close();
				return sw.ToString();
			}
		}
		#region NPC Methods
		private void WriteDamageImmunities(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			xmlWriter.WriteStartElement("damageimmunities");
			xmlWriter.WriteAttributeString("type", "string");
			if (npcModel.DamageImmunityModelList != null)
			{
				foreach (SelectableActionModel damageImmunities in npcModel.DamageImmunityModelList)
				{
					if (damageImmunities.Selected) 
					{ 
						stringBuilder.Append(damageImmunities.ActionDescription.ToLower()).Append(", "); 
					}		
				}
			}				
			if (stringBuilder.Length >= 2)	
			{ 
				stringBuilder.Remove(stringBuilder.Length - 2, 2); 
			}
			stringBuilder.Append("; ");
			if (npcModel.SpecialWeaponImmunityModelList != null)
			{
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
							{ 
								stringBuilder.Append(specialWeaponDmgImmunity.ActionDescription).Append(", "); 
							}
						}
						if (stringBuilder.Length >= 2) 
						{ 
							stringBuilder.Remove(stringBuilder.Length - 2, 2); 
						}
						stringBuilder.Append(Immunity);
					}
				}
			}
				
			string weaponDamageImmunityString = stringBuilder.ToString().Trim();
			if (weaponDamageImmunityString.EndsWith(";", true, CultureInfo.CurrentCulture)) 
			{ 
				weaponDamageImmunityString = weaponDamageImmunityString.Substring(0, weaponDamageImmunityString.Length - 1); 
			}
			xmlWriter.WriteString(weaponDamageImmunityString);
			xmlWriter.WriteEndElement();
		}
		private void WriteDamageResistances(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			xmlWriter.WriteStartElement("damageresistances");
			xmlWriter.WriteAttributeString("type", "string");
			if (npcModel.DamageResistanceModelList != null)
			{
				foreach (SelectableActionModel damageResistances in npcModel.DamageResistanceModelList)
				{
					if (damageResistances.Selected) 
					{ 
						stringBuilder.Append(damageResistances.ActionDescription.ToLower()).Append(", "); 
					}
				}
			}				
			if (stringBuilder.Length >= 2) 
			{ 
				stringBuilder.Remove(stringBuilder.Length - 2, 2); 
			}
			stringBuilder.Append("; ");
			if (npcModel.SpecialWeaponResistanceModelList != null)
			{
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
							{
								stringBuilder.Append(specialWeaponDmgResistance.ActionDescription).Append(", ");
							}
								
						}
						if (stringBuilder.Length >= 2)
						{
							stringBuilder.Remove(stringBuilder.Length - 2, 2);
						}							
						stringBuilder.Append(Resistance);
					}
				}
			}				
			string weaponDamageResistanceString = stringBuilder.ToString().Trim();
			if (weaponDamageResistanceString.StartsWith(";", true, CultureInfo.CurrentCulture)) { weaponDamageResistanceString = weaponDamageResistanceString.Remove(0, 1); }
			if (weaponDamageResistanceString.EndsWith(";", true, CultureInfo.CurrentCulture)) { weaponDamageResistanceString = weaponDamageResistanceString.Substring(0, weaponDamageResistanceString.Length - 1); }				
			xmlWriter.WriteString(weaponDamageResistanceString);
			xmlWriter.WriteEndElement();
		}
		private void WriteDamageVulnerabilities(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			xmlWriter.WriteStartElement("damagevulnerabilities");
			xmlWriter.WriteAttributeString("type", "string");
			if (npcModel.DamageVulnerabilityModelList != null)
				foreach (SelectableActionModel damageVulnerabilities in npcModel.DamageVulnerabilityModelList)
				{
					if (damageVulnerabilities.Selected == true)
						stringBuilder.Append(damageVulnerabilities.ActionDescription.ToLower()).Append(", ");
				}
			if (stringBuilder.Length >= 2)
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			string weaponDamageVulnerabilityString = stringBuilder.ToString().Trim();

			xmlWriter.WriteValue(weaponDamageVulnerabilityString);
			xmlWriter.WriteEndElement();
		}
		#endregion
		public string GenerateCampaignXmlContent(ModuleModel moduleModel)
		{
			using (StringWriter sw = new StringWriterWithEncoding(Encoding.UTF8))
			using (XmlWriter xmlWriter = XmlWriter.Create(sw, GetXmlWriterSettings()))
			{
				xmlWriter.WriteStartDocument();
				xmlWriter.WriteStartElement("root");
				xmlWriter.WriteAttributeString("version", "4");
				xmlWriter.WriteStartElement("password");
				xmlWriter.WriteEndElement(); // close password
				xmlWriter.WriteStartElement("ruleset");
				xmlWriter.WriteString("5E");
				xmlWriter.WriteEndElement(); // close ruleset
				xmlWriter.WriteStartElement("username");
				xmlWriter.WriteString("DM");
				xmlWriter.WriteEndElement(); // close username
				xmlWriter.WriteEndElement(); // close root
				xmlWriter.WriteEndDocument(); // close document
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
		private string WriteXmlDocumentToString(XmlDocument xmlDocument)
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
