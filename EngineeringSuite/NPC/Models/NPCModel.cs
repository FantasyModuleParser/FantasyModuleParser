using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC
{
    public class NPCModel
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
        public int SavingThrowDex { get; set; }
        public int SavingThrowCon { get; set; }
        public int SavingThrowInt { get; set; }
        public int SavingThrowWis { get; set; }
        public int SavingThrowCha { get; set; }
        public int Blindsight { get; set; }
        public bool BlindBeyond { get; set; }
        public int Darkvision { get; set; }
        public int Tremorsense { get; set; }
        public int Truesight { get; set; }
        public int PassivePerception { get; set; }
        public int ChallengeRating { get; set; }
        public int XP { get; set; }
        public string NPCToken { get; set; }
        public List<string> DamageResistance { get; set; }
        public List<string> DamageVulnerability { get; set; }
        public List<string> DamageImmunity { get; set; }
        public List<string> ConditionImmunity { get; set; }
        public List<string> SpecialWeaponResistance { get; set; }
        public List<string> SpecialWeaponImmunity { get; set; }
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
        public List<string> StandardLanguages { get; set; }
        public List<string> ExoticLanguages { get; set; }
        public List<string> MonstrousLanguages { get; set; }
        public List<string> UserLanguages { get; set; }
        public string LanguageOptions { get; set; }
        public string LanguageOptionsText { get; set; }
        public bool Telepathy { get; set; }
        public string TelepathyRange { get; set; }
        public string Traits1 { get; set; }
        public string TraitsDesc1 { get; set; }
        public string Traits2 { get; set; }
        public string TraitsDesc2 { get; set; }
        public string Traits3 { get; set; }
        public string TraitsDesc3 { get; set; }
        public string Traits4 { get; set; }
        public string TraitsDesc4 { get; set; }
        public string Traits5 { get; set; }
        public string TraitsDesc5 { get; set; }
        public string Traits6 { get; set; }
        public string TraitsDesc6 { get; set; }
        public string Traits7 { get; set; }
        public string TraitsDesc7 { get; set; }
        public string Traits8 { get; set; }
        public string TraitsDesc8 { get; set; }
        public string Traits9 { get; set; }
        public string TraitsDesc9 { get; set; }
        public string Traits10 { get; set; }
        public string TraitsDesc10 { get; set; }
        public string Traits11 { get; set; }
        public string TraitsDesc11 { get; set; }
        public bool InnateSpellcastingSection { get; set; }
        public bool Psionics { get; set; }
        public string SpellcastingAbility { get; set; }
        public int SpellSaveDC { get; set; }
        public int SpellHitBonus { get; set; }
        public string ComponentText { get; set; }
        public string InnateAtWill { get; set; }
        public string FivePerDay { get; set; }
        public string FourPerDay { get; set; }
        public string ThreePerDay { get; set; }
        public string TwoPerDay { get; set; }
        public string OnePerDay { get; set; }
    }
}
