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

		[Description("Mounts and Other Animals")]
		MountsAndOtherAnimals = 4,

		[Description("Tack Harness and Drawn Vehicles")]
		TackHarnessAndDrawnVehicles = 5,

		[Description("Waterborne Vehicles")]
		WaterborneVehicles = 6,

		[Description("Treasure")]
		Treasure = 7
	}
}
