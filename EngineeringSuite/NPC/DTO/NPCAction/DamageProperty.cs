using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.DTO.NPCAction
{
    class DamageProperty
    {
        int numOfDice;
        int dieType; // valid values:  4, 6, 8, 10, 12, 20
        int bonus;
        String damageType;
    }
}
