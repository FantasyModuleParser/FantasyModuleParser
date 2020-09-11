using FantasyModuleParser.Importer.NPC;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
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
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.PDF.aarakocra_brave.txt");
            return _iImportNPC.ImportTextToNPCModel(fileContent);
        }

        [TestMethod()]
        public void Import_AarakocraBrave_Test()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.PDF.aarakocra_brave.txt");

            NPCModel actualNPCModel = _iImportNPC.ImportTextToNPCModel(fileContent);
            Assert.AreEqual("Aarakocra Brave", actualNPCModel.NPCName);
            Assert.AreEqual("Medium", actualNPCModel.Size);
            Assert.AreEqual("humanoid", actualNPCModel.NPCType);
            Assert.AreEqual("(aarakocra)", actualNPCModel.Tag);
            Assert.AreEqual("neutral good", actualNPCModel.Alignment);
            Assert.AreEqual("14", actualNPCModel.AC);
            Assert.AreEqual("78 (12d8 + 24)", actualNPCModel.HP);
            Assert.AreEqual(30, actualNPCModel.Speed);
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
            Skills_ExoticLanguages(actualNPCModel);

            // Validate Challenge Rating
            Assert.AreEqual("3", actualNPCModel.ChallengeRating);
            Assert.AreEqual(700, actualNPCModel.XP);

            // Traits
            Assert.AreEqual(2, actualNPCModel.Traits.Count);
            Assert.AreEqual("Dive Attack", actualNPCModel.Traits[0].ActionName);
            Assert.AreEqual("Flyby", actualNPCModel.Traits[1].ActionName);
            Assert.AreEqual("If the aarakocra is flying and dives at least 30 feet straight toward a target and then hits it with a melee weapon attack, the attack deals an extra 10 (3d6) damage to the target.", actualNPCModel.Traits[0].ActionDescription);
            Assert.AreEqual("The aarakocra doesn't provoke an opportunity attack when it flies out of an enemy's reach.", actualNPCModel.Traits[1].ActionDescription);

            // Action
            Assert.AreEqual(3, actualNPCModel.NPCActions.Count);
            Assert.AreEqual("Multiattack", actualNPCModel.NPCActions[0].ActionName);
            Assert.AreEqual("Talon", actualNPCModel.NPCActions[1].ActionName);
            Assert.AreEqual("Javelin", actualNPCModel.NPCActions[2].ActionName);
            Assert.AreEqual("The aarakocra makes three melee attacks: one with its javelin and two with its talons, or two ranged attacks.", actualNPCModel.NPCActions[0].ActionDescription);
            Assert.AreEqual("Melee Weapon Attack: +6 to hit, reach 5 ft., one target. Hit: 7 (1d6 + 4) slashing damage.", actualNPCModel.NPCActions[1].ActionDescription);
            Assert.AreEqual("Melee Weapon Attack: +6 to hit, reach 5 ft., one target. Hit: 7 (1d6 + 4) piercing damage. Or Ranged Weapon Attack: +6 to hit, range 30/120 ft., one target. Hit: 7 (1d6 + 4) piercing damage.", actualNPCModel.NPCActions[2].ActionDescription);
        }

        [TestMethod()]
        public void Import_AarakocraTalonOfSyranita_Test()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.PDF.aarakocra_talon_of_syranita.txt");

            NPCModel actualNPCModel = _iImportNPC.ImportTextToNPCModel(fileContent);
            Assert.AreEqual("Aarakocra Talon of Syranita", actualNPCModel.NPCName);
            Assert.AreEqual("Medium", actualNPCModel.Size);
            Assert.AreEqual("humanoid", actualNPCModel.NPCType);
            Assert.AreEqual("(aarakocra)", actualNPCModel.Tag);
            Assert.AreEqual("neutral good", actualNPCModel.Alignment);
            Assert.AreEqual("13", actualNPCModel.AC);
            Assert.AreEqual("97 (15d8 + 30)", actualNPCModel.HP);
            Assert.AreEqual(20, actualNPCModel.Speed);
            Assert.AreEqual(50, actualNPCModel.Fly);

            // Validate Stats
            Assert.AreEqual(10, actualNPCModel.AttributeStr);
            Assert.AreEqual(16, actualNPCModel.AttributeDex);
            Assert.AreEqual(14, actualNPCModel.AttributeCon);
            Assert.AreEqual(11, actualNPCModel.AttributeInt);
            Assert.AreEqual(17, actualNPCModel.AttributeWis);
            Assert.AreEqual(11, actualNPCModel.AttributeCha);

            // Validate Skills
            Assert.AreEqual(6, actualNPCModel.Nature);
            Assert.AreEqual(9, actualNPCModel.Perception);
            Assert.AreEqual(9, actualNPCModel.Stealth);
            Assert.AreEqual(9, actualNPCModel.Survival);

            // Validate Senses
            Assert.AreEqual(19, actualNPCModel.PassivePerception);

            // Validate Languages
            Skills_ExoticLanguages(actualNPCModel);

            // Validate Challenge Rating
            Assert.AreEqual("6", actualNPCModel.ChallengeRating);
            Assert.AreEqual(2300, actualNPCModel.XP);

            // Traits
            Assert.AreEqual(2, actualNPCModel.Traits.Count);
            Assert.AreEqual("Dive Attack", actualNPCModel.Traits[0].ActionName);
            Assert.AreEqual("Fury of Syranita", actualNPCModel.Traits[1].ActionName);
            Assert.AreEqual("If the aarakocra is flying and dives at least 30 feet straight toward a target and then hits it with a melee weapon attack, the attack deals an extra 7 (2d6) damage to the target.", actualNPCModel.Traits[0].ActionDescription);
            Assert.AreEqual("As a bonus action, the aarakocra can expend a spell slot to cause its melee or ranged weapon attacks to magically deal an extra 13 (3d8) lightning or thunder damage to a target on a hit. This benefit lasts until the end of the turn. If the aarakora expends a spell slot of 2nd level or higher, the extra damage increases by 1d8 for each level above 1st (maximum 6d8).", actualNPCModel.Traits[1].ActionDescription);

            // Spellcasting
            Assert.AreEqual("at will", actualNPCModel.CantripSpells);
            Assert.AreEqual("light, mending, resistance, spare the dying", actualNPCModel.CantripSpellList);
            Assert.AreEqual("4 slots", actualNPCModel.FirstLevelSpells);
            Assert.AreEqual("guiding bolt, healing word, protection from evil and good", actualNPCModel.FirstLevelSpellList);
            Assert.AreEqual("3 slots", actualNPCModel.SecondLevelSpells);
            Assert.AreEqual("augury, hold person, zone of truth", actualNPCModel.SecondLevelSpellList);
            Assert.AreEqual("3 slots", actualNPCModel.ThirdLevelSpells);
            Assert.AreEqual("beacon of hope, call lightning, mass healing word", actualNPCModel.ThirdLevelSpellList);
            Assert.AreEqual("3 slots", actualNPCModel.FourthLevelSpells);
            Assert.AreEqual("freedom of movement, ice storm", actualNPCModel.FourthLevelSpellList);
            Assert.AreEqual("1 slot", actualNPCModel.FifthLevelSpells);
            Assert.AreEqual("conjure (air) elemental", actualNPCModel.FifthLevelSpellList);
            // Action
            Assert.AreEqual(3, actualNPCModel.NPCActions.Count);
            Assert.AreEqual("Multiattack", actualNPCModel.NPCActions[0].ActionName);
            Assert.AreEqual("Talon", actualNPCModel.NPCActions[1].ActionName);
            Assert.AreEqual("Javelin", actualNPCModel.NPCActions[2].ActionName);
            Assert.AreEqual("The aarakocra makes two melee attacks or two ranged attacks.", actualNPCModel.NPCActions[0].ActionDescription);
            Assert.AreEqual("Melee Weapon Attack: +6 to hit, reach 5 ft., one target. Hit: 5 (1d4 + 3) slashing damage.", actualNPCModel.NPCActions[1].ActionDescription);
            Assert.AreEqual("Melee Weapon Attack: +6 to hit, reach 5 ft., one target. Hit: 6 (1d6 + 3) piercing damage. Or Ranged Weapon Attack: +6 to hit, range 30/120 ft., one target. Hit: 6 (1d6 + 3) piercing damage.", actualNPCModel.NPCActions[2].ActionDescription);
        }
        private void AssertLanguageModelList(ObservableCollection<LanguageModel> expected, ObservableCollection<LanguageModel> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);

            // Sort both lists by ActionName so that the following loop will guarantee to work
            List<LanguageModel> expectedLanguages = expected.ToList();
            List<LanguageModel> actualLanguages = actual.ToList();

            expectedLanguages.Sort((x, y) => x.Language.CompareTo(y.Language));
            actualLanguages.Sort((x, y) => x.Language.CompareTo(y.Language));
            for (int idx = 0; idx < expectedLanguages.Count; idx++)
            {
                Assert.AreEqual(expectedLanguages[idx].Language, actualLanguages[idx].Language);
                Assert.AreEqual(expectedLanguages[idx].Selected, actualLanguages[idx].Selected);
            }
        }

        public void Skills_ExoticLanguages(NPCModel actualNPCModel)
        {
            ObservableCollection<LanguageModel> expectedLanguages = new LanguageController().GenerateExoticLanguages();

            foreach (LanguageModel language in expectedLanguages)
            {
                switch (language.Language)
                {
                    case "Auran":
                        language.Selected = true;
                        break;
                }
            }
            AssertLanguageModelList(expectedLanguages, actualNPCModel.ExoticLanguages);
        }

    }
}
