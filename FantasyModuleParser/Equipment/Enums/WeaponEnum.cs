using System.ComponentModel;

namespace FantasyModuleParser.Equipment.Enums
{
    public enum WeaponEnum
    {
		[Description("Simple Melee Weapon")]
		SMW = 0,

		[Description("Simple Ranged Weapon")]
		SRW = 1,

		[Description("Martial Melee Weapon")]
		MMW = 2,

		[Description("Martial Ranged Weapon")]
		MRW = 3,
	}
}
