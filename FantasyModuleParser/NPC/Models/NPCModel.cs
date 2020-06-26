using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Skills;
using FantasyModuleParser.NPC.UserControls.NPCTabs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FantasyModuleParser.NPC
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
        private int _speed;
        public int Speed { get { return _speed; } set { Set(ref _speed, value); } }
        private int _burrow;
        public int Burrow { get { return _burrow; } set { Set(ref _burrow, value); } }
        private int _climb;
        public int Climb { get { return _climb; } set { Set(ref _climb, value); } }
        private int _fly;
        public int Fly { get { return _fly; } set { Set(ref _fly, value); } }
        private bool _hover;
        public bool Hover { get { return _hover; } set { Set(ref _hover, value); } }
        private int _swim;
        public int Swim { get { return _swim; } set { Set(ref _swim, value); } }
        private int _attributeStr;
        public int AttributeStr { get { return _attributeStr; } set { Set(ref _attributeStr, value); } }
        private int _attributeDex;
        public int AttributeDex { get { return _attributeDex; } set { Set(ref _attributeDex, value); } }
        private int _attributeCon;
        public int AttributeCon { get { return _attributeCon; } set { Set(ref _attributeCon, value); } }
        private int _attributeInt;
        public int AttributeInt { get { return _attributeInt; } set { Set(ref _attributeInt, value); } }
        private int _attributeWis;
        public int AttributeWis { get { return _attributeWis; } set { Set(ref _attributeWis, value); } }
        private int _attributeCha;
        public int AttributeCha { get { return _attributeCha; } set { Set(ref _attributeCha, value); } }
        private int _savingThrowStr;
        public int SavingThrowStr { get { return _savingThrowStr; } set { Set(ref _savingThrowStr, value); } }
        private int _savingThrowDex;
        public int SavingThrowDex { get { return _savingThrowDex; } set { Set(ref _savingThrowDex, value); } }
        private int _savingThrowCon;
        public int SavingThrowCon { get { return _savingThrowCon; } set { Set(ref _savingThrowCon, value); } }
        private int _savingThrowInt;
        public int SavingThrowInt { get { return _savingThrowInt; } set { Set(ref _savingThrowInt, value); } }
        private int _savingThrowWis;
        public int SavingThrowWis { get { return _savingThrowWis; } set { Set(ref _savingThrowWis, value); } }
        private int _savingThrowCha;
        public int SavingThrowCha { get { return _savingThrowCha; } set { Set(ref _savingThrowCha, value); } }
        private bool _savingThrowStrBool;
        public bool SavingThrowStrBool { get { return _savingThrowStrBool; } set { Set(ref _savingThrowStrBool, value); } }
        private bool _savingThrowDexBool;
        public bool SavingThrowDexBool { get { return _savingThrowDexBool; } set { Set(ref _savingThrowDexBool, value); } }
        private bool _savingThrowConBool;
        public bool SavingThrowConBool { get { return _savingThrowConBool; } set { Set(ref _savingThrowConBool, value); } }
        private bool _savingThrowIntBool;
        public bool SavingThrowIntBool { get { return _savingThrowIntBool; } set { Set(ref _savingThrowIntBool, value); } }
        private bool _savingThrowWisBool;
        public bool SavingThrowWisBool { get { return _savingThrowWisBool; } set { Set(ref _savingThrowWisBool, value); } }
        private bool _savingThrowChaBool;
        public bool SavingThrowChaBool { get { return _savingThrowChaBool; } set { Set(ref _savingThrowChaBool, value); } }
        private int _blindsight;
        public int Blindsight { get { return _blindsight; } set { Set(ref _blindsight, value); } }
        private bool _blindBeyond;
        public bool BlindBeyond { get { return _blindBeyond; } set { Set(ref _blindBeyond, value); } }
        private int _darkvision;
        public int Darkvision { get { return _darkvision; } set { Set(ref _darkvision, value); } }
        private int _tremorsense;
        public int Tremorsense { get { return _tremorsense; } set { Set(ref _tremorsense, value); } }
        private int _truesight;
        public int Truesight { get { return _truesight; } set { Set(ref _truesight, value); } }
        private int _passivePerception;
        public int PassivePerception { get { return _passivePerception; } set { Set(ref _passivePerception, value); } }
        private string _challengeRating;
        public string ChallengeRating { get { return _challengeRating; } set { Set(ref _challengeRating, value); } }
        private int _xp;
        public int XP { get { return _xp; } set { Set(ref _xp, value); } }
        public string NPCToken { get; set; }
        public List<SelectableActionModel> DamageResistanceModelList { get; set; }
        public List<SelectableActionModel> DamageVulnerabilityModelList { get; set; }
        public List<SelectableActionModel> DamageImmunityModelList { get; set; }
        public List<SelectableActionModel> ConditionImmunityModelList { get; set; }
        public List<SelectableActionModel> SpecialWeaponResistanceModelList { get; set; }
        public List<SelectableActionModel> SpecialWeaponDmgResistanceModelList { get; set; }
        public List<SelectableActionModel> SpecialWeaponImmunityModelList { get; set; }
        public List<SelectableActionModel> SpecialWeaponDmgImmunityModelList { get; set; }
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
        public int Perception { get; set; }
        public int Performance { get; set; }
        public int Persuasion { get; set; }
        public int Religion { get; set; }
        public int SleightOfHand { get; set; }
        public int Stealth { get; set; }
        public int Survival { get; set; }
        public ObservableCollection<LanguageModel> StandardLanguages { get; set; }
        public ObservableCollection<LanguageModel> ExoticLanguages { get; set; }
        public ObservableCollection<LanguageModel> MonstrousLanguages { get; set; }
        public ObservableCollection<LanguageModel> UserLanguages { get; set; }
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
        public string InnateSpellcastingAbility { get; set; }
        public bool InnateSpellSaveDCCheck { get; set; }
        public int InnateSpellSaveDC { get; set; }
        public bool InnateSpellHitBonusCheck { get; set; }
        public int InnateSpellHitBonus { get; set; }
        public string ComponentText { get; set; }
        public string InnateAtWill { get; set; }
        public string FivePerDay { get; set; }
        public string FourPerDay { get; set; }
        public string ThreePerDay { get; set; }
        public string TwoPerDay { get; set; }
        public string OnePerDay { get; set; }
        public bool SpellcastingSection { get; set; }
        public string SpellcastingCasterLevel { get; set; }
        public string SCSpellcastingAbility { get; set; }
        public bool SpellcastingSpellSaveDCCheck { get; set; }
        public int SpellcastingSpellSaveDC { get; set; }
        public bool SpellcastingSpellHitBonusCheck { get; set; }
        public int SpellcastingSpellHitBonus { get; set; }
        public string SpellcastingSpellClass { get; set; }
        public string FlavorText { get; set; }
        public string CantripSpells { get; set; }
        public string CantripSpellList { get; set; }
        public string FirstLevelSpells { get; set; }
        public string FirstLevelSpellList { get; set; }
        public string SecondLevelSpells { get; set; }
        public string SecondLevelSpellList { get; set; }
        public string ThirdLevelSpells { get; set; }
        public string ThirdLevelSpellList { get; set; }
        public string FourthLevelSpells { get; set; }
        public string FourthLevelSpellList { get; set; }
        public string FifthLevelSpells { get; set; }
        public string FifthLevelSpellList { get; set; }
        public string SixthLevelSpells { get; set; }
        public string SixthLevelSpellList { get; set; }
        public string SeventhLevelSpells { get; set; }
        public string SeventhLevelSpellList { get; set; }
        public string EighthLevelSpells { get; set; }
        public string EighthLevelSpellList { get; set; }
        public string NinthLevelSpells { get; set; }
        public string NinthLevelSpellList { get; set; }
        public bool MarkedSpellsCheck { get; set; }
        public string MarkedSpells { get; set; }
        public string Description { get; set; }
        public string NonID { get; set; }
        public string NPCImage { get; set; }
        public ObservableCollection<ActionModelBase> NPCActions { get; } = new ObservableCollection<ActionModelBase>();
        public ObservableCollection<LairAction> LairActions { get; } = new ObservableCollection<LairAction>();
        public ObservableCollection<LegendaryActionModel> LegendaryActions { get; } = new ObservableCollection<LegendaryActionModel>();
        public ObservableCollection<ActionModelBase> Reactions { get; } = new ObservableCollection<ActionModelBase>();

        public NPCModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool Set<T>(ref T backingField, T value, [CallerMemberName] string propertyname = null)
        {
            // Check if the value and backing field are actualy different
            if (EqualityComparer<T>.Default.Equals(backingField, value))
            {
                return false;
            }

            // Setting the backing field and the RaisePropertyChanged
            backingField = value;
            OnPropertyChanged(propertyname);
            return true;
        }
    }
}
