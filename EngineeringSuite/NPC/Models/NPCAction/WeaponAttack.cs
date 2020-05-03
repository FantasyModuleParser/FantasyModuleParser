﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringSuite.NPC.Models.NPCAction.Enums;

namespace EngineeringSuite.NPC.Models.NPCAction
{

	public class WeaponAttack : ActionModelBase
	{
		public string WeaponName { get; set; }
		public WeaponType WeaponType { get; set; }

		public bool IsMagic { get; set; }
		public bool IsSilver { get; set; }
		public bool IsAdamantine { get; set; }
		public bool IsColdForgedIron { get; set; }
		public bool IsVersatile { get; set; }

		public int ToHit { get; set; }
		public int Reach { get; set; }
		public int WeaponRangeShort { get; set; }
		public int WeaponRangeLong { get; set; }
		public TargetType TargetType { get; set; }

		public string OtherText { get; set; }

		public List<DamageProperty> DamageProperties { get; set; }
		public DamageProperty PrimaryDamage;
		public DamageProperty SecondaryDamage;

		public WeaponAttack()
		{
			PrimaryDamage = new DamageProperty();
			SecondaryDamage = new DamageProperty();
			WeaponType = WeaponType.MWA; // Default to the first entry
			PrimaryDamage.NumOfDice = 1;
			PrimaryDamage.DieType = DieType.D6;
			PrimaryDamage.Bonus = 0;
			Reach = 5;
			WeaponRangeShort = 30;
			WeaponRangeLong = 60;
		}

		private string GenerateWeaponAttackDescription()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(WeaponType + ": ");
			if (ToHit > -1)
			{
				sb.Append("+");
			}
			sb.Append(ToHit + " to hit, reach " + Reach + " ft., " + TargetType + ". Hit: ");
			int PrimaryDamageTotal = (int)PrimaryDamage.DieType * PrimaryDamage.NumOfDice / 2 + PrimaryDamage.Bonus;
			sb.Append(PrimaryDamageTotal + " (" + PrimaryDamage.NumOfDice + "d" + PrimaryDamage.DieType + " + " + PrimaryDamage.Bonus + ") " + PrimaryDamage.DamageType + " damage.");

			return sb.ToString();
		}
	}
}
