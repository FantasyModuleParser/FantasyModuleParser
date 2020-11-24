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

		[Description("time")]
		Time = 3,

		[Description("until dispelled")]
		UntilDispelled = 4,

		[Description("until dispelled or triggered")]
		UntilDispelledOrTriggered = 5,
	}
}
