using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Skills;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace FantasyModuleParser.Exporters
{
	public class NPCExporter
	{
		static private void NPCLocation(XmlWriter xmlWriter, ModuleModel moduleModel, List<NPCModel> NPCList)
		{
			foreach (NPCModel npc in NPCList)
			{
				xmlWriter.WriteStartElement(NPCNameToXMLFormat(npc));
				xmlWriter.WriteStartElement("link");
				xmlWriter.WriteAttributeString("type", "windowreference");
				xmlWriter.WriteStartElement("class");
				xmlWriter.WriteString("npc");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("recordname");
				xmlWriter.WriteString("reference.npcdata." + NPCNameToXMLFormat(npc) + "@" + moduleModel.Name);
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
		static public void CreateReferenceByFirstLetter(XmlWriter xmlWriter, ModuleModel moduleModel, List<NPCModel> NPCList)
		{
			NPCList.Sort((npcOne, npcTwo) => npcOne.NPCName.CompareTo(npcTwo.NPCName));
			var AlphabetList = NPCList.GroupBy(x => x.NPCName.ToUpper()[0]).Select(x => x.ToList()).ToList();
			foreach (List<NPCModel> npcList in AlphabetList)
			{
				string actualLetter = npcList[0].NPCName[0] + "";
				ProcessNPCListByLetter(xmlWriter, moduleModel, actualLetter, npcList);
			}
		}
		static private void ProcessNPCListByLetter(XmlWriter xmlWriter, ModuleModel moduleModel, string actualLetter, List<NPCModel> NPCList)
		{
			xmlWriter.WriteStartElement("typeletter" + actualLetter);
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(actualLetter);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("index");
			NPCLocation(xmlWriter, moduleModel, NPCList);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
		}
		static public void CreateReferenceByCR(XmlWriter xmlWriter, ModuleModel moduleModel, List<NPCModel> NPCList)
		{
			NPCList.Sort((npcOne, npcTwo) => npcOne.ChallengeRating.CompareTo(npcTwo.ChallengeRating));
			var CRList = NPCList.GroupBy(x => x.ChallengeRating.ToUpper()[0]).Select(x => x.ToList()).ToList();
			foreach (List<NPCModel> npcList in CRList)
			{
				string actualCR = npcList[0].ChallengeRating + "";
				ProcessNPCListByCR(xmlWriter, moduleModel, actualCR, npcList);
			}
		}
		static private void ProcessNPCListByCR(XmlWriter xmlWriter, ModuleModel moduleModel, string actualCR, List<NPCModel> NPCList)
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
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString("CR " + actualCR);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("index");
			NPCLocation(xmlWriter, moduleModel, NPCList);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
		}
		static public void CreateReferenceByType(XmlWriter xmlWriter, ModuleModel moduleModel, List<NPCModel> NPCList)
		{
			NPCList.Sort((npcOne, npcTwo) => npcOne.NPCType.CompareTo(npcTwo.NPCType));
			var TypeList = NPCList.GroupBy(x => x.NPCType.ToLower()[0]).Select(x => x.ToList()).ToList();
			foreach (List<NPCModel> npcList in TypeList)
			{
				string actualType = npcList[0].NPCType + "";
				ProcessNPCListByType(xmlWriter, moduleModel, actualType, npcList);
			}
		}
		static private void ProcessNPCListByType(XmlWriter xmlWriter, ModuleModel moduleModel, string actualType, List<NPCModel> NPCList)
		{
			xmlWriter.WriteStartElement("type_" + NPCTypeToXMLFormat(actualType));
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(actualType);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("index");
			NPCLocation(xmlWriter, moduleModel, NPCList);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
		}
		static public string NPCNameToXMLFormat(NPCModel npcModel)
		{
			string name = npcModel.NPCName.ToLower();
			return name.Replace(" ", "_").Replace(",", "").Replace("(", "_").Replace(")", "");
		}
		static private string NPCTypeToXMLFormat(string actualType)
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
			xmlWriter.WriteStartElement("link");
			xmlWriter.WriteAttributeString("type", "windowreference");
			xmlWriter.WriteStartElement("class");
			xmlWriter.WriteString("imagewindow");
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("recordname");
			xmlWriter.WriteString("image." + NPCNameToXMLFormat(npcModel) + "@" + moduleModel.Name);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("description");
			xmlWriter.WriteStartElement("field");
			xmlWriter.WriteString("name");
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("source");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteEndElement();
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

			xmlWriter.WriteStartElement("abilities");
			xmlWriter.WriteStartElement("charisma");
			xmlWriter.WriteStartElement("bonus");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(ChaBonus);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("modifier");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(ChaModifier + ChaBonus);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("score");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(npcModel.AttributeCha);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("constitution");
			xmlWriter.WriteStartElement("bonus");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(ConBonus);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("modifier");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(ConModifier + ConBonus);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("score");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(npcModel.AttributeCon);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("dexterity");
			xmlWriter.WriteStartElement("bonus");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(DexBonus);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("modifier");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(DexModifier + DexBonus);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("score");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(npcModel.AttributeDex);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("intelligence");
			xmlWriter.WriteStartElement("bonus");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(IntBonus);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("modifier");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(IntModifier + IntBonus);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("score");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(npcModel.AttributeInt);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("strength");
			xmlWriter.WriteStartElement("bonus");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(StrBonus);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("modifier");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(StrModifier + StrBonus);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("score");
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(npcModel.AttributeStr); // Add Attibute value
			xmlWriter.WriteEndElement(); // Close </score>
			xmlWriter.WriteEndElement(); // Close </strength>
			xmlWriter.WriteStartElement("wisdom"); // Open <wisdom>
			xmlWriter.WriteStartElement("bonus"); // Open <bonus>
			xmlWriter.WriteAttributeString("type", "number"); // Add type="number"
			xmlWriter.WriteValue(WisBonus); // Add bonus value
			xmlWriter.WriteEndElement(); // Close </bonus>
			xmlWriter.WriteStartElement("modifier"); // Open <modifier>
			xmlWriter.WriteAttributeString("type", "string"); // Add type="string"
			xmlWriter.WriteValue(WisModifier + WisBonus); // Add bonus value with + or minus
			xmlWriter.WriteEndElement(); // Close </modifier>
			xmlWriter.WriteStartElement("score"); // Open <score>
			xmlWriter.WriteAttributeString("type", "number"); // Add type="number"
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
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(acValue);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("actext");
			xmlWriter.WriteAttributeString("type", "string");
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
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(action.ActionDescription);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("name");
				xmlWriter.WriteAttributeString("type", "string");
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
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(npcModel.Alignment);
			xmlWriter.WriteEndElement();
		}
		static public void WriteConditionImmunities(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			xmlWriter.WriteStartElement("conditionimmunities");
			xmlWriter.WriteAttributeString("type", "string");
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
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteString(npcModel.ChallengeRating);
			xmlWriter.WriteEndElement();
		}
		public void WriteDamageImmunities(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			xmlWriter.WriteStartElement("damageimmunities");
			xmlWriter.WriteAttributeString("type", "string");
			if (npcModel.DamageImmunityModelList != null)
			{
				foreach (SelectableActionModel damageImmunities in npcModel.DamageImmunityModelList)
				{
					if (damageImmunities.Selected)
					{
						stringBuilder.Append(damageImmunities.ActionDescription.ToLower()).Append(", ");
					}
				}
			}
			if (stringBuilder.Length >= 2)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			stringBuilder.Append("; ");
			if (npcModel.SpecialWeaponImmunityModelList != null)
			{
				foreach (SelectableActionModel specialWeaponImmunity in npcModel.SpecialWeaponImmunityModelList)
				{
					if (specialWeaponImmunity.Selected == true && specialWeaponImmunity.ActionName != "NoSpecial")
					{
						switch (specialWeaponImmunity.ActionName)
						{
							case "Nonmagical":
								Immunity = " from nonmagical attacks";
								break;
							case "NonmagicalSilvered":
								Immunity = " from nonmagical attacks that aren't silvered";
								break;
							case "NonmagicalAdamantine":
								Immunity = " from nonmagical attacks that aren't adamantine";
								break;
							case "NonmagicalColdForgedIron":
								Immunity = " from nonmagical attacks that aren't cold-forged iron";
								break;
						}
						foreach (SelectableActionModel specialWeaponDmgImmunity in npcModel.SpecialWeaponDmgImmunityModelList)
						{
							if (specialWeaponDmgImmunity.Selected)
							{
								stringBuilder.Append(specialWeaponDmgImmunity.ActionDescription).Append(", ");
							}
						}
						if (stringBuilder.Length >= 2)
						{
							stringBuilder.Remove(stringBuilder.Length - 2, 2);
						}
						stringBuilder.Append(Immunity);
					}
				}
			}
			string weaponDamageImmunityString = stringBuilder.ToString().Trim();
			if (weaponDamageImmunityString.EndsWith(";", true, CultureInfo.CurrentCulture))
			{
				weaponDamageImmunityString = weaponDamageImmunityString.Substring(0, weaponDamageImmunityString.Length - 1);
			}
			xmlWriter.WriteString(weaponDamageImmunityString);
			xmlWriter.WriteEndElement();
		}
		public void WriteDamageResistances(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			xmlWriter.WriteStartElement("damageresistances");
			xmlWriter.WriteAttributeString("type", "string");
			if (npcModel.DamageResistanceModelList != null)
			{
				foreach (SelectableActionModel damageResistances in npcModel.DamageResistanceModelList)
				{
					if (damageResistances.Selected)
					{
						stringBuilder.Append(damageResistances.ActionDescription.ToLower()).Append(", ");
					}
				}
			}
			if (stringBuilder.Length >= 2)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			stringBuilder.Append("; ");
			if (npcModel.SpecialWeaponResistanceModelList != null)
			{
				foreach (SelectableActionModel specialWeaponResistance in npcModel.SpecialWeaponResistanceModelList)
				{
					if (specialWeaponResistance.Selected == true && specialWeaponResistance.ActionName != "NoSpecial")
					{

						switch (specialWeaponResistance.ActionName)
						{
							case "Nonmagical":
								Resistance = " from nonmagical attacks";
								break;
							case "NonmagicalSilvered":
								Resistance = " from nonmagical attacks that aren't silvered";
								break;
							case "NonmagicalAdamantine":
								Resistance = " from nonmagical attacks that aren't adamantine";
								break;
							case "NonmagicalColdForgedIron":
								Resistance = " from nonmagical attacks that aren't cold-forged iron";
								break;
						}
						foreach (SelectableActionModel specialWeaponDmgResistance in npcModel.SpecialWeaponDmgResistanceModelList)
						{
							if (specialWeaponDmgResistance.Selected)
							{
								stringBuilder.Append(specialWeaponDmgResistance.ActionDescription).Append(", ");
							}
						}
						if (stringBuilder.Length >= 2)
						{
							stringBuilder.Remove(stringBuilder.Length - 2, 2);
						}
						stringBuilder.Append(Resistance);
					}
				}
			}
			string weaponDamageResistanceString = stringBuilder.ToString().Trim();
			if (weaponDamageResistanceString.StartsWith(";", true, CultureInfo.CurrentCulture))
			{
				weaponDamageResistanceString = weaponDamageResistanceString.Remove(0, 1);
			}
			if (weaponDamageResistanceString.EndsWith(";", true, CultureInfo.CurrentCulture))
			{
				weaponDamageResistanceString = weaponDamageResistanceString.Substring(0, weaponDamageResistanceString.Length - 1);
			}
			xmlWriter.WriteString(weaponDamageResistanceString);
			xmlWriter.WriteEndElement();
		}
		public void WriteDamageVulnerabilities(XmlWriter xmlWriter, NPCModel npcModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			xmlWriter.WriteStartElement("damagevulnerabilities");
			xmlWriter.WriteAttributeString("type", "string");
			if (npcModel.DamageVulnerabilityModelList != null)
			{
				foreach (SelectableActionModel damageVulnerabilities in npcModel.DamageVulnerabilityModelList)
				{
					if (damageVulnerabilities.Selected == true)
					{
						stringBuilder.Append(damageVulnerabilities.ActionDescription.ToLower()).Append(", ");
					}
				}
			}
			if (stringBuilder.Length >= 2)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			string weaponDamageVulnerabilityString = stringBuilder.ToString().Trim();
			xmlWriter.WriteValue(weaponDamageVulnerabilityString);
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
			xmlWriter.WriteAttributeString("type", "string");
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
			xmlWriter.WriteAttributeString("type", "number");
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
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString(lairaction.ActionDescription);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("name");
					xmlWriter.WriteAttributeString("type", "string");
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
			xmlWriter.WriteAttributeString("type", "string");
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
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(legendaryaction.ActionDescription);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("name");
				xmlWriter.WriteAttributeString("type", "string");
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
			xmlWriter.WriteAttributeString("type", "string");
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
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(reaction.ActionDescription);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("name");
				xmlWriter.WriteAttributeString("type", "string");
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
			xmlWriter.WriteAttributeString("type", "string");
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
			xmlWriter.WriteAttributeString("type", "string");
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
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(speedString);
			xmlWriter.WriteEndElement();
		}
		static public void WriteSize(XmlWriter xmlWriter, NPCModel npcModel)
		{
			xmlWriter.WriteStartElement("size");
			xmlWriter.WriteAttributeString("type", "string");
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
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(stringBuilder.ToString());
			xmlWriter.WriteEndElement();
		}
		static public void WriteSkills(XmlWriter xmlWriter, NPCModel npcModel)
		{
			string skillsString = npcModel.SkillAttributesToString();
			xmlWriter.WriteStartElement("skills");
			xmlWriter.WriteAttributeString("type", "string");
			xmlWriter.WriteValue(skillsString);
			xmlWriter.WriteEndElement();
		}
		static public void WriteText(XmlWriter xmlWriter, NPCModel npcModel)
		{
			NPCController npcController = new NPCController();
			xmlWriter.WriteStartElement("text");
			xmlWriter.WriteAttributeString("type", "formattedtext");
			xmlWriter.WriteRaw(npcController.GenerateFantasyGroundsDescriptionXML(npcModel.Description));
			xmlWriter.WriteEndElement();
		}
		static public void WriteToken(XmlWriter xmlWriter, NPCModel npcModel, ModuleModel moduleModel)
		{
			xmlWriter.WriteStartElement("token");
			xmlWriter.WriteAttributeString("type", "token");
			if (npcModel.NPCToken == null || npcModel.NPCToken == " " || !moduleModel.IncludeTokens)
			{
				xmlWriter.WriteString("");
			}
			else
			{
				xmlWriter.WriteValue("tokens\\" + Path.GetFileName(npcModel.NPCToken) + "@" + moduleModel.Name);
			}
			xmlWriter.WriteEndElement();
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
					xmlWriter.WriteAttributeString("type", "string");
					xmlWriter.WriteString(traits.ActionDescription);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("name");
					xmlWriter.WriteAttributeString("type", "string");
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
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(innateCastingDescription);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("name");
				xmlWriter.WriteAttributeString("type", "string");
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
					stringBuilder.Append("\\rCantrips (" + npcModel.CantripSpells.ToLower() + "): " + npcModel.CantripSpellList.ToLower());
				}
				if (npcModel.FirstLevelSpellList != null)
				{
					stringBuilder.Append("\\r1st level (" + npcModel.FirstLevelSpells.ToLower() + "): " + npcModel.FirstLevelSpellList.ToLower());
				}
				if (npcModel.SecondLevelSpellList != null)
				{
					stringBuilder.Append("\\r2nd level (" + npcModel.SecondLevelSpells.ToLower() + "): " + npcModel.SecondLevelSpellList.ToLower());
				}
				if (npcModel.ThirdLevelSpellList != null)
				{
					stringBuilder.Append("\\r3rd level (" + npcModel.ThirdLevelSpells.ToLower() + "): " + npcModel.ThirdLevelSpellList.ToLower());
				}
				if (npcModel.FourthLevelSpellList != null)
				{
					stringBuilder.Append("\\r4th level (" + npcModel.FourthLevelSpells.ToLower() + "): " + npcModel.FourthLevelSpellList.ToLower());
				}
				if (npcModel.FifthLevelSpellList != null)
				{
					stringBuilder.Append("\\r5th level (" + npcModel.FifthLevelSpells.ToLower() + "): " + npcModel.FifthLevelSpellList.ToLower());
				}
				if (npcModel.SixthLevelSpellList != null)
				{
					stringBuilder.Append("\\r6th level (" + npcModel.SixthLevelSpells.ToLower() + "): " + npcModel.SixthLevelSpellList.ToLower());
				}
				if (npcModel.SeventhLevelSpellList != null)
				{
					stringBuilder.Append("\\r7th level (" + npcModel.SeventhLevelSpells.ToLower() + "): " + npcModel.SeventhLevelSpellList.ToLower());
				}
				if (npcModel.EighthLevelSpellList != null)
				{
					stringBuilder.Append("\\r8th level (" + npcModel.EighthLevelSpells.ToLower() + "): " + npcModel.EighthLevelSpellList.ToLower());
				}
				if (npcModel.NinthLevelSpellList != null)
				{
					stringBuilder.Append("\\r9th level (" + npcModel.NinthLevelSpells.ToLower() + "): " + npcModel.NinthLevelSpellList.ToLower());
				}
				string spellcastingDescription = stringBuilder.ToString();
				xmlWriter.WriteStartElement("id-" + actionID.ToString("D4"));
				xmlWriter.WriteStartElement("desc");
				xmlWriter.WriteAttributeString("type", "string");
				xmlWriter.WriteString(spellcastingDescription);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("name");
				xmlWriter.WriteAttributeString("type", "string");
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
			xmlWriter.WriteAttributeString("type", "number");
			xmlWriter.WriteValue(npcModel.XP);
			xmlWriter.WriteEndElement();
		}
	}
}
