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
		static public void SpellListByClass(XmlWriter xmlWriter, ModuleModel moduleModel)
		{
			List<SpellModel> SpellList = GetFatSpellModelList(moduleModel);
			SpellList.Sort((spellOne, spellTwo) => spellOne.SpellName.CompareTo(spellTwo.SpellName));
			//var AlphabetList = SpellList.GroupBy(x => x.SpellName.ToUpper()[0]).Select(x => x.ToList()).ToList();
			foreach (string castByValue in GetSortedSpellCasterList(moduleModel))
			{
				xmlWriter.WriteStartElement(castByValue.ToLower().Replace("(", "").Replace(")", "").Replace(" ", ""));  /* <castby> */
				xmlWriter.WriteStartElement("description"); /* <castby> <description> */
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(castByValue + " Spells");
				xmlWriter.WriteEndElement(); /* <castby> <description> </description> */
				xmlWriter.WriteStartElement("groups"); /* <castby> <groups> */
				SpellList.Sort((spellOne, spellTwo) => spellOne.SpellLevel.CompareTo(spellTwo.SpellLevel));
				var LevelList = SpellList.GroupBy(x => (int)x.SpellLevel).Select(x => x.ToList()).ToList();

				foreach (SpellModel spellModel in SpellList)
				{
					if (spellModel.CastBy.Contains(castByValue))
					{
						foreach (List<SpellModel> levelList in LevelList)
						{
							xmlWriter.WriteStartElement("level" + (int)levelList[0].SpellLevel); /* <castby> <groups> <level#> */
							xmlWriter.WriteStartElement("description"); /* <castby> <groups> <level#> <description> */
							xmlWriter.WriteAttributeString("type", "string");
							if (levelList[0].SpellLevel == Spells.Enums.SpellLevel.Cantrip)
							{
								xmlWriter.WriteString("Cantrips");
							}
							else
							{
								xmlWriter.WriteString("Level " + (int)levelList[0].SpellLevel + " Spells");
							}
							xmlWriter.WriteEndElement(); /* <castby> <groups> <level#> <description> </description> */
							xmlWriter.WriteStartElement("index");   /* <castby> <groups> <level#> <index> */
							foreach (SpellModel spellLevelList in levelList)
							{
								xmlWriter.WriteStartElement(spellLevelList.SpellName.ToLower().Replace(" ", "").Replace("'", ""));
								/* <castby> <groups> <level#> <index> <spellname> */
								xmlWriter.WriteStartElement("link"); /* <castby> <groups> <level#> <index> <spellname> <link> */
								xmlWriter.WriteAttributeString("type", "windowreference");
								xmlWriter.WriteStartElement("class");  /* <castby> <groups> <level#> <index> <spellname> <link> <class> */
								xmlWriter.WriteString("reference_spell");
								xmlWriter.WriteEndElement(); /* <castby> <groups> <level#> <index> <spellname> <link> <class> </class> */
								xmlWriter.WriteStartElement("recordname"); /* <castby> <groups> <level#> <index> <spellname> <link> <recordname> */
								if (moduleModel.IsLockedRecords)
								{
									xmlWriter.WriteString("reference.spelldata." + spellLevelList.SpellName.ToLower().Replace(" ", "").Replace("'", "") + "@" + moduleModel.Name);
								}
								else
								{
									xmlWriter.WriteString("reference.spelldata." + spellLevelList.SpellName.ToLower().Replace(" ", "").Replace("'", ""));
								}
								xmlWriter.WriteEndElement(); /* <castby> <groups> <level#> <index> <spellname> <link> <recordname> </recordname> */
								xmlWriter.WriteStartElement("description"); /* <castby> <groups> <level#> <index> <spellname> <link> <description> */
								xmlWriter.WriteStartElement("field"); /* <castby> <groups> <level#> <index> <spellname> <link> <description> <field> */
								xmlWriter.WriteString("name");
								xmlWriter.WriteEndElement(); /* <castby> <groups> <level#> <index> <spellname> <link> <description> <field> </field> */
								xmlWriter.WriteEndElement(); /* <castby> <groups> <level#> <index> <spellname> <link> <description> </description> */
								xmlWriter.WriteEndElement(); /* <castby> <groups> <level#> <index> <spellname> <link> </link> */
								xmlWriter.WriteStartElement("source"); /* <castby> <groups> <level#> <index> <spellname> <source> */
								xmlWriter.WriteString("Class " + castByValue);
								xmlWriter.WriteEndElement(); /* <castby> <groups> <level#> <index> <spellname> <source> </source> */
								xmlWriter.WriteEndElement(); /* <castby> <groups> <level#> <index> <spellname> </spellname> */
							}
							xmlWriter.WriteEndElement();  /* <castby> <groups> <level#> <index> </index> */
							xmlWriter.WriteEndElement();  /* <castby> <groups> <level#> </level#> */
						}
					}
				}
				xmlWriter.WriteEndElement();  /* <castby> <groups> </groups> */
				xmlWriter.WriteEndElement();  /* <castby> </castby> */
			}
		}
		static public void SpellLocation(XmlWriter xmlWriter, ModuleModel moduleModel, List<SpellModel> SpellList)
		{
			foreach (SpellModel spell in SpellList)
			{
				xmlWriter.WriteStartElement(SpellNameToXMLFormat(spell)); /* <spellname> */
				xmlWriter.WriteStartElement("link"); /* <spellname> <link> */
				xmlWriter.WriteAttributeString("type", "windowreference");
				xmlWriter.WriteStartElement("class"); /* <spellname> <link> <class> */
				xmlWriter.WriteString("reference_spell");
				xmlWriter.WriteEndElement(); /* <spellname> <link> <class> </class>*/
				xmlWriter.WriteStartElement("recordname"); /* <spellname> <link> <recordname> */
				if (moduleModel.IsLockedRecords)
				{
					xmlWriter.WriteString("reference.spelldata." + SpellNameToXMLFormat(spell) + '@' + moduleModel.Name);
				}
				else
				{
					xmlWriter.WriteString("reference.spelldata." + SpellNameToXMLFormat(spell));
				}
				xmlWriter.WriteEndElement(); /* <spellname>  <link> <recordname> </recordname> */
				xmlWriter.WriteStartElement("description"); /* <spellname> <link> <description> */
				xmlWriter.WriteStartElement("field"); /* <spellname> <link> <description> <field> */
				xmlWriter.WriteString("name");
				xmlWriter.WriteEndElement(); /* <spellname> <link> <description> <field> </field> */
				xmlWriter.WriteEndElement(); /* <spellname> <link> <description> </description> */
				xmlWriter.WriteEndElement(); /* <spellname> <link> </link> */
				xmlWriter.WriteStartElement("source"); /* <spellname> <source> */
				xmlWriter.WriteAttributeString("type", "number");
				xmlWriter.WriteEndElement(); /* <spellname> <source> </source> */
				xmlWriter.WriteEndElement(); /* <spellname> </spellname> */
			}
		}
		static public void CreateSpellReferenceByFirstLetter(XmlWriter xmlWriter, ModuleModel moduleModel, List<SpellModel> SpellList)
		{
			SpellList.Sort((spellOne, spellTwo) => spellOne.SpellName.CompareTo(spellTwo.SpellName));
			var AlphabetList = SpellList.GroupBy(x => x.SpellName.ToUpper()[0]).Select(x => x.ToList()).ToList();
			foreach (List<SpellModel> spellList in AlphabetList)
			{
				string actualLetter = spellList[0].SpellName[0] + "";
				ProcessSpellListByLetter(xmlWriter, moduleModel, actualLetter, spellList);
			}
		}
		static public void ProcessSpellListByLetter(XmlWriter xmlWriter, ModuleModel moduleModel, string actualLetter, List<SpellModel> SpellList)
		{
			xmlWriter.WriteStartElement("typeletter" + actualLetter); /* <typeletter_*> */
			xmlWriter.WriteStartElement("description"); /* <typeletter_*> <description> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(actualLetter);
			xmlWriter.WriteEndElement(); /* <typeletter_*> <description> </description> */
			xmlWriter.WriteStartElement("index"); /* <typeletter_*> <index> */
			SpellLocation(xmlWriter, moduleModel, SpellList); /* See Line 90 */
			xmlWriter.WriteEndElement(); /* <typeletter_*> <index> </index> */
			xmlWriter.WriteEndElement(); /* <typeletter_*> </typeletter_*> */
		}
		static public HashSet<string> GenerateSpellCasterList(ModuleModel moduleModel)
		{
			HashSet<string> casterList = new HashSet<string>();

			if (moduleModel != null && moduleModel.Categories != null)
			{
				foreach (CategoryModel categoryModel in moduleModel.Categories)
				{
					if (categoryModel.SpellModels != null)
					{
						foreach (SpellModel spellModel in categoryModel.SpellModels)
						{
							if (!String.IsNullOrWhiteSpace(spellModel.CastBy))
							{
								foreach (string castByValue in spellModel.CastBy.Split(','))
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
		static public IEnumerable<string> GetSortedSpellCasterList(ModuleModel moduleModel)
		{
			return GenerateSpellCasterList(moduleModel).OrderBy(item => item);
		}
		static public List<SpellModel> GetFatSpellModelList(ModuleModel moduleModel)
		{
			List<SpellModel> spellModels = new List<SpellModel>();
			if (moduleModel.Categories == null)
			{
				return spellModels;
			}
			foreach (CategoryModel categoryModel in moduleModel.Categories)
			{
				if (categoryModel.SpellModels == null)
				{
					return spellModels;
				}
				foreach (SpellModel spellModel in categoryModel.SpellModels)
				{
					spellModels.Add(spellModel);
				}
			}
			return spellModels;
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
