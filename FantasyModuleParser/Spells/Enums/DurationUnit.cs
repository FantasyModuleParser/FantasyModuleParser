using System.ComponentModel;

namespace FantasyModuleParser.Spells.Enums
{
    public enum DurationUnit
    {
		[Description("--")]
		None = 0,

		[Description("round")]
		Round = 1,

		[Description("minute")]
		Minute = 2,

		[Description("hour")]
		Hour = 3,

		[Description("day")]
		Day = 4,
	}
}
