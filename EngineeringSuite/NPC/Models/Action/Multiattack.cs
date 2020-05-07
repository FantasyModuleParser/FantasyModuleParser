using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.Models.Action
{
    public class Multiattack : ActionModelBase
    {
        private const string actionName = "Multiattack";

        public Multiattack()
        {
            this.ActionName = actionName;
        }

        public Multiattack(string description)
        {
            this.ActionName = actionName;
            this.ActionDescription = description;
        }

    }
}
