using System.ComponentModel;

namespace FantasyModuleParser.Equipment.Enums
{
	public enum RarityEnum
	{
		[Description("Common")]
		Common = 0,

		[Description("Uncommon")]
		Uncommon = 1,

		[Description("Rare")]
		Rare = 2,

		[Description("Very Rare")]
		VeryRare = 3,

		[Description("Legendary")]
		Legendary = 4,

		[Description("Artifact")]
		Artifact = 5,
	}
}
