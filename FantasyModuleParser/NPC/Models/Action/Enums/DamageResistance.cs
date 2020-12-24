using System.ComponentModel;

namespace FantasyModuleParser.NPC.Models.Action.Enums
{
	public enum DamageResistance
	{
		[Description("bludgeoning")]
		Bludgeoning = 0,

		[Description("piercing")]
		Piercing = 1,

		[Description("slashing")]
		Slashing = 2,

		[Description("acid")]
		Acid = 3,

		[Description("cold")]
		Cold = 4,

		[Description("fire")]
		Fire = 5,

		[Description("force")]
		Force = 6,

		[Description("lightning")]
		Lightning = 7,

		[Description("necrotic")]
		Necrotic = 8,

		[Description("poison")]
		Poison = 9,

		[Description("psychic")]
		Psychic = 10,

		[Description("radiant")]
		Radiant = 11,

		[Description("thunder")]
		Thunder = 12,

		[Description("damage from spells")]
		DamageFromSpells = 13,
	}
}