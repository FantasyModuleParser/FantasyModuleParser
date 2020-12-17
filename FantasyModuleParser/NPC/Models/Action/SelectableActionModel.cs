using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.NPC.Models.Action
{
    public class SelectableActionModel : ActionModelBase
    {
        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool Selected { get; set; }

        public SelectableActionModel()
        {
            Selected = false;
        }

        public SelectableActionModel(bool selected)
        {
            Selected = selected;
        }

        public SelectableActionModel(int actionId, string actionName, string actionDescription, bool selected) : base(actionId, actionName, actionDescription)
        {
            Selected = selected;
        }

        public override string ToString()
        {
            return ActionDescription;
        }
    }
}
