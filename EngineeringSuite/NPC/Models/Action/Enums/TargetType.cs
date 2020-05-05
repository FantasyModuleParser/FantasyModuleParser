using System.ComponentModel;

namespace EngineeringSuite.NPC.Models.Action.Enums
{
	public enum TargetType
	{
		[Description("one target")]
		target = 0,

		[Description("one creature")]
		creature = 1,
	}
}