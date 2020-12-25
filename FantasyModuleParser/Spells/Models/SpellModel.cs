using FantasyModuleParser.Spells.Enums;
using Newtonsoft.Json;

namespace FantasyModuleParser.Spells.Models
{
    public class SpellModel
    {
        public string SpellName { get; set; }
        public SpellLevel SpellLevel { get; set; }
        public SpellSchool SpellSchool { get; set; }
        public bool IsRitual { get; set; }
        public int CastingTime { get; set; }
        public CastingType CastingType { get; set; }
        public string ReactionDescription { get; set; }
        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Range { get; set; }
        public RangeType RangeType { get; set; }
        public UnitType Unit { get; set; }
        public SelfType SelfType { get; set; }
        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string RangeDescription { get; set; }
        public bool IsVerbalComponent { get; set; }
        public bool IsSomaticComponent { get; set; }
        public bool IsMaterialComponent { get; set; }
        public string ComponentText { get; set; }
        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ComponentDescription { get; set; }
        public string DurationText { get; set; }
        public int DurationTime { get; set; }
        public DurationType DurationType { get; set; }
        public DurationUnit DurationUnit { get; set; }
        public string CastBy { get; set; }
        public string Description { get; set; }
    }
}
