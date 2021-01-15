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

		[Description("Ranged")]
		Ranged = 3,

		[Description("Sight")]
		Sight = 4,

		[Description("Special")]
		Special = 5,

		[Description("Unlimited")]
		Unlimited = 6,
    }
}
