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
        #endregion
    }
}
