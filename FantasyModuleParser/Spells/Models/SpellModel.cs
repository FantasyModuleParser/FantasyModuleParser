using FantasyModuleParser.Spells.Enums;

namespace FantasyModuleParser.Spells.Models
{
    public class SpellModel
    {
        public string SpellName { get; set; }
        public SpellLevel SpellLevel { get; set; }
        public SpellSchool SpellSchool { get; set; }
        public bool IsRitual { get; set; }
        public string CastingTime { get; set; }
        public CastingType CastingType { get; set; }
        public string ReactionDescription { get; set; }
        public string Range { get; set; }
        public RangeType RangeType { get; set; }
        public bool IsVerbalComponent { get; set; }
        public bool IsSomaticComponent { get; set; }
        public bool IsMaterialComponent { get; set; }
        public int DurationTime { get; set; }
        public DurationType DurationType { get; set; }
        public DurationUnit DurationUnit { get; set; }
        public string CastBy { get; set; }
        public string Description { get; set; }
    }
}
