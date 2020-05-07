using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringSuite.Extensions;
using EngineeringSuite.NPC.Models.Action;
using EngineeringSuite.NPC.Models.Action.Enums;

namespace EngineeringSuite.NPC.Models.Action
{

	public class WeaponAttack : ActionModelBase
	{
		public WeaponType WeaponType { get; set; }

		public bool IsMagic { get; set; }
		public bool IsSilver { get; set; }
		public bool IsAdamantine { get; set; }
		public bool IsColdForgedIron { get; set; }
		public bool IsVersatile { get; set; }    
		public bool AddSecondDamage { get; set; }
		public bool OtherTextCheck { get; set; }

		public int ToHit { get; set; }
		public int Reach { get; set; }
		public int WeaponRangeShort { get; set; }
		public int WeaponRangeLong { get; set; }
		public TargetType TargetType { get; set; }

		public string OtherText { get; set; }

		public DamageProperty PrimaryDamage { get; set; }
		public DamageProperty SecondaryDamage { get; set; }

		public WeaponAttack()
		{
			PrimaryDamage = new DamageProperty();
			SecondaryDamage = new DamageProperty();
			WeaponType = WeaponType.MWA;
			PrimaryDamage.NumOfDice = 1;
			PrimaryDamage.DieType = DieType.D6;
			PrimaryDamage.Bonus = 0;
			Reach = 5;
			WeaponRangeShort = 30;
			WeaponRangeLong = 60;
		}

		public string GenerateWeaponAttackDescription()
		{
			StringBuilder sb = new StringBuilder();
			int PrimaryDamageTotal = (int)PrimaryDamage.DieType * PrimaryDamage.NumOfDice / 2 + PrimaryDamage.Bonus;
			int SecondaryDamageTotal = (int)SecondaryDamage.DieType * SecondaryDamage.NumOfDice / 2 + SecondaryDamage.Bonus;
			if (WeaponType == WeaponType.WA)
			{
				sb.Append("Melee Weapon Attack: ");
			}
			else
			{
				sb.Append(WeaponType.GetDescription() + ": ");
			}
			if (ToHit > -1)
			{
				sb.Append("+");
			}
			sb.Append(ToHit + " to hit, ");
			if (WeaponType != WeaponType.SA || WeaponType != WeaponType.WA)
			{
				if (WeaponType == WeaponType.MWA || WeaponType == WeaponType.MSA)
				{
					sb.Append("reach " + Reach);
				}
				if (WeaponType == WeaponType.RWA)
				{
					sb.Append("range " + WeaponRangeShort + "/" + WeaponRangeLong);
				}
				sb.Append(" ft., " + TargetType + ". Hit: ");
				sb.Append(PrimaryDamageTotal + " (" + PrimaryDamage.NumOfDice + PrimaryDamage.DieType.GetDescription());
				if (PrimaryDamage.Bonus != 0)
				{
					sb.Append(" + " + PrimaryDamage.Bonus);
				}
				sb.Append(") " + PrimaryDamage.DamageType + " damage");
				if (AddSecondDamage == true)
				{
					sb.Append(" plus " + SecondaryDamageTotal + " (" + SecondaryDamage.NumOfDice + SecondaryDamage.DieType.GetDescription());
					if (SecondaryDamage.Bonus != 0)
					{
						sb.Append(" + " + SecondaryDamage.Bonus);
					}
					sb.Append(") " + SecondaryDamage.DamageType + " damage");
				}
				if (OtherTextCheck == true)
				{
					sb.Append(" " + OtherText);
				}
				sb.Append(".");
				//TODO:  This is a double take, but saving the result to ActionDescription & returning the value
				ActionDescription = sb.ToString();
				return ActionDescription;
			}
			else if (WeaponType == WeaponType.SA)
			{
				sb.Append("reach " + Reach + " ft. or range " + WeaponRangeShort + " ft., " + TargetType + ". Hit: ");
				sb.Append(PrimaryDamageTotal + " (" + PrimaryDamage.NumOfDice + PrimaryDamage.DieType.GetDescription());
				if (PrimaryDamage.Bonus != 0)
				{
					sb.Append(" + " + PrimaryDamage.Bonus);
				}
				sb.Append(") " + PrimaryDamage.DamageType + " damage");
				if (AddSecondDamage == true)
				{
					sb.Append(" plus " + SecondaryDamageTotal + " (" + SecondaryDamage.NumOfDice + SecondaryDamage.DieType.GetDescription());
					if (SecondaryDamage.Bonus != 0)
					{
						sb.Append(" + " + SecondaryDamage.Bonus);
					}
					sb.Append(") " + SecondaryDamage.DamageType + " damage");
				}
				if (OtherTextCheck == true)
				{
					sb.Append(" " + OtherText);
				}
				sb.Append(".");
				//TODO:  This is a double take, but saving the result to ActionDescription & returning the value
				ActionDescription = sb.ToString();
				return ActionDescription;
			}
			else
			{
				sb.Append("reach " + Reach + " ft., " + TargetType + ". Hit: ");
				sb.Append(PrimaryDamageTotal + " (" + PrimaryDamage.NumOfDice + PrimaryDamage.DieType.GetDescription());
				if (PrimaryDamage.Bonus != 0)
				{
					sb.Append(" + " + PrimaryDamage.Bonus);
				}
				sb.Append(") " + PrimaryDamage.DamageType + " damage");
				if (AddSecondDamage == true)
				{
					sb.Append(" plus " + SecondaryDamageTotal + " (" + SecondaryDamage.NumOfDice + SecondaryDamage.DieType.GetDescription());
					if (SecondaryDamage.Bonus != 0)
					{
						sb.Append(" + " + SecondaryDamage.Bonus);
					}
					sb.Append(") " + SecondaryDamage.DamageType + " damage");
				}
				if (OtherTextCheck == true)
				{
					sb.Append(" " + OtherText);
				}
				sb.Append(".");
				//TODO:  This is a double take, but saving the result to ActionDescription & returning the value
				ActionDescription = sb.ToString();
				return ActionDescription;
			}
		}
	}
}
