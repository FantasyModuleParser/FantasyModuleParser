using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Action.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace FantasyModuleParser.Importer.NPC.Tests
{
    [TestClass()]
    public class ImportDnDBeyondNPCTests
    {

        private IImportNPC _iImportNPC;
        NPCModel actualNPCModel = null;

        [TestInitialize]
        public void Initialize()
        {
            _iImportNPC = new ImportDnDBeyondNPC();
            actualNPCModel = LoadEngineerSuiteTestNPCFile();
        }

        private NPCModel LoadEngineerSuiteTestNPCFile()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.DnDBeyond.Aboleth.txt");

            return _iImportNPC.ImportTextToNPCModel(fileContent);
        }

        private NPCModel LoadEngineerSuiteTestNPCFile(String npcName)
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.DnDBeyond." + npcName + ".txt");

            return _iImportNPC.ImportTextToNPCModel(fileContent);
        }

        private string GetEmbeddedResourceFileContent(string embeddedResourcePath)
        {

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@embeddedResourcePath))
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        [TestMethod()]
        public void Test_Validate_Aboleth_Traits()
        {
            Assert.AreEqual(3, actualNPCModel.Traits.Count);
            Assert.AreEqual("Amphibious", actualNPCModel.Traits[0].ActionName);
            Assert.AreEqual("The aboleth can breathe air and water.", actualNPCModel.Traits[0].ActionDescription);

            Assert.AreEqual("Mucous Cloud", actualNPCModel.Traits[1].ActionName);
            Assert.AreEqual("While underwater, the aboleth is surrounded by transformative mucus. A creature that touches the aboleth or that hits it with a melee attack while within 5 feet of it must make a DC 14 Constitution saving throw. On a failure, the creature is diseased for 1d4 hours. The diseased creature can breathe only underwater.", actualNPCModel.Traits[1].ActionDescription);

            Assert.AreEqual("Probing Telepathy", actualNPCModel.Traits[2].ActionName);
            Assert.AreEqual("If a creature communicates telepathically with the aboleth, the aboleth learns the creature's greatest desires if the aboleth can see the creature.", actualNPCModel.Traits[2].ActionDescription);
        }

        [TestMethod]
        public void Test_Validate_Aboleth_Telepathy()
        {
            Assert.AreEqual(true, actualNPCModel.Telepathy);
            Assert.AreEqual("120 ft.", actualNPCModel.TelepathyRange);
        }
        [TestMethod]
        public void Test_Validate_Aboleth_StandardAction_Count()
        {
            Assert.AreEqual(4, actualNPCModel.NPCActions.Count);
        }
        [TestMethod]
        public void Test_Validate_Aboleth_StandardAction_Multiattack()
        {
            ActionModelBase standardAction = actualNPCModel.NPCActions[0];
            Assert.AreEqual(typeof(Multiattack), standardAction.GetType());
            Assert.AreEqual("The aboleth makes three tentacle attacks.", (standardAction as Multiattack).ActionDescription);
        }
        [TestMethod]
        public void Test_Validate_Aboleth_StandardAction_Tentacle()
        {
            ActionModelBase standardAction = actualNPCModel.NPCActions[1];
            Assert.AreEqual(typeof(WeaponAttack), standardAction.GetType());

            WeaponAttack actualWeaponAttack = standardAction as WeaponAttack;
            Assert.AreEqual("Tentacle", actualWeaponAttack.ActionName);
            Assert.AreEqual(WeaponType.MWA, actualWeaponAttack.WeaponType);
            Assert.AreEqual(9, actualWeaponAttack.ToHit);
            Assert.AreEqual(10, actualWeaponAttack.Reach);
            Assert.AreEqual(TargetType.target, actualWeaponAttack.TargetType);

            Assert.AreEqual(2, actualWeaponAttack.PrimaryDamage.NumOfDice);
            Assert.AreEqual(DieType.D6, actualWeaponAttack.PrimaryDamage.DieType);
            Assert.AreEqual(5, actualWeaponAttack.PrimaryDamage.Bonus);
            Assert.AreEqual(DamageType.Bludgeoning, actualWeaponAttack.PrimaryDamage.DamageType);

            // Other Text fluff
            Assert.AreEqual(true, actualWeaponAttack.OtherTextCheck);
            Assert.AreEqual("If the target is a creature, it must succeed on a DC 14 Constitution saving throw or become diseased. The disease has no effect for 1 minute and can be removed by any magic that cures disease. After 1 minute, the diseased creature's skin becomes translucent and slimy, the creature can't regain hit points unless it is underwater, and the disease can be removed only by heal or another disease-curing spell of 6th level or higher. When the creature is outside a body of water, it takes 6 (1d12) acid damage every 10 minutes unless moisture is applied to the skin before 10 minutes have passed.",
                actualWeaponAttack.OtherText);
        }
        [TestMethod]
        public void Test_Validate_Aboleth_StandardAction_Tail()
        {
            ActionModelBase standardAction = actualNPCModel.NPCActions[2];
            Assert.AreEqual(typeof(WeaponAttack), standardAction.GetType());

            WeaponAttack actualWeaponAttack = standardAction as WeaponAttack;
            Assert.AreEqual(WeaponType.MWA, actualWeaponAttack.WeaponType);
            Assert.AreEqual(9, actualWeaponAttack.ToHit);
            Assert.AreEqual(10, actualWeaponAttack.Reach);
            Assert.AreEqual(TargetType.target, actualWeaponAttack.TargetType);

            Assert.AreEqual(3, actualWeaponAttack.PrimaryDamage.NumOfDice);
            Assert.AreEqual(DieType.D6, actualWeaponAttack.PrimaryDamage.DieType);
            Assert.AreEqual(5, actualWeaponAttack.PrimaryDamage.Bonus);
            Assert.AreEqual(DamageType.Bludgeoning, actualWeaponAttack.PrimaryDamage.DamageType);
        }
        [TestMethod]
        public void Test_Validate_Aboleth_StandardAction_Enslave()
        {
            ActionModelBase standardAction = actualNPCModel.NPCActions[3];
            Assert.AreEqual(typeof(OtherAction), standardAction.GetType());

            OtherAction standardOtherAction = standardAction as OtherAction;
            Assert.AreEqual("Enslave (3/Day)", standardOtherAction.ActionName);
            Assert.AreEqual(
@"The aboleth targets one creature it can see within 30 feet of it. The target must succeed on a DC 14 Wisdom saving throw or be magically charmed by the aboleth until the aboleth dies or until it is on a different plane of existence from the target. The charmed target is under the aboleth's control and can't take reactions, and the aboleth and the target can communicate telepathically with each other over any distance.

Whenever the charmed target takes damage, the target can repeat the saving throw. On a success, the effect ends. No more than once every 24 hours, the target can also repeat the saving throw when it is at least 1 mile away from the aboleth.",
                    standardOtherAction.ActionDescription);
        }
        [TestMethod]
        public void Test_Validate_Aboleth_LegendaryAction_Count()
        {
            Assert.AreEqual(4, actualNPCModel.LegendaryActions.Count);
        }
        [TestMethod]
        public void Test_Validate_Aboleth_LegendaryAction_Options()
        {
            LegendaryActionModel legendaryAction = actualNPCModel.LegendaryActions[0];
            Assert.AreEqual("Options", legendaryAction.ActionName);
            Assert.AreEqual("The aboleth can take 3 legendary actions, choosing from the options below. Only one legendary action option can be used at a time and only at the end of another creature's turn. The aboleth regains spent legendary actions at the start of its turn.", legendaryAction.ActionDescription);
        }
        [TestMethod]
        public void Test_Validate_Aboleth_LegendaryAction_Detect()
        {
            LegendaryActionModel legendaryAction = actualNPCModel.LegendaryActions[1];
            Assert.AreEqual("Detect", legendaryAction.ActionName);
            Assert.AreEqual("The aboleth makes a Wisdom (Perception) check.", legendaryAction.ActionDescription);
        }
        [TestMethod]
        public void Test_Validate_Aboleth_LegendaryAction_Tail_Swipe()
        {
            LegendaryActionModel legendaryAction = actualNPCModel.LegendaryActions[2];
            Assert.AreEqual("Tail Swipe", legendaryAction.ActionName);
            Assert.AreEqual("The aboleth makes one tail attack.", legendaryAction.ActionDescription);
        }
        [TestMethod]
        public void Test_Validate_Aboleth_LegendaryAction_Psychic_Drain()
        {
            LegendaryActionModel legendaryAction = actualNPCModel.LegendaryActions[3];
            Assert.AreEqual("Psychic Drain (Costs 2 Actions)", legendaryAction.ActionName);
            Assert.AreEqual("One creature charmed by the aboleth takes 10 (3d6) psychic damage, and the aboleth regains hit points equal to the damage the creature takes.", legendaryAction.ActionDescription);
        }
    }
}