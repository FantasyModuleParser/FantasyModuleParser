using FantasyModuleParser.Importer.NPC;
using FantasyModuleParser.NPC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMPTests.Importer.NPC
{
    [TestClass]
    public class ImportDnDBeyondNPCLineTests
    {
        private ImportDnDBeyondNPC _importNPC = new ImportDnDBeyondNPC();
        private NPCModel actualNPCModel = new NPCModel();
        #region Spellcasting
        [TestMethod]
        [DynamicData(nameof(SpellcastingData), DynamicDataSourceType.Method)]
        public void Test_Parse_Spellcasting_Description(NPCModel expectedNpcModel, string text)
        {
            _importNPC.ParseSpellCastingAttributes(actualNPCModel, text);
            AssertSpellcasting(expectedNpcModel, actualNPCModel);
        }

        private void AssertSpellcasting(NPCModel expectedNPCModel, NPCModel actualNPCModel)
        {
            Assert.AreEqual(expectedNPCModel.SpellcastingCasterLevel, actualNPCModel.SpellcastingCasterLevel);
            Assert.AreEqual(expectedNPCModel.SCSpellcastingAbility, actualNPCModel.SCSpellcastingAbility);
            Assert.AreEqual(expectedNPCModel.SpellcastingSpellSaveDC, actualNPCModel.SpellcastingSpellSaveDC);
            Assert.AreEqual(expectedNPCModel.SpellcastingSpellHitBonus, actualNPCModel.SpellcastingSpellHitBonus);
            Assert.AreEqual(expectedNPCModel.SpellcastingSpellClass, actualNPCModel.SpellcastingSpellClass);
            Assert.AreEqual(expectedNPCModel.FlavorText, actualNPCModel.FlavorText);
        }

        private static IEnumerable<object[]> SpellcastingData()
        {
            yield return new object[] {
                generateNPCModel_Spellcasting("18th", "Constitution", 8, 12, "Sorcerer", ""),
                "Spellcasting. V1_npc_all is an 18th-level spellcaster. His spellcasting ability is Constitution (spell save DC 8, +12 to hit with spell attacks)." +
                " V1_npc_all has the following Sorcerer spells prepared:"};
            yield return new object[] {
                generateNPCModel_Spellcasting("11th", "Wisdom", 16, 8, "Cleric", ""),
                "Spellcasting. The naga is an 11th-level spellcaster. Its spellcasting ability is Wisdom (spell save DC 16, +8 to hit with spell attacks), and it needs only verbal components to cast its spells. It has the following cleric spells prepared:"};

            // DnD Beyond - Evil Mage  (https://www.dndbeyond.com/monsters/evil-mage)
            yield return new object[] {
                generateNPCModel_Spellcasting("4th", "Intelligence", 13, 5, "Wizard", ""),
                "Spellcasting. The mage is a 4th-level spellcaster that uses Intelligence as its spellcasting ability (spell save DC 13; +5 to hit with spell attacks). The mage knows the following spells from the wizard’s spell list:" };
        }
        private static NPCModel generateNPCModel_Spellcasting(
            string spellCasterLevel, string ability, int saveDC, int hitBonus, string spellClass, string flavorText
            //string cantripSpells, string cantripSpellList, string firstSpells, string firstSpellList,
            //string secondSpells, string secondSpellList, string thirdSpells, string thirdSpellList,
            //string fourthSpells, string fourthSpellList, string fifthSpells, string fifthSpellList,
            //string sixthSpells, string sixthSpellList, string seventhSpells, string seventhSpellList,
            //string eighthSpells, string eighthSpellList, string ninthSpells, string ninthSpellList,
            //string markedSpells
            )
        {
            NPCModel npcModel = new NPCModel();

            npcModel.SpellcastingCasterLevel = spellCasterLevel;
            npcModel.SCSpellcastingAbility = ability;
            npcModel.SpellcastingSpellSaveDC = saveDC;
            npcModel.SpellcastingSpellHitBonus = hitBonus;
            npcModel.SpellcastingSpellClass = spellClass;
            npcModel.FlavorText = flavorText;
            //npcModel.CantripSpells = cantripSpellSlots;
            //npcModel.CantripSpellList = cantripSpellList;
            //npcModel.FirstLevelSpells = firstSpellSlots;
            //npcModel.FirstLevelSpellList = firstSpellList;

            //npcModel.SecondLevelSpells = secondSpellSlots;
            //npcModel.SecondLevelSpellList = secondSpellList;

            //npcModel.ThirdLevelSpells = thirdSpellSlots;
            //npcModel.ThirdLevelSpellList = thirdSpellList;

            //npcModel.FourthLevelSpells = fourthSpellSlots;
            //npcModel.FourthLevelSpellList = fourthSpellList;

            //npcModel.FifthLevelSpells = fifthSpellSlots;
            //npcModel.FifthLevelSpellList = fifthSpellList;

            //npcModel.SixthLevelSpells = sixthSpellSlots;
            //npcModel.SixthLevelSpellList = sixthSpellList;

            //npcModel.SeventhLevelSpells = seventhSpellSlots;
            //npcModel.SeventhLevelSpellList = seventhSpellList;

            //npcModel.EighthLevelSpells = eighthSpellSlots;
            //npcModel.EighthLevelSpellList = eighthSpellList;

            //npcModel.NinthLevelSpells = ninthSpellSlots;
            //npcModel.NinthLevelSpellList = ninthSpellList;
            //npcModel.MarkedSpells = markedSpells;

            return npcModel;
        }
        #endregion

        #region Legendary Actions
        /// <summary>
        /// Unit test is to ensure that exceptions are being handled correctly for Legendary Action parsing
        /// </summary>
        /// <param name="text"></param>
        /// <param name="expectException"></param>
        [TestMethod]
        [DynamicData(nameof(LegendaryActionErrorData), DynamicDataSourceType.Method)]
        public void Test_Parse_LegendaryAction_ErrorHandling(string text, bool expectException)
        {
            if (expectException)
                Assert.ThrowsException<ApplicationException>(() => _importNPC.ParseLegendaryAction(actualNPCModel, text));
            else
                // No Exception should be thrown here
                _importNPC.ParseLegendaryAction(actualNPCModel, text);
        }

        private static IEnumerable<object[]> LegendaryActionErrorData()
        {
            yield return new object[] { "Options. This creature has 5 legendary actions.", false };
            yield return new object[] { "", false };
            yield return new object[] { "Options", true };
        }
        #endregion
    }
}
