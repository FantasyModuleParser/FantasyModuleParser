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
			xmlWriter.WriteString(getEquipmentPrimaryHeaderEnum(equipmentModel));
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
			else if (equipmentModel.PrimaryEquipmentEnumType == PrimaryEquipmentEnum.MountsAndOtherAnimals)
			{
				xmlWriter.WriteString(equipmentModel.AnimalsEnumType.GetDescription());
			}
			else if (equipmentModel.PrimaryEquipmentEnumType == PrimaryEquipmentEnum.WaterborneVehicles)
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
			if (equipmentModel.CostDenomination != CurrencyEnum.None)
			{
				xmlWriter.WriteString(equipmentModel.CostValue + " " + equipmentModel.CostDenomination.GetDescription());
			}
			else xmlWriter.WriteString(equipmentModel.CostValue);
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
			if (equipmentModel.PrimaryEquipmentEnumType == PrimaryEquipmentEnum.Weapon)
			{
				xmlWriter.WriteStartElement("bonus"); /* <bonus> */
			}
			else
			{
				xmlWriter.WriteStartElement("ac"); /* <ac> */
			}				
			xmlWriter.WriteAttributeString("type", "string");
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
			//if (!string.IsNullOrEmpty(equipmentModel.Armor.DexterityBonus.ToString()))
			//{
				xmlWriter.WriteStartElement("dexbonus"); /* <dexbonus> */
				xmlWriter.WriteAttributeString("type", "string");
				//xmlWriter.WriteString(equipmentModel.Armor.DexterityBonus.ToString());
				xmlWriter.WriteString("Yes");
				xmlWriter.WriteEndElement(); /* <dexbonus> </dexbonus> */
			//}			
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
				xmlWriter.WriteValue("Str " + equipmentModel.Armor.StrengthRequirement);
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

		static public void EquipmentSpeed(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("speed");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(equipmentModel.Speed + " " + equipmentModel.Measurement.GetDescription());
			xmlWriter.WriteEndElement();
		}

		static public void EquipmentCapacity(XmlWriter xmlWriter, EquipmentModel equipmentModel)
		{
			xmlWriter.WriteStartElement("carryingcapacity");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(equipmentModel.Capacity);
			xmlWriter.WriteEndElement();
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

		static public void EquipmentLists(XmlWriter xmlWriter, ModuleModel moduleModel, List<EquipmentModel> EquipmentList)
		{
			EquipmentList.Sort((equipOne, equipTwo) => equipOne.Name.CompareTo(equipTwo.Name));
			var EquipmentByPrimaryList = EquipmentList.GroupBy(x => x.PrimaryEquipmentEnumType).Select(x => x.ToList()).ToList();

			int equipListId = 1;
			foreach (List<EquipmentModel> equipmentList in EquipmentByPrimaryList)
			{
				//string primaryEquipmentTypeDescription =
				//	_getReferenceClassForPrimaryEquipmentEnumType(equipmentList[0].PrimaryEquipmentEnumType);
				
				ProcessEquipListByType(xmlWriter, moduleModel, equipmentList[0], equipListId);

				// Increase the ID by one, which is used in the method ProcessEquipListByType
				equipListId++;
			}
		}

		const string referenceAdventureGearTable = "reference_adventuringgeartable";
		const string referenceArmorTable = "reference_armortable";
		const string referenceWeaponTable = "reference_weapontable";
		const string referenceMountsAndOtherAnimalsTable = "reference_mountsandotheranimalstable";
		const string referenceWaterVehicles = "reference_waterbornevehiclestable";

		/// <summary>
		/// Due to an implemenation decision by Smiteworks, the reference Table value is there so Fantasy Grounds knows
		/// how to display the data in an aggregate fashion.  
		/// </summary>
		/// <param name="primaryEquipmentEnum"></param>
		/// <returns></returns>
		private static string _getReferenceClassForPrimaryEquipmentEnumType(PrimaryEquipmentEnum primaryEquipmentEnum)
        {
            switch (primaryEquipmentEnum)
            {
				case PrimaryEquipmentEnum.AdventuringGear:
				case PrimaryEquipmentEnum.TackHarnessAndDrawnVehicles:
				case PrimaryEquipmentEnum.Treasure:
				case PrimaryEquipmentEnum.Tools:
					return referenceAdventureGearTable;
				case PrimaryEquipmentEnum.Armor:
					return referenceArmorTable;
				case PrimaryEquipmentEnum.Weapon:
					return referenceWeaponTable;
				case PrimaryEquipmentEnum.MountsAndOtherAnimals:
					return referenceMountsAndOtherAnimalsTable;
				case PrimaryEquipmentEnum.WaterborneVehicles:
					return referenceWaterVehicles;
				default:
					return "ENUM_NEEDS_REFERENCE";
            }
        }

		static private void ProcessEquipListByType(XmlWriter xmlWriter, ModuleModel moduleModel, EquipmentModel primaryEquipmentModel, int localizedIdValue)
		{

			xmlWriter.WriteStartElement("id-" + localizedIdValue.ToString("D5")); /* <id-*> */
			Equipment_Index_ListLink(moduleModel, primaryEquipmentModel, xmlWriter);

			// <name type="string">ADVENTURING GEAR</name>
			Equipment_Index_Name(xmlWriter, primaryEquipmentModel);
			xmlWriter.WriteEndElement();
		}

		public static void EquipmentLists_Equipment(ModuleModel moduleModel, List<EquipmentModel> FatEquipmentList, XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("equipment"); /* <equipmentlists> <equipment> */
			EquipmentLists_Equipment_Name(xmlWriter);
			EquipmentLists_Equipment_Index(moduleModel, FatEquipmentList, xmlWriter);
			xmlWriter.WriteEndElement(); /* <equipmentlists> <equipment> </equipment> */
		}

		/// <summary>
		/// <equipmentlists> <equipment> <index>
		/// </summary>
		public static void EquipmentLists_Equipment_Index(ModuleModel moduleModel, List<EquipmentModel> FatEquipmentList, XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("index"); /* <equipmentlists> <equipment> <index> */
			EquipmentLists(xmlWriter, moduleModel, FatEquipmentList);
			xmlWriter.WriteEndElement(); /* <equipmentlists> <equipment> <index> </index> */
		}

		public static void EquipmentLists_Equipment_Name(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name"); /* <equipmentlists> <equipment> <name> */
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("Equipment");
			xmlWriter.WriteEndElement(); /* <equipmentlists> <equipment> <name> </name> */
		}

		private static void Equipment_Index_Name(XmlWriter xmlWriter, EquipmentModel primaryEquipmentModel)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(primaryEquipmentModel.PrimaryEquipmentEnumType.GetDescription().ToUpper());
			xmlWriter.WriteEndElement();
		}

		static private void Equipment_Index_ListLink(ModuleModel moduleModel, EquipmentModel primaryEquipmentModel, XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("listlink"); /* <id-*> <listlink> */
			xmlWriter.WriteAttributeString("type", "windowreference");
			Equipment_Index_ListLink_Class(primaryEquipmentModel, xmlWriter);
			Equipment_Index_ListLink_RecordName(primaryEquipmentModel, moduleModel, xmlWriter);
			xmlWriter.WriteEndElement(); /* </listlink> */
		}

		/// <summary>
		/// Example:  <class>reference_adventuringgeartable</class>
		/// </summary>
		/// <param name="primaryEquipmentTypeDescription"></param>
		/// <param name="xmlWriter"></param>
		static private void Equipment_Index_ListLink_Class(EquipmentModel primaryEquipmentModel, XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("class");
			xmlWriter.WriteString(_getReferenceClassForPrimaryEquipmentEnumType(primaryEquipmentModel.PrimaryEquipmentEnumType));
			xmlWriter.WriteEndElement();
		}

		/// <summary>
		/// Example :: reference.equipmentlists.adventuringgeartable@DD5E SRD Data
		/// </summary>
		/// <param name="primaryEquipmentTypeDescription"></param>
		/// <param name="moduleModel"></param>
		/// <param name="xmlWriter"></param>
		static private void Equipment_Index_ListLink_RecordName(EquipmentModel primaryEquipmentModel, ModuleModel moduleModel, XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("reference.equipmentlists."
				+ primaryEquipmentModel.PrimaryEquipmentEnumType.GetDescription().Replace(" ", String.Empty).ToLower()
				+ "@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
		}

		// An Example of what this method would produce:

		/*
		 *       
		<adventuringgeartable>
        <description type="string">Adventuring Gear Table</description>
        <groups>
          <section000>
            <description type="string">Ammunition</description>
            <equipment>
              <arrow>
                <link type="windowreference">
                  <class>reference_equipment</class>
                  <recordname>reference.equipmentdata.arrow@*</recordname>
                </link>
                <name type="string">Arrow</name>
                <cost type="string">5 cp</cost>
                <weight type="number">0.05</weight>
              </arrow>
		 */

		// For Developers: The assumption is that EquipmentList is a culmination of ALL equipment items.
		// This means that the list needs to be grouped by Primary, then each subsequent list is grouped By secondary type
		public static void IndividualEquipmentClassList(XmlWriter xmlWriter, List<EquipmentModel> EquipmentList, ModuleModel moduleModel)
		{
			// Just good to check and ensure if EquipmentList is empty;  If true, then just do nothing and return
			if (EquipmentList.Count <= 0)
				return;

			// 1. Group by the PrimaryType
			var EquipmentGroupByPrimaryType = EquipmentList.GroupBy(x => x.PrimaryEquipmentEnumType).Select(x => x.ToList()).ToList();

			foreach(List<EquipmentModel> primaryEquipmentList in EquipmentGroupByPrimaryType)
			{
				// 2. Create the non-standard xml tag with the key name matching a no-whitespace, all lower primaryType description
				// <adventuringgear>, <armor>, etc....
				xmlWriter.WriteStartElement(primaryEquipmentList[0].PrimaryEquipmentEnumType.GetDescription().Replace(" ", string.Empty).ToLower());

				// 3. Write the Description tag
				EquipmentList_CustomPrimary_Description(xmlWriter, primaryEquipmentList);

				// 4. Group the list by it's secondary Enum, noting that the secondary enum is based on the selected Primary Type enum
				EquipmentList_CustomSecondary_Groups(xmlWriter, primaryEquipmentList, moduleModel);

				xmlWriter.WriteEndElement();
			}

		}

		private static void EquipmentList_CustomSecondary_Groups(XmlWriter xmlWriter, List<EquipmentModel> primaryEquipmentList, ModuleModel moduleModel)
		{
			var EquipmentGroupBySecondaryType = primaryEquipmentList.GroupBy(x => getEquipmentSecondaryEnum(x)).Select(x => x.ToList()).ToList();

			xmlWriter.WriteStartElement("groups");

			int sectionId = 0;
			foreach(List<EquipmentModel> secondaryTypeGroup in EquipmentGroupBySecondaryType)
            {
                EquipmentList_CustomSecondary_Groups_Section(xmlWriter, sectionId, secondaryTypeGroup, moduleModel);
				sectionId++;
			}

            xmlWriter.WriteEndElement();
        }

		private static void EquipmentList_CustomSecondary_Groups_Section(XmlWriter xmlWriter, int sectionId, List<EquipmentModel> secondaryList, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("section" + sectionId.ToString("D3"));
			EquipmentList_CustomSecondary_Description(xmlWriter, secondaryList);

			EquipmentList_CustomSecondary_Groups_Section_Equipment(xmlWriter, secondaryList, moduleModel);
			xmlWriter.WriteEndElement();
		}

		private static void EquipmentList_CustomSecondary_Groups_Section_Equipment(XmlWriter xmlWriter, List<EquipmentModel> secondaryTypeGroup, ModuleModel moduleModel)
        {
            xmlWriter.WriteStartElement("equipment");
            foreach (EquipmentModel secondaryEquipmentModelItem in secondaryTypeGroup)
            {
				xmlWriter.WriteStartElement(secondaryEquipmentModelItem.Name.Replace(" ", string.Empty).ToLower());
				EquipmentList_CustomSecondary_Groups_Section_Equipment_Item_Link(xmlWriter, secondaryEquipmentModelItem, moduleModel);
				xmlWriter.WriteEndElement();
                //TODO
                /*
				<arrow>
					<link type="windowreference">
					  <class>reference_equipment</class>
					  <recordname>reference.equipmentdata.arrow@*</recordname>
					</link>
					<name type="string">Arrow</name>
					<cost type="string">5 cp</cost>
					<weight type="number">0.05</weight>
				</arrow> 
				 */
            }
            xmlWriter.WriteEndElement();
        }

		private static void EquipmentList_CustomSecondary_Groups_Section_Equipment_Item_Link(XmlWriter xmlWriter, EquipmentModel equip, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("link");
			xmlWriter.WriteAttributeString("type", "windowreference");
			EquipmentList_CustomSecondary_Groups_Section_Equipment_Item_Class(xmlWriter, equip);
			EquipmentList_CustomSecondary_Groups_Section_Equipment_Item_Recordname(xmlWriter, equip, moduleModel);
			xmlWriter.WriteEndElement();
			EquipmentName(xmlWriter, equip);
			EquipmentCost(xmlWriter, equip);
			if (equip.PrimaryEquipmentEnumType != PrimaryEquipmentEnum.MountsAndOtherAnimals &&
				equip.PrimaryEquipmentEnumType != PrimaryEquipmentEnum.WaterborneVehicles &&
				equip.PrimaryEquipmentEnumType != PrimaryEquipmentEnum.Armor)
			{
				EquipmentWeight(xmlWriter, equip);
			}
			if (equip.PrimaryEquipmentEnumType == PrimaryEquipmentEnum.Armor)
			{
				EquipmentBaseAC(xmlWriter, equip);
				EquipmentDexBonus(xmlWriter, equip);
				EquipmentStrength(xmlWriter, equip);
				EquipmentStealth(xmlWriter, equip);
				// Put Weight at the bottom of the list... may be FGU quirk.
				EquipmentWeight(xmlWriter, equip);
			}
			if(equip.PrimaryEquipmentEnumType == PrimaryEquipmentEnum.Weapon)
			{
				EquipmentDamage(xmlWriter, equip);
				EquipmentProperties(xmlWriter, equip);
			}
			if(equip.PrimaryEquipmentEnumType == PrimaryEquipmentEnum.MountsAndOtherAnimals && equip.AnimalsEnumType == AnimalsEnum.Mounts)
			{
				EquipmentSpeed(xmlWriter, equip);
				EquipmentCapacity(xmlWriter, equip);
			}
			if (equip.PrimaryEquipmentEnumType == PrimaryEquipmentEnum.WaterborneVehicles)
			{
				EquipmentSpeed(xmlWriter, equip);
			}
		}

		private static void EquipmentList_CustomSecondary_Groups_Section_Equipment_Item_Class(XmlWriter xmlWriter, EquipmentModel equip)
		{
			xmlWriter.WriteStartElement("class");
			xmlWriter.WriteString(getSecondaryGroupsSectionEquipmentItemClass(equip));
			xmlWriter.WriteEndElement();
		}

		private static string getSecondaryGroupsSectionEquipmentItemClass(EquipmentModel equip)
		{
			switch (equip.PrimaryEquipmentEnumType)
			{
				case PrimaryEquipmentEnum.AdventuringGear:
					return "reference_equipment";
				case PrimaryEquipmentEnum.MountsAndOtherAnimals:
					return "reference_mountsandotheranimals";
				case PrimaryEquipmentEnum.Armor:
					return "reference_armor";
				case PrimaryEquipmentEnum.Tools:
					return "reference_equipment";
				case PrimaryEquipmentEnum.Treasure:
					return "reference_equipment";
				case PrimaryEquipmentEnum.WaterborneVehicles:
					return getVehiclesSubtype(equip);
				case PrimaryEquipmentEnum.Weapon:
					return "reference_weapon";
				// NOTE that default is required with all switch statements!!
				default:
					return "reference_equipment";
			}
		}

		private static string getVehiclesSubtype(EquipmentModel equip)
		{
			switch (equip.VehiclesEnumType)
			{
				case VehiclesEnum.WaterCraft:
					return "reference_waterbornevehicles";
				case VehiclesEnum.AirBorne:
					return "reference_waterbornevehicles";
				default:
					return "reference_equipment";
			}
		}
		private static void EquipmentList_CustomSecondary_Groups_Section_Equipment_Item_Recordname(XmlWriter xmlWriter, EquipmentModel equip, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("reference.equipmentdata." + EquipmentNameToXML(equip) + "@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
		}
		private static void EquipmentList_CustomPrimary_Description(XmlWriter xmlWriter, List<EquipmentModel> primaryEquipmentList)
		{
			Description_Tag(xmlWriter, getEquipmentPrimaryEnum(primaryEquipmentList[0]));
		}

		private static string getEquipmentPrimaryEnum(EquipmentModel equipmentModel)
		{
			return equipmentModel.PrimaryEquipmentEnumType.GetDescription() + " Table";
		}
		private static string getEquipmentPrimaryHeaderEnum(EquipmentModel equipmentModel)
		{
			return equipmentModel.PrimaryEquipmentEnumType.GetDescription();
		}

		private static void EquipmentList_CustomSecondary_Description(XmlWriter xmlWriter, List<EquipmentModel> primaryEquipmentList)
        {
			Description_Tag(xmlWriter, getEquipmentSecondaryEnum(primaryEquipmentList[0]));
        }

		static string getEquipmentSecondaryEnum(EquipmentModel equipmentModel)
        {
			switch (equipmentModel.PrimaryEquipmentEnumType)
			{
				case PrimaryEquipmentEnum.AdventuringGear:
					return equipmentModel.AdventuringGearEnumType.GetDescription();
				case PrimaryEquipmentEnum.MountsAndOtherAnimals:
					return equipmentModel.AnimalsEnumType.GetDescription();
				case PrimaryEquipmentEnum.Armor:
					return equipmentModel.ArmorEnumType.GetDescription();
				case PrimaryEquipmentEnum.Tools:
					return equipmentModel.ToolsEnumType.GetDescription();
				case PrimaryEquipmentEnum.Treasure:
					return equipmentModel.TreasureEnumType.GetDescription();
				case PrimaryEquipmentEnum.WaterborneVehicles:
					return equipmentModel.VehiclesEnumType.GetDescription();
				case PrimaryEquipmentEnum.Weapon:
					return equipmentModel.WeaponEnumType.GetDescription();
				// NOTE that default is required with all switch statements!!
				default:
					return equipmentModel.AdventuringGearEnumType.GetDescription();
			}
		}

		static private void Description_Tag(XmlWriter xmlWriter, string descriptionValue)
        {
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(descriptionValue);
			xmlWriter.WriteEndElement();
		}
	}
}