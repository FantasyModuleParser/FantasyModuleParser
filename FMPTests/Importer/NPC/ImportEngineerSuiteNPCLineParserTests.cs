using FantasyModuleParser.Importer.NPC;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Action.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMPTests.Importer.NPC
{
    [TestClass()]
    public class ImportEngineerSuiteNPCLineParserTests
    {
        private ImportEngineerSuiteNPC _importEngineerSuiteNPC;
        NPCModel actualNPCModel = null;

        [TestInitialize]
        public void Initialize()
        {
            _importEngineerSuiteNPC = new ImportEngineerSuiteNPC();
            actualNPCModel = new NPCModel();
        }
        #region Size and Alignment
        [TestMethod]
        [DynamicData(nameof(SizeAndAlignmentData), DynamicDataSourceType.Method)]
        public void Test_Parse_SizeAndAlignment(NPCModel expectedNpcModel, string sizeAndAlignment)
        {
            _importEngineerSuiteNPC.ParseSizeAndAlignment(actualNPCModel, sizeAndAlignment);
            AssertSizeAndAlignment(expectedNpcModel, actualNPCModel);
        }

        private void AssertSizeAndAlignment(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            Assert.AreEqual(expectedNPCModel.Size, actualNPCModel.Size);
            Assert.AreEqual(expectedNPCModel.NPCType, actualNPCModel.NPCType);
            Assert.AreEqual(expectedNPCModel.Tag, actualNPCModel.Tag);
            Assert.AreEqual(expectedNPCModel.Alignment, actualNPCModel.Alignment);
        }

        private static IEnumerable<object[]> SizeAndAlignmentData()
        {
            yield return new object[] { generateNPCModel_SizeAndAlignment("tiny", "beast", "(devil)", "lawful neutral"), "Tiny beast (devil), lawful neutral" };
            yield return new object[] { generateNPCModel_SizeAndAlignment("medium", "humanoid", null, "lawful good"), "Medium humanoid, lawful good" };
            yield return new object[] { generateNPCModel_SizeAndAlignment("huge", "giant", null, "chaotic evil"), "Huge giant, chaotic evil" };
        }
        private static NPCModel generateNPCModel_SizeAndAlignment(string size, string type, string tag, string alignment)
        {
            NPCModel npcModel = new NPCModel();
            npcModel.Size = size;
            npcModel.NPCType = type;
            npcModel.Tag = tag;
            npcModel.Alignment = alignment;
            return npcModel;
        }
        #endregion

        #region Armor Class
        [TestMethod]
        [DynamicData(nameof(ArmorClassData), DynamicDataSourceType.Method)]
        public void Test_Parse_ArmorClass(NPCModel expectedNpcModel, string armorClass)
        {
            _importEngineerSuiteNPC.ParseArmorClass(actualNPCModel, armorClass);
            AssertArmorClass(expectedNpcModel, actualNPCModel);
        }

        private void AssertArmorClass(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            Assert.AreEqual(expectedNPCModel.AC, actualNPCModel.AC);
        }

        private static IEnumerable<object[]> ArmorClassData()
        {
            yield return new object[] { generateNPCModel_ArmorClass("16"), "Armor Class 16" };
            yield return new object[] { generateNPCModel_ArmorClass("20"), "Armor Class 20" };
            yield return new object[] { generateNPCModel_ArmorClass("12"), "Armor Class 12" };
        }
        private static NPCModel generateNPCModel_ArmorClass(string armorClass)
        {
            NPCModel npcModel = new NPCModel();
            npcModel.AC = armorClass;
            return npcModel;
        }
        #endregion

        #region Hit Points
        [TestMethod]
        [DynamicData(nameof(HitPointsData), DynamicDataSourceType.Method)]
        public void Test_Parse_HitPoints(NPCModel expectedNpcModel, string speedAttributes)
        {
            _importEngineerSuiteNPC.ParseHitPoints(actualNPCModel, speedAttributes);
            AssertHitPoints(expectedNpcModel, actualNPCModel);
        }

        private void AssertHitPoints(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            Assert.AreEqual(expectedNPCModel.HP, actualNPCModel.HP);
        }

        private static IEnumerable<object[]> HitPointsData()
        {
            yield return new object[] { generateNPCModel_HitPoints("90 (10d8 + 44)"), "Hit Points 90 (10d8 + 44)" };
            yield return new object[] { generateNPCModel_HitPoints("100 (10d12 + 50)"), "Hit Points 100 (10d12 + 50)" };
        }
        private static NPCModel generateNPCModel_HitPoints(string hitPoints)
        {
            NPCModel npcModel = new NPCModel();
            npcModel.HP = hitPoints;
            return npcModel;
        }
        #endregion

        #region Speed Attributes
        [TestMethod]
        [DynamicData(nameof(SpeedAttributeData), DynamicDataSourceType.Method)]
        public void Test_Parse_SpeedAttributes(NPCModel expectedNpcModel, string speedAttributes)
        {
            _importEngineerSuiteNPC.ParseSpeedAttributes(actualNPCModel, speedAttributes);
            AssertSpeedAttributes(expectedNpcModel, actualNPCModel);
        }

        private void AssertSpeedAttributes(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            Assert.AreEqual(expectedNPCModel.Speed, actualNPCModel.Speed);
            Assert.AreEqual(expectedNPCModel.Burrow, actualNPCModel.Burrow);
            Assert.AreEqual(expectedNPCModel.Climb, actualNPCModel.Climb);
            Assert.AreEqual(expectedNPCModel.Fly, actualNPCModel.Fly);
            Assert.AreEqual(expectedNPCModel.Hover, actualNPCModel.Hover);
            Assert.AreEqual(expectedNPCModel.Swim, actualNPCModel.Swim);
        }

        private static IEnumerable<object[]> SpeedAttributeData()
        {
            yield return new object[] { generateNPCModel_SpeedAttributeData(0, 0, 0, 0, false, 0), "" };
            yield return new object[] { generateNPCModel_SpeedAttributeData(0, 0, 0, 0, false, 0), "Speed 0 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(30, 0, 0, 0, false, 0), "Speed 30 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(60, 0, 0, 0, false, 0), "Speed 60 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(0, 30, 0, 0, false, 0), "Speed 0 ft., burrow 30 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(0, 0, 30, 0, false, 0), "Speed 0 ft., climb 30 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(0, 0, 0, 30, false, 0), "Speed 0 ft., fly 30 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(0, 0, 0, 30, true, 0), "Speed 0 ft., fly 30 ft. (hover)"};
            yield return new object[] { generateNPCModel_SpeedAttributeData(0, 0, 0, 0, false, 30), "Speed 0 ft., swim 30 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(30, 30, 0, 0, false, 0), "Speed 30 ft., burrow 30 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(30, 0, 30, 0, false, 0), "Speed 30 ft., climb 30 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(30, 0, 0, 30, false, 0), "Speed 30 ft., fly 30 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(30, 0, 0, 30, true, 0), "Speed 30 ft., fly 30 ft. (hover)"};
            yield return new object[] { generateNPCModel_SpeedAttributeData(30, 0, 0, 0, false, 30), "Speed 30 ft., swim 30 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(30, 30, 30, 0, false, 0), "Speed 30 ft., climb 30 ft., burrow 30 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(30, 30, 0, 30, false, 0), "Speed 30 ft., fly 30 ft., burrow 30 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(30, 30, 0, 30, true, 0), "Speed 30 ft., fly 30 ft. (hover), burrow 30 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(30, 30, 0, 0, false, 30), "Speed 30 ft., burrow 30 ft., swim 30 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(30, 30, 30, 30, false, 0), "Speed 30 ft., climb 30 ft., fly 30 ft., burrow 30 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(30, 30, 30, 30, true, 0), "Speed 30 ft., climb 30 ft., fly 30 ft. (hover), burrow 30 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(30, 30, 30, 0, false, 30), "Speed 30 ft., climb 30 ft., burrow 30 ft., swim 30 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(30, 30, 30, 30, false, 30), "Speed 30 ft., climb 30 ft., fly 30 ft., burrow 30 ft., swim 30 ft."};
            yield return new object[] { generateNPCModel_SpeedAttributeData(30, 30, 30, 30, true, 30), "Speed 30 ft., climb 30 ft., fly 30 ft. (hover), burrow 30 ft., swim 30 ft."};
        }
        private static NPCModel generateNPCModel_SpeedAttributeData(int speed, int burrow, int climb, int fly, bool hover, int swim)
        {
            NPCModel npcModel = new NPCModel();
            npcModel.Speed = speed;
            npcModel.Burrow = burrow;
            npcModel.Climb = climb;
            npcModel.Fly = fly;
            npcModel.Hover = hover;
            npcModel.Swim = swim;
            return npcModel;
        }
        #endregion

        #region Stat Attributes
        [TestMethod]
        [DynamicData(nameof(StatAttributeData), DynamicDataSourceType.Method)]
        public void Test_Parse_StatAttributes(NPCModel expectedNpcModel, string statAttributes)
        {
            _importEngineerSuiteNPC.ParseStatAttributes(actualNPCModel, statAttributes);
            AssertStatAttributes(expectedNpcModel, actualNPCModel);
        }

        private void AssertStatAttributes(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            Assert.AreEqual(expectedNPCModel.AttributeStr, actualNPCModel.AttributeStr);
            Assert.AreEqual(expectedNPCModel.AttributeDex, actualNPCModel.AttributeDex);
            Assert.AreEqual(expectedNPCModel.AttributeCon, actualNPCModel.AttributeCon);
            Assert.AreEqual(expectedNPCModel.AttributeInt, actualNPCModel.AttributeInt);
            Assert.AreEqual(expectedNPCModel.AttributeWis, actualNPCModel.AttributeWis);
            Assert.AreEqual(expectedNPCModel.AttributeCha, actualNPCModel.AttributeCha);
        }

        private static IEnumerable<object[]> StatAttributeData()
        {
            yield return new object[] { generateNPCModel_StatAttributes(10,11,12,13,14,15), "STR DEX CON INT WIS CHA 10 (+0) 11 (+0) 12 (+1) 13 (+1) 14 (+2) 15 (+2)" };
            yield return new object[] { generateNPCModel_StatAttributes(20,19,16,14,12,8), "STR DEX CON INT WIS CHA 20 (+5) 19 (+4) 16 (+3) 14 (+2) 12 (+1) 8 (-1)" };
        }
        private static NPCModel generateNPCModel_StatAttributes(int str, int dex, int con, int intel, int wis, int cha)
        {
            NPCModel npcModel = new NPCModel();
            npcModel.AttributeStr = str;
            npcModel.AttributeDex = dex;
            npcModel.AttributeCon = con;
            npcModel.AttributeInt = intel;
            npcModel.AttributeWis = wis;
            npcModel.AttributeCha = cha;
            return npcModel;
        }
        #endregion

        #region Saving Throws
        [TestMethod]
        [DynamicData(nameof(SavingThrowData), DynamicDataSourceType.Method)]
        public void Test_Parse_SavingThrows(NPCModel expectedNpcModel, string savingThrows)
        {
            _importEngineerSuiteNPC.ParseSavingThrows(actualNPCModel, savingThrows);
            AssertSavingThrows(expectedNpcModel, actualNPCModel);
        }

        private void AssertSavingThrows(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            Assert.AreEqual(expectedNPCModel.SavingThrowStr, actualNPCModel.SavingThrowStr);
            Assert.AreEqual(expectedNPCModel.SavingThrowDex, actualNPCModel.SavingThrowDex);
            Assert.AreEqual(expectedNPCModel.SavingThrowCon, actualNPCModel.SavingThrowCon);
            Assert.AreEqual(expectedNPCModel.SavingThrowInt, actualNPCModel.SavingThrowInt);
            Assert.AreEqual(expectedNPCModel.SavingThrowWis, actualNPCModel.SavingThrowWis);
            Assert.AreEqual(expectedNPCModel.SavingThrowCha, actualNPCModel.SavingThrowCha);

            Assert.AreEqual(expectedNPCModel.SavingThrowStrBool, actualNPCModel.SavingThrowStrBool);
            Assert.AreEqual(expectedNPCModel.SavingThrowDexBool, actualNPCModel.SavingThrowDexBool);
            Assert.AreEqual(expectedNPCModel.SavingThrowConBool, actualNPCModel.SavingThrowConBool);
            Assert.AreEqual(expectedNPCModel.SavingThrowIntBool, actualNPCModel.SavingThrowIntBool);
            Assert.AreEqual(expectedNPCModel.SavingThrowWisBool, actualNPCModel.SavingThrowWisBool);
            Assert.AreEqual(expectedNPCModel.SavingThrowChaBool, actualNPCModel.SavingThrowChaBool);
        }

        private static IEnumerable<object[]> SavingThrowData()
        {
            yield return new object[] { generateNPCModel_SavingThrows(1, false, 2, false, 3, false, 0, true, 5, false, 6, false), "Saving Throws Str +1, Dex +2, Con +3, Int +0, Wis +5, Cha +6" };
            yield return new object[] { generateNPCModel_SavingThrows(1, false, 0, false, 0, false, 0, false, 0, false, 0, false), "Saving Throws Str +1" };
            yield return new object[] { generateNPCModel_SavingThrows(0, false, 2, false, 0, false, 0, false, 0, false, 0, false), "Saving Throws Dex +2" };
            yield return new object[] { generateNPCModel_SavingThrows(0, false, 0, false, 3, false, 0, false, 0, false, 0, false), "Saving Throws Con +3" };
            yield return new object[] { generateNPCModel_SavingThrows(0, false, 0, false, 0, false, 4, false, 0, false, 0, false), "Saving Throws Int +4" };
            yield return new object[] { generateNPCModel_SavingThrows(0, false, 0, false, 0, false, 0, false, 5, false, 0, false), "Saving Throws Wis +5" };
            yield return new object[] { generateNPCModel_SavingThrows(0, false, 0, false, 0, false, 0, false, 0, false, 6, false), "Saving Throws Cha +6" };
            yield return new object[] { generateNPCModel_SavingThrows(-1, false, 0, false, 0, false, 0, false, 0, false, 0, false), "Saving Throws Str -1" };
            yield return new object[] { generateNPCModel_SavingThrows(0, false, -2, false, 0, false, 0, false, 0, false, 0, false), "Saving Throws Dex -2" };
            yield return new object[] { generateNPCModel_SavingThrows(0, false, 0, false, -3, false, 0, false, 0, false, 0, false), "Saving Throws Con -3" };
            yield return new object[] { generateNPCModel_SavingThrows(0, false, 0, false, 0, false, -4, false, 0, false, 0, false), "Saving Throws Int -4" };
            yield return new object[] { generateNPCModel_SavingThrows(0, false, 0, false, 0, false, 0, false, -5, false, 0, false), "Saving Throws Wis -5" };
            yield return new object[] { generateNPCModel_SavingThrows(0, false, 0, false, 0, false, 0, false, 0, false, -6, false), "Saving Throws Cha -6" };
            yield return new object[] { generateNPCModel_SavingThrows(1, false, 2, false, 0, false, 0, false, 0, false, 0, false), "Saving Throws Str +1, Dex +2" };
            yield return new object[] { generateNPCModel_SavingThrows(1, false, 0, false, 2, false, 0, false, 0, false, 0, false), "Saving Throws Str +1, Con +2" };
            yield return new object[] { generateNPCModel_SavingThrows(1, false, 0, false, 0, false, 3, false, 0, false, 0, false), "Saving Throws Str +1, Int +3" };
            yield return new object[] { generateNPCModel_SavingThrows(1, false, 0, false, 0, false, 0, false, 4, false, 0, false), "Saving Throws Str +1, Wis +4" };
            yield return new object[] { generateNPCModel_SavingThrows(1, false, 0, false, 0, false, 0, false, 0, false, 6, false), "Saving Throws Str +1, Cha +6" };
            yield return new object[] { generateNPCModel_SavingThrows(0, true, 0, false, 0, false, 0, false, 0, false, 0, false), "Saving Throws Str +0" };
            yield return new object[] { generateNPCModel_SavingThrows(0, false, 0, true, 0, false, 0, false, 0, false, 0, false), "Saving Throws Dex +0" };
            yield return new object[] { generateNPCModel_SavingThrows(0, false, 0, false, 0, true, 0, false, 0, false, 0, false), "Saving Throws Con +0" };
            yield return new object[] { generateNPCModel_SavingThrows(0, false, 0, false, 0, false, 0, true, 0, false, 0, false), "Saving Throws Int +0" };
            yield return new object[] { generateNPCModel_SavingThrows(0, false, 0, false, 0, false, 0, false, 0, true, 0, false), "Saving Throws Wis +0" };
            yield return new object[] { generateNPCModel_SavingThrows(0, false, 0, false, 0, false, 0, false, 0, false, 0, true), "Saving Throws Cha +0" };
            yield return new object[] { generateNPCModel_SavingThrows(0, true, 0, true, 0, true, 0, true, 0, true, 0, true), "Saving Throws Str +0, Dex +0, Con +0, Int +0, Wis +0, Cha +0" };
        }
        private static NPCModel generateNPCModel_SavingThrows(
            int str, bool strAsZero, 
            int dex, bool dexAsZero, 
            int con, bool conAsZero, 
            int intel, bool intelAsZero,
            int wis, bool wisAsZero,
            int cha, bool chaAsZero)
        {
            NPCModel npcModel = new NPCModel();
            npcModel.SavingThrowStr = str;
            npcModel.SavingThrowStrBool = strAsZero;
            npcModel.SavingThrowDex = dex;
            npcModel.SavingThrowDexBool = dexAsZero;
            npcModel.SavingThrowCon = con;
            npcModel.SavingThrowConBool = conAsZero;
            npcModel.SavingThrowInt = intel;
            npcModel.SavingThrowIntBool = intelAsZero;
            npcModel.SavingThrowWis = wis;
            npcModel.SavingThrowWisBool = wisAsZero;
            npcModel.SavingThrowCha = cha;
            npcModel.SavingThrowChaBool = chaAsZero;
            return npcModel;
        }
        #endregion

        #region Skill Attributes
        [TestMethod]
        [DynamicData(nameof(SkillAttributesData), DynamicDataSourceType.Method)]
        public void Test_Parse_SkillAttributes(NPCModel expectedNpcModel, string skillAttributes)
        {
            _importEngineerSuiteNPC.ParseSkillAttributes(actualNPCModel, skillAttributes);
            AssertSkills(expectedNpcModel, actualNPCModel);
        }

        private void AssertSkills(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            Assert.AreEqual(expectedNPCModel.Acrobatics, actualNPCModel.Acrobatics);
            Assert.AreEqual(expectedNPCModel.AnimalHandling, actualNPCModel.AnimalHandling);
            Assert.AreEqual(expectedNPCModel.Arcana, actualNPCModel.Arcana);
            Assert.AreEqual(expectedNPCModel.Athletics, actualNPCModel.Athletics);
            Assert.AreEqual(expectedNPCModel.Deception, actualNPCModel.Deception);
            Assert.AreEqual(expectedNPCModel.History, actualNPCModel.History);
            Assert.AreEqual(expectedNPCModel.Insight, actualNPCModel.Insight);
            Assert.AreEqual(expectedNPCModel.Intimidation, actualNPCModel.Intimidation);
            Assert.AreEqual(expectedNPCModel.Investigation, actualNPCModel.Investigation);
            Assert.AreEqual(expectedNPCModel.Medicine, actualNPCModel.Medicine);
            Assert.AreEqual(expectedNPCModel.Nature, actualNPCModel.Nature);
            Assert.AreEqual(expectedNPCModel.Perception, actualNPCModel.Perception);
            Assert.AreEqual(expectedNPCModel.Performance, actualNPCModel.Performance);
            Assert.AreEqual(expectedNPCModel.Persuasion, actualNPCModel.Persuasion);
            Assert.AreEqual(expectedNPCModel.Religion, actualNPCModel.Religion);
            Assert.AreEqual(expectedNPCModel.SleightOfHand, actualNPCModel.SleightOfHand);
            Assert.AreEqual(expectedNPCModel.Stealth, actualNPCModel.Stealth);
            Assert.AreEqual(expectedNPCModel.Survival, actualNPCModel.Survival);
        }

        private static IEnumerable<object[]> SkillAttributesData()
        {
            yield return new object[] { generateNPCModel_SkillAttributes(1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18), "Skills Acrobatics +1, Animal Handling +2, Arcana +3, Athletics +4, Deception +5, History +6, Insight +7, Intimidation +8, Investigation +9, Medicine +10, Nature +11, Perception +12, Performance +13, Persuasion +14, Religion +15, Sleight of Hand +16, Stealth +17, Survival +18" };
        }
        private static NPCModel generateNPCModel_SkillAttributes(
            int Acrobatics,
            int AnimalHandling,
            int Arcana,
            int Athletics,
            int Deception,
            int History,
            int Insight,
            int Intimidation,
            int Investigation,
            int Medicine,
            int Nature,
            int Perception,
            int Performance,
            int Persuasion,
            int Religion,
            int SleightOfHand,
            int Stealth,
            int Survival)
        {
            NPCModel npcModel = new NPCModel();
            npcModel.Acrobatics = Acrobatics;
            npcModel.AnimalHandling = AnimalHandling;
            npcModel.Arcana = Arcana;
            npcModel.Athletics = Athletics;
            npcModel.Deception = Deception;
            npcModel.History = History;
            npcModel.Insight = Insight;
            npcModel.Intimidation = Intimidation;
            npcModel.Investigation = Investigation;
            npcModel.Medicine = Medicine;
            npcModel.Nature = Nature;
            npcModel.Perception = Perception;
            npcModel.Performance = Performance;
            npcModel.Persuasion = Persuasion;
            npcModel.Religion = Religion;
            npcModel.SleightOfHand = SleightOfHand;
            npcModel.Stealth = Stealth;
            npcModel.Survival = Survival;
            return npcModel;
        }
        #endregion

        #region Damage Vulnerabilities
        [TestMethod]
        [DynamicData(nameof(DamageVulnerabilitesData), DynamicDataSourceType.Method)]
        public void Test_Parse_DamageVulnerabilites(NPCModel expectedNpcModel, string damageVulnerabilites)
        {
            _importEngineerSuiteNPC.ParseDamageVulnerabilities(actualNPCModel, damageVulnerabilites);
            AssertDamageVulnerabilites(expectedNpcModel, actualNPCModel);
        }

        private void AssertDamageVulnerabilites(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            Assert.AreEqual(expectedNPCModel.Acrobatics, actualNPCModel.Acrobatics);

            foreach(SelectableActionModel expectedDamageVulnerability in expectedNPCModel.DamageVulnerabilityModelList)
            {
                SelectableActionModel actualDamageVulnerability = actualNPCModel.DamageVulnerabilityModelList.First(item => item.ActionName.Equals(expectedDamageVulnerability.ActionName));
                Assert.IsNotNull(actualDamageVulnerability);
                Assert.AreEqual(expectedDamageVulnerability.Selected, actualDamageVulnerability.Selected);
            }
        }

        private static IEnumerable<object[]> DamageVulnerabilitesData()
        {
            yield return new object[] { generateNPCModel_DamageVulnerabilites(
                new DamageType[] { DamageType.Acid, DamageType.Fire, DamageType.Lightning, DamageType.Poison, 
                    DamageType.Radiant, DamageType.Bludgeoning, DamageType.Slashing }),
                "Damage Vulnerabilities acid, fire, lightning, poison, radiant; bludgeoning and slashing" };
        }
        private static NPCModel generateNPCModel_DamageVulnerabilites(DamageType[] damageVulnerabilities)
        {
            NPCModel npcModel = new NPCModel();
            NPCController npcController = new NPCController();
            npcModel.DamageVulnerabilityModelList = npcController.GetSelectableActionModelList(typeof(DamageType));

            foreach(DamageType damageType in damageVulnerabilities)
            {
                string damageTypeName = damageType.ToString();
                npcModel.DamageVulnerabilityModelList.First(item => item.ActionName.Equals(damageType.ToString())).Selected = true;
            }

            return npcModel;
        }
        #endregion

        #region Damage Resistances
        [TestMethod]
        [DynamicData(nameof(DamageResistancesData), DynamicDataSourceType.Method)]
        public void Test_Parse_DamageResistances(NPCModel expectedNpcModel, string damageResistances)
        {
            _importEngineerSuiteNPC.ParseDamageResistances(actualNPCModel, damageResistances);
            AssertDamageResistances(expectedNpcModel, actualNPCModel);
        }

        private void AssertDamageResistances(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            foreach (SelectableActionModel expectedDamageImmunity in expectedNPCModel.DamageResistanceModelList)
            {
                SelectableActionModel actualDamageImmunity = actualNPCModel.DamageResistanceModelList.First
                    (item => item.ActionName.Equals(expectedDamageImmunity.ActionName));

                Assert.IsNotNull(actualDamageImmunity);
                Assert.AreEqual(expectedDamageImmunity.Selected, actualDamageImmunity.Selected);
            }
        }

        private static IEnumerable<object[]> DamageResistancesData()
        {
            yield return new object[] { generateNPCModel_DamageResistances(
                new DamageType[] { DamageType.Cold, DamageType.Force, DamageType.Necrotic,
                    DamageType.Psychic, DamageType.Thunder }, WeaponResistance.Nonmagical),
                "Damage Resistances cold, force, necrotic, psychic, thunder from nonmagical weapons" };
        }
        private static NPCModel generateNPCModel_DamageResistances(DamageType[] damageVulnerabilities, WeaponResistance? weaponResistance)
        {
            NPCModel npcModel = new NPCModel();
            NPCController npcController = new NPCController();
            npcModel.DamageResistanceModelList = npcController.GetSelectableActionModelList(typeof(DamageType));
            npcModel.SpecialWeaponDmgResistanceModelList = npcController.GetSelectableActionModelList(typeof(WeaponResistance));
            foreach (DamageType damageType in damageVulnerabilities)
            {
                string damageTypeName = damageType.ToString();
                npcModel.DamageResistanceModelList.First(item => item.ActionName.Equals(damageType.ToString())).Selected = true;
            }

            if (weaponResistance != null)
            {
                npcModel.SpecialWeaponDmgResistanceModelList.First(item => item.ActionName.Equals(weaponResistance.ToString())).Selected = true;
            }

            return npcModel;
        }
        #endregion

        #region Damage Immunities
        [TestMethod]
        [DynamicData(nameof(DamageImmunitiesData), DynamicDataSourceType.Method)]
        public void Test_Parse_DamageImmunities(NPCModel expectedNpcModel, string damageImmunities)
        {
            _importEngineerSuiteNPC.ParseDamageImmunities(actualNPCModel, damageImmunities);
            AssertDamageImmunities(expectedNpcModel, actualNPCModel);
        }

        private void AssertDamageImmunities(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            foreach (SelectableActionModel expectedDamageImmunity in expectedNPCModel.DamageImmunityModelList)
            {
                SelectableActionModel actualDamageImmunity = actualNPCModel.DamageImmunityModelList.First
                    (item => item.ActionName.Equals(expectedDamageImmunity.ActionName));

                Assert.IsNotNull(actualDamageImmunity);
                Assert.AreEqual(expectedDamageImmunity.Selected, actualDamageImmunity.Selected);
            }
        }

        private static IEnumerable<object[]> DamageImmunitiesData()
        {
            yield return new object[] { generateNPCModel_DamageImmunities(
                new DamageType[] { DamageType.Acid, DamageType.Force, DamageType.Poison,
                    DamageType.Thunder, DamageType.Slashing }, WeaponImmunity.NonmagicalSilvered),
                "Damage Immunities acid, force, poison, thunder; slashing from nonmagical weapons that aren't silvered" };
        }
        private static NPCModel generateNPCModel_DamageImmunities(DamageType[] damageImmunities, WeaponImmunity? weaponImmunity)
        {
            NPCModel npcModel = new NPCModel();
            NPCController npcController = new NPCController();
            npcModel.DamageImmunityModelList = npcController.GetSelectableActionModelList(typeof(DamageType));
            npcModel.SpecialWeaponDmgImmunityModelList = npcController.GetSelectableActionModelList(typeof(WeaponImmunity));
            foreach (DamageType damageType in damageImmunities)
            {
                string damageTypeName = damageType.ToString();
                npcModel.DamageImmunityModelList.First(item => item.ActionName.Equals(damageType.ToString())).Selected = true;
            }

            if(weaponImmunity != null)
            {
                npcModel.SpecialWeaponDmgImmunityModelList.First(item => item.ActionName.Equals(weaponImmunity.ToString())).Selected = true;
            }

            return npcModel;
        }
        #endregion

        #region Condition Immunities
        [TestMethod]
        [DynamicData(nameof(ConditionImmunitiesData), DynamicDataSourceType.Method)]
        public void Test_Parse_ConditionImmunities(NPCModel expectedNpcModel, string conditionImmunities)
        {
            _importEngineerSuiteNPC.ParseConditionImmunities(actualNPCModel, conditionImmunities);
            AssertConditionImmunities(expectedNpcModel, actualNPCModel);
        }

        private void AssertConditionImmunities(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            foreach (SelectableActionModel expectedConditionImmunity in expectedNPCModel.ConditionImmunityModelList)
            {
                SelectableActionModel actualConditionImmunity = actualNPCModel.ConditionImmunityModelList.First
                    (item => item.ActionName.Equals(expectedConditionImmunity.ActionName));

                Assert.IsNotNull(actualConditionImmunity);
                Assert.AreEqual(expectedConditionImmunity.Selected, actualConditionImmunity.Selected);
            }
        }

        private static IEnumerable<object[]> ConditionImmunitiesData()
        {
            yield return new object[] { generateNPCModel_ConditionImmunities(
                new ConditionType[] { ConditionType.Charmed, ConditionType.Frightened, ConditionType.Invisible,
                                    ConditionType.Paralyzed, ConditionType.Prone, ConditionType.Restrained}),
                "Condition Immunities blinded, frightened, invisible, paralyzed, prone, restrained" };
        }
        private static NPCModel generateNPCModel_ConditionImmunities(ConditionType[] conditionImmunities)
        {
            NPCModel npcModel = new NPCModel();
            NPCController npcController = new NPCController();
            npcModel.ConditionImmunityModelList = npcController.GetSelectableActionModelList(typeof(ConditionType));
            foreach (ConditionType damageType in conditionImmunities)
            {
                npcModel.ConditionImmunityModelList.First(item => item.ActionName.Equals(damageType.ToString())).Selected = true;
            }

            return npcModel;
        }
        #endregion
    }
}
