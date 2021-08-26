using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC;
using System.IO;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	class ImageExporter
	{
		public static void DababaseXML_Root_Reference_Imagelists(XmlWriter xmlWriter, ModuleModel module, NPCModel npc)
		{
			if (module.IncludeImages)
			{
				xmlWriter.WriteStartElement("imagelists");
				Imagelists_ByCategory(xmlWriter, module, npc);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Imagelists_ByCategory(XmlWriter xmlWriter, ModuleModel module, NPCModel npc)
		{
			xmlWriter.WriteStartElement("bycategory");
			Imagelists_ByCategory_Description(xmlWriter);
			Imagelists_ByCategory_Groups(xmlWriter, module, npc);
			xmlWriter.WriteEndElement();
		}

		private static void Imagelists_ByCategory_Groups(XmlWriter xmlWriter, ModuleModel module, NPCModel npc)
		{
			xmlWriter.WriteStartElement("groups");
			Imagelists_ByCategory_Groups_Category(xmlWriter, module, npc);
			xmlWriter.WriteEndElement();
		}

		private static void Imagelists_ByCategory_Groups_Category(XmlWriter xmlWriter, ModuleModel module, NPCModel npc)
		{
			foreach (CategoryModel category in module.Categories)
			{
				xmlWriter.WriteStartElement(CommonMethods.CategoryNameToXML(category));
				Imagelists_ByCategory_Groups_Category_Description(xmlWriter, category);
				Imagelists_ByCategory_Groups_Category_Index(xmlWriter, category, npc);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Imagelists_ByCategory_Groups_Category_Index(XmlWriter xmlWriter, CategoryModel category, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("index");
			foreach (NPCModel npc in category.NPCModels)
			{
				Index_ImageName(xmlWriter, npcModel, npc);
			}
			xmlWriter.WriteEndElement();
		}

		private static void Index_ImageName(XmlWriter xmlWriter, NPCModel npcModel, NPCModel npc)
		{
			if (!string.IsNullOrEmpty(npcModel.NPCImage))
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
