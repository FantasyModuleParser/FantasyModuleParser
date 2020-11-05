using System.ComponentModel;

namespace FantasyModuleParser.Spells.Enums
{
    public enum RangeType
    {
		[Description("--")]
		None = 0,

		[Description("Self")]
		Self = 1,

		[Description("Touch")]
		Touch = 2,

		[Description("ranged")]
		Ranged = 3,

		[Description("Sight")]
		Sight = 4,

		[Description("Unlimited")]
		Unlimited = 5,
    }
}
