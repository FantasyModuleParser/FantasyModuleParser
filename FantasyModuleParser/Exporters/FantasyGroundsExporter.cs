using FantasyModuleParser.Extensions;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
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
			string moduleFolderPath = Path.Combine(settingsModel.FGModuleFolderLocation, moduleModel.ModFilename);
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
			string ThumbnailFilename = Path.Combine(settingsService.Load().FGModuleFolderLocation, moduleModel.ModFilename, "thumbnail.png");
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
					#region Token Data
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
					#endregion
				}
            }
			/// <summary>
			/// Names all images to match NPC name
			/// </summary>
			foreach (NPCModel npcModel in FatNPCList)
			{
				if (moduleModel.IncludeImages)
                {
					#region Image Data
					if (!string.IsNullOrEmpty(npcModel.NPCImage))
					{
						if (npcModel.NPCImage.StartsWith("file:///"))
						{
							npcModel.NPCImage.Remove(0, 8);
						}
						string Filename = Path.GetFileName(npcModel.NPCImage).Replace("-", "").Replace(" ", "").Replace(",", "");
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
					#endregion
				}
			}

			using (StringWriter sw = new StringWriterWithEncoding(Encoding.UTF8))
			using (XmlWriter xmlWriter = XmlWriter.Create(sw, GetXmlWriterSettings()))
			{
				xmlWriter.WriteStartDocument();

				xmlWriter.WriteComment("Generated by Fantasy Module Parser");
				xmlWriter.WriteComment("Written by Theodore Story, Darkpool, and Battlemarch (c) 2021");

				xmlWriter.WriteStartElement("root"); // <root>
				xmlWriter.WriteAttributeString("version", "4.0"); /* <root version="4.0"> */
				if (moduleModel.IncludeImages)
                {
					#region Image XML
					xmlWriter.WriteStartElement("image"); /* <root version="4.0"> <image> */
					foreach (CategoryModel categoryModel in moduleModel.Categories)
					{
						xmlWriter.WriteStartElement("category"); /* <root version="4.0"> <image> <category> */
						xmlWriter.WriteAttributeString("name", categoryModel.Name); /* <root version="4.0"> <image> <category> */
						xmlWriter.WriteAttributeString("baseicon", "0"); 
						xmlWriter.WriteAttributeString("decalicon", "0");
						foreach (NPCModel npcModel in categoryModel.NPCModels)
						{
							if (!string.IsNullOrEmpty(npcModel.NPCImage))
							{
								xmlWriter.WriteStartElement(Path.GetFileNameWithoutExtension(npcModel.NPCImage).Replace(" ", "").Replace("-", "")); 
								/* <root version="4.0"> <image> <category> <image_name> */
								xmlWriter.WriteStartElement("image"); /* <root version="4.0"> <image> <category> <image_name> <image> */
								xmlWriter.WriteAttributeString("type", "image");
								xmlWriter.WriteStartElement("bitmap"); /* <root version="4.0"> <image> <category> <image_name> <image> <bitmap> */
								xmlWriter.WriteAttributeString("type", "string");
                                xmlWriter.WriteString("images" + "\\" + Path.GetFileName(npcModel.NPCImage).Replace(" ", "").Replace("-", ""));
								xmlWriter.WriteEndElement(); /* <root version="4.0"> <image> <category> <image_name> <image> <bitmap> </bitmap> */
								xmlWriter.WriteEndElement(); /* <root version="4.0"> <image> <category> <image_name> <image> </image> */
								WriteLocked(xmlWriter);
								NPCExporter.WriteName(xmlWriter, npcModel);
								if (!string.IsNullOrEmpty(npcModel.NonID))
                                {
									xmlWriter.WriteStartElement("nonid_name"); /* <root version="4.0"> <image> <category> <image_name> <nonid_name> */
									xmlWriter.WriteAttributeString("type", "string");
									xmlWriter.WriteString(npcModel.NonID);
									xmlWriter.WriteEndElement(); /* <root version="4.0"> <image> <category> <image_name> <nonid_name> </nonid_name> */
								}
								if (!string.IsNullOrEmpty(npcModel.NonID))
                                {
									xmlWriter.WriteStartElement("isidentified"); /* <root version="4.0"> <image> <category> <image_name> <isidentified> */
									xmlWriter.WriteAttributeString("type", "number");
									xmlWriter.WriteString("0");
									xmlWriter.WriteEndElement(); /* <root version="4.0"> <image> <category> <image_name> <isidentified> </isidentified> */
								}
								xmlWriter.WriteEndElement(); /* <root version="4.0"> <image> <category> <image_name> </image_name> */
							}
						}
						xmlWriter.WriteEndElement(); /* <root version="4.0"> <image> <category> </category> */
					}
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <image> </image> */
					#endregion
				}
				#region Reference Section
				xmlWriter.WriteStartElement("reference"); /* <root version="4.0"> <reference> */
				if (moduleModel.IsLockedRecords)
                {
					xmlWriter.WriteAttributeString("static", "true");
				}
				if (moduleModel.IncludeNPCs)
                {
					#region NPC Data
					xmlWriter.WriteStartElement("npcdata"); /* <root version="4.0"> <reference> <npcdata> */

					//Category section in the XML generation
					foreach (CategoryModel categoryModel in moduleModel.Categories)
					{
						xmlWriter.WriteStartElement("category"); /* <root version="4.0"> <reference> <npcdata> <category> */
						xmlWriter.WriteAttributeString("name", categoryModel.Name);
						xmlWriter.WriteAttributeString("baseicon", "0");
						xmlWriter.WriteAttributeString("decalicon", "0");

						//Now, write out each NPC with NPC Name
						foreach (NPCModel npcModel in FatNPCList)
						{
							xmlWriter.WriteStartElement(NPCExporter.NPCNameToXMLFormat(npcModel)); 
							/* <root version="4.0"> <reference> <npcdata> <category> <npcModel.NPCName> */
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
							xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <npcdata> <category> <npcModel.NPCName> </npcModel.NPCName> */
						}
						xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <npcdata> <category> </category> */
					}
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <npcdata> </npcdata> */
					#endregion
				}
				if (moduleModel.IncludeSpells)
                {
					#region Spell Data
					xmlWriter.WriteStartElement("spelldata"); /* <root version="4.0"> <reference> <spelldata> */
					foreach (CategoryModel categoryModel in moduleModel.Categories)
					{
						xmlWriter.WriteStartElement("category"); /* <root version="4.0"> <reference> <spelldata> <category> */
						xmlWriter.WriteAttributeString("name", categoryModel.Name);
						xmlWriter.WriteAttributeString("baseicon", "0");
						xmlWriter.WriteAttributeString("decalicon", "0");
						foreach (SpellModel spellModel in FatSpellList)
						{
							xmlWriter.WriteStartElement(SpellExporter.SpellNameToXMLFormat(spellModel)); 
							/* <root version="4.0"> <reference> <spelldata> <category> <spell_name> */
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
							xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <spelldata> <category> <spell_name> </spell_name> */
						}
						xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <spelldata> <category> </category> */
					}
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <spelldata> </spelldata> */
					#endregion
				}
				#region Equipment Data
				/*	xmlWriter.WriteStartElement("equipmentdata");
				 *	xmlWriter.WriteAttributeString("static", "true"); 
				 *	xmlWriter.WriteString(" ");
				 *	xmlWriter.WriteEndElement(); */
				#endregion
				#region Equipment Lists
				/*	xmlWriter.WriteStartElement("equipmentlists");
				 *	xmlWriter.WriteStartElement("equipment");
				 *	xmlWriter.WriteStartElement("name");
				 *	xmlWriter.WriteAttributeString("type", "string");
				 *	xmlWriter.WriteString("Equipment");
				 *	xmlWriter.WriteEndElement();
				 *	xmlWriter.WriteEndElement();
				 *	xmlWriter.WriteEndElement(); */
				#endregion
				if (moduleModel.IncludeImages)
                {
					#region Image Lists
					xmlWriter.WriteStartElement("imagelists"); /* <root version="4.0"> <reference> <imagelists> */
					xmlWriter.WriteStartElement("bycategory"); /* <root version="4.0"> <reference> <imagelists> <bycategory> */
					xmlWriter.WriteStartElement("description"); /* <root version="4.0"> <reference> <imagelists> <bycategory> <description> */
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString("Images");
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <imagelists> <bycategory> <description> </description>*/
					xmlWriter.WriteStartElement("groups"); /* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> */
					foreach (CategoryModel categoryModel in moduleModel.Categories)
					{
						xmlWriter.WriteStartElement(CategoryNameToXML(categoryModel)); 
						/* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> */
						xmlWriter.WriteStartElement("description"); 
						/* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> <description> */
						xmlWriter.WriteAttributeString("type", "string");
						xmlWriter.WriteString(categoryModel.Name);
						xmlWriter.WriteEndElement(); 
						/* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> <description> </description> */
						xmlWriter.WriteStartElement("index"); /* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> <index> */
						foreach (NPCModel npcModel in categoryModel.NPCModels)
						{
							if (!string.IsNullOrEmpty(npcModel.NPCImage))
							{
								xmlWriter.WriteStartElement(Path.GetFileNameWithoutExtension(npcModel.NPCImage).Replace(" ", "").Replace("-", ""));
								/* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> <index> <image_name> */
								xmlWriter.WriteStartElement("link"); 
								/* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> <index> <image_name> <link> */
								xmlWriter.WriteAttributeString("type", "windowreference");
								xmlWriter.WriteStartElement("class"); 
								/* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> <index> <image_name> <link> <class> */
								xmlWriter.WriteString("imagewindow");
								xmlWriter.WriteEndElement(); 
								/* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> <index> <image_name> <link> <class>
								 * </class>*/
								xmlWriter.WriteStartElement("recordname"); 
								/* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> <index> <image_name> <link>
								 * <recordname> */
								xmlWriter.WriteString("image." + Path.GetFileNameWithoutExtension(npcModel.NPCImage).Replace(" ", "").Replace("-", ""));
								xmlWriter.WriteEndElement(); 
								/* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> <index> <image_name> <link>
								 * <recordname> </recordname>*/
								xmlWriter.WriteStartElement("description"); 
								/* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> <index> <image_name> <link>
								 * <description> */
								xmlWriter.WriteStartElement("field");
								/* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> <index> <image_name> <link>
								 * <description> <field> */
								xmlWriter.WriteString("name");
								xmlWriter.WriteEndElement();
								/* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> <index> <image_name> <link>
								 * <description> <field> </field> */
								xmlWriter.WriteEndElement();
								/* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> <index> <image_name> <link>
								 * <description> </description> */
								xmlWriter.WriteEndElement();
								/* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> <index> <image_name> <link> </link> */
								xmlWriter.WriteStartElement("source"); 
								/* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> <index> <image_name> <source> */
								xmlWriter.WriteAttributeString("type", "string");
								xmlWriter.WriteEndElement(); 
								/* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> <index> <image_name> <source> </source> */
								xmlWriter.WriteEndElement(); 
								/* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> <index> <image_name> </image_name> */
							}
						}
						xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> <index> </index> */
						xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> <category> </category> */
					}
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <imagelists> <bycategory> <groups> </groups> */
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <imagelists> <bycategory> </bycategory> */
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <imagelists> </imagelists> */
					#endregion
				}
				if (moduleModel.IncludeNPCs)
                {
					#region NPC Lists
					xmlWriter.WriteStartElement("npclists"); /* <root version="4.0"> <reference> <npclists> */
					xmlWriter.WriteStartElement("npcs"); /* <root version="4.0"> <reference> <npclists> <npcs> */
					xmlWriter.WriteStartElement("name"); /* <root version="4.0"> <reference> <npclists> <npcs> <name> */
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString("NPCs");
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <npclists> <npcs> <name> </name> */
					xmlWriter.WriteStartElement("index"); /* <root version="4.0"> <reference> <npclists> <npcs> <index> */
					WriteIDLinkList(xmlWriter, moduleModel, "id01", "reference.npclists.byletter@" + moduleModel.Name, "NPCs - Alphabetical Index");
					WriteIDLinkList(xmlWriter, moduleModel, "id02", "reference.npclists.bylevel@" + moduleModel.Name, "NPCs - Challenge Rating Index");
					WriteIDLinkList(xmlWriter, moduleModel, "id03", "reference.npclists.bytype@" + moduleModel.Name, "NPCs - Class Index");
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <npclists> <npcs> <index> </index> */
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <npclists> <npcs> </npcs> */
					xmlWriter.WriteStartElement("byletter"); /* <root version="4.0"> <reference> <npclists> <byletter> */
					xmlWriter.WriteStartElement("description"); /* <root version="4.0"> <reference> <npclists> <byletter> <description> */
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString("NPCs");
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <npclists> <byletter> <description> </description> */
					xmlWriter.WriteStartElement("groups"); /* <root version="4.0"> <reference> <npclists> <byletter> <groups> */
					NPCExporter.CreateReferenceByFirstLetter(xmlWriter, moduleModel, FatNPCList);
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <npclists> <byletter> <groups> </groups> */
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <npclists> <byletter> </byletter> */
					xmlWriter.WriteStartElement("bylevel"); /* <root version="4.0"> <reference> <npclists> <bylevel> */
					xmlWriter.WriteStartElement("description"); /* <root version="4.0"> <reference> <npclists> <bylevel> <description> */
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString("NPCs"); 
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <npclists> <bylevel> <description> </description> */
					xmlWriter.WriteStartElement("groups"); /* <root version="4.0"> <reference> <npclists> <bylevel> <groups> */
					NPCExporter.CreateReferenceByCR(xmlWriter, moduleModel, FatNPCList);
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <npclists> <bylevel> <groups> </groups> */
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <npclists> <bylevel> </bylevel> */
					xmlWriter.WriteStartElement("bytype"); /* <root version="4.0"> <reference> <npclists> <bytype> */
					xmlWriter.WriteStartElement("description"); /* <root version="4.0"> <reference> <npclists> <bytype> <description> */
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString("NPCs");
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <npclists> <bytype> <description> </description> */
					xmlWriter.WriteStartElement("groups"); /* <root version="4.0"> <reference> <npclists> <bytype> <groups> */
					NPCExporter.CreateReferenceByType(xmlWriter, moduleModel, FatNPCList);
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <npclists> <bytype> <groups> </groups> */
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <npclists> <bytype> </bytype> */
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <npclists> </npclists> */
					#endregion
				}
				if (moduleModel.IncludeSpells)
                {
					#region Spell Lists
					xmlWriter.WriteStartElement("spelllists"); /* <root version="4.0"> <reference> <spelllists> */
					xmlWriter.WriteStartElement("spells"); /* <root version="4.0"> <reference> <spelllists> <spells> */
					xmlWriter.WriteStartElement("name"); /* <root version="4.0"> <reference> <spelllists> <spells> <name> */
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString("Spells");
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <spelllists> <spells> <name> </name> */
					xmlWriter.WriteStartElement("index"); /* <root version="4.0"> <reference> <spelllists> <spells> <index> */
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
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <spelllists> <spells> <index> <text> </text> */
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <spelllists> <spells> <index> </index> */

					#region Spell Index
					xmlWriter.WriteStartElement("_index_"); /* <root version="4.0"> <reference> <spelllists> <spells> <_index_> */
					xmlWriter.WriteStartElement("description"); /* <root version="4.0"> <reference> <spelllists> <spells> <_index_> <description> */
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString("Spell Index");
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <spelllists> <spells> <_index_> <description> </description> */
					xmlWriter.WriteStartElement("groups"); /* <root version="4.0"> <reference> <spelllists> <spells> <_index_> <groups> */
					SpellExporter.CreateSpellReferenceByFirstLetter(xmlWriter, moduleModel, FatSpellList);
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <spelllists> <spells> <_index_> <groups> </groups> */
					xmlWriter.WriteEndElement(); /* <root version="4.0"> <reference> <spelllists> <spells> <_index_> </index> */
					#endregion
					SpellExporter.SpellListByClass(xmlWriter, moduleModel);
					xmlWriter.WriteEndElement(); // Close </spelllists>
					#endregion
				}
				if (moduleModel.IncludeTables)
				{
					#region Tables Data
					xmlWriter.WriteStartElement("tables"); /* <root> <reference> <tables> */
					foreach (CategoryModel categoryModel in moduleModel.Categories)
					{
						xmlWriter.WriteStartElement("category"); /* <root> <reference> <tables> <category> */
						xmlWriter.WriteAttributeString("name", categoryModel.Name);
						xmlWriter.WriteAttributeString("baseicon", "0");
						xmlWriter.WriteAttributeString("decalicon", "0");

						//Now, write out each NPC with NPC Name
						foreach (TableModel tableModel in FatTableList)
						{
							xmlWriter.WriteStartElement(TableExporter.TableNameToXMLFormat(tableModel)); 
							/* <root> <reference> <tables> <category> <tableModel.Name> */
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
							xmlWriter.WriteEndElement(); /* <root> <reference> <tables> <category> <tableModel.Name> </tableModel.Name>*/
						}
						xmlWriter.WriteEndElement(); /* <root> <reference> <tables> <category> </category>*/
					}
					xmlWriter.WriteEndElement(); /* <root> <reference> <tables> </tables>*/
					#endregion
				}

				#region Reference Manual
				xmlWriter.WriteStartElement("referencemanual");  /* <root> <reference> <referencemanual */
				xmlWriter.WriteStartElement("name"); /* <root> <reference> <referencemanual <name> */
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(moduleModel.Name);
				xmlWriter.WriteEndElement(); /* <root> <reference> <referencemanual <name> </name> */
				xmlWriter.WriteStartElement("chapters"); /* <root> <reference> <referencemanual <chapters> */
				xmlWriter.WriteStartElement("chapter_00"); /* <root> <reference> <referencemanual <chapters> <chapter_00> */
				xmlWriter.WriteStartElement("name"); /* <root> <reference> <referencemanual <chapters> <chapter_00> <name> */
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(moduleModel.Name);
				xmlWriter.WriteEndElement(); /* <root> <reference> <referencemanual <chapters> <chapter_00> <name> </name> */
				xmlWriter.WriteStartElement("subchapters"); /* <root> <reference> <referencemanual <chapters> <chapter_00> <subchapters> */
				int subchapterID = 0;
				#region NPC Reference Manual Section
				if (moduleModel.IncludeNPCs)
				{
					xmlWriter.WriteStartElement("subchapter_" + subchapterID.ToString("D2")); // open <subchapters> <subchapter_**>
					xmlWriter.WriteStartElement("name"); // open <subchapters> <subchapter_*> <name>
					xmlWriter.WriteAttributeString("type", "string"); // <name type=string>
					xmlWriter.WriteString("NPCs");
					xmlWriter.WriteEndElement(); // close </name>
					xmlWriter.WriteStartElement("refpages"); // open <refpages>
					xmlWriter.WriteStartElement("id-0001"); // open <refpages> <a1>
					xmlWriter.WriteStartElement("blocks"); // open <refpages> <a1> <blocks>
					xmlWriter.WriteStartElement("id-0001"); // open <refpages> <a1> <blocks> <id-0001>
					WriteBlockFormatting(xmlWriter);
					xmlWriter.WriteStartElement("p");
					xmlWriter.WriteString("The following NPCs are able to be found in " + moduleModel.Name + ".");
					xmlWriter.WriteStartElement("linklist");
					FatNPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
					foreach (NPCModel npcModel in FatNPCList)
					{
						xmlWriter.WriteStartElement("link");
						xmlWriter.WriteAttributeString("class", "npc");
						xmlWriter.WriteAttributeString("recordname", WriteRecordNameNPC(npcModel));
						xmlWriter.WriteString(npcModel.NPCName);
						xmlWriter.WriteEndElement();
					}
					xmlWriter.WriteEndElement(); // close </linklist>
					xmlWriter.WriteEndElement(); // close </p>
					xmlWriter.WriteEndElement(); // close </text>
					xmlWriter.WriteEndElement(); // close </id-0001>
					xmlWriter.WriteEndElement(); // close </blocks>
					WriteListLink(xmlWriter);
					xmlWriter.WriteStartElement("name"); // open <name>
					xmlWriter.WriteAttributeString("type", "string"); // <name type=string>
					xmlWriter.WriteString(moduleModel.Name + " NPCs"); // <name type=string> * NPCs
					xmlWriter.WriteEndElement(); // close </name>
					xmlWriter.WriteEndElement(); // close </id-0001>
					FatNPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
					int npcID = 2;
					foreach (NPCModel npcModel in FatNPCList)
					{
						NPCController npcController = new NPCController();
						xmlWriter.WriteStartElement("id-" + npcID.ToString("D4")); // <open id-****>
						xmlWriter.WriteStartElement("blocks"); // <npc_name> <blocks>
						xmlWriter.WriteStartElement("id-0001"); // <npc_name> <blocks> <id-0001>
						WriteBlockFormatting(xmlWriter);
						if (!string.IsNullOrEmpty(npcModel.Description) && npcModel.Description.Length > 2)
						{
							xmlWriter.WriteRaw(npcController.GenerateFantasyGroundsDescriptionXML(npcModel.Description));
						}
						else
						{
							xmlWriter.WriteStartElement("p"); // <p>
							xmlWriter.WriteEndElement(); // </p>
						}
						if (npcModel.NPCImage.Length > 2)
						{
							xmlWriter.WriteStartElement("link");  // <link>
							xmlWriter.WriteAttributeString("class", "imagewindow"); // <link class="imagewindow">
							xmlWriter.WriteAttributeString("recordname", WriteImageXML(npcModel)); // <link class="imagewindow" recordname="image.*">
							xmlWriter.WriteStartElement("b"); // <b>
							xmlWriter.WriteString("Image:");
							xmlWriter.WriteEndElement(); // </b>
							xmlWriter.WriteEndElement(); // </link>
							xmlWriter.WriteString(npcModel.NPCName);
						}
						xmlWriter.WriteStartElement("link");  // <link>
						xmlWriter.WriteAttributeString("class", "npc"); // <link class="npc">
						xmlWriter.WriteAttributeString("recordname", WriteRecordNameNPC(npcModel)); // <link class="npc" recordname="reference.npcdata.*">
						xmlWriter.WriteStartElement("b"); // <b>
						xmlWriter.WriteString("NPC:");
						xmlWriter.WriteEndElement(); // </b>
						xmlWriter.WriteEndElement(); // </link>
						xmlWriter.WriteString(npcModel.NPCName);
						xmlWriter.WriteEndElement(); // </text>
						xmlWriter.WriteEndElement(); // </id-0001>
						xmlWriter.WriteEndElement(); // </blocks>
						WriteListLink(xmlWriter);
						xmlWriter.WriteStartElement("name"); // open <name>
						xmlWriter.WriteAttributeString("type", "string"); // <name type=string>
						xmlWriter.WriteString(npcModel.NPCName); // <name type=string> NPC Name
						xmlWriter.WriteEndElement(); // close </name>
						xmlWriter.WriteEndElement(); // </id-****>
						npcID = ++npcID;
					}
					xmlWriter.WriteEndElement(); // close </refpages>
					xmlWriter.WriteEndElement(); // close </subchapter_**>
					subchapterID = ++subchapterID;
				}
				#endregion
				#region Spell Reference Manual Section
				if (moduleModel.IncludeSpells)
				{
					xmlWriter.WriteStartElement("subchapter_" + subchapterID.ToString("D2")); // open <subchapters> <subchapter_**>
					xmlWriter.WriteStartElement("name"); // open <subchapters> <subchapter_*> <name>
					xmlWriter.WriteAttributeString("type", "string"); // <name type=string>
					xmlWriter.WriteString("Spells");
					xmlWriter.WriteEndElement(); // close </name>
					xmlWriter.WriteStartElement("refpages"); // open <refpages>
					xmlWriter.WriteStartElement("id-0001"); // open <refpages> <a1>
					xmlWriter.WriteStartElement("blocks"); // open <refpages> <a1> <blocks>
					xmlWriter.WriteStartElement("id-0001"); // open <refpages> <a1> <blocks> <id-0001>
					WriteBlockFormatting(xmlWriter);
					xmlWriter.WriteStartElement("p");
					xmlWriter.WriteString("The following Spells are able to be found in " + moduleModel.Name + ".");
					xmlWriter.WriteStartElement("linklist");
					FatSpellList.Sort((spellOne, spellTwo) => spellOne.SpellName.CompareTo(spellTwo.SpellName));
					foreach (SpellModel spellModel in FatSpellList)
					{
						xmlWriter.WriteStartElement("link");
						xmlWriter.WriteAttributeString("class", "power");
						xmlWriter.WriteAttributeString("recordname", WriteRecordNameSpell(spellModel));
						xmlWriter.WriteString(spellModel.SpellName);
						xmlWriter.WriteEndElement();
					}
					xmlWriter.WriteEndElement(); // close </linklist>
					xmlWriter.WriteEndElement(); // close </p>
					xmlWriter.WriteEndElement(); // close </text>
					xmlWriter.WriteEndElement(); // close </id-0001>
					xmlWriter.WriteEndElement(); // close </blocks>
					WriteListLink(xmlWriter);
					xmlWriter.WriteStartElement("name"); // open <name>
					xmlWriter.WriteAttributeString("type", "string"); // <name type=string>
					xmlWriter.WriteString(moduleModel.Name + " Spells"); // <name type=string> * Spells
					xmlWriter.WriteEndElement(); // close </name>
					xmlWriter.WriteEndElement(); // close </id-0001>
					FatSpellList.Sort((spellOne, spellTwo) => spellOne.SpellName.CompareTo(spellTwo.SpellName));
					int spellID = 2;
					foreach (SpellModel spellModel in FatSpellList)	
					{
						NPCController npcController = new NPCController();
						xmlWriter.WriteStartElement("id-" + spellID.ToString("D4")); // <open id-****>
						xmlWriter.WriteStartElement("blocks"); // <npc_name> <blocks>
						xmlWriter.WriteStartElement("id-0001"); // <npc_name> <blocks> <id-0001>
						WriteBlockFormatting(xmlWriter);
						xmlWriter.WriteRaw("<p><h>"); // <p><h>
						xmlWriter.WriteString(spellModel.SpellName);
						xmlWriter.WriteRaw("</h></p><p><i>"); // </h></p><p><i>
						if (spellModel.SpellLevel.GetDescription().Equals("cantrip"))
						{
							xmlWriter.WriteString(spellModel.SpellSchool + " cantrip");
						}
						else
						{
							xmlWriter.WriteString(spellModel.SpellLevel.GetDescription() + "-level " + spellModel.SpellSchool);
							if (spellModel.IsRitual.Equals(1))
							{
								xmlWriter.WriteString(" (ritual)");
							}
						}
						xmlWriter.WriteRaw("</i></p><p><b>");
						xmlWriter.WriteString("Casting Time:");
						xmlWriter.WriteRaw("</b>");
						xmlWriter.WriteString(" " + spellModel.CastingTime + " " + spellModel.CastingType.GetDescription());
						if (spellModel.CastingTime > 1)
						{
							xmlWriter.WriteString("s");
						}
						xmlWriter.WriteRaw("</p><p><b>");
						xmlWriter.WriteString("Range:");
						xmlWriter.WriteRaw("</b>");
						xmlWriter.WriteString(" " + spellModel.RangeDescription);
						xmlWriter.WriteRaw("</p><p><b>");
						xmlWriter.WriteString("Components:");
						xmlWriter.WriteRaw("</b>");
						xmlWriter.WriteString(" " + spellModel.ComponentDescription);
						xmlWriter.WriteRaw("</p><p><b>");
						xmlWriter.WriteString("Duration:");
						xmlWriter.WriteRaw("</b>");
						xmlWriter.WriteString(" " + spellModel.DurationText);
						xmlWriter.WriteRaw("</p>");
						xmlWriter.WriteRaw(npcController.GenerateFantasyGroundsDescriptionXML(spellModel.Description) + "<p>");
						xmlWriter.WriteStartElement("link");  // <link>
						xmlWriter.WriteAttributeString("class", "power"); // <link class="power">
						xmlWriter.WriteAttributeString("recordname", WriteRecordNameSpell(spellModel)); // <link class="power" recordname="reference.spelldata.*">
						xmlWriter.WriteRaw("<b>"); // <b>
						xmlWriter.WriteString("Spell:");
						xmlWriter.WriteRaw("</b>"); // </b>
						xmlWriter.WriteEndElement(); // </link>
						xmlWriter.WriteString(spellModel.SpellName);
						xmlWriter.WriteRaw("</p>"); // </p>
						xmlWriter.WriteEndElement(); // </text>
						xmlWriter.WriteEndElement(); // </id-0001>
						xmlWriter.WriteEndElement(); // </blocks>
						WriteListLink(xmlWriter);
						xmlWriter.WriteStartElement("name"); // open <name>
						xmlWriter.WriteAttributeString("type", "string"); // <name type=string>
						xmlWriter.WriteString(spellModel.SpellName); // <name type=string> NPC Name
						xmlWriter.WriteEndElement(); // close </name>
						xmlWriter.WriteEndElement(); // </id-****>
						spellID = ++spellID;
					}
					xmlWriter.WriteEndElement(); // </subchapter_**>
					subchapterID = ++subchapterID;
				}
				#endregion
				xmlWriter.WriteEndElement(); // close </refpages>
				xmlWriter.WriteEndElement(); // close </chapter_**>
				xmlWriter.WriteEndElement(); // close </chapters>
				xmlWriter.WriteEndElement(); // Close </referencemanual>
                #endregion
                

                #endregion

                #region Library
                // For the Blank DB XML unit test, need to check if any NPCs exist
                if (moduleModel.Categories != null && moduleModel.Categories.Count > 0)
				{
					xmlWriter.WriteStartElement("library");  /* <library> */
				xmlWriter.WriteStartElement(WriteLibraryNameLowerCase(moduleModel)); /* <library> <libname> */
					xmlWriter.WriteStartElement("name"); /* <library> <libname> <name> */
					xmlWriter.WriteAttributeString("type", "string");          
					xmlWriter.WriteString(moduleModel.Name + " Reference Library");   
					xmlWriter.WriteEndElement(); /* <library> <libname> <name> </name> */
					xmlWriter.WriteStartElement("categoryname"); /* <library> <libname> <categoryname> */
					xmlWriter.WriteAttributeString("type", "string");     
					xmlWriter.WriteString(moduleModel.Category);             
					xmlWriter.WriteEndElement();  /* <library> <libname> <categoryname> </categoryname> */
					xmlWriter.WriteStartElement("entries");
					int libraryID = 1;
					if (moduleModel.IncludeImages)
                    {
						xmlWriter.WriteStartElement("r" + libraryID.ToString("D2") + "images"); /* <library> <libname> <r**images> */
						xmlWriter.WriteStartElement("librarylink"); /* <library> <libname> <r**images> <librarylink> */
						xmlWriter.WriteAttributeString("type", "windowreference");
						xmlWriter.WriteStartElement("class"); /* <library> <libname> <r**images> <librarylink> <class> */
						xmlWriter.WriteString("reference_colindex");
						xmlWriter.WriteEndElement();  /* <library> <libname> <r**images> <librarylink> <class> </class> */
						xmlWriter.WriteStartElement("recordname"); /* <library> <libname> <r**images> <librarylink> <recordname> */
						xmlWriter.WriteString("reference.imagelists.bycategory@" + moduleModel.Name);
						xmlWriter.WriteEndElement(); /* <library> <libname> <r**images> <librarylink> <recordname> </recordname> */
						xmlWriter.WriteEndElement(); /* <library> <libname> <r**images> <librarylink> </librarylink> */
						xmlWriter.WriteStartElement("name"); /* <library> <libname> <r**images> <name> */
						xmlWriter.WriteAttributeString("type", "string");
						xmlWriter.WriteString("Images");
						xmlWriter.WriteEndElement(); /* <library> <libname> <r**images> <name> </name> */
						xmlWriter.WriteEndElement(); /* <library> <libname> <r**images> </r**images> */
						libraryID = ++libraryID;
					}
					if (moduleModel.IncludeNPCs)
                    {
						if (moduleModel.Categories[0].NPCModels.Count > 0)
						{
							xmlWriter.WriteStartElement("r" + libraryID.ToString("D2") + "monsters"); /* <library> <libname> <r**monsters> */
							xmlWriter.WriteStartElement("librarylink"); /* <library> <libname> <r**monsters> <librarylink> */
							xmlWriter.WriteAttributeString("type", "windowreference");
							xmlWriter.WriteStartElement("class"); /* <library> <libname> <r**monsters> <librarylink> <class> */
							xmlWriter.WriteString("referenceindex");
							xmlWriter.WriteEndElement(); /* <library> <libname> <r**monsters> <librarylink> <class> </class> */
							xmlWriter.WriteStartElement("recordname"); /* <library> <libname> <r**monsters> <librarylink> <recordname> */
							xmlWriter.WriteString("reference.npclists.npcs@" + moduleModel.Name);
							xmlWriter.WriteEndElement(); /* <library> <libname> <r**monsters> <librarylink> <recordname> </recordname> */
							xmlWriter.WriteEndElement(); /* <library> <libname> <r**monsters> <librarylink> </librarylink> */
							xmlWriter.WriteStartElement("name"); /* <library> <libname> <r**monsters> <name> */
							xmlWriter.WriteAttributeString("type", "string");
							xmlWriter.WriteString("NPCs");
							xmlWriter.WriteEndElement(); /* <library> <libname> <r**monsters> <name> </name> */
							xmlWriter.WriteEndElement(); /* <library> <libname> <r**monsters> </r**monsters> */
							libraryID = ++libraryID;
						}
					}
					if (moduleModel.IncludeSpells)
                    {
						if (moduleModel.Categories[0].SpellModels.Count > 0)
						{
							xmlWriter.WriteStartElement("r" + libraryID.ToString("D2") + "spells");
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
							libraryID = ++libraryID;
						}
					}
					if (moduleModel.IncludeTables)
					{
						if (moduleModel.Categories[0].TableModels.Count > 0)
						{
							xmlWriter.WriteStartElement("r" + libraryID.ToString("D2") + "tables");
							xmlWriter.WriteStartElement("librarylink");
							xmlWriter.WriteAttributeString("type", "windowreference");
							xmlWriter.WriteStartElement("class");
							xmlWriter.WriteString("referenceindex");
							xmlWriter.WriteEndElement();
							xmlWriter.WriteStartElement("recordname");
							xmlWriter.WriteString("..");
							xmlWriter.WriteEndElement();
							xmlWriter.WriteEndElement();
							xmlWriter.WriteStartElement("name");
							xmlWriter.WriteAttributeString("type", "string");
							xmlWriter.WriteString("Tables");
							xmlWriter.WriteEndElement();
							xmlWriter.WriteEndElement();
							libraryID = ++libraryID;
						}
					}
					xmlWriter.WriteStartElement("r" + libraryID.ToString("D2") + "referencemanual");
					xmlWriter.WriteStartElement("librarylink");
					xmlWriter.WriteAttributeString("type", "windowreference");
					xmlWriter.WriteStartElement("class");
					xmlWriter.WriteString("reference_manual");
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("recordname");
					xmlWriter.WriteString("reference.referencemanual@" + moduleModel.Name);
					xmlWriter.WriteEndElement(); // close </recordname>
					xmlWriter.WriteEndElement(); // close </librarylink>
					xmlWriter.WriteStartElement("name");
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString("Reference Manual");
					xmlWriter.WriteEndElement(); // close </name>
					xmlWriter.WriteEndElement(); // close </r05referencemanual>
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
		#region Reference Manual XML
		static private void WriteBlockFormatting(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("align"); /* <align> */
			xmlWriter.WriteAttributeString("type", "string"); 
			xmlWriter.WriteString("center"); 
			xmlWriter.WriteEndElement(); /* <align> </align> */
			xmlWriter.WriteStartElement("blocktype"); /* <blocktype> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("singletext");
			xmlWriter.WriteEndElement(); /* <blocktype> </blocktype> */
			xmlWriter.WriteStartElement("text"); /* <text> */
			xmlWriter.WriteAttributeString("type", "formattedtext");
		}
		static private string WriteRecordNameNPC(NPCModel npcModel)
		{
			return "reference.npcdata." + NPCExporter.NPCNameToXMLFormat(npcModel);
		}
		static private string WriteRecordNameSpell(SpellModel spellModel)
		{
			return "reference.spelldata." + SpellExporter.SpellNameToXMLFormat(spellModel);
		}
		static private string WriteImageXML(NPCModel npcModel)
		{
			return Path.GetFileNameWithoutExtension(npcModel.NPCImage).Replace(" ", "").Replace("-", "");
		}
		static private void WriteListLink(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("listlink"); /* <listlink> */
			xmlWriter.WriteAttributeString("type", "windowreference");
			xmlWriter.WriteStartElement("class"); /* <listlink> <class> */
			xmlWriter.WriteString("reference_manualtextwide");
			xmlWriter.WriteEndElement(); /* <listlink> <class> </class> */
			xmlWriter.WriteStartElement("recordname"); /* <listlink> <recordname> */
			xmlWriter.WriteString("..");
			xmlWriter.WriteEndElement(); /* <listlink> <recordname> </recordname> */
			xmlWriter.WriteEndElement(); /* <listlink> </listlink> */
		}
		static private void WriteLineBreak(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("br");
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
			xmlWriter.WriteStartElement("locked"); /* <locked> */
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteString(lockedRecords.ToString());
			xmlWriter.WriteEndElement(); /* <locked> </locked> */
		}
		static private void WriteIDLinkList(XmlWriter xmlWriter, ModuleModel moduleModel, string id, string listId, string listDescription)
		{
			xmlWriter.WriteStartElement(id); /* <id> */
			xmlWriter.WriteStartElement("listlink"); /* <id> <listlink> */
			xmlWriter.WriteAttributeString("type", "windowreference");
			xmlWriter.WriteStartElement("class"); /* <id> <listlink> <class> */
			xmlWriter.WriteString("reference_colindex");
			xmlWriter.WriteEndElement(); /* <id> <listlink> <class> </class> */
			xmlWriter.WriteStartElement("recordname"); /* <id> <listlink> <recordname> */
			xmlWriter.WriteString(listId);				
			xmlWriter.WriteEndElement(); /* <id> <listlink> <recordname> </recordname> */
			xmlWriter.WriteEndElement(); /* <id> <listlink> </listlink> */
			xmlWriter.WriteStartElement("name"); /* <id> <name> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(listDescription);
			xmlWriter.WriteEndElement(); /* <id> <name> </name> */
			xmlWriter.WriteEndElement(); /* <id> </id> */
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
				ConformanceLevel = ConformanceLevel.Auto,
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