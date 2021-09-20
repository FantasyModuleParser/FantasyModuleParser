using System.ComponentModel;

namespace FantasyModuleParser.Classes.Enums
{
    public enum ClassMenuOptionEnum
    {
		[Description("Proficiency Bonus")]
		ProficiencyBonus,

		[Description("Spell Slots")]
		SpellSlots,

		[Description("Proficiencies")]
		Proficiencies,

		[Description("Multiclass Proficiencies")]
		MulticlassProficiencies,

		[Description("Class Features")]
		ClassFeatures,

		[Description("Class Specialization")]
		ClassSpecialization,

		[Description("Starting Equipment")]
		StartingEquipment
    }
}
