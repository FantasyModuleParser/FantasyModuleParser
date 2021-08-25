﻿using FantasyModuleParser.Equipment.Enums;
using FantasyModuleParser.Equipment.Models;
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
			List<EquipmentModel> FatEquipmentList = CommonMethods.GenerateFatEquipmentList(moduleModel);
			FatNPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
			FatSpellList.Sort((spellOne, spellTwo) => spellOne.SpellName.CompareTo(spellTwo.SpellName));
			FatTableList.Sort((tableOne, tableTwo) => tableOne.Name.CompareTo(tableTwo.Name));
			FatEquipmentList.Sort((equipOne, equipTwo) => equipOne.Name.CompareTo(equipTwo.Name));
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
						string NPCTokenFileName = Path.Combine(settingsService.Load().FGModuleFolderLocation, moduleModel.ModFilename, "tokens", Filename);
						string NPCTokenDirectory = Path.Combine(settingsService.Load().FGModuleFolderLocation, moduleModel.ModFilename, "tokens");
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
						string Filename = Path.GetFileName(npcModel.NPCImage).Replace("-", "").Replace(" ", "").Replace(",", "");
						string NPCImageFileName = Path.Combine(settingsService.Load().FGModuleFolderLocation, moduleModel.ModFilename, "images", Filename);
						string NPCImageDirectory = Path.Combine(settingsService.Load().FGModuleFolderLocation, moduleModel.ModFilename, "images");
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
						if (npcModel.NPCImage.StartsWith("file:///"))
						{
							npcModel.NPCImage = npcModel.NPCImage.Remove(0, 8);
						}
						File.Copy(npcModel.NPCImage, NPCImageFileName);
					}
					#endregion
				}
			}

			using (StringWriter sw = new StringWriterWithEncoding(Encoding.UTF8))
			using (XmlWriter xmlWriter = XmlWriter.Create(sw, GetXmlWriterSettings()))
            {
               
            }
        }

        #region NPC Methods for Reference Data XML
        
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
        static private string WriteRecordNameEquipment(EquipmentModel equipmentModel)
        {
            return "reference.equipmentdata." + EquipmentExporter.EquipmentNameToXML(equipmentModel);
        }
        static private string WriteImageXML(NPCModel npcModel)
		{
			return "image." + Path.GetFileNameWithoutExtension(npcModel.NPCImage).Replace(" ", "").Replace("-", "");
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