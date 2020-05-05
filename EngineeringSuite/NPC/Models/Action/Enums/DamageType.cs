using System.ComponentModel;

namespace EngineeringSuite.NPC.Models.Action.Enums
{
	public enum DamageType
	{
		[Description("bludgeoning")]
		bludgeoning = 0,

		[Description("piercing")]
		piercing = 1,

		[Description("slashing")]
		slashing = 2,

		[Description("acid")]
		acid = 3,

		[Description("cold")]
		cold = 4,

		[Description("fire")]
		fire = 5,

		[Description("force")]
		force = 6,

		[Description("lightning")]
		lightning = 7,

		[Description("necrotic")]
		necrotic = 8,

		[Description("poison")]
		poison = 9,

		[Description("psychic")]
		psychic = 10,

		[Description("radiant")]
		radiant = 11,

		[Description("thunder")]
		thunder = 12,
	}
}