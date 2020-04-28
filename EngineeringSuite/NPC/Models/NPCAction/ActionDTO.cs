using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.DTO.NPCAction
{
    internal class ActionDTO
    {
        public String id { get; set; }
        public Multiattack multiattack { get; set; }
        public List<WeaponAttack> weaponAttackList { get; set; }
        public List<OtherAction> otherActionList { get; set; }

        public override string ToString()
        {
            return multiattack.ToString() + "\n AND OTHER STUFF!!!";
        }
    }
}
