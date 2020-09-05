using FantasyModuleParser.Importer.Utils;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Action.Enums;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FantasyModuleParser.Importer.NPC
{
    public abstract class ImportNPCBase : IImportNPC
    {
        private ImportCommonUtils importCommonUtils = new ImportCommonUtils();
        public abstract NPCModel ImportTextToNPCModel(string importTextContent);

        /// <summary>
        /// 'Multiattack. .This creature makes 3 attacks.'
        /// </summary>
        public void ParseStandardAction(NPCModel npcModel, string standardAction)
        {
            // Don't deal w/ an empty string.
            if (standardAction.Length == 0 || standardAction.Trim().Length == 0)
                return;


            if (standardAction.StartsWith(Multiattack.LocalActionName))
            {
                ParseMultiattackAction(npcModel, standardAction);
                return;
            }

            // For any standard action, it will contain one of the following from the WeaponType enum description
            if (standardAction.Contains(GetDescription(typeof(WeaponType), WeaponType.MSA)) ||
                standardAction.Contains(GetDescription(typeof(WeaponType), WeaponType.MWA)) ||
                standardAction.Contains(GetDescription(typeof(WeaponType), WeaponType.RSA)) ||
                standardAction.Contains(GetDescription(typeof(WeaponType), WeaponType.RWA)) ||
                standardAction.Contains(GetDescription(typeof(WeaponType), WeaponType.SA)) ||
                standardAction.Contains(GetDescription(typeof(WeaponType), WeaponType.WA)))
            {
                ParseWeaponAttackAction(npcModel, standardAction);
                return;
            }

            // Special Case;  If the string has more than 5 words, and none of those 5 words has a period character,
            //  then append the line to the previously created Action's Description as a new line
            string[] standardActionArray = standardAction.Split(' ');
            for (int idx = 0; idx < 5; idx++)
            {
                if (standardActionArray[idx].Contains("."))
                {
                    // if not Multiattack or standard action, then it's an OtherAction
                    ParseOtherAction(npcModel, standardAction);
                    return;
                }
            }

            ActionModelBase actionModelBase = npcModel.NPCActions.Last();
            actionModelBase.ActionDescription = actionModelBase.ActionDescription + "\n\n" + standardAction;
        }

        private static string[] ParseOtherAction(NPCModel npcModel, string standardAction)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string[] standardActionArray;
            OtherAction otherActionModel = new OtherAction();
            standardActionArray = standardAction.Split('.');
            for (int idx = 1; idx < standardActionArray.Length; idx++)
            {
                stringBuilder.Append(standardActionArray[idx].Trim()).Append(". ");
            }
            otherActionModel.ActionName = standardActionArray[0];
            otherActionModel.ActionDescription = stringBuilder.Remove(stringBuilder.Length - 2, 2).ToString().Trim();
            npcModel.NPCActions.Add(otherActionModel);
            return standardActionArray;
        }

        private void ParseWeaponAttackAction(NPCModel npcModel, string standardAction)
        {
            WeaponAttack weaponAttackModel = new WeaponAttack();

            weaponAttackModel.WeaponType = importCommonUtils.GetWeaponTypeFromString(standardAction);
            weaponAttackModel.ActionName = standardAction.Split('.')[0];

            int firstColonIndex = standardAction.IndexOf(':');
            string weaponDescription = standardAction.Substring(firstColonIndex + 2);

            string[] weaponDescriptionDataSplit = weaponDescription.Split(',');

            foreach (string weaponDescriptionData in weaponDescriptionDataSplit)
            {
                if (weaponDescriptionData.Contains("to hit"))
                {
                    weaponAttackModel.ToHit = parseAttributeStringToInt(weaponDescriptionData.Split(' ')[0]);
                }
                if (weaponDescriptionData.Contains("reach"))
                {
                    weaponAttackModel.Reach = parseAttributeStringToInt(weaponDescriptionData.Split(' ')[2]);
                }
                if (weaponDescriptionData.Contains("range"))
                {
                    int rangeIndex = weaponDescriptionData.IndexOf("range ", StringComparison.Ordinal);
                    string rangeStringValue = weaponDescriptionData.Substring(rangeIndex + 6).Split(' ')[0];

                    if (rangeStringValue.Contains("/"))
                    {
                        weaponAttackModel.WeaponRangeShort = int.Parse(rangeStringValue.Split('/')[0], CultureInfo.CurrentCulture);
                        weaponAttackModel.WeaponRangeLong = int.Parse(rangeStringValue.Split('/')[1], CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        weaponAttackModel.WeaponRangeShort = int.Parse(rangeStringValue, CultureInfo.CurrentCulture);

                    }
                }
                if (weaponDescriptionData.Contains("one target"))
                {
                    weaponAttackModel.TargetType = TargetType.target;
                }
                if (weaponDescriptionData.Contains("one creature"))
                {
                    weaponAttackModel.TargetType = TargetType.creature;
                }
            }

            ParseWeaponAttackDamageText(weaponAttackModel, weaponDescription);

            // Update the WA description
            ActionController actionController = new ActionController();
            actionController.GenerateWeaponDescription(weaponAttackModel);

            npcModel.NPCActions.Add(weaponAttackModel);
        }

        private void ParseWeaponAttackDamageText(WeaponAttack weaponAttackModel, string weaponDescription)
        {
            Regex PrimarySecondaryDamageRegex = new Regex(@".*damage.*plus.*damage\.");
            Regex PrimaryOnlyDamageRegex = new Regex(@".*?damage");
            Regex PrimaryWithVersatileRegex = new Regex(@".*?if used with two hands.*");
            string damagePropertyData = weaponDescription.Substring(weaponDescription.IndexOf("Hit: ", StringComparison.Ordinal) + 4);
            string flavorText = "";
            // This is the versatile weapon check
            //if (PrimaryWithVersatileRegex.IsMatch(damagePropertyData))
            //{
            //    string[] damagePropertyDataSplit = damagePropertyData.Split(new string[] { " plus " }, StringSplitOptions.None);
            //    weaponAttackModel.PrimaryDamage = importCommonUtils.ParseDamageProperty(damagePropertyDataSplit[0]);
            //    weaponAttackModel.SecondaryDamage = importCommonUtils.ParseDamageProperty(damagePropertyDataSplit[1]);
            //    weaponAttackModel.IsVersatile = true;

            //    // Parse out any flavor text
            //    ParseWeaponAttackFlavorText(weaponAttackModel, damagePropertyData, PrimaryWithVersatileRegex);
            //}
            //// Check for a secondary damage type
            //else 
            if (PrimarySecondaryDamageRegex.IsMatch(damagePropertyData))
            {
                string[] damagePropertyDataSplit = damagePropertyData.Split(new string[] { " plus " }, StringSplitOptions.None);
                weaponAttackModel.PrimaryDamage = importCommonUtils.ParseDamageProperty(damagePropertyDataSplit[0]);
                weaponAttackModel.SecondaryDamage = importCommonUtils.ParseDamageProperty(damagePropertyDataSplit[1]);


                // Parse out any flavor text
                ParseWeaponAttackFlavorText(weaponAttackModel, damagePropertyData, PrimarySecondaryDamageRegex);
            }
            else
            {
                weaponAttackModel.PrimaryDamage = importCommonUtils.ParseDamageProperty(damagePropertyData);
                weaponAttackModel.SecondaryDamage = null;
                //weaponAttackModel.IsVersatile = false;
                // Parse out any flavor text
                ParseWeaponAttackFlavorText(weaponAttackModel, damagePropertyData, PrimaryOnlyDamageRegex);
            }

            weaponAttackModel.IsMagic = damagePropertyData.Contains("magic");
            weaponAttackModel.IsSilver = damagePropertyData.Contains("silver");
            weaponAttackModel.IsAdamantine = damagePropertyData.Contains("adamantine");
            weaponAttackModel.IsColdForgedIron = damagePropertyData.Contains("cold-forged iron");
            weaponAttackModel.IsVersatile = PrimaryWithVersatileRegex.IsMatch(damagePropertyData);
        }

        private void ParseWeaponAttackFlavorText(WeaponAttack weaponAttackModel, string damagePropertyData, Regex regex)
        {
            // Check for any flavor text
            int regexMatchLength = regex.Match(damagePropertyData).Value.Length;
            Regex PrimaryWithVersatileRegex = new Regex(@".*?if used with two hands.*");
            if (PrimaryWithVersatileRegex.IsMatch(damagePropertyData))
            {
                regexMatchLength = PrimaryWithVersatileRegex.Match(damagePropertyData).Value.Length;
            }
            if (damagePropertyData.Length != regexMatchLength)
            {
                // in the case that the last character is a period, just ignore flavor text
                if (damagePropertyData.Substring(regexMatchLength).Equals(".", StringComparison.Ordinal))
                {
                    weaponAttackModel.OtherTextCheck = false;
                    weaponAttackModel.OtherText = "";
                }
                else
                {
                    weaponAttackModel.OtherTextCheck = true;
                    weaponAttackModel.OtherText = damagePropertyData.Substring(regexMatchLength);
                    // Removes a use case where the above line returns ". <DESCRIPTION HERE>"
                    if (weaponAttackModel.OtherText[0] == '.')
                        weaponAttackModel.OtherText = weaponAttackModel.OtherText.Substring(2);
                }
            }
        }

        private static string[] ParseMultiattackAction(NPCModel npcModel, string standardAction)
        {
            const string delimiter = ".";
            StringBuilder stringBuilder = new StringBuilder();
            string[] standardActionArray;
            Multiattack multiattackModel = new Multiattack();
            standardActionArray = standardAction.Split('.');
            for (int idx = 1; idx < standardActionArray.Length; idx++)
            {
                stringBuilder.Append(standardActionArray[idx].Trim()).Append(delimiter);
            }
            multiattackModel.ActionDescription = stringBuilder.Remove(stringBuilder.Length - delimiter.Length, delimiter.Length).ToString();
            npcModel.NPCActions.Add(multiattackModel);
            return standardActionArray;
        }

        private int parseAttributeStringToInt(string savingThrowValue)
        {
            if (savingThrowValue.Length == 0 || savingThrowValue.Trim().Length == 0)
                return 0;
            savingThrowValue = savingThrowValue.Replace('+', ' ');
            savingThrowValue = savingThrowValue.Replace(',', ' ');
            string savingThrowValueSubstring = savingThrowValue.Trim();
            return int.Parse(savingThrowValueSubstring, CultureInfo.CurrentCulture);
        }

        private string GetDescription(Type EnumType, object enumValue)
        {
            var descriptionAttribute = EnumType
                .GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;


            return descriptionAttribute != null
                ? descriptionAttribute.Description
                : enumValue.ToString();
        }
    }
}
