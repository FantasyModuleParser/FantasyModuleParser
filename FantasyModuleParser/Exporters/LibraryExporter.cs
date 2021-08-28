using FantasyModuleParser.Main.Models;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	class LibraryExporter
	{
		public static void Database_Library(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			if (moduleModel.Categories != null && moduleModel.Categories.Count > 0)
			{
				xmlWriter.WriteStartElement("library");
				Database_Library_Libname(xmlWriter, moduleModel);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Database_Library_Libname(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement(WriteLibraryNameLowerCase(moduleModel));
			Xml_Library_ModuleReferenceLibrary(xmlWriter, moduleModel);
			Xml_CategoryName(xmlWriter, moduleModel);
			Database_Library_LibName_Entries(xmlWriter, moduleModel);
			xmlWriter.WriteEndElement();                                                
		}

		private static void Database_Library_LibName_Entries(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("entries");
			int libraryID = 1;
			if (moduleModel.IncludeImages)
			{
				Entry_Images(xmlWriter, moduleModel, libraryID);
				libraryID = ++libraryID;
			}
			if (moduleModel.IncludeNPCs)
			{
				Entry_NPCs(xmlWriter, moduleModel, libraryID);
				libraryID = ++libraryID;
			}
			if (moduleModel.IncludeSpells)
			{
				Entry_Spells(xmlWriter, moduleModel, libraryID);
				libraryID = ++libraryID;
			}
			if (moduleModel.IncludeTables)
			{
				Entry_Tables(xmlWriter, moduleModel, libraryID);
				libraryID = ++libraryID;
			}
			if (moduleModel.IncludesEquipment)
			{
				Entry_Equipment(xmlWriter, moduleModel, libraryID);
				libraryID = ++libraryID;
			}
			Entry_ReferenceManual(xmlWriter, moduleModel, libraryID);
			xmlWriter.WriteEndElement();  // close entries                               
		}

		private static void Xml_Library_ModuleReferenceLibrary(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(moduleModel.Name + " Reference Library");
			xmlWriter.WriteEndElement();
		}

		private static void Xml_CategoryName(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("categoryname");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(moduleModel.Category);
			xmlWriter.WriteEndElement();
		}

		private static void Entry_ReferenceManual(XmlWriter xmlWriter, ModuleModel moduleModel, int libraryID)
		{
			xmlWriter.WriteStartElement("id-" + libraryID.ToString("D4"));
			ReferenceManual_LibraryLink(xmlWriter, moduleModel);
			Xml_Name_ReferenceManual(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Name_ReferenceManual(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("Reference Manual");
			xmlWriter.WriteEndElement();
		}

		private static void ReferenceManual_LibraryLink(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("librarylink");
			xmlWriter.WriteAttributeString("type", "windowreference");
			Xml_Class_ReferenceManual(xmlWriter);
			Xml_Recordname_ReferenceManual(xmlWriter, moduleModel);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Recordname_ReferenceManual(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("reference.referencemanual@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Class_ReferenceManual(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("class");
			xmlWriter.WriteString("reference_manual");
			xmlWriter.WriteEndElement();
		}

		private static void Entry_Equipment(XmlWriter xmlWriter, ModuleModel moduleModel, int libraryID)
		{
			if (moduleModel.Categories[0].EquipmentModels.Count > 0)
			{
				xmlWriter.WriteStartElement("id-" + libraryID.ToString("D4"));
				Entry_Equipment_LibraryLink(xmlWriter, moduleModel);
				Xml_Name_Equipment(xmlWriter);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Xml_Name_Equipment(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("Equipment");
			xmlWriter.WriteEndElement();
		}

		private static void Entry_Equipment_LibraryLink(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("librarylink");
			xmlWriter.WriteAttributeString("type", "windowreference");
			Xml_Class_ReferenceIndex(xmlWriter);
			Xml_RecordName_Equipment(xmlWriter, moduleModel);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_RecordName_Equipment(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("reference.equipmentlists.equipment@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Class_ReferenceIndex(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("class");
			xmlWriter.WriteString("referenceindex");
			xmlWriter.WriteEndElement();
		}

		private static void Entry_Tables(XmlWriter xmlWriter, ModuleModel moduleModel, int libraryID)
		{
			if (moduleModel.Categories[0].TableModels.Count > 0)
			{
				xmlWriter.WriteStartElement("id-" + libraryID.ToString("D4"));
				Entry_Tables_LibraryLink(xmlWriter, moduleModel);
				Xml_Name_Tables(xmlWriter);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Xml_Name_Tables(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("Tables");
			xmlWriter.WriteEndElement();
		}

		private static void Entry_Tables_LibraryLink(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("librarylink");
			xmlWriter.WriteAttributeString("type", "windowreference");
			Xml_Class_ReferenceIndex(xmlWriter);
			Xml_RecordName_Tables(xmlWriter, moduleModel);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_RecordName_Tables(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("reference.tables@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Entry_Spells(XmlWriter xmlWriter, ModuleModel moduleModel, int libraryID)
		{
			if (moduleModel.Categories[0].SpellModels.Count > 0)
			{
				xmlWriter.WriteStartElement("id-" + libraryID.ToString("D4"));
				Entry_Spells_LibraryLink(xmlWriter, moduleModel);
				Xml_Name_Spells(xmlWriter);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Xml_Name_Spells(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("Spells");
			xmlWriter.WriteEndElement();
		}

		private static void Entry_Spells_LibraryLink(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("librarylink");
			xmlWriter.WriteAttributeString("type", "windowreference");
			Xml_Class_ReferenceIndex(xmlWriter);
			Xml_RecordName_SpellList(xmlWriter, moduleModel);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_RecordName_SpellList(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("reference.spelllists.spells@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Entry_NPCs(XmlWriter xmlWriter, ModuleModel moduleModel, int libraryID)
		{
			if (moduleModel.Categories[0].NPCModels.Count > 0)
			{
				xmlWriter.WriteStartElement("id-" + libraryID.ToString("D4"));
				Entry_NPCs_LibraryLink(xmlWriter, moduleModel);
				Xml_Name_NPCs(xmlWriter);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Xml_Name_NPCs(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("NPCs");
			xmlWriter.WriteEndElement();
		}

		private static void Entry_NPCs_LibraryLink(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("librarylink");
			xmlWriter.WriteAttributeString("type", "windowreference");
			Xml_Class_ReferenceIndex(xmlWriter);
			Xml_RecordName_NPCList(xmlWriter, moduleModel);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_RecordName_NPCList(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("reference.npclists.npcs@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Entry_Images(XmlWriter xmlWriter, ModuleModel moduleModel, int libraryID)
		{
			xmlWriter.WriteStartElement("id-" + libraryID.ToString("D4"));
			Entry_Images_LibraryLink(xmlWriter, moduleModel);
			Xml_Name_Images(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Name_Images(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("Images");
			xmlWriter.WriteEndElement();
		}

		private static void Entry_Images_LibraryLink(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("librarylink");
			xmlWriter.WriteAttributeString("type", "windowreference");
			Xml_Class_ReferenceColIndex(xmlWriter);
			Xml_RecordName_ImageLists(xmlWriter, moduleModel);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_RecordName_ImageLists(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("reference.imagelists.bycategory@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Class_ReferenceColIndex(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("class");
			xmlWriter.WriteString("reference_colindex");
			xmlWriter.WriteEndElement();
		}

		static private string WriteLibraryNameLowerCase(ModuleModel moduleModel)
		{
			string libname = moduleModel.Name.ToLower();
			return libname.Replace(" ", "").Replace("'", "");
		}
	}
}
