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

        [TestMethod()]
        public void defaultConstructorDescription()
        {
            weaponAttack.GenerateWeaponAttackDescription();

            Assert.AreEqual("Melee Weapon Attack: +0 to hit, reach 5 ft., one target. Hit: 3 (1d6)  damage.", weaponAttack.ActionDescription);

        }
    }
}