using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using System.Text;

namespace FantasyModuleParser.NPC.ViewModels.Tests
{
    [TestClass()]
    public class DescriptionUCViewModelTests
    {
        private DescriptionUCViewModel viewModel;
        [TestInitialize]
        public void Initialize()
        {
            viewModel = new DescriptionUCViewModel();
        }

        [TestMethod()]
        [DataRow("# Description", "<h>Description</h>")]
        [DataRow(@"**Bold**", "<p><b>Bold</b></p>")]
        [DataRow(@"*Italized*", "<p><i>Italized</i></p>")]
        [DataRow(@"***Bold And Italized***", "<p><i><b>Bold And Italized</b></i></p>")]
        [DataRow(@"* Item 1
* Item 2", 
            @"<list>
              <li>Item 1</li>
              <li>Item 2</li>
              </list>")]
        [DataRow("```Chatbox Text```", "<p><frame>Chatbox Text</frame></p>")]
        public void ValidateXMLTest(string actualContent, string expectedContent)
        {
            string transformedActualContent = viewModel.ValidateXML(actualContent);

            Assert.AreEqual(expectedContent, transformedActualContent);
        }
        [TestMethod]
        public void Validate_ValidateXML_IntegrationTest_1()
        {
            string data = "# Core Spawn Crawler\r\n" +
                "The smallest and most numerous of the core spawn," +
                " these eyeless creatures scurry through the subterranean darkness with the help of their four irregular," +
                " gangly arms and hooked prehensile tails." +
                " Core spawn crawlers rarely travel alone, and a group of these agile predators is known as a vein of crawlers." +
                " Their clattering taloned limbs warn of their presence as they scuttle through the shadow-haunted depths of the earth." +
                "\r\n# Core Spawn" +
                "\r\nThe Elder Evils assault the multiverse in strange and calamitous ways." +
                " Sometimes they breach the Material Plane by exploiting the unfathomable energy and darkness found in the world's depths." +
                " These terrestrial manifestations of loathsome alien agendas are known as core spawn," +
                " and they are as varied in their physiology as they are horrific.";

            string expected = "<h>Core Spawn Crawler</h>\r\n" +
                "<p>The smallest and most numerous of the core spawn," +
                " these eyeless creatures scurry through the subterranean darkness with the help of their four irregular," +
                " gangly arms and hooked prehensile tails." +
                " Core spawn crawlers rarely travel alone, and a group of these agile predators is known as a vein of crawlers." +
                " Their clattering taloned limbs warn of their presence as they scuttle through the shadow-haunted depths of the earth.</p>" +
                "\r\n<h>Core Spawn</h>" +
                "\r\n<p>The Elder Evils assault the multiverse in strange and calamitous ways." +
                " Sometimes they breach the Material Plane by exploiting the unfathomable energy and darkness found in the world's depths." +
                " These terrestrial manifestations of loathsome alien agendas are known as core spawn," +
                " and they are as varied in their physiology as they are horrific.</p>";

            string transformedActualContent = viewModel.ValidateXML(data);

            Assert.AreEqual(expected, transformedActualContent);
        }

        [TestMethod()]
        // FMPTests.Resources.V1_npc_all.npc
        [DataRow("FMPTests.Resources.MarkdownDescriptions.AncestralBarbarianTest.md", "FMPTests.Resources.MarkdownDescriptions.ExpectedResult.AncestralBarbarianExpected.txt")]
        public void Validate_DescriptionXMLTransform_File(string actualFileDataPath, string expectedFileDataPath)
        {
            string actualDataContent = GetEmbeddedResourceFileContent(actualFileDataPath);
            string expectedDataContent = GetEmbeddedResourceFileContent(expectedFileDataPath);

            string transformedActualContent = viewModel.ValidateXML(actualDataContent);

            Assert.AreEqual(expectedDataContent, transformedActualContent);
        }

        private string GetEmbeddedResourceFileContent(string embeddedResourcePath)
        {

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@embeddedResourcePath))
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}