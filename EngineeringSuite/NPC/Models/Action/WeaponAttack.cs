using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			PrimaryDamage.DieType = DieType.d6;
			PrimaryDamage.Bonus = 0;
			Reach = 5;
			WeaponRangeShort = 30;
			WeaponRangeLong = 60;
		}

		public string GenerateWeaponAttackDescription()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(WeaponType + ": ");
			if (ToHit > -1)
			{
				sb.Append("+");
			}
			sb.Append(ToHit + " to hit, reach " + Reach + " ft., " + TargetType + ". Hit: ");
			int PrimaryDamageTotal = (int)PrimaryDamage.DieType * PrimaryDamage.NumOfDice / 2 + PrimaryDamage.Bonus;
			sb.Append(PrimaryDamageTotal + " (" + PrimaryDamage.NumOfDice + PrimaryDamage.DieType + " + " + PrimaryDamage.Bonus + ") " + PrimaryDamage.DamageType + " damage.");

			//TODO:  This is a double take, but saving the result to ActionDescription & returning the value
			this.ActionDescription = sb.ToString();
			return this.ActionDescription;
		}
	}
}
