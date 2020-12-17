using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Skills;
using FantasyModuleParser.NPC.UserControls.NPCTabs;
using Newtonsoft.Json;
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
        #region Private Variables
        private int _speed;
        private int _burrow;
        private int _climb;
        private int _fly;
        private bool _hover;
        private int _swim;
        private int _attributeStr;
        private int _attributeDex;
        private int _attributeCon;
        private int _attributeInt;
        private int _attributeWis;
        private int _attributeCha;
        private int _savingThrowStr;
        private int _savingThrowDex;
        private int _savingThrowCon;
        private int _savingThrowInt;
        private int _savingThrowWis;
        private int _savingThrowCha;
        private bool _savingThrowStrBool;
        private bool _savingThrowDexBool;
        private bool _savingThrowConBool;
        private bool _savingThrowIntBool;
        private bool _savingThrowWisBool;
        private bool _savingThrowChaBool;
        private int _blindsight;
        private bool _blindBeyond;
        private int _darkvision;
        private int _tremorsense;
        private int _truesight;
        private int _passivePerception;
        private string _challengeRating;
        private int _xp;
        private bool _conditionOther;
        private string _conditionOtherText;
        private int _acrobatics;
        private int _animalHandling;
        private int _arcana;
        private int _athletics;
        private int _deception;
        private int _history;
        private int _insight;
        private int _intimidation;
        private int _investigation;
        private int _medicine;
        private int _nature;
        private int _perception;
        private int _performance;
        private int _persuasion;
        private int _religion;
        private int _sleightOfHand;
        private int _stealth;
        private int _survival;
        #region Innate Spellcasting
        private bool _innateSpellcastingSection;
        private bool _markAsPsionics;
        private string _innateSpellcastingAbility;
        private int _innateSpellSaveDC;
        private int _innateSpellHitBonus;
        private string _innateComponentText;
        private string _innateAtWill;
        private string _innateOnePerDay;
        private string _innateTwoPerDay;
        private string _innateThreePerDay;
        private string _innateFourPerDay;
        private string _innateFivePerDay;
        #endregion
        #region Spellcasting
        private bool _SpellcastingSection;
        private string _SpellcastingCasterLevel;
        private string _SCSpellcastingAbility;
        private int _SpellcastingSpellSaveDC;
        private int _SpellcastingSpellHitBonus;
        private string _SpellcastingSpellClass;
        private string _FlavorText;
        private string _CantripSpells;
        private string _CantripSpellList;
        private string _FirstLevelSpells;
        private string _FirstLevelSpellList;
        private string _SecondLevelSpells;
        private string _SecondLevelSpellList;
        private string _ThirdLevelSpells;
        private string _ThirdLevelSpellList;
        private string _FourthLevelSpells;
        private string _FourthLevelSpellList;
        private string _FifthLevelSpells;
        private string _FifthLevelSpellList;
        private string _SixthLevelSpells;
        private string _SixthLevelSpellList;
        private string _SeventhLevelSpells;
        private string _SeventhLevelSpellList;
        private string _EighthLevelSpells;
        private string _EighthLevelSpellList;
        private string _NinthLevelSpells;
        private string _NinthLevelSpellList;
        private bool _MarkedSpellsCheck;
        private string _MarkedSpells;
        private string _NPCImage;
        private string _npcToken;
        #endregion

        #endregion
        #region Public Variables
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
        public int Speed { get { return _speed; } set { Set(ref _speed, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Burrow { get { return _burrow; } set { Set(ref _burrow, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Climb { get { return _climb; } set { Set(ref _climb, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Fly { get { return _fly; } set { Set(ref _fly, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool Hover { get { return _hover; } set { Set(ref _hover, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Swim { get { return _swim; } set { Set(ref _swim, value); } }
        public int AttributeStr { get { return _attributeStr; } set { Set(ref _attributeStr, value); } }
        public int AttributeDex { get { return _attributeDex; } set { Set(ref _attributeDex, value); } }
        public int AttributeCon { get { return _attributeCon; } set { Set(ref _attributeCon, value); } }
        public int AttributeInt { get { return _attributeInt; } set { Set(ref _attributeInt, value); } }
        public int AttributeWis { get { return _attributeWis; } set { Set(ref _attributeWis, value); } }
        public int AttributeCha { get { return _attributeCha; } set { Set(ref _attributeCha, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SavingThrowStr { get { return _savingThrowStr; } set { Set(ref _savingThrowStr, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SavingThrowDex { get { return _savingThrowDex; } set { Set(ref _savingThrowDex, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SavingThrowCon { get { return _savingThrowCon; } set { Set(ref _savingThrowCon, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SavingThrowInt { get { return _savingThrowInt; } set { Set(ref _savingThrowInt, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SavingThrowWis { get { return _savingThrowWis; } set { Set(ref _savingThrowWis, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SavingThrowCha { get { return _savingThrowCha; } set { Set(ref _savingThrowCha, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool SavingThrowStrBool { get { return _savingThrowStrBool; } set { Set(ref _savingThrowStrBool, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool SavingThrowDexBool { get { return _savingThrowDexBool; } set { Set(ref _savingThrowDexBool, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool SavingThrowConBool { get { return _savingThrowConBool; } set { Set(ref _savingThrowConBool, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool SavingThrowIntBool { get { return _savingThrowIntBool; } set { Set(ref _savingThrowIntBool, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool SavingThrowWisBool { get { return _savingThrowWisBool; } set { Set(ref _savingThrowWisBool, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool SavingThrowChaBool { get { return _savingThrowChaBool; } set { Set(ref _savingThrowChaBool, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Blindsight { get { return _blindsight; } set { Set(ref _blindsight, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool BlindBeyond { get { return _blindBeyond; } set { Set(ref _blindBeyond, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Darkvision { get { return _darkvision; } set { Set(ref _darkvision, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Tremorsense { get { return _tremorsense; } set { Set(ref _tremorsense, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Truesight { get { return _truesight; } set { Set(ref _truesight, value); } }
        public int PassivePerception { get { return _passivePerception; } set { Set(ref _passivePerception, value); } }
        public string ChallengeRating { get { return _challengeRating; } set { Set(ref _challengeRating, value); } }
        public int XP { get { return _xp; } set { Set(ref _xp, value); } }
        public string NPCToken { get { return _npcToken; } set { Set(ref _npcToken, value); } }
        public List<SelectableActionModel> DamageResistanceModelList { get; set; }
        public List<SelectableActionModel> DamageVulnerabilityModelList { get; set; }
        public List<SelectableActionModel> DamageImmunityModelList { get; set; }
        public List<SelectableActionModel> ConditionImmunityModelList { get; set; }
        public List<SelectableActionModel> SpecialWeaponResistanceModelList { get; set; }
        public List<SelectableActionModel> SpecialWeaponDmgResistanceModelList { get; set; }
        public List<SelectableActionModel> SpecialWeaponImmunityModelList { get; set; }
        public List<SelectableActionModel> SpecialWeaponDmgImmunityModelList { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool ConditionOther { get { return _conditionOther; } set { Set(ref _conditionOther, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string ConditionOtherText { get { return _conditionOtherText; } set { Set(ref _conditionOtherText, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Acrobatics { get { return _acrobatics; } set { Set(ref _acrobatics, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int AnimalHandling { get { return _animalHandling; } set { Set(ref _animalHandling, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Arcana { get { return _arcana; } set { Set(ref _arcana, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Athletics { get { return _athletics; } set { Set(ref _athletics, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Deception { get { return _deception; } set { Set(ref _deception, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int History { get { return _history; } set { Set(ref _history, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Insight { get { return _insight; } set { Set(ref _insight, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Intimidation { get { return _intimidation; } set { Set(ref _intimidation, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Investigation { get { return _investigation; } set { Set(ref _investigation, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Medicine { get { return _medicine; } set { Set(ref _medicine, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Nature { get { return _nature; } set { Set(ref _nature, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Perception { get { return _perception; } set { Set(ref _perception, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Performance { get { return _performance; } set { Set(ref _performance, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Persuasion { get { return _persuasion; } set { Set(ref _persuasion, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Religion { get { return _religion; } set { Set(ref _religion, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SleightOfHand { get { return _sleightOfHand; } set { Set(ref _sleightOfHand, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Stealth { get { return _stealth; } set { Set(ref _stealth, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Survival { get { return _survival; } set { Set(ref _survival, value); } }
        public ObservableCollection<LanguageModel> StandardLanguages { get; set; }
        public ObservableCollection<LanguageModel> ExoticLanguages { get; set; }
        public ObservableCollection<LanguageModel> MonstrousLanguages { get; set; }
        public ObservableCollection<LanguageModel> UserLanguages { get; set; }
        public string LanguageOptions { get; set; }
        public string LanguageOptionsText { get; set; }
        public bool Telepathy { get; set; }
        public string TelepathyRange { get; set; }
        public ObservableCollection<ActionModelBase> Traits { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool InnateSpellcastingSection { get { return _innateSpellcastingSection; } set { Set(ref _innateSpellcastingSection, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool Psionics { get { return _markAsPsionics; } set { Set(ref _markAsPsionics, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string InnateSpellcastingAbility { get { return _innateSpellcastingAbility; } set { Set(ref _innateSpellcastingAbility, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int InnateSpellSaveDC { get { return _innateSpellSaveDC; } set { Set(ref _innateSpellSaveDC, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int InnateSpellHitBonus { get { return _innateSpellHitBonus; } set { Set(ref _innateSpellHitBonus, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string ComponentText { get { return _innateComponentText; } set { Set(ref _innateComponentText, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string InnateAtWill { get { return _innateAtWill; } set { Set(ref _innateAtWill, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string FivePerDay { get { return _innateFivePerDay; } set { Set(ref _innateFivePerDay, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string FourPerDay { get { return _innateFourPerDay; } set { Set(ref _innateFourPerDay, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string ThreePerDay { get { return _innateThreePerDay; } set { Set(ref _innateThreePerDay, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string TwoPerDay { get { return _innateTwoPerDay; } set { Set(ref _innateTwoPerDay, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string OnePerDay { get { return _innateOnePerDay; } set { Set(ref _innateOnePerDay, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool SpellcastingSection { get { return _SpellcastingSection; } set { Set(ref _SpellcastingSection, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string SpellcastingCasterLevel { get { return _SpellcastingCasterLevel; } set { Set(ref _SpellcastingCasterLevel, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string SCSpellcastingAbility { get { return _SCSpellcastingAbility; } set { Set(ref _SCSpellcastingAbility, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SpellcastingSpellSaveDC { get { return _SpellcastingSpellSaveDC; } set { Set(ref _SpellcastingSpellSaveDC, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SpellcastingSpellHitBonus { get { return _SpellcastingSpellHitBonus; } set { Set(ref _SpellcastingSpellHitBonus, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string SpellcastingSpellClass { get { return _SpellcastingSpellClass; } set { Set(ref _SpellcastingSpellClass, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string FlavorText { get { return _FlavorText; } set { Set(ref _FlavorText, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string CantripSpells { get { return _CantripSpells; } set { Set(ref _CantripSpells, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string CantripSpellList { get { return _CantripSpellList; } set { Set(ref _CantripSpellList, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string FirstLevelSpells { get { return _FirstLevelSpells; } set { Set(ref _FirstLevelSpells, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string FirstLevelSpellList { get { return _FirstLevelSpellList; } set { Set(ref _FirstLevelSpellList, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string SecondLevelSpells { get { return _SecondLevelSpells; } set { Set(ref _SecondLevelSpells, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string SecondLevelSpellList { get { return _SecondLevelSpellList; } set { Set(ref _SecondLevelSpellList, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string ThirdLevelSpells { get { return _ThirdLevelSpells; } set { Set(ref _ThirdLevelSpells, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string ThirdLevelSpellList { get { return _ThirdLevelSpellList; } set { Set(ref _ThirdLevelSpellList, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string FourthLevelSpells { get { return _FourthLevelSpells; } set { Set(ref _FourthLevelSpells, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string FourthLevelSpellList { get { return _FourthLevelSpellList; } set { Set(ref _FourthLevelSpellList, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string FifthLevelSpells { get { return _FifthLevelSpells; } set { Set(ref _FifthLevelSpells, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string FifthLevelSpellList { get { return _FifthLevelSpellList; } set { Set(ref _FifthLevelSpellList, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string SixthLevelSpells { get { return _SixthLevelSpells; } set { Set(ref _SixthLevelSpells, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string SixthLevelSpellList { get { return _SixthLevelSpellList; } set { Set(ref _SixthLevelSpellList, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string SeventhLevelSpells { get { return _SeventhLevelSpells; } set { Set(ref _SeventhLevelSpells, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string SeventhLevelSpellList { get { return _SeventhLevelSpellList; } set { Set(ref _SeventhLevelSpellList, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string EighthLevelSpells { get { return _EighthLevelSpells; } set { Set(ref _EighthLevelSpells, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string EighthLevelSpellList { get { return _EighthLevelSpellList; } set { Set(ref _EighthLevelSpellList, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string NinthLevelSpells { get { return _NinthLevelSpells; } set { Set(ref _NinthLevelSpells, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string NinthLevelSpellList { get { return _NinthLevelSpellList; } set { Set(ref _NinthLevelSpellList, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool MarkedSpellsCheck { get { return _MarkedSpellsCheck; } set { Set(ref _MarkedSpellsCheck, value); } }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string MarkedSpells { get { return _MarkedSpells; } set { Set(ref _MarkedSpells, value); } }
        public string Description { get; set; }
        public string NonID { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string NPCImage { get { return _NPCImage; } set { Set(ref _NPCImage, value); } }
        public ObservableCollection<ActionModelBase> NPCActions { get; } = new ObservableCollection<ActionModelBase>();
        public ObservableCollection<LairAction> LairActions { get; } = new ObservableCollection<LairAction>();
        public ObservableCollection<LegendaryActionModel> LegendaryActions { get; } = new ObservableCollection<LegendaryActionModel>();
        public ObservableCollection<ActionModelBase> Reactions { get; } = new ObservableCollection<ActionModelBase>();
        #endregion
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
