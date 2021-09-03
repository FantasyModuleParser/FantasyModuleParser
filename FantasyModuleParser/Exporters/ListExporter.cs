using FantasyModuleParser.Main.Models;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	class ListExporter
	{
		public static void DatabaseXML_Root_Lists(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("lists");
			EquipmentExporter.DatabaseXML_Root_Lists_ItemLists(xmlWriter, module);
			ImageExporter.DababaseXML_Root_Lists_Imagelists(xmlWriter, module);
			NPCExporter.DatabaseXML_Root_Lists_NPClists(xmlWriter, module);
			SpellExporter.DatabaseXML_Root_Lists_Spelllists(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}
	}
}
