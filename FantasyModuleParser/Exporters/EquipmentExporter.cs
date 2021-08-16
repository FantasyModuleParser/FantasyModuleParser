﻿using FantasyModuleParser.Equipment.Enums;
using FantasyModuleParser.Equipment.Models;
using FantasyModuleParser.Extensions;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC.Controllers;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		static public void EquipmentBaseAC(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			if (equipmentModel.ArmorEnumType == ArmorEnum.Shield || equipmentModel.PrimaryEquipmentEnumType == PrimaryEquipmentEnum.Weapon)
			{
				xmlWriter.WriteStartElement("bonus"); /* <bonus> */
			}
			else
			{
				xmlWriter.WriteStartElement("ac"); /* <ac> */
			}				
			xmlWriter.WriteAttributeString("type", "number");
			if (equipmentModel.PrimaryEquipmentEnumType == PrimaryEquipmentEnum.Armor)
			{
				xmlWriter.WriteValue(equipmentModel.Armor.ArmorValue);
			}
			if (equipmentModel.PrimaryEquipmentEnumType == PrimaryEquipmentEnum.Weapon)
			{
				xmlWriter.WriteValue(equipmentModel.Weapon.PrimaryDamage.Bonus);
			}			
			xmlWriter.WriteEndElement(); /* <ac> </ac> or <bonus> </bonus> depending on Equipment Type*/
		}

		static public void EquipmentDexBonus(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			if (!string.IsNullOrEmpty(equipmentModel.Armor.DexterityBonus.ToString()))
			{
				xmlWriter.WriteStartElement("dexbonus"); /* <dexbonus> */
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(equipmentModel.Armor.DexterityBonus.ToString());
				xmlWriter.WriteEndElement(); /* <dexbonus> </dexbonus> */
			}			
		}
		
		static public void EquipmentStealth(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("stealth"); /* <stealth> */
			xmlWriter.WriteAttributeString("type", "string");
			if (equipmentModel.Armor.IsStealthDisadvantage)
			{
				xmlWriter.WriteString("Disadvantage");
			}
			else xmlWriter.WriteString("-");
			xmlWriter.WriteEndElement(); /* <stealth> </stealth> */
		}

		static public void EquipmentStrength(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("strength"); /* <strength> */
			xmlWriter.WriteAttributeString("type", "string");
			if (equipmentModel.Armor.StrengthRequirement > 0)
			{
				xmlWriter.WriteValue(equipmentModel.Armor.StrengthRequirement);
			}
			else xmlWriter.WriteString("-");
			xmlWriter.WriteEndElement(); /* <strength> </strength> */
		}

		static public void EquipmentDescription(XmlWriter xmlWriter, EquipmentModel equipmentModel, NPCController npcController)
		{
			xmlWriter.WriteStartElement("description"); /* <description> */
			xmlWriter.WriteAttributeString("type", "formattedtext");
			xmlWriter.WriteRaw(npcController.GenerateFantasyGroundsDescriptionXML(equipmentModel.Description));
			xmlWriter.WriteEndElement(); /* <description> </description> */
		}

		static public void EquipmentIdentified(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("isidentified"); /* <isidentified> */
			xmlWriter.WriteAttributeString("type", "number");
			if (equipmentModel.IsIdentified)
			{
				xmlWriter.WriteValue("1");
			}
			else
			{
				xmlWriter.WriteValue("0");
			}
			xmlWriter.WriteEndElement(); /* <isidentified> </isidentified> */
		}

		static public void EquipmentNonIDName(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("nonid_name"); /* <nonid_name> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(equipmentModel.NonIdName);
			xmlWriter.WriteEndElement(); /* <nonid_name> </nonid_name> */
		}

		static public void EquipmentNonIDDescription(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("nonidentified"); /* <nonidentified> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(equipmentModel.NonIdDescription);
			xmlWriter.WriteEndElement(); /* <nonidentified> </nonidentified> */
		}

		static public void EquipmentDamage(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("damage"); /* <damage> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(DamageString(equipmentModel));
			xmlWriter.WriteEndElement(); /* <damage> </damage> */
		}

		static public string DamageString(EquipmentModel equipmentModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(PrimaryDamage(equipmentModel));
			if (equipmentModel.Weapon.HasSecondaryDamage())
			{
				stringBuilder.Append(" + " + SecondaryDamage(equipmentModel));
			}
			return stringBuilder.ToString();
		}

		static private string PrimaryDamage(EquipmentModel equipmentModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (equipmentModel.Weapon.PrimaryDamage.NumOfDice > 0)
			{
				stringBuilder.Append(equipmentModel.Weapon.PrimaryDamage.NumOfDice + equipmentModel.Weapon.PrimaryDamage.DieType.GetDescription());
				if (equipmentModel.Weapon.PrimaryDamage.Bonus > 0)
				{
					stringBuilder.Append(" + " + equipmentModel.Weapon.PrimaryDamage.Bonus);
				}
				if (equipmentModel.Weapon.PrimaryDamage.Bonus < 0)
				{
					StringBuilder stringBuilder1 = new StringBuilder();
					stringBuilder1.Append(equipmentModel.Weapon.PrimaryDamage.Bonus);
					stringBuilder1.Remove(0, 1);
					stringBuilder.Append(" - " + stringBuilder1);
				}
				stringBuilder.Append(" " + equipmentModel.Weapon.PrimaryDamage.DamageType.GetDescription());
			}
			return stringBuilder.ToString();
		}

		static private string SecondaryDamage(EquipmentModel equipmentModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (equipmentModel.Weapon.BonusDamage.NumOfDice > 0)
			{
				stringBuilder.Append(equipmentModel.Weapon.BonusDamage.NumOfDice + equipmentModel.Weapon.BonusDamage.DieType.GetDescription());
				if (equipmentModel.Weapon.BonusDamage.Bonus > 0)
				{
					stringBuilder.Append(" + " + equipmentModel.Weapon.BonusDamage.Bonus);
				}
				if (equipmentModel.Weapon.BonusDamage.Bonus < 0)
				{
					StringBuilder stringBuilder1 = new StringBuilder();
					stringBuilder1.Append(equipmentModel.Weapon.BonusDamage.Bonus);
					stringBuilder1.Remove(0, 1);
					stringBuilder.Append(" - " + stringBuilder1);
				}
				stringBuilder.Append(" " + equipmentModel.Weapon.BonusDamage.DamageType.GetDescription());
			}
			return stringBuilder.ToString();
		}

		static public void EquipmentProperties(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			if (equipmentModel.Weapon.WeaponProperties.Count > 0 || equipmentModel.Weapon.MaterialProperties.Count > 0 || equipmentModel.WeaponEnumType == WeaponEnum.MRW || equipmentModel.WeaponEnumType == WeaponEnum.SRW)
			{
				xmlWriter.WriteStartElement("properties"); /* <properties> */
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(WeaponProperty(equipmentModel));
				xmlWriter.WriteEndElement(); /* <properties> </properties> */
			}			
		}

		static private string WeaponProperty(EquipmentModel equipmentModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (equipmentModel.WeaponEnumType == WeaponEnum.MRW)
			{
				stringBuilder.Append("Ammunition (range " + equipmentModel.Weapon.ShortRange + "/" + equipmentModel.Weapon.LongRange + "), ");
			}
			if (equipmentModel.WeaponEnumType == WeaponEnum.SRW)
			{
				stringBuilder.Append("Thrown (range " + equipmentModel.Weapon.ShortRange + "/" + equipmentModel.Weapon.LongRange + "), ");
			}
			foreach (Enum property in equipmentModel.Weapon.WeaponProperties)
			{
				stringBuilder.Append(property.GetDescription() + ", ");
			}
			foreach (Enum material in equipmentModel.Weapon.MaterialProperties)
			{
				stringBuilder.Append(material.GetDescription() + ", ");
			}
			return stringBuilder.ToString(0, stringBuilder.Length - 2);
		}

		static private void EquipmentLocation(XmlWriter xmlWriter, ModuleModel moduleModel, List<EquipmentModel> EquipmentList)
		{
			foreach (EquipmentModel equip in EquipmentList)
			{
				xmlWriter.WriteStartElement(EquipmentNameToXMLFormat(equip)); /* <equipment_name> */
				xmlWriter.WriteStartElement("link"); /* <equipment_name> <link> */
				xmlWriter.WriteAttributeString("type", "windowreference");
				EquipmentClass(xmlWriter);
				EquipmentRecordname(xmlWriter, equip, moduleModel);
				xmlWriter.WriteEndElement(); /* <equipment_name> <link> </link> */
				EquipmentName(xmlWriter, equip);
				EquipmentCost(xmlWriter, equip);
				EquipmentWeight(xmlWriter, equip);
				xmlWriter.WriteEndElement(); /* <equipment_name> </equipment_name> */
			}
		}

		static public void EquipmentLists(XmlWriter xmlWriter, ModuleModel moduleModel, List<EquipmentModel> EquipmentList)
		{
			EquipmentList.Sort((equipOne, equipTwo) => equipOne.Name.CompareTo(equipTwo.Name));
			var EquipmentByPrimaryList = EquipmentList.GroupBy(x => x.PrimaryEquipmentEnumType).Select(x => x.ToList()).ToList();

			int equipListId = 1;
			foreach (List<EquipmentModel> equipmentList in EquipmentByPrimaryList)
			{
				string primaryEquipmentTypeDescription = equipmentList[0].PrimaryEquipmentEnumType.GetDescription();
				
				ProcessEquipListByType(xmlWriter, moduleModel,  primaryEquipmentTypeDescription, equipListId);

				// Increase the ID by one, which is used in the method ProcessEquipListByType
				equipListId++;
			}
		}

		static private void ProcessEquipListByType(XmlWriter xmlWriter, ModuleModel moduleModel, string primaryEquipmentTypeDescription, int localizedIdValue)
        {

            xmlWriter.WriteStartElement("id-" + localizedIdValue.ToString("D5")); /* <id-*> */
            Equipment_Index_ListLink(moduleModel, primaryEquipmentTypeDescription, xmlWriter);

            // <name type="string">ADVENTURING GEAR</name>
            Equipment_Index_Name(xmlWriter, primaryEquipmentTypeDescription);
            xmlWriter.WriteEndElement();
        }

        private static void Equipment_Index_Name(XmlWriter xmlWriter, string primaryEquipmentTypeDescription)
        {
            xmlWriter.WriteStartElement("name");
            xmlWriter.WriteAttributeString("type", "string");
            xmlWriter.WriteString(primaryEquipmentTypeDescription.ToUpper());
            xmlWriter.WriteEndElement();
        }

        static private void Equipment_Index_ListLink(ModuleModel moduleModel, string primaryEquipmentTypeDescription, XmlWriter xmlWriter)
        {
			xmlWriter.WriteStartElement("linklist"); /* <id-*> <linklist> */
			xmlWriter.WriteAttributeString("type", "windowreference");
			Equipment_Index_ListLink_Class(primaryEquipmentTypeDescription, xmlWriter);
			Equipment_Index_ListLink_RecordName(primaryEquipmentTypeDescription, moduleModel, xmlWriter);
			xmlWriter.WriteEndElement(); /* </linklist> */
			
		}

		/// <summary>
		/// Example:  <class>reference_adventuringgeartable</class>
		/// </summary>
		/// <param name="primaryEquipmentTypeDescription"></param>
		/// <param name="xmlWriter"></param>
		static private void Equipment_Index_ListLink_Class(string primaryEquipmentTypeDescription, XmlWriter xmlWriter)
        {
			xmlWriter.WriteStartElement("class");
			xmlWriter.WriteString("reference_" + (primaryEquipmentTypeDescription.Replace(" ", String.Empty)).ToLower());
			xmlWriter.WriteEndElement();
        }

		/// <summary>
		/// Example :: reference.equipmentlists.adventuringgeartable@DD5E SRD Data
		/// </summary>
		/// <param name="primaryEquipmentTypeDescription"></param>
		/// <param name="moduleModel"></param>
		/// <param name="xmlWriter"></param>
		static private void Equipment_Index_ListLink_RecordName(string primaryEquipmentTypeDescription, ModuleModel moduleModel, XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("recordname");
            xmlWriter.WriteString("reference.equipmentlists."
                + (primaryEquipmentTypeDescription.Replace(" ", String.Empty)).ToLower()
				+ "@" + moduleModel.Name);
            xmlWriter.WriteEndElement();
		}

		static public string EquipmentNameToXMLFormat(EquipmentModel equipmentModel)
		{
			string name = equipmentModel.Name.ToLower();
			return name.Replace(" ", "_").Replace(",", "").Replace("(", "_").Replace(")", "");
		}

		private static void EquipmentListClass(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("class"); /* <class> */
			//xmlWriter.WriteString("reference_" + EquipmentTypeToXMLFormat(EquipmentType) + "table");
			xmlWriter.WriteEndElement(); /* <class> </class> */
		}
		private static void EquipmentClass(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("class"); /* <equipment_name> <link> <class> */
			xmlWriter.WriteString("reference_equipment");
			xmlWriter.WriteEndElement(); /* <equipment_name> <link> <class> </class> */
		}

		static private void EquipmentRecordname(XmlWriter xmlWriter, EquipmentModel equipmentModel, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("recordname"); /* <equipment_name> <link> <recordname> */
			xmlWriter.WriteString("reference.equipmentdata." + EquipmentNameToXMLFormat(equipmentModel) + "@" + moduleModel.Name);
			xmlWriter.WriteEndElement(); /* <equipment_name> <link> <recordname> </recordname> */
		}
	}
}
