using Microsoft.VisualStudio.TestTools.UnitTesting;
using FantasyModuleParser.Importer.NPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Versioning;
using System.Reflection;
using System.IO;
using FantasyModuleParser.NPC;

namespace FantasyModuleParser.Importer.NPC.Tests
{
    [TestClass()]
    public class ImportEngineerSuiteNPCTests
    {
        private ImportEngineerSuiteNPC _importEngineerSuiteNPC;

        [TestInitialize]
        public void Initialize()
        {
            _importEngineerSuiteNPC = new ImportEngineerSuiteNPC();
        }

        private string GetEmbeddedResourceFileContent(string embeddedResourcePath)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("FMPTests.Resources.V1_npc_basestats.npc"))
            using (StreamReader reader = new StreamReader(stream))
            {
                 return reader.ReadToEnd();
            }
        }

        private NPCModel LoadEngineerSuiteTestNPCFile()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.V1_npc_all.npc");

            return _importEngineerSuiteNPC.ParseEngineerSuiteNPCContent(fileContent);
        }

        #region Base Stats

        [TestMethod()]
        public void BaseStats_Name()
        {
            Assert.AreEqual("V1_npc_basestats", LoadEngineerSuiteTestNPCFile().NPCName);
        }
        [TestMethod()]
        public void BaseStats_Size()
        {
            Assert.AreEqual("Tiny", LoadEngineerSuiteTestNPCFile().Size);
        }
        [TestMethod()]
        public void BaseStats_Type()
        {
            Assert.AreEqual("beast", LoadEngineerSuiteTestNPCFile().NPCType);
        }
        [TestMethod()]
        public void BaseStats_Tag()
        {
            Assert.AreEqual("devil", LoadEngineerSuiteTestNPCFile().Tag);
        }
        [TestMethod()]
        public void BaseStats_Alignment()
        {
            Assert.AreEqual("devil", LoadEngineerSuiteTestNPCFile().Alignment);
        }
        [TestMethod()]
        public void BaseStats_ArmorClass()
        {
            Assert.AreEqual("16", LoadEngineerSuiteTestNPCFile().AC);
        }
        [TestMethod()]
        public void BaseStats_HitPoints()
        {
            Assert.AreEqual("90 (10d8 + 44)", LoadEngineerSuiteTestNPCFile().HP);
        }
        [TestMethod()]
        public void BaseStats_Gender()
        {
            Assert.AreEqual("male", LoadEngineerSuiteTestNPCFile().NPCGender);
        }
        [TestMethod()]
        public void BaseStats_Speeds_Base()
        {
            Assert.AreEqual(10, LoadEngineerSuiteTestNPCFile().Speed);
        }
        [TestMethod()]
        public void BaseStats_Speeds_Burrow()
        {
            Assert.AreEqual(20, LoadEngineerSuiteTestNPCFile().Burrow);
        }
        [TestMethod()]
        public void BaseStats_Speeds_Climb()
        {
            Assert.AreEqual(30, LoadEngineerSuiteTestNPCFile().Climb);
        }
        [TestMethod()]
        public void BaseStats_Speeds_Fly()
        {
            Assert.AreEqual(40, LoadEngineerSuiteTestNPCFile().Fly);
        }
        [TestMethod()]
        public void BaseStats_Speeds_Hover()
        {
            Assert.AreEqual(true, LoadEngineerSuiteTestNPCFile().Hover);
        }
        [TestMethod()]
        public void BaseStats_Speeds_Swim()
        {
            Assert.AreEqual(50, LoadEngineerSuiteTestNPCFile().Swim);
        }
        [TestMethod()]
        public void BaseStats_Swim()
        {
            Assert.AreEqual(50, LoadEngineerSuiteTestNPCFile().Swim);
        }
        [TestMethod()]
        public void BaseStats_AbilityScores()
        {
            NPCModel parsedNPCModel = LoadEngineerSuiteTestNPCFile();
            Assert.AreEqual(10, parsedNPCModel.AttributeStr);
            Assert.AreEqual(11, parsedNPCModel.AttributeDex);
            Assert.AreEqual(12, parsedNPCModel.AttributeCon);
            Assert.AreEqual(13, parsedNPCModel.AttributeInt);
            Assert.AreEqual(14, parsedNPCModel.AttributeWis);
            Assert.AreEqual(15, parsedNPCModel.AttributeCha);
        }
        [TestMethod()]
        public void BaseStats_SavingThrows()
        {
            NPCModel parsedNPCModel = LoadEngineerSuiteTestNPCFile();
            Assert.AreEqual(1, parsedNPCModel.SavingThrowStr);
            Assert.AreEqual(2, parsedNPCModel.SavingThrowDex);
            Assert.AreEqual(3, parsedNPCModel.SavingThrowCon);
            Assert.AreEqual(0, parsedNPCModel.SavingThrowInt);
            Assert.AreEqual(5, parsedNPCModel.SavingThrowWis);
            Assert.AreEqual(6, parsedNPCModel.SavingThrowCha);

            Assert.AreEqual(false, parsedNPCModel.SavingThrowStrBool);
            Assert.AreEqual(false, parsedNPCModel.SavingThrowDexBool);
            Assert.AreEqual(false, parsedNPCModel.SavingThrowConBool);
            Assert.AreEqual(true, parsedNPCModel.SavingThrowIntBool);
            Assert.AreEqual(false, parsedNPCModel.SavingThrowWisBool);
            Assert.AreEqual(false, parsedNPCModel.SavingThrowChaBool);
        }

        [TestMethod()]
        public void BaseStats_SenseRanges()
        {
            NPCModel parsedNPCModel = LoadEngineerSuiteTestNPCFile();
            Assert.AreEqual(60, parsedNPCModel.Blindsight);
            Assert.AreEqual(70, parsedNPCModel.Darkvision);
            Assert.AreEqual(80, parsedNPCModel.Tremorsense);
            Assert.AreEqual(90, parsedNPCModel.Truesight);
            Assert.AreEqual(40, parsedNPCModel.PassivePerception);

            Assert.AreEqual(true, parsedNPCModel.BlindBeyond);
        }
        [TestMethod()]
        public void BaseStats_ChallengeRating()
        {
            Assert.AreEqual("8", LoadEngineerSuiteTestNPCFile().ChallengeRating);
        }
        [TestMethod()]
        public void BaseStats_Experience()
        {
            Assert.AreEqual(3900, LoadEngineerSuiteTestNPCFile().XP);
        }
        #endregion

        #region Resistances

        private NPCModel LoadV1NPCResistanceNPCModel()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.V1_npc_resists.npc");

            return _importEngineerSuiteNPC.ParseEngineerSuiteNPCContent(fileContent);
        }

        #endregion
    }
}