using System.ComponentModel;

namespace FantasyModuleParser.Spells.Enums
{
    public enum CastingType
    {
		[Description("--")]
		None = 0,

		[Description("action")]
		Action = 1,

		[Description("bonus action")]
		BonusAction = 2,

		[Description("reaction")]
		Reaction = 3,

		[Description("hour")]
		Hour = 4,

		[Description("minute")]
		Minute = 5,
	}
}
