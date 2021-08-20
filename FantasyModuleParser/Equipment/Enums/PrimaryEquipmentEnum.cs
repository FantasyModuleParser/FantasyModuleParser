using System.ComponentModel;

namespace FantasyModuleParser.Equipment.Enums
{
    public enum PrimaryEquipmentEnum
    {
		[Description("Adventuring Gear")]
		AdventuringGear = 0,

		[Description("Armor")]
		Armor = 1,

		[Description("Weapon")]
		Weapon = 2,

		[Description("Tools")]
		Tools = 3,

		[Description("Mounts & Other Animals")]
		Animals = 4,

		[Description("Tack, Harness, and Drawn Vehicles")]
		Tack = 5,

		[Description("Waterborne Vehicles")]
		Vehicles = 6,

		[Description("Treasure")]
		Treasure = 7
	}
}
