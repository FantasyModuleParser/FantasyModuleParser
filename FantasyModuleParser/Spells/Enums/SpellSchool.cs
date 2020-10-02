using System.ComponentModel;

namespace FantasyModuleParser.Spells.Enums
{
    public enum SpellSchool
    {
		[Description("abjuration")]
		Abjuration = 0,

		[Description("conjuration")]
		Conjuration = 1,

		[Description("divination")]
		Divination = 2,

		[Description("enchantment")]
		Enchantment = 3,

		[Description("evocation")]
		Evocation = 4,

		[Description("illusion")]
		Illusion = 5,

		[Description("necromancy")]
		Necromancy = 6,

		[Description("transmutation")]
		Transmutation = 7,
	}
}
