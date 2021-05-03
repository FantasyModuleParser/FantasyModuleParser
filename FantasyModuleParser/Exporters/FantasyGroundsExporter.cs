﻿using FantasyModuleParser.Extensions;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.Spells.Models;
using FantasyModuleParser.Tables.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
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
		/// <summary>
		/// Check whether the module is meant for only GM's eyes or also players
		/// </summary>
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

			/// <summary>
			/// Make sure selected Module Folder exists
			/// </summary>
			if (string.IsNullOrEmpty(settingsModel.FGModuleFolderLocation))
			{
				log.Warn("Create Module -- No Module path has been set. Saved Path :: " + settingsModel.FGModuleFolderLocation);
				throw new ApplicationException("No Module Path has been set");
			}
			
			/// <summary>
			/// Make sure a name has been given to the Module
			/// </summary>
			if (string.IsNullOrEmpty(moduleModel.Name))
			{
				log.Warn("Create Module -- No Module name has been saved.");
				throw new ApplicationException("No Module Name has been set");
			}

			// Create the folder all content will go into based on the Module name
			string moduleFolderPath = Path.Combine(settingsModel.FGModuleFolderLocation, moduleModel.Name);
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

			/// <summary>
			/// Check whether *.mod file exists
			/// If it does, delete it.
			/// Then Zip the Module folder, save to *.mod, and delete Module folder.
			/// Lastly, put an entry into the Log file stating that it saved correctly and where.
			/// </summary>
			if (File.Exists(@Path.Combine(settingsModel.FGModuleFolderLocation, moduleModel.ModFilename + ".mod")))
			{
				File.Delete(@Path.Combine(settingsModel.FGModuleFolderLocation, moduleModel.ModFilename + ".mod"));
			}
			ZipFile.CreateFromDirectory(moduleFolderPath, Path.Combine(settingsModel.FGModuleFolderLocation, moduleModel.ModFilename + ".mod"));
			DeleteDirectory(moduleFolderPath);
			log.Debug("Module Created!!  Saved to :: " + Path.Combine(settingsModel.FGModuleFolderLocation, moduleModel.ModFilename + ".mod"));
		}
		/// <summary>
		/// Makes sure all files in the module folder are deletable and executes the delete command
		/// </summary>
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
		/// Currently covers NPCs, Spells, and Tables.
		/// </summary>
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
								WriteLocked(xmlWriter);
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
							xmlWriter.WriteStartElement(NPCExporter.NPCNameToXMLFormat(npcModel)); // Open <npcModel.NPCName>
							WriteLocked(xmlWriter);
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
							xmlWriter.WriteStartElement(SpellExporter.SpellNameToXMLFormat(spellModel));
							WriteLocked(xmlWriter);
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
					NPCExporter.CreateReferenceByFirstLetter(xmlWriter, moduleModel, FatNPCList);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("bylevel");
					xmlWriter.WriteStartElement("description");
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString("NPCs");
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("groups");
					NPCExporter.CreateReferenceByCR(xmlWriter, moduleModel, FatNPCList);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("bytype");
					xmlWriter.WriteStartElement("description");
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString("NPCs");
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("groups");
					NPCExporter.CreateReferenceByType(xmlWriter, moduleModel, FatNPCList);
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
					foreach (string castByValue in SpellExporter.GetSortedSpellCasterList(moduleModel))
					{
						string referenceId = "reference.spellists.";
						referenceId += castByValue.Replace(" ", "").Replace("(", "").Replace(")", "").ToLower();
						referenceId += "@" + moduleModel.Name;
						WriteIDLinkList(xmlWriter, moduleModel, "id-" + spellListId.ToString("D4"), referenceId, castByValue);

						spellListId++;
					}
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();

					#region Spell Index
					xmlWriter.WriteStartElement("_index_");
					xmlWriter.WriteStartElement("description");
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString("Spell Index");
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("groups");
					SpellExporter.CreateSpellReferenceByFirstLetter(xmlWriter, moduleModel, FatSpellList);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					#endregion
					#region Spell List By Class
					SpellExporter.SpellListByClass(xmlWriter, moduleModel);
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
							xmlWriter.WriteStartElement(TableExporter.TableNameToXMLFormat(tableModel)); // Open <tableModel.Name>
							TableExporter.WriteTableLocked(xmlWriter, tableModel);
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
		#region NPC Methods for Reference Data XML
		public void WriteDamageImmunities(XmlWriter xmlWriter, NPCModel npcModel)
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
		public void WriteDamageResistances(XmlWriter xmlWriter, NPCModel npcModel)
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
		public void WriteDamageVulnerabilities(XmlWriter xmlWriter, NPCModel npcModel)
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
		#endregion
		#region Common methods
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


