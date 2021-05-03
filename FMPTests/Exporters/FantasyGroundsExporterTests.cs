using Microsoft.VisualStudio.TestTools.UnitTesting;
using FantasyModuleParser.Exporters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyModuleParser.Main.Services;
using System.IO;
using System.Reflection;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC;
using FantasyModuleParser.Main.Models;
using System.Xml.Linq;
using FantasyModuleParser.Tables.Models;

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
            moduleModel = new ModuleModel
            {
                Name = "Test Module",
                Author = "Fredska",
                Category = "Supplement",
                //moduleModel.ModulePath = "Path\\To\\Everything";

                Categories = new System.Collections.ObjectModel.ObservableCollection<CategoryModel>()
            };
            moduleModel.Categories.Add(new CategoryModel() { Name = "Supplement" });

            exporter = new FantasyGroundsExporter();
        }

        [TestMethod()]
        public void GenerateDefinitionXmlContentTest()
        {
            string xmlContent = exporter.GenerateDefinitionXmlContent(moduleModel);

            string expected = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                "<root version=\"4\">\r\n" +
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

			ModuleModel moduleModel = new ModuleModel
			{
				Name = "IntegrationTest_Mage1",
				Author = "Automated MS Test v2",
				Category = "Automated",
				//moduleModel.ModulePath = 
				//    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FantasyModuleParser_UnitTests");

				Categories = new System.Collections.ObjectModel.ObservableCollection<CategoryModel>()
			};
			moduleModel.Categories.Add(new CategoryModel() { Name = "Automated" });
            moduleModel.Categories[0].NPCModels.Add(npcModel);

            // And finally, for the actual integration test run
            exporter.CreateModule(moduleModel);
        }

        public void Create_BlankDB_XMLFile()
        {
            string xmlContent = exporter.GenerateDBXmlFile(moduleModel);

            string expected = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                              "<root version=\"4\" dataversion=\"20200528\" release=\"8|CoreRPG:4\" />";

            string cleaned = xmlContent.Replace("\n", "").Replace("\r", "");

            Assert.AreEqual(XmlPrettyPrint(expected), XmlPrettyPrint(cleaned));
        }

        public void Create_DBXml_OneNewNPC()
        {
            // For this test, load a newly initalized NPC into the ModuleModel data object
            moduleModel.Categories = new System.Collections.ObjectModel.ObservableCollection<CategoryModel>();;
            moduleModel.Categories.Add(new CategoryModel() { Name = "Automated" });
            moduleModel.Categories[0].NPCModels.Add(new NPCController().InitializeNPCModel());

            string xmlContent = exporter.GenerateDBXmlFile(moduleModel);

            string expected = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                              "<root version=\"4\" dataversion=\"20200528\" release=\"8|CoreRPG:4\" />";
            Assert.AreEqual(XmlPrettyPrint(expected), XmlPrettyPrint(xmlContent));
        }

        //[TestMethod]
        public void Create_Module_MultipleCategories_ImageTest()
        {
            // * Initialize Test Data

            string[] mage1PathParams = new string[]
            {
                Directory.GetCurrentDirectory(),
                "Resources",
                "Mage1.json"
            };

            NPCController npcController = new NPCController();
            // Make a whole bunch-o-copies of the Mage1.json, and change the NPCImage string value for each appropriately
            NPCModel[] npcModels = new NPCModel[] {
                npcController.Load(Path.Combine(mage1PathParams)),
                npcController.Load(Path.Combine(mage1PathParams)),
                npcController.Load(Path.Combine(mage1PathParams)),
                npcController.Load(Path.Combine(mage1PathParams)),
                npcController.Load(Path.Combine(mage1PathParams))
            };

            npcModels[1].NPCName = "Mage2";
            npcModels[2].NPCName = "Mage3";
            npcModels[3].NPCName = "Mage4";
            npcModels[4].NPCName = "Mage5";

            npcModels[1].NPCImage = @"C:\Users\darkpool\AppData\Roaming\NPC Engineer\Saved NPC Files\MonsterManual_DEV\Displacer Beast.jpg";
            npcModels[2].NPCImage = @"C:\Users\darkpool\AppData\Roaming\NPC Engineer\Saved NPC Files\MonsterManual_DEV\Shambling Mound.jpg";
            npcModels[3].NPCImage = @"C:\Users\darkpool\AppData\Roaming\NPC Engineer\Saved NPC Files\MonsterManual_DEV\Wood Woad.jpg";
            npcModels[4].NPCImage = @"C:\Users\darkpool\AppData\Roaming\NPC Engineer\Saved NPC Files\MonsterManual_DEV\Displacer Beast.jpg";

			// Setup the ModuleModel data
			// Module Path = Windows -> MyDocuments -> FantasyModuleParser_UnitTests
			// Module Name = IntegrationTest_Mage1

			ModuleModel moduleModel = new ModuleModel
			{
				Name = "IntegrationTest_MultipleCategories",
				Author = "Automated MS Test v2",
				Category = "Automated",
				//moduleModel.ModulePath =
				//    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FantasyModuleParser_UnitTests");

				Categories = new System.Collections.ObjectModel.ObservableCollection<CategoryModel>()
			};
			moduleModel.Categories.Add(new CategoryModel() { Name = "Automated" });
            moduleModel.Categories.Add(new CategoryModel() { Name = "MyNPCs" });
            moduleModel.Categories.Add(new CategoryModel() { Name = "Fey Creatures" });
            moduleModel.Categories[0].NPCModels.Add(npcModels[0]);
            moduleModel.Categories[0].NPCModels.Add(npcModels[1]);
            moduleModel.Categories[1].NPCModels.Add(npcModels[2]);
            moduleModel.Categories[1].NPCModels.Add(npcModels[3]);
            moduleModel.Categories[2].NPCModels.Add(npcModels[4]);

            // And finally, for the actual integration test run
            exporter.CreateModule(moduleModel);
        }

        [TestMethod]
        public void CreateModule_SingleTable()
        {
            TableModel tableModel = new TableModel()
            {
                Name = "Example Table",
                Description = "This is an example of the table model",
                Notes = "<p>Some notes that appear</p><p><b>This should be in bold</b>",
                RollMethod = Tables.ViewModels.Enums.RollMethodEnum.TableValues,
                OutputType = Tables.ViewModels.Enums.OutputTypeEnum.Chat,
                ShowResultsInChat = true,
                IsLocked = true,
                ColumnHeaderLabels = new List<string>() { "From","To","Column 1","Equipment Drop"},
                BasicStringGridData = new List<List<string>>()
                {
                    new List<string>(){"1","1","Some Data","Armor Piece 1"},
                    new List<string>(){"2","6", "A bad time","Cursed Weapon"}
                }
            };

            CategoryModel categoryModel = new CategoryModel()
            {
                Name = "ExampleCategory",
                TableModels = new System.Collections.ObjectModel.ObservableCollection<TableModel>()
                {
                    tableModel
                }
            };

            ModuleModel moduleModel = new ModuleModel()
            {
                Name = "TableExampleModule",
                Author = "E2E_Test",
                Category = "Testing",
                Categories = new System.Collections.ObjectModel.ObservableCollection<CategoryModel>()
                {
                    categoryModel
                }
            };

            exporter.CreateModule(moduleModel);
            Assert.IsTrue(true);
        }

        private static string XmlPrettyPrint(string input)
        {
            XDocument xDocument = new XDocument(input);
            return xDocument.ToString();
        }
    }
}