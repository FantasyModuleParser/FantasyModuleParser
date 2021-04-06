using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.Models.Skills;
using FantasyModuleParser.NPC.ViewModel;
using log4net;
using System.Text;
using System.Windows;

namespace FantasyModuleParser.NPC.ViewModels
{

    public class PreviewNPCViewModel : ViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PreviewNPCViewModel));
        // private static readonly char[] trimCharsSpaceComma = { ' ', ',' };
		// private static readonly string delimiter = ", ";

        public NPCModel NpcModel { get; set; }
        private readonly NPCController npcController;

        public string SpeedDescription { get; set; }
        public string SkillsDescription { get; set; }
        public string StrengthAttribute { get; set; }
        public string DexterityAttribute { get; set; }
        public string ConstitutionAttribute { get; set; }
        public string IntelligenceAttribute { get; set; }
        public string WisdomAttribute { get; set; }
        public string CharismaAttribute { get; set; }
        public string SavingThrows { get; set; }
        public string Senses { get; set; }
        public string DamageVulnerabilities { get; set; }
        public string DamageResistances { get; set; }
        public string DamageImmunities { get; set; }
        public string ConditionImmunities { get; set; }
        public string Languages { get; set; }
        public string Challenge { get; set; }
        public string InnateSpellcastingLabel { get; set; }
        public string InnateSpellcasting { get; set; }
        public string SpellcastingLabel { get; set; }
        public string Spellcasting { get; set; }
        public string SpellcastingCantripsLabel { get; set; }
        public string SpellcastingCantrips { get; set; }
        public string SpellcastingFirstLabel { get; set; }
        public string SpellcastingFirst { get; set; }
        public string SpellcastingSecondLabel { get; set; }
        public string SpellcastingSecond { get; set; }
        public string SpellcastingThirdLabel { get; set; }
        public string SpellcastingThird { get; set; }
        public string SpellcastingFourthLabel { get; set; }
        public string SpellcastingFourth { get; set; }
        public string SpellcastingFifthLabel { get; set; }
        public string SpellcastingFifth { get; set; }
        public string SpellcastingSixthLabel { get; set; }
        public string SpellcastingSixth { get; set; }
        public string SpellcastingSeventhLabel { get; set; }
        public string SpellcastingSeventh { get; set; }
        public string SpellcastingEighthLabel { get; set; }
        public string SpellcastingEighth { get; set; }
        public string SpellcastingNinthLabel { get; set; }
        public string SpellcastingNinth { get; set; }
        public string SpellcastingMarkedSpells { get; set; }
        public string ActionsVisibility { get; set; }
        public string WeaponName1 { get; set; }

        public PreviewNPCViewModel()
        {
            npcController = new NPCController();
            NpcModel = npcController.GetNPCModel();
            InitalizeViewModel();
        }

        public PreviewNPCViewModel(NPCModel npcModel)
        {
            NpcModel = npcModel;
            InitalizeViewModel();
        }

        public void InitalizeViewModel()
        {
            SpeedDescription = NpcModel.GetAllSpeeds();
            SkillsDescription = NpcModel.SkillAttributesToString();

			#region UpdateAbilityScores
			StrengthAttribute = NpcModel.UpdateStrengthAttributeModifier;
			DexterityAttribute = NpcModel.UpdateDexterityAttributeModifier;
			ConstitutionAttribute = NpcModel.UpdateConstitutionAttributeModifier;
			IntelligenceAttribute = NpcModel.UpdateIntelligenceAttributeModifier;
			WisdomAttribute = NpcModel.UpdateWisdomAttributeModifier;
			CharismaAttribute = NpcModel.UpdateCharismaAttributeModifier;
			#endregion

			SavingThrows = NpcModel.UpdateSavingThrowsString();
            Senses = NpcModel.UpdateSenses();
            DamageVulnerabilities = NpcModel.UpdateDamageVulnerabilities();
            DamageResistances = NpcModel.UpdateDamageResistances();
            DamageImmunities = NpcModel.UpdateDamageImmunities();
            ConditionImmunities = NpcModel.UpdateConditionImmunities();
            Languages = NpcModel.UpdateLanguages();
            Challenge = NpcModel.UpdateChallengeRating;
            InnateSpellcastingLabel = NpcModel.UpdateInnateSpellcastingLabel();
            InnateSpellcasting = NpcModel.UpdateInnateSpellcasting();
            Spellcasting = NpcModel.UpdateSpellcasting();

            #region UpdateSpellSlotsLabels
			SpellcastingCantripsLabel = NpcModel.RetrieveCantripsSpellSlotsString;
            SpellcastingFirstLabel = NpcModel.RetrieveFirstLevelSpellSlotsString;
            SpellcastingSecondLabel = NpcModel.RetrieveSecondLevelSpellSlotsString;
            SpellcastingThirdLabel = NpcModel.RetrieveThirdLevelSpellSlotsString;
            SpellcastingFourthLabel = NpcModel.RetrieveFourthLevelSpellSlotsString;
            SpellcastingFifthLabel = NpcModel.RetrieveFifthLevelSpellSlotsString;
            SpellcastingSixthLabel = NpcModel.RetrieveSixthLevelSpellSlotsString;
            SpellcastingSeventhLabel = NpcModel.RetrieveSeventhLevelSpellSlotsString;
            SpellcastingEighthLabel = NpcModel.RetrieveEighthLevelSpellSlotsString;
            SpellcastingNinthLabel = NpcModel.RetrieveNinthLevelSpellSlotsString;
            SpellcastingMarkedSpells = NpcModel.RetrieveMarkedLevelSpellSlotsString;
            #endregion
        }

		#region UpdateSkills
		public Visibility ShowSkills
		{
			get { return SkillsDescription.Length > 0 ? Visibility.Visible : Visibility.Collapsed; }
		}
		#endregion

		#region UpdateSavingThrows
		public Visibility ShowSavingThrows
		{
			get { return SavingThrows.Length > 0 ? Visibility.Visible : Visibility.Collapsed; }
		}
		#endregion

		#region UpdateDamageVulnerabilities
		public Visibility ShowDamageVulnerabilities
		{
			get { return DamageVulnerabilities.Length > 0 ? Visibility.Visible : Visibility.Collapsed; }
		}
		#endregion

		#region UpdateDamageResistances
		public Visibility ShowDamageResistances
		{
			get { return DamageResistances.Length > 0 ? Visibility.Visible : Visibility.Collapsed; }
		}

		#endregion

		#region UpdateDamageImmunities
		public Visibility ShowDamageImmunities
		{
			get { return DamageImmunities.Length > 0 ? Visibility.Visible : Visibility.Collapsed; }
		}

		#endregion

		#region UpdateConditionImmunities
		public Visibility ShowConditionImmunities
		{
			get { return ConditionImmunities.Length > 0 ? Visibility.Visible : Visibility.Collapsed; }
		}
		#endregion

		#region UpdateLanguages
		public Visibility ShowLanguages
		{
			get { return Languages.Length > 0 ? Visibility.Visible : Visibility.Collapsed; }
		}
        #endregion

		#region UpdateInnateSpellcasting
		public Visibility ShowInnateSpellcasting
		{
			get { return NpcModel.InnateSpellcastingSection == true ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowInnateAtWill
		{
			get { return NpcModel.InnateAtWill != null && NpcModel.InnateAtWill.Length > 0 ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowInnateFivePerDay
		{
			get { return NpcModel.FivePerDay != null && NpcModel.FivePerDay.Length > 0 ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowInnateFourPerDay
		{
			get { return NpcModel.FourPerDay != null && NpcModel.FourPerDay.Length > 0 ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowInnateThreePerDay
		{
			get { return NpcModel.ThreePerDay != null && NpcModel.ThreePerDay.Length > 0 ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowInnateTwoPerDay
		{
			get { return NpcModel.TwoPerDay != null && NpcModel.TwoPerDay.Length > 0 ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowInnateOnePerDay
		{
			get { return NpcModel.OnePerDay != null && NpcModel.OnePerDay.Length > 0 ? Visibility.Visible : Visibility.Collapsed; }
		}
		#endregion

		#region UpdateSpellcasting
		public Visibility ShowSpellcasting
		{
			get { return NpcModel.SpellcastingSection == true ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowCantrips
		{
			get { return NpcModel.CantripSpellList != null ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowFirst
		{
			get { return NpcModel.FirstLevelSpellList != null ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowSecond
		{
			get { return NpcModel.SecondLevelSpellList != null ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowThird
		{
			get { return NpcModel.ThirdLevelSpellList != null ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowFourth
		{
			get { return NpcModel.FourthLevelSpellList != null ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowFifth
		{
			get { return NpcModel.FifthLevelSpellList != null ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowSixth
		{
			get { return NpcModel.SixthLevelSpellList != null ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowSeventh
		{
			get { return NpcModel.SeventhLevelSpellList != null ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowEighth
		{
			get { return NpcModel.EighthLevelSpellList != null ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowNinth
		{
			get { return NpcModel.NinthLevelSpellList != null ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowMarked
		{
			get { return NpcModel.MarkedSpells != null ? Visibility.Visible : Visibility.Collapsed; }
		}
		#endregion

		public Visibility PresentAction
		{
			get { return NpcModel.NPCActions != null ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility PresentActionLine
		{
			get { return NpcModel.NPCActions != null ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility PresentReactions
		{
			get { return NpcModel.Reactions.Count >= 1 ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility PresentReactionsLine
		{
			get { return NpcModel.Reactions.Count >= 1 ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility PresentLegActions
		{
			get { return NpcModel.LegendaryActions.Count >= 1 ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility PresentLegActionsLine
		{
			get { return NpcModel.LegendaryActions.Count >= 1 ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility PresentLairActions
		{
			get { return NpcModel.LairActions.Count >= 1 ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility PresentLairActionsLine
		{
			get { return NpcModel.LairActions.Count >= 1 ? Visibility.Visible : Visibility.Collapsed; }
		}
	}
}
