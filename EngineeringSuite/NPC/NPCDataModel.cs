using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC
{
    public class NPCDataModel
    {
        public string NPCName { get; set; }
        public string Size { get; set; }
        public string NPCType { get; set; } 
        public string Tag { get; set; }
        public string Alignment { get; set; }
        public string AC { get; set; }
        public string HP { get; set; }
        public string NPCGender { get; set; }
        public bool Unique { get; set; }
        public bool NPCNamed { get; set; }
        public int Speed { get; set; }
        public int Burrow { get; set; }
        public int Climb { get; set; }
        public int Fly { get; set; }
        public bool Hover { get; set; }
        public int Swim { get; set; }
        public int AttributeStr { get; set; }
        public int AttributeDex { get; set; }
        public int AttributeCon { get; set; }
        public int AttributeInt { get; set; }
        public int AttributeWis { get; set; }
        public int AttributeCha { get; set; }
        public int SavingThrowStr { get; set; }
        public bool SavingThrowStr0 { get; set; }
        public int SavingThrowDex { get; set; }
        public bool SavingThrowDex0 { get; set; }
        public int SavingThrowCon { get; set; }
        public bool SavingThrowCon0 { get; set; }
        public int SavingThrowInt { get; set; }
        public bool SavingThrowInt0 { get; set; }
        public int SavingThrowWis { get; set; }
        public bool SavingThrowWis0 { get; set; }
        public int SavingThrowCha { get; set; }
        public bool SavingThrowCha0 { get; set; }
        public int Blindsight { get; set; }
        public bool BlindBeyond { get; set; }
        public int Darkvision { get; set; }
        public int Tremorsense { get; set; }
        public int Truesight { get; set; }
        public int PassivePerception { get; set; }
        public int ChallengeRating { get; set; }
        public int XP { get; set; }
        public string NPCToken { get; set; }
        public bool VulnerabilityAcid { get; set; }
        public bool VulnerabilityCold { get; set; }
        public bool VulnerabilityFire { get; set; }
        public bool VulnerabilityForce { get; set; }
        public bool VulnerabilityLightning { get; set; }
        public bool VulnerabilityNecrotic { get; set; }
        public bool VulnerabilityPoison { get; set; }
        public bool VulnerabilityPsychic { get; set; }
        public bool VulnerabilityRadiant { get; set; }
        public bool VulnerabilityThunder { get; set; }
        public bool VulnerabilityBludgeoning { get; set; }
        public bool VulnerabilityPiercing { get; set; }
        public bool VulnerabilitySlashing { get; set; }
        public bool ResistanceAcid { get; set; }
        public bool ResistanceCold { get; set; }
        public bool ResistanceFire { get; set; }
        public bool ResistanceForce { get; set; }
        public bool ResistanceLightning { get; set; }
        public bool ResistanceNecrotic { get; set; }
        public bool ResistancePoison { get; set; }
        public bool ResistancePsychic { get; set; }
        public bool ResistanceRadiant { get; set; }
        public bool ResistanceThunder { get; set; }
        public bool ResistanceBludgeoning { get; set; }
        public bool ResistancePiercing { get; set; }
        public bool ResistanceSlashing { get; set; }
        public bool ResistanceNoSpecialWeapon { get; set; }
        public bool ResistanceWeaponNonmagical { get; set; }
        public bool ResistanceWeaponNonmagicalSilvered { get; set; }
        public bool ResistanceWeaponNonmagicalAdamantine { get; set; }
        public bool ResistanceWeaponNonmagicalColdForgedIron { get; set; }
        public bool ResistanceWeaponMagical { get; set; }
        public bool ImmunityAcid { get; set; }
        public bool ImmunityCold { get; set; }
        public bool ImmunityFire { get; set; }
        public bool ImmunityForce { get; set; }
        public bool ImmunityLightning { get; set; }
        public bool ImmunityNecrotic { get; set; }
        public bool ImmunityPoison { get; set; }
        public bool ImmunityPsychic { get; set; }
        public bool ImmunityRadiant { get; set; }
        public bool ImmunityThunder { get; set; }
        public bool ImmunityBludgeoning { get; set; }
        public bool ImmunityPiercing { get; set; }
        public bool ImmunitySlashing { get; set; }
        public bool ImmunityNoSpecialWeapon { get; set; }
        public bool ImmunityWeaponNonmagical { get; set; }
        public bool ImmunityWeaponNonmagicalSilvered { get; set; }
        public bool ImmunityWeaponNonmagicalAdamantine { get; set; }
        public bool ImmunityWeaponNonmagicalColdForgedIron { get; set; }
        public bool ConditionBlinded { get; set; }
        public bool ConditionCharmed { get; set; }
        public bool ConditionDeafened { get; set; }
        public bool ConditionExhaustion { get; set; }
        public bool ConditionFrightened { get; set; }
        public bool ConditionGrappled { get; set; }
        public bool ConditionIncapacitated { get; set; }
        public bool ConditionInvisible { get; set; }
        public bool ConditionParalyzed { get; set; }
        public bool ConditionPetrified { get; set; }
        public bool ConditionPoisoned { get; set; }
        public bool ConditionProne { get; set; }
        public bool ConditionRestrained { get; set; }
        public bool ConditionStunned { get; set; }
        public bool ConditionUnconscious { get; set; }
        public bool ConditionOther { get; set; }
        public string ConditionOtherText { get; set; }
        public int Acrobatics { get; set; }
        public int AnimalHandling { get; set; }
        public int Arcana { get; set; }
        public int Athletics { get; set; }
        public int Deception { get; set; }
        public int History { get; set; }
        public int Insight { get; set; }
        public int Intimidation { get; set; }
        public int Investigation { get; set; }
        public int Medicine { get; set; }
        public int Nature { get; set; }
        public int Performance { get; set; }
        public int Persuasion { get; set; }
        public int Religion { get; set; }
        public int SleightOfHand { get; set; }
        public int Stealth { get; set; }
        public int Survival { get; set; }
        public List<string> StandardLanguages { get; } = new List<string>();
        public List<string> ExoticLanguages { get; } = new List<string>();
        public List<string> MonstrousLanguages { get; } = new List<string>();
        public List<string> UserLanguages { get; } = new List<string>();
    }
}
