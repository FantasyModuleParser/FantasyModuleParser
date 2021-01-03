using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Action.Enums;
using System.IO;
using System.Reflection;

namespace FantasyModuleParser.Importer.Utils.Tests
{
    [TestClass()]
    public class ImportCommonUtilsTests
    {

        private ImportCommonUtils importCommonUtils;
        private DamageProperty actualDamageProperty;

        [TestInitialize]
        public void Initialize()
        {
            importCommonUtils = new ImportCommonUtils();
        }

        #region Parse Damage Property
        [TestMethod]
        [DynamicData(nameof(DamagePropertyData), DynamicDataSourceType.Method)]
        public void Test_Parse_DamageProperty(DamageProperty expectedDamageProperty, string text)
        {
            actualDamageProperty = importCommonUtils.ParseDamageProperty(text);
            AssertDamageProperty(expectedDamageProperty, actualDamageProperty);
        }

        private void AssertDamageProperty(DamageProperty expectedDamageProperty, DamageProperty actualDamageProperty)
        {
            Assert.AreEqual(expectedDamageProperty.NumOfDice, actualDamageProperty.NumOfDice);
            Assert.AreEqual(expectedDamageProperty.DieType, actualDamageProperty.DieType);
            Assert.AreEqual(expectedDamageProperty.Bonus, actualDamageProperty.Bonus);
            Assert.AreEqual(expectedDamageProperty.DamageType, actualDamageProperty.DamageType);
        }

        private static IEnumerable<object[]> DamagePropertyData()
        {
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D4, 0, DamageType.Lightning), "(1d4) lightning" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 0, DamageType.Lightning), "(1d6) lightning" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D8, 0, DamageType.Lightning), "(1d8) lightning" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D10, 0, DamageType.Lightning), "(1d10) lightning" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D12, 0, DamageType.Lightning), "(1d12) lightning" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D20, 0, DamageType.Lightning), "(1d20) lightning" };
            yield return new object[] { generateNPCModel_DamageProperty(2, DieType.D6, 0, DamageType.Lightning), "(2d6) lightning" };
            yield return new object[] { generateNPCModel_DamageProperty(15, DieType.D6, 0, DamageType.Lightning), "(15d6) lightning" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 5, DamageType.Lightning), "(1d6 + 5) lightning" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 25, DamageType.Lightning), "(1d6 + 25) lightning" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, -10, DamageType.Lightning), "(1d6 - 10) lightning" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 0, DamageType.Bludgeoning), "(1d6) bludgeoning" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 0, DamageType.Piercing), "(1d6) piercing" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 0, DamageType.Slashing), "(1d6) slashing" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 0, DamageType.Acid), "(1d6) acid" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 0, DamageType.Cold), "(1d6) cold" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 0, DamageType.Fire), "(1d6) fire" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 0, DamageType.Force), "(1d6) force" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 0, DamageType.Lightning), "(1d6) lightning" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 0, DamageType.Necrotic), "(1d6) necrotic" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 0, DamageType.Poison), "(1d6) poison" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 0, DamageType.Psychic), "(1d6) psychic" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 0, DamageType.Radiant), "(1d6) radiant" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 0, DamageType.Thunder), "(1d6) thunder" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 0, DamageType.Thunder), "3 (1d6) thunder" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 0, DamageType.Thunder), " 3 (1d6) thunder" };
            yield return new object[] { generateNPCModel_DamageProperty(1, DieType.D6, 0, DamageType.Lightning), "3 (1d6) lightning, silver, adamantine, cold-forged iron, magic damage" };
        }
        private static DamageProperty generateNPCModel_DamageProperty(int numOfDice, DieType dieType, int bonus, DamageType damageType)
        {
            return new DamageProperty()
            {
                NumOfDice = numOfDice,
                DieType = dieType,
                Bonus = bonus,
                DamageType = damageType
            };
        }
        #endregion

        public static string GetEmbeddedResourceFileContent(string embeddedResourcePath)
        {

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@embeddedResourcePath))
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}