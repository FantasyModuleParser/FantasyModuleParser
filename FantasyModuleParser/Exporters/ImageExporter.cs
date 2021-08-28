using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC;
using System.IO;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	class ImageExporter
	{
		public static void DababaseXML_Root_Reference_Imagelists(XmlWriter xmlWriter, ModuleModel module)
		{
			if (module.IncludeImages)
			{
				xmlWriter.WriteStartElement("imagelists");
				Imagelists_ByCategory(xmlWriter, module);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Imagelists_ByCategory(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("bycategory");
			Imagelists_ByCategory_Description(xmlWriter);
			Imagelists_ByCategory_Groups(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void Imagelists_ByCategory_Groups(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("groups");
			Imagelists_ByCategory_Groups_Category(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void Imagelists_ByCategory_Groups_Category(XmlWriter xmlWriter, ModuleModel module)
		{
			foreach (CategoryModel category in module.Categories)
			{
				xmlWriter.WriteStartElement(CommonMethods.CategoryNameToXML(category));
				Imagelists_ByCategory_Groups_Category_Description(xmlWriter, category);
				Imagelists_ByCategory_Groups_Category_Index(xmlWriter, category);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Imagelists_ByCategory_Groups_Category_Index(XmlWriter xmlWriter, CategoryModel category)
		{
			xmlWriter.WriteStartElement("index");
			foreach (NPCModel npc in category.NPCModels)
			{
				Index_ImageName(xmlWriter, npc);
			}
			xmlWriter.WriteEndElement();
		}

		private static void Index_ImageName(XmlWriter xmlWriter, NPCModel npc)
		{
			if (!string.IsNullOrEmpty(npc.NPCImage))
			{
				xmlWriter.WriteStartElement(Path.GetFileNameWithoutExtension(npc.NPCImage).Replace(" ", "").Replace("-", ""));
				INdex_ImageName_Link(xmlWriter, npc);
				Index_ImageName_Source(xmlWriter);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Index_ImageName_Source(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("source");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteEndElement();
		}

		private static void INdex_ImageName_Link(XmlWriter xmlWriter, NPCModel npc)
		{
			xmlWriter.WriteStartElement("link");
			xmlWriter.WriteAttributeString("type", "windowreference");
			Index_ImageName_Link_Class(xmlWriter);
			Index_ImageName_Link_Recordname(xmlWriter, npc);
			Index_ImageName_Link_Description(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void Index_ImageName_Link_Description(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("description");
			Index_ImageName_LInk_Description_Field(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void Index_ImageName_LInk_Description_Field(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("field");
			xmlWriter.WriteString("name");
			xmlWriter.WriteEndElement();
		}

		private static void Index_ImageName_Link_Recordname(XmlWriter xmlWriter, NPCModel npc)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("image." + Path.GetFileNameWithoutExtension(npc.NPCImage).Replace(" ", "").Replace("-", ""));
			xmlWriter.WriteEndElement();
		}

		private static void Index_ImageName_Link_Class(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("class");
			xmlWriter.WriteString("imagewindow");
			xmlWriter.WriteEndElement();
		}

		private static void Imagelists_ByCategory_Groups_Category_Description(XmlWriter xmlWriter, CategoryModel category)
		{
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(category.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Imagelists_ByCategory_Description(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("Images");
			xmlWriter.WriteEndElement();
		}
	}
}
