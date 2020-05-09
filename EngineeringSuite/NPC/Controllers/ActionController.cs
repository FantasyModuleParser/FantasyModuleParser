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

        public void RemoveActionFromNPC(ActionModelBase action)
        {
            GetNPCModel().NPCActions.Remove(action);
        }

        public void RaiseActionInNPCActionList(ActionModelBase action)
        {
            ObservableCollection<ActionModelBase> npcActions = GetNPCModel().NPCActions;

            int actionIndex = npcActions.IndexOf(action);
            if(actionIndex == 0)
            {
                //Do nothing!  Already at the top of the list
            }
            if(actionIndex == 1)
            {
                // Check to see if Multiattack exists;  If it does, leave it at the 'top'
                if (!npcActions[0].ActionName.Equals(Multiattack.ActionName))
                {
                    npcActions.Move(actionIndex, actionIndex - 1);
                }
            }
            if(actionIndex > 1)
            {
                npcActions.Move(actionIndex, actionIndex - 1);
            }
        }

        public void LowerActionInNPCActionsList(ActionModelBase action)
        {
            ObservableCollection<ActionModelBase> npcActions = GetNPCModel().NPCActions;

            //Ignore trying to move a Multiattack
            if (action.ActionName.Equals(Multiattack.ActionName))
            {
                return; //no-op
            }

            int actionIndex = npcActions.IndexOf(action);

            //If the action is at the bottom of the list, do nothing
            if((actionIndex + 1) == npcActions.Count)
            {
                return; //no-op
            }

            //Otherwise, move it!
            npcActions.Move(actionIndex, actionIndex + 1);
        }

        public void RaiseActionInLairActionList(Models.Action.LairActions action)
        {
            ObservableCollection<Models.Action.LairActions> lairActions = GetNPCModel().LairActions;

            int actionIndex = lairActions.IndexOf(action);
            if (actionIndex == 0)
            {
                //Do nothing!  Already at the top of the list
            }
            if (actionIndex > 0)
            {
                lairActions.Move(actionIndex, actionIndex - 1);
            }
        }

        public void LowerActionInLairActionsList(Models.Action.LairActions action)
        {
            ObservableCollection<Models.Action.LairActions> lairActions = GetNPCModel().LairActions;

            int actionIndex = lairActions.IndexOf(action);

            //If the action is at the bottom of the list, do nothing
            if ((actionIndex + 1) == lairActions.Count)
            {
                return; //no-op
            }

            //Otherwise, move it!
            lairActions.Move(actionIndex, actionIndex + 1);
        }


        public void UpdateLairAction(Models.Action.LairActions lairAction)
        {
            NPCModel npcModel = GetNPCModel();
            ObservableCollection<LairActions> LairActions = npcModel.LairActions;
            var obj = LairActions.FirstOrDefault(x => x.ActionName == lairAction.ActionName);
            if (obj != null)
            {
                lairAction.ActionID = obj.ActionID;
                LairActions.Remove(obj);
                LairActions.Add(lairAction);
            }
            else
            {
                lairAction.ActionID = LairActions.Count;
                LairActions.Add(lairAction);
            }
        }

        public void RemoveLairAction(Models.Action.LairActions lairAction)
        {
            GetNPCModel().LairActions.Remove(lairAction);
        }
    }
}
