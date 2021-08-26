using FantasyModuleParser.Equipment.Models;
using FantasyModuleParser.Main.Models;
using System.Collections.Generic;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	class ReferenceExporter
	{
		public static void DatabaseXML_Root_Reference(XmlWriter xmlWriter, ModuleModel module, List<EquipmentModel> FatEquipmentList)
		{
			xmlWriter.WriteStartElement("reference"); /* <root version="4.0"> <reference> */
			if (module.IsLockedRecords)
			{
				xmlWriter.WriteAttributeString("static", "true");
			}
			NPCExporter.DatabaseXML_Root_Reference_Npcdata(xmlWriter, module);
			SpellExporter.DatabaseXML_Root_Reference_Spelldata(xmlWriter, module);
			EquipmentExporter.DatabaseXML_Root_Reference_Equipmentdata(xmlWriter, module);
			EquipmentExporter.DatabaseXML_Root_Reference_EquipmentLists(xmlWriter, FatEquipmentList, module);
			xmlWriter.WriteEndElement();
		}
	}
}
