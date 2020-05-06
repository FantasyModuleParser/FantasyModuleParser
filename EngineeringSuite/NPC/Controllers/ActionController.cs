using EngineeringSuite.NPC.Models.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.Controllers
{
    public class ActionController
    {
        public void GenerateWeaponDescription(WeaponAttack weaponAttack)
        {
            _ = weaponAttack.GenerateWeaponAttackDescription();  // TODO:  This method call may not exist currently in the WeaponAttack class!
        }
    }
}
