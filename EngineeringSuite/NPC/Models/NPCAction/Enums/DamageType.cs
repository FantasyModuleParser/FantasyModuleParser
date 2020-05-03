using System.ComponentModel;

namespace EngineeringSuite.NPC.Models.NPCAction.Enums
{
	public enum DamageType
	{
		[Description("Bludgeoning")]
		Bludgeoning = 0,

		[Description("Piercing")]
		Piercing = 1,

		[Description("Slashing")]
		Slashing = 2,

		[Description("Acid")]
		Acid = 3,

		[Description("Cold")]
		Cold = 4,

		[Description("Fire")]
		Fire = 5,

		[Description("Force")]
		Force = 6,

		[Description("Lightning")]
		Lightning = 7,

		[Description("Necrotic")]
		Necrotic = 8,

		[Description("Poison")]
		Poison = 9,

		[Description("Psychic")]
		Psychic = 10,

		[Description("Radiant")]
		Radiant = 11,

		[Description("Thunder")]
		Thunder = 12,
	}
}