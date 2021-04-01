using FantasyModuleParser.Importer.NPC;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Action.Enums;
using FantasyModuleParser.NPC.Models.Skills;
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
            yield return new object[] { generateNPCModel_SizeAndAlignment("Tiny", "beast", "(devil)", "lawful neutral"), "Tiny beast (devil), lawful neutral" };
            yield return new object[] { generateNPCModel_SizeAndAlignment("Medium", "humanoid", null, "lawful good"), "Medium humanoid, lawful good" };
            yield return new object[] { generateNPCModel_SizeAndAlignment("Huge", "giant", null, "chaotic evil"), "Huge giant, chaotic evil" };
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
            yield return new object[] { generateNPCModel_StatAttributes(20, 19, 16, 14, 12, 8), "STR DEX CON INT WIS CHA 20 (+5) 19 (+4) 16 (+3) 14 (+2) 12 (+1) 8 (-1)" };
            yield return new object[] { generateNPCModel_StatAttributes(20, 19, 16, 14, 12, 8), "STR DEX CON INT WIS CHA 20(+5) 19 (+4) 16(+3) 14  (+2) 12   (+1) 8(-1)" };
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
        private const string DAMAGE_VULNERABILITY_PREFIX = "Damage Vulnerabilities";
        private static IEnumerable<object[]> DamageVulnerabilitesData()
        {
            yield return new object[] { generateNPCModel_DamageVulnerabilites(new DamageType[] {}), "" };
            yield return new object[] { generateNPCModel_DamageVulnerabilites( new DamageType[] { DamageType.Acid}), DAMAGE_VULNERABILITY_PREFIX + " acid" };
            yield return new object[] { generateNPCModel_DamageVulnerabilites(new DamageType[] { DamageType.Bludgeoning }), DAMAGE_VULNERABILITY_PREFIX + " bludgeoning" };
            yield return new object[] { generateNPCModel_DamageVulnerabilites(new DamageType[] { DamageType.Cold }), DAMAGE_VULNERABILITY_PREFIX + " cold" };
            yield return new object[] { generateNPCModel_DamageVulnerabilites(new DamageType[] { DamageType.Fire }), DAMAGE_VULNERABILITY_PREFIX + " fire" };
            yield return new object[] { generateNPCModel_DamageVulnerabilites(new DamageType[] { DamageType.Force }), DAMAGE_VULNERABILITY_PREFIX + " force" };
            yield return new object[] { generateNPCModel_DamageVulnerabilites(new DamageType[] { DamageType.Lightning }), DAMAGE_VULNERABILITY_PREFIX + " lightning" };
            yield return new object[] { generateNPCModel_DamageVulnerabilites(new DamageType[] { DamageType.Necrotic }), DAMAGE_VULNERABILITY_PREFIX + " necrotic" };
            yield return new object[] { generateNPCModel_DamageVulnerabilites(new DamageType[] { DamageType.Piercing }), DAMAGE_VULNERABILITY_PREFIX + " piercing" };
            yield return new object[] { generateNPCModel_DamageVulnerabilites(new DamageType[] { DamageType.Poison }), DAMAGE_VULNERABILITY_PREFIX + " poison" };
            yield return new object[] { generateNPCModel_DamageVulnerabilites(new DamageType[] { DamageType.Psychic }), DAMAGE_VULNERABILITY_PREFIX + " psychic" };
            yield return new object[] { generateNPCModel_DamageVulnerabilites(new DamageType[] { DamageType.Radiant }), DAMAGE_VULNERABILITY_PREFIX + " radiant" };
            yield return new object[] { generateNPCModel_DamageVulnerabilites(new DamageType[] { DamageType.Slashing }), DAMAGE_VULNERABILITY_PREFIX + " slashing" };
            yield return new object[] { generateNPCModel_DamageVulnerabilites(new DamageType[] { DamageType.Thunder }), DAMAGE_VULNERABILITY_PREFIX + " thunder" };
            yield return new object[] { generateNPCModel_DamageVulnerabilites(
                new DamageType[] { DamageType.Acid, DamageType.Fire, DamageType.Lightning, DamageType.Poison, 
                    DamageType.Radiant, DamageType.Bludgeoning, DamageType.Slashing }),
                DAMAGE_VULNERABILITY_PREFIX + " acid, fire, lightning, poison, radiant; bludgeoning and slashing" };
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

        private const string CONDITION_IMMUNITY_PREFIX = "Condition Immunities";
        private static IEnumerable<object[]> ConditionImmunitiesData()
        {
            yield return new object[] { generateNPCModel_ConditionImmunities(new ConditionType[] { ConditionType.Blinded}), CONDITION_IMMUNITY_PREFIX + " blinded" };
            yield return new object[] { generateNPCModel_ConditionImmunities(new ConditionType[] { ConditionType.Charmed}), CONDITION_IMMUNITY_PREFIX + " charmed" };
            yield return new object[] { generateNPCModel_ConditionImmunities(new ConditionType[] { ConditionType.Deafened }), CONDITION_IMMUNITY_PREFIX + " deafened" };
            yield return new object[] { generateNPCModel_ConditionImmunities(new ConditionType[] { ConditionType.Exhaustion }), CONDITION_IMMUNITY_PREFIX + " exhaustion" };
            yield return new object[] { generateNPCModel_ConditionImmunities(new ConditionType[] { ConditionType.Frightened }), CONDITION_IMMUNITY_PREFIX + " frightened" };
            yield return new object[] { generateNPCModel_ConditionImmunities(new ConditionType[] { ConditionType.Grappled }), CONDITION_IMMUNITY_PREFIX + " grappled" };
            yield return new object[] { generateNPCModel_ConditionImmunities(new ConditionType[] { ConditionType.Incapacitated }), CONDITION_IMMUNITY_PREFIX + " incapacitated" };
            yield return new object[] { generateNPCModel_ConditionImmunities(new ConditionType[] { ConditionType.Invisible }), CONDITION_IMMUNITY_PREFIX + " invisible" };
            yield return new object[] { generateNPCModel_ConditionImmunities(new ConditionType[] { ConditionType.Paralyzed }), CONDITION_IMMUNITY_PREFIX + " paralyzed" };
            yield return new object[] { generateNPCModel_ConditionImmunities(new ConditionType[] { ConditionType.Petrified }), CONDITION_IMMUNITY_PREFIX + " petrified" };
            yield return new object[] { generateNPCModel_ConditionImmunities(new ConditionType[] { ConditionType.Poisoned }), CONDITION_IMMUNITY_PREFIX + " poisoned" };
            yield return new object[] { generateNPCModel_ConditionImmunities(new ConditionType[] { ConditionType.Prone }), CONDITION_IMMUNITY_PREFIX + " prone" };
            yield return new object[] { generateNPCModel_ConditionImmunities(new ConditionType[] { ConditionType.Restrained }), CONDITION_IMMUNITY_PREFIX + " restrained" };
            yield return new object[] { generateNPCModel_ConditionImmunities(new ConditionType[] { ConditionType.Stunned }), CONDITION_IMMUNITY_PREFIX + " stunned" };
            yield return new object[] { generateNPCModel_ConditionImmunities(new ConditionType[] { ConditionType.Unconscious }), CONDITION_IMMUNITY_PREFIX + " unconscious" };

            yield return new object[] { generateNPCModel_ConditionImmunities(
                new ConditionType[] { ConditionType.Blinded, ConditionType.Frightened, ConditionType.Invisible,
                                    ConditionType.Paralyzed, ConditionType.Prone, ConditionType.Restrained}),
                CONDITION_IMMUNITY_PREFIX + " blinded, frightened, invisible, paralyzed, prone, restrained" };
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

        #region Senses
        [TestMethod]
        [DynamicData(nameof(VisionAttributesData), DynamicDataSourceType.Method)]
        public void Test_Parse_Senses(NPCModel expectedNpcModel, string text)
        {
            _importEngineerSuiteNPC.ParseVisionAttributes(actualNPCModel, text);
            AssertVisionAttributes(expectedNpcModel, actualNPCModel);
        }

        private void AssertVisionAttributes(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            Assert.AreEqual(expectedNPCModel.Blindsight, actualNPCModel.Blindsight);
            Assert.AreEqual(expectedNPCModel.BlindBeyond, actualNPCModel.BlindBeyond);
            Assert.AreEqual(expectedNPCModel.Darkvision, actualNPCModel.Darkvision);
            Assert.AreEqual(expectedNPCModel.Tremorsense, actualNPCModel.Tremorsense);
            Assert.AreEqual(expectedNPCModel.Truesight, actualNPCModel.Truesight);
            Assert.AreEqual(expectedNPCModel.PassivePerception, actualNPCModel.PassivePerception);
        }

        private static IEnumerable<object[]> VisionAttributesData()
        {
            yield return new object[] { generateNPCModel_VisionAttributes(60, true, 70,80,90,22),
                "Senses blindsight 60 ft. (blind beyond this radius), darkvision 70 ft., tremorsense 80 ft., truesight 90 ft., passive Perception 22" };
            yield return new object[] { generateNPCModel_VisionAttributes(10, false, 30,50,70,12),
                "Senses blindsight 10 ft., darkvision 30 ft., tremorsense 50 ft., truesight 70 ft., passive Perception 12" };
        }
        private static NPCModel generateNPCModel_VisionAttributes(int blindSight, bool blindBeyond, 
            int darkVision, int tremorSense, int trueSight, int passivePerception)
        {
            NPCModel npcModel = new NPCModel();

            npcModel.Blindsight = blindSight;
            npcModel.BlindBeyond = blindBeyond;
            npcModel.Darkvision = darkVision;
            npcModel.Tremorsense = tremorSense;
            npcModel.Truesight = trueSight;
            npcModel.PassivePerception = passivePerception;

            return npcModel;
        }
        #endregion

        #region Languages
        [TestMethod]
        [DynamicData(nameof(LanguagesData), DynamicDataSourceType.Method)]
        public void Test_Parse_Languages(NPCModel expectedNpcModel, string text)
        {
            _importEngineerSuiteNPC.ParseLanguages(actualNPCModel, text);
            AssertLanguages(expectedNpcModel, actualNPCModel);
        }

        private void AssertLanguages(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            foreach (LanguageModel expectedLang in expectedNPCModel.StandardLanguages)
                Assert.AreEqual(expectedLang.Selected, 
                    actualNPCModel.StandardLanguages.First(item => item.Language.Equals(expectedLang.Language)).Selected);

            foreach (LanguageModel expectedLang in expectedNPCModel.ExoticLanguages)
                Assert.AreEqual(expectedLang.Selected,
                    actualNPCModel.ExoticLanguages.First(item => item.Language.Equals(expectedLang.Language)).Selected);

            foreach (LanguageModel expectedLang in expectedNPCModel.MonstrousLanguages)
                Assert.AreEqual(expectedLang.Selected,
                    actualNPCModel.MonstrousLanguages.First(item => item.Language.Equals(expectedLang.Language)).Selected);
        }

        private static IEnumerable<object[]> LanguagesData()
        {
            yield return new object[] { generateNPCModel_Languages("Common", null, null, null, null), "Languages Common" };
            yield return new object[] { generateNPCModel_Languages("Common, Dwarvish", null, null, null, null), "Languages Common, Dwarvish" };
            yield return new object[] { generateNPCModel_Languages(null, "Abyssal", null, null, null), "Languages Abyssal" };
            yield return new object[] { generateNPCModel_Languages(null, "Abyssal, Auran", null, null, null), "Languages Abyssal, Auran" };
            yield return new object[] { generateNPCModel_Languages(null, null, "Hook Horror", null, null), "Languages Hook Horror" };
            yield return new object[] { generateNPCModel_Languages(null, null, "Hook Horror, Gnoll", null, null), "Languages Hook Horror, Gnoll" };
            yield return new object[] { generateNPCModel_Languages("Common", "Abyssal", "Hook Horror", null, null), "Languages Hook Horror, Common, Abyssal" };
            yield return new object[] { generateNPCModel_Languages("Common", null, null, null, "90"), "Languages Common, telepathy 90" };
            yield return new object[] { generateNPCModel_Languages("Common", null, null, null, "120 ft."), "Languages Common, telepathy 120 ft." };
            yield return new object[] { generateNPCModel_Languages("Common", null, null, null, "60 ft"), "Languages Common, telepathy 60 ft" };
            yield return new object[] { generateNPCModel_Languages("Common", null, null, null, "30 yd."), "Languages Common, telepathy 30 yd." };
        }
        private static NPCModel generateNPCModel_Languages(string standard, string exotic, string monster, string user, string telepathyRange)
        {
            // Custom Controller for handling Languages
            LanguageController languageController = new LanguageController();

            NPCModel npcModel = new NPCModel();
            npcModel.StandardLanguages = languageController.GenerateStandardLanguages();
            npcModel.ExoticLanguages = languageController.GenerateExoticLanguages();
            npcModel.MonstrousLanguages = languageController.GenerateMonsterLanguages();

            if(standard != null)
                foreach(string lang in standard.Split(','))
                    npcModel.StandardLanguages.First(item => item.Language.Equals(lang.Trim())).Selected = true;

            if(exotic != null)
                foreach (string lang in exotic.Split(','))
                    npcModel.ExoticLanguages.First(item => item.Language.Equals(lang.Trim())).Selected = true;

            if(monster != null)
                foreach (string lang in monster.Split(','))
                    npcModel.MonstrousLanguages.First(item => item.Language.Equals(lang.Trim())).Selected = true;

            if(telepathyRange != null)
            {
                npcModel.Telepathy = true;
                npcModel.TelepathyRange = telepathyRange;
            }

            return npcModel;
        }
        #endregion

        #region Challenge Rating and XP
        [TestMethod]
        [DynamicData(nameof(ChallengeXPData), DynamicDataSourceType.Method)]
        public void Test_Parse_ChallengeXP(NPCModel expectedNpcModel, string text)
        {
            _importEngineerSuiteNPC.ParseChallengeRatingAndXP(actualNPCModel, text);
            AssertChallengeXP(expectedNpcModel, actualNPCModel);
        }

        private void AssertChallengeXP(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            Assert.AreEqual(expectedNPCModel.ChallengeRating, actualNPCModel.ChallengeRating);
            Assert.AreEqual(expectedNPCModel.XP, actualNPCModel.XP);
        }

        private static IEnumerable<object[]> ChallengeXPData()
        {
            yield return new object[] { generateNPCModel_ChallengeXP("8", 3900), "Challenge 8 (3,900 XP)" };
            yield return new object[] { generateNPCModel_ChallengeXP("3",  450), "Challenge 3 (450 XP)" };
        }
        private static NPCModel generateNPCModel_ChallengeXP(string challengeRating, int xpAward)
        {
            NPCModel npcModel = new NPCModel();

            npcModel.ChallengeRating = challengeRating;
            npcModel.XP = xpAward;

            return npcModel;
        }
        #endregion

        #region Traits  (NOT IMPLEMENTED)
        #endregion

        #region Spellcasting
        [TestMethod]
        [DynamicData(nameof(SpellcastingData), DynamicDataSourceType.Method)]
        public void Test_Parse_Spellcasting(NPCModel expectedNpcModel, string text)
        {
            _importEngineerSuiteNPC.ParseSpellCastingAttributes(actualNPCModel, text);
            AssertSpellcasting(expectedNpcModel, actualNPCModel);
        }

        private void AssertSpellcasting(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            Assert.AreEqual(expectedNPCModel.SpellcastingCasterLevel, actualNPCModel.SpellcastingCasterLevel);
            Assert.AreEqual(expectedNPCModel.SCSpellcastingAbility, actualNPCModel.SCSpellcastingAbility);
            Assert.AreEqual(expectedNPCModel.SpellcastingSpellSaveDC, actualNPCModel.SpellcastingSpellSaveDC);
            Assert.AreEqual(expectedNPCModel.SpellcastingSpellHitBonus, actualNPCModel.SpellcastingSpellHitBonus);
            Assert.AreEqual(expectedNPCModel.SpellcastingSpellClass, actualNPCModel.SpellcastingSpellClass);
            Assert.AreEqual(expectedNPCModel.FlavorText, actualNPCModel.FlavorText);
            Assert.AreEqual(expectedNPCModel.CantripSpellSlots, actualNPCModel.CantripSpellSlots);
            Assert.AreEqual(expectedNPCModel.CantripSpellList, actualNPCModel.CantripSpellList);
            Assert.AreEqual(expectedNPCModel.FirstLevelSpellSlots, actualNPCModel.FirstLevelSpellSlots);
            Assert.AreEqual(expectedNPCModel.FirstLevelSpellList, actualNPCModel.FirstLevelSpellList);
            Assert.AreEqual(expectedNPCModel.SecondLevelSpellSlots, actualNPCModel.SecondLevelSpellSlots);
            Assert.AreEqual(expectedNPCModel.SecondLevelSpellList, actualNPCModel.SecondLevelSpellList);
            Assert.AreEqual(expectedNPCModel.ThirdLevelSpellSlots, actualNPCModel.ThirdLevelSpellSlots);
            Assert.AreEqual(expectedNPCModel.ThirdLevelSpellList, actualNPCModel.ThirdLevelSpellList);
            Assert.AreEqual(expectedNPCModel.FourthLevelSpellSlots, actualNPCModel.FourthLevelSpellSlots);
            Assert.AreEqual(expectedNPCModel.FourthLevelSpellList, actualNPCModel.FourthLevelSpellList);
            Assert.AreEqual(expectedNPCModel.FifthLevelSpellSlots, actualNPCModel.FifthLevelSpellSlots);
            Assert.AreEqual(expectedNPCModel.FifthLevelSpellList, actualNPCModel.FifthLevelSpellList);
            Assert.AreEqual(expectedNPCModel.SixthLevelSpellSlots, actualNPCModel.SixthLevelSpellSlots);
            Assert.AreEqual(expectedNPCModel.SixthLevelSpellList, actualNPCModel.SixthLevelSpellList);
            Assert.AreEqual(expectedNPCModel.SeventhLevelSpellSlots, actualNPCModel.SeventhLevelSpellSlots);
            Assert.AreEqual(expectedNPCModel.SeventhLevelSpellList, actualNPCModel.SeventhLevelSpellList);
            Assert.AreEqual(expectedNPCModel.EighthLevelSpellSlots, actualNPCModel.EighthLevelSpellSlots);
            Assert.AreEqual(expectedNPCModel.EighthLevelSpellList, actualNPCModel.EighthLevelSpellList);
            Assert.AreEqual(expectedNPCModel.NinthLevelSpellSlots, actualNPCModel.NinthLevelSpellSlots);
            Assert.AreEqual(expectedNPCModel.NinthLevelSpellList, actualNPCModel.NinthLevelSpellList);
            Assert.AreEqual(expectedNPCModel.MarkedSpells, actualNPCModel.MarkedSpells);
        }

        private static IEnumerable<object[]> SpellcastingData()
        {
            yield return new object[] { 
                generateNPCModel_Spellcasting("18th", "Constitution", 8, 12, "Sorcerer", "", 
                "at will", "Cantrips1",
                "9 slots", "Spell 1st", "8 slots", "Spell 2nd",
                "7 slots", "Spell 3rd", "6 slots", "Spell 4th",
                "5 slots", "Spell 5th", "4 slots", "Spell 6th",
                "3 slots", "Spell 7th", "2 slots", "Spell 8th",
                "1 slot", "Spell 9th", "*Spell 2nd"),
                "Spellcasting. V1_npc_all is an 18th-level spellcaster. His spellcasting ability is Constitution (spell save DC 8, +12 to hit with spell attacks)." +
                " V1_npc_all has the following Sorcerer spells prepared:" +
                "\\rCantrips (at will): Cantrips1" +
                "\\r1st level (9 slots): Spell 1st" +
                "\\r2nd level (8 slots): Spell 2nd" +
                "\\r3rd level (7 slots): Spell 3rd" +
                "\\r4th level (6 slots): Spell 4th" +
                "\\r5th level (5 slots): Spell 5th" +
                "\\r6th level (4 slots): Spell 6th" +
                "\\r7th level (3 slots): Spell 7th" +
                "\\r8th level (2 slots): Spell 8th" +
                "\\r9th level (1 slot): Spell 9th" +
                "\\r*Spell 2nd" };
        }
        private static NPCModel generateNPCModel_Spellcasting(
            string spellCasterLevel, string ability, int saveDC, int hitBonus, string spellClass, string flavorText,
            string cantripSpells, string cantripSpellList, string firstSpells, string firstSpellList,
            string secondSpells, string secondSpellList, string thirdSpells, string thirdSpellList,
            string fourthSpells, string fourthSpellList, string fifthSpells, string fifthSpellList,
            string sixthSpells, string sixthSpellList, string seventhSpells, string seventhSpellList,
            string eighthSpells, string eighthSpellList, string ninthSpells, string ninthSpellList,
            string markedSpells)
        {
            NPCModel npcModel = new NPCModel();

            npcModel.SpellcastingCasterLevel = spellCasterLevel;
            npcModel.SCSpellcastingAbility = ability;
            npcModel.SpellcastingSpellSaveDC = saveDC;
            npcModel.SpellcastingSpellHitBonus = hitBonus;
            npcModel.SpellcastingSpellClass = spellClass;
            npcModel.FlavorText = flavorText;
            npcModel.CantripSpellSlots = cantripSpells;
            npcModel.CantripSpellList = cantripSpellList;
            npcModel.FirstLevelSpellSlots = firstSpells;
            npcModel.FirstLevelSpellList = firstSpellList;

            npcModel.SecondLevelSpellSlots = secondSpells;
            npcModel.SecondLevelSpellList = secondSpellList;

            npcModel.ThirdLevelSpellSlots = thirdSpells;
            npcModel.ThirdLevelSpellList = thirdSpellList;

            npcModel.FourthLevelSpellSlots = fourthSpells;
            npcModel.FourthLevelSpellList = fourthSpellList;

            npcModel.FifthLevelSpellSlots = fifthSpells;
            npcModel.FifthLevelSpellList = fifthSpellList;

            npcModel.SixthLevelSpellSlots = sixthSpells;
            npcModel.SixthLevelSpellList = sixthSpellList;

            npcModel.SeventhLevelSpellSlots = seventhSpells;
            npcModel.SeventhLevelSpellList = seventhSpellList;

            npcModel.EighthLevelSpellSlots = eighthSpells;
            npcModel.EighthLevelSpellList = eighthSpellList;

            npcModel.NinthLevelSpellSlots = ninthSpells;
            npcModel.NinthLevelSpellList = ninthSpellList;
            npcModel.MarkedSpells = markedSpells;

            return npcModel;
        }
        #endregion

        #region Actions

        #region Multiattack
        [TestMethod]
        [DynamicData(nameof(MultiattackData), DynamicDataSourceType.Method)]
        public void Test_Parse_Actions_Multiattack(NPCModel expectedNpcModel, string text)
        {
            _importEngineerSuiteNPC.ParseStandardAction(actualNPCModel, text);
            AssertAction_Multiattack(expectedNpcModel, actualNPCModel);
        }

        private void AssertAction_Multiattack(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            Assert.AreEqual(expectedNPCModel.NPCActions.Count, actualNPCModel.NPCActions.Count);
            Assert.AreEqual(typeof(Multiattack), actualNPCModel.NPCActions[0].GetType());
            Assert.AreEqual(expectedNPCModel.NPCActions[0].ActionName, actualNPCModel.NPCActions[0].ActionName);
            Assert.AreEqual(expectedNPCModel.NPCActions[0].ActionDescription, actualNPCModel.NPCActions[0].ActionDescription);
        }

        private static IEnumerable<object[]> MultiattackData()
        {
            yield return new object[] { generateNPCModel_Multiattack(".This creature makes 3 attacks."), "Multiattack. .This creature makes 3 attacks." };
        }
        private static NPCModel generateNPCModel_Multiattack(string actionDescription)
        {
            NPCModel npcModel = new NPCModel();
            Multiattack multiattack = new Multiattack() { ActionDescription = actionDescription };
            npcModel.NPCActions.Add(multiattack);

            return npcModel;
        }
        #endregion

        #region Weapon (Attack) Action
        [TestMethod]
        [DynamicData(nameof(WeaponActionData), DynamicDataSourceType.Method)]
        public void Test_Parse_Actions_WeaponAction(NPCModel expectedNpcModel, string text)
        {
            _importEngineerSuiteNPC.ParseStandardAction(actualNPCModel, text);
            AssertAction_WeaponAction(expectedNpcModel, actualNPCModel);
        }

        private void AssertAction_WeaponAction(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            Assert.AreEqual(expectedNPCModel.NPCActions.Count, actualNPCModel.NPCActions.Count);
            Assert.AreEqual(expectedNPCModel.NPCActions[0].GetType(), actualNPCModel.NPCActions[0].GetType());
            Assert.AreEqual(typeof(WeaponAttack), expectedNPCModel.NPCActions[0].GetType());
            Assert.AreEqual(typeof(WeaponAttack), actualNPCModel.NPCActions[0].GetType());

            WeaponAttack expectedWeaponAttack = expectedNPCModel.NPCActions[0] as WeaponAttack;
            WeaponAttack actualWeaponAttack = actualNPCModel.NPCActions[0] as WeaponAttack;

            Assert.AreEqual(expectedWeaponAttack.ActionName, actualWeaponAttack.ActionName);
            Assert.AreEqual(expectedWeaponAttack.WeaponType, actualWeaponAttack.WeaponType);
            Assert.AreEqual(expectedWeaponAttack.ToHit, actualWeaponAttack.ToHit);
            Assert.AreEqual(expectedWeaponAttack.Reach, actualWeaponAttack.Reach);
            Assert.AreEqual(expectedWeaponAttack.WeaponRangeShort, actualWeaponAttack.WeaponRangeShort);
            Assert.AreEqual(expectedWeaponAttack.WeaponRangeLong, actualWeaponAttack.WeaponRangeLong);
            Assert.AreEqual(expectedWeaponAttack.TargetType, actualWeaponAttack.TargetType);
            Assert.AreEqual(expectedWeaponAttack.IsAdamantine, actualWeaponAttack.IsAdamantine);
            Assert.AreEqual(expectedWeaponAttack.IsColdForgedIron, actualWeaponAttack.IsColdForgedIron);
            Assert.AreEqual(expectedWeaponAttack.IsMagic, actualWeaponAttack.IsMagic);
            Assert.AreEqual(expectedWeaponAttack.IsSilver, actualWeaponAttack.IsSilver);
            Assert.AreEqual(expectedWeaponAttack.IsVersatile, actualWeaponAttack.IsVersatile);
            Assert.AreEqual(expectedWeaponAttack.PrimaryDamage, actualWeaponAttack.PrimaryDamage);
            Assert.AreEqual(expectedWeaponAttack.SecondaryDamage, actualWeaponAttack.SecondaryDamage);
        }

        private static IEnumerable<object[]> WeaponActionData()
        {
            yield return new object[] { generateNPCModel_WeaponAction("All Specialstat Dagger", WeaponType.MWA,
                0, 5, 30, 60, TargetType.target, true, true, true, true, true,
                new DamageProperty(){Bonus = 0, DamageType = DamageType.Lightning, DieType = DieType.D6, NumOfDice = 1 },
                null), 
                "All Specialstat Dagger. Melee Weapon Attack: +0 to hit, reach 5 ft., one target. " +
                "Hit: 3 (1d6) lightning, silver, adamantine, cold-forged iron, magic damage " +
                "or 4 (1d8) lightning, silver, adamantine, cold-forged iron, magic damage if used with two hands." };
            yield return new object[] { generateNPCModel_WeaponAction("Longbow", WeaponType.RWA,
                6, 5, 120, 600, TargetType.target, false, false, false, false, false,
                new DamageProperty(){Bonus = 3, DamageType = DamageType.Slashing, DieType = DieType.D8, NumOfDice = 2 },
                null),
                "Longbow. Ranged Weapon Attack: +6 to hit, range 120/600 ft., one target. Hit: 12 (2d8 + 3) slashing damage." };
            yield return new object[] { generateNPCModel_WeaponAction("No Idea Spell Attack", WeaponType.SA,
                5, 10, 50, 60, TargetType.creature, false, false, false, false, false,
                new DamageProperty(){Bonus = 2, DamageType = DamageType.Fire, DieType = DieType.D8, NumOfDice = 2 },
                null),
                "No Idea Spell Attack. Melee or Ranged Spell Attack: +5 to hit, reach 10 ft. or range 50 ft., one creature. Hit: 11 (2d8 + 2) fire damage." };
            yield return new object[] { generateNPCModel_WeaponAction("Bonus Damage Dagger", WeaponType.MSA,
                5, 10, 30, 60, TargetType.target, false, false, false, false, false,
                new DamageProperty(){Bonus = 0, DamageType = DamageType.Piercing, DieType = DieType.D6, NumOfDice = 1 },
                new DamageProperty(){Bonus = -4, DamageType = DamageType.Acid, DieType = DieType.D10, NumOfDice = 6 }),
                "Bonus Damage Dagger. Melee Spell Attack: +5 to hit, reach 10 ft., one target. Hit: 3 (1d6) piercing damage plus 29 (6d10 - 4) acid damage." };
            yield return new object[] { generateNPCModel_WeaponAction("Spear", WeaponType.WA,
                7, 5, 20, 60, TargetType.target, false, false, false, false, true,
                new DamageProperty(){Bonus = 4, DamageType = DamageType.Piercing, DieType = DieType.D6, NumOfDice = 1 },
                new DamageProperty(){Bonus = 0, DamageType = DamageType.Fire, DieType = DieType.D6, NumOfDice = 1 }),
                "Spear. Melee or Ranged Weapon Attack: +7 to hit, reach 5 ft. or range 20/60 ft., one target. Hit: 7 (1d6 + 4) piercing damage, or 8 (1d8 + 4) piercing damage if used with two hands to make a melee attack, plus 3 (1d6) fire damage." };
            yield return new object[] { generateNPCModel_WeaponAction("Claws", WeaponType.MWA,
                11, 15, 30, 60, TargetType.target, false, false, false, false, false,
                new DamageProperty(){Bonus = 6, DamageType = DamageType.Slashing, DieType = DieType.D8, NumOfDice = 2 },
                null),
                "Claws. Melee Weapon Attack: +11 to hit, reach 15 ft., one target.Hit: 15 (2d8+6) slashing damage." };
        }
        private static NPCModel generateNPCModel_WeaponAction(string actionName, WeaponType weaponType, int toHit, int reach,
            int weaponShortRange, int weaponLongRange,
            TargetType targetType, bool isAdamantine, bool isColdForgedIron, bool isMagic, bool isSilver, bool isVersatile,
            DamageProperty primaryDamage, DamageProperty secondaryDamage)
        {
            NPCModel npcModel = new NPCModel();
            WeaponAttack weaponAttack = new WeaponAttack();
            weaponAttack.ActionName = actionName;
            weaponAttack.WeaponType = weaponType;
            weaponAttack.ToHit = toHit;
            weaponAttack.Reach = reach;
            weaponAttack.WeaponRangeShort = weaponShortRange;
            weaponAttack.WeaponRangeLong = weaponLongRange;
            weaponAttack.TargetType = targetType;

            // Boolean values
            weaponAttack.IsAdamantine = isAdamantine;
            weaponAttack.IsColdForgedIron = isColdForgedIron;
            weaponAttack.IsMagic = isMagic;
            weaponAttack.IsSilver = isSilver;
            weaponAttack.IsVersatile = isVersatile;

            weaponAttack.PrimaryDamage = primaryDamage;
            weaponAttack.SecondaryDamage = secondaryDamage;

            npcModel.NPCActions.Add(weaponAttack);

            return npcModel;
        }
        #endregion

        #region Other Action
        [TestMethod]
        [DynamicData(nameof(OtherActionData), DynamicDataSourceType.Method)]
        public void Test_Parse_Actions_OtherAction(NPCModel expectedNpcModel, string text)
        {
            _importEngineerSuiteNPC.ParseStandardAction(actualNPCModel, text);
            AssertAction_OtherAction(expectedNpcModel, actualNPCModel);
        }

        private void AssertAction_OtherAction(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            Assert.AreEqual(expectedNPCModel.NPCActions.Count, actualNPCModel.NPCActions.Count);
            Assert.AreEqual(typeof(OtherAction), actualNPCModel.NPCActions[0].GetType());
            Assert.AreEqual(expectedNPCModel.NPCActions[0].ActionName, actualNPCModel.NPCActions[0].ActionName);
            Assert.AreEqual(expectedNPCModel.NPCActions[0].ActionDescription, actualNPCModel.NPCActions[0].ActionDescription);
        }

        private static IEnumerable<object[]> OtherActionData()
        {
            yield return new object[] { generateNPCModel_OtherAction("Some Other Action", "Some Other Action Flavor Text Here."), "Some Other Action. Some Other Action Flavor Text Here." };
        }
        private static NPCModel generateNPCModel_OtherAction(string actionName, string actionDescription)
        {
            NPCModel npcModel = new NPCModel();
            OtherAction otherAction = new OtherAction() {ActionName = actionName, ActionDescription = actionDescription };
            npcModel.NPCActions.Add(otherAction);

            return npcModel;
        }
        #endregion

        #endregion

        #region Lair Actions
        [TestMethod]
        [DynamicData(nameof(LairActionData), DynamicDataSourceType.Method)]
        public void Test_Parse_LairAction(NPCModel expectedNpcModel, string text)
        {
            _importEngineerSuiteNPC.ParseLairAction(actualNPCModel, text);
            AssertLairAction(expectedNpcModel, actualNPCModel);
        }

        private void AssertLairAction(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            // Check to see if expected & actual NPCs have a lair action
            Assert.AreEqual(1, expectedNPCModel.LairActions.Count);
            Assert.AreEqual(1, actualNPCModel.LairActions.Count);

            Assert.AreEqual(expectedNPCModel.LairActions[0].ActionName, actualNPCModel.LairActions[0].ActionName);
            Assert.AreEqual(expectedNPCModel.LairActions[0].ActionDescription, actualNPCModel.LairActions[0].ActionDescription);
        }

        private static IEnumerable<object[]> LairActionData()
        {
            yield return new object[] { generateNPCModel_LairActions("0", "All the options of the lair:"), "All the options of the lair:" };
            yield return new object[] { generateNPCModel_LairActions("0", "One humanoid V1_npc_all can see within 30 feet of him must succeed on a DC 14 Wisdom saving throw or be magically charmed for 1 day.")
                , "One humanoid V1_npc_all can see within 30 feet of him must succeed on a DC 14 Wisdom saving throw or be magically charmed for 1 day."};
            yield return new object[] {generateNPCModel_LairActions("0", "V1_npc_all magically teleports, along with any equipment he is wearing or carrying, up to 60 feet to an unoccupied space he can see."),
                "V1_npc_all magically teleports, along with any equipment he is wearing or carrying, up to 60 feet to an unoccupied space he can see."};
        }
        private static NPCModel generateNPCModel_LairActions(string actionName, string actionDescription)
        {
            NPCModel npcModel = new NPCModel();

            LairAction lairAction = new LairAction() { ActionName = actionName, ActionDescription = actionDescription };

            npcModel.LairActions.Add(lairAction);

            return npcModel;
        }
        #endregion

        #region Legendary Actions
        [TestMethod]
        [DynamicData(nameof(LegendaryActionData), DynamicDataSourceType.Method)]
        public void Test_Parse_LegendaryAction(NPCModel expectedNpcModel, string text)
        {
            _importEngineerSuiteNPC.ParseLegendaryAction(actualNPCModel, text);
            AssertLegendaryAction(expectedNpcModel, actualNPCModel);
        }

        private void AssertLegendaryAction(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            // Check to see if expected & actual NPCs have a lair action
            Assert.AreEqual(1, expectedNPCModel.LegendaryActions.Count);
            Assert.AreEqual(1, actualNPCModel.LegendaryActions.Count);

            Assert.AreEqual(expectedNPCModel.LegendaryActions[0].ActionName, actualNPCModel.LegendaryActions[0].ActionName);
            Assert.AreEqual(expectedNPCModel.LegendaryActions[0].ActionDescription, actualNPCModel.LegendaryActions[0].ActionDescription);
        }

        private static IEnumerable<object[]> LegendaryActionData()
        {
            yield return new object[] { generateNPCModel_LegendaryActions("Options", "This creature has 5 legendary actions."), "Options. This creature has 5 legendary actions." };
            yield return new object[] { generateNPCModel_LegendaryActions("Movement", "(Costs 1 action) The creature can move upto it's fly speed without provoking AOO."), "Movement. (Costs 1 action) The creature can move upto it's fly speed without provoking AOO." };
            yield return new object[] { generateNPCModel_LegendaryActions("Tail Whip", "(Costs 2 actions) The creature can use it's tail whip attack action."), "Tail Whip. (Costs 2 actions) The creature can use it's tail whip attack action." };
        }
        private static NPCModel generateNPCModel_LegendaryActions(string actionName, string actionDescription)
        {
            NPCModel npcModel = new NPCModel();

            LegendaryActionModel legendaryAction = new LegendaryActionModel() { ActionName = actionName, ActionDescription = actionDescription };

            npcModel.LegendaryActions.Add(legendaryAction);

            return npcModel;
        }
        #endregion

        #region Reactions
        [TestMethod]
        [DynamicData(nameof(ReactionsData), DynamicDataSourceType.Method)]
        public void Test_Parse_Reactions(NPCModel expectedNpcModel, string text)
        {
            _importEngineerSuiteNPC.ParseReaction(actualNPCModel, text);
            AssertReactions(expectedNpcModel, actualNPCModel);
        }

        private void AssertReactions(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            // Check to see if expected & actual NPCs have a lair action
            Assert.AreEqual(1, expectedNPCModel.Reactions.Count);
            Assert.AreEqual(1, actualNPCModel.Reactions.Count);

            Assert.AreEqual(expectedNPCModel.Reactions[0].ActionName, actualNPCModel.Reactions[0].ActionName);
            Assert.AreEqual(expectedNPCModel.Reactions[0].ActionDescription, actualNPCModel.Reactions[0].ActionDescription);
        }

        private static IEnumerable<object[]> ReactionsData()
        {
            yield return new object[] { generateNPCModel_Reactions("Parry", "You know what it does.. NINJA DODGE."), "Parry. You know what it does.. NINJA DODGE." };
        }
        private static NPCModel generateNPCModel_Reactions(string actionName, string actionDescription)
        {
            NPCModel npcModel = new NPCModel();

            ActionModelBase reactionData = new ActionModelBase() { ActionName = actionName, ActionDescription = actionDescription };

            npcModel.Reactions.Add(reactionData);

            return npcModel;
        }
        #endregion
    }
}
