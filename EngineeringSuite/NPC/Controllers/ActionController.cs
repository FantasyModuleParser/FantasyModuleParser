using EngineeringSuite.NPC.Models.Action;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.Controllers
{
    public class ActionController : ControllerBase
    {
        public void GenerateWeaponDescription(WeaponAttack weaponAttack)
        {
            weaponAttack.GenerateWeaponAttackDescription();
        }

        public void UpdateWeaponAttackAction(WeaponAttack weaponAttack)
        {
            //Refresh the description, just in case changes were done
            GenerateWeaponDescription(weaponAttack);

            // Clone the object due to pass-by-reference
            WeaponAttack clone = CommonMethod.CloneJson(weaponAttack);
            updateNPCActions(clone);
        }

        private void updateNPCActions(ActionModelBase action)
        {
            NPCModel npcModel = GetNPCModel();
            ObservableCollection<ActionModelBase> NPCActions = npcModel.NPCActions;
            var obj = NPCActions.FirstOrDefault(x => x.ActionName == action.ActionName);
            if (obj != null)
            {
                action.ActionID = obj.ActionID;
                NPCActions.Remove(obj);
                NPCActions.Add(action);
            }

            else
            {
                action.ActionID = NPCActions.Count;
                NPCActions.Add(action);
            }
        }
    }
}
