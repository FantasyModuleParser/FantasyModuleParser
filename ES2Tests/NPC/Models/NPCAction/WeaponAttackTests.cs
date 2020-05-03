using Microsoft.VisualStudio.TestTools.UnitTesting;
using EngineeringSuite.NPC.Models.NPCAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.Models.NPCAction.Tests
{
    [TestClass()]
    public class WeaponAttackTests
    {
        private WeaponAttack weaponAttack;

        [TestInitialize]
        public void Initialize()
        {
            weaponAttack = new WeaponAttack();
        }

        #region Weapon Attack Description Tests
        [TestMethod, TestCategory("Weapon Attack Description")]
        public void defaultConstructorDescription()
        {
            weaponAttack.GenerateWeaponAttackDescription();
            assertWeaponAttackDescription("Melee Weapon Attack: +0 to hit, reach 5 ft., one target. Hit: 3 (1d6) damage.");
        }

        #region Stat Changes

        [TestMethod, TestCategory("Weapon Attack Description")]
        public void toHitDescription()
        {
            weaponAttack.ToHit = 5;
            assertWeaponAttackDescription("Melee Weapon Attack: +5 to hit, reach 5 ft., one target. Hit: 3 (1d6) damage.");
        }

        [TestMethod, TestCategory("Weapon Attack Description")]
        public void reachDescription()
        {
            weaponAttack.Reach = 15;
            assertWeaponAttackDescription("Melee Weapon Attack: +0 to hit, reach 15 ft., one target. Hit: 3 (1d6) damage.");
        }

        [TestMethod, TestCategory("Weapon Attack Description")]
        public void rangedWeaponAttackDescription()
        {
            weaponAttack.WeaponType = "Ranged Weapon Attack";
            assertWeaponAttackDescription("Ranged Weapon Attack: +0 to hit, range 30/60 ft., one target. Hit: 3 (1d6) damage.");
        }

        [TestMethod, TestCategory("Weapon Attack Description")]
        public void meleeSpellAttackDescription()
        {
            weaponAttack.Reach = 15;
            assertWeaponAttackDescription("Melee Spell Attack: +0 to hit, reach 5 ft., one target. Hit: 3 (1d6) damage.");
        }

        #endregion

        #region Special Properties (Magic, Silver, Adamantine, Cold-Forged Iron, Versatile)

        [TestMethod, TestCategory("Weapon Attack Description")]
        public void magicDescription()
        {
            weaponAttack.IsMagic = true;
            assertWeaponAttackDescription("Melee Weapon Attack: +0 to hit, reach 5 ft., one target. Hit: 3 (1d6), magic damage.");
        }


        [TestMethod, TestCategory("Weapon Attack Description")]
        public void silverDescription()
        {
            weaponAttack.IsMagic = true;
            assertWeaponAttackDescription("Melee Weapon Attack: +0 to hit, reach 5 ft., one target. Hit: 3 (1d6), silver damage.");
        }

        #endregion

        private void assertWeaponAttackDescription(String expected)
        {
            weaponAttack.GenerateWeaponAttackDescription();
            Assert.AreEqual(expected, weaponAttack.ActionDescription);
        }
    }
}