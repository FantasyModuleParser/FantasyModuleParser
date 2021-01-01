using FantasyModuleParser.Spells.Enums;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Text;
using System.Windows;
using FantasyModuleParser.Extensions;

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
        [DefaultValue(0)]
        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Range { get; set; }
        [DefaultValue(0)]
        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public RangeType RangeType { get; set; }
        [DefaultValue(0)]
        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public UnitType Unit { get; set; }
        [DefaultValue(0)]
        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SelfType SelfType { get; set; }
        [DefaultValue("None")]
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
        [DefaultValue("None")]
        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DurationType DurationType { get; set; }
        [DefaultValue("None")]
        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DurationUnit DurationUnit { get; set; }
        public string CastBy { get; set; }
        public string Description { get; set; }
    }
}
