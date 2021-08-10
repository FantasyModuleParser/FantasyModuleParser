using FantasyModuleParser.Extensions;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Action.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FantasyModuleParser.NPC.Tests
{
    [TestClass()]
    public class NPCModelTests
    {
        [TestMethod()]
        [DynamicData(nameof(DamageVulnerabilityTestData_Singular), DynamicDataSourceType.Method)]
        [DynamicData(nameof(DamageVulnerabilityTestData_BPS), DynamicDataSourceType.Method)]
        public void UpdateDamageVulnerabilitiesTest(NPCModel npcModel, string expected)
        {
            string actualVulnerabilityDescription = npcModel.UpdateDamageVulnerabilities();
            Assert.AreEqual(expected, actualVulnerabilityDescription);
        }

        private static IEnumerable<object[]> DamageVulnerabilityTestData_Singular()
        {
            foreach (DamageType damageType in Enum.GetValues(typeof(DamageType)))
            {
                yield return new object[] { new NPCModel() { DamageVulnerabilityModelList = _generateSelectableActionModelList(new List<DamageType>() { damageType }) }
                                                , damageType.ToString().ToLower() };
            }
        }

        private static IEnumerable<object[]> DamageVulnerabilityTestData_BPS()
        {
            yield return new object[] { new NPCModel() { 
                DamageVulnerabilityModelList = _generateSelectableActionModelList(new List<DamageType>() { DamageType.Bludgeoning, DamageType.Piercing }) }
                                            , "bludgeoning and piercing" };
            yield return new object[] { new NPCModel() {
                DamageVulnerabilityModelList = _generateSelectableActionModelList(new List<DamageType>() { DamageType.Bludgeoning, DamageType.Slashing }) }
                                            , "bludgeoning and slashing" };
            yield return new object[] { new NPCModel() {
                DamageVulnerabilityModelList = _generateSelectableActionModelList(new List<DamageType>() { DamageType.Bludgeoning, DamageType.Piercing, DamageType.Slashing }) }
                                            , "bludgeoning, piercing and slashing" };
        }



        [TestMethod()]
        [DynamicData(nameof(DamageImmunityTestData_Singular), DynamicDataSourceType.Method)]
        public void UpdateDamageImmunitiesTest(NPCModel npcModel, string expected)
        {
            string actualResistanceDescription = npcModel.UpdateDamageImmunities();
            Assert.AreEqual(expected, actualResistanceDescription);
        }

        private static IEnumerable<object[]> DamageImmunityTestData_Singular()
        {
            foreach (DamageType damageType in Enum.GetValues(typeof(DamageType)))
            {
                yield return new object[] { new NPCModel() { DamageImmunityModelList = _generateSelectableActionModelList(new List<DamageType>() { damageType }) }
                                                , damageType.ToString().ToLower() };
            }
        }

        [TestMethod()]
        [DynamicData(nameof(DamageResistanceTestData_Singular), DynamicDataSourceType.Method)]
        [DynamicData(nameof(DamageResistanceTestData_BPS), DynamicDataSourceType.Method)]
        [DynamicData(nameof(DamageResistanceTestData_BPS_With_MagicElement), DynamicDataSourceType.Method)]
        public void UpdateDamageResistancesTest(NPCModel npcModel, string expected)
        {
            string actualResistanceDescription = npcModel.UpdateDamageResistances();
            Assert.AreEqual(expected, actualResistanceDescription);
        }

        private static IEnumerable<object[]> DamageResistanceTestData_Singular()
        {
            foreach (DamageType damageType in Enum.GetValues(typeof(DamageType)))
            {
                yield return new object[] { new NPCModel() { DamageResistanceModelList = _generateSelectableActionModelList(new List<DamageType>() { damageType }) }
                                                , damageType.ToString().ToLower() };
            }
        }

        private static IEnumerable<object[]> DamageResistanceTestData_BPS()
        {
            yield return new object[] { new NPCModel() {
                DamageResistanceModelList = _generateSelectableActionModelList(new List<DamageType>() { DamageType.Bludgeoning, DamageType.Piercing }) }
                                            , "bludgeoning and piercing" };
            yield return new object[] { new NPCModel() {
                DamageResistanceModelList = _generateSelectableActionModelList(new List<DamageType>() { DamageType.Bludgeoning, DamageType.Slashing }) }
                                            , "bludgeoning and slashing" };
            yield return new object[] { new NPCModel() {
                DamageResistanceModelList = _generateSelectableActionModelList(new List<DamageType>() { DamageType.Bludgeoning, DamageType.Piercing, DamageType.Slashing }) }
                                            , "bludgeoning, piercing and slashing" };
        }

        private static IEnumerable<object[]> DamageResistanceTestData_BPS_With_MagicElement()
        {
            yield return new object[] { new NPCModel() {
                DamageResistanceModelList = _generateSelectableActionModelList(new List<DamageType>() { DamageType.Fire, DamageType.Bludgeoning }) }
                                            , "fire; bludgeoning" };
            yield return new object[] { new NPCModel() {
                DamageResistanceModelList = _generateSelectableActionModelList(new List<DamageType>() { DamageType.Fire, DamageType.Slashing }) }
                                            , "fire; slashing" };
            yield return new object[] { new NPCModel() {
                DamageResistanceModelList = _generateSelectableActionModelList(new List<DamageType>() { DamageType.Fire, DamageType.Piercing }) }
                                            , "fire; piercing" };
            yield return new object[] { new NPCModel() {
                DamageResistanceModelList = _generateSelectableActionModelList(new List<DamageType>() { DamageType.Fire, DamageType.Bludgeoning, DamageType.Piercing }) }
                                            , "fire; bludgeoning and piercing" };
            yield return new object[] { new NPCModel() {
                DamageResistanceModelList = _generateSelectableActionModelList(new List<DamageType>() { DamageType.Fire, DamageType.Bludgeoning, DamageType.Slashing }) }
                                            , "fire; bludgeoning and slashing" };
            yield return new object[] { new NPCModel() {
                DamageResistanceModelList = _generateSelectableActionModelList(new List<DamageType>() { DamageType.Fire, DamageType.Bludgeoning, DamageType.Piercing, DamageType.Slashing }) }
                                            , "fire; bludgeoning, piercing and slashing" };
        }




        private static List<SelectableActionModel> _generateSelectableActionModelList(List<DamageType> damageTypes)
        {
            NPCController npcController = new NPCController();
            List<SelectableActionModel> selectableActionModels =
                npcController.GetSelectableActionModelList(typeof(DamageType));

            List<string> selectedDamageTypeNames = new List<string>();
            foreach (DamageType damage in damageTypes)
            {
                selectedDamageTypeNames.Add(damage.GetDescription().ToUpper());
            }
            foreach (SelectableActionModel selectableAction in selectableActionModels)
            {
                if (selectedDamageTypeNames.Contains(selectableAction.ActionName.ToUpper()))
                {
                    selectableAction.Selected = true;
                }
            }
            return selectableActionModels;
        }
    }
}