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
        public void UpdateMultiAttack(Multiattack multiAttack)
        {
            // Now, we're in the UpdateMultiAttack method

            // The reason for this deep clone is because C# is pass-by-reference
            // For multiattack, this doesn't matter much... but if you were to make a 
            // WeaponAttack object, update the NPCActions with the WA object, things are fine...
            // However, if you then make changes to that WA object.. they get persisted instead of creating
            // a new list object.

            // Cloning prevents this, because we make a stand-alone copy of our object, and persist
            // that to our NPCActions collection.
            Multiattack clone = CommonMethod.CloneJson(multiAttack);
            updateNPCActions(clone);
        }

        public void UpdateOtherAction(OtherAction otherAction)
        {
            OtherAction clone = CommonMethod.CloneJson(otherAction);
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
