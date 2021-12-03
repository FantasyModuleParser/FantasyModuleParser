using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Action.Enums;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FantasyModuleParser.Importer.Utils
{
    public class ImportCommonUtils
    {

        public DamageProperty ParseDamageProperty(string weaponAttackData)
        {
            DamageProperty damageProperty = new DamageProperty();
            Regex DamageRegex = new Regex(@".*\dd\d.* damage");

            // Massage data for usage going forward
            string damagePropertyData = weaponAttackData.Substring(weaponAttackData.IndexOf('(') + 1).Trim();

            // Remove the term 'damage'
            if (damagePropertyData.Contains("damage"))
			{
                if(DamageRegex.IsMatch(damagePropertyData))
                {
                    damagePropertyData = damagePropertyData.Substring(0, damagePropertyData.IndexOf("damage", StringComparison.Ordinal)).Trim();
                }
				else 
                {
                    StringBuilder buildDamage = new StringBuilder();
                    string[] splitDamage = damagePropertyData.Split(' ');
                    buildDamage.Append(splitDamage[0]).Append("d1) ").Append(splitDamage[1]);
                    damagePropertyData = buildDamage.ToString();
                }
            }
                

            // 2d8 + 2) lightning

            // Get Num of dice
            damageProperty.NumOfDice = getNumOfDice(damagePropertyData);

            // Get DieType
            damageProperty.DieType = getDieType(damagePropertyData);

            // Get Bonus
            damageProperty.Bonus = getBonus(damagePropertyData);

            // Get DamageType enum
            damageProperty.DamageType = getDamageType(damagePropertyData);


            return damageProperty;
        }

        private int getNumOfDice(string damagePropertyData)
        {
            try
            {
                string numOfDiceData = damagePropertyData.Substring(0, damagePropertyData.IndexOf('d'));
                return int.Parse(numOfDiceData, CultureInfo.CurrentCulture);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Failed to parse the following snippet in getNumOfDice() :: " + damagePropertyData, e);
            }
            
        }

        private DieType getDieType(string damagePropertyData)
        {
            string dieTypeData = "";

            int dieTypeStartIndex = damagePropertyData.IndexOf('d');
            int dieTypeEndIndex = damagePropertyData.IndexOf('+');

            // If no '+' exists, then either it's a minus symbol, or no symbol exists
            //if(dieTypeEndIndex == -1)
            //    dieTypeEndIndex = damagePropertyData.IndexOf('-');

            //if(dieTypeEndIndex == -1)
            //{
            //    dieTypeData = damagePropertyData.Substring(dieTypeStartIndex, 2);
            //    dieTypeData.Replace(')', ' ').Trim();
            //}
            //else
            //{
            //    dieTypeData = damagePropertyData.Substring(dieTypeStartIndex, 2);
            //}

            dieTypeData = damagePropertyData.Substring(dieTypeStartIndex, 3);
            dieTypeData = dieTypeData.Replace(')', ' ').Replace('+', ' ').Replace('-', ' ').Trim();


            //string dieTypeData = damagePropertyData.Trim().Split(' ')[0].Substring(damagePropertyData.IndexOf('d')).ToLower(CultureInfo.CurrentCulture);
            //dieTypeData = dieTypeData.Replace(')', ' ').Trim();
            foreach (DieType dieTypeEnum in Enum.GetValues(typeof(DieType)))
            {
                if (dieTypeData.Equals(GetDescription(typeof(DieType), dieTypeEnum), StringComparison.Ordinal))
                {
                    return dieTypeEnum;
                }
            }
            return DieType.D4;
        }

        private int getBonus(string damagePropertyData)
        {
            if ((damagePropertyData.Contains("+") || damagePropertyData.Contains("-")) && !damagePropertyData.Contains("cold-forged"))
            {
                string bonusData1 = "";
                if(damagePropertyData.Contains("+"))
                    bonusData1 = damagePropertyData.Substring(damagePropertyData.IndexOf('+'));
                else
                    bonusData1 = damagePropertyData.Substring(damagePropertyData.IndexOf('-'));
                //string bonusData1 = damagePropertyData.Substring(damagePropertyData.IndexOf(' '));
                string bonusData2 = bonusData1.Substring(0, bonusData1.IndexOf(") ", StringComparison.Ordinal)).Trim();

                int modifier = 1;
                if (damagePropertyData.Contains("-"))
                    modifier = -1;

                return modifier * int.Parse(bonusData2.Substring(1), CultureInfo.CurrentCulture);
            }
            else
            {
                return 0;
            }
        }

        private DamageType getDamageType(string damagePropertyData)
        {
            string damageTypeData = damagePropertyData.Substring(damagePropertyData.IndexOf(')') + 1).Trim();
            if (damageTypeData.Contains(","))
                damageTypeData = damageTypeData.Split(',')[0];

            foreach (DamageType enumType in Enum.GetValues(typeof(DamageType)))
            {
                if (damageTypeData.Equals(GetDescription(typeof(DamageType), enumType).ToLower(CultureInfo.CurrentCulture), StringComparison.Ordinal))
                {
                    return enumType;
                }
            }

            return DamageType.Acid;
        }

        public string GetDescription(Type EnumType, object enumValue)
        {
            var descriptionAttribute = EnumType
                .GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;


            return descriptionAttribute != null
                ? descriptionAttribute.Description
                : enumValue.ToString();
        }

        public WeaponType GetWeaponTypeFromString(string standardActionData)
        {
            string standardActionDataLower = standardActionData.ToLower();
            if (standardActionDataLower.Contains(GetDescription(typeof(WeaponType), WeaponType.SA).ToLower(CultureInfo.CurrentCulture)))
                return WeaponType.SA;
            if (standardActionDataLower.Contains(GetDescription(typeof(WeaponType), WeaponType.WA).ToLower(CultureInfo.CurrentCulture)))
                return WeaponType.WA;
            if (standardActionDataLower.Contains(GetDescription(typeof(WeaponType), WeaponType.MSA).ToLower(CultureInfo.CurrentCulture)))
                return WeaponType.MSA;
            if (standardActionDataLower.Contains(GetDescription(typeof(WeaponType), WeaponType.MWA).ToLower(CultureInfo.CurrentCulture)))
                return WeaponType.MWA;
            if (standardActionDataLower.Contains(GetDescription(typeof(WeaponType), WeaponType.RSA).ToLower(CultureInfo.CurrentCulture)))
                return WeaponType.RSA;
            if (standardActionDataLower.Contains(GetDescription(typeof(WeaponType), WeaponType.RWA).ToLower(CultureInfo.CurrentCulture)))
                return WeaponType.RWA;
            
            Console.WriteLine("Standard Action failed to parse any weapon type;  Default to MWA");
            return WeaponType.MWA;
        }
    }
}
