﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.NPC.Models.Action
{
    public class WeaponImmunityModel : SelectableActionModel
    {
        public WeaponImmunityModel(int actionId, string actionName, string actionDescription, bool selected) : base(actionId, actionName, actionDescription, selected)
        {
        }
    }
}
