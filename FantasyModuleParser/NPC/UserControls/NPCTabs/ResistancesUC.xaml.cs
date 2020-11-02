using FantasyModuleParser.NPC.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.NPC.UserControls.NPCTabs
{
	/// <summary>
	/// Interaction logic for ResistancesUC.xaml
	/// </summary>
	public partial class ResistancesUC : UserControl
    {
		private ResistanceUserControlViewModel resistanceUserControlViewModel;
        public ResistancesUC()
        {
            InitializeComponent();
			resistanceUserControlViewModel = new ResistanceUserControlViewModel();
			DataContext = resistanceUserControlViewModel;
			HeaderLabel.Visibility = Visibility.Hidden;
		}

		public void Refresh()
		{
			(DataContext as ResistanceUserControlViewModel).Refresh();
		}

		private void ResVulnImm_Click(object sender, RoutedEventArgs e)
		{
			setAllVisibilitiesToHidden();
			if (DamageVulnerability.IsSelected)
			{
				HeaderLabel.Content = "Damage Vulnerabilities";
				listDamageVulnerability.Visibility = Visibility.Visible;
				SecondHeaderLabel.Visibility = Visibility.Hidden;
			}
			if (DamageImmunity.IsSelected)
			{
				HeaderLabel.Content = "Damage Immunities";
				listDamageImmunity.Visibility = Visibility.Visible;
				SecondHeaderLabel.Visibility = Visibility.Visible;
				SecondHeaderLabel.Content = "Special Weapon Immunities";
				listWeaponImmunity.Visibility = Visibility.Visible;
			}
			if (DamageResistance.IsSelected)
			{
				HeaderLabel.Content = "Damage Resistances";
				listDamageResistance.Visibility = Visibility.Visible;
				SecondHeaderLabel.Visibility = Visibility.Visible;
				SecondHeaderLabel.Content = "Special Weapon Resistances";
				listWeaponResistances.Visibility = Visibility.Visible;
			}
			if (ConditionImmunity.IsSelected)
			{
				HeaderLabel.Content = "Condition Immunities";
				listConditionImmunity.Visibility = Visibility.Visible;
				chkOther.Visibility = Visibility.Visible;
				strOther.Visibility = Visibility.Visible;
				SecondHeaderLabel.Visibility = Visibility.Hidden;
			}

		}

		private void setAllVisibilitiesToHidden()
		{
			HeaderLabel.Visibility = Visibility.Visible;
			listConditionImmunity.Visibility = Visibility.Hidden;
			listDamageImmunity.Visibility = Visibility.Hidden;
			listDamageResistance.Visibility = Visibility.Hidden;
			listDamageVulnerability.Visibility = Visibility.Hidden;
			SecondHeaderLabel.Visibility = Visibility.Hidden;
			listWeaponImmunity.Visibility = Visibility.Hidden;
			listWeaponResistances.Visibility = Visibility.Hidden;
			chkOther.Visibility = Visibility.Hidden;
			strOther.Visibility = Visibility.Hidden;
		}

		/*
		 *        private NPCModel initializeLanguageSelection(NPCModel npcModel)
        {
            npcModel.StandardLanguages = initSpecificLanguageSet(languageController.GenerateStandardLanguages(), npcModel.StandardLanguages);
            npcModel.MonstrousLanguages = initSpecificLanguageSet(languageController.GenerateMonsterLanguages(), npcModel.MonstrousLanguages);
            npcModel.ExoticLanguages = initSpecificLanguageSet(languageController.GenerateExoticLanguages(), npcModel.ExoticLanguages);
            npcModel.UserLanguages = initSpecificLanguageSet(languageController.GenerateUserLanguages(), npcModel.UserLanguages);

            return npcModel;
        }*/
	}
}
