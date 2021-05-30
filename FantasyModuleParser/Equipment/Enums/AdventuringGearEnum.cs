
using System.ComponentModel;

namespace FantasyModuleParser.Equipment.Enums
{
    public enum AdventuringGearEnum
    {
		[Description("Ammunition")]
		AdventuringGear = 0,

		[Description("Arcane Focus")]
		ArcaneFocus = 1,

		[Description("Druidic Focus")]
		DruidicFocus = 2,

		[Description("Holy Symbol")]
		HolySymbol = 3,

		[Description("Equipment Kits")]
		EquipmentKits = 4,

		[Description("Equipment Packs")]
		EquipmentPacks = 5,

		[Description("Standard")]
		Standard = 6
	}
}
