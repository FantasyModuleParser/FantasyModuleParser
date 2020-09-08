using FantasyModuleParser.Importer.NPC;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Models.Skills;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FantasyModuleParser.Importer.NPC.Tests
{
    [TestClass()]
    public class ImportPDFNPCFileTests
    {
        private IImportNPC _iImportNPC;
        NPCModel actualNPCModel = null;

        [TestInitialize]
        public void Initialize()
        {
            _iImportNPC = new ImportPDFNPC();
            actualNPCModel = LoadPDFTestNPCFile();
        }

        private string GetEmbeddedResourceFileContent(string embeddedResourcePath)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@embeddedResourcePath))
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        private NPCModel LoadPDFTestNPCFile()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.aarakocra_brave.txt");
            return _iImportNPC.ImportTextToNPCModel(fileContent);
        }

        [TestMethod()]
        public void Import_AarakocraBrave_Test()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.aarakocra_brave.txt");

            NPCModel actualNPCModel = _iImportNPC.ImportTextToNPCModel(fileContent);
            Assert.AreEqual("Aarakocra Brave", actualNPCModel.NPCName);
            Assert.AreEqual("Medium", actualNPCModel.Size);
            Assert.AreEqual("humanoid", actualNPCModel.NPCType);
            Assert.AreEqual("(aarakocra)", actualNPCModel.Tag);
            Assert.AreEqual("neutral good", actualNPCModel.Alignment);
            Assert.AreEqual("14", actualNPCModel.AC);
            Assert.AreEqual("78 (12d8 + 24)", actualNPCModel.HP);
            Assert.AreEqual(20, actualNPCModel.Speed);
            Assert.AreEqual(60, actualNPCModel.Fly);

            // Validate Stats
            Assert.AreEqual(10, actualNPCModel.AttributeStr);
            Assert.AreEqual(18, actualNPCModel.AttributeDex);
            Assert.AreEqual(14, actualNPCModel.AttributeCon);
            Assert.AreEqual(11, actualNPCModel.AttributeInt);
            Assert.AreEqual(13, actualNPCModel.AttributeWis);
            Assert.AreEqual(11, actualNPCModel.AttributeCha);

            // Validate Skills
            Assert.AreEqual(6, actualNPCModel.Acrobatics);
            Assert.AreEqual(5, actualNPCModel.Perception);

            // Validate Senses
            Assert.AreEqual(15, actualNPCModel.PassivePerception);

            // Validate Languages
            Assert.AreEqual("Auran", actualNPCModel.ExoticLanguages);
            // Validate Challenge Rating
            Assert.AreEqual(3, actualNPCModel.ChallengeRating);
            Assert.AreEqual(700, actualNPCModel.XP);

            // Traits
            Assert.AreEqual(2, actualNPCModel.Traits.Count);
            Assert.AreEqual("Dive Attack", actualNPCModel.Traits[0].ActionName);
            Assert.AreEqual("Flyby", actualNPCModel.Traits[1].ActionName);
            Assert.AreEqual("If the aarakocra is flying and dives at least 30 feet straight toward a target and then hits it with a melee weapon attack, the attack deals an extra 10(3d6) damage to the target.", actualNPCModel.Traits[0].ActionDescription);
            Assert.AreEqual("The aarakocra doesn't provoke an opportunity attack when it flies out of an enemy's reach.", actualNPCModel.Traits[1].ActionDescription);
        }
    }
}
