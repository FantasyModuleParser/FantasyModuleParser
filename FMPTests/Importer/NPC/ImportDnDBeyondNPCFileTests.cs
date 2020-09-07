using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Action.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
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

            // The description for Enslave is actually on three lines instead of one.  As such, the StringReader will be employed for accuracy
            StringReader stringReader = new StringReader(standardOtherAction.ActionDescription);
            String actualDescriptionLine = "";
            int lineIndexCount = 0;
            while((actualDescriptionLine = stringReader.ReadLine())!= null){
                if (lineIndexCount == 0)
                    Assert.AreEqual("The aboleth targets one creature it can see within 30 feet of it. The target must succeed on a DC 14 Wisdom saving throw or be magically charmed by the aboleth until the aboleth dies or until it is on a different plane of existence from the target. The charmed target is under the aboleth's control and can't take reactions, and the aboleth and the target can communicate telepathically with each other over any distance.", actualDescriptionLine);
                if (lineIndexCount == 2)
                    Assert.AreEqual("Whenever the charmed target takes damage, the target can repeat the saving throw. On a success, the effect ends. No more than once every 24 hours, the target can also repeat the saving throw when it is at least 1 mile away from the aboleth.", actualDescriptionLine);

                lineIndexCount++;
            }
            Assert.AreEqual(3, lineIndexCount);
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

        #region Deva
        [TestMethod]
        public void Test_Validate_Deva_LoadFile()
        {
            actualNPCModel = LoadEngineerSuiteTestNPCFile("Deva");
        }
        [TestMethod]
        public void Test_Validate_Deva_BaseStats()
        {
            actualNPCModel = LoadEngineerSuiteTestNPCFile("Deva");
            Assert.AreEqual(18, actualNPCModel.AttributeStr);
            Assert.AreEqual(18, actualNPCModel.AttributeDex);
            Assert.AreEqual(18, actualNPCModel.AttributeCon);
            Assert.AreEqual(17, actualNPCModel.AttributeInt);
            Assert.AreEqual(20, actualNPCModel.AttributeWis);
            Assert.AreEqual(20, actualNPCModel.AttributeCha);
        }
        [TestMethod]
        public void Test_Validate_Deva_SavingThrows()
        {
            actualNPCModel = LoadEngineerSuiteTestNPCFile("Deva");
            Assert.AreEqual(9, actualNPCModel.SavingThrowWis);
            Assert.AreEqual(9, actualNPCModel.SavingThrowCha);
        }
        [TestMethod]
        public void Test_Validate_Deva_Skills()
        {
            actualNPCModel = LoadEngineerSuiteTestNPCFile("Deva");
            Assert.AreEqual(9, actualNPCModel.Insight);
            Assert.AreEqual(9, actualNPCModel.Perception);
        }
        [TestMethod]
        public void Test_Validate_Deva_DamageResistance()
        {
            actualNPCModel = LoadEngineerSuiteTestNPCFile("Deva");
            Assert.AreEqual(13, actualNPCModel.DamageResistanceModelList.Count);
            Assert.IsTrue(actualNPCModel.DamageResistanceModelList.FirstOrDefault(x => x.ActionName.Equals("Radiant")).Selected);
            Assert.IsTrue(actualNPCModel.DamageResistanceModelList.FirstOrDefault(x => x.ActionName.Equals("Bludgeoning")).Selected);
            Assert.IsTrue(actualNPCModel.DamageResistanceModelList.FirstOrDefault(x => x.ActionName.Equals("Piercing")).Selected);
            Assert.IsTrue(actualNPCModel.DamageResistanceModelList.FirstOrDefault(x => x.ActionName.Equals("Slashing")).Selected);
            Assert.IsTrue(actualNPCModel.SpecialWeaponResistanceModelList.FirstOrDefault(x => x.ActionName.Equals(WeaponResistance.Nonmagical.ToString())).Selected);
        }
        [TestMethod]
        public void Test_Validate_Deva_InnateSpellCasting()
        {
            actualNPCModel = LoadEngineerSuiteTestNPCFile("Deva");
            Assert.AreEqual(true, actualNPCModel.InnateSpellcastingSection);
            Assert.AreEqual("Charisma", actualNPCModel.InnateSpellcastingAbility);
            Assert.AreEqual(17, actualNPCModel.InnateSpellSaveDC);
            Assert.AreEqual("requiring only verbal components:", actualNPCModel.ComponentText);
            Assert.AreEqual("detect evil and good", actualNPCModel.InnateAtWill);
            Assert.AreEqual(null, actualNPCModel.FivePerDay);
            Assert.AreEqual(null, actualNPCModel.FourPerDay);
            Assert.AreEqual(null, actualNPCModel.ThreePerDay);
            Assert.AreEqual(null, actualNPCModel.TwoPerDay);
            Assert.AreEqual("commune, raise dead", actualNPCModel.OnePerDay);
        }
        #endregion
        [TestMethod]
        public void Test_Validate_Aarakocra_Actions()
        {
            actualNPCModel = LoadEngineerSuiteTestNPCFile("Aarakocra");
            Assert.AreEqual(2, actualNPCModel.NPCActions.Count);
        }
        [TestMethod]
        public void Test_Validate_Aarakocra_Talons()
        {
            actualNPCModel = LoadEngineerSuiteTestNPCFile("Aarakocra");
            ActionModelBase standardAction = actualNPCModel.NPCActions[0];
            Assert.AreEqual(typeof(WeaponAttack), standardAction.GetType());

            WeaponAttack actualWeaponAttack = standardAction as WeaponAttack;
            Assert.AreEqual("Talon", actualWeaponAttack.ActionName);
            Assert.AreEqual(WeaponType.MWA, actualWeaponAttack.WeaponType);
            Assert.AreEqual(4, actualWeaponAttack.ToHit);
            Assert.AreEqual(5, actualWeaponAttack.Reach);
            Assert.AreEqual(TargetType.target, actualWeaponAttack.TargetType);

            Assert.AreEqual(1, actualWeaponAttack.PrimaryDamage.NumOfDice);
            Assert.AreEqual(DieType.D4, actualWeaponAttack.PrimaryDamage.DieType);
            Assert.AreEqual(2, actualWeaponAttack.PrimaryDamage.Bonus);
            Assert.AreEqual(DamageType.Slashing, actualWeaponAttack.PrimaryDamage.DamageType);
        }
        [TestMethod]
        public void Test_Validate_Aarakocra_Javelin()
        {
            actualNPCModel = LoadEngineerSuiteTestNPCFile("Aarakocra");
            ActionModelBase standardAction = actualNPCModel.NPCActions[1];
            Assert.AreEqual(typeof(WeaponAttack), standardAction.GetType());

            WeaponAttack actualWeaponAttack = standardAction as WeaponAttack;
            Assert.AreEqual("Javelin", actualWeaponAttack.ActionName);
            Assert.AreEqual(WeaponType.WA, actualWeaponAttack.WeaponType);
            Assert.AreEqual(4, actualWeaponAttack.ToHit);
            Assert.AreEqual(5, actualWeaponAttack.Reach);
            Assert.AreEqual(30, actualWeaponAttack.WeaponRangeShort);
            Assert.AreEqual(120, actualWeaponAttack.WeaponRangeLong);
            Assert.AreEqual(TargetType.target, actualWeaponAttack.TargetType);

            Assert.AreEqual(1, actualWeaponAttack.PrimaryDamage.NumOfDice);
            Assert.AreEqual(DieType.D6, actualWeaponAttack.PrimaryDamage.DieType);
            Assert.AreEqual(2, actualWeaponAttack.PrimaryDamage.Bonus);
            Assert.AreEqual(DamageType.Piercing, actualWeaponAttack.PrimaryDamage.DamageType);
        }
        [TestMethod]
        public void Test_Validate_Bullywug_Spear()
        {
            actualNPCModel = LoadEngineerSuiteTestNPCFile("Bullywug");
            ActionModelBase standardAction = actualNPCModel.NPCActions[2];
            Assert.AreEqual(typeof(WeaponAttack), standardAction.GetType());

            WeaponAttack actualWeaponAttack = standardAction as WeaponAttack;
            Assert.AreEqual("Spear", actualWeaponAttack.ActionName);
            Assert.AreEqual(WeaponType.WA, actualWeaponAttack.WeaponType);
            Assert.AreEqual(3, actualWeaponAttack.ToHit);
            Assert.AreEqual(5, actualWeaponAttack.Reach);
            Assert.AreEqual(20, actualWeaponAttack.WeaponRangeShort);
            Assert.AreEqual(60, actualWeaponAttack.WeaponRangeLong);
            Assert.AreEqual(TargetType.target, actualWeaponAttack.TargetType);

            Assert.AreEqual(1, actualWeaponAttack.PrimaryDamage.NumOfDice);
            Assert.AreEqual(DieType.D6, actualWeaponAttack.PrimaryDamage.DieType);
            Assert.AreEqual(1, actualWeaponAttack.PrimaryDamage.Bonus);
            Assert.AreEqual(DamageType.Piercing, actualWeaponAttack.PrimaryDamage.DamageType);
            Assert.AreEqual(true, actualWeaponAttack.IsVersatile);
            Assert.AreEqual(false, actualWeaponAttack.OtherTextCheck);
            Assert.AreEqual(null, actualWeaponAttack.OtherText);
        }
        [TestMethod]
        public void Test_Validate_Cambion_Spear()
        {
            actualNPCModel = LoadEngineerSuiteTestNPCFile("Cambion");
            ActionModelBase standardAction = actualNPCModel.NPCActions[1];
            Assert.AreEqual(typeof(WeaponAttack), standardAction.GetType());

            WeaponAttack actualWeaponAttack = standardAction as WeaponAttack;
            Assert.AreEqual("Spear", actualWeaponAttack.ActionName);
            Assert.AreEqual(WeaponType.WA, actualWeaponAttack.WeaponType);
            Assert.AreEqual(7, actualWeaponAttack.ToHit);
            Assert.AreEqual(5, actualWeaponAttack.Reach);
            Assert.AreEqual(20, actualWeaponAttack.WeaponRangeShort);
            Assert.AreEqual(60, actualWeaponAttack.WeaponRangeLong);
            Assert.AreEqual(TargetType.target, actualWeaponAttack.TargetType);

            Assert.AreEqual(1, actualWeaponAttack.PrimaryDamage.NumOfDice);
            Assert.AreEqual(DieType.D6, actualWeaponAttack.PrimaryDamage.DieType);
            Assert.AreEqual(4, actualWeaponAttack.PrimaryDamage.Bonus);
            Assert.AreEqual(DamageType.Piercing, actualWeaponAttack.PrimaryDamage.DamageType);
            Assert.AreEqual(true, actualWeaponAttack.IsVersatile);
            Assert.AreEqual(false, actualWeaponAttack.OtherTextCheck);
            Assert.AreEqual(null, actualWeaponAttack.OtherText);
        }


        #region Guardian Naga
        [TestMethod]
        public void Test_Validate_GuardianNaga_LoadFile()
        {
            actualNPCModel = LoadEngineerSuiteTestNPCFile("GuardianNaga");
        }

        [TestMethod]
        public void Test_Validate_GuardianNaga_Spellcasting()
        {
            actualNPCModel = LoadEngineerSuiteTestNPCFile("GuardianNaga");
            Assert.AreEqual("11th", actualNPCModel.SpellcastingCasterLevel);
            Assert.AreEqual("Wisdom", actualNPCModel.SCSpellcastingAbility);
            Assert.AreEqual(16, actualNPCModel.SpellcastingSpellSaveDC);
            Assert.AreEqual(8, actualNPCModel.SpellcastingSpellHitBonus);
            Assert.AreEqual("Cleric", actualNPCModel.SpellcastingSpellClass);
            Assert.AreEqual("", actualNPCModel.FlavorText);

            Assert.AreEqual("at will", actualNPCModel.CantripSpells);
            Assert.AreEqual("mending, sacred flame, thaumaturgy", actualNPCModel.CantripSpellList);

            Assert.AreEqual("4 slots", actualNPCModel.FirstLevelSpells);
            Assert.AreEqual("command, cure wounds, shield of faith", actualNPCModel.FirstLevelSpellList);

            //Assert.AreEqual(expectedNPCModel.SecondLevelSpells, actualNPCModel.SecondLevelSpells);
            //Assert.AreEqual(expectedNPCModel.SecondLevelSpellList, actualNPCModel.SecondLevelSpellList);

            //Assert.AreEqual(expectedNPCModel.ThirdLevelSpells, actualNPCModel.ThirdLevelSpells);
            //Assert.AreEqual(expectedNPCModel.ThirdLevelSpellList, actualNPCModel.ThirdLevelSpellList);

            //Assert.AreEqual(expectedNPCModel.FourthLevelSpells, actualNPCModel.FourthLevelSpells);
            //Assert.AreEqual(expectedNPCModel.FourthLevelSpellList, actualNPCModel.FourthLevelSpellList);

            //Assert.AreEqual(expectedNPCModel.FifthLevelSpells, actualNPCModel.FifthLevelSpells);
            //Assert.AreEqual(expectedNPCModel.FifthLevelSpellList, actualNPCModel.FifthLevelSpellList);

            //Assert.AreEqual(expectedNPCModel.SixthLevelSpells, actualNPCModel.SixthLevelSpells);
            //Assert.AreEqual(expectedNPCModel.SixthLevelSpellList, actualNPCModel.SixthLevelSpellList);

            //Assert.AreEqual(expectedNPCModel.SeventhLevelSpells, actualNPCModel.SeventhLevelSpells);
            //Assert.AreEqual(expectedNPCModel.SeventhLevelSpellList, actualNPCModel.SeventhLevelSpellList);

            //Assert.AreEqual(expectedNPCModel.EighthLevelSpells, actualNPCModel.EighthLevelSpells);
            //Assert.AreEqual(expectedNPCModel.EighthLevelSpellList, actualNPCModel.EighthLevelSpellList);

            //Assert.AreEqual(expectedNPCModel.NinthLevelSpells, actualNPCModel.NinthLevelSpells);
            //Assert.AreEqual(expectedNPCModel.NinthLevelSpellList, actualNPCModel.NinthLevelSpellList);
        }

        [TestMethod]
        public void Test_Validate_GuardianNaga_StandardActions()
        {
            actualNPCModel = LoadEngineerSuiteTestNPCFile("GuardianNaga");
            Assert.AreEqual(2, actualNPCModel.NPCActions.Count);
        }
        #endregion
    }
}