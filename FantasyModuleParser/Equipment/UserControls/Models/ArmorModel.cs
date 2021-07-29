using Newtonsoft.Json;
using System.ComponentModel;

namespace FantasyModuleParser.Equipment.UserControls.Models
{
    public class ArmorModel
    {
        public int ArmorValue { get; set; }
        public int StrengthRequirement { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string DexterityBonus { get; set; }
        public bool IsStealthDisadvantage { get; set; }
    }
}
