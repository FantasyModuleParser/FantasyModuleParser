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
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using FantasyModuleParser.NPC.Models.Action.Enums;

namespace FantasyModuleParser.NPC
{
	public class NPCModel : ModelBase
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

		public static readonly string strSpaceOpenParenPlus = " (+";
		public static readonly string strSpaceOpenParen = " (";
		public static readonly string strCloseParen = ")";
		public static readonly string strCloseParenColon = "):";
		private static readonly char[] trimCharsSpaceComma = { ' ', ',' };
		private static readonly string delimiter = ", ";

		#endregion  // Constants

		#region Constructors

		public NPCModel()
		{
			skillAttributes = Enum.GetValues(typeof(SkillAttributes)).Cast<SkillAttributes>().ToDictionary(t => t, t => 0);
		}

		#endregion  // Constructors

		#region Public Variables

		public NPCController NpcController { get; set; }
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion  // Public Variables

		#region Private Variables
		
		#region TODO speed class
		private int _speed;
		private int _burrow;
		private int _climb;
		private int _fly;
		private bool _hover;
		private int _swim;
		#endregion

		#region TODO characteristics class
		private int _attributeStr;
		private int _attributeDex;
		private int _attributeCon;
		private int _attributeInt;
		private int _attributeWis;
		private int _attributeCha;
		#endregion

		#region TODO saving throws class
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
		#endregion

		#region TODO preception class
		private int _blindsight;
		private bool _blindBeyond;
		private int _darkvision;
		private int _tremorsense;
		private int _truesight;
		private int _passivePerception;
		#endregion

		private string _challengeRating;
		private int _xp;
		private bool _conditionOther;
		private string _conditionOtherText;

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
		private string _CantripSpellSlots;
		private string _CantripSpellList;
		private string _FirstLevelSpellSlots;
		private string _FirstLevelSpellList;
		private string _SecondLevelSpellSlots;
		private string _SecondLevelSpellList;
		private string _ThirdLevelSpellSlots;
		private string _ThirdLevelSpellList;
		private string _FourthLevelSpellSlots;
		private string _FourthLevelSpellList;
		private string _FifthLevelSpellSlots;
		private string _FifthLevelSpellList;
		private string _SixthLevelSpellSlots;
		private string _SixthLevelSpellList;
		private string _SeventhLevelSpellSlots;
		private string _SeventhLevelSpellList;
		private string _EighthLevelSpellSlots;
		private string _EighthLevelSpellList;
		private string _NinthLevelSpellSlots;
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

		public List<SelectableActionModel> DamageResistanceModelList { get; set; } = new List<SelectableActionModel>();
		public List<SelectableActionModel> DamageVulnerabilityModelList { get; set; } = new List<SelectableActionModel>();
		public List<SelectableActionModel> DamageImmunityModelList { get; set; } = new List<SelectableActionModel>();
		public List<SelectableActionModel> ConditionImmunityModelList { get; set; } = new List<SelectableActionModel>();
		public List<SelectableActionModel> SpecialWeaponResistanceModelList { get; set; } = new List<SelectableActionModel>();
		public List<SelectableActionModel> SpecialWeaponDmgResistanceModelList { get; set; } = new List<SelectableActionModel>();
		public List<SelectableActionModel> SpecialWeaponImmunityModelList { get; set; } = new List<SelectableActionModel>();
		public List<SelectableActionModel> SpecialWeaponDmgImmunityModelList { get; set; } = new List<SelectableActionModel>();

		public ObservableCollection<LanguageModel> StandardLanguages { get; set; } = new ObservableCollection<LanguageModel>();
		public ObservableCollection<LanguageModel> ExoticLanguages { get; set; } = new ObservableCollection<LanguageModel>();
		public ObservableCollection<LanguageModel> MonstrousLanguages { get; set; } = new ObservableCollection<LanguageModel>();
		public ObservableCollection<LanguageModel> UserLanguages { get; set; } = new ObservableCollection<LanguageModel>();

