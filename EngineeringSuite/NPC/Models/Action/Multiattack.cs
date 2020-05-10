using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.Models.Action
{
    public class Multiattack : ActionModelBase
    {
        public new const string LocalActionName = "Multiattack";

        public Multiattack()
        {
            ActionName = LocalActionName;
        }

        public Multiattack(string description)
        {
            ActionName = LocalActionName;
            this.ActionDescription = description;
        }

    }
}
