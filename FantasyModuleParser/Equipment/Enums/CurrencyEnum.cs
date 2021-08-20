
using System.ComponentModel;

namespace FantasyModuleParser.Equipment.Enums
{
	public enum CurrencyEnum
	{
		[Description("-")]
		None = 0,

		[Description("cp")]
		CP = 1,

		[Description("ep")]
		EP = 2,

		[Description("sp")]
		SP = 3,

		[Description("gp")]
		GP = 4,

		[Description("pp")]
		PP = 5,
	}
}
