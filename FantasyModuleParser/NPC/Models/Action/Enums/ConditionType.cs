using System.ComponentModel;

namespace FantasyModuleParser.NPC.Models.Action.Enums
{
    public enum ConditionType
    {
		[Description("blinded")]
		Blinded = 0,

		[Description("charmed")]
		Charmed = 1,

		[Description("deafened")]
		Deafened = 2,

		[Description("exhaustion")]
		Exhaustion = 3,

		[Description("frightened")]
		Frightened = 4,

		[Description("grappled")]
		Grappled = 5,

		[Description("incapacitated")]
		Incapacitated = 6,

		[Description("invisible")]
		Invisible = 7,

		[Description("paralyzed")]
		Paralyzed = 8,

		[Description("petrified")]
		Petrified = 9,

		[Description("poisoned")]
		Poisoned = 10,

		[Description("prone")]
		Prone = 11,

		[Description("restrained")]
		Restrained = 12,

		[Description("stunned")]
		Stunned = 13,

		[Description("unconscious")]
		Unconscious = 14,
	}
}
