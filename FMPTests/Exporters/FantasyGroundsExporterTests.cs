using Microsoft.VisualStudio.TestTools.UnitTesting;
using FantasyModuleParser.Exporters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyModuleParser.NPC.Models;
using FantasyModuleParser.Main.Services;

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
    }
}