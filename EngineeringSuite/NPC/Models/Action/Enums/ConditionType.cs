using System.ComponentModel;

namespace EngineeringSuite.NPC.Models.Action.Enums
{
    public enum ConditionType
    {
		[Description("Blinded")]
		Blinded = 0,

		[Description("Charmed")]
		Charmed = 1,

		[Description("Deafened")]
		Deafened = 2,

		[Description("Exhaustion")]
		Exhaustion = 3,

		[Description("Frightened")]
		Frightened = 4,

		[Description("Grappled")]
		Grappled = 5,

		[Description("Incapacitated")]
		Incapacitated = 6,

		[Description("Invisible")]
		Invisible = 7,

		[Description("Paralyzed")]
		Paralyzed = 8,

		[Description("Petrified")]
		Petrified = 9,

		[Description("Poisoned")]
		Poisoned = 10,

		[Description("Prone")]
		Prone = 11,

		[Description("Restrained")]
		Restrained = 12,

		[Description("Stunned")]
		Stunned = 13,

		[Description("Unconscious")]
		Unconscious = 14,
	}
}
