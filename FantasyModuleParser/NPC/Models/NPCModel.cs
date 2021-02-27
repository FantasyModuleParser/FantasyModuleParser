using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Skills;
using log4net;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace FantasyModuleParser.NPC
{
    public class NPCModel
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NPCModel));

        #region Enumerators

        public enum SkillAttributes
        {
            Acrobatics,
			[Description("Animal Handling")]
            AnimalHandling,
            Arcana,
            Athletics,
            Deception,
            History,
            Insight,
            Intimidation,
            Investigation,
            Medicine,
            Nature,
            Perception,
            Performance,
            Persuasion,
            Religion,
			[Description("Sleight Of Hand")]
            SleightOfHand,
            Stealth,
            Survival
        };

        #endregion  // Enumerators

        #region Constants

        public const string strSpaceOpenParenPlus = " (+";
        public const string strSpaceOpenParen = " (";
        public const string strCloseParen = ")";
        public const string strCloseParenColon = "):";

        #endregion  // Constants

        #region Constructors

        public NPCModel()
        {
        }

        #endregion  // Constructors

        #region Public Variables

        public NPCController NpcController { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion  // Public Variables

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
        #endregion  // Innate Spellcasting

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
        private string _FirstLevelSpellSlots;
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
		#endregion  // Spellcasting

		#endregion  // Private Variables

		#region Auto-Implemented Properties
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

        public List<SelectableActionModel> DamageResistanceModelList { get; set; }
        public List<SelectableActionModel> DamageVulnerabilityModelList { get; set; }
        public List<SelectableActionModel> DamageImmunityModelList { get; set; }
        public List<SelectableActionModel> ConditionImmunityModelList { get; set; }
        public List<SelectableActionModel> SpecialWeaponResistanceModelList { get; set; }
        public List<SelectableActionModel> SpecialWeaponDmgResistanceModelList { get; set; }
        public List<SelectableActionModel> SpecialWeaponImmunityModelList { get; set; }
        public List<SelectableActionModel> SpecialWeaponDmgImmunityModelList { get; set; }

        public ObservableCollection<LanguageModel> StandardLanguages { get; set; }
        public ObservableCollection<LanguageModel> ExoticLanguages { get; set; }
        public ObservableCollection<LanguageModel> MonstrousLanguages { get; set; }
        public ObservableCollection<LanguageModel> UserLanguages { get; set; }

        public string LanguageOptions { get; set; }
        public string LanguageOptionsText { get; set; }
        public bool Telepathy { get; set; }
        public string TelepathyRange { get; set; }
        public ObservableCollection<ActionModelBase> Traits { get; set; }

        #endregion  // Auto-Implemented Properties

        #region Properties
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

		#region Saving Throw Properties

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

		#endregion Saving Throw Properties

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

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool ConditionOther { get { return _conditionOther; } set { Set(ref _conditionOther, value); } }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string ConditionOtherText { get { return _conditionOtherText; } set { Set(ref _conditionOtherText, value); } }

        #endregion  // Properties

        #region Begin Skill Attributes Properties

        // Create the dictionary to hold all of the SkillAttributes
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Dictionary<SkillAttributes, int> skillAttributes = new Dictionary<SkillAttributes, int>();

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Acrobatics { get => _acrobatics; set => Set(ref _acrobatics, value); }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int AnimalHandling { get => _animalHandling; set => Set(ref _animalHandling, value); }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Arcana { get => _arcana; set => Set(ref _arcana, value); }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Athletics { get => _athletics; set => Set(ref _athletics, value); }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Deception { get => _deception; set => Set(ref _deception, value); }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int History { get => _history; set => Set(ref _history, value); }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Insight { get => _insight; set => Set(ref _insight, value); }

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
        public int SleightOfHand { get => _sleightOfHand; set { Set(ref _sleightOfHand, value); } }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Stealth { get => _stealth; set { Set(ref _stealth, value); } }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Survival { get => _survival; set => Set(ref _survival, value); }

        #endregion  // Skill Attributes Properties

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
        public string FirstLevelSpells { get { return _FirstLevelSpellSlots; } set { Set(ref _FirstLevelSpellSlots, value); } }

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

        // Characteristic modifier properties as strings
        public string UpdateStrengthAttributeModifier => DetermineAttributeModifierString(AttributeStr);
        public string UpdateDexterityAttributeModifier => DetermineAttributeModifierString(AttributeDex);
        public string UpdateConstitutionAttributeModifier => DetermineAttributeModifierString(AttributeCon);
        public string UpdateIntelligenceAttributeModifier => DetermineAttributeModifierString(AttributeInt);
        public string UpdateWisdomAttributeModifier => DetermineAttributeModifierString(AttributeWis);
        public string UpdateCharismaAttributeModifier => DetermineAttributeModifierString(AttributeCha);

		public string RetrieveCantripsSpellSlotsString => GetSpellSlotsString("Cantrips", CantripSpells);
		public string RetrieveFirstLevelSpellSlotsString => GetSpellSlotsString("1st", FirstLevelSpells);  // TODO checge to be FistLevelSpellSlots
		public string RetrieveSecondLevelSpellSlotsString => GetSpellSlotsString("2nd", SecondLevelSpells);
		public string RetrieveThirdLevelSpellSlotsString => GetSpellSlotsString("3rd", ThirdLevelSpells);
		public string RetrieveFourthLevelSpellSlotsString => GetSpellSlotsString("4th", FourthLevelSpells);
		public string RetrieveFifthLevelSpellSlotsString => GetSpellSlotsString("5th", FifthLevelSpells);
		public string RetrieveSixthLevelSpellSlotsString => GetSpellSlotsString("6th", SixthLevelSpells);
		public string RetrieveSeventhLevelSpellSlotsString => GetSpellSlotsString("7th", SeventhLevelSpells);
		public string RetrieveEighthLevelSpellSlotsString => GetSpellSlotsString("8th", EighthLevelSpells);
		public string RetrieveNinthLevelSpellSlotsString => GetSpellSlotsString("9th", NinthLevelSpells);
		public string RetrieveMarkedLevelSpellSlotsString => GetSpellSlotsString("* ", MarkedSpells);

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lvl"></param>
        /// <param name="spellSlotsAtLevel"></param>
        /// <returns></returns>
        public static string GetSpellSlotsString(string lvl, string spellSlotsAtLevel)
		{
            if (string.IsNullOrWhiteSpace(spellSlotsAtLevel)) { return string.Empty; }

			string strLvl = lvl.Equals("Cantrips") || lvl.Equals("* ") ? lvl : lvl + " level";

			// "1st level (" + spellListAtLevel.ToLower() + "):"
			return string.Format("{0}{1}{2}{3}", strLvl, strSpaceOpenParen, spellSlotsAtLevel.ToLower().Trim(), strCloseParenColon);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string DetermineAttributeModifierString(int value)
        {
            // formats as +1, -1, +0 (for arguments 1, -1, 0)
            return string.Format(" ({0:+#;-#;+0})", -5 + (value / 2)); ;
        }

        /// <summary>
        /// Parse each skills attributes substring and retrieve it's value
        /// </summary>
        /// <param name="skillAttributeName">The name of the skill attribute.</param>
        /// <param name="skillValue">The numerical value of the attribute.</param>
        /// <returns>Returns true if skill was parsed correctly.</returns>
        private static bool ParseAttributeStringToInt(string skillAttributeString, out string skillAttributeName, out int skillValue)
		{
            skillAttributeName = null;
            skillValue = 0;

            // For each skillAttributeValue: "Arcana +3" or "+2 Animal Handling"; split on spaces " "
            // each of these arrays should contain 2, 3 or 4 substrings
            string[] currentSkillString = skillAttributeString.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			// make sure that there are at least 2 elements here: the attribute and the number
			if (currentSkillString.Length < 2)
            {
                log.Error("A problem occured parsing skill attribute string: " + skillAttributeString);
                return false;
            }

            // is the last substring a number or not?
            // var lastItem = currentSkillString[^1];
            bool success = Int32.TryParse(currentSkillString[currentSkillString.Length - 1], out skillValue);

            // the last element is a number, this is the expected format
            if (success)
            {
                skillAttributeName = currentSkillString[0];
                // skillValue is correct as it is
                return true;
            }

            // the last element wasn't a number, so is the first element the number?
            if (Int32.TryParse(currentSkillString[0], out skillValue))
            {
                // the attribute name should be the element following the number in the currentSkillString array
                skillAttributeName = currentSkillString[1];
                // and at this point, we believe that skillValue is correct/valid
                return true;
            }

            // Int32.TryParse failed, there isn't much we can to to recover
            log.Error("A problem occured parsing skill attribute string: " + skillAttributeString);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public string GetAllSpeeeds()
		{
			return string.Format("{0}{1}{2}{3}{4}",
                string.Format("{0} ft.", this.Speed),
                this.Climb == 0 ? string.Empty : string.Format(", climb {0} ft.", this.Climb),
                this.Fly == 0 ? string.Empty : string.Format(", fly {0} ft.{1}", this.Fly, this.Hover ? " (hover)" : string.Empty),
                this.Burrow == 0 ? string.Empty : string.Format(", burrow {0} ft.", this.Burrow),
                this.Swim == 0 ? string.Empty : string.Format(", swim {0} ft.", this.Swim));
		}

		public string UpdateSavingThrowsString()
		{
            string delimiter = ", ";

            //Tuple<string, bool, int>[] tuples = {
            //    Tuple.Create("Str", SavingThrowStrBool, SavingThrowStr),
            //    Tuple.Create("Dex", SavingThrowDexBool, SavingThrowDex),
            //    Tuple.Create("Con", SavingThrowConBool, SavingThrowCon),
            //    Tuple.Create("Int", SavingThrowIntBool, SavingThrowInt),
            //    Tuple.Create("Wis", SavingThrowWisBool, SavingThrowWis),
            //    Tuple.Create("Cha", SavingThrowChaBool, SavingThrowCha)
            //};

            //string pointValues = tuples.Aggregate(string.Empty,
            //                                      (x, p) =>
            //                                       x + string.Format("{0}, {1} ", p.Item1.ToString(), p.Item2.ToString())
            //                                      (sav.isChecked || sav.value != 0 ? string.Format("Str {0:+#;-#;+0}{1}", sav.name, delimiter) : String.Empty)
            //                                      );


            //Func<(string name, bool isChecked, int value), string> concatThem = sav => (sav.isChecked || sav.value != 0 ? string.Format("Str {0:+#;-#;+0}{1}", sav.name, delimiter) : String.Empty);

            // formats as +1, -1, +0 (for arguments 1, -1, 0)
            return string.Format("{0}{1}{2}{3}{4}{5}",
                SavingThrowStrBool || SavingThrowStr != 0 ? string.Format("Str {0:+#;-#;+0}{1}", SavingThrowStr, delimiter) : String.Empty,
                SavingThrowDexBool || SavingThrowDex != 0 ? string.Format("Dex {0:+#;-#;+0}{1}", SavingThrowDex, delimiter) : String.Empty,
                SavingThrowConBool || SavingThrowCon != 0 ? string.Format("Con {0:+#;-#;+0}{1}", SavingThrowCon, delimiter) : String.Empty,
                SavingThrowIntBool || SavingThrowInt != 0 ? string.Format("Int {0:+#;-#;+0}{1}", SavingThrowInt, delimiter) : String.Empty,
                SavingThrowWisBool || SavingThrowWis != 0 ? string.Format("Wis {0:+#;-#;+0}{1}", SavingThrowWis, delimiter) : String.Empty,
                SavingThrowChaBool || SavingThrowCha != 0 ? string.Format("Cha {0:+#;-#;+0}{1}", SavingThrowCha, delimiter) : String.Empty
                ).TrimEnd(' ', ',');
        }

        /// <summary>
        /// 'Skills Acrobatics +1, Animal Handling +2, Arcana +3, Athletics +4, Deception +5, History +6, Insight +7, Intimidation +8, Investigation +9,
        ///  Medicine +10, Nature +11, Perception +12, Performance +13, Persuasion +14, Religion +15, Sleight of Hand +16, Stealth +17, Survival +18'
        /// </summary>
        /// <param name="line">The Skills string to be parsed in its entirity</param>
        public void ParseSkillAttributes(string line)
        {
            // Remove leading text "Skill "
            string skillAttributes = Regex.Replace(line.Trim(), "^Skills", String.Empty, RegexOptions.IgnoreCase).Trim();

			string[] skillAttributeArray = skillAttributes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);  // Split string on comma, ","

			foreach (string skillAttributeValue in skillAttributeArray)
            {
				if (!ParseAttributeStringToInt(skillAttributeValue, out string skillAttributeName, out int value)) { continue; }

                switch (skillAttributeName)
                {
                    case "Acrobatics":
                        Acrobatics = value;
                        break;
                    case "Animal":
                        AnimalHandling = value;
                        break;
                    case "Arcana":
                        Arcana = value;
                        break;
                    case "Athletics":
                        Athletics = value;
                        break;
                    case "Deception":
                        Deception = value;
                        break;
                    case "History":
                        History = value;
                        break;
                    case "Insight":
                        Insight = value;
                        break;
                    case "Intimidation":
                        Intimidation = value;
                        break;
                    case "Investigation":
                        Investigation = value;
                        break;
                    case "Medicine":
                        Medicine = value;
                        break;
                    case "Nature":
                        Nature = value;
                        break;
                    case "Perception":
                        Perception = value;
                        break;
                    case "Performance":
                        Performance = value;
                        break;
                    case "Persuasion":
                        Persuasion = value;
                        break;
                    case "Religion":
                        Religion = value;
                        break;
                    case "Sleight":
                        SleightOfHand = value;
                        break;
                    case "Stealth":
                        Stealth = value;
                        break;
                    case "Survival":
                        Survival = value;
                        break;
                    default:
                        // TODO: add error reporting code here
                        break;
                }
            }

        }

        /// <summary>
        /// Loop through the SkillAttributes enumeration and add the name & value to the string if the value is not 0
        /// </summary>
        /// <returns>A string representation of all the current values of the skillAttributes </returns>
        public string SkillAttributesToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (Acrobatics != 0)        { stringBuilder.Append($"Acrobatics {(Acrobatics >= 0 ? "+" : String.Empty)}{Acrobatics}, "); }
            if (AnimalHandling != 0)    { stringBuilder.Append($"Animal Handling {(AnimalHandling >= 0 ? "+" : String.Empty)}{AnimalHandling}, "); }
            if (Arcana != 0)            { stringBuilder.Append($"Arcana {(Arcana >= 0 ? "+" : String.Empty)}{Arcana}, "); }
            if (Athletics != 0)         { stringBuilder.Append($"Athletics {(Athletics >= 0 ? "+" : String.Empty)}{Athletics}, "); }
            if (Deception != 0)         { stringBuilder.Append($"Deception {(Deception >= 0 ? "+" : String.Empty)}{Deception}, "); }
            if (History != 0)           { stringBuilder.Append($"History {(History >= 0 ? "+" : String.Empty)}{History}, "); }
            if (Insight != 0)           { stringBuilder.Append($"Insight {(Insight >= 0 ? "+" : String.Empty)}{Insight}, "); }
            if (Intimidation != 0)      { stringBuilder.Append($"Intimidation {(Intimidation >= 0 ? "+" : String.Empty)}{Intimidation}, "); }
            if (Investigation != 0)     { stringBuilder.Append($"Investigation {(Investigation >= 0 ? "+" : String.Empty)}{Investigation}, "); }
            if (Medicine != 0)          { stringBuilder.Append($"Medicine {(Medicine >= 0 ? "+" : String.Empty)}{Medicine}, "); }
            if (Nature != 0)            { stringBuilder.Append($"Nature {(Nature >= 0 ? "+" : String.Empty)}{Nature}, "); }
            if (Perception != 0)        { stringBuilder.Append($"Perception {(Perception >= 0 ? "+" : String.Empty)}{Perception}, "); }
            if (Performance != 0)       { stringBuilder.Append($"Performance {(Performance >= 0 ? "+" : String.Empty)}{Performance}, "); }
            if (Persuasion != 0)        { stringBuilder.Append($"Persuasion {(Persuasion >= 0 ? "+" : String.Empty)}{Persuasion}, "); }
            if (Religion != 0)          { stringBuilder.Append($"Religion {(Religion >= 0 ? "+" : String.Empty)}{Religion}, "); }
            if (SleightOfHand != 0)     { stringBuilder.Append($"Sleight Of Hand {(SleightOfHand >= 0 ? "+" : String.Empty)}{SleightOfHand}, "); }
            if (Stealth != 0)           { stringBuilder.Append($"Stealth {(Stealth >= 0 ? "+" : String.Empty)}{Stealth}, "); }
            if (Survival != 0)          { stringBuilder.Append($"Survival {(Survival >= 0 ? "+" : String.Empty)}{Survival}, "); }

            // if (stringBuilder.Length >= 2) { stringBuilder.Remove(stringBuilder.Length - 2, 2); }

            return stringBuilder.ToString().TrimEnd(' ', ',');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public string OkToSaveToFile(object sender, RoutedEventArgs e)
        {
            // TODO each of these if blocks, has 2 lines of the same text, create a method to handle it
            string warningMessageDoNotSave = "";

            if (string.IsNullOrEmpty(NPCType))
            {
                log.Warn("NPC Type is missing from " + NPCName);
                warningMessageDoNotSave += "NPC Type is missing from " + NPCName + "\n";
            }
            if (string.IsNullOrEmpty(Size))
            {
                log.Warn("Size is missing from " + NPCName);
                warningMessageDoNotSave += "Size is missing from " + NPCName + "\n";
            }
            if (string.IsNullOrEmpty(AC))
            {
                log.Warn("AC is missing from " + NPCName);
                warningMessageDoNotSave += "AC is missing from " + NPCName + "\n";
            }
            if (string.IsNullOrEmpty(Alignment))
            {
                log.Warn("Alignment is missing from " + NPCName);
                warningMessageDoNotSave += "Alignment is missing from " + NPCName + "\n";
            }
            if (string.IsNullOrEmpty(ChallengeRating))
            {
                log.Warn("Challenge Rating is missing from " + NPCName);
                warningMessageDoNotSave += "Challenge Rating is missing from " + NPCName + "\n";
            }
            if (string.IsNullOrEmpty(HP))
            {
                log.Warn("Hit Points are missing from " + NPCName);
                warningMessageDoNotSave += "Hit Points are missing from " + NPCName + "\n";
            }
            if (string.IsNullOrEmpty(LanguageOptions))
            {
                log.Warn("Language Option (usually No special conditions) is missing from " + NPCName);
                warningMessageDoNotSave += "Language Option (usually No special conditions) is missing from " + NPCName + "\n";
            }
            if (Telepathy == true && string.IsNullOrEmpty(TelepathyRange))
            {
                log.Warn("Telepathy Range is missing from " + NPCName);
                warningMessageDoNotSave += "Telepathy Range is missing from " + NPCName + "\n";
            }
            if (InnateSpellcastingSection == true && string.IsNullOrEmpty(InnateSpellcastingAbility))
            {
                log.Warn("Innate Spellcasting Ability is missing from " + NPCName);
                warningMessageDoNotSave += "Innate Spellcasting Ability is missing from " + NPCName + "\n";
            }
            if (SpellcastingSection == true)
            {
                if (string.IsNullOrEmpty(SpellcastingCasterLevel))
				{
                    log.Warn("Spellcaster Level is missing from " + NPCName);
                    warningMessageDoNotSave += "What level spellcaster is " + NPCName + "\n";
                }
                if (string.IsNullOrEmpty(SCSpellcastingAbility))
                {
                    log.Warn("Spellcasting Ability is missing from " + NPCName);
                    warningMessageDoNotSave += "Spellcasting Ability is missing from " + NPCName + "\n";
                }
                if (string.IsNullOrEmpty(SpellcastingSpellClass))
                {
                    log.Warn("Spellcasting Class is missing from " + NPCName);
                    warningMessageDoNotSave += "What class of spells does " + NPCName + " know \n";
                }
                if (string.IsNullOrEmpty(CantripSpells) && !string.IsNullOrEmpty(CantripSpellList))
                {
                    log.Warn("Number of Cantrip slots is missing from " + NPCName);
                    warningMessageDoNotSave += "Choose how many Cantrip spell slots " + NPCName + " has \n";
                }
                if (string.IsNullOrEmpty(FirstLevelSpells) && !string.IsNullOrEmpty(FirstLevelSpellList))
                {
                    log.Warn("Number of First Level Spell slots is missing from " + NPCName);
                    warningMessageDoNotSave += "Choose how many First Level Spell slots " + NPCName + " has \n";
                }
                if (string.IsNullOrEmpty(SecondLevelSpells) && !string.IsNullOrEmpty(SecondLevelSpellList))
                {
                    log.Warn("Number of Second Level Spell slots is missing from " + NPCName);
                    warningMessageDoNotSave += "Choose how many Second Level Spell slots " + NPCName + " has \n";
                }
                if (string.IsNullOrEmpty(ThirdLevelSpells) && !string.IsNullOrEmpty(ThirdLevelSpellList))
                {
                    log.Warn("Number of Third Level Spell slots is missing from " + NPCName);
                    warningMessageDoNotSave += "Choose how many Third Level Spell slots " + NPCName + " has \n";
                }
                if (string.IsNullOrEmpty(FourthLevelSpells) && !string.IsNullOrEmpty(FourthLevelSpellList))
                {
                    log.Warn("Number of Fourth Level Spell slots is missing from " + NPCName);
                    warningMessageDoNotSave += "Choose how many Fourth Level Spell slots " + NPCName + " has \n";
                }
                if (string.IsNullOrEmpty(FifthLevelSpells) && !string.IsNullOrEmpty(FifthLevelSpellList))
                {
                    log.Warn("Number of Fifth Level Spell slots is missing from " + NPCName);
                    warningMessageDoNotSave += "Choose how many Fifth Level Spell slots " + NPCName + " has \n";
                }
                if (string.IsNullOrEmpty(SixthLevelSpells) && !string.IsNullOrEmpty(SixthLevelSpellList))
                {
                    log.Warn("Number of Sixth Level Spell slots is missing from " + NPCName);
                    warningMessageDoNotSave += "Choose how many Sixth Level Spell slots " + NPCName + " has \n";
                }
                if (string.IsNullOrEmpty(SeventhLevelSpells) && !string.IsNullOrEmpty(SeventhLevelSpellList))
                {
                    log.Warn("Number of Seventh Level Spell slots is missing from " + NPCName);
                    warningMessageDoNotSave += "Choose how many Seventh Level Spell slots " + NPCName + " has \n";
                }
                if (string.IsNullOrEmpty(EighthLevelSpells) && !string.IsNullOrEmpty(EighthLevelSpellList))
                {
                    log.Warn("Number of Eighth Level Spell slots is missing from " + NPCName);
                    warningMessageDoNotSave += "Choose how many Eighth Level Spell slots " + NPCName + " has \n";
                }
                if (string.IsNullOrEmpty(NinthLevelSpells) && !string.IsNullOrEmpty(NinthLevelSpellList))
                {
                    log.Warn("Number of Ninth Level Spell slots is missing from " + NPCName);
                    warningMessageDoNotSave += "Choose how many Ninth Level Spell slots " + NPCName + " has \n";
                }
            }
            return warningMessageDoNotSave;
        }

        #endregion  // Methods

        protected virtual void OnPropertyChanged(string propertyName)
        {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
