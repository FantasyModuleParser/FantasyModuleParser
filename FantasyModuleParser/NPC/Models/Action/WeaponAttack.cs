using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FantasyModuleParser.Extensions;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Action.Enums;

namespace FantasyModuleParser.NPC.Models.Action
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
		public bool AddVersatileDamage { get; set; }
		public bool OtherTextCheck { get; set; }

		public int ToHit { get; set; }
		public int Reach { get; set; }
		public int WeaponRangeShort { get; set; }
		public int WeaponRangeLong { get; set; }
		public TargetType TargetType { get; set; }

		public string OtherText { get; set; }

		public DamageProperty PrimaryDamage { get; set; }
		public DamageProperty SecondaryDamage { get; set; }
		public DamageProperty VersatileDamage { get; set; }

		public WeaponAttack()
		{
			PrimaryDamage = new DamageProperty();
			SecondaryDamage = new DamageProperty();
			VersatileDamage = new DamageProperty();
			WeaponType = WeaponType.MWA;
			PrimaryDamage.NumOfDice = 1;
			PrimaryDamage.DieType = DieType.D6;
			PrimaryDamage.Bonus = 0;
			SecondaryDamage.NumOfDice = 1;
			SecondaryDamage.DieType = DieType.D6;
			SecondaryDamage.Bonus = 0;
			VersatileDamage.NumOfDice = 1;
			VersatileDamage.DieType = DieType.D8;
			VersatileDamage.Bonus = 0;
			Reach = 5;
			WeaponRangeShort = 30;
			WeaponRangeLong = 60;
		}

		public string GenerateWeaponAttackDescription()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int PrimaryDamageTotal = PrimaryDamage.NumOfDice * ((int)PrimaryDamage.DieType + 1) / 2 + PrimaryDamage.Bonus;
			int SecondaryDamageTotal = 0;
			int VersatileDamageTotal = 0;
			if (VersatileDamage != null)
				VersatileDamageTotal = VersatileDamage.NumOfDice * ((int)VersatileDamage.DieType + 1) / 2 + VersatileDamage.Bonus;
			if (SecondaryDamage != null)
				SecondaryDamageTotal = SecondaryDamage.NumOfDice * ((int)SecondaryDamage.DieType + 1) / 2 + SecondaryDamage.Bonus;

			if (WeaponType == WeaponType.WA)
			{
				stringBuilder.Append("Melee Weapon Attack: ");
			}
			else
			{
				stringBuilder.Append(WeaponType.GetDescription() + ": ");
			}

			ToHitStringBuilder(stringBuilder);
			stringBuilder.Append(ToHit + " to hit, ");

			if (WeaponType == WeaponType.MWA || WeaponType == WeaponType.WA || WeaponType == WeaponType.MSA || WeaponType == WeaponType.SA)
			{
				stringBuilder.Append("reach " + Reach);
			}
			else
			{
				stringBuilder.Append("range " + WeaponRangeShort);

				if (WeaponType == WeaponType.RWA)
				{
					stringBuilder.Append("/" + WeaponRangeLong);
				}
			}

			if (WeaponType == WeaponType.SA && PrimaryDamage.NumOfDice == 0)
			{
				{
					stringBuilder.Append(" ft. or range " + WeaponRangeShort);
				}
				stringBuilder.Append(" ft., " + TargetType.GetDescription() + ". Hit: " + PrimaryDamageTotal);
			}
			else if (WeaponType == WeaponType.SA && PrimaryDamage.NumOfDice > 0)
			{
				stringBuilder.Append(" ft. or range " + WeaponRangeShort);
			}

			stringBuilder.Append(" ft., " + TargetType.GetDescription() + ". Hit: ");
			if (PrimaryDamage.NumOfDice > 0)
				stringBuilder.Append(PrimaryDamageTotal + " (" + PrimaryDamage.NumOfDice + PrimaryDamage.DieType.GetDescription());
			
			AddPrimaryDamageToStringBuilder(stringBuilder);

			if (WeaponType == WeaponType.MWA || WeaponType == WeaponType.WA)
            {
				if (AddSecondDamage && AddVersatileDamage)
				{
					AddSecondaryDamageToStringBuilder(stringBuilder, SecondaryDamageTotal);
					AddVersatileDamageToStringBuilder(stringBuilder, VersatileDamageTotal);
				}
				else if (AddVersatileDamage && !AddSecondDamage)
				{
					AddVersatileDamageToStringBuilder(stringBuilder, VersatileDamageTotal);
				}
				else if (AddSecondDamage && !AddVersatileDamage)
				{
					AddSecondaryDamageToStringBuilder(stringBuilder, SecondaryDamageTotal);
				}
			}
			else
            {
				if (AddSecondDamage)
					AddSecondaryDamageToStringBuilder(stringBuilder, SecondaryDamageTotal);
			}
			
			stringBuilder.Append(".");

			if (WeaponType == WeaponType.WA)
			{
				stringBuilder.Append(" Or Ranged Weapon Attack: ");

				ToHitStringBuilder(stringBuilder);
			}
			if (WeaponType == WeaponType.WA && PrimaryDamage.NumOfDice == 0)
			{
				stringBuilder.Append(ToHit + " to hit, range " + WeaponRangeShort + "/" + WeaponRangeLong + " ft., " + TargetType.GetDescription() + ". Hit: " + PrimaryDamageTotal);
				
				AddPrimaryDamageToStringBuilder(stringBuilder);

				if (AddSecondDamage)
					AddSecondaryDamageToStringBuilder(stringBuilder, SecondaryDamageTotal);

				stringBuilder.Append(".");
			}
			if (WeaponType == WeaponType.WA && PrimaryDamage.NumOfDice > 0)
			{
				stringBuilder.Append(ToHit + " to hit, range " + WeaponRangeShort + "/" + WeaponRangeLong + " ft., " + TargetType.GetDescription() + ". Hit: " + PrimaryDamageTotal + " (" + PrimaryDamage.NumOfDice + PrimaryDamage.DieType.GetDescription());

				AddPrimaryDamageToStringBuilder(stringBuilder);

				if (AddSecondDamage)
				{
					stringBuilder.Append(" ");
					AddSecondaryDamageToStringBuilder(stringBuilder, SecondaryDamageTotal);
				}
				stringBuilder.Append(".");

			}

			if (OtherTextCheck) stringBuilder.Append(" " + OtherText);
			ActionDescription = stringBuilder.ToString().TrimEnd();
			return ActionDescription;
		}

		private void AddPrimaryDamageToStringBuilder(StringBuilder stringBuilder)
		{
			if (PrimaryDamage.Bonus > 0 && PrimaryDamage.NumOfDice > 0)
				stringBuilder.Append(" + " + PrimaryDamage.Bonus);
			else if (PrimaryDamage.Bonus < 0 || PrimaryDamage.NumOfDice == 0)
				stringBuilder.Append(" " + PrimaryDamage.Bonus);

			stringBuilder.Append(") ");
			stringBuilder.Append(PrimaryDamage.DamageType.GetDescription().ToLower());
			if (IsSilver)
				stringBuilder.Append(", silver");
			if (IsAdamantine)
				stringBuilder.Append(", adamantine");
			if (IsColdForgedIron)
				stringBuilder.Append(", cold-forged iron");
			if (IsMagic)
				stringBuilder.Append(", magic");
			stringBuilder.Append(" damage");
		}

		private void ToHitStringBuilder(StringBuilder stringBuilder)
		{
			if (ToHit > -1)
				stringBuilder.Append("+");
		}

		private void AddSecondaryDamageToStringBuilder(StringBuilder stringBuilder, int SecondaryDamageTotal)
		{
			if (SecondaryDamage.NumOfDice == 0)
			{
				stringBuilder.Append(" plus " + SecondaryDamageTotal + " ");
			}
			else 
			{
				stringBuilder.Append(" plus " + SecondaryDamageTotal + " (" + SecondaryDamage.NumOfDice + SecondaryDamage.DieType.GetDescription());

				if (SecondaryDamage.Bonus > 0)
					stringBuilder.Append(" + ").Append(SecondaryDamage.Bonus);
				else if (SecondaryDamage.Bonus < 0)
					stringBuilder.Append(" ").Append(SecondaryDamage.Bonus);

				stringBuilder.Append(") ");
			}
			stringBuilder.Append(SecondaryDamage.DamageType.GetDescription().ToLower() + " damage");
		}
		private void AddVersatileDamageToStringBuilder(StringBuilder stringBuilder, int VersatileDamageTotal)
		{
			if (VersatileDamage.NumOfDice == 0)
            {
				MessageBox.Show("Number of dice must be at least 1");
			}
			else
			{
				stringBuilder.Append(" or " + VersatileDamageTotal + " (" + VersatileDamage.NumOfDice + VersatileDamage.DieType.GetDescription());

				if (VersatileDamage.Bonus > 0)
					stringBuilder.Append(" + " + VersatileDamage.Bonus);
				else if (VersatileDamage.Bonus < 0)
					stringBuilder.Append(VersatileDamage.Bonus);

				stringBuilder.Append(") ");
			}
			stringBuilder.Append(VersatileDamage.DamageType.GetDescription().ToLower() + " damage if used with two hands");
		}
	}
}
