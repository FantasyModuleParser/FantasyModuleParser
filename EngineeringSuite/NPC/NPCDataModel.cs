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
        public List<string> DamageResistance { get; } = new List<string>();
        public List<string> DamageVulnerability { get; } = new List<string>();
        public List<string> DamageImmunity { get; } = new List<string>();
        public List<string> ConditionImmunity { get; } = new List<string>();
        public bool ConditionOther { get; set; }
        public string ConditionOtherText { get; set; }
        public bool ResistanceNoSpecialWeapon { get; set; }
        public bool ResistanceWeaponNonmagical { get; set; }
        public bool ResistanceWeaponNonmagicalSilvered { get; set; }
        public bool ResistanceWeaponNonmagicalAdamantine { get; set; }
        public bool ResistanceWeaponNonmagicalColdForgedIron { get; set; }
        public bool ResistanceWeaponMagical { get; set; }
        public bool ImmunityNoSpecialWeapon { get; set; }
        public bool ImmunityWeaponNonmagical { get; set; }
        public bool ImmunityWeaponNonmagicalSilvered { get; set; }
        public bool ImmunityWeaponNonmagicalAdamantine { get; set; }
        public bool ImmunityWeaponNonmagicalColdForgedIron { get; set; }
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
        public string LanguageOptions { get; set; }
        public string LanguageOptionsText { get; set; }
        public bool Telepathy { get; set; }
        public string TelepathyRange { get; set; }
    }
}
