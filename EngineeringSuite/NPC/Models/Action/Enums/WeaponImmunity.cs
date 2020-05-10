using System.ComponentModel;

namespace EngineeringSuite.NPC.Models.Action.Enums
{
	public enum WeaponImmunity
	{
		[Description("No Special Weapon Immunity")]
		NoSpecial = 0,
		[Description("Immune to nonmagical Weapons")]
		Nonmagical = 1,
		[Description("Immune to nonmagical weapons that aren't silvered")]
		NonmagicalSilvered = 2,
		[Description("Immune to nonmagical weapons that aren't adamantine")]
		NonmagicalAdamantine = 3,
		[Description("Immune to nonmagical weapons that aren't cold-forged iron")]
		NonmagicalColdForgedIron = 4,
	}
}
