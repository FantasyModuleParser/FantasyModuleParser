using FantasyModuleParser.Exporters;
using FantasyModuleParser.Extensions;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Skills;
using FantasyModuleParser.Spells.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
    public class FantasyGroundsCampaign : ICampaign
    {
        string Immunity;
        string Resistance;
        private readonly SettingsService settingsService;

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

            string campaignFolderPath = Path.Combine(settingsService.Load().FGCampaignFolderLocation, moduleModel.Name);
            Directory.CreateDirectory(campaignFolderPath);

            string dbXmlFileContent = GenerateDBXmlFile(moduleModel);
            string campaignXmlFileContent = GenerateCampaignXmlContent(moduleModel);

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

        private List<NPCModel> GenerateFatNPCList(ModuleModel moduleModel)
        {
            List<NPCModel> FatNPCList = new List<NPCModel>();

            foreach (CategoryModel category in moduleModel.Categories)	
			{ 
				foreach (NPCModel npcModel in category.NPCModels) 
				{ 
					FatNPCList.Add(npcModel); 
				} 
			}
            return FatNPCList;
        }

        private List<SpellModel> GenerateFatSpellList(ModuleModel moduleModel)
        {
            List<SpellModel> FatSpellList = new List<SpellModel>();
            foreach (CategoryModel category in moduleModel.Categories)	
			{ 
				foreach (SpellModel spellModel in category.SpellModels) 
				{ 
					FatSpellList.Add(spellModel); 
				} 
			}
            return FatSpellList;
        }

		public string GenerateDBXmlFile(ModuleModel moduleModel)
		{
			List<NPCModel> FatNPCList = GenerateFatNPCList(moduleModel);
			List<SpellModel> FatSpellList = GenerateFatSpellList(moduleModel);
			HashSet<string> UniqueCasterClass = new HashSet<string>();
			FatNPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
			FatSpellList.Sort((spellOne, spellTwo) => spellOne.SpellName.CompareTo(spellTwo.SpellName));
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
				xmlWriter.WriteComment("Written by Theodore Story, Darkpool, and Dusk (c) 2020");

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
								FantasyGroundsExporter.WriteName(xmlWriter, npcModel);
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
							xmlWriter.WriteStartElement(NPCNameToXMLFormat(npcModel)); // Open <npcModel.NPCName>
							FantasyGroundsExporter.WriteAbilities(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteAC(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteActions(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteAlignment(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteConditionImmunities(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteCR(xmlWriter, npcModel);
							WriteDamageImmunities(xmlWriter, npcModel);
							WriteDamageResistances(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteDamageVulnerabilities(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteHP(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteLairActions(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteLanguages(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteLegendaryActions(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteName(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteReactions(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteSavingThrows(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteSenses(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteSize(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteSkills(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteSpeed(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteText(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteToken(xmlWriter, npcModel, moduleModel);
							FantasyGroundsExporter.WriteType(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteTraits(xmlWriter, npcModel);
							FantasyGroundsExporter.WriteXP(xmlWriter, npcModel);
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
							xmlWriter.WriteStartElement(SpellNameToXMLFormat(spellModel));
							WriteSpellName(xmlWriter, spellModel);
							WriteSpellDescription(xmlWriter, spellModel);
							WriteSpellLevel(xmlWriter, spellModel);
							WriteSpellSchool(xmlWriter, spellModel);
							WriteSpellSource(xmlWriter, spellModel);
							WriteCastingTime(xmlWriter, spellModel);
							WriteSpellRange(xmlWriter, spellModel);
							WriteSpellDuration(xmlWriter, spellModel);
							WriteSpellComponents(xmlWriter, spellModel);
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
				#region Tables
				//	xmlWriter.WriteStartElement("tables");      
				//	xmlWriter.WriteString(" ");                 
				//	xmlWriter.WriteEndElement();                
				#endregion
				#endregion
				xmlWriter.WriteEndElement(); // Closes </root>
				xmlWriter.WriteEndDocument();
				xmlWriter.Close();
				return sw.ToString();
			}
		}
		#region Spell Methods for Reference Data XML
		private void SpellLocation(XmlWriter xmlWriter, ModuleModel moduleModel, List<SpellModel> SpellList)
		{
			foreach (SpellModel spell in SpellList)
			{
				xmlWriter.WriteStartElement(SpellNameToXMLFormat(spell));
				xmlWriter.WriteStartElement("link");
				xmlWriter.WriteAttributeString("type", "windowreference");
				xmlWriter.WriteStartElement("class");
				xmlWriter.WriteString("reference_spell");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("recordname");
				if (moduleModel.IsLockedRecords)
				{
					xmlWriter.WriteString("reference.spelldata." + SpellNameToXMLFormat(spell) + "@" + moduleModel.Name);
				}
				else
				{
					xmlWriter.WriteString("reference.spelldata." + SpellNameToXMLFormat(spell));
				}
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("description");
				xmlWriter.WriteStartElement("field");
				xmlWriter.WriteString("name");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("source");
				xmlWriter.WriteAttributeString("type", "number");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
			}
		}
		public void CreateSpellReferenceByFirstLetter(XmlWriter xmlWriter, ModuleModel moduleModel, List<SpellModel> SpellList)
		{
			SpellList.Sort((spellOne, spellTwo) => spellOne.SpellName.CompareTo(spellTwo.SpellName));
			var AlphabetList = SpellList.GroupBy(x => x.SpellName.ToUpper()[0]).Select(x => x.ToList()).ToList();
			foreach (List<SpellModel> spellList in AlphabetList)
			{
				string actualLetter = spellList[0].SpellName[0] + "";
				ProcessSpellListByLetter(xmlWriter, moduleModel, actualLetter, spellList);
			}
		}
		private void ProcessSpellListByLetter(XmlWriter xmlWriter, ModuleModel moduleModel, string actualLetter, List<SpellModel> SpellList)
		{
			xmlWriter.WriteStartElement("typeletter" + actualLetter);
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(actualLetter);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("index");
			SpellLocation(xmlWriter, moduleModel, SpellList);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
		}
		private string SpellNameToXMLFormat(SpellModel spellModel)
		{
			string name = spellModel.SpellName.ToLower();
			return name.Replace(" ", "_").Replace(",", "");
		}
		private void WriteSpellName(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(spellModel.SpellName);
			xmlWriter.WriteEndElement();
		}
		private void WriteSpellDescription(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "formattedtext");
			xmlWriter.WriteString(spellModel.Description);
			xmlWriter.WriteEndElement();
		}
		private void WriteSpellLevel(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("level");
			xmlWriter.WriteAttributeString("type", "number");
			if (spellModel.SpellLevel.GetDescription().Equals("cantrip")) 
			{ 
				xmlWriter.WriteString("0"); 
			}
			else 
			{ 
				xmlWriter.WriteValue((int)spellModel.SpellLevel); 
			}
			xmlWriter.WriteEndElement();
		}
		private void WriteSpellSchool(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("school");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(spellModel.SpellSchool.GetDescription());
			xmlWriter.WriteEndElement();
		}
		private void WriteSpellSource(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("source");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(spellModel.CastBy);
			xmlWriter.WriteEndElement();
		}
		private void WriteSpellRange(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("range");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(spellModel.RangeDescription);
			xmlWriter.WriteEndElement();
		}
		private void WriteCastingTime(XmlWriter xmlWriter, SpellModel spellModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(spellModel.CastingTime + " " + spellModel.CastingType.GetDescription());
			xmlWriter.WriteStartElement("castingtime");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(stringBuilder.ToString());
			xmlWriter.WriteEndElement();
		}

		private void WriteSpellDuration(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("duration");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(spellModel.DurationText);
			xmlWriter.WriteEndElement();
		}
		private void WriteSpellComponents(XmlWriter xmlWriter, SpellModel spellModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (spellModel.IsVerbalComponent)
			{
				stringBuilder.Append("V");
				if (spellModel.IsSomaticComponent)
				{
					stringBuilder.Append(", S");
					if (spellModel.IsMaterialComponent) 
					{ 
						stringBuilder.Append(", M (" + spellModel.ComponentText + ")"); 
					}
				}
				else if (spellModel.IsMaterialComponent) 
				{ 
					stringBuilder.Append(", M (" + spellModel.ComponentText + ")"); 
				}
			}
			else if (!spellModel.IsVerbalComponent && spellModel.IsSomaticComponent)
			{
				stringBuilder.Append("S");
				if (spellModel.IsMaterialComponent) 
				{ 
					stringBuilder.Append(", M (" + spellModel.ComponentText + ")"); 
				}
			}
			else if (!spellModel.IsVerbalComponent && !spellModel.IsSomaticComponent && spellModel.IsMaterialComponent)
			{
				stringBuilder.Append("M (" + spellModel.ComponentText + ")");
			}				

			xmlWriter.WriteStartElement("components");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(stringBuilder.ToString());
			xmlWriter.WriteEndElement();
		}
		#endregion
		#region NPC Methods for Reference Data XML
		private void NPCLocation(XmlWriter xmlWriter, ModuleModel moduleModel, List<NPCModel> NPCList)
		{
			foreach (NPCModel npc in NPCList)
			{
				xmlWriter.WriteStartElement(NPCNameToXMLFormat(npc));
				xmlWriter.WriteStartElement("link");
				xmlWriter.WriteAttributeString("type", "windowreference");
				xmlWriter.WriteStartElement("class");
				xmlWriter.WriteString("npc");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("recordname");
				xmlWriter.WriteString("reference.npcdata." + NPCNameToXMLFormat(npc) + "@" + moduleModel.Name);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("description");
				xmlWriter.WriteStartElement("field");
				xmlWriter.WriteString("name");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("source");
				xmlWriter.WriteAttributeString("type", "number");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
			}
		}
		public void CreateReferenceByFirstLetter(XmlWriter xmlWriter, ModuleModel moduleModel, List<NPCModel> NPCList)
		{
			NPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
			var AlphabetList = NPCList.GroupBy(x => x.NPCName.ToUpper()[0]).Select(x => x.ToList()).ToList();
			foreach (List<NPCModel> npcList in AlphabetList)
			{
				string actualLetter = npcList[0].NPCName[0] + "";
				ProcessNPCListByLetter(xmlWriter, moduleModel, actualLetter, npcList);
			}
		}
		private void ProcessNPCListByLetter(XmlWriter xmlWriter, ModuleModel moduleModel, string actualLetter, List<NPCModel> NPCList)
		{
			xmlWriter.WriteStartElement("typeletter" + actualLetter);
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(actualLetter);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("index");
			NPCLocation(xmlWriter, moduleModel, NPCList);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
		}
		public void CreateReferenceByCR(XmlWriter xmlWriter, ModuleModel moduleModel, List<NPCModel> NPCList)
		{
			NPCList.Sort((npcOne, npcTwo) => npcOne.ChallengeRating.CompareTo(npcTwo.ChallengeRating));
			var CRList = NPCList.GroupBy(x => x.ChallengeRating.ToUpper()[0]).Select(x => x.ToList()).ToList();
			foreach (List<NPCModel> npcList in CRList)
			{
				string actualCR = npcList[0].ChallengeRating + "";
				ProcessNPCListByCR(xmlWriter, moduleModel, actualCR, npcList);
			}
		}
		private void ProcessNPCListByCR(XmlWriter xmlWriter, ModuleModel moduleModel, string actualCR, List<NPCModel> NPCList)
		{
			if (actualCR == "1/8") 
			{ 
				xmlWriter.WriteStartElement("CR0125"); 
			}
			else if (actualCR == "1/4") 
			{
				xmlWriter.WriteStartElement("CR025"); 
			}
			else if (actualCR == "1/2") 
			{ 
				xmlWriter.WriteStartElement("CR05"); 
			}
			else 
			{ 
				xmlWriter.WriteStartElement("CR" + actualCR); 
			}
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("CR " + actualCR);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("index");
			NPCLocation(xmlWriter, moduleModel, NPCList);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
		}
		public void CreateReferenceByType(XmlWriter xmlWriter, ModuleModel moduleModel, List<NPCModel> NPCList)
		{
			NPCList.Sort((npcOne, npcTwo) => npcOne.NPCType.CompareTo(npcTwo.NPCType));
			var TypeList = NPCList.GroupBy(x => x.NPCType.ToLower()[0]).Select(x => x.ToList()).ToList();
			foreach (List<NPCModel> npcList in TypeList)
			{
				string actualType = npcList[0].NPCType + "";
				ProcessNPCListByType(xmlWriter, moduleModel, actualType, npcList);
			}
		}
		private void ProcessNPCListByType(XmlWriter xmlWriter, ModuleModel moduleModel, string actualType, List<NPCModel> NPCList)
		{
			xmlWriter.WriteStartElement("type_" + NPCTypeToXMLFormat(actualType));
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(actualType);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("index");
			NPCLocation(xmlWriter, moduleModel, NPCList);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
		}
		private string NPCNameToXMLFormat(NPCModel npcModel)
		{
			string name = npcModel.NPCName.ToLower();
			return name.Replace(" ", "_").Replace(",", "").Replace("(", "_").Replace(")", "");
		}
		private string NPCTypeToXMLFormat(string actualType)
		{
			string npcType = actualType.ToLower();
			return npcType.Replace(" ", "");
		}
		public void SortNPCListByCategory(XmlWriter xmlWriter, NPCModel npcModel, ModuleModel moduleModel, CategoryModel categoryModel, List<NPCModel> NPCList)
		{
			NPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
			var AlphabetList = NPCList.GroupBy(x => x.NPCName.ToUpper()[0]).Select(x => x.ToList()).ToList();
			foreach (List<NPCModel> npcList in AlphabetList)
			{
				string actualLetter = npcList[0].NPCName[0] + "";
				ProcessNPCListByCategoryLetter(xmlWriter, npcModel, moduleModel);
			}
		}
		private void ProcessNPCListByCategoryLetter(XmlWriter xmlWriter, NPCModel npcModel, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement(NPCNameToXMLFormat(npcModel));
			xmlWriter.WriteStartElement("link");
			xmlWriter.WriteAttributeString("type", "windowreference");
			xmlWriter.WriteStartElement("class");
			xmlWriter.WriteString("imagewindow");
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("image." + NPCNameToXMLFormat(npcModel) + "@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteStartElement("field");
			xmlWriter.WriteString("name");
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("source");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
		}
		#endregion
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
