using FantasyModuleParser.Main.Models;
using System;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	class ReferenceManualExporter
	{
		public static void DatabaseXML_Root_Reference_ReferenceManual(XmlWriter xmlWriter, ModuleModel module)
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
			xmlWriter.WriteStartElement("chapter_" + chapterID.ToString("D2");
			Xml_Name_ModuleName(xmlWriter, module);
			Chapter_xx_Subchapters(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void Chapter_xx_Subchapters(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("subchapters"); /* <root> <reference> <referencemanual <chapters> <chapter_00> <subchapters> */
			Chapter_xx_Subchapters_Subchapter_xx(xmlWriter, module);
		}

		private static void Chapter_xx_Subchapters_Subchapter_xx(XmlWriter xmlWriter, ModuleModel module)
		{
			int subchapterID = 0;
			if (module.IncludeNPCs)
			{
				xmlWriter.WriteStartElement("subchapter_" + subchapterID.ToString("D2")); // open <subchapters> <subchapter_**>
				CommonMethods.Xml_Name_Npcs(xmlWriter);
				Subchapter_xx_NPCRefPages(xmlWriter);
				xmlWriter.WriteEndElement(); // close </subchapter_**>
				subchapterID = ++subchapterID;
			}

			if (moduleModel.IncludeSpells)
			{
				xmlWriter.WriteStartElement("subchapter_" + subchapterID.ToString("D2")); // open <subchapters> <subchapter_**>
				xmlWriter.WriteStartElement("name"); // open <subchapters> <subchapter_*> <name>
				xmlWriter.WriteAttributeString("type", "string"); // <name type=string>
				xmlWriter.WriteString("Spells");
				xmlWriter.WriteEndElement(); // close </name>
				xmlWriter.WriteStartElement("refpages"); // open <refpages>
				xmlWriter.WriteStartElement("id-0001"); // open <refpages> <a1>
				xmlWriter.WriteStartElement("blocks"); // open <refpages> <a1> <blocks>
				xmlWriter.WriteStartElement("id-0001"); // open <refpages> <a1> <blocks> <id-0001>
				WriteBlockFormatting(xmlWriter);
				xmlWriter.WriteStartElement("p");
				xmlWriter.WriteString("The following Spells are able to be found in " + moduleModel.Name + ".");
				xmlWriter.WriteStartElement("linklist");
				FatSpellList.Sort((spellOne, spellTwo) => spellOne.SpellName.CompareTo(spellTwo.SpellName));
				foreach (SpellModel spellModel in FatSpellList)
				{
					xmlWriter.WriteStartElement("link");
					xmlWriter.WriteAttributeString("class", "power");
					xmlWriter.WriteAttributeString("recordname", WriteRecordNameSpell(spellModel));
					xmlWriter.WriteString(spellModel.SpellName);
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndElement(); // close </linklist>
				xmlWriter.WriteEndElement(); // close </p>
				xmlWriter.WriteEndElement(); // close </text>
				xmlWriter.WriteEndElement(); // close </id-0001>
				xmlWriter.WriteEndElement(); // close </blocks>
				WriteListLink(xmlWriter);
				xmlWriter.WriteStartElement("name"); // open <name>
				xmlWriter.WriteAttributeString("type", "string"); // <name type=string>
				xmlWriter.WriteString(moduleModel.Name + " Spells"); // <name type=string> * Spells
				xmlWriter.WriteEndElement(); // close </name>
				xmlWriter.WriteEndElement(); // close </id-0001>
				FatSpellList.Sort((spellOne, spellTwo) => spellOne.SpellName.CompareTo(spellTwo.SpellName));
				int spellID = 2;
				foreach (SpellModel spellModel in FatSpellList)
				{
					NPCController npcController = new NPCController();
					xmlWriter.WriteStartElement("id-" + spellID.ToString("D4")); // <open id-****>
					xmlWriter.WriteStartElement("blocks"); // <npc_name> <blocks>
					xmlWriter.WriteStartElement("id-0001"); // <npc_name> <blocks> <id-0001>
					WriteBlockFormatting(xmlWriter);
					xmlWriter.WriteRaw("<p><h>"); // <p><h>
					xmlWriter.WriteString(spellModel.SpellName);
					xmlWriter.WriteRaw("</h></p><p><i>"); // </h></p><p><i>
					if (spellModel.SpellLevel.GetDescription().Equals("cantrip"))
					{
						xmlWriter.WriteString(spellModel.SpellSchool + " cantrip");
					}
					else
					{
						xmlWriter.WriteString(spellModel.SpellLevel.GetDescription() + "-level " + spellModel.SpellSchool);
						if (spellModel.IsRitual.Equals(1))
						{
							xmlWriter.WriteString(" (ritual)");
						}
					}
					xmlWriter.WriteRaw("</i></p><p><b>");
					xmlWriter.WriteString("Casting Time:");
					xmlWriter.WriteRaw("</b>");
					xmlWriter.WriteString(" " + spellModel.CastingTime + " " + spellModel.CastingType.GetDescription());
					if (spellModel.CastingTime > 1)
					{
						xmlWriter.WriteString("s");
					}
					xmlWriter.WriteRaw("</p><p><b>");
					xmlWriter.WriteString("Range:");
					xmlWriter.WriteRaw("</b>");
					xmlWriter.WriteString(" " + spellModel.RangeDescription);
					xmlWriter.WriteRaw("</p><p><b>");
					xmlWriter.WriteString("Components:");
					xmlWriter.WriteRaw("</b>");
					xmlWriter.WriteString(" " + spellModel.ComponentDescription);
					xmlWriter.WriteRaw("</p><p><b>");
					xmlWriter.WriteString("Duration:");
					xmlWriter.WriteRaw("</b>");
					xmlWriter.WriteString(" " + spellModel.DurationText);
					xmlWriter.WriteRaw("</p>");
					xmlWriter.WriteRaw(npcController.GenerateFantasyGroundsDescriptionXML(spellModel.Description) + "<p>");
					xmlWriter.WriteStartElement("link");  // <link>
					xmlWriter.WriteAttributeString("class", "power"); // <link class="power">
					xmlWriter.WriteAttributeString("recordname", WriteRecordNameSpell(spellModel)); // <link class="power" recordname="reference.spelldata.*">
					xmlWriter.WriteRaw("<b>"); // <b>
					xmlWriter.WriteString("Spell:");
					xmlWriter.WriteRaw("</b>"); // </b>
					xmlWriter.WriteEndElement(); // </link>
					xmlWriter.WriteString(spellModel.SpellName);
					xmlWriter.WriteRaw("</p>"); // </p>
					xmlWriter.WriteEndElement(); // </text>
					xmlWriter.WriteEndElement(); // </id-0001>
					xmlWriter.WriteEndElement(); // </blocks>
					WriteListLink(xmlWriter);
					xmlWriter.WriteStartElement("name"); // open <name>
					xmlWriter.WriteAttributeString("type", "string"); // <name type=string>
					xmlWriter.WriteString(spellModel.SpellName); // <name type=string> NPC Name
					xmlWriter.WriteEndElement(); // close </name>
					xmlWriter.WriteEndElement(); // </id-****>
					spellID = ++spellID;
				}
				xmlWriter.WriteEndElement(); // </subchapter_**>
				subchapterID = ++subchapterID;
			}

			if (moduleModel.IncludesEquipment)
			{
				xmlWriter.WriteStartElement("subchapter_" + subchapterID.ToString("D2")); // open <subchapters> <subchapter_**>
				xmlWriter.WriteStartElement("name"); // open <subchapters> <subchapter_*> <name>
				xmlWriter.WriteAttributeString("type", "string"); // <name type=string>
				xmlWriter.WriteString("Equipment");
				xmlWriter.WriteEndElement(); // close </name>
				xmlWriter.WriteStartElement("refpages"); // open <refpages>
				xmlWriter.WriteStartElement("id-0001"); // open <refpages> <a1>
				xmlWriter.WriteStartElement("blocks"); // open <refpages> <a1> <blocks>
				xmlWriter.WriteStartElement("id-0001"); // open <refpages> <a1> <blocks> <id-0001>
				WriteBlockFormatting(xmlWriter);
				xmlWriter.WriteStartElement("p");
				xmlWriter.WriteString("The following Equipment are able to be found in " + moduleModel.Name + ".");
				xmlWriter.WriteStartElement("linklist");
				FatEquipmentList.Sort((equipOne, equipTwo) => equipOne.Name.CompareTo(equipTwo.Name));
				foreach (EquipmentModel equipmentModel in FatEquipmentList)
				{
					xmlWriter.WriteStartElement("link");
					xmlWriter.WriteAttributeString("class", "item");
					xmlWriter.WriteAttributeString("recordname", WriteRecordNameEquipment(equipmentModel));
					xmlWriter.WriteString(equipmentModel.Name);
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndElement(); // close </linklist>
				xmlWriter.WriteEndElement(); // close </p>
				xmlWriter.WriteEndElement(); // close </text>
				xmlWriter.WriteEndElement(); // close </id-0001>
				xmlWriter.WriteEndElement(); // close </blocks>
				WriteListLink(xmlWriter);
				xmlWriter.WriteStartElement("name"); // open <name>
				xmlWriter.WriteAttributeString("type", "string"); // <name type=string>
				xmlWriter.WriteString(moduleModel.Name + " Equipment"); // <name type=string> * Equipment
				xmlWriter.WriteEndElement(); // close </name>
				xmlWriter.WriteEndElement(); // close </id-0001>
				FatEquipmentList.Sort((equipOne, equipTwo) => equipOne.Name.CompareTo(equipTwo.Name));
				int equipID = 2;
				foreach (EquipmentModel equipmentModel in FatEquipmentList)
				{
					NPCController npcController = new NPCController();
					xmlWriter.WriteStartElement("id-" + equipID.ToString("D4")); // <open id-****>
					xmlWriter.WriteStartElement("blocks"); // <npc_name> <blocks>
					xmlWriter.WriteStartElement("id-0001"); // <npc_name> <blocks> <id-0001>
					WriteBlockFormatting(xmlWriter);
					xmlWriter.WriteRaw("<p><h>"); // <p><h>
					xmlWriter.WriteString(equipmentModel.Name);
					xmlWriter.WriteRaw("</h></p><p><i>"); // </h></p><p><i>
					if (string.IsNullOrEmpty(equipmentModel.Description))
					{
					}
					else
					{
						xmlWriter.WriteRaw(npcController.GenerateFantasyGroundsDescriptionXML(equipmentModel.Description));
					}
					xmlWriter.WriteRaw("</i></p><p>");
					xmlWriter.WriteStartElement("link");  // <link>
					xmlWriter.WriteAttributeString("class", "item"); // <link class="power">
					xmlWriter.WriteAttributeString("recordname", WriteRecordNameEquipment(equipmentModel)); // <link class="power" recordname="reference.spelldata.*">
					xmlWriter.WriteRaw("<b>"); // <b>
					xmlWriter.WriteString("Item:");
					xmlWriter.WriteRaw("</b>"); // </b>
					xmlWriter.WriteEndElement(); // </link>
					xmlWriter.WriteString(equipmentModel.Name);
					xmlWriter.WriteRaw("</p>"); // </p>
					xmlWriter.WriteEndElement(); // </text>
					xmlWriter.WriteEndElement(); // </id-0001>
					xmlWriter.WriteEndElement(); // </blocks>
					WriteListLink(xmlWriter);
					xmlWriter.WriteStartElement("name"); // open <name>
					xmlWriter.WriteAttributeString("type", "string"); // <name type=string>
					xmlWriter.WriteString(equipmentModel.Name); // <name type=string> NPC Name
					xmlWriter.WriteEndElement(); // close </name>
					xmlWriter.WriteEndElement(); // </id-****>
					equipID = ++equipID;
				}
				xmlWriter.WriteEndElement(); // </subchapter_**>
				subchapterID = ++subchapterID;
			}
		}

		private static void Subchapter_xx_NPCRefPages(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("refpages"); // open <refpages>
			xmlWriter.WriteStartElement("id-0001"); // open <refpages> <a1>
			xmlWriter.WriteStartElement("blocks"); // open <refpages> <a1> <blocks>
			xmlWriter.WriteStartElement("id-0001"); // open <refpages> <a1> <blocks> <id-0001>
			WriteBlockFormatting(xmlWriter);
			xmlWriter.WriteStartElement("p");
			xmlWriter.WriteString("The following NPCs are able to be found in " + moduleModel.Name + ".");
			xmlWriter.WriteStartElement("linklist");
			FatNPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
			foreach (NPCModel npcModel in FatNPCList)
			{
				xmlWriter.WriteStartElement("link");
				xmlWriter.WriteAttributeString("class", "npc");
				xmlWriter.WriteAttributeString("recordname", WriteRecordNameNPC(npcModel));
				xmlWriter.WriteString(npcModel.NPCName);
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement(); // close </linklist>
			xmlWriter.WriteEndElement(); // close </p>
			xmlWriter.WriteEndElement(); // close </text>
			xmlWriter.WriteEndElement(); // close </id-0001>
			xmlWriter.WriteEndElement(); // close </blocks>
			WriteListLink(xmlWriter);
			xmlWriter.WriteStartElement("name"); // open <name>
			xmlWriter.WriteAttributeString("type", "string"); // <name type=string>
			xmlWriter.WriteString(moduleModel.Name + " NPCs"); // <name type=string> * NPCs
			xmlWriter.WriteEndElement(); // close </name>
			xmlWriter.WriteEndElement(); // close </id-0001>
			FatNPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
			int npcID = 2;
			foreach (NPCModel npcModel in FatNPCList)
			{
				NPCController npcController = new NPCController();
				xmlWriter.WriteStartElement("id-" + npcID.ToString("D4")); // <open id-****>
				xmlWriter.WriteStartElement("blocks"); // <npc_name> <blocks>
				xmlWriter.WriteStartElement("id-0001"); // <npc_name> <blocks> <id-0001>
				WriteBlockFormatting(xmlWriter);
				if (!string.IsNullOrEmpty(npcModel.Description) && npcModel.Description.Length > 2)
				{
					xmlWriter.WriteRaw(npcController.GenerateFantasyGroundsDescriptionXML(npcModel.Description));
				}
				else
				{
					xmlWriter.WriteStartElement("p"); // <p>
					xmlWriter.WriteEndElement(); // </p>
				}
				if (npcModel.NPCImage.Length > 2)
				{
					xmlWriter.WriteStartElement("link");  // <link>
					xmlWriter.WriteAttributeString("class", "imagewindow"); // <link class="imagewindow">
					xmlWriter.WriteAttributeString("recordname", WriteImageXML(npcModel)); // <link class="imagewindow" recordname="image.*">
					xmlWriter.WriteStartElement("b"); // <b>
					xmlWriter.WriteString("Image:");
					xmlWriter.WriteEndElement(); // </b>
					xmlWriter.WriteEndElement(); // </link>
					xmlWriter.WriteString(npcModel.NPCName);
				}
				xmlWriter.WriteStartElement("link");  // <link>
				xmlWriter.WriteAttributeString("class", "npc"); // <link class="npc">
				xmlWriter.WriteAttributeString("recordname", WriteRecordNameNPC(npcModel)); // <link class="npc" recordname="reference.npcdata.*">
				xmlWriter.WriteStartElement("b"); // <b>
				xmlWriter.WriteString("NPC:");
				xmlWriter.WriteEndElement(); // </b>
				xmlWriter.WriteEndElement(); // </link>
				xmlWriter.WriteString(npcModel.NPCName);
				xmlWriter.WriteEndElement(); // </text>
				xmlWriter.WriteEndElement(); // </id-0001>
				xmlWriter.WriteEndElement(); // </blocks>
				WriteListLink(xmlWriter);
				xmlWriter.WriteStartElement("name"); // open <name>
				xmlWriter.WriteAttributeString("type", "string"); // <name type=string>
				xmlWriter.WriteString(npcModel.NPCName); // <name type=string> NPC Name
				xmlWriter.WriteEndElement(); // close </name>
				xmlWriter.WriteEndElement(); // </id-****>
				npcID = ++npcID;
			}
			xmlWriter.WriteEndElement(); // close </refpages>
		}

		private static void Xml_Name_ModuleName(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(module.Name);
			xmlWriter.WriteEndElement();
		}
	}
}
