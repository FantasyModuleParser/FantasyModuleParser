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
        public void ValidateXMLTest(string actualContent, string expectedContent)
        {
            string transformedActualContent = viewModel.ValidateXML(actualContent);

            Assert.AreEqual(expectedContent, transformedActualContent);
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