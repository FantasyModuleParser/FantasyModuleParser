using Microsoft.VisualStudio.TestTools.UnitTesting;
using FantasyModuleParser.Spells.Models;
using FantasyModuleParser.Spells.Enums;
using System.Collections.Generic;
using FantasyModuleParser.Importer.Spells;
using System.Linq;

namespace FMPTests_Spells.Importer
{
    [TestClass]
    public class ImportSpellLineParserTest
    {

        private IImportSpell _importSpell;
        [TestInitialize]
        public void Initialize()
        {
            _importSpell = new ImportSpellBase();
           
        }

        #region Spell Level and School
        [TestMethod]
        [DynamicData(nameof(SpellLevelAndSchoolData), DynamicDataSourceType.Method)]
        public void TestSpellLevelAndSchool(SpellModel expectedSpellModel, string importData)
        {
            SpellModel actualSpellModel = _importSpell.ImportTextToSpellModel(importData);
            Assert.AreEqual(expectedSpellModel.SpellLevel, actualSpellModel.SpellLevel);
            Assert.AreEqual(expectedSpellModel.SpellSchool, actualSpellModel.SpellSchool);
            Assert.AreEqual(expectedSpellModel.IsRitual, actualSpellModel.IsRitual);
        }
        

        private static IEnumerable<object[]> SpellLevelAndSchoolData()
        {
            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.First, SpellSchool.Abjuration), "1st-level abjuration" };
            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.Second, SpellSchool.Abjuration), "2nd-level abjuration" };
            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.Third, SpellSchool.Abjuration), "3rd-level abjuration" };
            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.Fourth, SpellSchool.Abjuration), "4th-level abjuration" };
            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.Fifth, SpellSchool.Abjuration), "5th-level abjuration" };
            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.Sixth, SpellSchool.Abjuration), "6th-level abjuration" };
            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.Seventh, SpellSchool.Abjuration), "7th-level abjuration" };
            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.Eighth, SpellSchool.Abjuration), "8th-level abjuration" };
            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.Ninth, SpellSchool.Abjuration), "9th-level abjuration" };

            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.Third, SpellSchool.Conjuration), "3rd-level conjuration" };
            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.Third, SpellSchool.Divination), "3rd-level divination" };
            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.Third, SpellSchool.Enchantment), "3rd-level enchantment" };
            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.Third, SpellSchool.Evocation), "3rd-level evocation" };
            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.Third, SpellSchool.Illusion), "3rd-level illusion" };
            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.Third, SpellSchool.Necromancy), "3rd-level necromancy" };
            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.Third, SpellSchool.Transmutation), "3rd-level transmuation" };

            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.Third, SpellSchool.Abjuration, true), "3rd-level abjuration (ritual)" };

        }
        private static SpellModel generateSpellModel_SpellLevelAndSchool(SpellLevel spellLevel, SpellSchool school)
        {
            return generateSpellModel_SpellLevelAndSchool(spellLevel, school, false);
        }
        private static SpellModel generateSpellModel_SpellLevelAndSchool(SpellLevel spellLevel, SpellSchool school, bool isRitual)
        {
            SpellModel spellModel = new SpellModel()
            {
                SpellLevel = spellLevel,
                SpellSchool = school,
                IsRitual = isRitual
            };
            return spellModel;
        }
        #endregion

        #region Casting Time
        [TestMethod]
        [DynamicData(nameof(CastingTimeData), DynamicDataSourceType.Method)]
        public void TestCastingTime(SpellModel expectedSpellModel, string importData)
        {
            SpellModel actualSpellModel = _importSpell.ImportTextToSpellModel(importData);
            Assert.AreEqual(expectedSpellModel.CastingTime, actualSpellModel.CastingTime);
            Assert.AreEqual(expectedSpellModel.CastingType, actualSpellModel.CastingType);
        }


        private static IEnumerable<object[]> CastingTimeData()
        {
            yield return new object[] { generateSpellModel_CastingTime(1, CastingType.Action), "1 action" };

        }

        private static SpellModel generateSpellModel_CastingTime(int castingTime, CastingType castingType)
        {
            SpellModel spellModel = new SpellModel()
            {
                CastingTime = castingTime,
                CastingType = castingType
            };
            return spellModel;
        }
        #endregion
        #region Range
        [TestMethod]
        [DynamicData(nameof(RangeData), DynamicDataSourceType.Method)]
        public void TestRange(SpellModel expectedSpellModel, string importData)
        {
            SpellModel actualSpellModel = _importSpell.ImportTextToSpellModel(importData);
            Assert.AreEqual(expectedSpellModel.Range, actualSpellModel.Range);
            Assert.AreEqual(expectedSpellModel.RangeType, actualSpellModel.RangeType);
        }


        private static IEnumerable<object[]> RangeData()
        {
            yield return new object[] { generateSpellModel_Range(60, RangeType.Ranged), "60 ft." };

        }

        private static SpellModel generateSpellModel_Range(int range, RangeType rangeType)
        {
            SpellModel spellModel = new SpellModel()
            {
                Range = range,
                RangeType = rangeType
            };
            return spellModel;
        }
        #endregion
        #region Components
        [TestMethod]
        [DynamicData(nameof(ComponentData), DynamicDataSourceType.Method)]
        public void TestComponents(SpellModel expectedSpellModel, string importData)
        {
            SpellModel actualSpellModel = _importSpell.ImportTextToSpellModel(importData);
            Assert.AreEqual(expectedSpellModel.IsVerbalComponent, actualSpellModel.IsVerbalComponent);
            Assert.AreEqual(expectedSpellModel.IsSomaticComponent, actualSpellModel.IsSomaticComponent);
            Assert.AreEqual(expectedSpellModel.IsMaterialComponent, actualSpellModel.IsMaterialComponent);
            Assert.AreEqual(expectedSpellModel.ComponentText, actualSpellModel.ComponentText);
        }


        private static IEnumerable<object[]> ComponentData()
        {
            yield return new object[] { generateSpellModel_Components(true, true, false, null), "V, S" };
        }

        private static SpellModel generateSpellModel_Components(bool isVerbal, bool isSomatic, bool isMaterial, string materialComponentText)
        {
            SpellModel spellModel = new SpellModel()
            {
                IsVerbalComponent = isVerbal,
                IsSomaticComponent = isSomatic,
                IsMaterialComponent = isMaterial,
                ComponentText = materialComponentText
            };
            return spellModel;
        }
        #endregion
        #region Duration
        [TestMethod]
        [DynamicData(nameof(DurationData), DynamicDataSourceType.Method)]
        public void TestDuration(SpellModel expectedSpellModel, string importData)
        {
            SpellModel actualSpellModel = _importSpell.ImportTextToSpellModel(importData);
            Assert.AreEqual(expectedSpellModel.DurationTime, actualSpellModel.DurationTime);
            Assert.AreEqual(expectedSpellModel.DurationType, actualSpellModel.DurationType);
            Assert.AreEqual(expectedSpellModel.DurationUnit, actualSpellModel.DurationUnit);
            Assert.AreEqual(expectedSpellModel.DurationText, actualSpellModel.DurationText);
        }
        private static IEnumerable<object[]> DurationData()
        {
            yield return new object[] { generateSpellModel_Duration(0, DurationType.Instantaneous, DurationUnit.None, null), "Instantaneous" };
        }

        private static SpellModel generateSpellModel_Duration(int durationTime, DurationType durationType, DurationUnit durationUnit, string durationText)
        {
            SpellModel spellModel = new SpellModel()
            {
                DurationTime = durationTime,
                DurationType = durationType,
                DurationUnit = durationUnit,
                DurationText = durationText
            };
            return spellModel;
        }
        #endregion
        #region Classes Cast By
        [TestMethod]
        [DynamicData(nameof(CharacterCastByData), DynamicDataSourceType.Method)]
        public void TestCastBy(SpellModel expectedSpellModel, string importData)
        {
            SpellModel actualSpellModel = _importSpell.ImportTextToSpellModel(importData);
            Assert.AreEqual(expectedSpellModel.CastBy, actualSpellModel.CastBy);
        }
        private static IEnumerable<object[]> CharacterCastByData()
        {
            yield return new object[] { generateSpellModel_CastBy("Cleric"), "Cleric" };
        }

        private static SpellModel generateSpellModel_CastBy(string characterClasses)
        {
            SpellModel spellModel = new SpellModel()
            {
                CastBy = characterClasses
            };
            return spellModel;
        }
        #endregion
        #region Description

        #endregion

    }
}
