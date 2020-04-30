using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.Models.NPCAction
{
    public class Multiattack
    {
        public Multiattack()
        {
        }

        public Multiattack(string description)
        {
            this.description = description;
        }

        public String description { get; set; }

        public override string ToString()
        {
            return " -- MultiAttack [" + this.description + "] -- ";
        }
    }
}
