using FantasyModuleParser.Equipment.Models;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC;
using FantasyModuleParser.Spells.Models;
using FantasyModuleParser.Tables.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.Exporters
{
	class CommonMethods
	{
		/// <summary>
		/// Generates a List of all NPCs across all Categories in one List<NPCModel> object.  Used for Reference Manual material.
		/// </summary>
		static public List<NPCModel> GenerateFatNPCList(ModuleModel moduleModel)
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
		static public List<SpellModel> GenerateFatSpellList(ModuleModel moduleModel)
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
		static public List<TableModel> GenerateFatTableList(ModuleModel moduleModel)
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
		static public List<EquipmentModel> GenerateFatEquipmentList(ModuleModel moduleModel)
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
	}
}
