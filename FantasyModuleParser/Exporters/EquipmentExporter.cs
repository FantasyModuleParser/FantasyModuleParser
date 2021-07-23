using FantasyModuleParser.Equipment.Enums;
using FantasyModuleParser.Equipment.Models;
using FantasyModuleParser.Extensions;
using log4net;
using System;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	class EquipmentExporter
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(EquipmentExporter));

		static public string EquipmentNameToXML(EquipmentModel equipmentModel)
		{
			string name = equipmentModel.Name.ToLower();
			return name.Replace(" ", "_").Replace(",", "").Replace("(", "_").Replace(")", "");
		}

		static public void EquipmentLocked(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			if (equipmentModel.IsLocked == true)
			{
				xmlWriter.WriteStartElement("locked"); /* <locked> */
				xmlWriter.WriteAttributeString("type", "number");
				xmlWriter.WriteString("1");
				xmlWriter.WriteEndElement(); /* </locked> */
			}
			else
			{
				xmlWriter.WriteStartElement("locked"); /* <locked> */
				xmlWriter.WriteAttributeString("type", "number");
				xmlWriter.WriteString("0");
				xmlWriter.WriteEndElement(); /* </locked> */
			}
		}

		static public void EquipmentName(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("name"); /* <name> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(equipmentModel.Name);
			xmlWriter.WriteEndElement(); /* <name> </name> */
		}

		static public void EquipmentType(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("type"); /* <type> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(equipmentModel.PrimaryEquipmentEnumType.GetDescription());
			xmlWriter.WriteEndElement(); /* <type> </type> */
		}

		static public void EquipmentSubtype(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("subtype"); /* <subtype> */
			xmlWriter.WriteAttributeString("type", "string");
			if (equipmentModel.PrimaryEquipmentEnumType == PrimaryEquipmentEnum.AdventuringGear)
			{
				xmlWriter.WriteString(equipmentModel.AdventuringGearEnumType.GetDescription());
			}
			else if (equipmentModel.PrimaryEquipmentEnumType == PrimaryEquipmentEnum.Armor)
			{
				xmlWriter.WriteString(equipmentModel.ArmorEnumType.GetDescription());
			}
			else if (equipmentModel.PrimaryEquipmentEnumType == PrimaryEquipmentEnum.Weapon)
			{
				xmlWriter.WriteString(equipmentModel.WeaponEnumType.GetDescription());
			}
			else if (equipmentModel.PrimaryEquipmentEnumType == PrimaryEquipmentEnum.Tools)
			{
				xmlWriter.WriteString(equipmentModel.ToolsEnumType.GetDescription());
			}
			else if (equipmentModel.PrimaryEquipmentEnumType == PrimaryEquipmentEnum.Animals)
			{
				xmlWriter.WriteString(equipmentModel.AnimalsEnumType.GetDescription());
			}
			else if (equipmentModel.PrimaryEquipmentEnumType == PrimaryEquipmentEnum.Vehicles)
			{
				xmlWriter.WriteString(equipmentModel.VehiclesEnumType.GetDescription());
			}
			else
			{
				xmlWriter.WriteString(equipmentModel.TreasureEnumType.GetDescription());
			}				
			xmlWriter.WriteEndElement(); /* <subtype> </subtype> */
		}

		static public void EquipmentCost(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("cost"); /* <cost> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(equipmentModel.CostValue + " " + equipmentModel.CostDenomination.GetDescription());
			xmlWriter.WriteEndElement(); /* <cost> </cost> */
		}
		static public void EquipmentWeight(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("weight"); /* <weight> */
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(equipmentModel.Weight);
			xmlWriter.WriteEndElement(); /* <weight> </weight> */
		}

		static public void EquipmentRarity(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("rarity"); /* <rarity> */
			xmlWriter.WriteAttributeString("type", "string");
			if (equipmentModel.EquipmentRarity != RarityEnum.Mundane)
			{
				xmlWriter.WriteString(equipmentModel.EquipmentRarity.GetDescription());
			}			
			xmlWriter.WriteEndElement(); /* <rarity> </rarity> */
		}

		static public void EquipmentBaseAC(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("ac"); /* <ac> */
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(equipmentModel.Armor.ArmorValue);
			xmlWriter.WriteEndElement(); /* <ac> </ac> */
		}

		static public void EquipmentDexBonus(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("dexbonus"); /* <dexbonus> */
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteString(equipmentModel.Armor.DexterityBonus);
			xmlWriter.WriteEndElement(); /* <dexbonus> </dexbonus> */
		}
		static public void EquipmentStealth(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			throw new NotImplementedException();
		}

		static public void EquipmentStrength(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			throw new NotImplementedException();
		}
	}
}
