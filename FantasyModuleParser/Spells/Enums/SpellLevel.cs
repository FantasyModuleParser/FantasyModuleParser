using System.ComponentModel;

namespace FantasyModuleParser.Spells.Enums
{
    public enum SpellLevel
    {
		[Description("cantrip")]
		Cantrip = 0,

		[Description("1st level")]
		First = 1,

		[Description("2nd level")]
		Second = 2,

		[Description("3rd level")]
		Third = 3,

		[Description("4th level")]
		Fourth = 4,

		[Description("5th level")]
		Fifth = 5,

		[Description("6th level")]
		Sixth = 6,

		[Description("7th level")]
		Seventh = 7,

		[Description("8th level")]
		Eighth = 8,

		[Description("9th level")]
		Ninth = 9,
	}
}
