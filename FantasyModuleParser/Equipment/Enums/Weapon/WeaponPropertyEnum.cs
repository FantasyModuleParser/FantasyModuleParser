using System.ComponentModel;

namespace FantasyModuleParser.Equipment.Enums.Weapon
{
    public enum WeaponPropertyEnum
    {
		[Description("Ammunition")]
		Ammunition = 0,

		[Description("Finesse")]
		Finesse = 1,

		[Description("Heavy")]
		Heavy = 2,

		[Description("Light")]
		Light = 3,

		[Description("Loading")]
		Loading = 4,

		[Description("Reach")]
		Reach = 5,

		[Description("Special")]
		Special = 6,

		[Description("Thrown")]
		Thrown = 7,

		[Description("Two-Handed")]
		TwoHanded = 8,

		[Description("Versatile")]
		Versatile = 9,
	}
}
