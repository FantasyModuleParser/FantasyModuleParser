﻿using FantasyModuleParser.Importer.NPC;
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
    }
}
