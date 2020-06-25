using FantasyModuleParser.Importer.NPC;
using FantasyModuleParser.NPC;
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
            yield return new object[] { generateNPCModel_SizeAndAlignment("tiny", "beast", "devil", "lawful neutral"), "Tiny beast (devil), lawful neutral" };
            yield return new object[] { generateNPCModel_SizeAndAlignment("medium", "humanoid", "", "lawful good"), "Medium humanoid, lawful good" };
            yield return new object[] { generateNPCModel_SizeAndAlignment("huge", "giant", "", "chaotic evil"), "Huge giant, chaotic evil" };
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
    }
}
