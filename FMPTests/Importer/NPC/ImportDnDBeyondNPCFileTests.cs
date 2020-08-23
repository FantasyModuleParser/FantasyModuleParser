using Microsoft.VisualStudio.TestTools.UnitTesting;
using FantasyModuleParser.Importer.NPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyModuleParser.NPC;
using System.IO;
using System.Reflection;

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
        }
    }
}