using FantasyModuleParser.Classes.Model;
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
			Class_Abilities(xmlWriter, classModel);
			xmlWriter.WriteEndElement();
		}

		private static void Class_Abilities(XmlWriter xmlWriter, ClassModel classModel)
		{
			xmlWriter.WriteStartElement("abilities");
			Class_Abilities_Specializations(xmlWriter, classModel);
			xmlWriter.WriteEndElement();
		}

		private static void Class_Abilities_Specializations(XmlWriter xmlWriter, ClassModel classModel)
		{
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

		private static string SpecializationToXML(ClassSpecialization specialization)
		{
			string SpecNameToXML = specialization.Name.Replace(" ", "").Replace("'", "").ToLower();
			return SpecNameToXML;
		}

		private static string ClassFeatureToXml(ClassFeature classFeature)
		{
			string feature = classFeature.Name.Replace(" ", "").Replace("'", "").Replace("-", "").ToLower();
			return "...." + "features." + feature + classFeature.Level;
		}
	}
}
