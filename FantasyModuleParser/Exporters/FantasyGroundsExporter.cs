﻿using FantasyModuleParser.Extensions;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Skills;
using FantasyModuleParser.Spells.Models;
using FantasyModuleParser.Spells.ViewModels;
using FantasyModuleParser.Tables.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	public class FantasyGroundsExporter : IExporter
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(FantasyGroundsExporter));

		string Immunity;
		string Resistance;
		private readonly SettingsService settingsService;

		public FantasyGroundsExporter()
		{
			settingsService = new SettingsService();
		}
		private static string DatabaseXML(ModuleModel moduleModel)
        {
			if (moduleModel.IsGMOnly)
            {
				return "db.xml";
            }
			else
            {
				return "client.xml";
            }
        }
		public void CreateModule(ModuleModel moduleModel)
		{
			SettingsModel settingsModel = settingsService.Load();

			if (string.IsNullOrEmpty(settingsModel.FGModuleFolderLocation))
			{
				log.Warn("Create Module -- No Module path has been set. Saved Path :: " + settingsModel.FGModuleFolderLocation);
				throw new ApplicationException("No Module Path has been set");
			}

			if (string.IsNullOrEmpty(moduleModel.Name))
			{
				log.Warn("Create Module -- No Module name has been saved.");
				throw new ApplicationException("No Module Name has been set");
			}

			string moduleFolderPath = Path.Combine(settingsModel.FGModuleFolderLocation, moduleModel.Name);

			// Create the folder all content will go into based on the Module name
			Directory.CreateDirectory(moduleFolderPath);

			// Save Thumbnail to Module Folder
			if (!string.IsNullOrEmpty(moduleModel.ThumbnailPath))
            {
				log.Debug("Create Module -- No thumbnail filename for module");
				SaveThumbnailImage(moduleModel);
			}

			// A blank module consists of two files;  db.xml & definition.xml
			string dbXmlFileContent = GenerateDBXmlFile(moduleModel);
			string definitionXmlFileContent = GenerateDefinitionXmlContent(moduleModel);

			// Write the string array to a new file named "WriteLines.txt".
			using (StreamWriter outputFile = new StreamWriter(Path.Combine(moduleFolderPath, DatabaseXML(moduleModel))))
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
			if (File.Exists(@Path.Combine(settingsModel.FGModuleFolderLocation, moduleModel.ModFilename + ".mod")))
			{
				File.Delete(@Path.Combine(settingsModel.FGModuleFolderLocation, moduleModel.ModFilename + ".mod"));
			}
			ZipFile.CreateFromDirectory(moduleFolderPath, Path.Combine(settingsModel.FGModuleFolderLocation, moduleModel.ModFilename + ".mod"));

			/// Delete created unzipped files
			DeleteDirectory(moduleFolderPath);

			log.Debug("Module Created!!  Saved to :: " + Path.Combine(settingsModel.FGModuleFolderLocation, moduleModel.ModFilename + ".mod"));
		}
		/// <summary>
		/// Generates a List of all NPCs across all Categories in one List<NPCModel> object.  Used for Reference Manual material.
		/// </summary>
		static private List<NPCModel> GenerateFatNPCList(ModuleModel moduleModel)
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
		/// <summary>
		/// Generates a List of all Spells across all Categories in one List<SpellModel> object. Used for Reference Manual material.
		/// </summary>
		static private List<SpellModel> GenerateFatSpellList(ModuleModel moduleModel)
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
		/// <summary>
		/// Generates a List of all Tables across all Categories in one List<TableModel> object. Used for Reference Manual material.
		/// </summary>
		static private List<TableModel> GenerateFatTableList(ModuleModel moduleModel)
		{
			List<TableModel> FatTableList = new List<TableModel>();
			foreach (CategoryModel category in moduleModel.Categories)
			{
				foreach (TableModel tableModel in category.TableModels)
				{
					FatTableList.Add(tableModel);
				}

			}
			return FatTableList;
		}
		private void DeleteDirectory(string target_dir)
		{
			string[] files = Directory.GetFiles(target_dir);
			string[] dirs = Directory.GetDirectories(target_dir);

			foreach (string file in files)
			{
				File.SetAttributes(file, FileAttributes.Normal);
				File.Delete(file);
			}

			foreach (string dir in dirs)
			{
				DeleteDirectory(dir);
			}

			Directory.Delete(target_dir, false);
		}
		/// <summary>
		/// Renames the selected thumbnail image to thumbnail.png
		/// </summary>
		private string NewThumbnailFileName(ModuleModel moduleModel)
        {
			string ThumbnailFilename = Path.Combine(settingsService.Load().ProjectFolderLocation, moduleModel.Name, "thumbnail.png");
			return ThumbnailFilename;

		}
		/// <summary>
		/// Checks whether a thumbnail image exists. 
		/// If the thumbnail image already exists, deletes and copies thumbnail image from PC location to module folder.
		/// Otherwise, copies thumbnail image from PC location to module folder.
		/// </summary>
		private void SaveThumbnailImage(ModuleModel moduleModel)
        {
			if (File.Exists(@Path.Combine(NewThumbnailFileName(moduleModel))))
			{
				File.Delete(@Path.Combine(NewThumbnailFileName(moduleModel)));
				File.Copy(moduleModel.ThumbnailPath, NewThumbnailFileName(moduleModel));
			}
			else
			{
				if(File.Exists(@moduleModel.ThumbnailPath))
				{
					File.Copy(moduleModel.ThumbnailPath, NewThumbnailFileName(moduleModel));
				}					
			}
		}
		/// <summary>
		/// Generates database file for use in Fantasy Grounds.
		/// Currently covers NPCs and Spells.
		/// </summary>
		public string GenerateDBXmlFile(ModuleModel moduleModel)
		{
			List<NPCModel> FatNPCList = GenerateFatNPCList(moduleModel);
			List<SpellModel> FatSpellList = GenerateFatSpellList(moduleModel);
			List<TableModel> FatTableList = GenerateFatTableList(moduleModel);
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
						string NPCTokenFileName = Path.Combine(settingsService.Load().FGModuleFolderLocation, moduleModel.Name, "tokens", Filename);
						string NPCTokenDirectory = Path.Combine(settingsService.Load().FGModuleFolderLocation, moduleModel.Name, "tokens");
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
						string NPCImageFileName = Path.Combine(settingsService.Load().FGModuleFolderLocation, moduleModel.Name, "images", Filename);
						string NPCImageDirectory = Path.Combine(settingsService.Load().FGModuleFolderLocation, moduleModel.Name, "images");
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
								WriteLocked(xmlWriter);
								WriteName(xmlWriter, npcModel);
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
				xmlWriter.WriteStartElement("reference");
				if (moduleModel.IsLockedRecords)
                {
					xmlWriter.WriteAttributeString("static", "true");
				}
				if (moduleModel.IncludeNPCs)
                {
					#region NPC Data
					xmlWriter.WriteStartElement("npcdata");

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
							WriteLocked(xmlWriter);
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
							WriteSize(xmlWriter, npcModel);
							WriteSkills(xmlWriter, npcModel);
							WriteSpeed(xmlWriter, npcModel);
							WriteText(xmlWriter, npcModel);
							WriteToken(xmlWriter, npcModel, moduleModel);
							WriteType(xmlWriter, npcModel);
							WriteTraits(xmlWriter, npcModel);
							WriteXP(xmlWriter, npcModel);
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
					xmlWriter.WriteStartElement("spelldata");
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
							WriteLocked(xmlWriter);
							WriteSpellName(xmlWriter, spellModel);
							WriteSpellDescription(xmlWriter, spellModel);
							WriteSpellLevel(xmlWriter, spellModel);
							WriteSpellSchool(xmlWriter, spellModel);
							WriteSpellRitual(xmlWriter, spellModel);
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
				if (moduleModel.IncludeImages)
                {
					#region Image Lists
					xmlWriter.WriteStartElement("imagelists");
					xmlWriter.WriteStartElement("bycategory");
					xmlWriter.WriteStartElement("description");
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString("Images");
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("groups");
					foreach (CategoryModel categoryModel in moduleModel.Categories)
					{
						xmlWriter.WriteStartElement(CategoryNameToXML(categoryModel));
						xmlWriter.WriteStartElement("description");
						xmlWriter.WriteAttributeString("type", "string");
						xmlWriter.WriteString(categoryModel.Name);
						xmlWriter.WriteEndElement();
						xmlWriter.WriteStartElement("index");
						foreach (NPCModel npcModel in categoryModel.NPCModels)
						{
							if (!string.IsNullOrEmpty(npcModel.NPCImage))
							{
								xmlWriter.WriteStartElement(Path.GetFileNameWithoutExtension(npcModel.NPCImage).Replace(" ", "").Replace("-", ""));
								xmlWriter.WriteStartElement("link");
								xmlWriter.WriteAttributeString("type", "windowreference");
								xmlWriter.WriteStartElement("class");
								xmlWriter.WriteString("imagewindow");
								xmlWriter.WriteEndElement();
								xmlWriter.WriteStartElement("recordname");
								xmlWriter.WriteString("image." + Path.GetFileNameWithoutExtension(npcModel.NPCImage).Replace(" ", "").Replace("-", ""));
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
						}
						xmlWriter.WriteEndElement();
						xmlWriter.WriteEndElement();
					}
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					#endregion
				}
				if (moduleModel.IncludeNPCs)
                {
					#region NPC Lists
					xmlWriter.WriteStartElement("npclists");
					xmlWriter.WriteStartElement("npcs");
					xmlWriter.WriteStartElement("name");
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString("NPCs");
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("index");
					WriteIDLinkList(xmlWriter, moduleModel, "id01", "reference.npclists.byletter@" + moduleModel.Name, "NPCs - Alphabetical Index");
					WriteIDLinkList(xmlWriter, moduleModel, "id02", "reference.npclists.bylevel@" + moduleModel.Name, "NPCs - Challenge Rating Index");
					WriteIDLinkList(xmlWriter, moduleModel, "id03", "reference.npclists.bytype@" + moduleModel.Name, "NPCs - Class Index");
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("byletter");
					xmlWriter.WriteStartElement("description");
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString("NPCs");
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("groups");
					CreateReferenceByFirstLetter(xmlWriter, moduleModel, FatNPCList);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("bylevel");
					xmlWriter.WriteStartElement("description");
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString("NPCs");
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("groups");
					CreateReferenceByCR(xmlWriter, moduleModel, FatNPCList);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("bytype");
					xmlWriter.WriteStartElement("description");
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString("NPCs");
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("groups");
					CreateReferenceByType(xmlWriter, moduleModel, FatNPCList);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					#endregion
				}
				if (moduleModel.IncludeSpells)
                {
					#region Spell Lists
					xmlWriter.WriteStartElement("spelllists");
					xmlWriter.WriteStartElement("spells");
					xmlWriter.WriteStartElement("name");
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString("Spells");
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("index");
					WriteIDLinkList(xmlWriter, moduleModel, "id-0001", "reference.spelllists._index_@" + moduleModel.Name, "(Index)");
					int spellListId = 2;
					foreach (string castByValue in GetSortedSpellCasterList(moduleModel))
					{
						string referenceId = "reference.spellists.";
						referenceId += castByValue.Replace(" ", "").Replace("(", "").Replace(")", "").ToLower();
						referenceId += "@" + moduleModel.Name;
						WriteIDLinkList(xmlWriter, moduleModel, "id-" + spellListId.ToString("D4"), referenceId, castByValue);

						spellListId++;
					}
					/// WriteIDLinkList(xmlWriter, moduleModel, "id002", "reference.spelllists.bard", "Bard");
					/// WriteIDLinkList(xmlWriter, moduleModel, "id003", "reference.spelllists.cleric", "Cleric");
					/// WriteIDLinkList(xmlWriter, moduleModel, "id004", "reference.spelllists.druid", "Druid");
					/// WriteIDLinkList(xmlWriter, moduleModel, "id005", "reference.spelllists.paladin", "Paladin");
					/// WriteIDLinkList(xmlWriter, moduleModel, "id006", "reference.spelllists.ranger", "Ranger");
					/// WriteIDLinkList(xmlWriter, moduleModel, "id007", "reference.spelllists.sorcerer", "Sorcerer");
					/// WriteIDLinkList(xmlWriter, moduleModel, "id008", "reference.spelllists.warlock", "Warlock");
					/// WriteIDLinkList(xmlWriter, moduleModel, "id009", "reference.spelllists.wizard", "Wizard");

					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();

					#region Spell Index
					xmlWriter.WriteStartElement("_index_");
					xmlWriter.WriteStartElement("description");
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString("Spell Index");
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("groups");
					CreateSpellReferenceByFirstLetter(xmlWriter, moduleModel, FatSpellList);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					#endregion
					#region Spell List By Class
					//foreach (string castByValue in getSortedSpellCasterList(moduleModel))
					//            {
					//	xmlWriter.WriteStartElement(castByValue.Replace("(", "").Replace(")", "").Replace(" ", ""));
					//		xmlWriter.WriteStartElement("description");
					//			xmlWriter.WriteAttributeString("type", "string");
					//			xmlWriter.WriteString(castByValue + " Spells");
					//		xmlWriter.WriteEndElement();
					//		xmlWriter.WriteStartElement("groups");
					SpellListByClass(xmlWriter, moduleModel);
					//		xmlWriter.WriteEndElement();
					//	xmlWriter.WriteEndElement();
					//}
					#endregion
					xmlWriter.WriteEndElement(); // Close </spelllists>
					#endregion
				}
				#region Tables
				if (moduleModel.IncludeTables)
				{
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
							xmlWriter.WriteStartElement(TableNameToXMLFormat(tableModel)); // Open <tableModel.Name>
							WriteTableLocked(xmlWriter, tableModel);
							WriteTableName(xmlWriter, tableModel);
							WriteTableDescription(xmlWriter, tableModel);
							WriteTableOutput(xmlWriter, tableModel);
							WriteTableNotes(xmlWriter, tableModel);
							WriteTableHideRolls(xmlWriter, tableModel);
							WriteTableRollModifier(xmlWriter, tableModel);
							WriteTableRollDice(xmlWriter, tableModel);
							xmlWriter.WriteEndElement(); // Closes </tableModel.Name>
						}
						xmlWriter.WriteEndElement(); // Close </category>
					}
					xmlWriter.WriteEndElement(); // Close </tables>  
				}         
				#endregion
				xmlWriter.WriteEndElement(); // Close </reference>
                #endregion
                #region Reference Manual
                xmlWriter.WriteStartElement("referencemanual");
				xmlWriter.WriteStartElement("name");
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(moduleModel.Name);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("chapters");
				xmlWriter.WriteStartElement("chapter_00");
				xmlWriter.WriteStartElement("name");
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(moduleModel.Name);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("subchapters");
				xmlWriter.WriteStartElement("subchapter_00");
				xmlWriter.WriteStartElement("name");
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString("NPCs");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("refpages");
				xmlWriter.WriteStartElement("aa_indextype");
				xmlWriter.WriteStartElement("name");
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString("(Index)");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("blocks");
				xmlWriter.WriteString(" ");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
                #endregion
                #region Library
                // For the Blank DB XML unit test, need to check if any NPCs exist
                if (moduleModel.Categories != null && moduleModel.Categories.Count > 0)
				{
					xmlWriter.WriteStartElement("library");              
					xmlWriter.WriteStartElement(WriteLibraryNameLowerCase(moduleModel)); 
					xmlWriter.WriteStartElement("name");                   
					xmlWriter.WriteAttributeString("type", "string");          
					xmlWriter.WriteString(moduleModel.Name + " Reference Library");   
					xmlWriter.WriteEndElement(); // close name						
					xmlWriter.WriteStartElement("categoryname");          
					xmlWriter.WriteAttributeString("type", "string");     
					xmlWriter.WriteString(moduleModel.Category);             
					xmlWriter.WriteEndElement();  // close categoryname                                                        
					xmlWriter.WriteStartElement("entries");
					if (moduleModel.IncludeImages)
                    {
						xmlWriter.WriteStartElement("r01images");
						xmlWriter.WriteStartElement("librarylink");
						xmlWriter.WriteAttributeString("type", "windowreference");
						xmlWriter.WriteStartElement("class");
						xmlWriter.WriteString("reference_colindex");
						xmlWriter.WriteEndElement();  // close class                           
						xmlWriter.WriteStartElement("recordname");
						xmlWriter.WriteString("reference.imagelists.bycategory@" + moduleModel.Name);
						xmlWriter.WriteEndElement();  // close recordname     
						xmlWriter.WriteEndElement();  // close librarylink
						xmlWriter.WriteStartElement("name");
						xmlWriter.WriteAttributeString("type", "string");
						xmlWriter.WriteString("Images");
						xmlWriter.WriteEndElement();  // close name            
						xmlWriter.WriteEndElement();  // close r01images
					}
					if (moduleModel.IncludeNPCs)
                    {
						if (moduleModel.Categories[0].NPCModels.Count > 0)
						{
							xmlWriter.WriteStartElement("r02monsters");
							xmlWriter.WriteStartElement("librarylink");
							xmlWriter.WriteAttributeString("type", "windowreference");
							xmlWriter.WriteStartElement("class");
							xmlWriter.WriteString("referenceindex");
							xmlWriter.WriteEndElement();
							xmlWriter.WriteStartElement("recordname");
							xmlWriter.WriteString("reference.npclists.npcs@" + moduleModel.Name);
							xmlWriter.WriteEndElement();
							xmlWriter.WriteEndElement();
							xmlWriter.WriteStartElement("name");
							xmlWriter.WriteAttributeString("type", "string");
							xmlWriter.WriteString("NPCs");
							xmlWriter.WriteEndElement();
							xmlWriter.WriteEndElement();
						}
					}
					if (moduleModel.IncludeSpells)
                    {
						if (moduleModel.Categories[0].SpellModels.Count > 0)
						{
							xmlWriter.WriteStartElement("r03spells");
							xmlWriter.WriteStartElement("librarylink");
							xmlWriter.WriteAttributeString("type", "windowreference");
							xmlWriter.WriteStartElement("class");
							xmlWriter.WriteString("referenceindex");
							xmlWriter.WriteEndElement();
							xmlWriter.WriteStartElement("recordname");
							xmlWriter.WriteString("reference.spelllists.spells@" + moduleModel.Name);
							xmlWriter.WriteEndElement();
							xmlWriter.WriteEndElement();
							xmlWriter.WriteStartElement("name");
							xmlWriter.WriteAttributeString("type", "string");
							xmlWriter.WriteString("Spells");
							xmlWriter.WriteEndElement();
							xmlWriter.WriteEndElement();
						}
					}
					xmlWriter.WriteEndElement();  // close entries                               
					xmlWriter.WriteEndElement();  // close libraryname                         
					xmlWriter.WriteEndElement();  // close library                          
				}
				#endregion
				xmlWriter.WriteEndElement(); // Closes </root>
				xmlWriter.WriteEndDocument();
				xmlWriter.Close();
				return sw.ToString();
			}
		}
		#region Spell Methods for Reference Data XML
		static private void SpellListByClass(XmlWriter xmlWriter, ModuleModel moduleModel)
        {
			List<SpellModel> SpellList = GetFatSpellModelList(moduleModel);
			SpellList.Sort((spellOne, spellTwo) => spellOne.SpellName.CompareTo(spellTwo.SpellName));
			//var AlphabetList = SpellList.GroupBy(x => x.SpellName.ToUpper()[0]).Select(x => x.ToList()).ToList();
			foreach (string castByValue in GetSortedSpellCasterList(moduleModel))
			{
				xmlWriter.WriteStartElement(castByValue.ToLower().Replace("(", "").Replace(")", "").Replace(" ", ""));  // <castby>
					xmlWriter.WriteStartElement("description"); // <castby> <description>
				xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString(castByValue + " Spells");
					xmlWriter.WriteEndElement(); // <castby> </description>
				xmlWriter.WriteStartElement("groups"); // <castby> <groups>
				SpellList.Sort((spellOne, spellTwo) => spellOne.SpellLevel.CompareTo(spellTwo.SpellLevel));
				var LevelList = SpellList.GroupBy(x => (int)x.SpellLevel).Select(x => x.ToList()).ToList();

				foreach (SpellModel spellModel in SpellList)
				{
					if (spellModel.CastBy.Contains(castByValue))
                    {
						foreach (List<SpellModel> levelList in LevelList)
						{
							xmlWriter.WriteStartElement("level" + (int)levelList[0].SpellLevel); // <castby> <groups> <level#>
							xmlWriter.WriteStartElement("description"); // <castby> <groups> <level#> <description>
							xmlWriter.WriteAttributeString("type", "string");
							if (levelList[0].SpellLevel == Spells.Enums.SpellLevel.Cantrip)
							{
								xmlWriter.WriteString("Cantrips");
							}									
							else
							{
								xmlWriter.WriteString("Level " + (int)levelList[0].SpellLevel + " Spells");
							}		
							xmlWriter.WriteEndElement(); // <castby> <groups> <level#> </description>
							xmlWriter.WriteStartElement("index");	// <castby> <groups> <level#> <index>
							foreach (SpellModel spellLevelList in levelList)
                            {
								xmlWriter.WriteStartElement(spellLevelList.SpellName.ToLower().Replace(" ", "").Replace("'", ""));  // <spellname>
									xmlWriter.WriteStartElement("link"); // <spellname> <link>
								xmlWriter.WriteAttributeString("type", "windowreference");
									xmlWriter.WriteStartElement("class");   // <spellname> <link> <class>
								xmlWriter.WriteString("reference_spell");
									xmlWriter.WriteEndElement(); // <spellname> <link> </class>
								xmlWriter.WriteStartElement("recordname"); // <spellname> <link> <recordname>
								if (moduleModel.IsLockedRecords)
								{
									xmlWriter.WriteString("reference.spelldata." + spellLevelList.SpellName.ToLower().Replace(" ", "").Replace("'", "") + "@" + moduleModel.Name);
								}
								else
								{
									xmlWriter.WriteString("reference.spelldata." + spellLevelList.SpellName.ToLower().Replace(" ", "").Replace("'", ""));
								}
								xmlWriter.WriteEndElement(); // <spellname> <link> </recordname>
								xmlWriter.WriteStartElement("description"); // <spellname> <link> <description>
								xmlWriter.WriteStartElement("field"); // <spellname> <link> <description> <field>
								xmlWriter.WriteString("name");
								xmlWriter.WriteEndElement(); // <spellname> <link> <description> </field>
								xmlWriter.WriteEndElement(); // <spellname> <link> </description>
								xmlWriter.WriteEndElement(); // <spellname> </link>
								xmlWriter.WriteStartElement("source"); // <spellname> <source>
								xmlWriter.WriteString("Class " + castByValue);
								xmlWriter.WriteEndElement(); // <spellname> </source>
								xmlWriter.WriteEndElement(); // </spellname>
							}
							xmlWriter.WriteEndElement();  // <castby> <groups> <level#> </index>
							xmlWriter.WriteEndElement();  // <castby> <groups> </level#>
						}
					}
				}
				xmlWriter.WriteEndElement();  // <castby> </groups>
				xmlWriter.WriteEndElement();  // </castby>
			}
		}
		static private void SpellLocation(XmlWriter xmlWriter, ModuleModel moduleModel, List<SpellModel> SpellList)
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
		static public void CreateSpellReferenceByFirstLetter(XmlWriter xmlWriter, ModuleModel moduleModel, List<SpellModel> SpellList)
		{
			SpellList.Sort((spellOne, spellTwo) => spellOne.SpellName.CompareTo(spellTwo.SpellName));
			var AlphabetList = SpellList.GroupBy(x => x.SpellName.ToUpper()[0]).Select(x => x.ToList()).ToList();
			foreach (List<SpellModel> spellList in AlphabetList)
			{
				string actualLetter = spellList[0].SpellName[0] + "";
				ProcessSpellListByLetter(xmlWriter, moduleModel, actualLetter, spellList);
			}
		}
		static private void ProcessSpellListByLetter(XmlWriter xmlWriter, ModuleModel moduleModel, string actualLetter, List<SpellModel> SpellList)
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
		static private HashSet<string> GenerateSpellCasterList(ModuleModel moduleModel)
		{
			HashSet<string> casterList = new HashSet<string>();

			if (moduleModel != null && moduleModel.Categories != null)
			{
				foreach (CategoryModel categoryModel in moduleModel.Categories)
				{
					if (categoryModel.SpellModels != null)
					{
						foreach (SpellModel spellModel in categoryModel.SpellModels)
						{
							if (!String.IsNullOrWhiteSpace(spellModel.CastBy))
							{
								foreach (string castByValue in spellModel.CastBy.Split(','))
								{
									casterList.Add(castByValue.Trim());
								}
							}
						}
					}
				}
			}

			return casterList;
		}
		static private IEnumerable<string> GetSortedSpellCasterList(ModuleModel moduleModel)
		{
			return GenerateSpellCasterList(moduleModel).OrderBy(item => item);
		}
		static private List<SpellModel> GetFatSpellModelList(ModuleModel moduleModel)
        {
			List<SpellModel> spellModels = new List<SpellModel>();
			if (moduleModel.Categories == null)
			{
				return spellModels;
			}
			foreach (CategoryModel categoryModel in moduleModel.Categories)
            {
				if (categoryModel.SpellModels == null)
				{
					return spellModels;
				}
				foreach(SpellModel spellModel in categoryModel.SpellModels)
                {
					spellModels.Add(spellModel);
                }
            }
			return spellModels;
        }
		static private string SpellNameToXMLFormat(SpellModel spellModel)
		{
			string name = spellModel.SpellName.ToLower();
			return name.Replace(" ", "_").Replace(",", "");
		}
		static private void WriteSpellName(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(spellModel.SpellName);
			xmlWriter.WriteEndElement();
		}
		static private void WriteSpellDescription(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "formattedtext");
			xmlWriter.WriteString(spellModel.Description);
			xmlWriter.WriteEndElement();
		}
		static private void WriteSpellLevel(XmlWriter xmlWriter, SpellModel spellModel)
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
		static private void WriteSpellSchool(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("school");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(spellModel.SpellSchool.GetDescription());
			xmlWriter.WriteEndElement();
		}
		static private void WriteSpellSource(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("source");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(spellModel.CastBy);
			xmlWriter.WriteEndElement();
		}
		static private void WriteSpellRange(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("range");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(spellModel.RangeDescription);
			xmlWriter.WriteEndElement();
		}
		static private void WriteCastingTime(XmlWriter xmlWriter, SpellModel spellModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(spellModel.CastingTime + " " + spellModel.CastingType.GetDescription());

			xmlWriter.WriteStartElement("castingtime");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(stringBuilder.ToString());
			xmlWriter.WriteEndElement();
		}

		static private void WriteSpellDuration(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("duration");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(spellModel.DurationText);
			xmlWriter.WriteEndElement();
		}
		static private void WriteSpellComponents(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("components");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(SpellViewModel.GenerateComponentDescription(spellModel));
			xmlWriter.WriteEndElement();
		}
		static private void WriteSpellRitual(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("ritual");
			xmlWriter.WriteAttributeString("type", "number");
			if (spellModel.IsRitual)
			{
				xmlWriter.WriteString("1");
			}				
			else
			{
				xmlWriter.WriteString("0");
			}				
			xmlWriter.WriteEndElement();
		}
		#endregion
		#region Common methods for Reference Manual XML
		static private void WriteIDLinkList(XmlWriter xmlWriter, ModuleModel moduleModel, string id, string listId, string listDescription)
		{
			xmlWriter.WriteStartElement(id);
			xmlWriter.WriteStartElement("listlink");
			xmlWriter.WriteAttributeString("type", "windowreference");
			xmlWriter.WriteStartElement("class");
			xmlWriter.WriteString("reference_colindex");
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("recordname");
			if (moduleModel.IsLockedRecords)
			{
				xmlWriter.WriteString(listId + "@" + moduleModel.Name);
			}				
			else
			{
				xmlWriter.WriteString(listId);
			}				
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(listDescription);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
		}
		static private string WriteLibraryNameLowerCase(ModuleModel moduleModel)
		{
			string libname = moduleModel.Name.ToLower();
			return libname.Replace(" ", "").Replace("'", "");
		}
		static private string CategoryNameToXML(CategoryModel categoryModel)
        {
			string categoryName = categoryModel.Name;
			return categoryName.Replace(" ", "").Replace(",", "").Replace("-", "").Replace("'", "").ToLower();
        }
		#endregion
		#region Common Methods
		static private void WriteLocked(XmlWriter xmlWriter)
		{
			ModuleModel moduleModel = new ModuleModel();
			string lockedRecords = "0";
			if (moduleModel.IsLockedRecords)
			{
				lockedRecords = "1";
			}				
			xmlWriter.WriteStartElement("locked");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteString(lockedRecords.ToString());
			xmlWriter.WriteEndElement();
		}
		#endregion
		#region NPC Methods for Reference Manual
		static private void NPCLocation(XmlWriter xmlWriter, ModuleModel moduleModel, List<NPCModel> NPCList)
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
		static public void CreateReferenceByFirstLetter(XmlWriter xmlWriter, ModuleModel moduleModel, List<NPCModel> NPCList)
		{
			NPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
			var AlphabetList = NPCList.GroupBy(x => x.NPCName.ToUpper()[0]).Select(x => x.ToList()).ToList();
			foreach (List<NPCModel> npcList in AlphabetList)
			{
				string actualLetter = npcList[0].NPCName[0] + "";
				ProcessNPCListByLetter(xmlWriter, moduleModel, actualLetter, npcList);
			}
		}
		static private void ProcessNPCListByLetter(XmlWriter xmlWriter, ModuleModel moduleModel, string actualLetter, List<NPCModel> NPCList)
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
		static public void CreateReferenceByCR(XmlWriter xmlWriter, ModuleModel moduleModel, List<NPCModel> NPCList)
		{
			NPCList.Sort((npcOne, npcTwo) => npcOne.ChallengeRating.CompareTo(npcTwo.ChallengeRating));
			var CRList = NPCList.GroupBy(x => x.ChallengeRating.ToUpper()[0]).Select(x => x.ToList()).ToList();
			foreach (List<NPCModel> npcList in CRList)
			{
				string actualCR = npcList[0].ChallengeRating + "";
				ProcessNPCListByCR(xmlWriter, moduleModel, actualCR, npcList);
			}
		}
		static private void ProcessNPCListByCR(XmlWriter xmlWriter, ModuleModel moduleModel, string actualCR, List<NPCModel> NPCList)
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
		static public void CreateReferenceByType(XmlWriter xmlWriter, ModuleModel moduleModel, List<NPCModel> NPCList)
		{
			NPCList.Sort((npcOne, npcTwo) => npcOne.NPCType.CompareTo(npcTwo.NPCType));
			var TypeList = NPCList.GroupBy(x => x.NPCType.ToLower()[0]).Select(x => x.ToList()).ToList();
			foreach (List<NPCModel> npcList in TypeList)
			{
				string actualType = npcList[0].NPCType + "";
				ProcessNPCListByType(xmlWriter, moduleModel, actualType, npcList);
			}
		}
		static private void ProcessNPCListByType(XmlWriter xmlWriter, ModuleModel moduleModel, string actualType, List<NPCModel> NPCList)
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
		static private string NPCNameToXMLFormat(NPCModel npcModel)
		{
			string name = npcModel.NPCName.ToLower();
			return name.Replace(" ", "_").Replace(",", "").Replace("(", "_").Replace(")", "");
		}
		static private string NPCTypeToXMLFormat(string actualType)
		{
			string npcType = actualType.ToLower();
			return npcType.Replace(" ", "");
		}
		static public void SortNPCListByCategory(XmlWriter xmlWriter, NPCModel npcModel, ModuleModel moduleModel, List<NPCModel> NPCList)
        {
            NPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
            var AlphabetList = NPCList.GroupBy(x => x.NPCName.ToUpper()[0]).Select(x => x.ToList()).ToList();
            foreach (List<NPCModel> npcList in AlphabetList)
            {
                string actualLetter = npcList[0].NPCName[0] + "";
                ProcessNPCListByCategoryLetter(xmlWriter, npcModel, moduleModel);
            }
        }
        static private void ProcessNPCListByCategoryLetter(XmlWriter xmlWriter, NPCModel npcModel, ModuleModel moduleModel)
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
		#region Table Methods for Reference Manual
		static private string TableNameToXMLFormat(TableModel tableModel)
		{
			string name = tableModel.Name.ToLower();
			return name.Replace(" ", "_").Replace(",", "").Replace("(", "_").Replace(")", "");
		}
		static private void WriteTableLocked(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("locked");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteString(tableModel.IsLocked ? "1" : "0");
			xmlWriter.WriteEndElement();
		}
		static private void WriteTableName(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(tableModel.Name);
			xmlWriter.WriteEndElement();
		}
		static private void WriteTableDescription(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(tableModel.Description);
			xmlWriter.WriteEndElement();
		}
		static private void WriteTableOutput(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("output");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(tableModel.OutputType.GetDescription().ToLower());
			xmlWriter.WriteEndElement();
		}
		static private void WriteTableNotes(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("notes");
			xmlWriter.WriteAttributeString("type", "formattedtext");
			xmlWriter.WriteString(tableModel.Notes);
			xmlWriter.WriteEndElement();
		}
		static private void WriteTableHideRolls(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("hiderollresults");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteString(tableModel.ShowResultsInChat ? "1" : "0");
			xmlWriter.WriteEndElement();
		}
		static private void WriteTableRollModifier(XmlWriter xmlWriter, TableModel tableModel)
		{
			xmlWriter.WriteStartElement("mod");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(tableModel.CustomRangeModifier);
			xmlWriter.WriteEndElement();
		}
		static private void WriteTableRollDice(XmlWriter xmlWriter, TableModel tableModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (tableModel.RollMethod.GetDescription() == "Custom dice roll")
			{
				for (int counter = 0; counter < tableModel.CustomRangeD4; counter++) { stringBuilder.Append("d4,"); }
				for (int counter = 0; counter < tableModel.CustomRangeD6; counter++) { stringBuilder.Append("d6,"); }
				for (int counter = 0; counter < tableModel.CustomRangeD8; counter++) { stringBuilder.Append("d8,"); }
				for (int counter = 0; counter < tableModel.CustomRangeD10; counter++) { stringBuilder.Append("d10,"); }
				for (int counter = 0; counter < tableModel.CustomRangeD12; counter++) { stringBuilder.Append("d12,"); }
				for (int counter = 0; counter < tableModel.CustomRangeD20; counter++) { stringBuilder.Append("d20,"); }
			}
			xmlWriter.WriteStartElement("dice");
			xmlWriter.WriteAttributeString("type", "dice");
			xmlWriter.WriteString(stringBuilder.ToString().TrimEnd(','));
			xmlWriter.WriteEndElement();
		}
		#endregion
		#region NPC Methods
		static private void WriteAbilities(XmlWriter xmlWriter, NPCModel npcModel)
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
			{
				ChaModifier = "+";
			}				
			else
			{
				ChaModifier = "";
			}				

			if (npcModel.AttributeCon >= 10)
			{
				ConModifier = "+";
			}				
			else
			{
				ConModifier = "";
			}

			if (npcModel.AttributeDex >= 10)
			{
				DexModifier = "+";
			}
			else
			{
				DexModifier = "";
			}
				
			if (npcModel.AttributeInt >= 10)
			{
				IntModifier = "+";
			}				
			else
			{
				IntModifier = "";
			}
				
			if (npcModel.AttributeStr >= 10)
			{
				StrModifier = "+";
			}				
			else
			{
				StrModifier = "";
			}				

			if (npcModel.AttributeWis >= 10)
			{
				WisModifier = "+";
			}				
			else
			{
				WisModifier = "";
			}				

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
		static private void WriteAC(XmlWriter xmlWriter, NPCModel npcModel)
		{
			string[] acArray = npcModel.AC.Split('(');
			string acValue = acArray[0].Trim();
			string acDescription = acArray.Length >= 2 ? "(" + acArray[1] : "";

			xmlWriter.WriteStartElement("ac");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(acValue);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("actext");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(acDescription);
			xmlWriter.WriteEndElement();
		}
		static private void WriteActions(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("actions");
			int actionID = 1;
			foreach (ActionModelBase action in npcModel.NPCActions)
			{
				xmlWriter.WriteStartElement("id-" + actionID.ToString("D4"));
				xmlWriter.WriteStartElement("desc");
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(action.ActionDescription);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("name");
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(action.ActionName);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				actionID = ++actionID;
			}
			xmlWriter.WriteEndElement();
		}
		static private void WriteAlignment(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("alignment");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(npcModel.Alignment);
			xmlWriter.WriteEndElement();
		}
		static private void WriteConditionImmunities(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			xmlWriter.WriteStartElement("conditionimmunities");
			xmlWriter.WriteAttributeString("type", "string");
			if (npcModel.ConditionImmunityModelList != null)
			{
				foreach (SelectableActionModel condition in npcModel.ConditionImmunityModelList)
				{
					if (condition.Selected)
					{
						stringBuilder.Append(condition.ActionDescription.ToLower()).Append(", ");
					}						
				}
			}
			if (npcModel.ConditionOther)
			{
				stringBuilder.Append(npcModel.ConditionOtherText.ToLower() + ", ");
			}				
			if (stringBuilder.Length >= 2)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			xmlWriter.WriteValue(stringBuilder.ToString().Trim());
			xmlWriter.WriteEndElement();
		}
		static private void WriteCR(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("cr");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(npcModel.ChallengeRating);
			xmlWriter.WriteEndElement();
		}
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
			if (weaponDamageResistanceString.StartsWith(";", true, CultureInfo.CurrentCulture))
			{
				weaponDamageResistanceString = weaponDamageResistanceString.Remove(0, 1);
			}				
			if (weaponDamageResistanceString.EndsWith(";", true, CultureInfo.CurrentCulture))
			{
				weaponDamageResistanceString = weaponDamageResistanceString.Substring(0, weaponDamageResistanceString.Length - 1);
			}				
			xmlWriter.WriteString(weaponDamageResistanceString);
			xmlWriter.WriteEndElement();
		}
		static private void WriteDamageVulnerabilities(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			xmlWriter.WriteStartElement("damagevulnerabilities");
			xmlWriter.WriteAttributeString("type", "string");
			if (npcModel.DamageVulnerabilityModelList != null)
			{
				foreach (SelectableActionModel damageVulnerabilities in npcModel.DamageVulnerabilityModelList)
				{
					if (damageVulnerabilities.Selected == true)
					{
						stringBuilder.Append(damageVulnerabilities.ActionDescription.ToLower()).Append(", ");
					}						
				}
			}				
			if (stringBuilder.Length >= 2)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}				
			string weaponDamageVulnerabilityString = stringBuilder.ToString().Trim();
			xmlWriter.WriteValue(weaponDamageVulnerabilityString);
			xmlWriter.WriteEndElement();
		}
		static private void WriteHP(XmlWriter xmlWriter, NPCModel npcModel)
		{
			if (npcModel.HP == null)
			{
				npcModel.HP = "0 (0)";
			}				
			string[] hpArray = npcModel.HP.Split('(');
			string hpValue = hpArray[0].Trim();
			string hpDieBreakdown = "";
			if (hpArray.Length == 2)
			{
				hpDieBreakdown = "(" + hpArray[1];
			}				
			xmlWriter.WriteStartElement("hd");
			xmlWriter.WriteAttributeString("type", "string");
			if (hpArray.Length == 2)
			{
				xmlWriter.WriteString(hpDieBreakdown);
			}
			else
			{
				xmlWriter.WriteString("");
			}				
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("hp");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteString(hpValue);
			xmlWriter.WriteEndElement();
		}
		static private void WriteLairActions(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("lairactions");
			int actionID = 1;
			if (npcModel.LairActions != null)
			{
				foreach (LairAction lairaction in npcModel.LairActions)
				{
					xmlWriter.WriteStartElement("id-" + actionID.ToString("D4"));
					xmlWriter.WriteStartElement("desc");
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString(lairaction.ActionDescription);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("name");
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString(lairaction.ActionName);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					actionID = ++actionID;
				}
			}				
			xmlWriter.WriteEndElement();
		}
		static private void WriteLanguages(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilderOption = new StringBuilder();
			foreach (LanguageModel languageModel in npcModel.StandardLanguages)
			{
				if (languageModel.Selected == true)
				{
					stringBuilder.Append(languageModel.Language).Append(", ");
				}					
			}
			foreach (LanguageModel languageModel in npcModel.ExoticLanguages)
			{
				if (languageModel.Selected == true)
				{
					stringBuilder.Append(languageModel.Language).Append(", ");
				}					
			}
			foreach (LanguageModel languageModel in npcModel.MonstrousLanguages)
			{
				if (languageModel.Selected == true)
				{
					stringBuilder.Append(languageModel.Language).Append(", ");
				}					
			}
			if (npcModel.UserLanguages != null && npcModel.UserLanguages.Count > 0)
			{
				foreach (LanguageModel languageModel in npcModel.UserLanguages)
				{
					if (languageModel.Selected == true)
					{
						stringBuilder.Append(languageModel.Language).Append(", ");
					}
						
				}
			}
			if (npcModel.Telepathy)
			{
				stringBuilder.Append("telepathy " + npcModel.TelepathyRange).Append(", ");
			}				
			if (stringBuilder.Length >= 2)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}				
			if (npcModel.LanguageOptions == "No special conditions" || npcModel.LanguageOptions == null)
			{
				stringBuilderOption.Append(stringBuilder);
			}				
			else if (npcModel.LanguageOptions == "Speaks no languages")
			{
				stringBuilderOption.Append('-');
			}				
			else if (npcModel.LanguageOptions == "Speaks all languages")
			{
				stringBuilderOption.Append("all").Append(", ");
				if (npcModel.Telepathy)
				{
					stringBuilderOption.Append("telepathy " + npcModel.TelepathyRange).Append(", ");
				}					
				stringBuilderOption.Remove(stringBuilderOption.Length - 2, 2);
			}
			else if (npcModel.LanguageOptions == "Can't speak; Knows selected languages")
			{
				stringBuilderOption.Append("understands" + stringBuilder + " but can't speak").Append(", ");
				if (npcModel.Telepathy)
				{
					stringBuilderOption.Append("telepathy " + npcModel.TelepathyRange).Append(", ");
				}					
				stringBuilderOption.Remove(stringBuilderOption.Length - 2, 2);
			}
			else if (npcModel.LanguageOptions == "Can't speak; Knows creator's languages")
			{
				stringBuilderOption.Append("understands the languages of its creator but can't speak").Append(", ");
				if (npcModel.Telepathy)
				{
					stringBuilderOption.Append("telepathy " + npcModel.TelepathyRange).Append(", ");
				}					
				stringBuilderOption.Remove(stringBuilderOption.Length - 2, 2);
			}
			else if (npcModel.LanguageOptions == "Can't speak; Knows languages known in life")
			{
				stringBuilderOption.Append("Understands all languages it spoke in life but can't speak").Append(", ");
				if (npcModel.Telepathy)
				{
					stringBuilderOption.Append("telepathy " + npcModel.TelepathyRange).Append(", ");
				}					
				stringBuilderOption.Remove(stringBuilderOption.Length - 2, 2);
			}
			else if (npcModel.LanguageOptions == "Alternative language text (enter below)")
			{
				stringBuilderOption.Append(npcModel.LanguageOptionsText.ToString().Trim()).Append(", ");
				if (npcModel.Telepathy)
				{
					stringBuilderOption.Append("telepathy " + npcModel.TelepathyRange).Append(", ");
				}					
				stringBuilderOption.Remove(stringBuilderOption.Length - 2, 2);
			}
			xmlWriter.WriteStartElement("languages");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(stringBuilderOption.ToString());
			xmlWriter.WriteEndElement();
		}
		static private void WriteLegendaryActions(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("legendaryactions");
			int actionID = 1;
			foreach (LegendaryActionModel legendaryaction in npcModel.LegendaryActions)
			{
				xmlWriter.WriteStartElement("id-" + actionID.ToString("D4"));
				xmlWriter.WriteStartElement("desc");
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(legendaryaction.ActionDescription);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("name");
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(legendaryaction.ActionName);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				actionID = ++actionID;
			}
			xmlWriter.WriteEndElement();
		}
		static private void WriteName(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(npcModel.NPCName);
			xmlWriter.WriteEndElement();
		}
		static private void WriteReactions(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("reactions");
			int actionID = 1;
			foreach (ActionModelBase reaction in npcModel.Reactions)
			{
				xmlWriter.WriteStartElement("id-" + actionID.ToString("D4"));
				xmlWriter.WriteStartElement("desc");
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(reaction.ActionDescription);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("name");
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(reaction.ActionName);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				actionID = ++actionID;
			}
			xmlWriter.WriteEndElement();
		}
		static private void WriteSavingThrows(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();

			if (npcModel.SavingThrowStr != 0 || npcModel.SavingThrowStrBool)
			{
				stringBuilder.Append("Str ").Append(npcModel.SavingThrowStr >= 0 ? "+" : "").Append(npcModel.SavingThrowStr).Append(", ");
			}				
			if (npcModel.SavingThrowDex != 0 || npcModel.SavingThrowDexBool)
			{
				stringBuilder.Append("Dex ").Append(npcModel.SavingThrowDex >= 0 ? "+" : "").Append(npcModel.SavingThrowDex).Append(", ");
			}				
			if (npcModel.SavingThrowCon != 0 || npcModel.SavingThrowConBool)
			{
				stringBuilder.Append("Con ").Append(npcModel.SavingThrowCon >= 0 ? "+" : "").Append(npcModel.SavingThrowCon).Append(", ");
			}				
			if (npcModel.SavingThrowInt != 0 || npcModel.SavingThrowIntBool)
			{
				stringBuilder.Append("Int ").Append(npcModel.SavingThrowInt >= 0 ? "+" : "").Append(npcModel.SavingThrowInt).Append(", ");
			}				
			if (npcModel.SavingThrowWis != 0 || npcModel.SavingThrowWisBool)
			{
				stringBuilder.Append("Wis ").Append(npcModel.SavingThrowWis >= 0 ? "+" : "").Append(npcModel.SavingThrowWis).Append(", ");
			}				
			if (npcModel.SavingThrowCha != 0 || npcModel.SavingThrowChaBool)
			{
				stringBuilder.Append("Cha ").Append(npcModel.SavingThrowCha >= 0 ? "+" : "").Append(npcModel.SavingThrowCha).Append(", ");
			}			

			if (stringBuilder.Length >= 2)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}				
			string savingThrowString = stringBuilder.ToString().Trim();

			xmlWriter.WriteStartElement("savingthrows");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(savingThrowString);
			xmlWriter.WriteEndElement();
		}
		static private void WriteSenses(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(AppendSenses("darkvision ", npcModel.Darkvision, " ft."));
			stringBuilder.Append(AppendBlindSenses("blindsight ", npcModel.Blindsight, " ft."));
			stringBuilder.Append(AppendSenses("tremorsense ", npcModel.Tremorsense, " ft."));
			stringBuilder.Append(AppendSenses("truesight ", npcModel.Truesight, " ft."));
			stringBuilder.Append(AppendSenses("passive perception ", npcModel.PassivePerception, ""));
			if (stringBuilder.Length >= 2)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}				
			string sensesString = stringBuilder.ToString().Trim();
			xmlWriter.WriteStartElement("senses");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(sensesString);
			xmlWriter.WriteEndElement();
		}
		static private string AppendSenses(string senseName, int senseValue, string senseRange)
		{
			if (senseValue != 0)
			{
				string delimiter = ", ";
				return senseName + senseValue + senseRange + delimiter;
			}
			return "";
		}
		static private string AppendBlindSenses(string senseName, int senseValue, string senseRange)
		{
			NPCModel npcModel = new NPCModel();
			string delimiter = ", ";
			if (senseValue != 0 && npcModel.BlindBeyond == false)
			{
				return senseName + senseValue + senseRange + delimiter;
			}				
			else if (senseValue != 0 && npcModel.BlindBeyond == true)
			{
				return senseName + senseValue + senseRange + " (blind beyond this radius)" + delimiter;
			}				
			return "";
		}
		static private void WriteSpeed(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();

			if (npcModel.Speed > 0)
			{
				stringBuilder.Append(npcModel.Speed + " ft.").Append(", ");
			}				
			if (npcModel.Burrow > 0)
			{
				stringBuilder.Append("burrow " + npcModel.Burrow + " ft.").Append(", ");
			}				
			if (npcModel.Climb > 0)
			{
				stringBuilder.Append("climb " + npcModel.Climb + " ft.").Append(", ");
			}				
			if (npcModel.Hover)
			{
				stringBuilder.Append("fly " + npcModel.Fly + " ft. (hover)").Append(", ");
			}				
			if (npcModel.Fly > 0 && !npcModel.Hover)
			{
				stringBuilder.Append("fly " + npcModel.Fly + " ft.").Append(", ");
			}				
			if (npcModel.Swim > 0)
			{
				stringBuilder.Append("swim " + npcModel.Swim + " ft.").Append(", ");
			}				
			if (stringBuilder.Length >= 2)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}				
			string speedString = stringBuilder.ToString().Trim();
			xmlWriter.WriteStartElement("speed");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(speedString);
			xmlWriter.WriteEndElement();
		}
		static private void WriteSize(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("size");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(npcModel.Size);
			xmlWriter.WriteEndElement();
		}
		static private void WriteType(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(npcModel.NPCType);
			if (npcModel.Tag != null)
			{
				stringBuilder.Append(" " + npcModel.Tag);
			}
			xmlWriter.WriteStartElement("type");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(stringBuilder.ToString());
			xmlWriter.WriteEndElement();
		}
		static private void WriteSkills(XmlWriter xmlWriter, NPCModel npcModel)
		{
			string skillsString = npcModel.SkillAttributesToString();
			xmlWriter.WriteStartElement("skills");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(skillsString);
			xmlWriter.WriteEndElement();
		}
		static private void WriteText(XmlWriter xmlWriter, NPCModel npcModel)
		{
			NPCController npcController = new NPCController();
			xmlWriter.WriteStartElement("text");
			xmlWriter.WriteAttributeString("type", "formattedtext");
			xmlWriter.WriteRaw(npcController.GenerateFantasyGroundsDescriptionXML(npcModel.Description));
			xmlWriter.WriteEndElement();
		}
		static private void WriteToken(XmlWriter xmlWriter, NPCModel npcModel, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("token");
			xmlWriter.WriteAttributeString("type", "token");
			if (npcModel.NPCToken == null || npcModel.NPCToken == " " || !moduleModel.IncludeTokens)
			{
				xmlWriter.WriteString("");
			}				
			else
			{
				xmlWriter.WriteValue("tokens\\" + Path.GetFileName(npcModel.NPCToken) + "@" + moduleModel.Name);
			}				
			xmlWriter.WriteEndElement();
		}
		static private void WriteTraits(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("traits");
			int actionID = 1;
			string innateName = "";
			string spellcastingName = "";

			if (npcModel.Traits != null)
			{
				foreach (ActionModelBase traits in npcModel.Traits)
				{
					xmlWriter.WriteStartElement("id-" + actionID.ToString("D4"));
					xmlWriter.WriteStartElement("desc");
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString(traits.ActionDescription);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("name");
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString(traits.ActionName);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					actionID = ++actionID;
				}
			}
			if (npcModel.Psionics)
			{
				innateName = "Innate Spellcasting (Psionics)";
			}				
			else if (npcModel.InnateSpellcastingSection && !npcModel.Psionics)
			{
				innateName = "Innate Spellcasting";
			}				
			if (innateName.Length > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(npcModel.InnateSpellcastingAbility))
				{
					stringBuilder.Append("The " + npcModel.NPCName.ToLower() + "'s innate spellcasting ability is " + npcModel.InnateSpellcastingAbility);
				}					
				else
				{
					log.Warn("Innate Spellcasting Ability missing for " + npcModel.NPCName);
					MessageBox.Show("Please fill in the Innate Spellcasting Ability");
				}
					
				if (npcModel.InnateSpellSaveDC != 0)
				{
					stringBuilder.Append(" (spell save DC " + npcModel.InnateSpellSaveDC);
					if (npcModel.InnateSpellHitBonus != 0)
					{
						stringBuilder.Append("spell hit bonus ").Append(npcModel.InnateSpellHitBonus >= 0 ? "+" : "").Append(npcModel.InnateSpellHitBonus);
					}						
					stringBuilder.Append(')');
				}
				else if (npcModel.InnateSpellHitBonus != 0)
				{
					stringBuilder.Append("(spell hit bonus ").Append(npcModel.InnateSpellHitBonus >= 0 ? "+" : "").Append(npcModel.InnateSpellHitBonus + ")");
				}					
				stringBuilder.Append(". ");
				stringBuilder.Append("It can innately cast the following spells, " + npcModel.ComponentText + ":");
				if (npcModel.InnateAtWill != null)
				{
					stringBuilder.Append("\\rAt will: " + npcModel.InnateAtWill);
				}					
				if (npcModel.FivePerDay != null)
				{
					stringBuilder.Append("\\r5/day each: " + npcModel.FivePerDay);
				}					
				if (npcModel.FourPerDay != null)
				{
					stringBuilder.Append("\\r4/day each: " + npcModel.FourPerDay);
				}					
				if (npcModel.ThreePerDay != null)
				{
					stringBuilder.Append("\\r3/day each: " + npcModel.ThreePerDay);
				}					
				if (npcModel.TwoPerDay != null)
				{
					stringBuilder.Append("\\r2/day each: " + npcModel.TwoPerDay);
				}					
				if (npcModel.OnePerDay != null)
				{
					stringBuilder.Append("\\r1/day each: " + npcModel.OnePerDay);
				}					
				string innateCastingDescription = stringBuilder.ToString();

				xmlWriter.WriteStartElement("id-" + actionID.ToString("D4"));
				xmlWriter.WriteStartElement("desc");
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(innateCastingDescription);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("name");
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(innateName);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				actionID = ++actionID;
			}
			if (npcModel.SpellcastingSection)
			{
				spellcastingName = "Spellcasting";
			}				
			if (spellcastingName.Length > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(npcModel.SpellcastingCasterLevel))
				{
					stringBuilder.Append("The " + npcModel.NPCName.ToLower() + " is a " + npcModel.SpellcastingCasterLevel + "-level spellcaster. ");
				}					
				else
				{
					log.Warn("Spellcasting level is missing for " + npcModel.NPCName);
					MessageBox.Show("Please fill in the Spellcasting Level");
				}
					
				if (!string.IsNullOrEmpty(npcModel.SCSpellcastingAbility))
				{
					stringBuilder.Append("Its spellcasting ability is " + npcModel.SCSpellcastingAbility);
				}					
				else
				{
					log.Warn("Spellcasting ability is missing for " + npcModel.NPCName);
					MessageBox.Show("Please fill in the Spellcasting Ability");
				}
					
				if (npcModel.SpellcastingSpellSaveDC != 0)
				{
					stringBuilder.Append(" (spell save DC " + npcModel.SpellcastingSpellSaveDC);
					if (npcModel.SpellcastingSpellHitBonus != 0)
					{
						stringBuilder.Append(", spell hit bonus ").Append(npcModel.SpellcastingSpellHitBonus >= 0 ? "+" : "").Append(npcModel.SpellcastingSpellHitBonus);
					}						
					stringBuilder.Append(')');
				}
				stringBuilder.Append(". ");
				if (!string.IsNullOrEmpty(npcModel.SpellcastingSpellClass))
				{
					stringBuilder.Append("The " + npcModel.NPCName.ToLower() + " has the following " + npcModel.SpellcastingSpellClass.ToLower() + " spells prepared:");
				}					
				else
				{
					stringBuilder.Append("The " + npcModel.NPCName.ToLower() + " has the following spells prepared:");
				}					
				if (npcModel.CantripSpellList != null)
				{
					stringBuilder.Append("\\rCantrips (" + npcModel.CantripSpells.ToLower() + "): " + npcModel.CantripSpellList.ToLower());
				}					
				if (npcModel.FirstLevelSpellList != null)
				{
					stringBuilder.Append("\\r1st level (" + npcModel.FirstLevelSpells.ToLower() + "): " + npcModel.FirstLevelSpellList.ToLower());
				}					
				if (npcModel.SecondLevelSpellList != null)
				{
					stringBuilder.Append("\\r2nd level (" + npcModel.SecondLevelSpells.ToLower() + "): " + npcModel.SecondLevelSpellList.ToLower());
				}				
				if (npcModel.ThirdLevelSpellList != null)
				{
					stringBuilder.Append("\\r3rd level (" + npcModel.ThirdLevelSpells.ToLower() + "): " + npcModel.ThirdLevelSpellList.ToLower());
				}					
				if (npcModel.FourthLevelSpellList != null)
				{
					stringBuilder.Append("\\r4th level (" + npcModel.FourthLevelSpells.ToLower() + "): " + npcModel.FourthLevelSpellList.ToLower());
				}					
				if (npcModel.FifthLevelSpellList != null)
				{
					stringBuilder.Append("\\r5th level (" + npcModel.FifthLevelSpells.ToLower() + "): " + npcModel.FifthLevelSpellList.ToLower());
				}
				if (npcModel.SixthLevelSpellList != null)
				{
					stringBuilder.Append("\\r6th level (" + npcModel.SixthLevelSpells.ToLower() + "): " + npcModel.SixthLevelSpellList.ToLower());
				}					
				if (npcModel.SeventhLevelSpellList != null)
				{
					stringBuilder.Append("\\r7th level (" + npcModel.SeventhLevelSpells.ToLower() + "): " + npcModel.SeventhLevelSpellList.ToLower());
				}					
				if (npcModel.EighthLevelSpellList != null)
				{
					stringBuilder.Append("\\r8th level (" + npcModel.EighthLevelSpells.ToLower() + "): " + npcModel.EighthLevelSpellList.ToLower());
				}					
				if (npcModel.NinthLevelSpellList != null)
				{
					stringBuilder.Append("\\r9th level (" + npcModel.NinthLevelSpells.ToLower() + "): " + npcModel.NinthLevelSpellList.ToLower());
				}					
				string spellcastingDescription = stringBuilder.ToString();
				xmlWriter.WriteStartElement("id-" + actionID.ToString("D4"));
				xmlWriter.WriteStartElement("desc");
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(spellcastingDescription);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("name");
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(spellcastingName);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				actionID = ++actionID;
			}
			xmlWriter.WriteEndElement();
		}
		static private void WriteXP(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("xp");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(npcModel.XP);
			xmlWriter.WriteEndElement();
		}
		#endregion
		/// <summary>
		/// Generates the Definition file used in Fantasy Grounds modules
		/// </summary>
		public string GenerateDefinitionXmlContent(ModuleModel moduleModel)
		{
			using (StringWriter sw = new StringWriterWithEncoding(Encoding.UTF8))
			using (XmlWriter xmlWriter = XmlWriter.Create(sw, GetXmlWriterSettings()))
			{
				xmlWriter.WriteStartDocument();
				xmlWriter.WriteStartElement("root");
				xmlWriter.WriteAttributeString("version", "4");
				xmlWriter.WriteStartElement("name");
				xmlWriter.WriteString(moduleModel.Name);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("category");
				xmlWriter.WriteString(moduleModel.Category);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("author");
				xmlWriter.WriteString(moduleModel.Author);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("ruleset");
				xmlWriter.WriteString("5E");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndDocument();
				xmlWriter.Close();
				return sw.ToString();
			}
		}
		/// <summary>
		/// XML Writer settings used to generate modules
		/// </summary>
		static private XmlWriterSettings GetXmlWriterSettings()
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


