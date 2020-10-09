using System.ComponentModel;

namespace FantasyModuleParser.Spells.Enums
{
    public enum RangeType
    {
		[Description("--")]
		None = 0,

		[Description("self")]
		Self = 1,

		[Description("touch")]
		Touch = 2,

		[Description("ranged")]
		Ranged = 3,

		[Description("sight")]
		Sight = 4,

		[Description("unlimited")]
		Unlimited = 5,
    }
}
