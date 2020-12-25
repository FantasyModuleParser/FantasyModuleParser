using System.ComponentModel;

namespace FantasyModuleParser.Spells.Enums
{
    public enum UnitType
    {
		[Description("feet")]
		Feet = 0,

		[Description("foot")]
		Foot = 1,

		[Description("mile")]
		Mile = 2,

		[Description("miles")]
		Miles = 3,
	}
}
