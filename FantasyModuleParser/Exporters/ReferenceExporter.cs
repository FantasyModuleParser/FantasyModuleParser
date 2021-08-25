using FantasyModuleParser.Main.Models;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	class ReferenceExporter
	{
		public static void DatabaseXML_Root_Reference(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("reference"); /* <root version="4.0"> <reference> */
			if (module.IsLockedRecords)
			{
				xmlWriter.WriteAttributeString("static", "true");
			}
			NPCExporter.DatabaseXML_Root_Reference_Npcdata(xmlWriter, module);
			SpellExporter.DatabaseXML_Root_Reference_Spelldata(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}
	}
}
