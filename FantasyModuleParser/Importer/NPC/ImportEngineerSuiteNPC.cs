﻿using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Models.Action;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace FantasyModuleParser.Importer.NPC
{
    public class ImportEngineerSuiteNPC
    {
        public ImportEngineerSuiteNPC()
        {

        }

        /// <summary>
        /// Parses & Imports data from .npc files generated by Engineer Suite - NPC Module by Maasq
        /// </summary>
        /// <param name="engineerSuiteNpcFileContent">The file content of an *.npc file created by the NPC Engineer module in Engineer Suite</param>
        /// <returns></returns>
        public NPCModel ParseEngineerSuiteNPCContent(string engineerSuiteNpcFileContent)
        {
            NPCModel parsedNPCModel = new NPCModel();

            string line = "";
            StringReader stringReader = new StringReader(engineerSuiteNpcFileContent);

            while((line = stringReader.ReadLine()) != null)
            {
                if (line.StartsWith("Armor Class", StringComparison.Ordinal))
                    ParseArmorClass(parsedNPCModel, line);
                if (line.StartsWith("Hit Points", StringComparison.Ordinal))
                    ParseHitPoints(parsedNPCModel, line);
                if (line.StartsWith("Speed", StringComparison.Ordinal))
                    ParseSpeedAttributes(parsedNPCModel, line);
                if (line.StartsWith("STR DEX CON INT WIS CHA", StringComparison.Ordinal))
                    ParseStatAttributes(parsedNPCModel, line);
            }

            return parsedNPCModel;
        }

        /// <summary>
        /// 'Tiny beast (devil), lawful neutral'
        /// </summary>
        public void ParseSizeAndAlignment(NPCModel npcModel, string sizeAndAlignment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 'Armor Class 16'
        /// </summary>
        public void ParseArmorClass(NPCModel npcModel, string armorClass)
        {
            if(armorClass.StartsWith("Armor Class ", StringComparison.Ordinal))
            {
                npcModel.AC = armorClass.Substring(12);
            }
        }

        /// <summary>
        /// 'Hit Points 90 (10d8 + 44)'
        /// </summary>
        public void ParseHitPoints(NPCModel npcModel, string hitPoints)
        {
            if(hitPoints.StartsWith("Hit Points"))
            {
                npcModel.HP = hitPoints.Substring(11);
            }
        }

        /// <summary>
        /// 'Speed 10 ft., burrow 20 ft., climb 30 ft., fly 40 ft. (hover), swim 50 ft.'
        /// </summary>S
        public void ParseSpeedAttributes(NPCModel npcModel, string speedAttributes)
        {
            if (speedAttributes == null || speedAttributes.Length == 0)
            {
                npcModel.Speed = 0;
                npcModel.Burrow = 0;
                npcModel.Climb = 0;
                npcModel.Fly = 0;
                npcModel.Hover = false;
                npcModel.Swim = 0;
                return;
            }
            foreach(string speedAttribute in speedAttributes.Split(','))
            {
                var trimmedSpeedAttribute = speedAttribute.Trim().ToLower(CultureInfo.CurrentCulture);
                if(trimmedSpeedAttribute.StartsWith("speed ", StringComparison.Ordinal))
                {
                    npcModel.Speed = int.Parse(trimmedSpeedAttribute.Split(' ')[1], CultureInfo.CurrentCulture);
                }
                if (trimmedSpeedAttribute.StartsWith("burrow ", StringComparison.Ordinal))
                {
                    npcModel.Burrow = int.Parse(trimmedSpeedAttribute.Split(' ')[1], CultureInfo.CurrentCulture);
                }
                if (trimmedSpeedAttribute.StartsWith("climb ", StringComparison.Ordinal))
                {
                    npcModel.Climb = int.Parse(trimmedSpeedAttribute.Split(' ')[1], CultureInfo.CurrentCulture);
                }
                if (trimmedSpeedAttribute.StartsWith("fly ", StringComparison.Ordinal))
                {
                    npcModel.Fly = int.Parse(trimmedSpeedAttribute.Split(' ')[1], CultureInfo.CurrentCulture);
                }
                if (trimmedSpeedAttribute.Contains("(hover)"))
                {
                    npcModel.Hover = true;
                }
                if (trimmedSpeedAttribute.StartsWith("swim ", StringComparison.Ordinal))
                {
                    npcModel.Swim = int.Parse(trimmedSpeedAttribute.Split(' ')[1], CultureInfo.CurrentCulture);
                }
            }
        }

        /// <summary>
        /// 'STR DEX CON INT WIS CHA 10 (+0) 11 (+0) 12 (+1) 13 (+1) 14 (+2) 15 (+2)'
        /// </summary>
        public void ParseStatAttributes(NPCModel npcModel, string statAttributes)
        {
            if(statAttributes.StartsWith("STR DEX CON INT WIS CHA")) { 
            string[] splitAttributes = statAttributes.Split(' ');
                npcModel.AttributeStr = int.Parse(splitAttributes[6], CultureInfo.CurrentCulture);
                npcModel.AttributeDex = int.Parse(splitAttributes[8], CultureInfo.CurrentCulture);
                npcModel.AttributeCon = int.Parse(splitAttributes[10], CultureInfo.CurrentCulture);
                npcModel.AttributeInt = int.Parse(splitAttributes[12], CultureInfo.CurrentCulture);
                npcModel.AttributeWis = int.Parse(splitAttributes[14], CultureInfo.CurrentCulture);
                npcModel.AttributeCha = int.Parse(splitAttributes[16], CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// 'Saving Throws Str +1, Dex +2, Con +3, Int +0, Wis +5, Cha +6'
        /// </summary>
        public void ParseSavingThrows(NPCModel npcModel, string savingThrows)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 'Skills Acrobatics +1, Animal Handling +2, Arcana +3, Athletics +4, Deception +5, History +6, Insight +7, Intimidation +8, Investigation +9, Medicine +10, Nature +11, Perception +12, Performance +13, Persuasion +14, Religion +15, Sleight of Hand +16, Stealth +17, Survival +18'
        /// </summary>
        public void ParseSkillAttributes(NPCModel npcModel, string skillAttributes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 'Damage Vulnerabilities acid, fire, lightning, poison, radiant; bludgeoning and slashing'
        /// </summary>
        public void ParseDamageVulnerabilities(NPCModel npcModel, string damageVulnerabilites)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 'Damage Resistances cold, force, necrotic, psychic, thunder from nonmagical weapons'
        /// </summary>
        public void ParseDamageResistances(NPCModel npcModel, string damageResistances)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 'Damage Immunities acid, force, poison, thunder; slashing from nonmagical weapons that aren't silvered'
        /// </summary>
        public void ParseDamageImmunities(NPCModel npcModel, string damageImmunities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 'Condition Immunities blinded, frightened, invisible, paralyzed, prone, restrained'
        /// </summary>
        public void ParseConditionImmunities(NPCModel npcModel, string conditionImmunities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 'Senses blindsight 60 ft. (blind beyond this radius), darkvision 70 ft., tremorsense 80 ft., truesight 90 ft., passive Perception 22'
        /// </summary>
        public void ParseVisionAttributes(NPCModel npcModel, string visionAttributes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 'Languages Aarakocra, Bullywug, Celestial, Common, Draconic, Elvish, Gnomish, Grell, Halfling, Ice toad, Infernal, Modron, Slaad, Sylvan, Thieves' cant, Thri-kreen, Umber hulk, telepathy 90'
        /// </summary>
        public void ParseLanguages(NPCModel npcModel, string languages)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 'Challenge 8 (3,900 XP)'
        /// </summary>
        public void ParseChallengeRatingAndXP(NPCModel npcModel, string challengeRatingAndXP)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 'Trait Number 1. Some trait goes here for flavor Anger. This NPC gets angry very, very easily Unit Test. Unit Test the third'
        /// </summary>
        public void ParseTraits(NPCModel npcModel, string traits)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 'Spellcasting. V1_npc_all is an 18th-level spellcaster. His spellcasting ability is Constitution (spell save DC 8, +12 to hit with spell attacks). V1_npc_all has the following Sorcerer spells prepared:\rCantrips (At will): Cantrips1\r1st level (9 slots): Spell 1st\r2nd level (8 slots): Spell 2nd\r3rd level (7 slots): Spell 3rd\r4th level (6 slots): Spell 4th\r5th level (5 slots): Spell 5th\r6th level (4 slots): Spell 6th\r7th level (3 slots): Spell 7th\r8th level (2 slots): Spell 8th\r9th level (1 slot): Spell 9th\r*Spell 2nd'
        /// </summary>
        public void ParseSpellCastingAttributes(NPCModel npcModel, string spellCastingAttributes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 'Multiattack. .This creature makes 3 attacks.'
        /// </summary>
        public void ParseStandardAction(NPCModel npcModel, string standardAction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 'Parry. You know what it does.. NINJA DODGE.'
        /// </summary>
        public void ParseReaction(NPCModel npcModel, string reaction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 'Options. This creature has 5 legendary actions.'
        /// </summary>
        public void ParseLegendaryAction(NPCModel npcModel, string legendaryAction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 'Options. This creature has 5 legendary actions.'
        /// </summary>
        public void ParseLairAction(NPCModel npcModel, string lairAction)
        {
            throw new NotImplementedException();
        }
    }
}
