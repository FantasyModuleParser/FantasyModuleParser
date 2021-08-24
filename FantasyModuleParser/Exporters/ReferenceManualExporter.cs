using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC;
using System.Collections.Generic;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	class ReferenceManualExporter
	{
		/// <summary>
		/// <root> <reference> <referencemanual> </referencemanual>
		/// </summary>
		public static void ReferenceManual(XmlWriter xmlWriter, ModuleModel moduleModel, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("referencemanual");  /* <root> <reference> <referencemanual */
			ReferenceManual_Name(xmlWriter, moduleModel);
			ReferenceManual_Chapters(xmlWriter, moduleModel, npcModel);
			xmlWriter.WriteEndElement(); 
		}
		/// <summary>
		/// <root> <reference> <referencemanual <name> </name>
		/// </summary>
		private static void ReferenceManual_Name(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(moduleModel.Name);
			xmlWriter.WriteEndElement();
		}
		/// <summary>
		/// <root> <reference> <referencemanual <chapters> </chapters>
		/// </summary>
		private static void ReferenceManual_Chapters(XmlWriter xmlWriter, ModuleModel moduleModel, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("chapters");
			ReferenceManual_Chapters_ChapterNumber(xmlWriter, moduleModel, npcModel);
			xmlWriter.WriteEndElement(); 
		}
		/// <summary>
		/// <root> <reference> <referencemanual <chapters> <chapter_00> </chapter_00>
		/// </summary>
		private static void ReferenceManual_Chapters_ChapterNumber(XmlWriter xmlWriter, ModuleModel moduleModel, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("chapter_00");
			ReferenceManual_Chapters_ChapterNumber_Name(xmlWriter, moduleModel);
			ReferenceManual_Chapters_ChapterNumber_Subchapters(xmlWriter, moduleModel, npcModel);
			xmlWriter.WriteEndElement();
		}

		/// <summary>
		/// <root> <reference> <referencemanual <chapters> <chapter_00> <name> </name> */
		/// </summary>
		private static void ReferenceManual_Chapters_ChapterNumber_Name(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(moduleModel.Name);
			xmlWriter.WriteEndElement();
		}

		/// <summary>
		/// <root> <reference> <referencemanual <chapters> <chapter_00> <subchapters> </subchapters>
		/// </summary>
		private static void ReferenceManual_Chapters_ChapterNumber_Subchapters(XmlWriter xmlWriter, ModuleModel moduleModel, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("subchapters");
			int subchapterID = 0;
			#region NPCs
			ReferenceManual_Chapters_ChapterNumber_Subchapters_NPCs(xmlWriter, moduleModel, npcModel, subchapterID);
			#endregion
			xmlWriter.WriteEndElement(); 
		}

		/// <summary>
		/// <root> <reference> <referencemanual <chapters> <chapter_00> <subchapters> <subchapter_**> </subchapter_**>
		/// </summary>
		private static void ReferenceManual_Chapters_ChapterNumber_Subchapters_NPCs(XmlWriter xmlWriter, ModuleModel moduleModel, NPCModel npcModel, int subchapterID)
		{
			if(moduleModel.IncludeNPCs)
			{
				xmlWriter.WriteStartElement("subchapter_" + subchapterID.ToString("D2"));
				ReferenceManual_Chapters_ChapterNumber_Subchapters_NPCs_Name(xmlWriter);
				Subchapters_NPCs_Refpages(xmlWriter, moduleModel);
				xmlWriter.WriteEndElement(); 
			}
		}
		/// <summary>
		/// <root> <reference> <referencemanual <chapters> <chapter_00> <subchapters> <subchapter_**> <name type="string">NPCs</name>
		/// </summary>
		private static void ReferenceManual_Chapters_ChapterNumber_Subchapters_NPCs_Name(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("NPCs");
			xmlWriter.WriteEndElement();
		}

		/// <summary>
		/// <root> <reference> <referencemanual <chapters> <chapter_00> <subchapters> <subchapter_**> <refpages> </refpages>
		/// </summary>
		private static void Subchapters_NPCs_Refpages(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			int refpagesID = 1;
			xmlWriter.WriteStartElement("refpages");
			Subchapters_NPCs_Refpages_ID(xmlWriter, modulemodel, refpagesID);
			xmlWriter.WriteEndElement();
		}

		/// <summary>
		/// <root> <reference> <referencemanual <chapters> <chapter_00> <subchapters> <subchapter_**> <refpages> <id-"****> </id-****>
		/// </summary>
		private static void Subchapters_NPCs_Refpages_ID(XmlWriter xmlWriter, ModuleModel moduleModel, int refpagesID)
		{
			xmlWriter.WriteStartElement("id-" + refpagesID.ToString("D4"));
			Subchapters_NPCs_Refpages_ID_Blocks(xmlWriter, moduleModel);
			WriteListLink(xmlWriter);
			// Start up again tomorrow HERE!!!!
			xmlWriter.WriteEndElement();
		}

		/// <summary>
		/// <root> <reference> <referencemanual <chapters> <chapter_00> <subchapters> <subchapter_**> <refpages> <id-"****> <blocks> </blocks>
		/// </summary>
		private static void Subchapters_NPCs_Refpages_ID_Blocks(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			int blocksID = 1;
			xmlWriter.WriteStartElement("blocks");
			Subchapters_NPCs_Refpages_ID_Blocks_ID(xmlWriter, moduleModel, blocksID);
			xmlWriter.WriteEndElement();
		}
		/// <summary>
		/// <root> <reference> <referencemanual <chapters> <chapter_00> <subchapters> <subchapter_**> <refpages> <id-"****> <blocks> <id-****> </id-****>
		/// </summary>
		private static void Subchapters_NPCs_Refpages_ID_Blocks_ID(XmlWriter xmlWriter, ModuleModel moduleModel, int blocksID)
		{
			xmlWriter.WriteStartElement("id-" + blocksID.ToString("D4"));
			WriteBlockFormatting_Align(xmlWriter);
			WriteBlockFormatting_Blocktype(xmlWriter);
			WriteBlockFormatting_Text(xmlWriter, moduleModel);
			xmlWriter.WriteEndElement();
		}

		private static void WriteBlockFormatting_Align(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("align");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("center");
			xmlWriter.WriteEndElement();
		}

		private static void WriteBlockFormatting_Blocktype(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("blocktype");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("singletext");
			xmlWriter.WriteEndElement();
		}

		private static void WriteBlockFormatting_Text(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("text"); /* <text> */
			xmlWriter.WriteAttributeString("type", "formattedtext");
			NPC_Text_P(xmlWriter, moduleModel);
			xmlWriter.WriteEndElement();
		}

		private static void NPC_Text_P(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("p");
			xmlWriter.WriteString("The following NPCs are able to be found in " + moduleModel.Name + ".");
			NPC_Text_P_Linklist(xmlWriter, moduleModel);
			xmlWriter.WriteEndElement();
		}

		private static void NPC_Text_P_Linklist(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("linklist");
			NPC_Text_P_Linklist_WriteRecordname(xmlWriter, moduleModel);
			xmlWriter.WriteEndElement();
		}

		private static void NPC_Text_P_Linklist_WriteRecordname(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			List<NPCModel> FatNPCList = CommonMethods.GenerateFatNPCList(moduleModel);
			FatNPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
			foreach (NPCModel npcModel in FatNPCList)
			{
				xmlWriter.WriteStartElement("link");
				xmlWriter.WriteAttributeString("class", "npc");
				xmlWriter.WriteAttributeString("recordname", WriteRecordNameNPC(npcModel));
				xmlWriter.WriteString(npcModel.NPCName);
				xmlWriter.WriteEndElement();
			}
		}

		private static string WriteRecordNameNPC(NPCModel npcModel)
		{
			return "reference.npcdata." + NPCExporter.NPCNameToXMLFormat(npcModel);
		}

		static private void WriteListLink(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("listlink"); /* <listlink> */
			xmlWriter.WriteAttributeString("type", "windowreference");
			WriteClass(xmlWriter);
			WriteRecordname(xmlWriter);
			xmlWriter.WriteEndElement(); /* <listlink> </listlink> */
		}

		private static void WriteClass(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("class"); /* <listlink> <class> */
			xmlWriter.WriteString("reference_manualtextwide");
			xmlWriter.WriteEndElement(); /* <listlink> <class> </class> */
		}

		private static void WriteRecordname(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("recordname"); /* <listlink> <recordname> */
			xmlWriter.WriteString("..");
			xmlWriter.WriteEndElement(); /* <listlink> <recordname> </recordname> */
		}
			
		public static void SubchapterD2(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("subchapter_" + subchapterID.ToString("D2"));
		}
		
	}
}
