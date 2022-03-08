using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.Extensions;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC.Controllers;
using System.Collections.Generic;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	class ClassExporter
	{
		public static void DatabaseXML_Root_Classes(XmlWriter xmlWriter, ModuleModel module)
		{
			if (module.IncludesClasses)
			{
				xmlWriter.WriteStartElement("class");
				Classes_Category(xmlWriter, module);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Classes_Category(XmlWriter xmlWriter, ModuleModel module)
		{
			List<ClassModel> FatClassList = CommonMethods.GenerateFatClassList(module);
			FatClassList.Sort((classOne, classTwo) => classOne.Name.CompareTo(classTwo.Name));
			foreach (CategoryModel category in module.Categories)
			{
				xmlWriter.WriteStartElement("category");
				xmlWriter.WriteAttributeString("name", category.Name);
				CommonMethods.BaseIcon_DecalIcon(xmlWriter);
				int classID = 0;
				foreach (ClassModel classModel in FatClassList)
				{
					Classes_Category_Class(xmlWriter, classID, classModel);
				}
				xmlWriter.WriteEndElement();
			}
		}

		private static void Classes_Category_Class(XmlWriter xmlWriter, int classID, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("id-" + classID.ToString("D4"));
			Class_Name(xmlWriter, classModel);
			Class_Text(xmlWriter, classModel);
			Class_HP(xmlWriter, classModel);
			Class_Proficiencies(xmlWriter, classModel);
			Class_Abilities(xmlWriter, classModel);
			Class_Equipment(xmlWriter);
			Class_Features(xmlWriter, classModel);
			xmlWriter.WriteEndElement();
		}

		private static void Class_Name(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("name");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteValue(classModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Class_Text(XmlWriter xmlWriter, ClassModel classModel)
		{
			NPCController npcController = new NPCController();
			xmlWriter.WriteStartElement("text");
			CommonMethods.Type_FormattedText(xmlWriter);
			xmlWriter.WriteRaw(npcController.GenerateFantasyGroundsDescriptionXML(classModel.Description));
			xmlWriter.WriteEndElement();
		}

		private static void Class_HP(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("hp");
			Class_HP_HitDice(xmlWriter, classModel);
			Class_HP_HPAt1stLevel(xmlWriter, classModel);
			Class_HP_HPAtHigherLevels(xmlWriter, classModel);
			xmlWriter.WriteEndElement();
		}

		private static void Class_HP_HitDice(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("hitdice");
			HitDice_Name(xmlWriter);
			HitDice_Text(xmlWriter, classModel);
			xmlWriter.WriteEndElement();
		}

		private static void HitDice_Name(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString("Hit Dice");
			xmlWriter.WriteEndElement();
		}

		private static void HitDice_Text(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("text");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString("1" + classModel.HitPointDiePerLevel.GetDescription().ToLower().ToString() + " per " + classModel.Name.ToLower() + " level");
			xmlWriter.WriteEndElement();
		}

		private static void Class_HP_HPAt1stLevel(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("hitpointsat1stlevel");
			HP1stLevel_Name(xmlWriter);
			HP1stLevel_Text(xmlWriter, classModel);
			xmlWriter.WriteEndElement();
		}

		private static void HP1stLevel_Name(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString("Hit Points at 1st Level");
			xmlWriter.WriteEndElement();
		}

		private static void HP1stLevel_Text(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("text");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString("1" + classModel.HitPointDiePerLevel.GetDescription() + " + your Constitution modifier");
			xmlWriter.WriteEndElement();
		}

		private static void Class_HP_HPAtHigherLevels(XmlWriter xmlWriter, ClassModel classModel)
		{
			HPHigherLevels_Name(xmlWriter);
			HPHigherLevels_Text(xmlWriter, classModel);
		}

		private static void HPHigherLevels_Name(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString("Hit Points at Higher Levels");
			xmlWriter.WriteEndElement();
		}

		private static void HPHigherLevels_Text(XmlWriter xmlWriter, ClassModel classModel)
		{
			int average = ((int)classModel.HitPointDiePerLevel / 2) + 1;
			xmlWriter.WriteStartElement("text");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString("1" + classModel.HitPointDiePerLevel.GetDescription() + " (or " + average + ") + your Constitution modifier per " + classModel.Name.ToLower() + " level after 1st");
			xmlWriter.WriteEndElement();
		}

		private static void Class_Proficiencies(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("proficiencies");
			Proficiencies_Armor(xmlWriter, classModel);
			Proficiencies_Weapons(xmlWriter, classModel);
			Proficiencies_Tools(xmlWriter, classModel);
			Proficiencies_SavingThrows(xmlWriter, classModel);
			Proficiencies_Skills(xmlWriter, classModel);
			xmlWriter.WriteEndElement();
		}

		private static void Proficiencies_Armor(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("armor");
			Armor_Name(xmlWriter);
			Armor_Text(xmlWriter, classModel);
			xmlWriter.WriteEndElement();
		}

		private static void Armor_Name(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString("Armor");
			xmlWriter.WriteEndElement();
		}

		private static void Armor_Text(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("text");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString(classModel.Proficiency.GetArmorProficienciesForExporter());
			xmlWriter.WriteEndElement();
		}

		private static void Proficiencies_Weapons(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("weapons");
			Weapons_Name(xmlWriter);
			Weapons_Text(xmlWriter, classModel);
			xmlWriter.WriteEndElement();
		}

		private static void Weapons_Name(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString("Weapons");
			xmlWriter.WriteEndElement();
		}

		private static void Weapons_Text(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("text");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString(classModel.Proficiency.WeaponProficiencies);
			xmlWriter.WriteEndElement();
		}

		private static void Proficiencies_Tools(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("tools");
			Tools_Name(xmlWriter);
			Tools_Text(xmlWriter, classModel);
			xmlWriter.WriteEndElement();
		}

		private static void Tools_Name(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString("Tools");
			xmlWriter.WriteEndElement();
		}

		private static void Tools_Text(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("text");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString(classModel.Proficiency.GetToolProficienciesForExporter());
			xmlWriter.WriteEndElement();
		}

		private static void Proficiencies_SavingThrows(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("savingthrows");
			SavingThrows_Name(xmlWriter);
			SavingThrows_Text(xmlWriter, classModel);
			xmlWriter.WriteEndElement();
		}

		private static void SavingThrows_Name(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString("Saving Throws");
			xmlWriter.WriteEndElement();
		}

		private static void SavingThrows_Text(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("text");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString(classModel.Proficiency.GetSavingThrowProficienciesForExporter());
			xmlWriter.WriteEndElement();
		}

		private static void Proficiencies_Skills(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("skills");
			Skills_Name(xmlWriter);
			Skills_Text(xmlWriter, classModel);
			xmlWriter.WriteEndElement();
		}

		private static void Skills_Name(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("name");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString("Skills");
			xmlWriter.WriteEndElement();
		}

		private static void Skills_Text(XmlWriter xmlWriter, ClassModel classModel)
		{
			int switchval = classModel.Proficiency.NumberOfSkillsToChoose;
			xmlWriter.WriteStartElement("text");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString(classModel.Proficiency.GetSkillProficienciesForExporter(switchval));
			xmlWriter.WriteEndElement();
		}

		private static void Class_Features(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("features");
			foreach (ClassFeature classFeature in classModel.ClassFeatures)
			{
				xmlWriter.WriteStartElement(ClassFeatureNametoXml(classFeature));
				Features_Level(xmlWriter, classFeature);
				Features_Name(xmlWriter, classFeature);
				if (classFeature.IsSpecializationChoice)
					Features_SpecializationChoice(xmlWriter, classFeature);
				if (classFeature.ClassSpecialization != null)
					if (!string.IsNullOrEmpty(classFeature.ClassSpecialization.Name))
						Features_Specialization(xmlWriter, classFeature);
				Features_Text(xmlWriter, classFeature);
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement();
		}

		private static void Features_Level(XmlWriter xmlWriter, ClassFeature classFeature)
		{
			xmlWriter.WriteStartElement("level");
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteString(classFeature.Level.ToString());
			xmlWriter.WriteEndElement();
		}

		private static void Features_Name(XmlWriter xmlWriter, ClassFeature classFeature)
		{
			xmlWriter.WriteStartElement("name");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString(classFeature.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Features_SpecializationChoice(XmlWriter xmlWriter, ClassFeature classFeature)
		{
			int SpecializationChoice = 0;
			xmlWriter.WriteStartElement("specializationchoice");
			CommonMethods.Type_Number(xmlWriter);
			if (classFeature.IsSpecializationChoice)
				SpecializationChoice = 1;
			xmlWriter.WriteValue(SpecializationChoice);
			xmlWriter.WriteEndElement();
		}

		private static void Features_Specialization(XmlWriter xmlWriter, ClassFeature classFeature)
		{
			xmlWriter.WriteStartElement("specialization");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString(classFeature.ClassSpecialization.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Features_Text(XmlWriter xmlWriter, ClassFeature classFeature)
		{
			NPCController npcController = new NPCController();
			xmlWriter.WriteStartElement("text");
			CommonMethods.Type_FormattedText(xmlWriter);
			xmlWriter.WriteRaw(npcController.GenerateFantasyGroundsDescriptionXML(classFeature.Description));
			xmlWriter.WriteEndElement();
		}

		private static string ClassFeatureToXml(ClassFeature classFeature)
		{
			string feature = classFeature.Name.Replace(" ", "").Replace("'", "").Replace("-", "").ToLower();
			return "....features." + feature + classFeature.Level;
		}

		private static string ClassFeatureNametoXml(ClassFeature classFeature)
		{
			string feature = classFeature.Name.Replace(" ", "").Replace("'", "").Replace("-", "").ToLower();
			return feature + classFeature.Level;
		}

		private static void Class_Abilities(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("abilities");
			Class_Abilities_Specializations(xmlWriter, classModel);
			xmlWriter.WriteEndElement();
		}

		private static void Class_Equipment(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("equipment");
			Equipment_Standard(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void Class_Abilities_Specializations(XmlWriter xmlWriter, ClassModel classModel)
		{
			if (classModel.ClassSpecializations == null)
				return;
			foreach (ClassSpecialization specialization in classModel.ClassSpecializations)
			{
				xmlWriter.WriteStartElement(SpecializationToXML(specialization));
				Abilities_Specializations_Level(xmlWriter, specialization);
				Abilities_Specializations_Name(xmlWriter, specialization);
				Abilities_Text(xmlWriter, specialization, classModel);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Abilities_Specializations_Level(XmlWriter xmlWriter, ClassSpecialization specialization)
		{
			xmlWriter.WriteStartElement("level");
			xmlWriter.WriteValue(specialization.Level);
			xmlWriter.WriteEndElement();
		}

		private static void Abilities_Specializations_Name(XmlWriter xmlWriter, ClassSpecialization specialization)
		{
			xmlWriter.WriteStartElement("name");
			xmlWriter.WriteString(specialization.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Abilities_Text(XmlWriter xmlWriter, ClassSpecialization specialization, ClassModel classModel)
		{
			NPCController npcController = new NPCController();
			xmlWriter.WriteStartElement("text");
			xmlWriter.WriteAttributeString("text", "formattedtext");
			xmlWriter.WriteRaw(npcController.GenerateFantasyGroundsDescriptionXML(specialization.Description));
			Text_Features(xmlWriter);
			Text_Linklist(xmlWriter, classModel);
			xmlWriter.WriteEndElement();
		}

		private static void Text_Features(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("h");
			xmlWriter.WriteString("Features");
			xmlWriter.WriteEndElement();
		}

		private static void Text_Linklist(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("linklist");
			foreach (ClassFeature classFeature in classModel.ClassFeatures)
			{
				xmlWriter.WriteStartElement("link");
				xmlWriter.WriteAttributeString("class", "reference_classfeature");
				xmlWriter.WriteAttributeString("recordname", ClassFeatureToXml(classFeature));
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement();
		}

		private static void Equipment_Standard(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("standard");
			Equipment_Standard_Group(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void Equipment_Standard_Group(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("group");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("standard");
			xmlWriter.WriteEndElement();
		}

		

		private static string SpecializationToXML(ClassSpecialization specialization)
		{
			string SpecNameToXML = specialization.Name.Replace(" ", "").Replace("'", "").ToLower();
			return SpecNameToXML;
		}

		
	}
}