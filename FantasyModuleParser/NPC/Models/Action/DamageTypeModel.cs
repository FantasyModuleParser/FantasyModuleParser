﻿namespace FantasyModuleParser.NPC.Models.Action
{
    public class DamageTypeModel : SelectableActionModel
    {
        public DamageTypeModel(int actionId, string actionName, string actionDescription, bool selected) : base(actionId, actionName, actionDescription, selected)
        {
        }
    }
}
