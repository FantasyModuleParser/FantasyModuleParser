using FantasyModuleParser.NPC;

namespace FantasyModuleParser.Importer.NPC
{
    public interface IImportNPC
    {
        NPCModel ImportTextToNPCModel(string importTextContent);

        void ParseSizeAndAlignment(NPCModel npcModel, string sizeAndAlignment);
        void ParseArmorClass(NPCModel npcModel, string armorClass);
        void ParseHitPoints(NPCModel npcModel, string hitPoints);
        void ParseStatAttributeStrength(NPCModel npcModel, string statAttributeStrength);
        void ParseStatAttributeDexterity(NPCModel npcModel, string statAttributeDexterity);
        void ParseStatAttributeConstitution(NPCModel npcModel, string statAttributeConstitution);
        void ParseStatAttributeIntelligence(NPCModel npcModel, string statAttributeIntelligence);
        void ParseStatAttributeWisdom(NPCModel npcModel, string statAttributeWisdom);
        void ParseStatAttributeCharisma(NPCModel npcModel, string statAttributeCharisma);
        void ParseSpeedAttributes(NPCModel npcModel, string speedAttributes);
        void ParseSavingThrows(NPCModel npcModel, string savingThrows);
        void ParseSkillAttributes(NPCModel npcModel, string skillAttributes);
        void ParseDamageVulnerabilities(NPCModel npcModel, string damageVulnerabilites);
        void ParseConditionImmunities(NPCModel npcModel, string conditionImmunities);
        void ParseVisionAttributes(NPCModel npcModel, string visionAttributes);
        void ParseLanguages(NPCModel npcModel, string languages);
        void ParseChallengeRatingAndXP(NPCModel npcModel, string challengeRatingAndXP);
        void ParseTraits(NPCModel npcModel, string traits);
        void ParseInnateSpellCastingAttributes(NPCModel npcModel, string innateSpellcastingAttributes);
        void ParseSpellCastingAttributes(NPCModel npcModel, string spellCastingAttributes);
        void ParseStandardAction(NPCModel npcModel, string standardAction);
        void ParseReaction(NPCModel npcModel, string reaction);
        void ParseLegendaryAction(NPCModel npcModel, string legendaryAction);
    }
}
