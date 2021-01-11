using FantasyModuleParser.Importer.Spells;
using FantasyModuleParser.Importer.Utils.Tests;
using FantasyModuleParser.Spells.Enums;
using FantasyModuleParser.Spells.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace FMPTests_Spells.Importer
{
    [TestClass]
    public class ImportPDFSpellFileTests
    {
        private IImportSpell _IImportSpell;
        [TestInitialize]
        public void Initialize()
        {
            _IImportSpell = new ImportSpellBase();
        }

        private SpellModel LoadSpellFileContent(String pathToFile)
        {
            string fileContent = ImportCommonUtilsTests.GetEmbeddedResourceFileContent("FMPTests.Resources.Spells." + pathToFile);
            return _IImportSpell.ImportTextToSpellModel(fileContent);
        }

        #region Hoard of the Dragon Queen supplement
        private const string HotDQ_BASE_PATH = "PDF.HoardDragonQueen_Supplement.";
        [TestMethod]
        public void AnimalFriendship()
        {
            SpellModel expectedSpellModel = LoadSpellFileContent(HotDQ_BASE_PATH + "AnimalFriendship.txt");

            Assert.AreEqual("Animal Friendship", expectedSpellModel.SpellName);

            Assert.AreEqual(expectedSpellModel.SpellLevel, FantasyModuleParser.Spells.Enums.SpellLevel.First);
            Assert.AreEqual(expectedSpellModel.SpellSchool, FantasyModuleParser.Spells.Enums.SpellSchool.Enchantment);
            Assert.AreEqual(expectedSpellModel.IsRitual, false);

            Assert.AreEqual(expectedSpellModel.CastingTime, 1);
            Assert.AreEqual(expectedSpellModel.CastingType, FantasyModuleParser.Spells.Enums.CastingType.Action);
            Assert.AreEqual(expectedSpellModel.ReactionDescription, null);

            Assert.AreEqual(expectedSpellModel.Range, 30);
            Assert.AreEqual(expectedSpellModel.RangeType, FantasyModuleParser.Spells.Enums.RangeType.Ranged);

            Assert.AreEqual(expectedSpellModel.IsVerbalComponent, true);
            Assert.AreEqual(expectedSpellModel.IsSomaticComponent, true);
            Assert.AreEqual(expectedSpellModel.IsMaterialComponent, true);
            Assert.AreEqual(expectedSpellModel.ComponentText, "a morsel of food");

            Assert.AreEqual(expectedSpellModel.DurationTime, 24);
            Assert.AreEqual(expectedSpellModel.DurationType, DurationType.Time);
            Assert.AreEqual(expectedSpellModel.DurationUnit, DurationUnit.Hour);
            Assert.AreEqual(expectedSpellModel.DurationText, null);

            //TODO:  Assert the Description
        }
        [TestMethod]
        public void ColorSpray()
        {
            SpellModel expectedSpellModel = LoadSpellFileContent(HotDQ_BASE_PATH + "ColorSpray.txt");

            Assert.AreEqual("Color Spray", expectedSpellModel.SpellName);
            Assert.AreEqual(expectedSpellModel.SpellLevel, FantasyModuleParser.Spells.Enums.SpellLevel.First);
            Assert.AreEqual(expectedSpellModel.SpellSchool, FantasyModuleParser.Spells.Enums.SpellSchool.Illusion);
            Assert.AreEqual(expectedSpellModel.IsRitual, false);

            Assert.AreEqual(expectedSpellModel.CastingTime, 1);
            Assert.AreEqual(expectedSpellModel.CastingType, FantasyModuleParser.Spells.Enums.CastingType.Action);
            Assert.AreEqual(expectedSpellModel.ReactionDescription, null);

            Assert.AreEqual(expectedSpellModel.Range, 15);
            Assert.AreEqual(expectedSpellModel.RangeType, FantasyModuleParser.Spells.Enums.RangeType.Self);
            Assert.AreEqual(expectedSpellModel.Unit, FantasyModuleParser.Spells.Enums.UnitType.Foot);
            Assert.AreEqual(expectedSpellModel.SelfType, FantasyModuleParser.Spells.Enums.SelfType.Cone);

            Assert.AreEqual(expectedSpellModel.IsVerbalComponent, true);
            Assert.AreEqual(expectedSpellModel.IsSomaticComponent, true);
            Assert.AreEqual(expectedSpellModel.IsMaterialComponent, true);
            Assert.AreEqual(expectedSpellModel.ComponentText, "a pinch of powder or sand that is colored red, yellow, and blue");

            Assert.AreEqual(expectedSpellModel.DurationTime, 1);
            Assert.AreEqual(expectedSpellModel.DurationType, DurationType.Time);
            Assert.AreEqual(expectedSpellModel.DurationUnit, DurationUnit.Round);
            Assert.AreEqual(expectedSpellModel.DurationText, null);
        }
        [TestMethod]
        public void Confusion()
        {
            SpellModel expectedSpellModel = LoadSpellFileContent(HotDQ_BASE_PATH + "Confusion.txt");

            Assert.AreEqual("Confusion", expectedSpellModel.SpellName);
        }
        [TestMethod]
        public void Druidcraft()
        {
            SpellModel expectedSpellModel = LoadSpellFileContent(HotDQ_BASE_PATH + "Druidcraft.txt");

            Assert.AreEqual("Druidcraft", expectedSpellModel.SpellName);
        }
        [TestMethod]
        public void FeatherFall()
        {
            SpellModel expectedSpellModel = LoadSpellFileContent(HotDQ_BASE_PATH + "Featherfall.txt");

            Assert.AreEqual("Feather Fall", expectedSpellModel.SpellName);
        }
        [TestMethod]
        public void InsectPlague()
        {
            SpellModel expectedSpellModel = LoadSpellFileContent(HotDQ_BASE_PATH + "InsectPlague.txt");

            Assert.AreEqual("Insect Plague", expectedSpellModel.SpellName);
        }
        [TestMethod]
        public void ScorchingRay()
        {
            SpellModel expectedSpellModel = LoadSpellFileContent(HotDQ_BASE_PATH + "ScorchingRay.txt");

            Assert.AreEqual("Scorching Ray", expectedSpellModel.SpellName);
        }
        [TestMethod]
        public void WaterWalk()
        {
            SpellModel expectedSpellModel = LoadSpellFileContent(HotDQ_BASE_PATH + "WaterWalk.txt");

            Assert.AreEqual("Water Walk", expectedSpellModel.SpellName);
        }
        #endregion

        [TestMethod]
        public void AscendingFlame()
        {
            SpellModel expectedSpellModel = LoadSpellFileContent("PDF.AscendingFlame.txt");

            Assert.AreEqual("Ascending Flame", expectedSpellModel.SpellName);

            Assert.AreEqual(expectedSpellModel.SpellLevel, FantasyModuleParser.Spells.Enums.SpellLevel.Cantrip);
            Assert.AreEqual(expectedSpellModel.SpellSchool, FantasyModuleParser.Spells.Enums.SpellSchool.Evocation);
            Assert.AreEqual(expectedSpellModel.IsRitual, false);

            Assert.AreEqual(expectedSpellModel.CastingTime, 1);
            Assert.AreEqual(expectedSpellModel.CastingType, FantasyModuleParser.Spells.Enums.CastingType.Action);
            Assert.AreEqual(expectedSpellModel.ReactionDescription, null);

            Assert.AreEqual(expectedSpellModel.Range, 60);
            Assert.AreEqual(expectedSpellModel.RangeType, FantasyModuleParser.Spells.Enums.RangeType.Ranged);

            Assert.AreEqual(expectedSpellModel.IsVerbalComponent, true);
            Assert.AreEqual(expectedSpellModel.IsSomaticComponent, true);
            Assert.AreEqual(expectedSpellModel.IsMaterialComponent, false);
            Assert.AreEqual(expectedSpellModel.ComponentText, null);

            Assert.AreEqual(expectedSpellModel.DurationTime, 0);
            Assert.AreEqual(expectedSpellModel.DurationType, DurationType.Instantaneous);
            Assert.AreEqual(expectedSpellModel.DurationUnit, DurationUnit.None);
            Assert.AreEqual(expectedSpellModel.DurationText, null);

            Assert.AreEqual(expectedSpellModel.CastBy, "Cleric");

            Assert.AreEqual(expectedSpellModel.Description, "A flame-like radiance burns a creature that you can see within range. The target must succeed on a Dexterity saving throw or take 1d6 radiant damage, granting you half of the damage dealt as temporary hit points. The target gains no benefit from cover for this saving throw. The spell's damage increases by 1d6 when you reach 5th level (2d6), 11th level (3d6), and 17th level (4d6).");
        }
    }
}
