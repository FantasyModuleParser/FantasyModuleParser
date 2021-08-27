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
				xmlWriter.WriteStartElement("librarylink");
				xmlWriter.WriteAttributeString("type", "windowreference");
				xmlWriter.WriteStartElement("class");
				xmlWriter.WriteString("referenceindex");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("recordname");
				xmlWriter.WriteString("reference.equipmentlists.equipment@" + moduleModel.Name);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("name");
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString("Equipment");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
			}
		}

		private static void Entry_Tables(XmlWriter xmlWriter, ModuleModel moduleModel, int libraryID)
		{
			if (moduleModel.Categories[0].TableModels.Count > 0)
			{
				xmlWriter.WriteStartElement("id-" + libraryID.ToString("D4"));
				xmlWriter.WriteStartElement("librarylink");
				xmlWriter.WriteAttributeString("type", "windowreference");
				xmlWriter.WriteStartElement("class");
				xmlWriter.WriteString("referenceindex");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("recordname");
				xmlWriter.WriteString("reference.tables@" + moduleModel.Name);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("name");
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString("Tables");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
			}
		}

		private static void Entry_Spells(XmlWriter xmlWriter, ModuleModel moduleModel, int libraryID)
		{
			if (moduleModel.Categories[0].SpellModels.Count > 0)
			{
				xmlWriter.WriteStartElement("id-" + libraryID.ToString("D4"));
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

		private static void Entry_NPCs(XmlWriter xmlWriter, ModuleModel moduleModel, int libraryID)
		{
			if (moduleModel.Categories[0].NPCModels.Count > 0)
			{
				xmlWriter.WriteStartElement("id-" + libraryID.ToString("D4")); /* <library> <libname> <r**monsters> */
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
			}
		}

		private static void Entry_Images(XmlWriter xmlWriter, ModuleModel moduleModel, int libraryID)
		{
			xmlWriter.WriteStartElement("id-" + libraryID.ToString("D4")); /* <library> <libname> <r**images> */
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
		}

		static private string WriteLibraryNameLowerCase(ModuleModel moduleModel)
		{
			string libname = moduleModel.Name.ToLower();
			return libname.Replace(" ", "").Replace("'", "");
		}
	}
}
