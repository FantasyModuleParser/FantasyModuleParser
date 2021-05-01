using FantasyModuleParser.Extensions;
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
					foreach (string castByValue in GetSortedSpellCasterList(moduleModel))
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
					CreateSpellReferenceByFirstLetter(xmlWriter, moduleModel, FatSpellList);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					#endregion
					#region Spell List By Class
					SpellListByClass(xmlWriter, moduleModel);
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
							WriteColumnLabels(xmlWriter, tableModel);
							WriteResultsColumn(xmlWriter, tableModel);
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
					xmlWriter.WriteString("reference.spelldata." + SpellNameToXMLFormat(spell) + '@' + moduleModel.Name);
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
		static private void WriteColumnLabels(XmlWriter xmlWriter, TableModel tableModel)
		{
			for (int columnHeaderIndex = 2; columnHeaderIndex < tableModel.ColumnHeaderLabels.Count; columnHeaderIndex++)
			{
				string columnHeaderValue = tableModel.ColumnHeaderLabels[columnHeaderIndex];
				xmlWriter.WriteStartElement(string.Format("labelcol{0}", columnHeaderIndex - 1));
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(columnHeaderValue);
				xmlWriter.WriteEndElement();
			}
		}
		static private void WriteResultsColumn(XmlWriter xmlWriter, TableModel tableModel)
		{
			int resultHeaders = tableModel.ColumnHeaderLabels.Count - 2;
			xmlWriter.WriteStartElement("resultscols");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(resultHeaders);
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


