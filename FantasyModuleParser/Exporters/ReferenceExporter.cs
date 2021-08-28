using FantasyModuleParser.Equipment.Models;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC;
using FantasyModuleParser.Spells.Models;
using System.Collections.Generic;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	class ReferenceExporter
	{
		public static void DatabaseXML_Root_Reference(XmlWriter xmlWriter, ModuleModel module, NPCModel npc, SpellModel spell)
		{
			xmlWriter.WriteStartElement("reference"); /* <root version="4.0"> <reference> */
			if (module.IsLockedRecords)
			{
				xmlWriter.WriteAttributeString("static", "true");
			}
			NPCExporter.DatabaseXML_Root_Reference_Npcdata(xmlWriter, module);
			SpellExporter.DatabaseXML_Root_Reference_Spelldata(xmlWriter, module);
			EquipmentExporter.DatabaseXML_Root_Reference_Equipmentdata(xmlWriter, module);
			EquipmentExporter.DatabaseXML_Root_Reference_EquipmentLists(xmlWriter, module);
			ImageExporter.DababaseXML_Root_Reference_Imagelists(xmlWriter, module, npc);
			NPCExporter.DatabaseXML_Root_Reference_NPClists(xmlWriter, module);
			SpellExporter.DatabaseXML_Root_Reference_Spelllists(xmlWriter, module);
			TableExporter.DatabaseXML_Root_Reference_Tables(xmlWriter, module);
			ReferenceManualExporter.DatabaseXML_Root_Reference_ReferenceManual(xmlWriter, module, npc, spell);
			xmlWriter.WriteEndElement();
		}
	}
}
