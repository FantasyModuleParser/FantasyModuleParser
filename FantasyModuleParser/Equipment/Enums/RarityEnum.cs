using System.ComponentModel;

namespace FantasyModuleParser.Equipment.Enums
{
	public enum RarityEnum
	{
		[Description("Non-Magical")]
		Mundane = 1,

		[Description("Common")]
		Common = 1,

		[Description("Uncommon")]
		Uncommon = 2,

		[Description("Rare")]
		Rare = 3,

		[Description("Very Rare")]
		VeryRare = 4,

		[Description("Legendary")]
		Legendary = 5,

		[Description("Artifact")]
		Artifact = 6,
	}
}
