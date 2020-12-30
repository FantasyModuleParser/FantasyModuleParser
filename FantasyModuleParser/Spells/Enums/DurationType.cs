using System.ComponentModel;

namespace FantasyModuleParser.Spells.Enums
{
    public enum DurationType
    {
		[Description("--")]
		None = 0,

		[Description("Concentration")]
		Concentration = 1,

		[Description("Instantaneous")]
		Instantaneous = 2,

		[Description("Time")]
		Time = 3,

		[Description("Until dispelled")]
		UntilDispelled = 4,

		[Description("Until dispelled or triggered")]
		UntilDispelledOrTriggered = 5,
	}
}
