using System.ComponentModel;

namespace EngineeringSuite.NPC.Models.NPCAction.Enums
{
	public enum WeaponType
	{
		[Description("Melee Weapon Attack")]
		MWA = 0,

		[Description("Ranged Weapon Attack")]
		RWA = 1,

		[Description("Melee or Ranged Weapon Attack")]
		WA = 2,

		[Description("Melee Spell Attack")]
		MSA = 3,

		[Description("Ranged Spell Attack")]
		RSA = 4,

		[Description("Melee or Ranged Spell Attack")]
		SA = 5,
	}
}