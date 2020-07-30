using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Action.Enums;
using FantasyModuleParser.NPC.Models.Skills;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FantasyModuleParser.Importer.NPC.Tests
{
    [TestClass()]
    public class ImportEngineerSuiteNPCTests
    {
        private ImportEngineerSuiteNPC _importEngineerSuiteNPC;
        NPCModel actualNPCModel = null;

        [TestInitialize]
        public void Initialize()
        {
            _importEngineerSuiteNPC = new ImportEngineerSuiteNPC();
            actualNPCModel = LoadEngineerSuiteTestNPCFile();
        }

        private string GetEmbeddedResourceFileContent(string embeddedResourcePath)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@embeddedResourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private NPCModel LoadEngineerSuiteTestNPCFile()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.V1_npc_all.npc");

            return _importEngineerSuiteNPC.ImportTextToNPCModel(fileContent);
        }

        #region Base Stats

        [TestMethod()]
        public void BaseStats_Name()
        {
            Assert.AreEqual("V1_npc_all", LoadEngineerSuiteTestNPCFile().NPCName);
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
            Assert.AreEqual("(devil)", LoadEngineerSuiteTestNPCFile().Tag);
        }
        [TestMethod()]
        public void BaseStats_Alignment()
        {
            Assert.AreEqual("lawful neutral", LoadEngineerSuiteTestNPCFile().Alignment);
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
            Assert.AreEqual(-5, parsedNPCModel.SavingThrowWis);
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
            Assert.AreEqual(22, parsedNPCModel.PassivePerception);

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
                    case "Lightning":
                    case "Poison":
                    case "Radiant":
                    case "Bludgeoning":
                    case "Slashing":
                        selectableActionModel.Selected = true;
                        break;
                }
            }

            AssertSelectableActionModelList(expectedDamageVulnerabilites, LoadEngineerSuiteTestNPCFile().DamageVulnerabilityModelList);
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

            AssertSelectableActionModelList(expectedDamageResistances, LoadEngineerSuiteTestNPCFile().DamageResistanceModelList);
        }

        [TestMethod()]
        public void Resistances_SpecialWeaponResistance()
        {
            NPCController controller = new NPCController();
            List<SelectableActionModel> expectedWeaponResistance = controller.GetSelectableActionModelList(typeof(WeaponResistance));

            expectedWeaponResistance.First(item => item.ActionName.Equals(WeaponResistance.Nonmagical.ToString(), StringComparison.Ordinal)).Selected = true;

            AssertSelectableActionModelList(expectedWeaponResistance, LoadEngineerSuiteTestNPCFile().SpecialWeaponResistanceModelList);

        }

        [TestMethod()]
        public void Resistances_DamageImmunities()
        {
            NPCController controller = new NPCController();
            List<SelectableActionModel> expectedDamageImmunityModelList = controller.GetSelectableActionModelList(typeof(DamageType));
            foreach (SelectableActionModel selectableActionModel in expectedDamageImmunityModelList)
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
            NPCModel actualNPCModel = LoadEngineerSuiteTestNPCFile();
            AssertSelectableActionModelList(expectedDamageImmunityModelList, actualNPCModel.DamageImmunityModelList);
            
        }

        [TestMethod()]
        public void Resistances_SpecialWeaponImmunity()
        {
            NPCController controller = new NPCController();
            List<SelectableActionModel> expectedActionModelList = controller.GetSelectableActionModelList(typeof(WeaponImmunity));

            expectedActionModelList.First(item => item.ActionName.Equals(WeaponImmunity.NonmagicalSilvered.ToString(), StringComparison.Ordinal)).Selected = true;

            AssertSelectableActionModelList(expectedActionModelList, actualNPCModel.SpecialWeaponImmunityModelList);
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

            AssertSelectableActionModelList(expectedActionModelList, LoadEngineerSuiteTestNPCFile().ConditionImmunityModelList);
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
                    case "Sylvan":
                        language.Selected = true;
                        break;
                }
            }

            AssertLanguageModelList(expectedLanguages, LoadEngineerSuiteTestNPCFile().ExoticLanguages);
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

            AssertLanguageModelList(expectedLanguages, LoadEngineerSuiteTestNPCFile().MonstrousLanguages);
        }

        [TestMethod()]
        public void Skills_Telepathy()
        {
            NPCModel actualNPCModel = LoadEngineerSuiteTestNPCFile();

            Assert.AreEqual(true, actualNPCModel.Telepathy);
            Assert.AreEqual("90", actualNPCModel.TelepathyRange);
        }

        #endregion

        #region Casting

        [TestMethod()]
        public void Casting_CasterInformation()
        {
            NPCModel actualNPCModel = LoadEngineerSuiteTestNPCFile();

            Assert.AreEqual("18th", actualNPCModel.SpellcastingCasterLevel);
            Assert.AreEqual("Constitution", actualNPCModel.SCSpellcastingAbility);
            Assert.AreEqual(8, actualNPCModel.SpellcastingSpellSaveDC);
            Assert.AreEqual(12, actualNPCModel.SpellcastingSpellHitBonus);
            Assert.AreEqual("Sorcerer", actualNPCModel.SpellcastingSpellClass);
            Assert.AreEqual("", actualNPCModel.FlavorText);
        }

        [TestMethod()]
        public void Casting_SpellSelection()
        {
            NPCModel actualNPCModel = LoadEngineerSuiteTestNPCFile();

            Assert.AreEqual("At will", actualNPCModel.CantripSpells);
            Assert.AreEqual("Cantrips1", actualNPCModel.CantripSpellList);
            Assert.AreEqual("9 slots", actualNPCModel.FirstLevelSpells);
            Assert.AreEqual("Spell 1st", actualNPCModel.FirstLevelSpellList);
            Assert.AreEqual("8 slots", actualNPCModel.SecondLevelSpells);
            Assert.AreEqual("Spell 2nd", actualNPCModel.SecondLevelSpellList);
            Assert.AreEqual("7 slots", actualNPCModel.ThirdLevelSpells);
            Assert.AreEqual("Spell 3rd", actualNPCModel.ThirdLevelSpellList);
            Assert.AreEqual("6 slots", actualNPCModel.FourthLevelSpells);
            Assert.AreEqual("Spell 4th", actualNPCModel.FourthLevelSpellList);
            Assert.AreEqual("5 slots", actualNPCModel.FifthLevelSpells);
            Assert.AreEqual("Spell 5th", actualNPCModel.FifthLevelSpellList);
            Assert.AreEqual("4 slots", actualNPCModel.SixthLevelSpells);
            Assert.AreEqual("Spell 6th", actualNPCModel.SixthLevelSpellList);
            Assert.AreEqual("3 slots", actualNPCModel.SeventhLevelSpells);
            Assert.AreEqual("Spell 7th", actualNPCModel.SeventhLevelSpellList);
            Assert.AreEqual("2 slots", actualNPCModel.EighthLevelSpells);
            Assert.AreEqual("Spell 8th", actualNPCModel.EighthLevelSpellList);
            Assert.AreEqual("1 slot", actualNPCModel.NinthLevelSpells);
            Assert.AreEqual("Spell 9th", actualNPCModel.NinthLevelSpellList);
        }
        [TestMethod()]
        public void Casting_MarkedSpells()
        {
            NPCModel actualNPCModel = LoadEngineerSuiteTestNPCFile();

            Assert.AreEqual("*Spell 2nd", actualNPCModel.MarkedSpells);
        }


        #endregion

        #region Actions

        #region Standard Actions
        [TestMethod()]
        public void Actions_Validate_StandardActionCount()
        {
            Assert.AreEqual(6, actualNPCModel.NPCActions.Count);
        }
        [TestMethod]
        public void Actions_Validate_FirstStandardAction()
        {
            Assert.IsInstanceOfType(actualNPCModel.NPCActions[0], typeof(Multiattack));

            Multiattack actualMultiattack = (Multiattack)actualNPCModel.NPCActions[0];

            Assert.AreEqual(".This creature makes 3 attacks.", actualMultiattack.ActionDescription);
        }

        private void AssertValidWeaponAttack(WeaponAttack expected, WeaponAttack actual)
        {
            Assert.AreEqual(expected.ActionName, actual.ActionName);
            Assert.AreEqual(expected.WeaponType, actual.WeaponType);
            Assert.AreEqual(expected.IsMagic, actual.IsMagic);
            Assert.AreEqual(expected.IsAdamantine, actual.IsAdamantine);
            Assert.AreEqual(expected.IsSilver, actual.IsSilver);
            Assert.AreEqual(expected.IsColdForgedIron, actual.IsColdForgedIron);
            Assert.AreEqual(expected.IsVersatile, actual.IsVersatile);
            Assert.AreEqual(expected.ToHit, actual.ToHit);
            Assert.AreEqual(expected.Reach, actual.Reach);
            Assert.AreEqual(expected.WeaponRangeShort, actual.WeaponRangeShort);
            Assert.AreEqual(expected.WeaponRangeLong, actual.WeaponRangeLong);
            Assert.AreEqual(expected.TargetType, actual.TargetType);
            Assert.AreEqual(expected.PrimaryDamage, actual.PrimaryDamage);
            Assert.AreEqual(expected.SecondaryDamage, actual.SecondaryDamage);
            Assert.AreEqual(expected.OtherText, actual.OtherText);
        }

        [TestMethod]
        public void Actions_Validate_SecondStandardAction()
        {
            Assert.IsInstanceOfType(actualNPCModel.NPCActions[1], typeof(WeaponAttack));

            WeaponAttack expectedWeaponAttack = new WeaponAttack();

            expectedWeaponAttack.ActionName = "All Specialstat Dagger";
            expectedWeaponAttack.WeaponType = WeaponType.MWA;
            expectedWeaponAttack.IsMagic = true;
            expectedWeaponAttack.IsAdamantine = true;
            expectedWeaponAttack.IsSilver = true;
            expectedWeaponAttack.IsColdForgedIron = true;
            expectedWeaponAttack.IsVersatile = true;
            expectedWeaponAttack.ToHit = 0;
            expectedWeaponAttack.Reach = 5;
            expectedWeaponAttack.WeaponRangeShort = 30; // Default
            expectedWeaponAttack.WeaponRangeLong = 60;  // Default
            expectedWeaponAttack.TargetType = TargetType.target;
            expectedWeaponAttack.PrimaryDamage = new DamageProperty(1, DieType.D6, 0, DamageType.Lightning);
            expectedWeaponAttack.SecondaryDamage = new DamageProperty(1, DieType.D8, 0, DamageType.Lightning); ;
            expectedWeaponAttack.OtherText = "";

            WeaponAttack actualWeaponAttack = (WeaponAttack)actualNPCModel.NPCActions[1];

            AssertValidWeaponAttack(expectedWeaponAttack, actualWeaponAttack); 
        }

        [TestMethod]
        public void Actions_Validate_ThirdStandardAction()
        {
            Assert.IsInstanceOfType(actualNPCModel.NPCActions[2], typeof(WeaponAttack));

            WeaponAttack expectedWeaponAttack = new WeaponAttack();

            expectedWeaponAttack.ActionName = "Longbow";
            expectedWeaponAttack.WeaponType = WeaponType.RWA;
            expectedWeaponAttack.IsMagic = false;
            expectedWeaponAttack.IsAdamantine = false;
            expectedWeaponAttack.IsSilver = false;
            expectedWeaponAttack.IsColdForgedIron = false;
            expectedWeaponAttack.IsVersatile = false;
            expectedWeaponAttack.ToHit = 6;
            expectedWeaponAttack.Reach = 5;
            expectedWeaponAttack.WeaponRangeShort = 120; 
            expectedWeaponAttack.WeaponRangeLong = 600;  
            expectedWeaponAttack.TargetType = TargetType.target;
            expectedWeaponAttack.PrimaryDamage = new DamageProperty(2, DieType.D8, 3, DamageType.Slashing);
            expectedWeaponAttack.SecondaryDamage = null;
            expectedWeaponAttack.OtherText = "";

            WeaponAttack actualWeaponAttack = (WeaponAttack)actualNPCModel.NPCActions[2];

            AssertValidWeaponAttack(expectedWeaponAttack, actualWeaponAttack);
        }

        [TestMethod]
        public void Actions_Validate_FourthStandardAction()
        {
            Assert.IsInstanceOfType(actualNPCModel.NPCActions[3], typeof(WeaponAttack));
            WeaponAttack actualWeaponAttack = (WeaponAttack)actualNPCModel.NPCActions[3];

            WeaponAttack expectedWeaponAttack = new WeaponAttack();

            expectedWeaponAttack.ActionName = "No Idea Spell Attack";
            expectedWeaponAttack.WeaponType = WeaponType.SA;
            expectedWeaponAttack.IsMagic = false;
            expectedWeaponAttack.IsAdamantine = false;
            expectedWeaponAttack.IsSilver = false;
            expectedWeaponAttack.IsColdForgedIron = false;
            expectedWeaponAttack.IsVersatile = false;
            expectedWeaponAttack.ToHit = 5;
            expectedWeaponAttack.Reach = 10;
            expectedWeaponAttack.WeaponRangeShort = 50; 
            expectedWeaponAttack.WeaponRangeLong = 60;  // ??????  Default maybe?
            expectedWeaponAttack.TargetType = TargetType.creature;
            expectedWeaponAttack.PrimaryDamage = new DamageProperty(2, DieType.D8, 2, DamageType.Fire);
            expectedWeaponAttack.SecondaryDamage = null;
            expectedWeaponAttack.OtherText = "";

            

            AssertValidWeaponAttack(expectedWeaponAttack, actualWeaponAttack);
        }

        [TestMethod]
        public void Actions_Validate_FifthStandardAction()
        {
            Assert.IsInstanceOfType(actualNPCModel.NPCActions[4], typeof(WeaponAttack));
            WeaponAttack actualWeaponAttack = (WeaponAttack)actualNPCModel.NPCActions[4];

            WeaponAttack expectedWeaponAttack = new WeaponAttack();

            expectedWeaponAttack.ActionName = "Bonus Damage Dagger";
            expectedWeaponAttack.WeaponType = WeaponType.MSA;
            expectedWeaponAttack.IsMagic = false;
            expectedWeaponAttack.IsAdamantine = false;
            expectedWeaponAttack.IsSilver = false;
            expectedWeaponAttack.IsColdForgedIron = false;
            expectedWeaponAttack.IsVersatile = false;
            expectedWeaponAttack.ToHit = 5;
            expectedWeaponAttack.Reach = 10;
            expectedWeaponAttack.WeaponRangeShort = 30; // Default
            expectedWeaponAttack.WeaponRangeLong = 60;  // Default
            expectedWeaponAttack.TargetType = TargetType.target;
            expectedWeaponAttack.PrimaryDamage = new DamageProperty(1, DieType.D6, 0, DamageType.Piercing);
            expectedWeaponAttack.SecondaryDamage = new DamageProperty(6, DieType.D10, -4, DamageType.Acid);
            expectedWeaponAttack.OtherText = "";

            

            AssertValidWeaponAttack(expectedWeaponAttack, actualWeaponAttack);
        }

        [TestMethod]
        public void Actions_Validate_SixthStandardAction()
        {
            Assert.IsInstanceOfType(actualNPCModel.NPCActions[5], typeof(OtherAction));
            OtherAction actualOtherAction = (OtherAction)actualNPCModel.NPCActions[5];

            Assert.AreEqual("Some Other Action", actualOtherAction.ActionName);
            Assert.AreEqual("Some Other Action Flavor Text Here.", actualOtherAction.ActionDescription);
        }

        #endregion

        #region Lair Actions
        [TestMethod]
        public void Actions_LairActionCount()
        {
            Assert.AreEqual(3, actualNPCModel.LairActions.Count);
        }

        /// <summary>
        /// Unit test for Lair Action.  Important to note that ActionName is a placeholder that is populated in ***Part 3***
        /// </summary>
        /// <param name="actionIndex"></param>
        /// <param name="expectedName"></param>
        /// <param name="expectedDescription"></param>
        [TestMethod]
        [DataRow(0, "Options", "All the options of the lair:")]
        [DataRow(1, "Charm", "One humanoid V1_npc_all can see within 30 feet of him must succeed on a DC 14 Wisdom saving throw or be magically charmed for 1 day.")]
        [DataRow(2, "Teleport", "V1_npc_all magically teleports, along with any equipment he is wearing or carrying, up to 60 feet to an unoccupied space he can see.")]
        public void Actions_Validate_LairActions(int actionIndex, string expectedName, string expectedDescription)
        {
            LairAction actualLairAction = actualNPCModel.LairActions[actionIndex];
            Assert.AreEqual(expectedName, actualLairAction.ActionName);
            Assert.AreEqual(expectedDescription, actualLairAction.ActionDescription);
        }
        #endregion

        #region Legendary Actions
        [TestMethod]
        public void Actions_LegendaryActionCount()
        {
            Assert.AreEqual(3, actualNPCModel.LegendaryActions.Count);
        }

        [TestMethod]
        [DataRow(0, "Options", "This creature has 5 legendary actions.")]
        [DataRow(1, "Movement", "(Costs 1 action) The creature can move upto it's fly speed without provoking AOO.")]
        [DataRow(2, "Tail Whip", "(Costs 2 actions) The creature can use it's tail whip attack action.")]
        public void Actions_Validate_LegendaryActions(int actionIndex, string expectedName, string expectedDescription)
        {
            LegendaryActionModel actualLegendaryAction = actualNPCModel.LegendaryActions[actionIndex];
            Assert.AreEqual(expectedName, actualLegendaryAction.ActionName);
            Assert.AreEqual(expectedDescription, actualLegendaryAction.ActionDescription);
        }
        #endregion

        #region Reactions
        [TestMethod]
        public void Actions_ReactionsCount()
        {
            Assert.AreEqual(1, actualNPCModel.Reactions.Count);
        }

        [TestMethod]
        [DataRow(0, "Parry", "You know what it does.. NINJA DODGE.")]
        public void Actions_Validate_Reactions(int actionIndex, string expectedName, string expectedDescription)
        {
            ActionModelBase actualReaction = actualNPCModel.Reactions[actionIndex];
            Assert.AreEqual(expectedName, actualReaction.ActionName);
            Assert.AreEqual(expectedDescription, actualReaction.ActionDescription);
        }
        #endregion

        #endregion

    }
}