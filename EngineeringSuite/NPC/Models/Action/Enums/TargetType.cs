using System.ComponentModel;

namespace EngineeringSuite.NPC.Models.Action.Enums
{
	public enum TargetType
	{
		[Description("One Target")]
		Target = 0,

		[Description("One Creature")]
		Creature = 1,
	}
}