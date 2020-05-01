using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.Models.NPCAction
{
    public class Multiattack : ActionModelBase
    {
        public Multiattack()
        {
            this.ActionName = "MultiAttack";
        }

        public Multiattack(string description)
        {
            this.ActionName = "MultiAttack";
            this.ActionDescription = description;
        }

        public override string ToString()
        {
            return " -- MultiAttack [" + this.ActionDescription + "] -- ";
        }
    }
}
