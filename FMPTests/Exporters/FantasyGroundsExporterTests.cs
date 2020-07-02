using Microsoft.VisualStudio.TestTools.UnitTesting;
using FantasyModuleParser.Exporters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyModuleParser.NPC.Models;
using FantasyModuleParser.Main.Services;
using System.IO;
using System.Reflection;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC;

namespace FantasyModuleParser.Exporters.Tests
{
    [TestClass()]
    public class FantasyGroundsExporterTests
    {
        private ModuleModel moduleModel;
        
        private FantasyGroundsExporter exporter;
        [TestInitialize]
        public void Initialize()
        {
            moduleModel = new ModuleModel();
            moduleModel.Name = "Test Module";
            moduleModel.Author = "Fredska";
            moduleModel.Category = "Supplement";
            moduleModel.ModulePath = "Path\\To\\Everything";

            exporter = new FantasyGroundsExporter();
        }

        [TestMethod()]
        public void GenerateDefinitionXmlContentTest()
        {
            string xmlContent = exporter.GenerateDefinitionXmlContent(moduleModel);

            string expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                "<root version=\"3.3\">\r\n" +
                "  <name>Test Module</name>\r\n" +
                "  <category>Supplement</category>\r\n" +
                "  <author>Fredska</author>\r\n" +
                "  <ruleset>5E</ruleset>\r\n" +
                "</root>";
            Assert.AreEqual(expected, xmlContent);
        }

        [TestMethod]
        public void CreateModule_Mage1_IntegrationTest()
        {

            // * Initialize Test Data

            string[] pathParams = new string[]
            {
                Directory.GetCurrentDirectory(),
                "Resources",
                "Mage1.json"
            };

            NPCController npcController = new NPCController();
            NPCModel npcModel = npcController.Load(Path.Combine(pathParams));

            // Setup the ModuleModel data
            // Module Path = Windows -> MyDocuments -> FantasyModuleParser_UnitTests
            // Module Name = IntegrationTest_Mage1

            ModuleModel moduleModel = new ModuleModel();
            moduleModel.Name = "IntegrationTest_Mage1";
            moduleModel.Author = "Automated MS Test v2";
            moduleModel.Category = "Automated";
            moduleModel.ModulePath = 
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FantasyModuleParser_UnitTests");

            moduleModel.NPCModels = new List<NPCModel>();
            moduleModel.NPCModels.Add(npcModel);

            // And finally, for the actual integration test run
            exporter.CreateModule(moduleModel);
        }
    }
}