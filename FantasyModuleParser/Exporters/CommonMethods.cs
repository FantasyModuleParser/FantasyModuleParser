using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.Equipment.Models;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC;
using FantasyModuleParser.Spells.Models;
using FantasyModuleParser.Tables.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	public class CommonMethods
	{
		/// <summary>
		/// Generates a List of all NPCs across all Categories in one List<NPCModel> object.  Used for Reference Manual material.
		/// </summary>
		public static List<NPCModel> GenerateFatNPCList(ModuleModel moduleModel)
		{
			List<NPCModel> FatNPCList = new List<NPCModel>();

			foreach (CategoryModel category in moduleModel.Categories)
			{
				foreach (NPCModel npcModel in category.NPCModels)
				{
					FatNPCList.Add(npcModel);
				}
			}
			return FatNPCList;
		}

		/// <summary>
		/// Generates a List of all Spells across all Categories in one List<SpellModel> object. Used for Reference Manual material.
		/// </summary>
		public static List<SpellModel> GenerateFatSpellList(ModuleModel moduleModel)
		{
			List<SpellModel> FatSpellList = new List<SpellModel>();
			foreach (CategoryModel category in moduleModel.Categories)
			{
				foreach (SpellModel spellModel in category.SpellModels)
				{
					FatSpellList.Add(spellModel);
				}

			}
			return FatSpellList;
		}

		/// <summary>
		/// Generates a List of all Tables across all Categories in one List<TableModel> object. Used for Reference Manual material.
		/// </summary>
		public static List<TableModel> GenerateFatTableList(ModuleModel moduleModel)
		{
			List<TableModel> FatTableList = new List<TableModel>();
			foreach (CategoryModel category in moduleModel.Categories)
			{
				foreach (TableModel tableModel in category.TableModels)
				{
					FatTableList.Add(tableModel);
				}

			}
			return FatTableList;
		}

		/// <summary>
		/// Generates a List of all Equipment across all Categories in one List<EquipmentModel> object.  Used for Reference Manual material.
		/// </summary>
		public static List<EquipmentModel> GenerateFatEquipmentList(ModuleModel moduleModel)
		{
			List<EquipmentModel> FatEquipmentList = new List<EquipmentModel>();
			foreach (CategoryModel category in moduleModel.Categories)
			{
				foreach (EquipmentModel equipmentModel in category.EquipmentModels)
				{
					FatEquipmentList.Add(equipmentModel);
				}
			}
			IEnumerable<EquipmentModel> query = from equipment in FatEquipmentList
												orderby equipment.PrimaryEquipmentEnumType, equipment.Name
												select equipment;
			return query.ToList();
		}

		public static List<ClassModel> GenerateFatClassList(ModuleModel moduleModel)
		{
			List<ClassModel> FatClassList = new List<ClassModel>();
			foreach (CategoryModel category in moduleModel.Categories)
			{
				foreach (ClassModel classModel in category.ClassModels)
				{
					FatClassList.Add(classModel);
				}
			}
			IEnumerable<ClassModel> query = from classModel in FatClassList
												orderby classModel.Name
												select classModel;
			return query.ToList();
		}

		public static void WriteModuleLocked(XmlWriter xmlWriter)
		{
			ModuleModel moduleModel = new ModuleModel();
			string lockedRecords = "0";
			if (moduleModel.IsLockedRecords)
			{
				lockedRecords = "1";
			}
			xmlWriter.WriteStartElement("locked"); /* <locked> */
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteString(lockedRecords.ToString());
			xmlWriter.WriteEndElement(); /* <locked> </locked> */
		}

		public static string CategoryNameToXML(CategoryModel categoryModel)
		{
			string categoryName = categoryModel.Name;
			return categoryName.Replace(" ", "").Replace(",", "").Replace("-", "").Replace("'", "").ToLower();
		}

		public static void WriteIDLinkList(XmlWriter xmlWriter, ModuleModel moduleModel, string id, string listId, string listDescription)
		{
			xmlWriter.WriteStartElement(id);
			WriteIDLinkList_Linklist(xmlWriter, listId);
			WriteIDLinkList_Name(xmlWriter, listDescription);
			xmlWriter.WriteEndElement();
		}

		private static void WriteIDLinkList_Name(XmlWriter xmlWriter, string listDescription)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(listDescription);
			xmlWriter.WriteEndElement();
		}

		private static void WriteIDLinkList_Linklist(XmlWriter xmlWriter, string listId)
		{
			xmlWriter.WriteStartElement("listlink");
			xmlWriter.WriteAttributeString("type", "windowreference");
			WriteIDLinkList_Linklist_Class(xmlWriter);
			WriteIDLinkList_Linklist_Recordname(xmlWriter, listId);
			xmlWriter.WriteEndElement();
		}

		private static void WriteIDLinkList_Linklist_Recordname(XmlWriter xmlWriter, string listId)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString(listId);
			xmlWriter.WriteEndElement();
		}

		private static void WriteIDLinkList_Linklist_Class(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("class");
			xmlWriter.WriteString("reference_colindex");
			xmlWriter.WriteEndElement();
		}

		public static void Xml_Description_Field_Name(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("description");
			Xml_Field_Name(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Field_Name(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("field");
			xmlWriter.WriteString("name");
			xmlWriter.WriteEndElement();
		}

		public static void Xml_Source_TypeNumber_Blank(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("source");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteEndElement();
		}

		public static void Xml_Description_ActualLetter(XmlWriter xmlWriter, string actualLetter)
		{
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(actualLetter);
			xmlWriter.WriteEndElement();
		}
	
		public static void Xml_Name_Npcs(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("NPCs");
			xmlWriter.WriteEndElement();
		}

		public static void BaseIcon_DecalIcon(XmlWriter xmlWriter)
		{
			xmlWriter.WriteAttributeString("baseicon", "0");
			xmlWriter.WriteAttributeString("decalicon", "0");
		}
	}
}
