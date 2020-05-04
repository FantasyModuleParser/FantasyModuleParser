using System.ComponentModel;

namespace EngineeringSuite.NPC.Models.NPCAction.Enums
{
	public enum TargetType
	{
		[Description("One Target")]
		Target = 0,

		[Description("One Creature")]
		Creature = 1,
	}
}