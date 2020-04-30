using EngineeringSuite.NPC.Models.NPCAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EngineeringSuite.NPC.Controllers
{
    class NPCActionController
    {
        // Returns an ActionOverviewModel object based on the itemNumber from the
        // compiled list, in this order:
        // MultiAttack (if applicable)
        // WeaponAttack (in List order)
        // OtherAttack (in List order)
        public ActionOverviewModel GetActionOverviewModel(ActionDataModel npcActions, int itemNumber)
        {
            ActionOverviewModel result = new ActionOverviewModel();
            //Count how many actions are available, in case null should be returned
            int actionCount = 0;
            if (npcActions.MultiAttack != null)
                actionCount++;
            if (npcActions.WeaponAttacks != null)
                actionCount += npcActions.WeaponAttacks.Count;
            if (npcActions.OtherActions != null)
                actionCount += npcActions.OtherActions.Count;

            if (itemNumber >= actionCount)
                return result;

            if(itemNumber == 0 && npcActions.MultiAttack != null)
            {
                result.ActionName = "MultiAttack";
                result.ActionDescription = npcActions.MultiAttack.description;
            } 
            else
            {
                int tempItemNumber = itemNumber;
                if (npcActions.MultiAttack != null)
                    tempItemNumber--;

                if(tempItemNumber <= npcActions.WeaponAttacks.Count)
                {
                    result.ActionName = npcActions.WeaponAttacks[tempItemNumber].WeaponName;
                    result.ActionDescription = npcActions.WeaponAttacks[tempItemNumber].GenerateWeaponAttackDescription();
                } 
                else
                {
                    tempItemNumber -= npcActions.WeaponAttacks.Count;
                    // Since we did our item count check earlier, we know that there has to 
                    // be at least one record in the OtherActions list
                    result.ActionName = npcActions.OtherActions[tempItemNumber].ActionName;
                    result.ActionDescription = npcActions.OtherActions[tempItemNumber].ActionDescription;
                }
            }

            return result;
        }
    }
}
