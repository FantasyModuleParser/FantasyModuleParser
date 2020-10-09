using System.ComponentModel;

namespace FantasyModuleParser.Spells.Enums
{
    public enum DurationType
    {
		[Description("--")]
		None = 0,

		[Description("concentration")]
		Concentration = 1,

		[Description("instantaneous")]
		Instantaneous = 2,

		[Description("time")]
		Time = 3,

		[Description("until dispelled")]
		UntilDispelled = 4,

		[Description("until dispelled or triggered")]
		UntilDispelledOrTriggered = 5,
	}
}
