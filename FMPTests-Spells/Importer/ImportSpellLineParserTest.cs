using Microsoft.VisualStudio.TestTools.UnitTesting;
using FantasyModuleParser.Spells.Models;
using FantasyModuleParser.Spells.Enums;
using System.Collections.Generic;

namespace FMPTests_Spells.Importer
{
    [TestClass]
    public class ImportSpellLineParserTest
    {
        [TestInitialize]
        public void Initialize()
        {

        }

        #region Spell Level and School
        [TestMethod]
        //[DynamicData(nameof(SpellLevelAndSchoolData), DynamicDataSourceType.Method)]
        public void TestSpellLevelAndSchool()
        {
        }

        private static IEnumerable<object[]> SpellLevelAndSchoolData()
        {
            yield return new object[] { generateSpellModel_SpellLevelAndSchool(SpellLevel.Fifth, SpellSchool.Abjuration), "5th-level abjuration" };
        }

        private static SpellModel generateSpellModel_SpellLevelAndSchool(SpellLevel spellLevel, SpellSchool school)
        {
            SpellModel spellModel = new SpellModel()
            {
                SpellLevel = spellLevel,
                SpellSchool = school
            };
            return spellModel;
        }
        #endregion
    }
}
