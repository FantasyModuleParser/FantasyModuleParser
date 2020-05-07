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
				sb.Append("+" + ToHit + " to hit, ");
			}
			else
			{
				sb.Append(ToHit + " to hit, ");
			}
			if (WeaponType == WeaponType.MWA || WeaponType == WeaponType.WA || WeaponType == WeaponType.MSA || WeaponType == WeaponType.SA)
			{
				sb.Append("reach " + Reach);
			}
			else
			{
				if (WeaponType == WeaponType.RWA)
				{
					sb.Append("range " + WeaponRangeShort + "/" + WeaponRangeLong);
				}
				else
				{
					sb.Append("range " + WeaponRangeShort);
				}
			}
			if (WeaponType == WeaponType.SA)
			{
				sb.Append(" ft. or range " + WeaponRangeShort + " ft., " + TargetType.GetDescription() + ". Hit: " + PrimaryDamageTotal + " (" + PrimaryDamage.NumOfDice + PrimaryDamage.DieType.GetDescription());
			}
			else
			{
				sb.Append(" ft., " + TargetType.GetDescription() + ". Hit: " + PrimaryDamageTotal + " (" + PrimaryDamage.NumOfDice + PrimaryDamage.DieType.GetDescription());
			}
			if (PrimaryDamage.Bonus != 0)
			{
				if (PrimaryDamage.Bonus > 0)
				{
					sb.Append(" + " + PrimaryDamage.Bonus + ") " + PrimaryDamage.DamageType + " damage");
				}
				else
				{
					sb.Append(PrimaryDamage.Bonus + ") " + PrimaryDamage.DamageType + " damage");
				}
			}
			else
			{
				sb.Append(") " + PrimaryDamage.DamageType + " damage");
			}
			if (AddSecondDamage == true)
			{
				sb.Append(" plus " + SecondaryDamageTotal + " (" + SecondaryDamage.NumOfDice + SecondaryDamage.DieType.GetDescription());
				if (SecondaryDamage.Bonus != 0)
				{
					if (SecondaryDamage.Bonus > 0)
					{
						sb.Append(" + " + SecondaryDamage.Bonus);
					}
					else
					{
						sb.Append(SecondaryDamage.Bonus);
					}
				}
				else
				{
					sb.Append(") " + SecondaryDamage.DamageType + " damage");
				}
			}
			else
			{
				sb.Append(".");
			}
			if (WeaponType == WeaponType.WA)
			{
				sb.Append(" Or Ranged Weapon Attack: ");
				if (ToHit > -1)
				{
					sb.Append("+" + ToHit + " to hit, range " + WeaponRangeShort + "/" + WeaponRangeLong + " ft., " + TargetType.GetDescription() + ". Hit: " + PrimaryDamageTotal + " (" + PrimaryDamage.NumOfDice + PrimaryDamage.DieType.GetDescription());
				}
				else
				{
					sb.Append(ToHit + " to hit, range " + WeaponRangeShort + "/" + WeaponRangeLong + " ft., " + TargetType.GetDescription() + ". Hit: " + PrimaryDamageTotal + " (" + PrimaryDamage.NumOfDice + PrimaryDamage.DieType.GetDescription());
				}

				if (PrimaryDamage.Bonus != 0)
				{
					if (PrimaryDamage.Bonus > 0)
					{
						sb.Append(" + " + PrimaryDamage.Bonus);
					}
					else
					{
						sb.Append(PrimaryDamage.Bonus);
					}
				}
				else
				{
					sb.Append(") " + PrimaryDamage.DamageType + " damage");
				}
				if (AddSecondDamage == true)
				{
					sb.Append(" plus " + SecondaryDamageTotal + " (" + SecondaryDamage.NumOfDice + SecondaryDamage.DieType.GetDescription());
					if (SecondaryDamage.Bonus != 0)
					{
						if (SecondaryDamage.Bonus > 0)
						{
							sb.Append(" + " + SecondaryDamage.Bonus);
						}
						else
						{
							sb.Append(SecondaryDamage.Bonus);
						}
					}
					else
					{
						sb.Append(") " + SecondaryDamage.DamageType + " damage.");
					}

				}
				if (OtherTextCheck == true)
				{
					sb.Append(" " + OtherText);
					ActionDescription = sb.ToString();
					return ActionDescription;
				}
				else
				{
					ActionDescription = sb.ToString();
					return ActionDescription;
				}
			}
			else
			{
				if (OtherTextCheck == true)
				{
					sb.Append(" " + OtherText);
					ActionDescription = sb.ToString();
					return ActionDescription;
				}
				else
				{
					ActionDescription = sb.ToString();
					return ActionDescription;
				}
			}
		}
	}
}
