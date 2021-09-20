﻿using FantasyModuleParser.Main.Models;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	public class DatabaseExporter
	{
        public static void DatabaseXML(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartDocument();
			DatabaseXML_Comments(xmlWriter);
			DatabaseXML_Root(xmlWriter, module);
			xmlWriter.WriteEndDocument();
			xmlWriter.Close();
		}

		private static void DatabaseXML_Comments(XmlWriter xmlWriter)
		{
			xmlWriter.WriteComment("Generated by Fantasy Module Parser");
			xmlWriter.WriteComment("Written by Theodore Story, Darkpool, and Battlemarch (c) 2021");
		}

		private static void DatabaseXML_Root(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("root");
			xmlWriter.WriteAttributeString("version", "4.0");
			ImageExporter.DatabaseXML_Root_Image(xmlWriter, module);
			EquipmentExporter.DatabaseXML_Root_Item(xmlWriter, module);
			ListExporter.DatabaseXML_Root_Lists(xmlWriter, module);
			NPCExporter.DatabaseXML_Root_Npc(xmlWriter, module);
			SpellExporter.DatabaseXML_Root_Spell(xmlWriter, module);
			TableExporter.DatabaseXML_Root_Tables(xmlWriter, module);
			ReferenceManualExporter.DatabaseXML_Root_ReferenceManual(xmlWriter, module);
			LibraryExporter.Database_Library(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}
	}
}
