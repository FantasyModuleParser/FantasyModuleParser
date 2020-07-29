using Microsoft.VisualStudio.TestTools.UnitTesting;
using FantasyModuleParser.NPC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyModuleParser.NPC.Controllers;

namespace FantasyModuleParser.NPC.ViewModels.Tests
{
    [TestClass()]
    public class PreviewNPCViewModelTests
    {
        private NPCModel _npcModel;

        [TestMethod()]
        // Data row is defined as speed, burrow, climb, fly, (bool) hover, swim
        [DataRow(0, 0, 0, 0, false, 0, "0 ft.")]
        [DataRow(30, 0, 0, 0, false, 0, "30 ft.")]
        [DataRow(60, 0, 0, 0, false, 0, "60 ft.")]
        [DataRow(0, 30, 0, 0, false, 0, "0 ft., burrow 30 ft.")]
        [DataRow(0, 0, 30, 0, false, 0, "0 ft., climb 30 ft.")]
        [DataRow(0, 0, 0, 30, false, 0, "0 ft., fly 30 ft.")]
        [DataRow(0, 0, 0, 30, true, 0, "0 ft., fly 30 ft. (hover)")]
        [DataRow(0, 0, 0, 0, false, 30, "0 ft., swim 30 ft.")]
        [DataRow(30, 30, 0, 0, false, 0, "30 ft., burrow 30 ft.")]
        [DataRow(30, 0, 30, 0, false, 0, "30 ft., climb 30 ft.")]
        [DataRow(30, 0, 0, 30, false, 0, "30 ft., fly 30 ft.")]
        [DataRow(30, 0, 0, 30, true, 0, "30 ft., fly 30 ft. (hover)")]
        [DataRow(30, 0, 0, 0, false, 30, "30 ft., swim 30 ft.")]
        [DataRow(30, 30, 30, 0, false, 0, "30 ft., climb 30 ft., burrow 30 ft.")]
        [DataRow(30, 30, 0, 30, false, 0, "30 ft., fly 30 ft., burrow 30 ft.")]
        [DataRow(30, 30, 0, 30, true, 0, "30 ft., fly 30 ft. (hover), burrow 30 ft.")]
        [DataRow(30, 30, 0, 0, false, 30, "30 ft., burrow 30 ft., swim 30 ft.")]
        [DataRow(30, 30, 30, 30, false, 0, "30 ft., climb 30 ft., fly 30 ft., burrow 30 ft.")]
        [DataRow(30, 30, 30, 30, true, 0, "30 ft., climb 30 ft., fly 30 ft. (hover), burrow 30 ft.")]
        [DataRow(30, 30, 30, 0, false, 30, "30 ft., climb 30 ft., burrow 30 ft., swim 30 ft.")]
        [DataRow(30, 30, 30, 30, false, 30, "30 ft., climb 30 ft., fly 30 ft., burrow 30 ft., swim 30 ft.")]
        [DataRow(30, 30, 30, 30, true, 30, "30 ft., climb 30 ft., fly 30 ft. (hover), burrow 30 ft., swim 30 ft.")]
        public void UpdateSpeedDescriptionTest(int speed, int burrow, int climb, int fly, bool hover, int swim, string expected)
        {
            _npcModel = new NPCController().InitializeNPCModel();
            _npcModel.Speed = speed;
            _npcModel.Burrow = burrow;
            _npcModel.Climb = climb;
            _npcModel.Fly = fly;
            _npcModel.Hover = hover;
            _npcModel.Swim = swim;

            // Initialize the PreviewNPCViewModel;

            PreviewNPCViewModel previewNPCViewModel = new PreviewNPCViewModel(_npcModel);

            Assert.AreEqual(expected, previewNPCViewModel.UpdateSpeedDescription());
        }

        [TestMethod]
        [DynamicData(nameof(Data), DynamicDataSourceType.Method)]
        public void UpdateSkillDescriptionTest(NPCModel npcModel, string expectedSkillDescription)
        {
            PreviewNPCViewModel previewNPCViewModel = new PreviewNPCViewModel(npcModel);

            Assert.AreEqual(expectedSkillDescription, previewNPCViewModel.SkillsDescription);
        }

        public static IEnumerable<object[]> Data()
        {
            yield return new object[] { generateNPCWithSkills(0, 0), "" };
            yield return new object[] { generateNPCWithSkills(1, 0), "Acrobatics +1" };
            yield return new object[] { generateNPCWithSkills(0, 1), "Athletics +1" };
            yield return new object[] { generateNPCWithSkills(6, 3), "Acrobatics +6, Athletics +3" };
        }

        public static NPCModel generateNPCWithSkills(int acrobatics, int athletics)
        {
            NPCModel npcModel = new NPCController().InitializeNPCModel();
            npcModel.Acrobatics = acrobatics;
            npcModel.Athletics = athletics;

            return npcModel;
        }

    }
}