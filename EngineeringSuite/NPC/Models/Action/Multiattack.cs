using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.Models.Action
{
    public class Multiattack : ActionModelBase
    {
        public const string ActionName = "Multiattack";
        
        public Multiattack()
        {
            //No-op
        }
        
        public Multiattack(string description)
        {
            this.ActionDescription = description;
        }

    }
}
