﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        private ImportSpellBase importSpellBase;
        [TestInitialize]
        public void Initialize()
        {
            importSpellBase = new ImportSpellBase();
           
        }

        #region Spell Level and School
        [TestMethod]
        [DynamicData(nameof(SpellLevelAndSchoolData), DynamicDataSourceType.Method)]
        public void TestSpellLevelAndSchool(SpellModel expectedSpellModel, string importData)
        {
            Assert.AreEqual(expectedSpellModel.SpellLevel, importSpellBase.ParseSpellLevel(importData));
            Assert.AreEqual(expectedSpellModel.SpellSchool, importSpellBase.parseSpellSchool(importData));
            Assert.AreEqual(expectedSpellModel.IsRitual, importSpellBase.checkIfRitual(importData));
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
            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.Third, SpellSchool.Transmutation), "3rd-level transmutation" };

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
            SpellModel actualSpellModel = new SpellModel();
            importSpellBase.ParseCastingTime(importData, actualSpellModel);
            Assert.AreEqual(expectedSpellModel.CastingTime, actualSpellModel.CastingTime);
            Assert.AreEqual(expectedSpellModel.CastingType, actualSpellModel.CastingType);
            Assert.AreEqual(expectedSpellModel.ReactionDescription, actualSpellModel.ReactionDescription);
        }


        private static IEnumerable<object[]> CastingTimeData()
        {
            yield return new object[] { generateSpellModel_CastingTime(1, CastingType.Action), "Casting Time: 1 action" };
            yield return new object[] { generateSpellModel_CastingTime(1, CastingType.BonusAction), "Casting Time: 1 bonus action" };
            yield return new object[] { generateSpellModel_CastingTime(1, CastingType.Hour), "Casting Time: 1 hour" };
            yield return new object[] { generateSpellModel_CastingTime(3, CastingType.Hour), "Casting Time: 3 hours" };
            yield return new object[] { generateSpellModel_CastingTime(1, CastingType.Minute), "Casting Time: 1 minute" };
            yield return new object[] { generateSpellModel_CastingTime(5, CastingType.Minute ), "Casting Time: 5 minutes" };
            yield return new object[] { generateSpellModel_CastingTime(1, CastingType.Reaction, "where the target can suck it"), "Casting Time: 1 reaction, where the target can suck it" };

        }

        private static SpellModel generateSpellModel_CastingTime(int castingTime, CastingType castingType)
        {
            return generateSpellModel_CastingTime(castingTime, castingType, null);
        }
        private static SpellModel generateSpellModel_CastingTime(int castingTime, CastingType castingType, string reactionDescription)
        {
            SpellModel spellModel = new SpellModel()
            {
                CastingTime = castingTime,
                CastingType = castingType,
                ReactionDescription = reactionDescription
            };
            return spellModel;
        }
        #endregion
        #region Range
        [TestMethod]
        [DynamicData(nameof(RangeData), DynamicDataSourceType.Method)]
        public void TestRange(SpellModel expectedSpellModel, string importData)
        {
            SpellModel actualSpellModel = new SpellModel();
            importSpellBase.ParseRange(importData, actualSpellModel);
            Assert.AreEqual(expectedSpellModel.Range, actualSpellModel.Range);
            Assert.AreEqual(expectedSpellModel.RangeType, actualSpellModel.RangeType);
        }


        private static IEnumerable<object[]> RangeData()
        {
            yield return new object[] { generateSpellModel_Range(60, RangeType.Ranged), "Range: 60 feet" };
            yield return new object[] { generateSpellModel_Range(90, RangeType.Ranged), "Range: 90 ft." };
            yield return new object[] { generateSpellModel_Range(120, RangeType.Ranged), "Range: 120 ft." };
            yield return new object[] { generateSpellModel_Range(0, RangeType.Self), "Range: self" };
            yield return new object[] { generateSpellModel_Range(0, RangeType.Sight), "Range: sight" };
            yield return new object[] { generateSpellModel_Range(0, RangeType.Touch), "Range: touch" };
            yield return new object[] { generateSpellModel_Range(0, RangeType.Unlimited), "Range: unlimited" };

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
            SpellModel actualSpellModel = new SpellModel();
            importSpellBase.ParseComponents(importData, actualSpellModel);
            Assert.AreEqual(expectedSpellModel.IsVerbalComponent, actualSpellModel.IsVerbalComponent);
            Assert.AreEqual(expectedSpellModel.IsSomaticComponent, actualSpellModel.IsSomaticComponent);
            Assert.AreEqual(expectedSpellModel.IsMaterialComponent, actualSpellModel.IsMaterialComponent);
            Assert.AreEqual(expectedSpellModel.ComponentText, actualSpellModel.ComponentText);
        }


        private static IEnumerable<object[]> ComponentData()
        {
            yield return new object[] { generateSpellModel_Components(true, true, false, null), "V, S" };
            yield return new object[] { generateSpellModel_Components(true, false, false, null), "V" };
            yield return new object[] { generateSpellModel_Components(false, true, false, null), "S" };
            yield return new object[] { generateSpellModel_Components(false, false, true, "main component1"), "M (main component1)" };
            yield return new object[] { generateSpellModel_Components(true, false, true, "main component2"), "V, M (main component2)" };
            yield return new object[] { generateSpellModel_Components(false, true, true, "main component3"), "S, M (main component3)" };
            yield return new object[] { generateSpellModel_Components(true, true, true, "main component4"), "V, S, M (main component4)" };
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
            SpellModel actualSpellModel = importSpellBase.ImportTextToSpellModel(importData);
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
            SpellModel actualSpellModel = importSpellBase.ImportTextToSpellModel(importData);
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