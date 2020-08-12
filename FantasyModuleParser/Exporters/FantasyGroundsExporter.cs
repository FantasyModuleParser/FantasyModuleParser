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
using System.Reflection;
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

		public string DatabaseXML(ModuleModel moduleModel)
        {
			string xmlRecord;
			if (moduleModel.IsGMOnly)
            {
				return xmlRecord = "db.xml";
            }
			else
            {
				return xmlRecord = "client.xml";
            }
        }

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
			Directory.CreateDirectory(moduleFolderPath);

			// Save Thumbnail to Module Folder
			if (moduleModel.ThumbnailPath != null && moduleModel.ThumbnailPath.Length != 0)
            {
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
			if (File.Exists(@Path.Combine(moduleModel.ModulePath, moduleModel.Name + ".mod")))
			{
				File.Delete(@Path.Combine(moduleModel.ModulePath, moduleModel.Name + ".mod"));
			}
			ZipFile.CreateFromDirectory(moduleFolderPath, Path.Combine(moduleModel.ModulePath, moduleModel.Name + ".mod"));
		}

		/// <summary>
		/// Generates a List of all NPCs across all Categories in one List<NPCModel> object.  Used for Reference Manual material
		/// </summary>
		/// <param name="moduleModel"></param>
		/// <returns></returns>
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

		private string NewThumbnailFileName(ModuleModel moduleModel)
        {
			string ThumbnailFilename = Path.Combine(moduleModel.ModulePath, moduleModel.Name, "thumbnail.png");
			return ThumbnailFilename;

		}
		private void SaveThumbnailImage(ModuleModel moduleModel)
        {
			if (File.Exists(@Path.Combine(NewThumbnailFileName(moduleModel))))
			{
				File.Delete(@Path.Combine(NewThumbnailFileName(moduleModel)));
				File.Copy(moduleModel.ThumbnailPath, NewThumbnailFileName(moduleModel));
			}
			else
			{
				File.Copy(moduleModel.ThumbnailPath, NewThumbnailFileName(moduleModel));
			}
		}

		public string GenerateDBXmlFile(ModuleModel moduleModel)
		{
			List<NPCModel> FatNPCList = GenerateFatNPCList(moduleModel);
			FatNPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));

			foreach (NPCModel npcModel in FatNPCList)
            {
				if (npcModel.NPCToken != null && npcModel.NPCToken.Length > 0)
                {
					string Filename = NPCNameToXMLFormat(npcModel) + "_token.png";
					string NPCTokenFileName = Path.Combine(moduleModel.ModulePath, moduleModel.Name, "tokens", Filename);
					string NPCTokenDirectory = Path.Combine(moduleModel.ModulePath, moduleModel.Name, "tokens");
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

			foreach (NPCModel npcModel in FatNPCList)
			{
				if (npcModel.NPCImage != null && npcModel.NPCImage.Length > 0)
				{
					string Filename = NPCNameToXMLFormat(npcModel) + ".jpg";
					string NPCImageFileName = Path.Combine(moduleModel.ModulePath, moduleModel.Name, "images", Filename);
					string NPCImageDirectory = Path.Combine(moduleModel.ModulePath, moduleModel.Name, "images");
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

			using (StringWriter sw = new StringWriterWithEncoding(Encoding.UTF8))
			using (XmlWriter xmlWriter = XmlWriter.Create(sw, GetXmlWriterSettings()))
			{
				xmlWriter.WriteStartDocument();

				xmlWriter.WriteComment("Generated by Fantasy Module Parser");
				xmlWriter.WriteComment("Written by Theodore Story, Darkpool, and Dusk (c) 2020");

				xmlWriter.WriteStartElement("root");
				xmlWriter.WriteAttributeString("version", "3.1");
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
                        if (npcModel.NPCImage != null && npcModel.NPCImage.Length > 0)
                        {
                            xmlWriter.WriteStartElement(NPCNameToXMLFormat(npcModel));
                            xmlWriter.WriteStartElement("image");
                            xmlWriter.WriteAttributeString("type", "image");
                            xmlWriter.WriteStartElement("bitmap");
                            xmlWriter.WriteString("images\\" + NPCNameToXMLFormat(npcModel) + ".jpg");
                            xmlWriter.WriteEndElement();
							WriteLocked(xmlWriter, npcModel);
							WriteName(xmlWriter, npcModel);
                            xmlWriter.WriteStartElement("nonid_name");
                            xmlWriter.WriteAttributeString("type", "string");
                            xmlWriter.WriteString(npcModel.NPCName);
                            xmlWriter.WriteEndElement();
                            xmlWriter.WriteEndElement();
							xmlWriter.WriteEndElement();
                        }
                    }
					xmlWriter.WriteEndElement();
                }
				xmlWriter.WriteEndElement();
                #endregion
                #region Reference Section
                xmlWriter.WriteStartElement("reference");
				if (moduleModel.IsLockedRecords)
                {
					xmlWriter.WriteAttributeString("static", "true");
				}
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
					WriteLocked(xmlWriter, npcModel);
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
                #region Spell Data
                //	xmlWriter.WriteStartElement("spelldata");           
                //	xmlWriter.WriteString(" ");                      
                //	xmlWriter.WriteEndElement();
                #endregion
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
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
                #endregion
                #region NPC Lists
                xmlWriter.WriteStartElement("npclists");                    // Open <npclists>
				xmlWriter.WriteStartElement("npcs");                        // Open <npcs>
				xmlWriter.WriteStartElement("name");                        // Open <name>
				xmlWriter.WriteAttributeString("type", "string");           // Add type="string"
				xmlWriter.WriteString("NPCs");                              // Type NPCs
				xmlWriter.WriteEndElement();                                // Close </name>
				xmlWriter.WriteStartElement("index");                       // Open <index>
				//TODO:  Needs a better descriptive method name
				WriteIDLinkList(xmlWriter, moduleModel, "id01", "reference.npclists.byletter", "NPCs - Alphabetical Index");
				WriteIDLinkList(xmlWriter, moduleModel, "id02", "reference.npclists.bylevel", "NPCs - Challenge Rating Index");
				WriteIDLinkList(xmlWriter, moduleModel, "id03", "reference.npclists.bytype", "NPCs - Class Index");
				xmlWriter.WriteEndElement();                                // Close </index>
				xmlWriter.WriteEndElement();                                // Close </npcs>
				xmlWriter.WriteStartElement("byletter");                    // Open <byletter>
				xmlWriter.WriteStartElement("description");                 // Open <description>
				xmlWriter.WriteAttributeString("type", "string");           // Add type="string"
				xmlWriter.WriteString("NPCs");                      // Write NPCs
				xmlWriter.WriteEndElement();                                // Close </description> 				
				xmlWriter.WriteStartElement("groups");                      // Open <groups>
				CreateReferenceByFirstLetter(xmlWriter, moduleModel, FatNPCList);        // Use CreateReferenceByFirstLetter Method
				xmlWriter.WriteEndElement();                                // Close </groups>
				xmlWriter.WriteEndElement();                                // Close </byletter>
				xmlWriter.WriteStartElement("bylevel");                    // Open <bycr>
				xmlWriter.WriteStartElement("description");                 // Open <description>
				xmlWriter.WriteAttributeString("type", "string");           // Add type="string"
				xmlWriter.WriteString("NPCs");                      // Write NPCs
				xmlWriter.WriteEndElement();                                // Close </description> 
				xmlWriter.WriteStartElement("groups");                      // Open <groups>
				CreateReferenceByCR(xmlWriter, moduleModel, FatNPCList);                    // Use CreateReferenceByCR Method
				xmlWriter.WriteEndElement();                                // Close </groups>
				xmlWriter.WriteEndElement();                                // Close </bycr>
				xmlWriter.WriteStartElement("bytype");                    // Open <bytype>
				xmlWriter.WriteStartElement("description");                 // Open <description>
				xmlWriter.WriteAttributeString("type", "string");           // Add type="string"
				xmlWriter.WriteString("NPCs");                        // Write NPCs
				xmlWriter.WriteEndElement();                                // Close </description> 
				xmlWriter.WriteStartElement("groups");                      // Open <groups>
				CreateReferenceByType(xmlWriter, moduleModel, FatNPCList);                 // Use CreateReferenceByType Method
				xmlWriter.WriteEndElement();                                // Close </groups>
				xmlWriter.WriteEndElement();                                // Close </bytype>
				xmlWriter.WriteEndElement();                                // Close </npclists>
                #endregion
                #region Tables
                //	xmlWriter.WriteStartElement("tables");      
                //	xmlWriter.WriteString(" ");                 
                //	xmlWriter.WriteEndElement();                
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
                if (moduleModel.Categories != null && moduleModel.Categories.Count > 0 && moduleModel.Categories[0].NPCModels.Count > 0)
				{
					xmlWriter.WriteStartElement("library");                      // Open <library>
					xmlWriter.WriteStartElement(WriteLibraryNameLowerCase(moduleModel));  // Open <Module Name>

					xmlWriter.WriteStartElement("name");                         // Open <name>
					xmlWriter.WriteAttributeString("type", "string");            // Add type="string"
					xmlWriter.WriteString(WriteLibraryNameUpperCase(moduleModel) + " Reference Library");        // Write "Library Name Reference Library"
					xmlWriter.WriteEndElement();								// Close </name>

					xmlWriter.WriteStartElement("categoryname");                 // Open <categoryname>
					xmlWriter.WriteAttributeString("type", "string");            // Add type="string"
					xmlWriter.WriteString(moduleModel.Category);                 // Write "Category Name"
					xmlWriter.WriteEndElement();                                 // Close </categoryname>
					                                
					xmlWriter.WriteStartElement("entries");                      // Open <entries>
					xmlWriter.WriteStartElement("r01monsters");                  // Open <r01monsters>
					xmlWriter.WriteStartElement("librarylink");                  // Open <librarylink>
					xmlWriter.WriteAttributeString("type", "windowreference");   // Add type="windowreference"
					xmlWriter.WriteStartElement("class");                        // Open <class>
					xmlWriter.WriteString("referenceindex");					// Write "referenceindexs"
					xmlWriter.WriteEndElement();                                 // Close </class>
					xmlWriter.WriteStartElement("recordname");                   // Open <recordname>
					xmlWriter.WriteString("reference.npclists.npcs");            // Write "reference.npclists.npcs"
					xmlWriter.WriteEndElement();                                 // Close </recordname>
					xmlWriter.WriteEndElement();                                 // Close </librarylink>
					xmlWriter.WriteStartElement("name");                         // Open <name>
					xmlWriter.WriteAttributeString("type", "string");            // Add type=string
					xmlWriter.WriteString("NPCs");                               // Write "NPCs"
					xmlWriter.WriteEndElement();                                 // Close </name>
					xmlWriter.WriteEndElement();                                 // Close </r01monsters>
					xmlWriter.WriteEndElement();                                 // Close </entries>
					xmlWriter.WriteEndElement();                                 // Close </Module Name>
					xmlWriter.WriteEndElement();                                 // Close </library>
				}
				#endregion
				xmlWriter.WriteEndElement(); // Closes </root>
				xmlWriter.WriteEndDocument();
				xmlWriter.Close();
				return sw.ToString();
			}
		}
		#region Common methods for Reference Manual XML
		private void WriteIDLinkList(XmlWriter xmlWriter, ModuleModel moduleModel, string id, string listId, string listDescription)
		{
			xmlWriter.WriteStartElement(id);                        // Open <id>
			xmlWriter.WriteStartElement("listlink");                    // Open <listlink>
			xmlWriter.WriteAttributeString("type", "windowreference");  // Add type="windowreference"
			xmlWriter.WriteStartElement("class");                       // Open <class>
			xmlWriter.WriteString("reference_colindex");                // Write "reference_colindex"
			xmlWriter.WriteEndElement();                                // Close </class>
			xmlWriter.WriteStartElement("recordname");                  // Open <recordname>
			if (moduleModel.IsLockedRecords == true)
            {
				xmlWriter.WriteString(listId + "@" + moduleModel.Name);             // Write "reference.npclist.bytype"
			}
			else
            {
				xmlWriter.WriteString(listId);

			}
			xmlWriter.WriteEndElement();                                // Close </recordname>
			xmlWriter.WriteEndElement();                                // Close </listlink>
			xmlWriter.WriteStartElement("name");                        // Open <name>
			xmlWriter.WriteAttributeString("type", "string");           // Add type="string"
			xmlWriter.WriteString(listDescription);						// Write "NPCs - Class Index"
			xmlWriter.WriteEndElement();                                // Close </name>
			xmlWriter.WriteEndElement();                                // Close </id>
		}
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
				if (moduleModel.IsLockedRecords)
                {
					xmlWriter.WriteString("reference.npcdata." + NPCNameToXMLFormat(npc) + "@" + moduleModel.Name);
				}
				else 
				{
					xmlWriter.WriteString("reference.npcdata." + NPCNameToXMLFormat(npc));
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
		private string WriteLibraryNameLowerCase(ModuleModel moduleModel)
		{
			string libname = moduleModel.Name.ToLower();
			return libname.Replace(" ", "");
		}
		private string WriteLibraryNameUpperCase(ModuleModel moduleModel)
		{
			string libname = moduleModel.Name;
			return libname.Replace(" ", "");
		}
		private string CategoryNameToXML(CategoryModel categoryModel)
        {
			string categoryName = categoryModel.Name;
			return categoryName.Replace(" ", "").Replace(",", "").Replace("-", "_").Replace("'", "_").ToLower();
        }
		public void CreateReferenceByFirstLetter(XmlWriter xmlWriter, ModuleModel moduleModel, List<NPCModel> NPCList)
		{
			NPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
			var AlphabetList = NPCList.GroupBy(x => x.NPCName.ToUpper()[0]).Select(x => x.ToList()).ToList();
			foreach (List<NPCModel> npcList in AlphabetList)
			{
				// Assumption here is GroupBy will correctly group so that each List<NPCModel> is populated with one NPCModel object
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
				// Assumption here is GroupBy will correctly group so that each List<NPCModel> is populated with one NPCModel object
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
				// Assumption here is GroupBy will correctly group so that each List<NPCModel> is populated with one NPCModel object
				string actualType = npcList[0].NPCType + "";
				ProcessNPCListByType(xmlWriter, moduleModel, actualType, npcList);
			}
		}
		private void ProcessNPCListByType(XmlWriter xmlWriter, ModuleModel moduleModel, string actualType, List<NPCModel> NPCList)
		{
			xmlWriter.WriteStartElement("type_" + actualType);
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
			return name.Replace(" ", "_").Replace(",", "");
		}
        #endregion

        public void SortNPCListByCategory(XmlWriter xmlWriter, NPCModel npcModel, ModuleModel moduleModel, CategoryModel categoryModel, List<NPCModel> NPCList)
        {
            NPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
            var AlphabetList = NPCList.GroupBy(x => x.NPCName.ToUpper()[0]).Select(x => x.ToList()).ToList();
            foreach (List<NPCModel> npcList in AlphabetList)
            {
                string actualLetter = npcList[0].NPCName[0] + "";
                ProcessNPCListByCategoryLetter(xmlWriter, npcModel, moduleModel, actualLetter, npcList);
            }
        }

        private void ProcessNPCListByCategoryLetter(XmlWriter xmlWriter, NPCModel npcModel, ModuleModel moduleModel, string actualLetter, List<NPCModel> NPCList)
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
        #region NPC Methods
        private void WriteLocked(XmlWriter xmlWriter, NPCModel npcModel)
		{
			ModuleModel moduleModel = new ModuleModel();
			string lockedRecords = "0";
			if (moduleModel.IsLockedRecords)
			{
				lockedRecords = "1";
			}

			xmlWriter.WriteStartElement("locked"); // Open <locked>
			xmlWriter.WriteAttributeString("type", "number"); // Add type=number
			xmlWriter.WriteString(lockedRecords.ToString()); // Value should be either a 0 or 1
															 // 0 = unlocked, 1 = locked
			xmlWriter.WriteEndElement(); // Close </locked>
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
				ChaModifier = "";

			if (npcModel.AttributeCon >= 10)
				ConModifier = "+";
			else
				ConModifier = "";

			if (npcModel.AttributeDex >= 10)
				DexModifier = "+";
			else
				DexModifier = "";

			if (npcModel.AttributeInt >= 10)
				IntModifier = "+";
			else
				IntModifier = "";

			if (npcModel.AttributeStr >= 10)
				StrModifier = "+";
			else
				StrModifier = "";

			if (npcModel.AttributeWis >= 10)
				WisModifier = "+";
			else
				WisModifier = "";

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
			string[] acArray = npcModel.AC.Split('(');
			string acValue = acArray[0].Trim(); // Removes any whitespace
			string acDescription = acArray.Length >= 2 ? "(" + acArray[1] : "";

			xmlWriter.WriteStartElement("ac"); // Open <ac>
			xmlWriter.WriteAttributeString("type", "number"); // Add type=number
			xmlWriter.WriteValue(acValue); // Add AC value
			xmlWriter.WriteEndElement(); // Close </ac>
			xmlWriter.WriteStartElement("actext"); // Open <actext>
			xmlWriter.WriteAttributeString("type", "string"); // Add type=string
			xmlWriter.WriteValue(acDescription); // Add AC Text string
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
				actionID = ++actionID;
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
			if (npcModel.ConditionImmunityModelList != null)
			{
				foreach (SelectableActionModel condition in npcModel.ConditionImmunityModelList)
				{
					if (condition.Selected)
						stringBuilder.Append(condition.ActionDescription.ToLower()).Append(", ");
				}
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
			if (npcModel.DamageImmunityModelList != null)
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

			if (npcModel.SpecialWeaponImmunityModelList != null)
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
			if (npcModel.DamageResistanceModelList != null)
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
			if (npcModel.SpecialWeaponResistanceModelList != null)
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
			if (weaponDamageResistanceString.StartsWith(";", true, CultureInfo.CurrentCulture))
				weaponDamageResistanceString = weaponDamageResistanceString.Remove(0, 1);
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

			if (npcModel.DamageVulnerabilityModelList != null)
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
			if (npcModel.HP == null)
			{
				npcModel.HP = "0 (0)";
			}
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
			if (npcModel.LairActions != null)
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
			StringBuilder stringBuilderOption = new StringBuilder();

			foreach (LanguageModel languageModel in npcModel.StandardLanguages)
			{
				if (languageModel.Selected == true)
					stringBuilder.Append(languageModel.Language).Append(", ");
			}
			foreach (LanguageModel languageModel in npcModel.ExoticLanguages)
			{
				if (languageModel.Selected == true)
					stringBuilder.Append(languageModel.Language).Append(", ");
			}
			foreach (LanguageModel languageModel in npcModel.MonstrousLanguages)
			{
				if (languageModel.Selected == true)
					stringBuilder.Append(languageModel.Language).Append(", ");
			}
			if (npcModel.UserLanguages != null && npcModel.UserLanguages.Count > 0)
			{
				foreach (LanguageModel languageModel in npcModel.UserLanguages)
				{
					if (languageModel.Selected == true)
						stringBuilder.Append(languageModel.Language).Append(", ");
				}
			}

			if (npcModel.Telepathy)
			{
				stringBuilder.Append("telepathy " + npcModel.TelepathyRange).Append(", ");
			}
			if (stringBuilder.Length >= 2)
				stringBuilder.Remove(stringBuilder.Length - 2, 2);


			if (npcModel.LanguageOptions == "No special conditions" || npcModel.LanguageOptions == null)
			{
				stringBuilderOption.Append(stringBuilder);
			}
			else if (npcModel.LanguageOptions == "Speaks no languages")
			{
				stringBuilderOption.Append("-");
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

			xmlWriter.WriteStartElement("languages"); // Open <languages>
			xmlWriter.WriteAttributeString("type", "string"); // Add type=string
			xmlWriter.WriteValue(stringBuilderOption.ToString());
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
				xmlWriter.WriteAttributeString("type", "string"); // Add type=string
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
			stringBuilder.Append(appendSenses("darkvision ", npcModel.Darkvision, " ft."));
			stringBuilder.Append(appendBlindSenses("blindsight ", npcModel.Blindsight, " ft."));
			stringBuilder.Append(appendSenses("tremorsense ", npcModel.Tremorsense, " ft."));
			stringBuilder.Append(appendSenses("truesight ", npcModel.Truesight, " ft."));
			stringBuilder.Append(appendSenses("passive perception ", npcModel.PassivePerception, ""));
			if (stringBuilder.Length >= 2)
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			string sensesString = stringBuilder.ToString().Trim();

			xmlWriter.WriteStartElement("senses"); // Open <senses>
			xmlWriter.WriteAttributeString("type", "string"); // Add type=string
			xmlWriter.WriteValue(sensesString);
			xmlWriter.WriteEndElement(); // Close </senses>
		}
		private string appendSenses(string senseName, int senseValue, string senseRange)
		{
			if (senseValue != 0)
			{
				string delimiter = ", ";
				return senseName + senseValue + senseRange + delimiter;
			}
			return "";
		}
		private string appendBlindSenses(string senseName, int senseValue, string senseRange)
		{
			NPCModel npcModel = new NPCModel();
			string delimiter = ", ";
			if (senseValue != 0 && npcModel.BlindBeyond == false)
				return senseName + senseValue + senseRange + delimiter;
			else if (senseValue != 0 && npcModel.BlindBeyond == true)
				return senseName + senseValue + senseRange + " (blind beyond this radius)" + delimiter;
			return "";
		}
		private void WriteSpeed(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();

			if (npcModel.Speed > 0)
				stringBuilder.Append(npcModel.Speed + " ft.").Append(", ");
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
			if (stringBuilder.Length >= 2)
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			string speedString = stringBuilder.ToString().Trim();

			xmlWriter.WriteStartElement("speed"); // Open <speed>
			xmlWriter.WriteAttributeString("type", "string"); // Add type=string
			xmlWriter.WriteValue(speedString);
			xmlWriter.WriteEndElement(); // Close </speed>
		}
		private void WriteSize(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("size"); // Open <size>
			xmlWriter.WriteAttributeString("type", "string"); // Add type=string
			xmlWriter.WriteValue(npcModel.Size);
			xmlWriter.WriteEndElement(); // Close </size>
		}
		private void WriteType(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(npcModel.NPCType);
			if (npcModel.Tag != null)
				stringBuilder.Append(" " + npcModel.Tag);

			xmlWriter.WriteStartElement("type"); // Open <size>
			xmlWriter.WriteAttributeString("type", "string"); // Add type=string
			xmlWriter.WriteValue(stringBuilder.ToString());
			xmlWriter.WriteEndElement(); // Close </size>
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
				stringBuilder.Append("Sleight Of Hand ").Append(npcModel.SleightOfHand >= 0 ? "+" : "").Append(npcModel.SleightOfHand).Append(", ");
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
			xmlWriter.WriteStartElement("text"); // Open <text>
			xmlWriter.WriteAttributeString("type", "formattedtext"); // Add type=formattedtext
			xmlWriter.WriteString("Edit this later");
			xmlWriter.WriteEndElement(); // Close </text>
		}
		private void WriteToken(XmlWriter xmlWriter, NPCModel npcModel, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("token"); // Open <token>
			xmlWriter.WriteAttributeString("type", "token"); // Add type=token
			if (npcModel.NPCToken == null || npcModel.NPCToken == " ")
			{
				xmlWriter.WriteString("");
			}
			else
			{
				if (moduleModel.IsLockedRecords == true)
                {
					xmlWriter.WriteValue("tokens" + '\\' + NPCNameToXMLFormat(npcModel) + "_token.png@" + moduleModel.Name);
				}
				else
                {
					xmlWriter.WriteValue("tokens" + '\\' + NPCNameToXMLFormat(npcModel) + "_token.png");

				}
			}
			xmlWriter.WriteEndElement(); // Close </token>
		}
		private void WriteTraits(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("traits"); // Open <actions>
			int actionID = 1;
			string innateName = "";
			string spellcastingName = "";

			if (npcModel.Traits != null)
			{
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
				stringBuilder.Append("The " + npcModel.NPCName.ToLower() + "'s innate spellcasting ability is " + npcModel.InnateSpellcastingAbility);
				if (npcModel.InnateSpellSaveDC != 0)
				{
					stringBuilder.Append(" (spell save DC " + npcModel.InnateSpellSaveDC);
					if (npcModel.InnateSpellHitBonus != 0)
					{
						stringBuilder.Append("spell hit bonus ").Append(npcModel.InnateSpellHitBonus >= 0 ? "+" : "").Append(npcModel.InnateSpellHitBonus);
					}
					stringBuilder.Append(")");
				}
				else if (npcModel.InnateSpellHitBonus != 0)
				{
					stringBuilder.Append("(spell hit bonus ").Append(npcModel.InnateSpellHitBonus >= 0 ? "+" : "").Append(npcModel.InnateSpellHitBonus + ")");
				}

				stringBuilder.Append(". ");
				stringBuilder.Append("It can innately cast the following spells, " + npcModel.ComponentText + ":");
				if (npcModel.InnateAtWill != null)
					stringBuilder.Append("\\rAt will: " + npcModel.InnateAtWill);
				if (npcModel.FivePerDay != null)
					stringBuilder.Append("\\r5/day each: " + npcModel.FivePerDay);
				if (npcModel.FourPerDay != null)
					stringBuilder.Append("\\r4/day each: " + npcModel.FourPerDay);
				if (npcModel.ThreePerDay != null)
					stringBuilder.Append("\\r3/day each: " + npcModel.ThreePerDay);
				if (npcModel.TwoPerDay != null)
					stringBuilder.Append("\\r2/day each: " + npcModel.TwoPerDay);
				if (npcModel.OnePerDay != null)
					stringBuilder.Append("\\r1/day each: " + npcModel.OnePerDay);
				string innateCastingDescription = stringBuilder.ToString();

				xmlWriter.WriteStartElement("id-" + actionID.ToString("D4")); // Open <id-####>
				xmlWriter.WriteStartElement("desc"); // Open <desc>
				xmlWriter.WriteAttributeString("type", "string"); // Add type=string
				xmlWriter.WriteString(innateCastingDescription); // Add Action Description
				xmlWriter.WriteEndElement(); // Close </desc>
				xmlWriter.WriteStartElement("name"); // Open <name>
				xmlWriter.WriteAttributeString("type", "string"); // Add type=string
				xmlWriter.WriteString(innateName); // Add Action Name
				xmlWriter.WriteEndElement(); // Close </name>
				xmlWriter.WriteEndElement(); // Close </id-####>
				actionID = ++actionID;
			}

			if (npcModel.SpellcastingSection)
				spellcastingName = "Spellcasting";
			if (spellcastingName.Length > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("The " + npcModel.NPCName.ToLower() + " is a " + npcModel.SpellcastingCasterLevel + "-level spellcaster. ");
				stringBuilder.Append("Its spellcasting ability is " + npcModel.SCSpellcastingAbility);
				if (npcModel.SpellcastingSpellSaveDC != 0)
				{
					stringBuilder.Append(" (spell save DC " + npcModel.SpellcastingSpellSaveDC);
					if (npcModel.SpellcastingSpellHitBonus != 0)
					{
						stringBuilder.Append(", spell hit bonus ").Append(npcModel.SpellcastingSpellHitBonus >= 0 ? "+" : "").Append(npcModel.SpellcastingSpellHitBonus);
					}
					stringBuilder.Append(")");
				}

				stringBuilder.Append(". ");
				stringBuilder.Append("The " + npcModel.NPCName.ToLower() + " has the following " + npcModel.SpellcastingSpellClass.ToLower() + " spells prepared:");
				stringBuilder.Append("\\rCantrips (" + npcModel.CantripSpells.ToLower() + "): " + npcModel.CantripSpellList.ToLower());
				if (npcModel.FirstLevelSpellList != null)
					stringBuilder.Append("\\r1st level (" + npcModel.FirstLevelSpells.ToLower() + "): " + npcModel.FirstLevelSpellList.ToLower());
				if (npcModel.SecondLevelSpellList != null)
					stringBuilder.Append("\\r2nd level (" + npcModel.SecondLevelSpells.ToLower() + "): " + npcModel.SecondLevelSpellList.ToLower());
				if (npcModel.ThirdLevelSpellList != null)
					stringBuilder.Append("\\r3rd level (" + npcModel.ThirdLevelSpells.ToLower() + "): " + npcModel.ThirdLevelSpellList.ToLower());
				if (npcModel.FourthLevelSpellList != null)
					stringBuilder.Append("\\r4th level (" + npcModel.FourthLevelSpells.ToLower() + "): " + npcModel.FourthLevelSpellList.ToLower());
				if (npcModel.FifthLevelSpellList != null)
					stringBuilder.Append("\\r5th level (" + npcModel.FifthLevelSpells.ToLower() + "): " + npcModel.FifthLevelSpellList.ToLower());
				if (npcModel.SixthLevelSpellList != null)
					stringBuilder.Append("\\r6th level (" + npcModel.SixthLevelSpells.ToLower() + "): " + npcModel.SixthLevelSpellList.ToLower());
				if (npcModel.SeventhLevelSpellList != null)
					stringBuilder.Append("\\r7th level (" + npcModel.SeventhLevelSpells.ToLower() + "): " + npcModel.SeventhLevelSpellList.ToLower());
				if (npcModel.EighthLevelSpellList != null)
					stringBuilder.Append("\\r8th level (" + npcModel.EighthLevelSpells.ToLower() + "): " + npcModel.EighthLevelSpellList.ToLower());
				if (npcModel.NinthLevelSpellList != null)
					stringBuilder.Append("\\r9th level (" + npcModel.NinthLevelSpells.ToLower() + "): " + npcModel.NinthLevelSpellList.ToLower());
				string spellcastingDescription = stringBuilder.ToString();

				xmlWriter.WriteStartElement("id-" + actionID.ToString("D4")); // Open <id-####>
				xmlWriter.WriteStartElement("desc"); // Open <desc>
				xmlWriter.WriteAttributeString("type", "string"); // Add type=string
				xmlWriter.WriteString(spellcastingDescription); // Add Action Description
				xmlWriter.WriteEndElement(); // Close </desc>
				xmlWriter.WriteStartElement("name"); // Open <name>
				xmlWriter.WriteAttributeString("type", "string"); // Add type=string
				xmlWriter.WriteString(spellcastingName); // Add Action Name
				xmlWriter.WriteEndElement(); // Close </name>
				xmlWriter.WriteEndElement(); // Close </id-####>
				actionID = ++actionID;
			}

			xmlWriter.WriteEndElement(); // Close </traits>
		}
		private void WriteXP(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("xp"); // Open <xp>
			xmlWriter.WriteAttributeString("type", "number"); // Add type=number
			xmlWriter.WriteValue(npcModel.XP);
			xmlWriter.WriteEndElement(); // Close </xp>
		}
		#endregion
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