		public string LanguageOptions { get; set; }
		public string LanguageOptionsText { get; set; }
		public bool Telepathy { get; set; }
		public string TelepathyRange { get; set; }
		public ObservableCollection<ActionModelBase> Traits { get; set; } = new ObservableCollection<ActionModelBase>();

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
		
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)] 
		public string NPCToken { get { return _npcToken; } set { Set(ref _npcToken, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public bool ConditionOther { get { return _conditionOther; } set { Set(ref _conditionOther, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string ConditionOtherText { get { return _conditionOtherText; } set { Set(ref _conditionOtherText, value); } }

		#endregion  // Properties

		#region Begin Skill Attributes Properties

		// Create the dictionary to hold all of the SkillAttributes
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public Dictionary<SkillAttributes, int> skillAttributes; // = new Dictionary<SkillAttributes, int>();  //not populated here
		
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Acrobatics //{ get => _acrobatics; set => Set(ref _acrobatics, value); }
		{
			get { return skillAttributes[SkillAttributes.Acrobatics]; }
			set { SetAttributes(ref skillAttributes, value); }
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int AnimalHandling // { get => _animalHandling; set => Set(ref _animalHandling, value); }
		{
			get { return skillAttributes[SkillAttributes.AnimalHandling]; }
			set { SetAttributes(ref skillAttributes, value); }
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Arcana //{ get => _arcana; set => Set(ref _arcana, value); }
		{
			get { return skillAttributes[SkillAttributes.Arcana]; }
			set { SetAttributes(ref skillAttributes, value); }
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Athletics //{ get => _athletics; set => Set(ref _athletics, value); }
		{
			get { return skillAttributes[SkillAttributes.Athletics]; }
			set { SetAttributes(ref skillAttributes, value); }
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Deception //{ get => _deception; set => Set(ref _deception, value); }
		{
			get { return skillAttributes[SkillAttributes.Deception]; }
			set { SetAttributes(ref skillAttributes, value); }
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int History //{ get => _history; set => Set(ref _history, value); }
		{
			get { return skillAttributes[SkillAttributes.History]; }
			set { SetAttributes(ref skillAttributes, value); }
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Insight //{ get => _insight; set => Set(ref _insight, value); }
		{
			get { return skillAttributes[SkillAttributes.Insight]; }
			set { SetAttributes(ref skillAttributes, value); }
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Intimidation //{ get { return _intimidation; } set { Set(ref _intimidation, value); } }
		{
			get { return skillAttributes[SkillAttributes.Intimidation]; }
			set { SetAttributes(ref skillAttributes, value); }
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Investigation //{ get { return _investigation; } set { Set(ref _investigation, value); } }
		{
			get { return skillAttributes[SkillAttributes.Investigation]; }
			set { SetAttributes(ref skillAttributes, value); }
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Medicine //{ get { return _medicine; } set { Set(ref _medicine, value); } }
		{
			get { return skillAttributes[SkillAttributes.Medicine]; }
			set { SetAttributes(ref skillAttributes, value); }
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Nature //{ get { return _nature; } set { Set(ref _nature, value); } }
		{
			get { return skillAttributes[SkillAttributes.Nature]; }
			set { SetAttributes(ref skillAttributes, value); }
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Perception// { get { return _perception; } set { Set(ref _perception, value); } }
		{
			get { return skillAttributes[SkillAttributes.Perception]; }
			set { SetAttributes(ref skillAttributes, value); }
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Performance //{ get { return _performance; } set { Set(ref _performance, value); } }
		{
			get { return skillAttributes[SkillAttributes.Performance]; }
			set { SetAttributes(ref skillAttributes, value); }
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Persuasion // { get { return _persuasion; } set { Set(ref _persuasion, value); } }
		{
			get { return skillAttributes[SkillAttributes.Persuasion]; }
			set { SetAttributes(ref skillAttributes, value); }
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Religion //{ get { return _religion; } set { Set(ref _religion, value); } }
		{
			get { return skillAttributes[SkillAttributes.Religion]; }
			set { SetAttributes(ref skillAttributes, value); }
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int SleightOfHand //{ get => _sleightOfHand; set { Set(ref _sleightOfHand, value); } }
		{
			get { return skillAttributes[SkillAttributes.SleightOfHand]; }
			set { SetAttributes(ref skillAttributes, value); }
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Stealth //{ get => _stealth; set { Set(ref _stealth, value); } }
		{
			get { return skillAttributes[SkillAttributes.Stealth]; }
			set { SetAttributes(ref skillAttributes, value); }
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Survival //{ get => _survival; set => Set(ref _survival, value); }
		{
			get { return skillAttributes[SkillAttributes.Survival]; }
			set { SetAttributes(ref skillAttributes, value); }
		}

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

		#region SpellLists & SpellSlots
		// Property Name has been set here due to usage w/ older save files (the C# data object was 
		// renamed from CantripSpells to CantripSpellSlots for clarity reasons).  This applies to
		// all spell slot data objects for the Casting User control
		[JsonProperty(
			DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
			PropertyName = "CantripSpells")]
		public string CantripSpellSlots { get { return _CantripSpellSlots; } set { Set(ref _CantripSpellSlots, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string CantripSpellList { get { return _CantripSpellList; } set { Set(ref _CantripSpellList, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
			PropertyName = "FirstLevelSpells")]
		public string FirstLevelSpellSlots { get { return _FirstLevelSpellSlots; } set { Set(ref _FirstLevelSpellSlots, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string FirstLevelSpellList { get { return _FirstLevelSpellList; } set { Set(ref _FirstLevelSpellList, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
			PropertyName = "SecondLevelSpells")]
		public string SecondLevelSpellSlots { get { return _SecondLevelSpellSlots; } set { Set(ref _SecondLevelSpellSlots, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string SecondLevelSpellList { get { return _SecondLevelSpellList; } set { Set(ref _SecondLevelSpellList, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
			PropertyName = "ThirdLevelSpells")]
		public string ThirdLevelSpellSlots { get { return _ThirdLevelSpellSlots; } set { Set(ref _ThirdLevelSpellSlots, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string ThirdLevelSpellList { get { return _ThirdLevelSpellList; } set { Set(ref _ThirdLevelSpellList, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
			PropertyName = "FourthLevelSpells")]
		public string FourthLevelSpellSlots { get { return _FourthLevelSpellSlots; } set { Set(ref _FourthLevelSpellSlots, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string FourthLevelSpellList { get { return _FourthLevelSpellList; } set { Set(ref _FourthLevelSpellList, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
			PropertyName = "FifthLevelSpells")]
		public string FifthLevelSpellSlots { get { return _FifthLevelSpellSlots; } set { Set(ref _FifthLevelSpellSlots, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string FifthLevelSpellList { get { return _FifthLevelSpellList; } set { Set(ref _FifthLevelSpellList, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
			PropertyName = "SixthLevelSpells")]
		public string SixthLevelSpellSlots { get { return _SixthLevelSpellSlots; } set { Set(ref _SixthLevelSpellSlots, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string SixthLevelSpellList { get { return _SixthLevelSpellList; } set { Set(ref _SixthLevelSpellList, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
			PropertyName = "SeventhLevelSpells")]
		public string SeventhLevelSpellSlots { get { return _SeventhLevelSpellSlots; } set { Set(ref _SeventhLevelSpellSlots, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string SeventhLevelSpellList { get { return _SeventhLevelSpellList; } set { Set(ref _SeventhLevelSpellList, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
			PropertyName = "EighthLevelSpells")]
		public string EighthLevelSpellSlots { get { return _EighthLevelSpellSlots; } set { Set(ref _EighthLevelSpellSlots, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string EighthLevelSpellList { get { return _EighthLevelSpellList; } set { Set(ref _EighthLevelSpellList, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
			PropertyName = "NinthLevelSpells")]
		public string NinthLevelSpellSlots { get { return _NinthLevelSpellSlots; } set { Set(ref _NinthLevelSpellSlots, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string NinthLevelSpellList { get { return _NinthLevelSpellList; } set { Set(ref _NinthLevelSpellList, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public bool MarkedSpellsCheck { get { return _MarkedSpellsCheck; } set { Set(ref _MarkedSpellsCheck, value); } }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string MarkedSpells { get { return _MarkedSpells; } set { Set(ref _MarkedSpells, value); } }
		#endregion  // SpellLists & SpellSlots

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

		public string RetrieveCantripsSpellSlotsString => GetSpellSlotsString("Cantrips", CantripSpellSlots);
		public string RetrieveFirstLevelSpellSlotsString => GetSpellSlotsString("1st", FirstLevelSpellSlots);
		public string RetrieveSecondLevelSpellSlotsString => GetSpellSlotsString("2nd", SecondLevelSpellSlots);
		public string RetrieveThirdLevelSpellSlotsString => GetSpellSlotsString("3rd", ThirdLevelSpellSlots);
		public string RetrieveFourthLevelSpellSlotsString => GetSpellSlotsString("4th", FourthLevelSpellSlots);
		public string RetrieveFifthLevelSpellSlotsString => GetSpellSlotsString("5th", FifthLevelSpellSlots);
		public string RetrieveSixthLevelSpellSlotsString => GetSpellSlotsString("6th", SixthLevelSpellSlots);
		public string RetrieveSeventhLevelSpellSlotsString => GetSpellSlotsString("7th", SeventhLevelSpellSlots);
		public string RetrieveEighthLevelSpellSlotsString => GetSpellSlotsString("8th", EighthLevelSpellSlots);
		public string RetrieveNinthLevelSpellSlotsString => GetSpellSlotsString("9th", NinthLevelSpellSlots);
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
			return string.Format("{0} ({1:+#;-#;+0})", value, -5 + (value / 2));
		}

		/// <summary>
		/// Parse each skills attributes substring and retrieve it's value
		/// </summary>
		/// <param name="skillAttributeName">out: The name of the skill attribute.</param>
		/// <param name="skillValue">out: The numerical value of the attribute.</param>
		/// <returns>Returns true if skill was parsed correctly.</returns>
		private static bool ParseAttributeStringToInt(string skillAttributeString, out string skillAttributeName, out int skillValue)
		{
			skillAttributeName = null;
			skillValue = 0;
			string problem = "A problem occured parsing skill attribute string: ";

			// For each skillAttributeValue: "Arcana +3" or "+2 Animal Handling"; split on spaces " "
			// each of these arrays should contain 2, 3 or 4 substrings
			string[] currentSkillString = skillAttributeString.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			// make sure that there are at least 2 elements here: the attribute and the number
			if (currentSkillString.Length < 2)
			{
				log.Error(problem + skillAttributeString);
				return false;
			}

			// is the last substring a number or not?
			// var lastItem = currentSkillString[^1];
			bool success = Int32.TryParse(currentSkillString[currentSkillString.Length - 1], out skillValue);

			// the last element is a number, this is the expected format
			if (success)
			{
				currentSkillString = currentSkillString.Where((o, i) => i != currentSkillString.Length - 1).ToArray();
				skillAttributeName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Join(" ", currentSkillString)).Replace(" ", "");
				// skillValue is correct as it is
				return true;
			}

			// the last element wasn't a number, so is the first element the number?
			if (Int32.TryParse(currentSkillString[0], out skillValue))
			{
				// the attribute name should be the element(s) following the number in the currentSkillString array
				skillAttributeName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(skillAttributeString.Trim().Split(null, 2)[1]).Replace(" ", "");

				// and at this point, we believe that skillValue is correct/valid
				return true;
			}

			// Int32.TryParse failed, there isn't much we can do to recover
			log.Error(problem + skillAttributeString);
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string UpdateChallengeRating => string.Format("{0} ({1} XP)", this.ChallengeRating, this.XP);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetAllSpeeds()
		{
			return string.Format("{0}{1}{2}{3}{4}",
				string.Format("{0} ft.", this.Speed),
				this.Climb == 0 ? string.Empty : string.Format(", climb {0} ft.", this.Climb),
				this.Fly == 0 ? string.Empty : string.Format(", fly {0} ft.{1}", this.Fly, this.Hover ? " (hover)" : string.Empty),
				this.Burrow == 0 ? string.Empty : string.Format(", burrow {0} ft.", this.Burrow),
				this.Swim == 0 ? string.Empty : string.Format(", swim {0} ft.", this.Swim));
		}

		/// <summary>
		/// For all the saving throws, format and concatenate them as a single string.
		/// If the saving throw checkbox is checked (usually used when the saving throw is 0 (zero)) or the saving
		/// throw value is != 0, then add that saving throw substring to the return string under construction
		/// </summary>
		/// <returns>The saving throws as a comma delimtered string, like "Dex +0, Int -2, Wis +11" </returns>
		public string UpdateSavingThrowsString()
		{
			// {0:+#;-#;+0} formats the string as +1, -1, +0 (for arguments 1, -1, 0)
			// use String Builder, since we know the max size, init the SB with that size: 8 chars per saving throw x up to 6 savingThrows
			(string name, bool isChecked, int value)[] threeUple = {
				("Str", this.SavingThrowStrBool, this.SavingThrowStr),
				("Dex", this.SavingThrowDexBool, this.SavingThrowDex),
				("Con", this.SavingThrowConBool, this.SavingThrowCon),
				("Int", this.SavingThrowIntBool, this.SavingThrowInt),
				("Wis", this.SavingThrowWisBool, this.SavingThrowWis),
				("Cha", this.SavingThrowChaBool, this.SavingThrowCha)
			};

			return threeUple.Aggregate(new StringBuilder(48), (sb, sav) => sb.Append(sav.isChecked || sav.value != 0 ?
				string.Format("{0} {1:+#;-#;+0}{2}", sav.name, sav.value, delimiter) : string.Empty)).ToString().Trim().TrimEnd(trimCharsSpaceComma);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string UpdateDamageVulnerabilities()
        {
            //	return this.DamageVulnerabilityModelList.Aggregate(new StringBuilder(), (sb, dv) =>
            //	sb.Append(dv.Selected == true ? dv.ActionName + delimiter : string.Empty)).ToString().Trim().TrimEnd(trimCharsSpaceComma);
            return _generateDamageTypeBaseDescription(this.DamageVulnerabilityModelList).ToString().Trim().TrimEnd(trimCharsSpaceComma);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string UpdateConditionImmunities()
		{
			StringBuilder foo = this.ConditionImmunityModelList.Aggregate(new StringBuilder(), (sb, ci) =>
			sb.Append(ci.Selected == true ? ci.ActionDescription + delimiter : string.Empty));

			return this.ConditionOther == true ?
				foo.Append(this.ConditionOtherText).ToString() : foo.ToString().Trim().TrimEnd(trimCharsSpaceComma);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string UpdateLanguages()
		{
			List<LanguageModel> combinedLanguages = this.StandardLanguages.Concat(this.ExoticLanguages).Concat(this.MonstrousLanguages).Concat(this.UserLanguages).ToList();
			StringBuilder tmp = combinedLanguages.Aggregate(new StringBuilder(), (sb, cl) => sb.Append(cl.Selected == true ? cl.Language + delimiter : string.Empty));

			if (this.Telepathy) { _ = tmp.Append("telepathy " + this.TelepathyRange); }

			string retStr = tmp.ToString().TrimEnd(trimCharsSpaceComma);

			const string noSpecCond = "No special conditions";
			const string spkNoLang = "Speaks no languages";
			const string spkAllLang = "Speaks all languages";
			const string csKSL = "Can't speak; Knows selected languages";
			const string csKCL = "Can't speak; Knows creator's languages";
			const string csKLL = "Can't speak; Knows languages known in life";
			const string alt = "Alternative language text (enter below)";
			const string strLangCreator = "understands the languages of its creator but can't speak";
			const string strLanfLife = "Understands all languages it spoke in life but can't speak";

			switch (LanguageOptions)
			{
				case null:
				case noSpecCond: // "No special conditions"
					{
						return retStr; // No-OP, retStr is already in the format we want
					}
				case spkNoLang: //  "Speaks no languages"
					{
						return "-"; // well, we threw away anthing that had already been collected in retStr :-(
					}
				case spkAllLang: // "Speaks all languages"
					{
						retStr = "all"; // this.Telepathy ? "all, telepathy " + this.TelepathyRange : "all";
						break;
					}
				case csKSL: //  "Can't speak; Knows selected languages"
					{
						retStr = string.Format("understands {0} but can't speak", retStr);
						break;
					}
				case csKCL: //  "Can't speak; Knows creator's languages"
					{
						retStr = strLangCreator;  // "understands the languages of its creator but can't speak"
						break;
					}
				case csKLL: // "Can't speak; Knows languages known in life"
					{
						retStr = strLanfLife;   // "Understands all languages it spoke in life but can't speak"
						break;
					}
				case alt: //  "Alternative language text (enter below)"
					{
						retStr = this.LanguageOptionsText.ToString().Trim().TrimEnd(trimCharsSpaceComma);
						break;
					}
				default:
					{
						retStr = string.Empty;
						break;
					}
			}
			return string.Format("{0}{1}", retStr, this.Telepathy ? ", telepathy " + this.TelepathyRange : string.Empty);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string UpdateSenses()
		{
			StringBuilder stringBuilder = new StringBuilder();
			_ = stringBuilder.Append(AppendSenses("darkvision ", this.Darkvision, " ft."));
			_ = stringBuilder.Append(AppendBlindSenses("blindsight ", this.Blindsight, " ft."));
			_ = stringBuilder.Append(AppendSenses("tremorsense ", this.Tremorsense, " ft."));
			_ = stringBuilder.Append(AppendSenses("truesight ", this.Truesight, " ft."));
			_ = stringBuilder.Append(AppendSenses("passive perception ", this.PassivePerception, ""));

			return stringBuilder.ToString().TrimEnd(trimCharsSpaceComma);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="senseName"></param>
		/// <param name="senseValue"></param>
		/// <param name="senseRange"></param>
		/// <returns></returns>
		static private string AppendSenses(string senseName, int senseValue, string senseRange)
		{
			return senseValue != 0 ? senseName + senseValue + senseRange + delimiter : string.Empty;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="senseName"></param>
		/// <param name="senseValue"></param>
		/// <param name="senseRange"></param>
		/// <returns></returns>
		private string AppendBlindSenses(string senseName, int senseValue, string senseRange)
		{
			// if senseValue == 0 then creating blind here is a waste, but the code flow is more legible
			string blind = this.BlindBeyond == true ? " (blind beyond this radius)" : string.Empty;
			return senseValue == 0 ? string.Empty : $"{senseName}{senseValue}{senseRange}{blind}{delimiter}";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string UpdateInnateSpellcastingLabel()
		{
			return this.InnateSpellcastingSection != true ? string.Empty :
				string.Format("Innate Spellcasting{0}.", this.Psionics == true ? " (Psionics)" : string.Empty);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string UpdateInnateSpellcasting()
		{
			if (this.InnateSpellcastingSection != true) { return string.Empty; }

			StringBuilder stringBuilder = new StringBuilder();
			_ = stringBuilder.Append($"The {this.NPCName.ToLower()}'s spellcasting ability is {this.InnateSpellcastingAbility} (");
			
			if (this.InnateSpellSaveDC != 0)
			{
				_ = stringBuilder.Append($"spell save DC {this.InnateSpellSaveDC}");
			}

			if (this.InnateSpellHitBonus != 0)
			{
				_ = stringBuilder.AppendFormat("{0}{1:+#;-#;+0} to hit with spell attacks",
					this.InnateSpellSaveDC != 0 ? ", " : string.Empty, this.InnateSpellHitBonus);
			}

			_ = stringBuilder.Append($"). The {this.NPCName.ToLower()} can innately cast the following spells, {this.ComponentText}:");
			return stringBuilder.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string UpdateSpellcasting()
		{
			if (this.SpellcastingSection != true) { return string.Empty; }

			StringBuilder stringBuilder = new StringBuilder();

			_ = stringBuilder.Append(
				$"The {this.NPCName.ToLower()} is a {this.SpellcastingCasterLevel} level spellcaster. Its spellcasting ability is {this.SCSpellcastingAbility}");

			if (this.SpellcastingSpellSaveDC != 0)
			{
				string scshb = this.SpellcastingSpellHitBonus != 0 ? string.Format(", {0:+#;-#;+0} to hit with spell attacks).", this.SpellcastingSpellHitBonus) : string.Empty;
				_ = stringBuilder.Append($" (spell save DC {this.SpellcastingSpellSaveDC}{scshb}).");
			}

			if (this.FlavorText != null)
			{
				_ = stringBuilder.Append($" {this.FlavorText}");
			}

			if (this.SpellcastingSpellClass == null)
			{
				MessageBox.Show("Spellcasting Class is null. Please select what class the spells are.");
				log.Error("Spellcasting Class is null. Please select what class the spells are.");
			}
			else
			{
				_ = stringBuilder.Append(" It has the following " + this.SpellcastingSpellClass.ToLower() + " spells prepared:");
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="damage"></param>
		/// <param name="specialWpn"></param>
		/// <param name="specialWpnDmg"></param>
		/// <returns></returns>
		private static string UpdateDamageImmunitiesAndResistances(List<SelectableActionModel> damage,
			List<SelectableActionModel> specialWpn, List<SelectableActionModel> specialWpnDmg)
        {
            StringBuilder sb = _generateDamageTypeBaseDescription(damage);

            string foo = string.Empty;
            foreach (SelectableActionModel selectableActionModel in specialWpn)
            {
                if (selectableActionModel.Selected == true && selectableActionModel.ActionName != "NoSpecial")
                {
                    if (selectableActionModel.ActionName.Equals(WeaponResistance.Nonmagical.ToString()))
                    {
                        foo = " from nonmagical attacks";
                    }
                    else if (selectableActionModel.ActionName.Equals(WeaponResistance.NonmagicalSilvered.ToString()))
                    {
                        foo = " from nonmagical attacks that aren't silvered";
                    }
                    else if (selectableActionModel.ActionName.Equals(WeaponResistance.NonmagicalAdamantine.ToString()))
                    {
                        foo = " from nonmagical attacks that aren't adamantine";
                    }
                    else if (selectableActionModel.ActionName.Equals(WeaponResistance.NonmagicalColdForgedIron.ToString()))
                    {
                        foo = " from nonmagical attacks that aren't cold-forged iron";
                    }
                    else if (selectableActionModel.ActionName.Equals(WeaponResistance.Magical.ToString())) // && specialWpn == this.SpecialWeaponDmgResistanceModelList)
                    {
                        foo = " from magic weapons";
                    }

                    _ = sb.Append(specialWpnDmg.Aggregate(new StringBuilder(), (sbSWD, swd) => sbSWD.Append(swd.Selected ? $"{swd.ActionDescription}, " : string.Empty)));
                    //if (sb.Length >= 2) { sb.Length -= 2; }
                    _ = sb.Append(foo);
                }
            }
            return sb.ToString().Trim();
        }

        private static StringBuilder _generateDamageTypeBaseDescription(List<SelectableActionModel> damage)
        {
            bool bpsDamageTypeFound = false;
            // Applicable in the final string 
            bool multipleBPSDamageTypeFound = false;
            foreach (SelectableActionModel sam in damage)
            {
                if (sam.Selected && _isSelectableActionModel_BPS(sam))
                {
                    // if bpsDamageTypeFound = true, then this means a second BPS is found
                    // i.e. bludgeoning and piercing
                    if (bpsDamageTypeFound)
                    {
                        multipleBPSDamageTypeFound = true;
                    }

                    // no matter what, set bpsDamageTypeFound = true;
                    bpsDamageTypeFound = true;
                }

            }
            StringBuilder sb = damage.Aggregate(new StringBuilder(), (sbDmg, dmg) => sbDmg.Append(dmg.Selected && !_isSelectableActionModel_BPS(dmg) ? $"{dmg.ActionDescription}, " : string.Empty));

            if (sb.Length > 0 && bpsDamageTypeFound)
            {
                // Truncate the ', ' at the end of the stringBuilder object and prepare for including the BPS selected options
                // i.e. fire, 
                sb.Length -= 2;
                sb.Append("; ");
            }

            damage.Aggregate(sb, (sbDmg, dmg) => sbDmg.Append(dmg.Selected && _isSelectableActionModel_BPS(dmg) ? $"{dmg.ActionDescription}, " : string.Empty));


            if (sb.Length >= 2) { sb.Length -= 2; }  // truncate the last 2 characters, which should be ", "
                                                     //if (sb.Length > 0) { _ = sb.Append("; "); }

            // Due to a unique quirk, if two or more BPS options are detected, then the last comma is replaced with ' and '
            // e.g.  bludgeoning, piercing -->   bludgeoning and piercing
            if (multipleBPSDamageTypeFound)
            {
                // Not sure how to get around this, but outputing sb to a string so it can be manipulated the way I want to
                string rawValue = sb.ToString();

                int place = rawValue.LastIndexOf(",");

                string result = rawValue.Remove(place, 1).Insert(place, " and");
                sb = new StringBuilder(result);
            }

            return sb;
        }

        /// <summary>
        /// This does a check against the SelectableActionModel object to see if it's Bludgeoning, Slashing or Piercing.  This is due
        /// to the format of Damage Vul / Resist / Immunity descriptions where BPS is to the right of all magical elements (e.g. Fire, cold, poison, etc...)
        /// </summary>
        /// <param name="selectableActionModel"></param>
        /// <returns></returns>
        private static bool _isSelectableActionModel_BPS(SelectableActionModel selectableActionModel)
        {
			string bludgeoningName = DamageType.Bludgeoning.ToString().ToUpper();
			string slashingName = DamageType.Slashing.ToString().ToUpper();
			string piercingName = DamageType.Piercing.ToString().ToUpper();

			string actionName = selectableActionModel.ActionName.ToUpper();

			return actionName.Equals(bludgeoningName) || actionName.Equals(slashingName) || actionName.Equals(piercingName);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string UpdateDamageImmunities()
		{
			return UpdateDamageImmunitiesAndResistances(this.DamageImmunityModelList, this.SpecialWeaponImmunityModelList, this.SpecialWeaponDmgImmunityModelList);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string UpdateDamageResistances()
		{
			return UpdateDamageImmunitiesAndResistances(this.DamageResistanceModelList, this.SpecialWeaponResistanceModelList, this.SpecialWeaponDmgResistanceModelList);
		}

		/// <summary>
		/// 'Skills Acrobatics +1, Animal Handling +2, Arcana +3, Athletics +4, Deception +5, History +6, Insight +7, Intimidation +8, Investigation +9,
		///  Medicine +10, Nature +11, Perception +12, Performance +13, Persuasion +14, Religion +15, Sleight of Hand +16, Stealth +17, Survival +18'
		/// </summary>
		/// <param name="line">The Skills string to be parsed in its entirity</param>
		public void ParseSkillAttributes(string line)
		{
			// Remove leading text "Skill "
			string pskillAttributes = Regex.Replace(line.Trim(), "^Skills", String.Empty, RegexOptions.IgnoreCase).Trim();

			string[] skillAttributeArray = pskillAttributes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);  // Split string on comma, ","

			foreach (string skillAttributeValue in skillAttributeArray)
			{
				if (!ParseAttributeStringToInt(skillAttributeValue, out string skillAttributeName, out int value)) { continue; }
				
				// When skill attributes dictionary is implemented, the following line of code should replace the switch statement
				this.skillAttributes[(SkillAttributes)Enum.Parse(typeof(SkillAttributes), skillAttributeName, true)] = value;
				continue;
			}

		}

		/// <summary>
		/// string foo = NPCModel.GetDescription(typeof(SkillAttributes), SkillAttributes.AnimalHandling);
		/// </summary>
		/// <param name="EnumType"></param>
		/// <param name="enumValue"></param>
		/// <returns></returns>
		public static string GetDescription(Type EnumType, object enumValue)
		{
			//DescriptionAttribute descriptionAttribute = EnumType.GetField(enumValue.ToString())
			//.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
			//return descriptionAttribute != null ? descriptionAttribute.Description : enumValue.ToString();

			return EnumType.GetField(enumValue.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault()
				is DescriptionAttribute descriptionAttribute ? descriptionAttribute.Description : enumValue.ToString();
		}

		/// <summary>
		/// Loop through the SkillAttributes enumeration and add the name & value to the string if the value is not 0
		/// </summary>
		/// <returns>A string representation of all the current values of the skillAttributes </returns>
		/// 
		public string SkillAttributesToString()
		{
			return skillAttributes.Aggregate(new StringBuilder(),
				(sb, kv) => sb.Append(kv.Value != 0 ?
				$"{NPCModel.GetDescription(typeof(SkillAttributes), kv.Key)} {kv.Value:+#;-#;+0}, " : string.Empty))
				.ToString().Trim().TrimEnd(trimCharsSpaceComma);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <returns></returns>
		public string OkToSaveToFile(object sender, RoutedEventArgs e)
		{
			StringBuilder warningMessageDoNotSave = new StringBuilder();

			if (string.IsNullOrEmpty(NPCType)) { warningMessageDoNotSave.Append(LogAndWarn("NPC Type is missing from ")); }
			if (string.IsNullOrEmpty(Size)) { warningMessageDoNotSave.Append(LogAndWarn("Size is missing from ")); }
			if (string.IsNullOrEmpty(AC)) { warningMessageDoNotSave.Append(LogAndWarn("AC is missing from ")); }
			if (string.IsNullOrEmpty(Alignment)) { warningMessageDoNotSave.Append(LogAndWarn("Alignment is missing from ")); }
			if (string.IsNullOrEmpty(ChallengeRating)) { warningMessageDoNotSave.Append(LogAndWarn("Challenge Rating is missing from ")); }
			if (string.IsNullOrEmpty(HP)) { warningMessageDoNotSave.Append(LogAndWarn("Hit Points are missing from ")); }
			if (string.IsNullOrEmpty(LanguageOptions)) { warningMessageDoNotSave.Append(LogAndWarn("Language Option (usually No special conditions) is missing from ")); }
			if (Telepathy == true && string.IsNullOrEmpty(TelepathyRange)) { warningMessageDoNotSave.Append(LogAndWarn("Telepathy Range is missing from ")); }
			if (InnateSpellcastingSection == true && string.IsNullOrEmpty(InnateSpellcastingAbility))
			{
				warningMessageDoNotSave.Append(LogAndWarn("Innate Spellcasting Ability is missing from "));
			}

			// Can we bail out here?
			if (SpellcastingSection != true) { return warningMessageDoNotSave.ToString(); }

			if (string.IsNullOrEmpty(SpellcastingCasterLevel)) { warningMessageDoNotSave.Append(LogAndWarn("Spellcaster Level is missing from ")); }
			if (string.IsNullOrEmpty(SCSpellcastingAbility)) { warningMessageDoNotSave.Append(LogAndWarn("Spellcasting Ability is missing from ")); }
			if (string.IsNullOrEmpty(SpellcastingSpellClass)) { warningMessageDoNotSave.Append(LogAndWarn("Spellcasting Class is missing from ")); }

			if (string.IsNullOrEmpty(CantripSpellSlots) && !string.IsNullOrEmpty(CantripSpellList))
			{
				warningMessageDoNotSave.Append(LogAndWarn("Number of Cantrip slots is missing from "));
			}

			if (string.IsNullOrEmpty(FirstLevelSpellSlots) && !string.IsNullOrEmpty(FirstLevelSpellList))
			{
				warningMessageDoNotSave.Append(LogAndWarn("Number of First Level Spell slots is missing from "));
			}

			if (string.IsNullOrEmpty(SecondLevelSpellSlots) && !string.IsNullOrEmpty(SecondLevelSpellList))
			{
				warningMessageDoNotSave.Append(LogAndWarn("Number of Second Level Spell slots is missing from "));
			}

			if (string.IsNullOrEmpty(ThirdLevelSpellSlots) && !string.IsNullOrEmpty(ThirdLevelSpellList))
			{
				warningMessageDoNotSave.Append(LogAndWarn("Number of Third Level Spell slots is missing from "));
			}

			if (string.IsNullOrEmpty(FourthLevelSpellSlots) && !string.IsNullOrEmpty(FourthLevelSpellList))
			{
				warningMessageDoNotSave.Append(LogAndWarn("Number of Fourth Level Spell slots is missing from "));
			}
			
			if (string.IsNullOrEmpty(FifthLevelSpellSlots) && !string.IsNullOrEmpty(FifthLevelSpellList))
			{
				warningMessageDoNotSave.Append(LogAndWarn("Number of Fifth Level Spell slots is missing from "));
			}

			if (string.IsNullOrEmpty(SixthLevelSpellSlots) && !string.IsNullOrEmpty(SixthLevelSpellList))
			{
				warningMessageDoNotSave.Append(LogAndWarn("Number of Sixth Level Spell slots is missing from "));
			}

			if (string.IsNullOrEmpty(SeventhLevelSpellSlots) && !string.IsNullOrEmpty(SeventhLevelSpellList))
			{
				warningMessageDoNotSave.Append(LogAndWarn("Number of Seventh Level Spell slots is missing from "));
			}

			if (string.IsNullOrEmpty(EighthLevelSpellSlots) && !string.IsNullOrEmpty(EighthLevelSpellList))
			{
				warningMessageDoNotSave.Append(LogAndWarn("Number of Eighth Level Spell slots is missing from "));
			}

			if (string.IsNullOrEmpty(NinthLevelSpellSlots) && !string.IsNullOrEmpty(NinthLevelSpellList))
			{
				warningMessageDoNotSave.Append(LogAndWarn("Number of Ninth Level Spell slots is missing from "));
			}

			return warningMessageDoNotSave.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="missing"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		private StringBuilder LogAndWarn(string missing)
		{
			log.Warn($"{missing}{this.NPCName}");
			return new StringBuilder($"{missing}{this.NPCName}\n");
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

		protected bool SetAttributes(ref Dictionary<SkillAttributes, int> attributes, int value, [CallerMemberName] string propertyname = null)
		{

			// Check if the value and backing field are actualy different
			if (attributes[(SkillAttributes)Enum.Parse(typeof(SkillAttributes), propertyname, true)] == value)
			{
				return false;
			}

			// Setting the backing field and the RaisePropertyChanged
			attributes[(SkillAttributes)Enum.Parse(typeof(SkillAttributes), propertyname, true)] = value;
			OnPropertyChanged(propertyname);
			return true;
		}

	}
}
