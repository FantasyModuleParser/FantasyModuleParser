﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action.Enums;
using FantasyModuleParser.Extensions;
using System.Collections.ObjectModel;
using FantasyModuleParser.NPC.Models.Skills;

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

        // Used to validate all Lists generated & presented in the Resistance tab
        private void AssertSelectableActionModelList(List<SelectableActionModel> expected, List<SelectableActionModel> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);

            // Sort both lists by ActionName so that the following loop will guarantee to work
            expected.Sort((x, y) => x.ActionName.CompareTo(y.ActionName));
            actual.Sort((x, y) => x.ActionName.CompareTo(y.ActionName));
            for (int idx = 0; idx < expected.Count; idx++)
            {
                Assert.AreEqual(expected[idx].ActionName, actual[idx].ActionName);
                Assert.AreEqual(expected[idx].Selected, actual[idx].Selected);
            }
        }
        [TestMethod()]
        public void Resistances_DamageVulnerabilities()
        {
            NPCController controller = new NPCController();
            List<SelectableActionModel> expectedDamageVulnerabilites = controller.GetSelectableActionModelList(typeof(DamageType));

            // For the unit test, the following are set to true based on the file v1_npc_all.npc
            // Acid, Fire, Lighting, Poison, Radiant, Bludgeoning, Slashing
            foreach (SelectableActionModel selectableActionModel in expectedDamageVulnerabilites)
            {
                switch (selectableActionModel.ActionName)
                {
                    case "Acid":
                    case "Fire":
                    case "Lighting":
                    case "Poison":
                    case "Radiant":
                    case "Bludgeoning":
                    case "Slashing":
                        selectableActionModel.Selected = true;
                        break;
                }
            }

            Assert.AreEqual(expectedDamageVulnerabilites, LoadEngineerSuiteTestNPCFile().DamageVulnerabilityModelList);
        }

        [TestMethod()]
        public void Resistances_DamageResistances()
        {
            NPCController controller = new NPCController();
            List<SelectableActionModel> expectedDamageResistances = controller.GetSelectableActionModelList(typeof(DamageType));

            foreach (SelectableActionModel selectableActionModel in expectedDamageResistances)
            {
                switch (selectableActionModel.ActionName)
                {
                    case "Cold":
                    case "Force":
                    case "Necrotic":
                    case "Psychic":
                    case "Thunder":
                        selectableActionModel.Selected = true;
                        break;
                }
            }

            Assert.AreEqual(expectedDamageResistances, LoadEngineerSuiteTestNPCFile().DamageResistanceModelList);

        }

        [TestMethod()]
        public void Resistances_SpecialWeaponResistance()
        {
            NPCController controller = new NPCController();
            List<SelectableActionModel> expectedWeaponResistance = controller.GetSelectableActionModelList(typeof(WeaponResistance));

            foreach (SelectableActionModel selectableActionModel in expectedWeaponResistance)
            {
                switch (selectableActionModel.ActionName)
                {
                    case "Resistant to nonmagical weapons":
                        selectableActionModel.Selected = true;
                        break;
                }
            }

            Assert.AreEqual(expectedWeaponResistance, LoadEngineerSuiteTestNPCFile().SpecialWeaponResistanceModelList);
        }

        [TestMethod()]
        public void Resistances_DamageImmunities()
        {
            NPCController controller = new NPCController();
            List<SelectableActionModel> expectedActionModelList = controller.GetSelectableActionModelList(typeof(DamageType));

            foreach (SelectableActionModel selectableActionModel in expectedActionModelList)
            {
                switch (selectableActionModel.ActionName)
                {
                    case "Acid":
                    case "Force":
                    case "Poison":
                    case "Thunder":
                    case "Slashing":
                        selectableActionModel.Selected = true;
                        break;
                }
            }

            Assert.AreEqual(expectedActionModelList, LoadEngineerSuiteTestNPCFile().DamageImmunityModelList);

        }

        [TestMethod()]
        public void Resistances_SpecialWeaponImmunity()
        {
            NPCController controller = new NPCController();
            List<SelectableActionModel> expectedActionModelList = controller.GetSelectableActionModelList(typeof(WeaponImmunity));

            foreach (SelectableActionModel selectableActionModel in expectedActionModelList)
            {
                switch (selectableActionModel.ActionName)
                {
                    case "Immune to nonmagical weapons that aren't silvered":
                        selectableActionModel.Selected = true;
                        break;
                }
            }

            Assert.AreEqual(expectedActionModelList, LoadEngineerSuiteTestNPCFile().SpecialWeaponImmunityModelList);
        }

        [TestMethod()]
        public void Resistances_ConditionImmunities()
        {
            NPCController controller = new NPCController();
            List<SelectableActionModel> expectedActionModelList = controller.GetSelectableActionModelList(typeof(ConditionType));

            foreach (SelectableActionModel selectableActionModel in expectedActionModelList)
            {
                switch (selectableActionModel.ActionName)
                {
                    case "Blinded":
                    case "Frightened":
                    case "Invisible":
                    case "Paralyzed":
                    case "Prone":
                    case "Restrained":
                        selectableActionModel.Selected = true;
                        break;
                }
            }

            Assert.AreEqual(expectedActionModelList, LoadEngineerSuiteTestNPCFile().ConditionImmunityModelList);
        }

        #endregion

        #region Skills

        [TestMethod()]
        public void Skills_Bonuses()
        {
            NPCModel actualNPCModel = LoadEngineerSuiteTestNPCFile();

            Assert.AreEqual(1, actualNPCModel.Acrobatics);
            Assert.AreEqual(2, actualNPCModel.AnimalHandling);
            Assert.AreEqual(3, actualNPCModel.Arcana);
            Assert.AreEqual(4, actualNPCModel.Athletics);
            Assert.AreEqual(5, actualNPCModel.Deception);
            Assert.AreEqual(6, actualNPCModel.History);
            Assert.AreEqual(7, actualNPCModel.Insight);
            Assert.AreEqual(8, actualNPCModel.Intimidation);
            Assert.AreEqual(9, actualNPCModel.Investigation);
            Assert.AreEqual(10, actualNPCModel.Medicine);
            Assert.AreEqual(11, actualNPCModel.Nature);
            Assert.AreEqual(12, actualNPCModel.Perception);
            Assert.AreEqual(13, actualNPCModel.Performance);
            Assert.AreEqual(14, actualNPCModel.Persuasion);
            Assert.AreEqual(15, actualNPCModel.Religion);
            Assert.AreEqual(16, actualNPCModel.SleightOfHand);
            Assert.AreEqual(17, actualNPCModel.Stealth);
            Assert.AreEqual(18, actualNPCModel.Survival);
        }

        // Used to validate all Lists generated & presented in the Resistance tab
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

        [TestMethod()]
        public void Skills_CommonLanguages()
        {
            ObservableCollection<LanguageModel> expectedLanguages = new LanguageController().GenerateStandardLanguages();

            foreach (LanguageModel language in expectedLanguages)
            {
                switch (language.Language)
                {
                    case "Common":
                    case "Elvish":
                    case "Gnomish":
                    case "Halfling":
                    case "Thieves' Cant":
                        language.Selected = true;
                        break;
                }
            }

            AssertLanguageModelList(expectedLanguages, LoadEngineerSuiteTestNPCFile().StandardLanguages);
        }

        [TestMethod()]
        public void Skills_ExoticLanguages()
        {
            ObservableCollection<LanguageModel> expectedLanguages = new LanguageController().GenerateExoticLanguages();

            foreach (LanguageModel language in expectedLanguages)
            {
                switch (language.Language)
                {
                    case "Celestial":
                    case "Draconic":
                    case "Infernal":
                    case "Slyvan":
                        language.Selected = true;
                        break;
                }
            }

            AssertLanguageModelList(expectedLanguages, LoadEngineerSuiteTestNPCFile().StandardLanguages);
        }

        [TestMethod()]
        public void Skills_MonstrousLanguages()
        {
            ObservableCollection<LanguageModel> expectedLanguages = new LanguageController().GenerateMonsterLanguages();

            foreach (LanguageModel language in expectedLanguages)
            {
                switch (language.Language)
                {
                    case "Aarakocra":
                    case "Bullywug":
                    case "Grell":
                    case "Ice Toad":
                    case "Modron":
                    case "Slaad":
                    case "Thri-kreen":
                    case "Umber hulk":
                        language.Selected = true;
                        break;
                }
            }

            AssertLanguageModelList(expectedLanguages, LoadEngineerSuiteTestNPCFile().StandardLanguages);
        }

        [TestMethod()]
        public void Skills_Telepathy()
        {
            NPCModel actualNPCModel = LoadEngineerSuiteTestNPCFile();

            Assert.AreEqual(true, actualNPCModel.Telepathy);
            Assert.AreEqual(90, actualNPCModel.TelepathyRange);
        }

        #endregion

    }
}