using FantasyModuleParser.Extensions;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.Spells.Models;
using FantasyModuleParser.Spells.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	class SpellExporter
	{
		public static void DatabaseXML_Root_Spell(XmlWriter xmlWriter, ModuleModel module)
		{
			if (module.IncludeSpells)
			{
				xmlWriter.WriteStartElement("spell");
				Spelldata_Category(xmlWriter, module);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Spelldata_Category(XmlWriter xmlWriter, ModuleModel module)
		{
			List<SpellModel> FatSpellList = CommonMethods.GenerateFatSpellList(module);
			FatSpellList.Sort((spellOne, spellTwo) => spellOne.SpellName.CompareTo(spellTwo.SpellName));
			foreach (CategoryModel categoryModel in module.Categories)
			{
				xmlWriter.WriteStartElement("category");
				xmlWriter.WriteAttributeString("name", categoryModel.Name);
				xmlWriter.WriteAttributeString("baseicon", "0");
				xmlWriter.WriteAttributeString("decalicon", "0");
				Spelldata_Category_SpellName(xmlWriter, FatSpellList);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Spelldata_Category_SpellName(XmlWriter xmlWriter, List<SpellModel> FatSpellList)
		{			
			int spellID = 1;
			foreach (SpellModel spellModel in FatSpellList)
			{
				xmlWriter.WriteStartElement(SpellNameToXMLFormat(spellModel));
				/* To Do: Create SpellLocked method */
				CommonMethods.WriteModuleLocked(xmlWriter);
				WriteSpellName(xmlWriter, spellModel);
				WriteSpellDescription(xmlWriter, spellModel);
				WriteSpellLevel(xmlWriter, spellModel);
				WriteSpellSchool(xmlWriter, spellModel);
				WriteSpellRitual(xmlWriter, spellModel);
				WriteSpellSource(xmlWriter, spellModel);
				WriteCastingTime(xmlWriter, spellModel);
				WriteSpellRange(xmlWriter, spellModel);
				WriteSpellDuration(xmlWriter, spellModel);
				WriteSpellComponents(xmlWriter, spellModel);
				xmlWriter.WriteEndElement();
				spellID++;
			}
		}

		public static void DatabaseXML_Root_Lists_Spelllists(XmlWriter xmlWriter, ModuleModel module)
		{
			if (module.IncludeSpells)
			{
				#region Spell Lists
				xmlWriter.WriteStartElement("spelllists");
				Lists_SpellLists_Spells(xmlWriter, module);
				xmlWriter.WriteEndElement();
				#endregion
			}
		}

		private static void Lists_SpellLists_Spells(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("spells");
			Xml_SpellLists_Spells_Name(xmlWriter);
			SpellLists_Spells_Index(xmlWriter, module);
			Spells_Index(xmlWriter, module);
			SpellListByClass(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void Spells_Index(XmlWriter xmlWriter, ModuleModel module)
		{
			List<SpellModel> FatSpellList = CommonMethods.GenerateFatSpellList(module);
			FatSpellList.Sort((spellOne, spellTwo) => spellOne.SpellName.CompareTo(spellTwo.SpellName));

			xmlWriter.WriteStartElement("_index_");
			Xml_Description_SpellIndex(xmlWriter);
			Spells__Index_Groups(xmlWriter, module, FatSpellList);
			xmlWriter.WriteEndElement();
		}

		private static void Spells__Index_Groups(XmlWriter xmlWriter, ModuleModel module, List<SpellModel> FatSpellList)
		{
			xmlWriter.WriteStartElement("groups");
			CreateSpellReferenceByFirstLetter(xmlWriter, module, FatSpellList);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Description_SpellIndex(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("Spell Index");
			xmlWriter.WriteEndElement();
		}

		private static void SpellLists_Spells_Index(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("index");
			CommonMethods.WriteIDLinkList(xmlWriter, module, "id-0001", "lists.spelllists._index_@" + module.Name, "(Index)");
			int spellListId = 2;
			foreach (string castByValue in GetSortedSpellCasterList(module))
			{
				string referenceId = "lists.spellists.";
				referenceId += castByValue.Replace(" ", "").Replace("(", "").Replace(")", "").ToLower();
				referenceId += "@" + module.Name;
				CommonMethods.WriteIDLinkList(xmlWriter, module, "id-" + spellListId.ToString("D4"), referenceId, castByValue);
				spellListId++;
			}
			xmlWriter.WriteEndElement();
		}

		private static void Xml_SpellLists_Spells_Name(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("Spells");
			xmlWriter.WriteEndElement();
		}

		static public void SpellListByClass(XmlWriter xmlWriter, ModuleModel module)
		{
			List<SpellModel> SpellList = GetFatSpellModelList(module);
			SpellList.Sort((spellOne, spellTwo) => spellOne.SpellName.CompareTo(spellTwo.SpellName));
			foreach (string castByValue in GetSortedSpellCasterList(module))
			{
				xmlWriter.WriteStartElement(castByValue.ToLower().Replace("(", "").Replace(")", "").Replace(" ", ""));
				CastyBy_Description(xmlWriter, castByValue);
				CastBy_Groups(xmlWriter, module, SpellList, castByValue);
				xmlWriter.WriteEndElement();
			}
		}

		private static void CastBy_Groups(XmlWriter xmlWriter, ModuleModel moduleModel, List<SpellModel> SpellList, string castByValue)
		{
			xmlWriter.WriteStartElement("groups");
			SpellList.Sort((spellOne, spellTwo) => spellOne.SpellLevel.CompareTo(spellTwo.SpellLevel));
			var LevelList = SpellList.GroupBy(x => (int)x.SpellLevel).Select(x => x.ToList()).ToList();

			foreach (SpellModel spellModel in SpellList)
			{
				if (spellModel.CastBy.Contains(castByValue))
				{
					CastBy_Groups_Level(xmlWriter, moduleModel, castByValue, LevelList);
				}
			}
			xmlWriter.WriteEndElement();
		}

		private static void CastBy_Groups_Level(XmlWriter xmlWriter, ModuleModel moduleModel, string castByValue, List<List<SpellModel>> LevelList)
		{
			foreach (List<SpellModel> levelList in LevelList)
			{
				xmlWriter.WriteStartElement("level" + (int)levelList[0].SpellLevel);
				Description_SpellLevel(xmlWriter, levelList);
				CastBy_Groups_Level_Index(xmlWriter, moduleModel, castByValue, levelList);
				xmlWriter.WriteEndElement();
			}
		}

		private static void CastBy_Groups_Level_Index(XmlWriter xmlWriter, ModuleModel moduleModel, string castByValue, List<SpellModel> levelList)
		{
			xmlWriter.WriteStartElement("index");
			CastBy_Groups_Level_Index_SpellName(xmlWriter, moduleModel, castByValue, levelList);
			xmlWriter.WriteEndElement();
		}

		private static void CastBy_Groups_Level_Index_SpellName(XmlWriter xmlWriter, ModuleModel moduleModel, string castByValue, List<SpellModel> levelList)
		{
			foreach (SpellModel spell in levelList)
			{
				xmlWriter.WriteStartElement(spell.SpellName.ToLower().Replace(" ", "").Replace("'", ""));
				SpellName_Link(xmlWriter, moduleModel, spell);
				SpellName_Source(xmlWriter, castByValue);
				xmlWriter.WriteEndElement();
			}
		}

		private static void SpellName_Source(XmlWriter xmlWriter, string castByValue)
		{
			xmlWriter.WriteStartElement("source");
			xmlWriter.WriteString("Class " + castByValue);
			xmlWriter.WriteEndElement();
		}

		private static void SpellName_Link(XmlWriter xmlWriter, ModuleModel moduleModel, SpellModel spell)
		{
			xmlWriter.WriteStartElement("link");
			xmlWriter.WriteAttributeString("type", "windowreference");
			SpellName_Link_Class(xmlWriter);
			SpellName_Link_RecordName(xmlWriter, moduleModel, spell);
			CommonMethods.Xml_Description_Field_Name(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void SpellName_Link_RecordName(XmlWriter xmlWriter, ModuleModel moduleModel, SpellModel spell)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("spell." + SpellNameToXMLFormat(spell) + "@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void SpellName_Link_Class(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("class");
			xmlWriter.WriteString("reference_spell");
			xmlWriter.WriteEndElement();
		}

		private static void Description_SpellLevel(XmlWriter xmlWriter, List<SpellModel> levelList)
		{
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "string");
			if (levelList[0].SpellLevel == Spells.Enums.SpellLevel.Cantrip)
			{
				xmlWriter.WriteString("Cantrips");
			}
			else
			{
				xmlWriter.WriteString("Level " + (int)levelList[0].SpellLevel + " Spells");
			}
			xmlWriter.WriteEndElement();
		}

		private static void CastyBy_Description(XmlWriter xmlWriter, string castByValue)
		{
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(castByValue + " Spells");
			xmlWriter.WriteEndElement();
		}

		private static void SpellLocation(XmlWriter xmlWriter, ModuleModel moduleModel, List<SpellModel> SpellList)
		{
			foreach (SpellModel spell in SpellList)
			{
				xmlWriter.WriteStartElement(SpellNameToXMLFormat(spell));
				SpellName_Link(xmlWriter, moduleModel, spell);
				CommonMethods.Xml_Source_TypeNumber_Blank(xmlWriter);
				xmlWriter.WriteEndElement();
			}
		}

		private static void CreateSpellReferenceByFirstLetter(XmlWriter xmlWriter, ModuleModel module, List<SpellModel> SpellList)
		{
			SpellList.Sort((spellOne, spellTwo) => spellOne.SpellName.CompareTo(spellTwo.SpellName));
			var AlphabetList = SpellList.GroupBy(x => x.SpellName.ToUpper()[0]).Select(x => x.ToList()).ToList();
			foreach (List<SpellModel> spellList in AlphabetList)
			{
				string actualLetter = spellList[0].SpellName[0] + "";
				ProcessSpellListByLetter(xmlWriter, module, actualLetter, spellList);
			}
		}
		
		private static void ProcessSpellListByLetter(XmlWriter xmlWriter, ModuleModel module, string actualLetter, List<SpellModel> SpellList)
		{
			xmlWriter.WriteStartElement("typeletter" + actualLetter);
			CommonMethods.Xml_Description_ActualLetter(xmlWriter, actualLetter);
			Xml_Index_SpellLocation(xmlWriter, module, SpellList);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Index_SpellLocation(XmlWriter xmlWriter, ModuleModel module, List<SpellModel> SpellList)
		{
			xmlWriter.WriteStartElement("index");
			SpellLocation(xmlWriter, module, SpellList);
			xmlWriter.WriteEndElement();
		}

		private static HashSet<string> GenerateSpellCasterList(ModuleModel module)
		{
			HashSet<string> casterList = new HashSet<string>();

			if (module != null && module.Categories != null)
			{
				foreach (CategoryModel category in module.Categories)
				{
					if (category.SpellModels != null)
					{
						foreach (SpellModel spell in category.SpellModels)
						{
							if (!string.IsNullOrWhiteSpace(spell.CastBy))
							{
								foreach (string castByValue in spell.CastBy.Split(','))
								{
									casterList.Add(castByValue.Trim());
								}
							}
						}
					}
				}
			}
			return casterList;
		}
		
		private static IEnumerable<string> GetSortedSpellCasterList(ModuleModel module)
		{
			return GenerateSpellCasterList(module).OrderBy(item => item);
		}
		
		static public List<SpellModel> GetFatSpellModelList(ModuleModel module)
		{
			List<SpellModel> spells = new List<SpellModel>();
			if (module.Categories == null)
			{
				return spells;
			}
			foreach (CategoryModel category in module.Categories)
			{
				if (category.SpellModels == null)
				{
					return spells;
				}
				foreach (SpellModel spell in category.SpellModels)
				{
					spells.Add(spell);
				}
			}
			return spells;
		}
		
		static public string SpellNameToXMLFormat(SpellModel spellModel)
		{
			string name = spellModel.SpellName.ToLower();
			return name.Replace(" ", "_").Replace(",", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace("&", "");
		}

		static public void WriteSpellName(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("name"); /* <name> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(spellModel.SpellName);
			xmlWriter.WriteEndElement(); /* <name> </name> */
		}
		static public void WriteSpellDescription(XmlWriter xmlWriter, SpellModel spellModel)
		{
			NPCController npcController = new NPCController();
			xmlWriter.WriteStartElement("description"); /* <description> */
			xmlWriter.WriteAttributeString("type", "formattedtext");
			xmlWriter.WriteRaw(npcController.GenerateFantasyGroundsDescriptionXML(spellModel.Description));
			xmlWriter.WriteEndElement(); /* <description> </description> */
		}
		static public void WriteSpellLevel(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("level"); /* <level> */
			xmlWriter.WriteAttributeString("type", "number");
			if (spellModel.SpellLevel.GetDescription().Equals("cantrip"))
			{
				xmlWriter.WriteString("0");
			}
			else
			{
				xmlWriter.WriteValue((int)spellModel.SpellLevel);
			}
			xmlWriter.WriteEndElement(); /* <level </level> */
		}
		static public void WriteSpellSchool(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("school"); /* <school> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(spellModel.SpellSchool.GetDescription());
			xmlWriter.WriteEndElement(); /* <school> </school> */
		}
		static public void WriteSpellSource(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("source"); /* <source> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(spellModel.CastBy);
			xmlWriter.WriteEndElement(); /* <source> </source> */
		}
		static public void WriteSpellRange(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("range"); /* <range> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(spellModel.RangeDescription);
			xmlWriter.WriteEndElement(); /* <range> </range> */
		}
		static public void WriteCastingTime(XmlWriter xmlWriter, SpellModel spellModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(spellModel.CastingTime + " " + spellModel.CastingType.GetDescription());

			xmlWriter.WriteStartElement("castingtime"); /* <castingtime> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(stringBuilder.ToString());
			xmlWriter.WriteEndElement(); /* <castingtime> </castingtime> */
		}
		static public void WriteSpellDuration(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("duration"); /* <duration> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(spellModel.DurationText);
			xmlWriter.WriteEndElement(); /* <duration> </duration> */
		}
		static public void WriteSpellComponents(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("components"); /* <components> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(SpellViewModel.GenerateComponentDescription(spellModel));
			xmlWriter.WriteEndElement(); /* <components> </components> */
		}
		static public void WriteSpellRitual(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("ritual"); /* <ritual> */
			xmlWriter.WriteAttributeString("type", "number");
			if (spellModel.IsRitual)
			{
				xmlWriter.WriteString("1");
			}
			else
			{
				xmlWriter.WriteString("0");
			}
			xmlWriter.WriteEndElement(); /* <ritual> </ritual> */
		}
	}
}
