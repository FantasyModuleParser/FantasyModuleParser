using FantasyModuleParser.Importer.NPC;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Action.Enums;
using FantasyModuleParser.NPC.Models.Skills;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FantasyModuleParser.Importer.NPC.Tests
{
    [TestClass()]
    public class ImportPDFNPCFileTests
    {
        private IImportNPC _iImportNPC;
        NPCModel actualNPCModel = null;

        [TestInitialize]
        public void Initialize()
        {
            _iImportNPC = new ImportPDFNPC();
        }

        private string GetEmbeddedResourceFileContent(string embeddedResourcePath)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@embeddedResourcePath))
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        private NPCModel LoadPDFTestNPCFile()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.PDF.aarakocra_brave.txt");
            return _iImportNPC.ImportTextToNPCModel(fileContent);
        }

        [TestMethod()]
        public void Import_AarakocraBrave_Test()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.PDF.aarakocra_brave.txt");
            NPCModel actualNPCModel = _iImportNPC.ImportTextToNPCModel(fileContent);

            Assert.AreEqual("Aarakocra Brave", actualNPCModel.NPCName);
            Assert.AreEqual("Medium", actualNPCModel.Size);
            Assert.AreEqual("humanoid", actualNPCModel.NPCType);
            Assert.AreEqual("(aarakocra)", actualNPCModel.Tag);
            Assert.AreEqual("neutral good", actualNPCModel.Alignment);
            Assert.AreEqual("14", actualNPCModel.AC);
            Assert.AreEqual("78 (12d8 + 24)", actualNPCModel.HP);
            Assert.AreEqual(30, actualNPCModel.Speed);
            Assert.AreEqual(60, actualNPCModel.Fly);

            // Validate Stats
            Assert.AreEqual(10, actualNPCModel.AttributeStr);
            Assert.AreEqual(18, actualNPCModel.AttributeDex);
            Assert.AreEqual(14, actualNPCModel.AttributeCon);
            Assert.AreEqual(11, actualNPCModel.AttributeInt);
            Assert.AreEqual(13, actualNPCModel.AttributeWis);
            Assert.AreEqual(11, actualNPCModel.AttributeCha);

            // Validate Skills
            Assert.AreEqual(6, actualNPCModel.Acrobatics);
            Assert.AreEqual(5, actualNPCModel.Perception);

            // Validate Senses
            Assert.AreEqual(15, actualNPCModel.PassivePerception);

            // Validate Languages
            Skills_Aarakocra_ExoticLanguages(actualNPCModel);

            // Validate Challenge Rating
            Assert.AreEqual("3", actualNPCModel.ChallengeRating);
            Assert.AreEqual(700, actualNPCModel.XP);

            // Traits
            Assert.AreEqual(2, actualNPCModel.Traits.Count);
            Assert.AreEqual("Dive Attack", actualNPCModel.Traits[0].ActionName);
            Assert.AreEqual("Flyby", actualNPCModel.Traits[1].ActionName);
            Assert.AreEqual("If the aarakocra is flying and dives at least 30 feet straight toward a target and then hits it with a melee weapon attack, the attack deals an extra 10 (3d6) damage to the target.", actualNPCModel.Traits[0].ActionDescription);
            Assert.AreEqual("The aarakocra doesn't provoke an opportunity attack when it flies out of an enemy's reach.", actualNPCModel.Traits[1].ActionDescription);

            // Action
            Assert.AreEqual(3, actualNPCModel.NPCActions.Count);
            Assert.AreEqual("Multiattack", actualNPCModel.NPCActions[0].ActionName);
            Assert.AreEqual("Talon", actualNPCModel.NPCActions[1].ActionName);
            Assert.AreEqual("Javelin", actualNPCModel.NPCActions[2].ActionName);
            Assert.AreEqual("The aarakocra makes three melee attacks: one with its javelin and two with its talons, or two ranged attacks.", actualNPCModel.NPCActions[0].ActionDescription);
            Assert.AreEqual("Melee Weapon Attack: +6 to hit, reach 5 ft., one target. Hit: 7 (1d6 + 4) slashing damage.", actualNPCModel.NPCActions[1].ActionDescription);
            Assert.AreEqual("Melee Weapon Attack: +6 to hit, reach 5 ft., one target. Hit: 7 (1d6 + 4) piercing damage. Or Ranged Weapon Attack: +6 to hit, range 30/120 ft., one target. Hit: 7 (1d6 + 4) piercing damage.", actualNPCModel.NPCActions[2].ActionDescription);
        }

        [TestMethod()]
        public void Import_AarakocraTalonOfSyranita_Test()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.PDF.aarakocra_talon_of_syranita.txt");
            NPCModel actualNPCModel = _iImportNPC.ImportTextToNPCModel(fileContent);

            Assert.AreEqual("Aarakocra Talon of Syranita", actualNPCModel.NPCName);
            Assert.AreEqual("Medium", actualNPCModel.Size);
            Assert.AreEqual("humanoid", actualNPCModel.NPCType);
            Assert.AreEqual("(aarakocra)", actualNPCModel.Tag);
            Assert.AreEqual("neutral good", actualNPCModel.Alignment);
            Assert.AreEqual("13", actualNPCModel.AC);
            Assert.AreEqual("97 (15d8 + 30)", actualNPCModel.HP);
            Assert.AreEqual(20, actualNPCModel.Speed);
            Assert.AreEqual(50, actualNPCModel.Fly);

            // Validate Stats
            Assert.AreEqual(10, actualNPCModel.AttributeStr);
            Assert.AreEqual(16, actualNPCModel.AttributeDex);
            Assert.AreEqual(14, actualNPCModel.AttributeCon);
            Assert.AreEqual(11, actualNPCModel.AttributeInt);
            Assert.AreEqual(17, actualNPCModel.AttributeWis);
            Assert.AreEqual(11, actualNPCModel.AttributeCha);

            // Validate Skills
            Assert.AreEqual(6, actualNPCModel.Nature);
            Assert.AreEqual(9, actualNPCModel.Perception);
            Assert.AreEqual(9, actualNPCModel.Stealth);
            Assert.AreEqual(9, actualNPCModel.Survival);

            // Validate Senses
            Assert.AreEqual(19, actualNPCModel.PassivePerception);

            // Validate Languages
            Skills_Aarakocra_ExoticLanguages(actualNPCModel);

            // Validate Challenge Rating
            Assert.AreEqual("6", actualNPCModel.ChallengeRating);
            Assert.AreEqual(2300, actualNPCModel.XP);

            // Traits
            Assert.AreEqual(2, actualNPCModel.Traits.Count);
            Assert.AreEqual("Dive Attack", actualNPCModel.Traits[0].ActionName);
            Assert.AreEqual("Fury of Syranita", actualNPCModel.Traits[1].ActionName);
            Assert.AreEqual("If the aarakocra is flying and dives at least 30 feet straight toward a target and then hits it with a melee weapon attack, the attack deals an extra 7 (2d6) damage to the target.", actualNPCModel.Traits[0].ActionDescription);
            Assert.AreEqual("As a bonus action, the aarakocra can expend a spell slot to cause its melee or ranged weapon attacks to magically deal an extra 13 (3d8) lightning or thunder damage to a target on a hit. This benefit lasts until the end of the turn. If the aarakora expends a spell slot of 2nd level or higher, the extra damage increases by 1d8 for each level above 1st (maximum 6d8).", actualNPCModel.Traits[1].ActionDescription);

            // Spellcasting
            Assert.AreEqual("at will", actualNPCModel.CantripSpells);
            Assert.AreEqual("light, mending, resistance, spare the dying", actualNPCModel.CantripSpellList);
            Assert.AreEqual("4 slots", actualNPCModel.FirstLevelSpells);
            Assert.AreEqual("guiding bolt, healing word, protection from evil and good", actualNPCModel.FirstLevelSpellList);
            Assert.AreEqual("3 slots", actualNPCModel.SecondLevelSpells);
            Assert.AreEqual("augury, hold person, zone of truth", actualNPCModel.SecondLevelSpellList);
            Assert.AreEqual("3 slots", actualNPCModel.ThirdLevelSpells);
            Assert.AreEqual("beacon of hope, call lightning, mass healing word", actualNPCModel.ThirdLevelSpellList);
            Assert.AreEqual("3 slots", actualNPCModel.FourthLevelSpells);
            Assert.AreEqual("freedom of movement, ice storm", actualNPCModel.FourthLevelSpellList);
            Assert.AreEqual("1 slot", actualNPCModel.FifthLevelSpells);
            Assert.AreEqual("conjure (air) elemental", actualNPCModel.FifthLevelSpellList);
            // Action
            Assert.AreEqual(3, actualNPCModel.NPCActions.Count);
            Assert.AreEqual("Multiattack", actualNPCModel.NPCActions[0].ActionName);
            Assert.AreEqual("Talon", actualNPCModel.NPCActions[1].ActionName);
            Assert.AreEqual("Javelin", actualNPCModel.NPCActions[2].ActionName);
            Assert.AreEqual("The aarakocra makes two melee attacks or two ranged attacks.", actualNPCModel.NPCActions[0].ActionDescription);
            Assert.AreEqual("Melee Weapon Attack: +6 to hit, reach 5 ft., one target. Hit: 5 (1d4 + 3) slashing damage.", actualNPCModel.NPCActions[1].ActionDescription);
            Assert.AreEqual("Melee Weapon Attack: +6 to hit, reach 5 ft., one target. Hit: 6 (1d6 + 3) piercing damage. Or Ranged Weapon Attack: +6 to hit, range 30/120 ft., one target. Hit: 6 (1d6 + 3) piercing damage.", actualNPCModel.NPCActions[2].ActionDescription);
        }

        [TestMethod()]
        public void Import_AbolethOverseer_Test()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.PDF.aboleth_overseer.txt");
            NPCModel actualNPCModel = _iImportNPC.ImportTextToNPCModel(fileContent);

            // Valideate Innate Spellcasting (Psionics)
            Assert.AreEqual(true, actualNPCModel.InnateSpellcastingSection);
            Assert.AreEqual(true, actualNPCModel.Psionics);
            Assert.AreEqual("Intelligence", actualNPCModel.InnateSpellcastingAbility);
            Assert.AreEqual(19, actualNPCModel.InnateSpellSaveDC);
            Assert.AreEqual("hypnotic pattern, invisibility, phantasmal force", actualNPCModel.InnateAtWill);
            Assert.AreEqual("hallucinatory terrain, major image", actualNPCModel.ThreePerDay);
            Assert.AreEqual("phantasmal killer, project image", actualNPCModel.TwoPerDay);
            Assert.AreEqual("mirage arcane, psychic scream, weird, plane shift (self only)", actualNPCModel.OnePerDay);

            // Validate Standard Actions
            Assert.AreEqual(5, actualNPCModel.NPCActions.Count);
            Assert.AreEqual("Multiattack", actualNPCModel.NPCActions[0].ActionName);
            Assert.AreEqual("Tentacle", actualNPCModel.NPCActions[1].ActionName);
            Assert.AreEqual("Tail", actualNPCModel.NPCActions[2].ActionName);
            Assert.AreEqual("Enslave (3/Day)", actualNPCModel.NPCActions[3].ActionName);
            Assert.AreEqual("Fling", actualNPCModel.NPCActions[4].ActionName);

            // Validate Legendary Actions
            Assert.AreEqual(5, actualNPCModel.LegendaryActions.Count);
            Assert.AreEqual("Options", actualNPCModel.LegendaryActions[0].ActionName);
            Assert.AreEqual("Enslave", actualNPCModel.LegendaryActions[1].ActionName);
            Assert.AreEqual("Tail Swipe", actualNPCModel.LegendaryActions[2].ActionName);
            Assert.AreEqual("Psychic Drain (Costs 2 Actions)", actualNPCModel.LegendaryActions[3].ActionName);
            Assert.AreEqual("Cast a Spell (Costs 3 Actions)", actualNPCModel.LegendaryActions[4].ActionName);
            Assert.AreEqual("The aboleth can take 3 legendary actions, choosing from the options below. Only one legendary action option can be used at a time and only at the end of another creature's turn. The aboleth regains spent legendary actions at the start of its turn.", actualNPCModel.LegendaryActions[0].ActionDescription);
            Assert.AreEqual("The aboleth uses its Enslave feature.", actualNPCModel.LegendaryActions[1].ActionDescription);
            Assert.AreEqual("The aboleth makes one tail attack.", actualNPCModel.LegendaryActions[2].ActionDescription);
            Assert.AreEqual("One creature charmed by the aboleth takes 28 (8d6) psychic damage, and the aboleth regains hit points equal to the damage the creature takes.", actualNPCModel.LegendaryActions[3].ActionDescription);
            Assert.AreEqual("The aboleth casts a spell from its list of innate spells, using a spell slot as normal.", actualNPCModel.LegendaryActions[4].ActionDescription);
        }

        [TestMethod()]
        public void Import_AnimatedWoodStatue_Test()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.PDF.animated_wood_statue.txt");
            NPCModel actualNPCModel = _iImportNPC.ImportTextToNPCModel(fileContent);

            // Validate Senses
            Assert.AreEqual(60, actualNPCModel.Blindsight);
            Assert.AreEqual(true, actualNPCModel.BlindBeyond);
            Assert.AreEqual(6, actualNPCModel.PassivePerception);

            // Validate Damage Immunity

            // Validate Traits
            Assert.AreEqual(2, actualNPCModel.Traits.Count);
            // Validate Actions
            Assert.AreEqual(2, actualNPCModel.NPCActions.Count);
        }

        [TestMethod()]
        public void Import_AzerPriestOfTheFlame_Test()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.PDF.azer_priest_of_the_flame.txt");
            NPCModel actualNPCModel = _iImportNPC.ImportTextToNPCModel(fileContent);

            // Validate Reactions
            Assert.AreEqual(1, actualNPCModel.Reactions.Count);
            Assert.AreEqual("Fiery Retribution (3/day)", actualNPCModel.Reactions[0].ActionName);
            Assert.AreEqual("When a creature within 5 feet of the azer hits the azer with an attack, and the azer can see the creature, the azer can force the creature to make a DC 14 Dexterity saving throw. The creature takes 13 (3d8) fire damage on a failed saving throw, and half as much damage on a successful one.", actualNPCModel.Reactions[0].ActionDescription);

            // Validate Spellcasting
            Assert.IsTrue(actualNPCModel.SpellcastingSection);

            Assert.AreEqual("9th", actualNPCModel.SpellcastingCasterLevel);
            Assert.AreEqual("Wisdom", actualNPCModel.SCSpellcastingAbility);
            Assert.AreEqual(14, actualNPCModel.SpellcastingSpellSaveDC);
            Assert.AreEqual(6, actualNPCModel.SpellcastingSpellHitBonus);
            Assert.AreEqual("Cleric", actualNPCModel.SpellcastingSpellClass);
            Assert.AreEqual("", actualNPCModel.FlavorText);

            Assert.AreEqual("at will", actualNPCModel.CantripSpells);
            Assert.AreEqual("mending, resistance, sacred flame", actualNPCModel.CantripSpellList);

            Assert.AreEqual("4 slots", actualNPCModel.FirstLevelSpells);
            Assert.AreEqual("command, healing word", actualNPCModel.FirstLevelSpellList);

            Assert.AreEqual("3 slots", actualNPCModel.SecondLevelSpells);
            Assert.AreEqual("aid, hold person, lesser restoration", actualNPCModel.SecondLevelSpellList);

            Assert.AreEqual("3 slots", actualNPCModel.ThirdLevelSpells);
            Assert.AreEqual("dispel magic, glyph of warding, meld into stone", actualNPCModel.ThirdLevelSpellList);

            Assert.AreEqual("3 slots", actualNPCModel.FourthLevelSpells);
            Assert.AreEqual("banishment, freedom of movement, stone shape", actualNPCModel.FourthLevelSpellList);

            Assert.AreEqual("1 slot", actualNPCModel.FifthLevelSpells);
            Assert.AreEqual("flame strike", actualNPCModel.FifthLevelSpellList);

        }
        [TestMethod()]
        public void Import_DevaShaitan_Test()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.PDF.deva_shaitan.txt");
            NPCModel actualNPCModel = _iImportNPC.ImportTextToNPCModel(fileContent);

            // Validate Damage Resistances


            // Validate Traits
            Assert.AreEqual(3, actualNPCModel.Traits.Count);
            Assert.AreEqual("Fiendish Weapons", actualNPCModel.Traits[0].ActionName);
            Assert.AreEqual("Shadow Stealth", actualNPCModel.Traits[1].ActionName);
            Assert.AreEqual("Shadow Step", actualNPCModel.Traits[2].ActionName);
        }

        [TestMethod()]
        public void Import_DriderCavestalker_Test()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.PDF.drider_cavestalker.txt");
            NPCModel actualNPCModel = _iImportNPC.ImportTextToNPCModel(fileContent);

            // Validate Information
            Assert.AreEqual("Drider Cavestalker", actualNPCModel.NPCName);
            Assert.AreEqual("Large", actualNPCModel.Size);
            Assert.AreEqual("monstrosity", actualNPCModel.NPCType);
            Assert.AreEqual("chaotic evil", actualNPCModel.Alignment);

            // Validate Spellcasting
            Assert.AreEqual(true, actualNPCModel.SpellcastingSection);
            Assert.AreEqual("9th", actualNPCModel.SpellcastingCasterLevel);
            Assert.AreEqual("Wisdom", actualNPCModel.SCSpellcastingAbility);
            Assert.AreEqual(13, actualNPCModel.SpellcastingSpellSaveDC);
            Assert.AreEqual("Ranger", actualNPCModel.SpellcastingSpellClass);
            Assert.AreEqual("4 slots", actualNPCModel.FirstLevelSpells);
            Assert.AreEqual("ensnaring strike, hunter's mark", actualNPCModel.FirstLevelSpellList);
            Assert.AreEqual("3 slots", actualNPCModel.SecondLevelSpells);
            Assert.AreEqual("cordon of arrows, spike growth", actualNPCModel.SecondLevelSpellList);
            Assert.AreEqual("2 slots", actualNPCModel.ThirdLevelSpells);
            Assert.AreEqual("conjure barrage, lightning arrow", actualNPCModel.ThirdLevelSpellList);
        }

        [TestMethod()]
        public void Import_DriderSorcerer_Test()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.PDF.drider_sorcerer.txt");
            NPCModel actualNPCModel = _iImportNPC.ImportTextToNPCModel(fileContent);

            // Validate Information
            Assert.AreEqual("Drider Sorcerer", actualNPCModel.NPCName);
            Assert.AreEqual("Large", actualNPCModel.Size);
            Assert.AreEqual("monstrosity", actualNPCModel.NPCType);
            Assert.AreEqual("chaotic evil", actualNPCModel.Alignment);

            // Validate Spellcasting
            Assert.AreEqual(true, actualNPCModel.SpellcastingSection);
            Assert.AreEqual("9th", actualNPCModel.SpellcastingCasterLevel);
            Assert.AreEqual("Charisma", actualNPCModel.SCSpellcastingAbility);
            Assert.AreEqual(15, actualNPCModel.SpellcastingSpellSaveDC);
            Assert.AreEqual(7, actualNPCModel.SpellcastingSpellHitBonus);
            Assert.AreEqual("Sorcerer", actualNPCModel.SpellcastingSpellClass);
            Assert.AreEqual("4 slots", actualNPCModel.FirstLevelSpells);
            Assert.AreEqual("charm person, detect magic", actualNPCModel.FirstLevelSpellList);
            Assert.AreEqual("3 slots", actualNPCModel.SecondLevelSpells);
            Assert.AreEqual("invisibility, web", actualNPCModel.SecondLevelSpellList);
            Assert.AreEqual("3 slots", actualNPCModel.ThirdLevelSpells);
            Assert.AreEqual("dispel magic, lightning bolt, suggestion", actualNPCModel.ThirdLevelSpellList);
            Assert.AreEqual("3 slots", actualNPCModel.FourthLevelSpells);
            Assert.AreEqual("confusion, greater invisibility", actualNPCModel.FourthLevelSpellList);
            Assert.AreEqual("1 slot", actualNPCModel.FifthLevelSpells);
            Assert.AreEqual("cone of cold", actualNPCModel.FifthLevelSpellList);
        }

        [TestMethod()]
        public void Import_GreaterCouatl_Test()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.PDF.greater_couatl.txt");
            NPCModel actualNPCModel = _iImportNPC.ImportTextToNPCModel(fileContent);

            // Valideate Innate Spellcasting
            Assert.AreEqual(true, actualNPCModel.InnateSpellcastingSection);
            Assert.AreEqual(false, actualNPCModel.Psionics);
            Assert.AreEqual("Charisma", actualNPCModel.InnateSpellcastingAbility);
            Assert.AreEqual(18, actualNPCModel.InnateSpellSaveDC);
            Assert.AreEqual("detect evil and good, detect magic, detect thoughts", actualNPCModel.InnateAtWill);
            Assert.AreEqual("bless, create food and water, cure wounds, lesser restoration, protection from poison, sanctuary, shield", actualNPCModel.ThreePerDay);
            Assert.AreEqual("dawn, dream, greater restoration, scrying", actualNPCModel.TwoPerDay);
            Assert.AreEqual("divine word, resurrection, temple of the gods", actualNPCModel.OnePerDay);

            // Validate Traits
            Assert.AreEqual(5, actualNPCModel.Traits.Count);
            Assert.AreEqual("Ethereal Jaunt", actualNPCModel.Traits[0].ActionName);
            Assert.AreEqual("Flyby", actualNPCModel.Traits[1].ActionName);
            Assert.AreEqual("Magic Weapons", actualNPCModel.Traits[2].ActionName);
            Assert.AreEqual("Shielded Mind", actualNPCModel.Traits[3].ActionName);
            Assert.AreEqual("Twist Free", actualNPCModel.Traits[4].ActionName);
        }

        [TestMethod()]
        public void Import_MarilithGeneral_Test()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.PDF.marilith_general.txt");
            NPCModel actualNPCModel = _iImportNPC.ImportTextToNPCModel(fileContent);

            // Valideate Innate Spellcasting
            Assert.AreEqual(true, actualNPCModel.InnateSpellcastingSection);
            Assert.AreEqual(false, actualNPCModel.Psionics);
            Assert.AreEqual("Charisma", actualNPCModel.InnateSpellcastingAbility);
            Assert.AreEqual(19, actualNPCModel.InnateSpellSaveDC);
            Assert.AreEqual("detect magic", actualNPCModel.InnateAtWill);
            Assert.AreEqual("fly, polymorph (self only), telekinesis", actualNPCModel.ThreePerDay);
            Assert.AreEqual("blade barrier, project image", actualNPCModel.OnePerDay);

            // Validate Traits
            Assert.AreEqual(3, actualNPCModel.Traits.Count);

            //Validate Damage Resistances
            AssertDamageResistances(actualNPCModel, new string[] { "cold","fire","lightning","bludgeoning","piercing","slashing" });

            //Validate Damage Immunities
            AssertDamageImmunities(actualNPCModel, new string[] { "poison" });

            // Validate Condition Immunities
            AssertConditionImmunities(actualNPCModel, new string[] { "poisoned" });

            // Validate Actions
            Assert.AreEqual(6, actualNPCModel.NPCActions.Count);

            // Validate Legendary Actions
            Assert.AreEqual(5, actualNPCModel.LegendaryActions.Count);

            // Validate Reactions
            Assert.AreEqual(1, actualNPCModel.Reactions.Count);
        }

        [TestMethod()]
        public void Import_NalfeshneeCaptain_Test()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.PDF.nalfeshnee_captain.txt");
            NPCModel actualNPCModel = _iImportNPC.ImportTextToNPCModel(fileContent);

            // Valideate Innate Spellcasting
            Assert.AreEqual(true, actualNPCModel.InnateSpellcastingSection);
            Assert.AreEqual(false, actualNPCModel.Psionics);
            Assert.AreEqual("Intelligence", actualNPCModel.InnateSpellcastingAbility);
            Assert.AreEqual(17, actualNPCModel.InnateSpellSaveDC);
            Assert.AreEqual("detect magic", actualNPCModel.InnateAtWill);
            Assert.AreEqual("call lightning, dispel magic, polymorph (self only), slow", actualNPCModel.ThreePerDay);
            Assert.AreEqual("feeblemind", actualNPCModel.OnePerDay);

            // Validate Traits
            Assert.AreEqual(1, actualNPCModel.Traits.Count);

            // Validate Actions
            Assert.AreEqual(6, actualNPCModel.NPCActions.Count);

            // Validate Legendary Actions
            Assert.AreEqual(5, actualNPCModel.LegendaryActions.Count);
        }

        [TestMethod()]
        public void Import_SwarmOfCrawlingClaws_Test()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.PDF.swarm_of_crawling_claws.txt");
            NPCModel actualNPCModel = _iImportNPC.ImportTextToNPCModel(fileContent);

            // Validate Traits
            Assert.AreEqual(2, actualNPCModel.Traits.Count);

            // Validate Actions
            Assert.AreEqual(1, actualNPCModel.NPCActions.Count);
        }

        [TestMethod()]
        public void Import_UltimateTyrant_Test()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.PDF.ultimate_tyrant.txt");
            NPCModel actualNPCModel = _iImportNPC.ImportTextToNPCModel(fileContent);

            // Validate Innate Spellcasting
            Assert.AreEqual(true, actualNPCModel.InnateSpellcastingSection);
            Assert.AreEqual(true, actualNPCModel.Psionics);
            Assert.AreEqual("Intelligence", actualNPCModel.InnateSpellcastingAbility);
            Assert.AreEqual(20, actualNPCModel.InnateSpellSaveDC);
            Assert.AreEqual("darkness, detect magic, detect thoughts, see invisibility", actualNPCModel.InnateAtWill);
            Assert.AreEqual("banishment, dispel magic, stoneskin", actualNPCModel.ThreePerDay);
            Assert.AreEqual("globe of invulnerability, wall of force, move earth", actualNPCModel.TwoPerDay);
            Assert.AreEqual("Abi-dalzim’s horrid wilting, power word stun", actualNPCModel.OnePerDay);

            // Validate Traits
            Assert.AreEqual(1, actualNPCModel.Traits.Count);

            // Validate Actions
            Assert.AreEqual(13, actualNPCModel.NPCActions.Count);

            // Validate Legendary Actions
            Assert.AreEqual(6, actualNPCModel.LegendaryActions.Count);

            // Validate Reactions
            Assert.AreEqual(1, actualNPCModel.Reactions.Count);
        }

        [TestMethod()]
        public void Import_YochlolElder_Test()
        {
            string fileContent = GetEmbeddedResourceFileContent("FMPTests.Resources.PDF.yochlol_elder.txt");
            NPCModel actualNPCModel = _iImportNPC.ImportTextToNPCModel(fileContent);

            // Validate Information
            Assert.AreEqual("Yochlol Elder", actualNPCModel.NPCName);
            Assert.AreEqual("Large", actualNPCModel.Size);
            Assert.AreEqual("fiend", actualNPCModel.NPCType);
            Assert.AreEqual("(demon, shapechanger)", actualNPCModel.Tag);
            Assert.AreEqual("chaotic evil", actualNPCModel.Alignment);
        }

        private void AssertLanguageModelList(ObservableCollection<LanguageModel> expected, ObservableCollection<LanguageModel> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);

            // Sort both lists by ActionName so that the following loop will guarantee to work
            List<LanguageModel> expectedLanguages = expected.ToList();
            List<LanguageModel> actualLanguages = actual.ToList();

            expectedLanguages.Sort((x, y) => x.Language.CompareTo(y.Language));
            actualLanguages.Sort((x, y) => x.Language.CompareTo(y.Language));
            for (int idx = 0; idx < expectedLanguages.Count; idx++)
            {
                Assert.AreEqual(expectedLanguages[idx].Language, actualLanguages[idx].Language);
                Assert.AreEqual(expectedLanguages[idx].Selected, actualLanguages[idx].Selected);
            }
        }

        public void Skills_Aarakocra_ExoticLanguages(NPCModel actualNPCModel)
        {
            ObservableCollection<LanguageModel> expectedLanguages = new LanguageController().GenerateExoticLanguages();

            foreach (LanguageModel language in expectedLanguages)
            {
                switch (language.Language)
                {
                    case "Auran":
                        language.Selected = true;
                        break;
                }
            }
            AssertLanguageModelList(expectedLanguages, actualNPCModel.ExoticLanguages);
        }

        private void AssertSelectableActionModelList(List<SelectableActionModel> expected, List<SelectableActionModel> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);

            // Sort both lists by ActionName so that the following loop will guarantee to work
            expected.Sort((x, y) => x.ActionName.CompareTo(y.ActionName));
            actual.Sort((x, y) => x.ActionName.CompareTo(y.ActionName));
            for (int idx = 0; idx < expected.Count; idx++)
            {
                Assert.AreEqual(expected[idx].ActionName, actual[idx].ActionName);
                Assert.AreEqual(expected[idx].Selected, actual[idx].Selected);
            }
        }
        public void AssertDamageImmunities(NPCModel actualNPCModel, string[] selectedDamageImmunities)
        {
            NPCController controller = new NPCController();
            List<SelectableActionModel> expectedDamageImmunities = controller.GetSelectableActionModelList(typeof(DamageType));

            foreach (SelectableActionModel selectableActionModel in expectedDamageImmunities)
            {
                selectableActionModel.Selected = selectedDamageImmunities.Contains(selectableActionModel.ActionName.ToLower());
            }

            AssertSelectableActionModelList(expectedDamageImmunities, actualNPCModel.DamageImmunityModelList);
        }
        public void AssertDamageResistances(NPCModel actualNPCModel, string[] selectedDamageResistances)
        {
            NPCController controller = new NPCController();
            List<SelectableActionModel> expectedDamageResistances = controller.GetSelectableActionModelList(typeof(DamageType));

            foreach (SelectableActionModel selectableActionModel in expectedDamageResistances)
            {
                selectableActionModel.Selected = selectedDamageResistances.Contains(selectableActionModel.ActionName.ToLower());
            }

            AssertSelectableActionModelList(expectedDamageResistances, actualNPCModel.DamageResistanceModelList);
        }

        public void AssertDamageVulnerabilities(NPCModel actualNPCModel, string[] selectedDamageVulnerabilities)
        {
            NPCController controller = new NPCController();
            List<SelectableActionModel> expectedDamageVulnerabilities = controller.GetSelectableActionModelList(typeof(DamageType));

            foreach (SelectableActionModel selectableActionModel in expectedDamageVulnerabilities)
            {
                selectableActionModel.Selected = selectedDamageVulnerabilities.Contains(selectableActionModel.ActionName.ToLower());
            }

            AssertSelectableActionModelList(expectedDamageVulnerabilities, actualNPCModel.DamageVulnerabilityModelList);
        }

        public void AssertConditionImmunities(NPCModel actualNPCModel, string[] selectedConditionImmunities)
        {
            NPCController controller = new NPCController();
            List<SelectableActionModel> expectedConditionImmunities = controller.GetSelectableActionModelList(typeof(ConditionType));

            foreach (SelectableActionModel selectableActionModel in expectedConditionImmunities)
            {
                selectableActionModel.Selected = selectedConditionImmunities.Contains(selectableActionModel.ActionName.ToLower());
            }

            AssertSelectableActionModelList(expectedConditionImmunities, actualNPCModel.ConditionImmunityModelList);
        }

    }
}
