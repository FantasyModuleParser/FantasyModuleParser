using FantasyModuleParser.Main.Models;
using System;
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
			Xml_CategoryName(xmlWriter, moduleModel);
			Xml_Library_ModuleReferenceLibrary(xmlWriter, moduleModel);
			Database_Library_LibName_Entries(xmlWriter, moduleModel);
			xmlWriter.WriteEndElement();                                                
		}

		private static void Database_Library_LibName_Entries(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("entries");
			if (moduleModel.IncludeImages)
			{
				Entry_Images(xmlWriter, moduleModel);
			}
			if (moduleModel.IncludeNPCs)
			{
				Entry_NPCs(xmlWriter, moduleModel);
			}
			if (moduleModel.IncludeSpells)
			{
				Entry_Spells(xmlWriter, moduleModel);
			}
			if (moduleModel.IncludeTables)
			{
				Entry_Tables(xmlWriter, moduleModel);
			}
			if (moduleModel.IncludesEquipment)
			{
				Entry_Equipment(xmlWriter, moduleModel);
			}
			// Entry_ReferenceManual(xmlWriter, moduleModel);
			xmlWriter.WriteEndElement();                             
		}

		private static void Entry_Equipment(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			if (moduleModel.Categories[0].EquipmentModels.Count > 0)
			{
				xmlWriter.WriteStartElement("item");
				Entry_Equipment_LibraryLink(xmlWriter, moduleModel);
				Xml_Name_Equipment(xmlWriter);
				Xml_RecordType_Item(xmlWriter);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Entry_Equipment_LibraryLink(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("librarylink");
			xmlWriter.WriteAttributeString("type", "windowreference");
			Xml_Class_ReferenceList(xmlWriter);
			Xml_RecordName_Blank(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void Entry_Images(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("image");
			Entry_Images_LibraryLink(xmlWriter, moduleModel);
			Xml_Name_Images(xmlWriter);
			Xml_RecordType_Image(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void Entry_Images_LibraryLink(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("librarylink");
			xmlWriter.WriteAttributeString("type", "windowreference");
			Xml_Class_ReferenceList(xmlWriter);
			Xml_RecordName_Blank(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void Entry_NPCs(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			if (moduleModel.Categories[0].NPCModels.Count > 0)
			{
				xmlWriter.WriteStartElement("npc");
				Entry_NPCs_LibraryLink(xmlWriter, moduleModel);
				Xml_Name_NPCs(xmlWriter);
				Xml_RecordType_Npc(xmlWriter);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Entry_NPCs_LibraryLink(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("librarylink");
			xmlWriter.WriteAttributeString("type", "windowreference");
			Xml_Class_ReferenceList(xmlWriter);
			Xml_RecordName_Blank(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void Entry_Spells(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			if (moduleModel.Categories[0].SpellModels.Count > 0)
			{
				xmlWriter.WriteStartElement("spell");
				Entry_Spells_LibraryLink(xmlWriter, moduleModel);
				Xml_Name_Spells(xmlWriter);
				Xml_RecordType_Spell(xmlWriter);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Entry_Spells_LibraryLink(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("librarylink");
			xmlWriter.WriteAttributeString("type", "windowreference");
			Xml_Class_ReferenceList(xmlWriter);
			Xml_RecordName_Blank(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void Entry_Tables(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			if (moduleModel.Categories[0].TableModels.Count > 0)
			{
				xmlWriter.WriteStartElement("table");
				Entry_Tables_LibraryLink(xmlWriter, moduleModel);
				Xml_Name_Tables(xmlWriter);
				Xml_RecordType_Table(xmlWriter);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Entry_Tables_LibraryLink(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("librarylink");
			xmlWriter.WriteAttributeString("type", "windowreference");
			Xml_Class_ReferenceList(xmlWriter);
			Xml_RecordName_Blank(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void Entry_ReferenceManual(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("reference_manual");
			ReferenceManual_LibraryLink(xmlWriter, moduleModel);
			Xml_Name_ReferenceManual(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void ReferenceManual_LibraryLink(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("librarylink");
			xmlWriter.WriteAttributeString("type", "windowreference");
			Xml_Class_ReferenceManual(xmlWriter);
			Xml_RecordName_Blank(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_CategoryName(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("categoryname");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(moduleModel.Category);
			xmlWriter.WriteEndElement();
		}

		//	private static void Xml_Class_ReferenceColIndex(XmlWriter xmlWriter)
		//	{
		//		xmlWriter.WriteStartElement("class");
		//		xmlWriter.WriteString("reference_colindex");
		//		xmlWriter.WriteEndElement();
		//	}

		// private static void Xml_Class_ReferenceIndex(XmlWriter xmlWriter)
		// {
		//		xmlWriter.WriteStartElement("class");
		//		xmlWriter.WriteString("referenceindex");
		//	 	xmlWriter.WriteEndElement();
		//	}

		private static void Xml_Class_ReferenceList(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("class");
			xmlWriter.WriteString("reference_list");
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Class_ReferenceManual(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("class");
			xmlWriter.WriteString("reference_manual");
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Library_ModuleReferenceLibrary(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(moduleModel.Name + " Reference Library");
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Name_Equipment(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("Items");
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Name_Images(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("Images");
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Name_NPCs(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("NPCs");
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Name_Spells(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("Spells");
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Name_Tables(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("Tables");
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Name_ReferenceManual(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("Reference Manual");
			xmlWriter.WriteEndElement();
		}

		private static void Xml_RecordName_Blank(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("..");
			xmlWriter.WriteEndElement();
		}

		private static void Xml_RecordName_Equipment(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("reference.equipmentlists.equipment@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_RecordName_ImageLists(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("reference.imagelists.bycategory@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_RecordName_NPCList(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("reference.npclists.npcs@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Recordname_ReferenceManual(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("reference.referencemanual@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_RecordName_SpellList(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("reference.spelllists.spells@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_RecordName_Tables(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("reference.tables@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_RecordType_Image(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("recordtype");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("image");
			xmlWriter.WriteEndElement();
		}
		
		private static void Xml_RecordType_Item(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("recordtype");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("item");
			xmlWriter.WriteEndElement();
		}

		private static void Xml_RecordType_Npc(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("recordtype");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("npc");
			xmlWriter.WriteEndElement();
		}

		private static void Xml_RecordType_Spell(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("recordtype");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("spell");
			xmlWriter.WriteEndElement();
		}

		private static void Xml_RecordType_Table(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("recordtype");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("table");
			xmlWriter.WriteEndElement();
		}

		static private string WriteLibraryNameLowerCase(ModuleModel moduleModel)
		{
			string libname = moduleModel.Name.ToLower();
			return libname.Replace(" ", "").Replace("'", "");
		}
	}
}
