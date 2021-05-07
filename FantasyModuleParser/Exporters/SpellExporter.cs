using FantasyModuleParser.Extensions;
using FantasyModuleParser.Main.Models;
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
				xmlWriter.WriteStartElement(castByValue.ToLower().Replace("(", "").Replace(")", "").Replace(" ", ""));  // <castby>
				xmlWriter.WriteStartElement("description"); // <castby> <description>
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(castByValue + " Spells");
				xmlWriter.WriteEndElement(); // <castby> </description>
				xmlWriter.WriteStartElement("groups"); // <castby> <groups>
				SpellList.Sort((spellOne, spellTwo) => spellOne.SpellLevel.CompareTo(spellTwo.SpellLevel));
				var LevelList = SpellList.GroupBy(x => (int)x.SpellLevel).Select(x => x.ToList()).ToList();

				foreach (SpellModel spellModel in SpellList)
				{
					if (spellModel.CastBy.Contains(castByValue))
					{
						foreach (List<SpellModel> levelList in LevelList)
						{
							xmlWriter.WriteStartElement("level" + (int)levelList[0].SpellLevel); // <castby> <groups> <level#>
							xmlWriter.WriteStartElement("description"); // <castby> <groups> <level#> <description>
							xmlWriter.WriteAttributeString("type", "string");
							if (levelList[0].SpellLevel == Spells.Enums.SpellLevel.Cantrip)
							{
								xmlWriter.WriteString("Cantrips");
							}
							else
							{
								xmlWriter.WriteString("Level " + (int)levelList[0].SpellLevel + " Spells");
							}
							xmlWriter.WriteEndElement(); // <castby> <groups> <level#> </description>
							xmlWriter.WriteStartElement("index");   // <castby> <groups> <level#> <index>
							foreach (SpellModel spellLevelList in levelList)
							{
								xmlWriter.WriteStartElement(spellLevelList.SpellName.ToLower().Replace(" ", "").Replace("'", ""));  // <spellname>
								xmlWriter.WriteStartElement("link"); // <spellname> <link>
								xmlWriter.WriteAttributeString("type", "windowreference");
								xmlWriter.WriteStartElement("class");   // <spellname> <link> <class>
								xmlWriter.WriteString("reference_spell");
								xmlWriter.WriteEndElement(); // <spellname> <link> </class>
								xmlWriter.WriteStartElement("recordname"); // <spellname> <link> <recordname>
								if (moduleModel.IsLockedRecords)
								{
									xmlWriter.WriteString("reference.spelldata." + spellLevelList.SpellName.ToLower().Replace(" ", "").Replace("'", "") + "@" + moduleModel.Name);
								}
								else
								{
									xmlWriter.WriteString("reference.spelldata." + spellLevelList.SpellName.ToLower().Replace(" ", "").Replace("'", ""));
								}
								xmlWriter.WriteEndElement(); // <spellname> <link> </recordname>
								xmlWriter.WriteStartElement("description"); // <spellname> <link> <description>
								xmlWriter.WriteStartElement("field"); // <spellname> <link> <description> <field>
								xmlWriter.WriteString("name");
								xmlWriter.WriteEndElement(); // <spellname> <link> <description> </field>
								xmlWriter.WriteEndElement(); // <spellname> <link> </description>
								xmlWriter.WriteEndElement(); // <spellname> </link>
								xmlWriter.WriteStartElement("source"); // <spellname> <source>
								xmlWriter.WriteString("Class " + castByValue);
								xmlWriter.WriteEndElement(); // <spellname> </source>
								xmlWriter.WriteEndElement(); // </spellname>
							}
							xmlWriter.WriteEndElement();  // <castby> <groups> <level#> </index>
							xmlWriter.WriteEndElement();  // <castby> <groups> </level#>
						}
					}
				}
				xmlWriter.WriteEndElement();  // <castby> </groups>
				xmlWriter.WriteEndElement();  // </castby>
			}
		}
		static public void SpellLocation(XmlWriter xmlWriter, ModuleModel moduleModel, List<SpellModel> SpellList)
		{
			foreach (SpellModel spell in SpellList)
			{
				xmlWriter.WriteStartElement(SpellNameToXMLFormat(spell));
				xmlWriter.WriteStartElement("link");
				xmlWriter.WriteAttributeString("type", "windowreference");
				xmlWriter.WriteStartElement("class");
				xmlWriter.WriteString("reference_spell");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("recordname");
				if (moduleModel.IsLockedRecords)
				{
					xmlWriter.WriteString("reference.spelldata." + SpellNameToXMLFormat(spell) + '@' + moduleModel.Name);
				}
				else
				{
					xmlWriter.WriteString("reference.spelldata." + SpellNameToXMLFormat(spell));
				}
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("description");
				xmlWriter.WriteStartElement("field");
				xmlWriter.WriteString("name");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("source");
				xmlWriter.WriteAttributeString("type", "number");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
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
			xmlWriter.WriteStartElement("typeletter" + actualLetter);
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(actualLetter);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("index");
			SpellLocation(xmlWriter, moduleModel, SpellList);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
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
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(spellModel.SpellName);
			xmlWriter.WriteEndElement();
		}
		static public void WriteSpellDescription(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "formattedtext");
			xmlWriter.WriteString(spellModel.Description);
			xmlWriter.WriteEndElement();
		}
		static public void WriteSpellLevel(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("level");
			xmlWriter.WriteAttributeString("type", "number");
			if (spellModel.SpellLevel.GetDescription().Equals("cantrip"))
			{
				xmlWriter.WriteString("0");
			}
			else
			{
				xmlWriter.WriteValue((int)spellModel.SpellLevel);
			}
			xmlWriter.WriteEndElement();
		}
		static public void WriteSpellSchool(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("school");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(spellModel.SpellSchool.GetDescription());
			xmlWriter.WriteEndElement();
		}
		static public void WriteSpellSource(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("source");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(spellModel.CastBy);
			xmlWriter.WriteEndElement();
		}
		static public void WriteSpellRange(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("range");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(spellModel.RangeDescription);
			xmlWriter.WriteEndElement();
		}
		static public void WriteCastingTime(XmlWriter xmlWriter, SpellModel spellModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(spellModel.CastingTime + " " + spellModel.CastingType.GetDescription());

			xmlWriter.WriteStartElement("castingtime");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(stringBuilder.ToString());
			xmlWriter.WriteEndElement();
		}
		static public void WriteSpellDuration(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("duration");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(spellModel.DurationText);
			xmlWriter.WriteEndElement();
		}
		static public void WriteSpellComponents(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("components");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(SpellViewModel.GenerateComponentDescription(spellModel));
			xmlWriter.WriteEndElement();
		}
		static public void WriteSpellRitual(XmlWriter xmlWriter, SpellModel spellModel)
		{
			xmlWriter.WriteStartElement("ritual");
			xmlWriter.WriteAttributeString("type", "number");
			if (spellModel.IsRitual)
			{
				xmlWriter.WriteString("1");
			}
			else
			{
				xmlWriter.WriteString("0");
			}
			xmlWriter.WriteEndElement();
		}
	}
}
