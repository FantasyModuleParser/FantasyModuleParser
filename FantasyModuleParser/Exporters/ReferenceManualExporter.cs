using FantasyModuleParser.Equipment.Models;
using FantasyModuleParser.Extensions;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.Spells.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	class ReferenceManualExporter
	{
		public static void DatabaseXML_Root_ReferenceManual(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("referencemanual");
			Xml_Name_ModuleName(xmlWriter, module);
			ReferenceManual_Chapters(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void ReferenceManual_Chapters(XmlWriter xmlWriter, ModuleModel module)		
		{
			xmlWriter.WriteStartElement("chapters");
			ReferenceManual_Chapters_Chapter_xx(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void ReferenceManual_Chapters_Chapter_xx(XmlWriter xmlWriter, ModuleModel module)
		{
			int chapterID = 0;
			xmlWriter.WriteStartElement("chapter_" + chapterID.ToString("D2"));
			Xml_Name_ModuleName(xmlWriter, module);
			Chapter_xx_Subchapters(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void Chapter_xx_Subchapters(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("subchapters");
			Chapter_xx_Subchapters_Subchapter_xx(xmlWriter, module);
		}

		private static void Chapter_xx_Subchapters_Subchapter_xx(XmlWriter xmlWriter, ModuleModel module)
		{
			int subchapterID = 0;
			Subchapter_NPCs(xmlWriter, module, subchapterID);
			Subchapter_Spells(xmlWriter, module, subchapterID);
			Subchapter_Equipment(xmlWriter, module, subchapterID);
		}

		private static void Subchapter_Equipment(XmlWriter xmlWriter, ModuleModel module, int subchapterID)
		{
			if (module.IncludesEquipment)
			{
				xmlWriter.WriteStartElement("subchapter_" + subchapterID.ToString("D2")); // open <subchapters> <subchapter_**>
				Xml_Name_Equipment(xmlWriter);
				Equipment_Refpages(xmlWriter, module);
				xmlWriter.WriteEndElement(); // </subchapter_**>
				subchapterID = ++subchapterID;
			}
		}

		private static void Equipment_Refpages(XmlWriter xmlWriter, ModuleModel module)
		{
			int refpagesID = 1;
			xmlWriter.WriteStartElement("refpages"); // open <refpages>
			Equipment_Refpages_RefpagesID(xmlWriter, module, refpagesID);
			refpagesID = ++refpagesID;
			List<EquipmentModel> FatEquipmentList = CommonMethods.GenerateFatEquipmentList(module);
			FatEquipmentList.Sort((equipOne, equipTwo) => equipOne.Name.CompareTo(equipTwo.Name));
			foreach (EquipmentModel equipmentModel in FatEquipmentList)
			{
				NPCController npcController = new NPCController();
				IndividualEquipment_Name(xmlWriter, refpagesID, equipmentModel, npcController);
				refpagesID = ++refpagesID;
			}
			xmlWriter.WriteEndElement();
		}

		private static void IndividualEquipment_Name(XmlWriter xmlWriter, int refpagesID, EquipmentModel equipmentModel, NPCController npcController)
		{
			xmlWriter.WriteStartElement("id-" + refpagesID.ToString("D4")); // <open id-****>
			IndividualEquipment_Name_Blocks(xmlWriter, equipmentModel, npcController);
			Xml_WriteListLink(xmlWriter);
			Xml_Name_EquipmentName(xmlWriter, equipmentModel);
			xmlWriter.WriteEndElement(); // </id-****>
		}

		private static void Xml_Name_EquipmentName(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(equipmentModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void IndividualEquipment_Name_Blocks(XmlWriter xmlWriter, EquipmentModel equipmentModel, NPCController npcController)
		{
			int blocksID = 1;
			xmlWriter.WriteStartElement("blocks");
			IndividualEquipment_Name_Blocks_BlocksID(xmlWriter, equipmentModel, npcController, blocksID);
			xmlWriter.WriteEndElement();
		}

		private static void IndividualEquipment_Name_Blocks_BlocksID(XmlWriter xmlWriter, EquipmentModel equipmentModel, NPCController npcController, int blocksID)
		{
			xmlWriter.WriteStartElement("id-" + blocksID.ToString("D4"));
			Xml_WriteBlockFormatting(xmlWriter);
			IndividualEquipment_Name_Blocks_BlocksID_Text(xmlWriter, equipmentModel, npcController);
			xmlWriter.WriteEndElement();
		}

		private static void IndividualEquipment_Name_Blocks_BlocksID_Text(XmlWriter xmlWriter, EquipmentModel equipmentModel, NPCController npcController)
		{
			xmlWriter.WriteStartElement("text");
			xmlWriter.WriteAttributeString("type", "formattedtext");
			xmlWriter.WriteRaw("<p><h>"); //
			xmlWriter.WriteString(equipmentModel.Name);
			xmlWriter.WriteRaw("</h></p><p><i>");
			if (string.IsNullOrEmpty(equipmentModel.Description))
			{
			}
			else
			{
				xmlWriter.WriteRaw(npcController.GenerateFantasyGroundsDescriptionXML(equipmentModel.Description));
			}
			xmlWriter.WriteRaw("</i></p><p>");
			Xml_Equipment_Link(xmlWriter, equipmentModel);
			xmlWriter.WriteString(equipmentModel.Name);
			xmlWriter.WriteRaw("</p>");
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Equipment_Link(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("link");
			xmlWriter.WriteAttributeString("class", "item");
			xmlWriter.WriteAttributeString("recordname", WriteRecordNameEquipment(equipmentModel));
			xmlWriter.WriteRaw("<b>");
			xmlWriter.WriteString("Item:");
			xmlWriter.WriteRaw("</b>");
			xmlWriter.WriteEndElement();
		}

		private static void Equipment_Refpages_RefpagesID(XmlWriter xmlWriter, ModuleModel module, int refpagesID)
		{
			xmlWriter.WriteStartElement("id-" + refpagesID.ToString("D4")); // open <refpages> <a1>
			int blocksID = 1;
			Equipment_Refpages_RefpagesID_Blocks(xmlWriter, module, blocksID);
			Xml_WriteListLink(xmlWriter);
			Xml_Name_Module_Equipment(xmlWriter, module);
			xmlWriter.WriteEndElement(); // close </id-0001>
		}

		private static void Xml_Name_Module_Equipment(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("name"); // open <name>
			xmlWriter.WriteAttributeString("type", "string"); // <name type=string>
			xmlWriter.WriteString(module.Name + " Equipment"); // <name type=string> * Equipment
			xmlWriter.WriteEndElement(); // close </name>
		}

		private static void Equipment_Refpages_RefpagesID_Blocks(XmlWriter xmlWriter, ModuleModel module, int blocksID)
		{
			xmlWriter.WriteStartElement("blocks"); // open <refpages> <a1> <blocks>
			Equipment_Refpages_RefpagesID_Blocks_BlocksID(xmlWriter, module, blocksID);
			xmlWriter.WriteEndElement(); // close </blocks>
		}

		private static void Equipment_Refpages_RefpagesID_Blocks_BlocksID(XmlWriter xmlWriter, ModuleModel module, int blocksID)
		{
			xmlWriter.WriteStartElement("id-" + blocksID.ToString("D4"));
			Xml_WriteBlockFormatting(xmlWriter);
			EquipmentList_Text(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void EquipmentList_Text(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("text");
			xmlWriter.WriteAttributeString("type", "formattedtext");
			EquipmentList_Text_P(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void EquipmentList_Text_P(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("p");
			xmlWriter.WriteString("The following Equipment are able to be found in " + module.Name + ".");
			EquipmentList_Text_P_Linklist(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void EquipmentList_Text_P_Linklist(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("linklist");
			List<EquipmentModel> FatEquipmentList = CommonMethods.GenerateFatEquipmentList(module);
			FatEquipmentList.Sort((equipOne, equipTwo) => equipOne.Name.CompareTo(equipTwo.Name));
			foreach (EquipmentModel equipmentModel in FatEquipmentList)
			{
				Link_ClassItem_Equipment(xmlWriter, equipmentModel);
			}
			xmlWriter.WriteEndElement();
		}

		private static void Link_ClassItem_Equipment(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("link");
			xmlWriter.WriteAttributeString("class", "item");
			xmlWriter.WriteAttributeString("recordname", WriteRecordNameEquipment(equipmentModel));
			xmlWriter.WriteString(equipmentModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Name_Equipment(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name"); 
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("Equipment");
			xmlWriter.WriteEndElement();
		}

		private static void Subchapter_Spells(XmlWriter xmlWriter, ModuleModel module, int subchapterID)
		{
			if (module.IncludeSpells)
			{
				xmlWriter.WriteStartElement("subchapter_" + subchapterID.ToString("D2"));
				Xml_Name_Spells(xmlWriter);
				int refpagesID = 1;
				Spells_Refpages(xmlWriter, module, refpagesID);
				xmlWriter.WriteEndElement();
				subchapterID = ++subchapterID;
			}
		}

		private static void Spells_Refpages(XmlWriter xmlWriter, ModuleModel module, int refpagesID)
		{
			xmlWriter.WriteStartElement("refpages");
			Spells_RefPagesID(xmlWriter, module, refpagesID);
			List<SpellModel> FatSpellList = CommonMethods.GenerateFatSpellList(module);
			FatSpellList.Sort((spellOne, spellTwo) => spellOne.SpellName.CompareTo(spellTwo.SpellName));
			int spellID = 2;
			foreach (SpellModel spell in FatSpellList)
			{
				NPCController npcController = new NPCController();
				IndividualSpell_IDs(xmlWriter, spell, spellID, npcController);
				spellID = ++spellID;
			}
			xmlWriter.WriteEndElement();
		}

		private static void Spells_RefPagesID(XmlWriter xmlWriter, ModuleModel module, int refpagesID)
		{
			xmlWriter.WriteStartElement("id-" + refpagesID.ToString("D4"));
			Spells_RefPages_Blocks(xmlWriter, module);
			Xml_WriteListLink(xmlWriter);
			Xml_Name_ModuleName_Spells(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Name_ModuleName_Spells(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(module.Name + " Spells");
			xmlWriter.WriteEndElement();
		}

		private static void Spells_RefPages_Blocks(XmlWriter xmlWriter, ModuleModel module)
		{
			int Spell_blocksID = 1;
			xmlWriter.WriteStartElement("blocks");
			Spell_RefPages_BlocksID(xmlWriter, module, Spell_blocksID);
			xmlWriter.WriteEndElement();
		}

		private static void Spell_RefPages_BlocksID(XmlWriter xmlWriter, ModuleModel module, int Spell_blocksID)
		{
			xmlWriter.WriteStartElement("id-" + Spell_blocksID.ToString("D4"));
			Xml_WriteBlockFormatting(xmlWriter);
			SpellsList_Text(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void SpellsList_Text(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("text");
			xmlWriter.WriteAttributeString("type", "formattedtext");
			SpellsList_Text_P(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void SpellsList_Text_P(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("p");
			xmlWriter.WriteString("The following Spells are able to be found in " + module.Name + ".");
			Spells_Linklist(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void IndividualSpell_IDs(XmlWriter xmlWriter, SpellModel spell, int spellID, NPCController npcController)
		{
			xmlWriter.WriteStartElement("id-" + spellID.ToString("D4"));
			int blockID = 1;
			IndividualSpells_Blocks(xmlWriter, spell, npcController, blockID);
			Xml_WriteListLink(xmlWriter);
			Xml_Name_SpellName(xmlWriter, spell);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Name_SpellName(XmlWriter xmlWriter, SpellModel spell)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(spell.SpellName);
			xmlWriter.WriteEndElement();
		}

		private static void IndividualSpells_Blocks(XmlWriter xmlWriter, SpellModel spell, NPCController npcController, int blockID)
		{
			xmlWriter.WriteStartElement("blocks");
			IndividualSpells_BlockID(xmlWriter, spell, npcController, blockID);
			xmlWriter.WriteEndElement();
		}

		private static void IndividualSpells_BlockID(XmlWriter xmlWriter, SpellModel spell, NPCController npcController, int blockID)
		{
			xmlWriter.WriteStartElement("id-" + blockID.ToString("D4"));
			Xml_WriteBlockFormatting(xmlWriter);
			IndividualSpell_Text(xmlWriter, spell, npcController);
			xmlWriter.WriteEndElement();
		}

		private static void IndividualSpell_Text(XmlWriter xmlWriter, SpellModel spell, NPCController npcController)
		{
			xmlWriter.WriteStartElement("text");
			xmlWriter.WriteAttributeString("type", "formattedtext");
			xmlWriter.WriteRaw("<p><h>");
			xmlWriter.WriteString(spell.SpellName);
			xmlWriter.WriteRaw("</h></p><p><i>");
			if (spell.SpellLevel.GetDescription().Equals("cantrip"))
			{
				xmlWriter.WriteString(spell.SpellSchool + " cantrip");
			}
			else
			{
				xmlWriter.WriteString(spell.SpellLevel.GetDescription() + "-level " + spell.SpellSchool);
				if (spell.IsRitual.Equals(1))
				{
					xmlWriter.WriteString(" (ritual)");
				}
			}
			xmlWriter.WriteRaw("</i></p><p><b>");
			xmlWriter.WriteString("Casting Time:");
			xmlWriter.WriteRaw("</b>");
			xmlWriter.WriteString(" " + spell.CastingTime + " " + spell.CastingType.GetDescription());
			if (spell.CastingTime > 1)
			{
				xmlWriter.WriteString("s");
			}
			xmlWriter.WriteRaw("</p><p><b>");
			xmlWriter.WriteString("Range:");
			xmlWriter.WriteRaw("</b>");
			xmlWriter.WriteString(" " + spell.RangeDescription);
			xmlWriter.WriteRaw("</p><p><b>");
			xmlWriter.WriteString("Components:");
			xmlWriter.WriteRaw("</b>");
			xmlWriter.WriteString(" " + spell.ComponentDescription);
			xmlWriter.WriteRaw("</p><p><b>");
			xmlWriter.WriteString("Duration:");
			xmlWriter.WriteRaw("</b>");
			xmlWriter.WriteString(" " + spell.DurationText);
			xmlWriter.WriteRaw("</p>");
			xmlWriter.WriteRaw(npcController.GenerateFantasyGroundsDescriptionXML(spell.Description) + "<p>");
			Link_Class_Power_Recordname(xmlWriter, spell);
			xmlWriter.WriteString(spell.SpellName);
			xmlWriter.WriteRaw("</p>");
			xmlWriter.WriteEndElement();
		}

		private static void Link_Class_Power_Recordname(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("link");
			xmlWriter.WriteAttributeString("class", "power");
			xmlWriter.WriteAttributeString("recordname", WriteRecordNameSpell(spellModel));
			xmlWriter.WriteRaw("<b>");
			xmlWriter.WriteString("Spell:");
			xmlWriter.WriteRaw("</b>");
			xmlWriter.WriteEndElement();
		}

		private static void Spells_Linklist(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("linklist");
			List<SpellModel> FatSpellList = CommonMethods.GenerateFatSpellList(module);
			FatSpellList.Sort((spellOne, spellTwo) => spellOne.SpellName.CompareTo(spellTwo.SpellName));
			foreach (SpellModel spell in FatSpellList)
			{
				Spells_Linklist_Link(xmlWriter, spell);
			}
			xmlWriter.WriteEndElement();
		}

		private static void Spells_Linklist_Link(XmlWriter xmlWriter, SpellModel spell)
		{
			xmlWriter.WriteStartElement("link");
			xmlWriter.WriteAttributeString("class", "power");
			xmlWriter.WriteAttributeString("recordname", WriteRecordNameSpell(spell));
			xmlWriter.WriteString(spell.SpellName);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Name_Spells(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("Spells");
			xmlWriter.WriteEndElement();
		}

		private static void Subchapter_NPCs(XmlWriter xmlWriter, ModuleModel module, int subchapterID)
		{
			if (module.IncludeNPCs)
			{
				xmlWriter.WriteStartElement("subchapter_" + subchapterID.ToString("D2"));
				CommonMethods.Xml_Name_Npcs(xmlWriter);
				Subchapter_xx_NPCRefPages(xmlWriter, module);
				xmlWriter.WriteEndElement();
				subchapterID = ++subchapterID;
			}
		}

		private static void Subchapter_xx_NPCRefPages(XmlWriter xmlWriter, ModuleModel module)
		{
			int refpagesID = 1;
			xmlWriter.WriteStartElement("refpages");
			RefPages_NPCList(xmlWriter, refpagesID, module);
			List<NPCModel> FatNPCList = CommonMethods.GenerateFatNPCList(module);
			FatNPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
			RefPages_IndividualNPCs(xmlWriter, refpagesID, FatNPCList);
			xmlWriter.WriteEndElement();
		}

		private static void RefPages_IndividualNPCs(XmlWriter xmlWriter, int refpagesID, List<NPCModel> FatNPCList)
		{
			foreach (NPCModel npcModel in FatNPCList)
			{
				NPCController npcController = new NPCController();
				xmlWriter.WriteStartElement("id-" + refpagesID.ToString("D4"));
				RefPages_IndividualNPCs_Blocks(xmlWriter, npcModel, npcController);
				xmlWriter.WriteEndElement();
				refpagesID = ++refpagesID;
			}
		}

		private static void RefPages_IndividualNPCs_Blocks(XmlWriter xmlWriter, NPCModel npcModel, NPCController npcController)
		{
			int blocksID = 1;
			xmlWriter.WriteStartElement("blocks");
			IndividualNPCs_Blocks_ID(xmlWriter, npcModel, npcController, blocksID);
			xmlWriter.WriteEndElement();
			Xml_WriteListLink(xmlWriter);
			Xml_Name_NPCName(xmlWriter, npcModel);
		}

		private static void Xml_Name_NPCName(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(npcModel.NPCName);
			xmlWriter.WriteEndElement();
		}

		private static void IndividualNPCs_Blocks_ID(XmlWriter xmlWriter, NPCModel npcModel, NPCController npcController, int blocksID)
		{
			xmlWriter.WriteStartElement("id-" + blocksID.ToString("D4"));
			Xml_WriteBlockFormatting(xmlWriter);
			IndividualNPCs_Blocks_ID_Text(xmlWriter, npcModel, npcController);
			xmlWriter.WriteEndElement();
		}

		private static void IndividualNPCs_Blocks_ID_Text(XmlWriter xmlWriter, NPCModel npcModel, NPCController npcController)
		{
			xmlWriter.WriteStartElement("text");
			xmlWriter.WriteAttributeString("type", "formattedtext");
			IndividualNPCs_Blocks_ID_Text_P(xmlWriter, npcModel, npcController);
			IndividualNPCs_Blocks_ID_Image(xmlWriter, npcModel);
			IndividualNPCs_Blocks_ID_Link(xmlWriter, npcModel);
			xmlWriter.WriteEndElement();
		}

		private static void IndividualNPCs_Blocks_ID_Link(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("link");
			xmlWriter.WriteAttributeString("class", "npc");
			xmlWriter.WriteAttributeString("recordname", WriteRecordNameNPC(npcModel));
			Xml_Bold_NPC(xmlWriter);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteString(npcModel.NPCName);
		}

		private static void Xml_Bold_NPC(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("b"); // <b>
			xmlWriter.WriteString("NPC:");
			xmlWriter.WriteEndElement(); // </b>
		}

		private static void IndividualNPCs_Blocks_ID_Image(XmlWriter xmlWriter, NPCModel npcModel)
		{
			if (npcModel.NPCImage.Length > 2)
			{
				xmlWriter.WriteStartElement("link");
				xmlWriter.WriteAttributeString("class", "imagewindow");
				xmlWriter.WriteAttributeString("recordname", WriteImageXML(npcModel));
				Xml_Bold_Image(xmlWriter);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteString(npcModel.NPCName);
			}
		}

		private static void Xml_Bold_Image(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("b");
			xmlWriter.WriteString("Image:");
			xmlWriter.WriteEndElement();
		}

		private static void IndividualNPCs_Blocks_ID_Text_P(XmlWriter xmlWriter, NPCModel npcModel, NPCController npcController)
		{
			if (!string.IsNullOrEmpty(npcModel.Description) && npcModel.Description.Length > 2)
			{
				xmlWriter.WriteRaw(npcController.GenerateFantasyGroundsDescriptionXML(npcModel.Description));
			}
			else
			{
				xmlWriter.WriteStartElement("p");
				xmlWriter.WriteEndElement();
			}
		}

		private static void RefPages_NPCList(XmlWriter xmlWriter, int refpagesID, ModuleModel module)
		{
			xmlWriter.WriteStartElement("id-" + refpagesID.ToString("D4"));
			RefPages_NPCList_Blocks(xmlWriter, module);
			Xml_WriteListLink(xmlWriter);
			Xml_Name_Module_NPCs(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Name_Module_NPCs(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(module.Name + " NPCs");
			xmlWriter.WriteEndElement();
		}

		private static void RefPages_NPCList_Blocks(XmlWriter xmlWriter, ModuleModel module)
		{
			int blocksID = 1;
			xmlWriter.WriteStartElement("blocks");
			RefPages_NPCList_Blocks_ID(xmlWriter, blocksID, module);
			xmlWriter.WriteEndElement();
		}

		private static void RefPages_NPCList_Blocks_ID(XmlWriter xmlWriter, int blocksID, ModuleModel module)
		{
			xmlWriter.WriteStartElement("id-" + blocksID.ToString("D4"));
			Xml_WriteBlockFormatting(xmlWriter);
			RefPages_NPCList_Blocks_ID_Text(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void RefPages_NPCList_Blocks_ID_Text(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("text");
			xmlWriter.WriteAttributeString("type", "formattedtext");
			RefPages_NPCList_Blocks_ID_Text_P(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void RefPages_NPCList_Blocks_ID_Text_P(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("p");
			xmlWriter.WriteString("The following NPCs are able to be found in " + module.Name + ".");
			RefPages_NPCList_Blocks_ID_Text_P_Linklist(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void RefPages_NPCList_Blocks_ID_Text_P_Linklist(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("linklist");
			List<NPCModel> FatNPCList = CommonMethods.GenerateFatNPCList(module);
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

		private static void Xml_WriteListLink(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("listlink");
			xmlWriter.WriteAttributeString("type", "windowreference");
			Xml_WriteClass_ReferenceManualTextWide(xmlWriter);
			Xml_WriteRecordName_DotDot(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_WriteClass_ReferenceManualTextWide(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("class");
			xmlWriter.WriteString("reference_manualtextwide");
			xmlWriter.WriteEndElement();
		}

		private static void Xml_WriteRecordName_DotDot(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("..");
			xmlWriter.WriteEndElement();
		}
		
		private static void Xml_Name_ModuleName(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(module.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_WriteBlockFormatting(XmlWriter xmlWriter)
		{
			Xml_WriteBlockFormatting_Align(xmlWriter);
			Xml_WriteBlockFormatting_Blocktype(xmlWriter);
		}

		private static void Xml_WriteBlockFormatting_Blocktype(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("blocktype"); /* <blocktype> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("singletext");
			xmlWriter.WriteEndElement(); /* <blocktype> </blocktype> */
		}

		private static void Xml_WriteBlockFormatting_Align(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("align"); /* <align> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("center");
			xmlWriter.WriteEndElement(); /* <align> </align> */
		}

		private static string WriteRecordNameNPC(NPCModel npcModel)
		{
			return "npc." + NPCExporter.NPCNameToXMLFormat(npcModel);
		}
		
		private static string WriteRecordNameSpell(SpellModel spellModel)
		{
			return "spell." + SpellExporter.SpellNameToXMLFormat(spellModel);
		}
		
		private static string WriteRecordNameEquipment(EquipmentModel equipmentModel)
		{
			return "item." + EquipmentExporter.EquipmentNameToXML(equipmentModel);
		}
		
		private static string WriteImageXML(NPCModel npcModel)
		{
			return "image." + Path.GetFileNameWithoutExtension(npcModel.NPCImage).Replace(" ", "").Replace("-", "");
		}
	}
}
