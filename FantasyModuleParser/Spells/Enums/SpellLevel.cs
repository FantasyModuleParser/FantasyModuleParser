using System.ComponentModel;

namespace FantasyModuleParser.Spells.Enums
{
    public enum SpellLevel
    {
		[Description("cantrip")]
		Cantrip = 0,

		[Description("1st level")]
		FirstLevel = 1,

		[Description("2nd level")]
		SecondLevel = 2,

		[Description("3rd level")]
		ThirdLevel = 3,

		[Description("4th level")]
		FourthLevel = 4,

		[Description("5th level")]
		FifthLevel = 5,

		[Description("6th level")]
		SixthLevel = 6,

		[Description("7th level")]
		SeventhLevel = 7,

		[Description("8th level")]
		EighthLevel = 8,

		[Description("9th level")]
		NinthLevel = 9,
	}
}
