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

		[Description("Animals")]
		Animals = 4,

		[Description("Vehicles")]
		Vehicles = 5,

		[Description("Treasure")]
		Treasure = 6
	}
}
