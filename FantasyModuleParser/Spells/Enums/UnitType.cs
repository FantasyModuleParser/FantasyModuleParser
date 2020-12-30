using System.ComponentModel;

namespace FantasyModuleParser.Spells.Enums
{
    public enum UnitType
    {
		[Description("--")]
		None = 0,

		[Description("feet")]
		Feet = 1,

		[Description("foot")]
		Foot = 2,

		[Description("mile")]
		Mile = 3,

		[Description("miles")]
		Miles = 4,
	}
}
