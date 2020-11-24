using System.ComponentModel;

namespace FantasyModuleParser.Spells.Enums
{
    public enum SpellSchool
    {
		[Description("Abjuration")]
		Abjuration = 0,

		[Description("Conjuration")]
		Conjuration = 1,

		[Description("Divination")]
		Divination = 2,

		[Description("Enchantment")]
		Enchantment = 3,

		[Description("Evocation")]
		Evocation = 4,

		[Description("Illusion")]
		Illusion = 5,

		[Description("Necromancy")]
		Necromancy = 6,

		[Description("Transmutation")]
		Transmutation = 7,
	}
}
