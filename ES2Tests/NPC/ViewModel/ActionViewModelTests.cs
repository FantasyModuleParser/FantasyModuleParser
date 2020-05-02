using Microsoft.VisualStudio.TestTools.UnitTesting;
using EngineeringSuite.NPC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringSuite.NPC.Models.NPCAction;

namespace EngineeringSuite.NPC.ViewModel.Tests
{
    [TestClass()]
    public class ActionViewModelTests
    {

        Multiattack multiattack;
        List<WeaponAttack> weaponAttacks;
        List<OtherAction> otherActions;

        ActionViewModel actionViewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            actionViewModel = new ActionViewModel();

            multiattack = new Multiattack("Multiattack Test");
            weaponAttacks = new List<WeaponAttack>();
            otherActions = new List<OtherAction>();

            weaponAttacks.Add(createWeaponAttack("Dagger", new DamageProperty(1, 4, 0, "piercing"), 5, 4));
            weaponAttacks.Add(createWeaponAttack("ShortSword", new DamageProperty(1, 6, 0, "piercing"), 5, 5));
            weaponAttacks.Add(createWeaponAttack("Grenade", new DamageProperty(4, 8, 0, "fire"), 10, 99));

            otherActions.Add(createOtherAction("Test OA 1", "This is a description"));
            otherActions.Add(createOtherAction("GODLIKE", "Never use this, even once..."));
        }

        private WeaponAttack createWeaponAttack(String actionName, DamageProperty damageProperty, int reach, int toHit)
        {
            WeaponAttack weaponAttack = new WeaponAttack();
            weaponAttack.ActionName = actionName;

            weaponAttack.damageProperties = new List<DamageProperty>();
            weaponAttack.damageProperties.Add(damageProperty);
            weaponAttack.Reach = reach;
            weaponAttack.ToHit = toHit;

            return weaponAttack;
            
        }
        
        private OtherAction createOtherAction(String name, String desc)
        {
            OtherAction result = new OtherAction();
            result.ActionName = name;
            result.ActionDescription = desc;

            return result;
        }

        #region Multiattack Unit tests

        [TestMethod]
        public void addMultiattack()
        {
            actionViewModel.updateMultiAttack(multiattack);
            Assert.IsTrue(actionViewModel.NPCActions.Count == 1);
            
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionName.Equals("Multiattack"));
        }

        // This unit test should prove that only one multiattack can be present for a NPC
        [TestMethod]
        public void addMultipleMultiattack()
        {
            actionViewModel.updateMultiAttack(multiattack);
            Assert.IsTrue(actionViewModel.NPCActions.Count == 1);
            actionViewModel.updateMultiAttack(multiattack);

            Assert.IsTrue(actionViewModel.NPCActions.Count == 1);
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionName.Equals("Multiattack"));
        }

        [TestMethod]
        public void updateMultiattack()
        {
            actionViewModel.updateMultiAttack(multiattack);
            Assert.IsTrue(actionViewModel.NPCActions.Count == 1);
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionID == 0);
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionName.Equals("Multiattack"));
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionDescription.Equals("Multiattack Test"));

            multiattack.ActionDescription = "Look at me now, Dad!";
            actionViewModel.updateMultiAttack(multiattack);
            Assert.IsTrue(actionViewModel.NPCActions.Count == 1);
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionID == 0);
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionName.Equals("Multiattack"));
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionDescription.Equals("Look at me now, Dad!"));
        }

        #endregion

        #region Weapon Attack Unit Tests
        [TestMethod]
        public void createWeaponAttackTest()
        {
            actionViewModel.updateWeaponAttack(weaponAttacks[0]);
            Assert.IsTrue(actionViewModel.NPCActions.Count == 1);
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionID == 0);
            Assert.IsTrue(actionViewModel.NPCActions[0] is WeaponAttack);

        }
        #endregion

        #region Other Action Unit Tests

        [TestMethod()]
        public void addOtherActionTest()
        {
            actionViewModel.updateOtherAction(otherActions[0]);

            Assert.IsTrue(actionViewModel.NPCActions.Count == 1);
        }

        [TestMethod()]
        public void addMultipleOtherActionTest()
        {
            actionViewModel.updateOtherAction(otherActions[0]);
            Assert.IsTrue(actionViewModel.NPCActions.Count == 1);
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionID == 0);
            actionViewModel.updateOtherAction(otherActions[1]);
            Assert.IsTrue(actionViewModel.NPCActions.Count == 2);
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionID == 0);
            Assert.IsTrue(actionViewModel.NPCActions[1].ActionID == 1);
        }

        [TestMethod()]
        public void updateSingleOtherAction()
        {
            actionViewModel.updateOtherAction(otherActions[0]);
            Assert.IsTrue(actionViewModel.NPCActions.Count == 1);
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionID == 0);
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionName.Equals("Test OA 1"));
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionDescription.Equals("This is a description"));

            // Update the existing otherAction
            otherActions[0].ActionDescription = "My New Other Action Awesome";

            //Validate the NPCActions list that the original item hasn't updated
            Assert.IsTrue(actionViewModel.NPCActions.Count == 1);
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionID == 0);
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionName.Equals("Test OA 1"));
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionDescription.Equals("This is a description"));

            //Now, run the update to see if the change is good
            actionViewModel.updateOtherAction(otherActions[0]);
            Assert.IsTrue(actionViewModel.NPCActions.Count == 1);
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionID == 0);
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionName.Equals("Test OA 1"));
            Assert.IsTrue(actionViewModel.NPCActions[0].ActionDescription.Equals("My New Other Action Awesome"));
        }

        #endregion
    }
}