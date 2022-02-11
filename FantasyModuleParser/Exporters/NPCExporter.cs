using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Skills;
using log4net;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	public class NPCExporter
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(NPCExporter));

		public static void DatabaseXML_Root_Npc(XmlWriter xmlWriter, ModuleModel module)
		{
			if (module.IncludeNPCs)
			{
				xmlWriter.WriteStartElement("npc"); /* <root version="4.0"> <reference> <npcdata> */
				NpcData_Category(xmlWriter, module);
				xmlWriter.WriteEndElement();
			}
		}

		public static void DatabaseXML_Root_Lists_NPClists(XmlWriter xmlWriter, ModuleModel module)
		{
			List<NPCModel> FatNPCList = CommonMethods.GenerateFatNPCList(module);
			FatNPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
			if (module.IncludeNPCs)
			{
				xmlWriter.WriteStartElement("npclists");
				Npclists_Npcs(xmlWriter, module);
				Npclists_ByLetter(xmlWriter, module, FatNPCList);
				Npclists_ByLevel(xmlWriter, module, FatNPCList);
				Npclists_ByType(xmlWriter, module, FatNPCList);
				xmlWriter.WriteEndElement();
			}
		}

		public static void Save_NPC_Tokens(ModuleModel module, SettingsService settings)
		{
			SettingsModel settingsModel = settings.Load();

			List<NPCModel> FatNPCList = CommonMethods.GenerateFatNPCList(module);
			FatNPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));

			foreach (NPCModel npcModel in FatNPCList)
			{
				if (module.IncludeTokens)
				{
					if (!string.IsNullOrEmpty(npcModel.NPCToken))
					{
						string Filename = Path.GetFileName(npcModel.NPCToken);
						string NPCTokenFileName = Path.Combine(settingsModel.FGModuleFolderLocation, module.ModFilename, "tokens", Filename);
						string NPCTokenDirectory = Path.Combine(settingsModel.FGModuleFolderLocation, module.ModFilename, "tokens");
						if (Directory.Exists(NPCTokenDirectory))
						{
							if (File.Exists(NPCTokenFileName))
							{
								File.Delete(NPCTokenFileName);
							}
						}
						else
						{
							Directory.CreateDirectory(NPCTokenDirectory);
						}
						File.Copy(npcModel.NPCToken, NPCTokenFileName);
					}
				}
			}
		}

		public static void Save_NPC_Images(ModuleModel module, SettingsService settings)
		{
			SettingsModel settingsModel = settings.Load();

			List<NPCModel> FatNPCList = CommonMethods.GenerateFatNPCList(module);
			FatNPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));

			foreach (NPCModel npcModel in FatNPCList)
			{
				if (module.IncludeImages)
				{
					if (!string.IsNullOrEmpty(npcModel.NPCImage))
					{
						string Filename = Path.GetFileName(npcModel.NPCImage).Replace("-", "").Replace(" ", "").Replace(",", "");
						string NPCImageFileName = Path.Combine(settingsModel.FGModuleFolderLocation, module.ModFilename, "images", Filename);
						string NPCImageDirectory = Path.Combine(settingsModel.FGModuleFolderLocation, module.ModFilename, "images");
						if (Directory.Exists(NPCImageDirectory))
						{
							if (File.Exists(NPCImageFileName))
							{
								File.Delete(NPCImageFileName);
							}
						}
						else
						{
							Directory.CreateDirectory(NPCImageDirectory);
						}
						if (npcModel.NPCImage.StartsWith("file:///"))
						{
							npcModel.NPCImage = npcModel.NPCImage.Remove(0, 8);
						}
						File.Copy(npcModel.NPCImage, NPCImageFileName);
					}
				}
			}
		}

		private static void NpcData_Category(XmlWriter xmlWriter, ModuleModel module)
		{
			foreach (CategoryModel category in module.Categories)
			{
				xmlWriter.WriteStartElement("category");
				xmlWriter.WriteAttributeString("name", category.Name);
				CommonMethods.BaseIcon_DecalIcon(xmlWriter);
				NpcData_Category_Npc(xmlWriter, module);
				xmlWriter.WriteEndElement();
			}
		}

		private static void NpcData_Category_Npc(XmlWriter xmlWriter, ModuleModel module)
		{
			List<NPCModel> FatNPCList = CommonMethods.GenerateFatNPCList(module);
			FatNPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
			int npcID = 1;
			foreach (NPCModel npcModel in FatNPCList)
			{
				xmlWriter.WriteStartElement(NPCNameToXMLFormat(npcModel));
				/* TO DO: Make WriteModuleLocked into WriteNPCLocked */
				CommonMethods.WriteModuleLocked(xmlWriter);
				WriteAbilities(xmlWriter, npcModel);
				WriteAC(xmlWriter, npcModel);
				WriteActions(xmlWriter, npcModel);
				WriteAlignment(xmlWriter, npcModel);
				WriteConditionImmunities(xmlWriter, npcModel);
				WriteCR(xmlWriter, npcModel);
				WriteDamageImmunities(xmlWriter, npcModel);
				WriteDamageResistances(xmlWriter, npcModel);
				WriteDamageVulnerabilities(xmlWriter, npcModel);
				WriteHP(xmlWriter, npcModel);
				WriteLairActions(xmlWriter, npcModel);
				WriteLanguages(xmlWriter, npcModel);
				WriteLegendaryActions(xmlWriter, npcModel);
				WriteName(xmlWriter, npcModel);
				WriteReactions(xmlWriter, npcModel);
				WriteSavingThrows(xmlWriter, npcModel);
				WriteSenses(xmlWriter, npcModel);
				WriteSize(xmlWriter, npcModel);
				WriteSkills(xmlWriter, npcModel);
				WriteSpeed(xmlWriter, npcModel);
				WriteText(xmlWriter, npcModel);
				WriteToken(xmlWriter, npcModel, module);
				WriteType(xmlWriter, npcModel);
				WriteTraits(xmlWriter, npcModel);
				WriteXP(xmlWriter, npcModel);
				xmlWriter.WriteEndElement();
				npcID++;
			}
		}

		private static void Npclists_ByType(XmlWriter xmlWriter, ModuleModel module, List<NPCModel> FatNPCList)
		{
			xmlWriter.WriteStartElement("bytype");
			Xml_Description_Npcs(xmlWriter);
			Npclists_ByType_Groups(xmlWriter, module, FatNPCList);
			xmlWriter.WriteEndElement();
		}

		private static void Npclists_ByType_Groups(XmlWriter xmlWriter, ModuleModel module, List<NPCModel> FatNPCList)
		{
			xmlWriter.WriteStartElement("groups");
			CreateReferenceByType(xmlWriter, module, FatNPCList);
			xmlWriter.WriteEndElement();
		}

		private static void Npclists_ByLevel(XmlWriter xmlWriter, ModuleModel module, List<NPCModel> FatNPCList)
		{
			xmlWriter.WriteStartElement("bylevel");
			Xml_Description_Npcs(xmlWriter);
			Npclists_ByLevel_Groups(xmlWriter, module, FatNPCList);
			xmlWriter.WriteEndElement();
		}

		private static void Npclists_ByLevel_Groups(XmlWriter xmlWriter, ModuleModel module, List<NPCModel> FatNPCList)
		{
			xmlWriter.WriteStartElement("groups");
			CreateReferenceByCR(xmlWriter, module, FatNPCList);
			xmlWriter.WriteEndElement();
		}

		private static void Npclists_ByLetter(XmlWriter xmlWriter, ModuleModel module, List<NPCModel> FatNPCList)
		{
			xmlWriter.WriteStartElement("byletter");
			Xml_Description_Npcs(xmlWriter);
			Npclists_ByLetter_Groups(xmlWriter, module, FatNPCList);
			xmlWriter.WriteEndElement();
		}

		private static void Npclists_ByLetter_Groups(XmlWriter xmlWriter, ModuleModel module, List<NPCModel> FatNPCList)
		{
			xmlWriter.WriteStartElement("groups");
			CreateReferenceByFirstLetter(xmlWriter, module, FatNPCList);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Description_Npcs(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("description");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString("NPCs");
			xmlWriter.WriteEndElement();
		}

		private static void Npclists_Npcs(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("npcs");
			CommonMethods.Xml_Name_Npcs(xmlWriter);
			Npclists_Npcs_Index(xmlWriter, module);
			xmlWriter.WriteEndElement();
		}

		private static void Npclists_Npcs_Index(XmlWriter xmlWriter, ModuleModel module)
		{
			xmlWriter.WriteStartElement("index");
			CommonMethods.WriteIDLinkList(xmlWriter, module, "id-0001", "lists.npclists.byletter@" + module.Name, "NPCs - Alphabetical Index");
			CommonMethods.WriteIDLinkList(xmlWriter, module, "id-0002", "lists.npclists.bylevel@" + module.Name, "NPCs - Challenge Rating Index");
			CommonMethods.WriteIDLinkList(xmlWriter, module, "id-0003", "lists.npclists.bytype@" + module.Name, "NPCs - Class Index");
			xmlWriter.WriteEndElement();
		}

		private static void NPCLocation(XmlWriter xmlWriter, ModuleModel moduleModel, List<NPCModel> NPCList)
		{
			foreach (NPCModel npc in NPCList)
			{
				xmlWriter.WriteStartElement(NPCNameToXMLFormat(npc));
				Npcname_Link(xmlWriter, moduleModel, npc);
				CommonMethods.Xml_Source_TypeNumber_Blank(xmlWriter);
				xmlWriter.WriteEndElement();
			}
		}

		private static void Npcname_Link(XmlWriter xmlWriter, ModuleModel moduleModel, NPCModel npc)
		{
			xmlWriter.WriteStartElement("link");
			CommonMethods.Type_WindowReference(xmlWriter);
			Class_NPC(xmlWriter);
			NPC_Recordname(xmlWriter, moduleModel, npc);
			CommonMethods.Xml_Description_Field_Name(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void NPC_Recordname(XmlWriter xmlWriter, ModuleModel moduleModel, NPCModel npc)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("npc." + NPCNameToXMLFormat(npc) + "@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Class_NPC(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("class");
			xmlWriter.WriteString("npc");
			xmlWriter.WriteEndElement();
		}

		private static void CreateReferenceByFirstLetter(XmlWriter xmlWriter, ModuleModel moduleModel, List<NPCModel> NPCList)
		{
			NPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
			var AlphabetList = NPCList.GroupBy(x => x.NPCName.ToUpper()[0]).Select(x => x.ToList()).ToList();
			foreach (List<NPCModel> npcList in AlphabetList)
			{
				string actualLetter = npcList[0].NPCName[0] + "";
				ProcessNPCListByLetter(xmlWriter, moduleModel, actualLetter, npcList);
			}
		}
		
		private static void ProcessNPCListByLetter(XmlWriter xmlWriter, ModuleModel moduleModel, string actualLetter, List<NPCModel> NPCList)
		{
			xmlWriter.WriteStartElement("typeletter" + actualLetter);
			CommonMethods.Xml_Description_ActualLetter(xmlWriter, actualLetter);
			Xml_Index_NPCLocation(xmlWriter, moduleModel, NPCList);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Index_NPCLocation(XmlWriter xmlWriter, ModuleModel moduleModel, List<NPCModel> NPCList)
		{
			xmlWriter.WriteStartElement("index");
			NPCLocation(xmlWriter, moduleModel, NPCList);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Description_ActualCR(XmlWriter xmlWriter, string actualCR)
		{
			xmlWriter.WriteStartElement("description");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString("CR " + actualCR);
			xmlWriter.WriteEndElement();
		}

		private static void Xml_Description_ActualType(XmlWriter xmlWriter, string actualType)
		{
			xmlWriter.WriteStartElement("description"); /* <type_NPCType> <description> */
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString(actualType);
			xmlWriter.WriteEndElement(); /* <type_NPCType> <description> </description> */
		}

		private static void CreateReferenceByCR(XmlWriter xmlWriter, ModuleModel moduleModel, List<NPCModel> NPCList)
		{
			NPCList.Sort((npcOne, npcTwo) => npcOne.ChallengeRating.CompareTo(npcTwo.ChallengeRating));
			var CRList = NPCList.GroupBy(x => x.ChallengeRating.ToUpper()[0]).Select(x => x.ToList()).ToList();
			foreach (List<NPCModel> npcList in CRList)
			{
				string actualCR = npcList[0].ChallengeRating + "";
				ProcessNPCListByCR(xmlWriter, moduleModel, actualCR, npcList);
			}
		}
		
		private static void ProcessNPCListByCR(XmlWriter xmlWriter, ModuleModel moduleModel, string actualCR, List<NPCModel> NPCList)
		{
			if (actualCR == "1/8")
			{
				xmlWriter.WriteStartElement("CR0125");
			}
			else if (actualCR == "1/4")
			{
				xmlWriter.WriteStartElement("CR025");
			}
			else if (actualCR == "1/2")
			{
				xmlWriter.WriteStartElement("CR05");
			}
			else
			{
				xmlWriter.WriteStartElement("CR" + actualCR);
			}
			Xml_Description_ActualCR(xmlWriter, actualCR);
			Xml_Index_NPCLocation(xmlWriter, moduleModel, NPCList);
			xmlWriter.WriteEndElement();
		}

		private static void CreateReferenceByType(XmlWriter xmlWriter, ModuleModel moduleModel, List<NPCModel> NPCList)
		{
			NPCList.Sort((npcOne, npcTwo) => npcOne.NPCType.CompareTo(npcTwo.NPCType));
			var TypeList = NPCList.GroupBy(x => x.NPCType.ToLower()[0]).Select(x => x.ToList()).ToList();
			foreach (List<NPCModel> npcList in TypeList)
			{
				string actualType = npcList[0].NPCType + "";
				ProcessNPCListByType(xmlWriter, moduleModel, actualType, npcList);
			}
		}
		
		private static void ProcessNPCListByType(XmlWriter xmlWriter, ModuleModel moduleModel, string actualType, List<NPCModel> NPCList)
		{
			xmlWriter.WriteStartElement("type_" + NPCTypeToXMLFormat(actualType));
			Xml_Description_ActualType(xmlWriter, actualType);
			Xml_Index_NPCLocation(xmlWriter, moduleModel, NPCList);
			xmlWriter.WriteEndElement();
		}

		static public string NPCNameToXMLFormat(NPCModel npcModel)
		{
			string name = npcModel.NPCName.ToLower();
			return name.Replace(" ", "_").Replace(",", "").Replace("(", "_").Replace(")", "");
		}
		
		private static string NPCTypeToXMLFormat(string actualType)
		{
			string npcType = actualType.ToLower();
			return npcType.Replace(" ", "");
		}
		
		static public void SortNPCListByCategory(XmlWriter xmlWriter, NPCModel npcModel, ModuleModel moduleModel, List<NPCModel> NPCList)
		{
			NPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
			var AlphabetList = NPCList.GroupBy(x => x.NPCName.ToUpper()[0]).Select(x => x.ToList()).ToList();
			foreach (List<NPCModel> npcList in AlphabetList)
			{
				string actualLetter = npcList[0].NPCName[0] + "";
				ProcessNPCListByCategoryLetter(xmlWriter, npcModel, moduleModel);
			}
		}
		
		static private void ProcessNPCListByCategoryLetter(XmlWriter xmlWriter, NPCModel npcModel, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement(NPCNameToXMLFormat(npcModel));
			NPCName_Link(xmlWriter, npcModel, moduleModel);
			NPCName_Source(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void NPCName_Link(XmlWriter xmlWriter, NPCModel npcModel, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("link");
			CommonMethods.Type_WindowReference(xmlWriter);
			Link_Class(xmlWriter);
			Link_RecordName(xmlWriter, npcModel, moduleModel);
			Link_Description(xmlWriter);
			xmlWriter.WriteEndElement();
		}
		
		private static void Link_Class(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("class");
			xmlWriter.WriteString("imagewindow");
			xmlWriter.WriteEndElement();
		}

		private static void Link_RecordName(XmlWriter xmlWriter, NPCModel npcModel, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("image." + NPCNameToXMLFormat(npcModel) + "@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
		}

		private static void Link_Description(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("description");
			Description_Field(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		private static void Description_Field(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("field");
			xmlWriter.WriteString("name");
			xmlWriter.WriteEndElement();
		}

		private static void NPCName_Source(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("source");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		static public void WriteAbilities(XmlWriter xmlWriter, NPCModel npcModel)
		{
			int ChaBonus = -5 + (npcModel.AttributeCha / 2);
			int ConBonus = -5 + (npcModel.AttributeCon / 2);
			int DexBonus = -5 + (npcModel.AttributeDex / 2);
			int IntBonus = -5 + (npcModel.AttributeInt / 2);
			int StrBonus = -5 + (npcModel.AttributeStr / 2);
			int WisBonus = -5 + (npcModel.AttributeWis / 2);

			string ChaModifier;
			string ConModifier;
			string DexModifier;
			string IntModifier;
			string StrModifier;
			string WisModifier;

			if (npcModel.AttributeCha >= 10)
			{
				ChaModifier = "+";
			}
			else
			{
				ChaModifier = "";
			}

			if (npcModel.AttributeCon >= 10)
			{
				ConModifier = "+";
			}
			else
			{
				ConModifier = "";
			}

			if (npcModel.AttributeDex >= 10)
			{
				DexModifier = "+";
			}
			else
			{
				DexModifier = "";
			}

			if (npcModel.AttributeInt >= 10)
			{
				IntModifier = "+";
			}
			else
			{
				IntModifier = "";
			}

			if (npcModel.AttributeStr >= 10)
			{
				StrModifier = "+";
			}
			else
			{
				StrModifier = "";
			}

			if (npcModel.AttributeWis >= 10)
			{
				WisModifier = "+";
			}
			else
			{
				WisModifier = "";
			}

			xmlWriter.WriteStartElement("abilities"); /* <abilities> */
			xmlWriter.WriteStartElement("charisma"); /* <abilities> <charisma> */
			xmlWriter.WriteStartElement("bonus"); /* <abilities> <charisma> <bonus> */
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteValue(ChaBonus);
			xmlWriter.WriteEndElement(); /* <abilities> <charisma> <bonus> </bonus> */
			xmlWriter.WriteStartElement("modifier"); /* <abilities> <charisma> <modifier> */
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteValue(ChaModifier + ChaBonus);
			xmlWriter.WriteEndElement(); /* <abilities> <charisma> <modifier> </modifier> */
			xmlWriter.WriteStartElement("score"); /* <abilities> <charisma> <score> */
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteValue(npcModel.AttributeCha);
			xmlWriter.WriteEndElement(); /* <abilities> <charisma> <score> </score> */
			xmlWriter.WriteEndElement(); /* <abilities> <charisma> </charisma>> */
			xmlWriter.WriteStartElement("constitution"); /* <abilities> <constitution> */
			xmlWriter.WriteStartElement("bonus"); /* <abilities> <constitution> <bonus> */
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteValue(ConBonus);
			xmlWriter.WriteEndElement(); /* <abilities> <constitution> <bonus> </bonus> */
			xmlWriter.WriteStartElement("modifier"); /* <abilities> <constitution> <modifier> */
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteValue(ConModifier + ConBonus);
			xmlWriter.WriteEndElement(); /* <abilities> <constitution> <modifier> </modifier> */
			xmlWriter.WriteStartElement("score"); /* <abilities> <constitution> <score> */
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteValue(npcModel.AttributeCon);
			xmlWriter.WriteEndElement(); /* <abilities> <constitution> <score> </score>*/
			xmlWriter.WriteEndElement(); /* <abilities> <constitution> </constitution> */
			xmlWriter.WriteStartElement("dexterity"); /* <abilities> <dexterity> */
			xmlWriter.WriteStartElement("bonus"); /* <abilities> <dexterity> <bonus> */
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteValue(DexBonus);
			xmlWriter.WriteEndElement(); /* <abilities> <dexterity> <bonus> </bonus> */
			xmlWriter.WriteStartElement("modifier"); /* <abilities> <dexterity> <modifier> */
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteValue(DexModifier + DexBonus);
			xmlWriter.WriteEndElement(); /* <abilities> <dexterity> <modifier> </modifier> */
			xmlWriter.WriteStartElement("score"); /* <abilities> <dexterity> <score> */
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteValue(npcModel.AttributeDex);
			xmlWriter.WriteEndElement(); /* <abilities> <dexterity> <score> </score> */
			xmlWriter.WriteEndElement(); /* <abilities> <dexterity> </dexterity> */
			xmlWriter.WriteStartElement("intelligence"); /* <abilities> <intelligence> */
			xmlWriter.WriteStartElement("bonus"); /* <abilities> <intelligence> <bonus> */
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteValue(IntBonus);
			xmlWriter.WriteEndElement(); /* <abilities> <intelligence> <bonus> </bonus> */
			xmlWriter.WriteStartElement("modifier"); /* <abilities> <intelligence> <modifier> */
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteValue(IntModifier + IntBonus);
			xmlWriter.WriteEndElement(); /* <abilities> <intelligence> <modifier> </modifier> */
			xmlWriter.WriteStartElement("score"); /* <abilities> <intelligence> <score> */
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteValue(npcModel.AttributeInt);
			xmlWriter.WriteEndElement(); /* <abilities> <intelligence> <score> </score> */
			xmlWriter.WriteEndElement(); /* <abilities> <intelligence> </intelligence> */
			xmlWriter.WriteStartElement("strength");
			xmlWriter.WriteStartElement("bonus");
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteValue(StrBonus);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("modifier");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteValue(StrModifier + StrBonus);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("score");
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteValue(npcModel.AttributeStr); // Add Attibute value
			xmlWriter.WriteEndElement(); // Close </score>
			xmlWriter.WriteEndElement(); // Close </strength>
			xmlWriter.WriteStartElement("wisdom"); // Open <wisdom>
			xmlWriter.WriteStartElement("bonus"); // Open <bonus>
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteValue(WisBonus); // Add bonus value
			xmlWriter.WriteEndElement(); // Close </bonus>
			xmlWriter.WriteStartElement("modifier"); // Open <modifier>
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteValue(WisModifier + WisBonus); // Add bonus value with + or minus
			xmlWriter.WriteEndElement(); // Close </modifier>
			xmlWriter.WriteStartElement("score"); // Open <score>
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteValue(npcModel.AttributeWis); // Add Attibute value
			xmlWriter.WriteEndElement(); // Close </score>
			xmlWriter.WriteEndElement(); // Close </intelligence>
			xmlWriter.WriteEndElement(); // Close </abilities>
		}
		
		static public void WriteAC(XmlWriter xmlWriter, NPCModel npcModel)
		{
			string[] acArray = npcModel.AC.Split('(');
			string acValue = acArray[0].Trim();
			string acDescription = acArray.Length >= 2 ? "(" + acArray[1] : "";

			xmlWriter.WriteStartElement("ac");
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteValue(acValue);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("actext");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteValue(acDescription);
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteActions(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("actions");
			int actionID = 1;
			foreach (ActionModelBase action in npcModel.NPCActions)
			{
				xmlWriter.WriteStartElement("id-" + actionID.ToString("D4"));
				xmlWriter.WriteStartElement("desc");
				CommonMethods.Type_String(xmlWriter);
				xmlWriter.WriteString(action.ActionDescription);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("name");
				CommonMethods.Type_String(xmlWriter);
				xmlWriter.WriteString(action.ActionName);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				actionID = ++actionID;
			}
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteAlignment(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("alignment");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString(npcModel.Alignment);
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteConditionImmunities(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			xmlWriter.WriteStartElement("conditionimmunities");
			CommonMethods.Type_String(xmlWriter);
			if (npcModel.ConditionImmunityModelList != null)
			{
				foreach (SelectableActionModel condition in npcModel.ConditionImmunityModelList)
				{
					if (condition.Selected)
					{
						stringBuilder.Append(condition.ActionDescription.ToLower()).Append(", ");
					}
				}
			}
			if (npcModel.ConditionOther)
			{
				stringBuilder.Append(npcModel.ConditionOtherText.ToLower() + ", ");
			}
			if (stringBuilder.Length >= 2)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			xmlWriter.WriteValue(stringBuilder.ToString().Trim());
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteCR(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("cr");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString(npcModel.ChallengeRating);
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteHP(XmlWriter xmlWriter, NPCModel npcModel)
		{
			if (npcModel.HP == null)
			{
				npcModel.HP = "0 (0)";
			}
			string[] hpArray = npcModel.HP.Split('(');
			string hpValue = hpArray[0].Trim();
			string hpDieBreakdown = "";
			if (hpArray.Length == 2)
			{
				hpDieBreakdown = "(" + hpArray[1];
			}
			xmlWriter.WriteStartElement("hd");
			CommonMethods.Type_String(xmlWriter);
			if (hpArray.Length == 2)
			{
				xmlWriter.WriteString(hpDieBreakdown);
			}
			else
			{
				xmlWriter.WriteString("");
			}
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("hp");
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteString(hpValue);
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteLairActions(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("lairactions");
			int actionID = 1;
			if (npcModel.LairActions != null)
			{
				foreach (LairAction lairaction in npcModel.LairActions)
				{
					xmlWriter.WriteStartElement("id-" + actionID.ToString("D4"));
					xmlWriter.WriteStartElement("desc");
					CommonMethods.Type_String(xmlWriter);
					xmlWriter.WriteString(lairaction.ActionDescription);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("name");
					CommonMethods.Type_String(xmlWriter);
					xmlWriter.WriteString(lairaction.ActionName);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					actionID = ++actionID;
				}
			}
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteLanguages(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilderOption = new StringBuilder();
			foreach (LanguageModel languageModel in npcModel.StandardLanguages)
			{
				if (languageModel.Selected == true)
				{
					stringBuilder.Append(languageModel.Language).Append(", ");
				}
			}
			foreach (LanguageModel languageModel in npcModel.ExoticLanguages)
			{
				if (languageModel.Selected == true)
				{
					stringBuilder.Append(languageModel.Language).Append(", ");
				}
			}
			foreach (LanguageModel languageModel in npcModel.MonstrousLanguages)
			{
				if (languageModel.Selected == true)
				{
					stringBuilder.Append(languageModel.Language).Append(", ");
				}
			}
			if (npcModel.UserLanguages != null && npcModel.UserLanguages.Count > 0)
			{
				foreach (LanguageModel languageModel in npcModel.UserLanguages)
				{
					if (languageModel.Selected == true)
					{
						stringBuilder.Append(languageModel.Language).Append(", ");
					}

				}
			}
			if (npcModel.Telepathy)
			{
				stringBuilder.Append("telepathy " + npcModel.TelepathyRange).Append(", ");
			}
			if (stringBuilder.Length >= 2)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			if (npcModel.LanguageOptions == "No special conditions" || npcModel.LanguageOptions == null)
			{
				stringBuilderOption.Append(stringBuilder);
			}
			else if (npcModel.LanguageOptions == "Speaks no languages")
			{
				stringBuilderOption.Append('-');
			}
			else if (npcModel.LanguageOptions == "Speaks all languages")
			{
				stringBuilderOption.Append("all").Append(", ");
				if (npcModel.Telepathy)
				{
					stringBuilderOption.Append("telepathy " + npcModel.TelepathyRange).Append(", ");
				}
				stringBuilderOption.Remove(stringBuilderOption.Length - 2, 2);
			}
			else if (npcModel.LanguageOptions == "Can't speak; Knows selected languages")
			{
				stringBuilderOption.Append("understands" + stringBuilder + " but can't speak").Append(", ");
				if (npcModel.Telepathy)
				{
					stringBuilderOption.Append("telepathy " + npcModel.TelepathyRange).Append(", ");
				}
				stringBuilderOption.Remove(stringBuilderOption.Length - 2, 2);
			}
			else if (npcModel.LanguageOptions == "Can't speak; Knows creator's languages")
			{
				stringBuilderOption.Append("understands the languages of its creator but can't speak").Append(", ");
				if (npcModel.Telepathy)
				{
					stringBuilderOption.Append("telepathy " + npcModel.TelepathyRange).Append(", ");
				}
				stringBuilderOption.Remove(stringBuilderOption.Length - 2, 2);
			}
			else if (npcModel.LanguageOptions == "Can't speak; Knows languages known in life")
			{
				stringBuilderOption.Append("Understands all languages it spoke in life but can't speak").Append(", ");
				if (npcModel.Telepathy)
				{
					stringBuilderOption.Append("telepathy " + npcModel.TelepathyRange).Append(", ");
				}
				stringBuilderOption.Remove(stringBuilderOption.Length - 2, 2);
			}
			else if (npcModel.LanguageOptions == "Alternative language text (enter below)")
			{
				stringBuilderOption.Append(npcModel.LanguageOptionsText.ToString().Trim()).Append(", ");
				if (npcModel.Telepathy)
				{
					stringBuilderOption.Append("telepathy " + npcModel.TelepathyRange).Append(", ");
				}
				stringBuilderOption.Remove(stringBuilderOption.Length - 2, 2);
			}
			xmlWriter.WriteStartElement("languages");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteValue(stringBuilderOption.ToString());
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteLegendaryActions(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("legendaryactions");
			int actionID = 1;
			foreach (LegendaryActionModel legendaryaction in npcModel.LegendaryActions)
			{
				xmlWriter.WriteStartElement("id-" + actionID.ToString("D4"));
				xmlWriter.WriteStartElement("desc");
				CommonMethods.Type_String(xmlWriter);
				xmlWriter.WriteString(legendaryaction.ActionDescription);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("name");
				CommonMethods.Type_String(xmlWriter);
				xmlWriter.WriteString(legendaryaction.ActionName);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				actionID = ++actionID;
			}
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteName(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("name");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString(npcModel.NPCName);
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteReactions(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("reactions");
			int actionID = 1;
			foreach (ActionModelBase reaction in npcModel.Reactions)
			{
				xmlWriter.WriteStartElement("id-" + actionID.ToString("D4"));
				xmlWriter.WriteStartElement("desc");
				CommonMethods.Type_String(xmlWriter);
				xmlWriter.WriteString(reaction.ActionDescription);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("name");
				CommonMethods.Type_String(xmlWriter);
				xmlWriter.WriteString(reaction.ActionName);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				actionID = ++actionID;
			}
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteSavingThrows(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();

			if (npcModel.SavingThrowStr != 0 || npcModel.SavingThrowStrBool)
			{
				stringBuilder.Append("Str ").Append(npcModel.SavingThrowStr >= 0 ? "+" : "").Append(npcModel.SavingThrowStr).Append(", ");
			}
			if (npcModel.SavingThrowDex != 0 || npcModel.SavingThrowDexBool)
			{
				stringBuilder.Append("Dex ").Append(npcModel.SavingThrowDex >= 0 ? "+" : "").Append(npcModel.SavingThrowDex).Append(", ");
			}
			if (npcModel.SavingThrowCon != 0 || npcModel.SavingThrowConBool)
			{
				stringBuilder.Append("Con ").Append(npcModel.SavingThrowCon >= 0 ? "+" : "").Append(npcModel.SavingThrowCon).Append(", ");
			}
			if (npcModel.SavingThrowInt != 0 || npcModel.SavingThrowIntBool)
			{
				stringBuilder.Append("Int ").Append(npcModel.SavingThrowInt >= 0 ? "+" : "").Append(npcModel.SavingThrowInt).Append(", ");
			}
			if (npcModel.SavingThrowWis != 0 || npcModel.SavingThrowWisBool)
			{
				stringBuilder.Append("Wis ").Append(npcModel.SavingThrowWis >= 0 ? "+" : "").Append(npcModel.SavingThrowWis).Append(", ");
			}
			if (npcModel.SavingThrowCha != 0 || npcModel.SavingThrowChaBool)
			{
				stringBuilder.Append("Cha ").Append(npcModel.SavingThrowCha >= 0 ? "+" : "").Append(npcModel.SavingThrowCha).Append(", ");
			}

			if (stringBuilder.Length >= 2)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			string savingThrowString = stringBuilder.ToString().Trim();

			xmlWriter.WriteStartElement("savingthrows");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteValue(savingThrowString);
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteSenses(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(AppendSenses("darkvision ", npcModel.Darkvision, " ft."));
			stringBuilder.Append(AppendBlindSenses("blindsight ", npcModel.Blindsight, " ft."));
			stringBuilder.Append(AppendSenses("tremorsense ", npcModel.Tremorsense, " ft."));
			stringBuilder.Append(AppendSenses("truesight ", npcModel.Truesight, " ft."));
			stringBuilder.Append(AppendSenses("passive perception ", npcModel.PassivePerception, ""));
			if (stringBuilder.Length >= 2)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			string sensesString = stringBuilder.ToString().Trim();
			xmlWriter.WriteStartElement("senses");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteValue(sensesString);
			xmlWriter.WriteEndElement();
		}
		
		static private string AppendSenses(string senseName, int senseValue, string senseRange)
		{
			if (senseValue != 0)
			{
				string delimiter = ", ";
				return senseName + senseValue + senseRange + delimiter;
			}
			return "";
		}
		
		static private string AppendBlindSenses(string senseName, int senseValue, string senseRange)
		{
			NPCModel npcModel = new NPCModel();
			string delimiter = ", ";
			if (senseValue != 0 && npcModel.BlindBeyond == false)
			{
				return senseName + senseValue + senseRange + delimiter;
			}
			else if (senseValue != 0 && npcModel.BlindBeyond == true)
			{
				return senseName + senseValue + senseRange + " (blind beyond this radius)" + delimiter;
			}
			return "";
		}
		
		static public void WriteSpeed(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();

			if (npcModel.Speed > 0)
			{
				stringBuilder.Append(npcModel.Speed + " ft.").Append(", ");
			}
			if (npcModel.Burrow > 0)
			{
				stringBuilder.Append("burrow " + npcModel.Burrow + " ft.").Append(", ");
			}
			if (npcModel.Climb > 0)
			{
				stringBuilder.Append("climb " + npcModel.Climb + " ft.").Append(", ");
			}
			if (npcModel.Hover)
			{
				stringBuilder.Append("fly " + npcModel.Fly + " ft. (hover)").Append(", ");
			}
			if (npcModel.Fly > 0 && !npcModel.Hover)
			{
				stringBuilder.Append("fly " + npcModel.Fly + " ft.").Append(", ");
			}
			if (npcModel.Swim > 0)
			{
				stringBuilder.Append("swim " + npcModel.Swim + " ft.").Append(", ");
			}
			if (stringBuilder.Length >= 2)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			string speedString = stringBuilder.ToString().Trim();
			xmlWriter.WriteStartElement("speed");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteValue(speedString);
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteSize(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("size");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteValue(npcModel.Size);
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteType(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(npcModel.NPCType);
			if (npcModel.Tag != null)
			{
				stringBuilder.Append(" " + npcModel.Tag);
			}
			xmlWriter.WriteStartElement("type");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteValue(stringBuilder.ToString());
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteSkills(XmlWriter xmlWriter, NPCModel npcModel)
		{
			string skillsString = npcModel.SkillAttributesToString();
			xmlWriter.WriteStartElement("skills");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteValue(skillsString);
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteText(XmlWriter xmlWriter, NPCModel npcModel)
		{
			NPCController npcController = new NPCController();
			xmlWriter.WriteStartElement("text");
			CommonMethods.Type_FormattedText(xmlWriter);
			xmlWriter.WriteRaw(npcController.GenerateFantasyGroundsDescriptionXML(npcModel.Description));
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteToken(XmlWriter xmlWriter, NPCModel npcModel, ModuleModel moduleModel)
		{
			if (moduleModel.IncludeTokens && npcModel.NPCToken.Length > 2 )
			{
				xmlWriter.WriteStartElement("token");
				xmlWriter.WriteAttributeString("type", "token");
				xmlWriter.WriteValue("tokens\\" + Path.GetFileName(npcModel.NPCToken) + "@" + moduleModel.Name);
				xmlWriter.WriteEndElement();
			}			
		}
		
		static public void WriteTraits(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("traits");
			int actionID = 1;
			string innateName = "";
			string spellcastingName = "";

			if (npcModel.Traits != null)
			{
				foreach (ActionModelBase traits in npcModel.Traits)
				{
					xmlWriter.WriteStartElement("id-" + actionID.ToString("D4"));
					xmlWriter.WriteStartElement("desc");
					CommonMethods.Type_String(xmlWriter);
					xmlWriter.WriteString(traits.ActionDescription);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("name");
					CommonMethods.Type_String(xmlWriter);
					xmlWriter.WriteString(traits.ActionName);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					actionID = ++actionID;
				}
			}
			if (npcModel.Psionics)
			{
				innateName = "Innate Spellcasting (Psionics)";
			}
			else if (npcModel.InnateSpellcastingSection && !npcModel.Psionics)
			{
				innateName = "Innate Spellcasting";
			}
			if (innateName.Length > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(npcModel.InnateSpellcastingAbility))
				{
					stringBuilder.Append("The " + npcModel.NPCName.ToLower() + "'s innate spellcasting ability is " + npcModel.InnateSpellcastingAbility);
				}
				else
				{
					log.Warn("Innate Spellcasting Ability missing for " + npcModel.NPCName);
					MessageBox.Show("Please fill in the Innate Spellcasting Ability");
				}

				if (npcModel.InnateSpellSaveDC != 0)
				{
					stringBuilder.Append(" (spell save DC " + npcModel.InnateSpellSaveDC);
					if (npcModel.InnateSpellHitBonus != 0)
					{
						stringBuilder.Append("spell hit bonus ").Append(npcModel.InnateSpellHitBonus >= 0 ? "+" : "").Append(npcModel.InnateSpellHitBonus);
					}
					stringBuilder.Append(')');
				}
				else if (npcModel.InnateSpellHitBonus != 0)
				{
					stringBuilder.Append("(spell hit bonus ").Append(npcModel.InnateSpellHitBonus >= 0 ? "+" : "").Append(npcModel.InnateSpellHitBonus + ")");
				}
				stringBuilder.Append(". ");
				stringBuilder.Append("It can innately cast the following spells, " + npcModel.ComponentText + ":");
				if (npcModel.InnateAtWill != null)
				{
					stringBuilder.Append("\\rAt will: " + npcModel.InnateAtWill);
				}
				if (npcModel.FivePerDay != null)
				{
					stringBuilder.Append("\\r5/day each: " + npcModel.FivePerDay);
				}
				if (npcModel.FourPerDay != null)
				{
					stringBuilder.Append("\\r4/day each: " + npcModel.FourPerDay);
				}
				if (npcModel.ThreePerDay != null)
				{
					stringBuilder.Append("\\r3/day each: " + npcModel.ThreePerDay);
				}
				if (npcModel.TwoPerDay != null)
				{
					stringBuilder.Append("\\r2/day each: " + npcModel.TwoPerDay);
				}
				if (npcModel.OnePerDay != null)
				{
					stringBuilder.Append("\\r1/day each: " + npcModel.OnePerDay);
				}
				string innateCastingDescription = stringBuilder.ToString();

				xmlWriter.WriteStartElement("id-" + actionID.ToString("D4"));
				xmlWriter.WriteStartElement("desc");
				CommonMethods.Type_String(xmlWriter);
				xmlWriter.WriteString(innateCastingDescription);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("name");
				CommonMethods.Type_String(xmlWriter);
				xmlWriter.WriteString(innateName);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				actionID = ++actionID;
			}
			if (npcModel.SpellcastingSection)
			{
				spellcastingName = "Spellcasting";
			}
			if (spellcastingName.Length > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(npcModel.SpellcastingCasterLevel))
				{
					stringBuilder.Append("The " + npcModel.NPCName.ToLower() + " is a " + npcModel.SpellcastingCasterLevel + "-level spellcaster. ");
				}
				else
				{
					log.Warn("Spellcasting level is missing for " + npcModel.NPCName);
					MessageBox.Show("Please fill in the Spellcasting Level");
				}

				if (!string.IsNullOrEmpty(npcModel.SCSpellcastingAbility))
				{
					stringBuilder.Append("Its spellcasting ability is " + npcModel.SCSpellcastingAbility);
				}
				else
				{
					log.Warn("Spellcasting ability is missing for " + npcModel.NPCName);
					MessageBox.Show("Please fill in the Spellcasting Ability");
				}

				if (npcModel.SpellcastingSpellSaveDC != 0)
				{
					stringBuilder.Append(" (spell save DC " + npcModel.SpellcastingSpellSaveDC);
					if (npcModel.SpellcastingSpellHitBonus != 0)
					{
						stringBuilder.Append(", spell hit bonus ").Append(npcModel.SpellcastingSpellHitBonus >= 0 ? "+" : "").Append(npcModel.SpellcastingSpellHitBonus);
					}
					stringBuilder.Append(')');
				}
				stringBuilder.Append(". ");
				if (!string.IsNullOrEmpty(npcModel.SpellcastingSpellClass))
				{
					stringBuilder.Append("The " + npcModel.NPCName.ToLower() + " has the following " + npcModel.SpellcastingSpellClass.ToLower() + " spells prepared:");
				}
				else
				{
					stringBuilder.Append("The " + npcModel.NPCName.ToLower() + " has the following spells prepared:");
				}
				if (npcModel.CantripSpellList != null)
				{
					stringBuilder.Append("\\rCantrips (" + npcModel.CantripSpellSlots.ToLower() + "): " + npcModel.CantripSpellList.ToLower());
				}
				if (npcModel.FirstLevelSpellList != null)
				{
					stringBuilder.Append("\\r1st level (" + npcModel.FirstLevelSpellSlots.ToLower() + "): " + npcModel.FirstLevelSpellList.ToLower());
				}
				if (npcModel.SecondLevelSpellList != null)
				{
					stringBuilder.Append("\\r2nd level (" + npcModel.SecondLevelSpellSlots.ToLower() + "): " + npcModel.SecondLevelSpellList.ToLower());
				}
				if (npcModel.ThirdLevelSpellList != null)
				{
					stringBuilder.Append("\\r3rd level (" + npcModel.ThirdLevelSpellSlots.ToLower() + "): " + npcModel.ThirdLevelSpellList.ToLower());
				}
				if (npcModel.FourthLevelSpellList != null)
				{
					stringBuilder.Append("\\r4th level (" + npcModel.FourthLevelSpellSlots.ToLower() + "): " + npcModel.FourthLevelSpellList.ToLower());
				}
				if (npcModel.FifthLevelSpellList != null)
				{
					stringBuilder.Append("\\r5th level (" + npcModel.FifthLevelSpellSlots.ToLower() + "): " + npcModel.FifthLevelSpellList.ToLower());
				}
				if (npcModel.SixthLevelSpellList != null)
				{
					stringBuilder.Append("\\r6th level (" + npcModel.SixthLevelSpellSlots.ToLower() + "): " + npcModel.SixthLevelSpellList.ToLower());
				}
				if (npcModel.SeventhLevelSpellList != null)
				{
					stringBuilder.Append("\\r7th level (" + npcModel.SeventhLevelSpellSlots.ToLower() + "): " + npcModel.SeventhLevelSpellList.ToLower());
				}
				if (npcModel.EighthLevelSpellList != null)
				{
					stringBuilder.Append("\\r8th level (" + npcModel.EighthLevelSpellSlots.ToLower() + "): " + npcModel.EighthLevelSpellList.ToLower());
				}
				if (npcModel.NinthLevelSpellList != null)
				{
					stringBuilder.Append("\\r9th level (" + npcModel.NinthLevelSpellSlots.ToLower() + "): " + npcModel.NinthLevelSpellList.ToLower());
				}
				string spellcastingDescription = stringBuilder.ToString();
				xmlWriter.WriteStartElement("id-" + actionID.ToString("D4"));
				xmlWriter.WriteStartElement("desc");
				CommonMethods.Type_String(xmlWriter);
				xmlWriter.WriteString(spellcastingDescription);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("name");
				CommonMethods.Type_String(xmlWriter);
				xmlWriter.WriteString(spellcastingName);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				actionID = ++actionID;
			}
			xmlWriter.WriteEndElement();
		}
		
		static public void WriteXP(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("xp");
			CommonMethods.Type_Number(xmlWriter);
			xmlWriter.WriteValue(npcModel.XP);
			xmlWriter.WriteEndElement();
		}
		
		static private void WriteDamageImmunities(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("damageimmunities");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString(npcModel.UpdateDamageImmunities());
			xmlWriter.WriteEndElement();
		}

		static private void WriteDamageResistances(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("damageresistances");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteString(npcModel.UpdateDamageResistances());
			xmlWriter.WriteEndElement();
		}

		static private void WriteDamageVulnerabilities(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("damagevulnerabilities");
			CommonMethods.Type_String(xmlWriter);
			xmlWriter.WriteValue(npcModel.UpdateDamageVulnerabilities());
			xmlWriter.WriteEndElement();
		}
	}
}
