using FantasyModuleParser.Exporters;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Spells.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FMPTests_Spells.Exporter
{
    [TestClass]
    public class ExportSpellsToModuleTest
    {
        private ModuleModel moduleModel;
        public void initializeData()
        {
            moduleModel = new ModuleModel()
            {
                Name = "SpellModule_UnitTest",
                Author = "MSUnit",
                ModFilename = "SpellModule_UnitTest",
                Category = "MSUnitCategory",
                Categories = new ObservableCollection<CategoryModel>()
            };
            CategoryModel categoryModel = new CategoryModel()
            {
                Name = "SpellCategory",
                SpellModels = new ObservableCollection<SpellModel>()
            };

            categoryModel.SpellModels.Add(new SpellModel()
            {
                SpellName = "Test Spell 1",
                SpellLevel = FantasyModuleParser.Spells.Enums.SpellLevel.First,
                SpellSchool = FantasyModuleParser.Spells.Enums.SpellSchool.Conjuration,
                Description = "A faint description of this spell is here === FIRST",
                CastBy = "Bard, Cleric, Wizard",
                CastingTime = 1,
                CastingType = FantasyModuleParser.Spells.Enums.CastingType.Action,
                DurationType = FantasyModuleParser.Spells.Enums.DurationType.Instantaneous,
                Range = 60,
                RangeType = FantasyModuleParser.Spells.Enums.RangeType.Ranged
            });

            categoryModel.SpellModels.Add(new SpellModel()
            {
                SpellName = "Test Spell 2",
                SpellLevel = FantasyModuleParser.Spells.Enums.SpellLevel.Second,
                SpellSchool = FantasyModuleParser.Spells.Enums.SpellSchool.Enchantment,
                Description = "A faint description of this spell is here --- SECOND",
                CastBy = "Ranger, Wizard",
                CastingTime = 1,
                CastingType = FantasyModuleParser.Spells.Enums.CastingType.BonusAction,
                DurationType = FantasyModuleParser.Spells.Enums.DurationType.Instantaneous,
                Range = 90,
                RangeType = FantasyModuleParser.Spells.Enums.RangeType.Ranged
            });

            moduleModel.Categories.Add(categoryModel);
        }

        [TestMethod]
        public void Test_ExportModuleWithSpells()
        {
            initializeData();

            IExporter exporter = new FantasyGroundsExporter();

            exporter.CreateModule(moduleModel);
        }
    }
}
