using System.ComponentModel;

namespace FantasyModuleParser.Spells.Enums
{
    public enum SelfType
    {
		[Description("--")]
		None = 0,

		[Description("radius")]
		Radius = 1,

		[Description("cone")]
		Cone = 2,

		[Description("cube")]
		Cube = 3,

		[Description("line")]
		Line = 4,

		[Description("sphere")]
		Sphere = 5,
	}
}
