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
        private int _innaateSpellHitBonus;
        private string _innateComponentText;
        private string _innateAtWill;
        private string _innateOnePerDay;
        private string _innateTwoPerDay;
        private string _innateThreePerDay;
        private string _innateFourPerDay;
        private string _innateFivePerDay;
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
        public int Burrow { get { return _burrow; } set { Set(ref _burrow, value); } }
        public int Climb { get { return _climb; } set { Set(ref _climb, value); } }
        public int Fly { get { return _fly; } set { Set(ref _fly, value); } }
        public bool Hover { get { return _hover; } set { Set(ref _hover, value); } }
        public int Swim { get { return _swim; } set { Set(ref _swim, value); } }
        public int AttributeStr { get { return _attributeStr; } set { Set(ref _attributeStr, value); } }
        public int AttributeDex { get { return _attributeDex; } set { Set(ref _attributeDex, value); } }
        public int AttributeCon { get { return _attributeCon; } set { Set(ref _attributeCon, value); } }
        public int AttributeInt { get { return _attributeInt; } set { Set(ref _attributeInt, value); } }
        public int AttributeWis { get { return _attributeWis; } set { Set(ref _attributeWis, value); } }
        public int AttributeCha { get { return _attributeCha; } set { Set(ref _attributeCha, value); } }
        public int SavingThrowStr { get { return _savingThrowStr; } set { Set(ref _savingThrowStr, value); } }
        public int SavingThrowDex { get { return _savingThrowDex; } set { Set(ref _savingThrowDex, value); } }
        public int SavingThrowCon { get { return _savingThrowCon; } set { Set(ref _savingThrowCon, value); } }
        public int SavingThrowInt { get { return _savingThrowInt; } set { Set(ref _savingThrowInt, value); } }
        public int SavingThrowWis { get { return _savingThrowWis; } set { Set(ref _savingThrowWis, value); } }
        public int SavingThrowCha { get { return _savingThrowCha; } set { Set(ref _savingThrowCha, value); } }
        public bool SavingThrowStrBool { get { return _savingThrowStrBool; } set { Set(ref _savingThrowStrBool, value); } }
        public bool SavingThrowDexBool { get { return _savingThrowDexBool; } set { Set(ref _savingThrowDexBool, value); } }
        public bool SavingThrowConBool { get { return _savingThrowConBool; } set { Set(ref _savingThrowConBool, value); } }
        public bool SavingThrowIntBool { get { return _savingThrowIntBool; } set { Set(ref _savingThrowIntBool, value); } }
        public bool SavingThrowWisBool { get { return _savingThrowWisBool; } set { Set(ref _savingThrowWisBool, value); } }
        public bool SavingThrowChaBool { get { return _savingThrowChaBool; } set { Set(ref _savingThrowChaBool, value); } }
        public int Blindsight { get { return _blindsight; } set { Set(ref _blindsight, value); } }
        public bool BlindBeyond { get { return _blindBeyond; } set { Set(ref _blindBeyond, value); } }
        public int Darkvision { get { return _darkvision; } set { Set(ref _darkvision, value); } }
        public int Tremorsense { get { return _tremorsense; } set { Set(ref _tremorsense, value); } }
        public int Truesight { get { return _truesight; } set { Set(ref _truesight, value); } }
        public int PassivePerception { get { return _passivePerception; } set { Set(ref _passivePerception, value); } }
        public string ChallengeRating { get { return _challengeRating; } set { Set(ref _challengeRating, value); } }
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
        public bool ConditionOther { get { return _conditionOther; } set { Set(ref _conditionOther, value); } }
        public string ConditionOtherText { get { return _conditionOtherText; } set { Set(ref _conditionOtherText, value); } }
        public int Acrobatics { get { return _acrobatics; } set { Set(ref _acrobatics, value); } }
        public int AnimalHandling { get { return _animalHandling; } set { Set(ref _animalHandling, value); } }
        public int Arcana { get { return _arcana; } set { Set(ref _arcana, value); } }
        public int Athletics { get { return _athletics; } set { Set(ref _athletics, value); } }
        public int Deception { get { return _deception; } set { Set(ref _deception, value); } }
        public int History { get { return _history; } set { Set(ref _history, value); } }
        public int Insight { get { return _insight; } set { Set(ref _insight, value); } }
        public int Intimidation { get { return _intimidation; } set { Set(ref _intimidation, value); } }
        public int Investigation { get { return _investigation; } set { Set(ref _investigation, value); } }
        public int Medicine { get { return _medicine; } set { Set(ref _medicine, value); } }
        public int Nature { get { return _nature; } set { Set(ref _nature, value); } }
        public int Perception { get { return _perception; } set { Set(ref _perception, value); } }
        public int Performance { get { return _performance; } set { Set(ref _performance, value); } }
        public int Persuasion { get { return _persuasion; } set { Set(ref _persuasion, value); } }
        public int Religion { get { return _religion; } set { Set(ref _religion, value); } }
        public int SleightOfHand { get { return _sleightOfHand; } set { Set(ref _sleightOfHand, value); } }
        public int Stealth { get { return _stealth; } set { Set(ref _stealth, value); } }
        public int Survival { get { return _survival; } set { Set(ref _survival, value); } }
        public ObservableCollection<LanguageModel> StandardLanguages { get; set; }
        public ObservableCollection<LanguageModel> ExoticLanguages { get; set; }
        public ObservableCollection<LanguageModel> MonstrousLanguages { get; set; }
        public ObservableCollection<LanguageModel> UserLanguages { get; set; }
        #endregion
        public string LanguageOptions { get; set; }
        public string LanguageOptionsText { get; set; }
        public bool Telepathy { get; set; }
        public string TelepathyRange { get; set; }
        public ObservableCollection<ActionModelBase> Traits { get; set; }
        public bool InnateSpellcastingSection { get { return _innateSpellcastingSection; } set { Set(ref _innateSpellcastingSection, value); } }
        public bool Psionics { get { return _markAsPsionics; } set { Set(ref _markAsPsionics, value); } }
        public string InnateSpellcastingAbility { get { return _innateSpellcastingAbility; } set { Set(ref _innateSpellcastingAbility, value); } }
        public int InnateSpellSaveDC { get { return _innateSpellSaveDC; } set { Set(ref _innateSpellSaveDC, value); } }
        public int InnateSpellHitBonus { get { return _innaateSpellHitBonus; } set { Set(ref _innaateSpellHitBonus, value); } }
        public string ComponentText { get { return _innateComponentText; } set { Set(ref _innateComponentText, value); } }
        public string InnateAtWill { get { return _innateAtWill; } set { Set(ref _innateAtWill, value); } }
        public string FivePerDay { get { return _innateFivePerDay; } set { Set(ref _innateFivePerDay, value); } }
        public string FourPerDay { get { return _innateFourPerDay; } set { Set(ref _innateFourPerDay, value); } }
        public string ThreePerDay { get { return _innateThreePerDay; } set { Set(ref _innateThreePerDay, value); } }
        public string TwoPerDay { get { return _innateTwoPerDay; } set { Set(ref _innateTwoPerDay, value); } }
        public string OnePerDay { get { return _innateOnePerDay; } set { Set(ref _innateOnePerDay, value); } }
        public bool SpellcastingSection { get; set; }
        public string SpellcastingCasterLevel { get; set; }
        public string SCSpellcastingAbility { get; set; }
        public int SpellcastingSpellSaveDC { get; set; }
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
