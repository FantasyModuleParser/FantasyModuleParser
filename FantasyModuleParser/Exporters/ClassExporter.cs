using FantasyModuleParser.Main.Models;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	class ClassExporter
	{
		public static void DatabaseXML_Root_Classes(XmlWriter xmlWriter, ModuleModel module)
		{
			if (module.IncludesClasses)
			{
				xmlWriter.WriteStartElement("class");
				Classes_Category(xmlWriter, module);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Classes_Category(XmlWriter xmlWriter, ModuleModel module)
		{
			foreach (CategoryModel category in module.Categories)
			{
				xmlWriter.WriteStartElement("category");
				xmlWriter.WriteAttributeString("name", category.Name);
				CommonMethods.BaseIcon_DecalIcon(xmlWriter);
				xmlWriter.WriteEndElement();
			}							
		}
	}
}
