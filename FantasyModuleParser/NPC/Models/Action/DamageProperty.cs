using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyModuleParser.NPC.Models.Action.Enums;

namespace FantasyModuleParser.NPC.Models.Action
{
    public class DamageProperty
    {
	    public int NumOfDice { get; set; }

        public DieType DieType { get; set; }

        public int Bonus { get; set; }

        public DamageType DamageType { get; set; }
    }
}
