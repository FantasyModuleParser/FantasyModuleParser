using System.ComponentModel;

namespace FantasyModuleParser.Spells.Enums
{
    public enum SpellLevel
    {
		[Description("cantrip")]
		Cantrip = 0,

		[Description("1st")]
		First = 1,

		[Description("2nd")]
		Second = 2,

		[Description("3rd")]
		Third = 3,

		[Description("4th")]
		Fourth = 4,

		[Description("5th")]
		Fifth = 5,

		[Description("6th")]
		Sixth = 6,

		[Description("7th")]
		Seventh = 7,

		[Description("8th")]
		Eighth = 8,

		[Description("9th")]
		Ninth = 9,
	}
}
