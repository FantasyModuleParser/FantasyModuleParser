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
                generateNPCModel_Spellcasting("11th", "Wisdom", 16, 8, "cleric", ""),
                "Spellcasting. The naga is an 11th-level spellcaster. Its spellcasting ability is Wisdom (spell save DC 16, +8 to hit with spell attacks), and it needs only verbal components to cast its spells. It has the following cleric spells prepared:"};

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
            //npcModel.CantripSpells = cantripSpells;
            //npcModel.CantripSpellList = cantripSpellList;
            //npcModel.FirstLevelSpells = firstSpells;
            //npcModel.FirstLevelSpellList = firstSpellList;

            //npcModel.SecondLevelSpells = secondSpells;
            //npcModel.SecondLevelSpellList = secondSpellList;

            //npcModel.ThirdLevelSpells = thirdSpells;
            //npcModel.ThirdLevelSpellList = thirdSpellList;

            //npcModel.FourthLevelSpells = fourthSpells;
            //npcModel.FourthLevelSpellList = fourthSpellList;

            //npcModel.FifthLevelSpells = fifthSpells;
            //npcModel.FifthLevelSpellList = fifthSpellList;

            //npcModel.SixthLevelSpells = sixthSpells;
            //npcModel.SixthLevelSpellList = sixthSpellList;

            //npcModel.SeventhLevelSpells = seventhSpells;
            //npcModel.SeventhLevelSpellList = seventhSpellList;

            //npcModel.EighthLevelSpells = eighthSpells;
            //npcModel.EighthLevelSpellList = eighthSpellList;

            //npcModel.NinthLevelSpells = ninthSpells;
            //npcModel.NinthLevelSpellList = ninthSpellList;
            //npcModel.MarkedSpells = markedSpells;

            return npcModel;
        }
        #endregion
    }
}
