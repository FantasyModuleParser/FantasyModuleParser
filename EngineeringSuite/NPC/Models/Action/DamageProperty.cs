using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringSuite.NPC.Models.Action.Enums;

namespace EngineeringSuite.NPC.Models.Action
{
    public class DamageProperty
    {
	    public int NumOfDice { get; set; }

        public DieType DieType { get; set; }

        public int Bonus { get; set; }

        public DamageType DamageType { get; set; }
    }
}
