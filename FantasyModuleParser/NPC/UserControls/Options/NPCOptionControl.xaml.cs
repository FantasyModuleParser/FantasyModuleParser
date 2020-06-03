using FantasyModuleParser.Main;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action.Enums;
using FantasyModuleParser.NPC.Views;
using FantasyModuleParser.NPC.UserControls.NPCTabs;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static FantasyModuleParser.Extensions.EnumerationExtension;

namespace FantasyModuleParser.NPC.UserControls.Options
{
    /// <summary>
    /// Interaction logic for NPCOptionControl.xaml
    /// </summary>
    public partial class NPCOptionControl : UserControl
    {
		#region Controllers
		public NPCController npcController { get; set; }
		#endregion

		#region Methods
		private void SwapTextBoxes(TextBox input, TextBox output)
		{
			string onHold = input.Text;
			input.Text = output.Text;
			output.Text = onHold;
		}
		private void InnateSpellcasting_Click(object sender, RoutedEventArgs e)
		{
			strComponentText.Text = "requiring no material components";
		}
		#endregion
		public NPCOptionControl()
		{
			InitializeComponent();
			npcController = new NPCController();
			//var npcModel = ((App)Application.Current).NpcModelObject;
			DataContext = npcController.GetNPCModel();
		}
		private void openfolder(string strPath, string strFolder)
		{
			var fPath = Path.Combine(strPath, strFolder);
			Directory.CreateDirectory(fPath);
			System.Diagnostics.Process.Start(fPath);
		}
		private void AppData_Click(object sender, RoutedEventArgs e)
		{
			openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP");
		}
		private void Projects_Click(object sender, RoutedEventArgs e)
		{
			openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP/Projects");
		}
		private void Artifacts_Click(object sender, RoutedEventArgs e)
		{
			openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP/Artifacts");
		}
		private void Equipment_Click(object sender, RoutedEventArgs e)
		{
			openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP/Equipment");
		}
		private void NPC_Click(object sender, RoutedEventArgs e)
		{
			openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP/NPC");
		}
		private void Parcel_Click(object sender, RoutedEventArgs e)
		{
			openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP/Parcel");
		}
		private void Spell_Click(object sender, RoutedEventArgs e)
		{
			openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP/Spell");
		}
		private void Table_Click(object sender, RoutedEventArgs e)
		{
			openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP/Table");
		}
		private void FG_Click(object sender, RoutedEventArgs e)
		{
			openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Fantasy Grounds");
		}
		private void Menu_Click(object sender, RoutedEventArgs e)
		{
			var menuitem = (MenuItem)sender;
			switch (menuitem.Name)
			{
				case "About":
					new About().Show();
					break;
				case "ManageCategories":
					new ManageCategories().Show();
					break;
				case "ManageProject":
					new ProjectManagement().Show();
					break;
				case "ProjectManagement":
					new ProjectManagement().Show();
					break;
				case "Settings":
					new Settings().Show();
					break;
				case "Supporters":
					new Supporters().Show();
					break;
			}
		}
		#region MenuOptions
		private void EditDeleteNPC_Click(object sender, RoutedEventArgs e)
		{
			EditDeleteNPC win2 = new EditDeleteNPC();
			win2.Show();
		}
		#endregion
		#region Actions
		private void ImportText_Click(object sender, RoutedEventArgs e)
		{
			new ImportText().Show();
		}
		private void FGListOptions_Click(object sender, RoutedEventArgs e)
		{
			new FGListOptions().Show();
		}
		#endregion
		#region NPCE_Up
		private void Up2_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits2.Text))
			{
				SwapTextBoxes(strTraits1, strTraits2);
				SwapTextBoxes(strTraitDesc1, strTraitDesc2);
			}
		}
		private void Up3_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits3.Text))
			{
				SwapTextBoxes(strTraits2, strTraits3);
				SwapTextBoxes(strTraitDesc2, strTraitDesc3);
			}
		}
		private void Up4_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits4.Text))
			{
				SwapTextBoxes(strTraits3, strTraits4);
				SwapTextBoxes(strTraitDesc3, strTraitDesc4);
			}
		}
		private void Up5_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits5.Text))
			{
				SwapTextBoxes(strTraits4, strTraits5);
				SwapTextBoxes(strTraitDesc4, strTraitDesc5);
			}
		}
		private void Up6_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits6.Text))
			{
				SwapTextBoxes(strTraits5, strTraits6);
				SwapTextBoxes(strTraitDesc5, strTraitDesc6);
			}
		}
		private void Up7_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits7.Text))
			{
				SwapTextBoxes(strTraits6, strTraits7);
				SwapTextBoxes(strTraitDesc6, strTraitDesc7);
			}
		}
		private void Up8_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits8.Text))
			{
				SwapTextBoxes(strTraits7, strTraits8);
				SwapTextBoxes(strTraitDesc7, strTraitDesc8);
			}
		}
		private void Up9_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits9.Text))
			{
				SwapTextBoxes(strTraits8, strTraits9);
				SwapTextBoxes(strTraitDesc8, strTraitDesc9);
			}
		}
		private void Up10_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits10.Text))
			{
				SwapTextBoxes(strTraits9, strTraits10);
				SwapTextBoxes(strTraitDesc9, strTraitDesc10);
			}
		}
		private void Up11_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits11.Text))
			{
				SwapTextBoxes(strTraits10, strTraits11);
				SwapTextBoxes(strTraitDesc10, strTraitDesc11);
			}
		}
		#endregion
		#region NPCE_Down
		private void Down1_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits1.Text))
			{
				SwapTextBoxes(strTraits1, strTraits2);
				SwapTextBoxes(strTraitDesc1, strTraitDesc2);
			}
		}
		private void Down2_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits2.Text))
			{
				SwapTextBoxes(strTraits2, strTraits3);
				SwapTextBoxes(strTraitDesc2, strTraitDesc3);
			}
		}
		private void Down3_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits3.Text))
			{
				SwapTextBoxes(strTraits3, strTraits4);
				SwapTextBoxes(strTraitDesc3, strTraitDesc4);
			}
		}
		private void Down4_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits4.Text))
			{
				SwapTextBoxes(strTraits4, strTraits5);
				SwapTextBoxes(strTraitDesc4, strTraitDesc5);
			}
		}
		private void Down5_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits5.Text))
			{
				SwapTextBoxes(strTraits5, strTraits6);
				SwapTextBoxes(strTraitDesc5, strTraitDesc6);
			}
		}
		private void Down6_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits6.Text))
			{
				SwapTextBoxes(strTraits6, strTraits7);
				SwapTextBoxes(strTraitDesc6, strTraitDesc7);
			}
		}
		private void Down7_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits7.Text))
			{
				SwapTextBoxes(strTraits7, strTraits8);
				SwapTextBoxes(strTraitDesc7, strTraitDesc8);
			}
		}
		private void Down8_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits8.Text))
			{
				SwapTextBoxes(strTraits8, strTraits9);
				SwapTextBoxes(strTraitDesc8, strTraitDesc9);
			}
		}
		private void Down9_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits9.Text))
			{
				SwapTextBoxes(strTraits9, strTraits10);
				SwapTextBoxes(strTraitDesc9, strTraitDesc10);
			}
		}
		private void Down10_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(strTraits10.Text))
			{
				SwapTextBoxes(strTraits10, strTraits11);
				SwapTextBoxes(strTraitDesc10, strTraitDesc11);
			}
		}
		#endregion
		#region NPCE_Cancel
		private void Cancel1_Click(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}
		private void Cancel2_Click(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}
		private void Cancel3_Click(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}
		private void Cancel4_Click(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}
		private void Cancel5_Click(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}
		private void Cancel6_Click(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}
		private void Cancel7_Click(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}
		private void Cancel8_Click(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}
		private void Cancel9_Click(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}
		private void Cancel10_Click(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}
		#endregion
		
		private void ResVulnImm_Click(object sender, RoutedEventArgs e)
		{
			stackDmgVulnerability.Visibility = BoolToVis(DamageVulnerability.IsSelected);
			stackDmgImmunity.Visibility = BoolToVis(DamageImmunity.IsSelected);
			stackDmgResistance.Visibility = BoolToVis(DamageResistance.IsSelected);
			stackConImmunity.Visibility = BoolToVis(ConditionImmunity.IsSelected);
			stackSpecialResistance.Visibility = BoolToVis(SpecialWeaponResistance.IsSelected);
			stackSpecialImmunity.Visibility = BoolToVis(SpecialWeaponImmunity.IsSelected);
		}

		private Visibility BoolToVis(bool isSelected)
		{
			return isSelected ? Visibility.Visible : Visibility.Hidden;
		}

		private void CreateNPCFile(object sender, RoutedEventArgs e)
		{

			NPCModel npcModel = ((App)Application.Current).NpcModel;

			if (npcModel == null)
				npcModel = new NPCModel
				{
					//DamageResistance = listDamageResistance.SelectedItems.Cast<ListBoxItem>().Where(a => a.IsSelected).Select(a => (string)a.Content).ToList(),
					//DamageVulnerability = listDamageVulnerability.SelectedItems.Cast<ListBoxItem>().Where(a => a.IsSelected).Select(a => (string)a.Content).ToList(),
					//DamageImmunity = listDamageImmunity.SelectedItems.Cast<ListBoxItem>().Where(a => a.IsSelected).Select(a => (string)a.Content).ToList(),
					//ConditionImmunity = listConditionImmunity.SelectedItems.Cast<ListBoxItem>().Where(a => a.IsSelected).Select(a => (string)a.Content).ToList(),
					//SpecialWeaponResistance = listWeaponResistances.SelectedItems.Cast<ListBoxItem>().Where(a => a.IsSelected).Select(a => (string)a.Content).ToList(),
					//SpecialWeaponImmunity = listWeaponImmunity.SelectedItems.Cast<ListBoxItem>().Where(a => a.IsSelected).Select(a => (string)a.Content).ToList(),
					ConditionOther = chkOther.IsChecked.Value,
					ConditionOtherText = strOther.Text,
					Acrobatics = int.Parse(strAcrobatics.Text),
					AnimalHandling = int.Parse(strAnimalHandling.Text),
					Arcana = int.Parse(strArcana.Text),
					Athletics = int.Parse(strAthletics.Text),
					Deception = int.Parse(strDeception.Text),
					History = int.Parse(strHistory.Text),
					Insight = int.Parse(strInsight.Text),
					Intimidation = int.Parse(strIntimidation.Text),
					Investigation = int.Parse(strInvestigation.Text),
					Medicine = int.Parse(strMedicine.Text),
					Nature = int.Parse(strNature.Text),
					Perception = int.Parse(strPerception.Text),
					Performance = int.Parse(strPerformance.Text),
					Persuasion = int.Parse(strPersuasion.Text),
					Religion = int.Parse(strReligion.Text),
					SleightOfHand = int.Parse(strSleightofHand.Text),
					Stealth = int.Parse(strStealth.Text),
					Survival = int.Parse(strSurvival.Text),
					StandardLanguages = listStandard.SelectedItems.Cast<ListBoxItem>().Where(a => a.IsSelected).Select(a => (string)a.Content).ToList(),
					ExoticLanguages = listExotic.SelectedItems.Cast<ListBoxItem>().Where(a => a.IsSelected).Select(a => (string)a.Content).ToList(),
					MonstrousLanguages = listMonstrous.SelectedItems.Cast<ListBoxItem>().Where(a => a.IsSelected).Select(a => (string)a.Content).ToList(),
					UserLanguages = listUser.SelectedItems.Cast<ListBoxItem>().Where(a => a.IsSelected).Select(a => (string)a.Content).ToList(),
					LanguageOptions = strLanguageOptions.Text,
					LanguageOptionsText = strLanguageOptionsText.Text,
					Telepathy = chkTelepathy.IsChecked.Value,
					TelepathyRange = strTelepathyRange.Text,
					Traits1 = strTraits1.Text,
					TraitsDesc1 = strTraitDesc1.Text,
					Traits2 = strTraits2.Text,
					TraitsDesc2 = strTraitDesc2.Text,
					Traits3 = strTraits3.Text,
					TraitsDesc3 = strTraitDesc3.Text,
					Traits4 = strTraits4.Text,
					TraitsDesc4 = strTraitDesc4.Text,
					Traits5 = strTraits5.Text,
					TraitsDesc5 = strTraitDesc5.Text,
					Traits6 = strTraits6.Text,
					TraitsDesc6 = strTraitDesc6.Text,
					Traits7 = strTraits7.Text,
					TraitsDesc7 = strTraitDesc7.Text,
					Traits8 = strTraits8.Text,
					TraitsDesc8 = strTraitDesc8.Text,
					Traits9 = strTraits9.Text,
					TraitsDesc9 = strTraitDesc9.Text,
					Traits10 = strTraits10.Text,
					TraitsDesc10 = strTraitDesc10.Text,
					Traits11 = strTraits11.Text,
					TraitsDesc11 = strTraitDesc11.Text,
					InnateSpellcastingSection = chkInnateSpellcastingSection.IsChecked.Value,
					Psionics = chkPsionics.IsChecked.Value,
					InnateSpellcastingAbility = strInnateSpellcastingAbility.Text,
					InnateSpellSaveDC = int.Parse(intInnateSpellSaveDC.Text),
					InnateSpellHitBonus = int.Parse(intInnateSpellHitBonus.Text),
					InnateAtWill = strInnatAtWill.Text,
					FivePerDay = strFivePerDay.Text,
					FourPerDay = strFourPerDay.Text,
					ThreePerDay = strThreePerDay.Text,
					TwoPerDay = strTwoPerDay.Text,
					OnePerDay = strOnePerDay.Text,
					SpellcastingCasterLevel = strCasterLevel.Text,
					SCSpellcastingAbility = strSCSpellcastingAbility.Text,
					SpellcastingSpellSaveDC = int.Parse(intSpellcastingSpellSaveDC.Text),
					SpellcastingSpellHitBonus = int.Parse(intSpellcastingSpellHitBonus.Text),
					SpellcastingSpellClass = strSpellcastingSpellClass.Text,
					FlavorText = strFlavorText.Text,
					CantripSpells = strCantripSpells.Text,
					CantripSpellList = strCantripSpellList.Text,
					FirstLevelSpells = strFirstLevelSpells.Text,
					FirstLevelSpellList = strFirstLevelSpellList.Text,
					SecondLevelSpells = strSecondLevelSpells.Text,
					SecondLevelSpellList = strSeventhLevelSpellList.Text,
					ThirdLevelSpells = strThirdLevelSpells.Text,
					ThirdLevelSpellList = strThirdLevelSpellList.Text,
					FourthLevelSpells = strFourthLevelSpells.Text,
					FourthLevelSpellList = strFourthLevelSpellList.Text,
					FifthLevelSpells = strfifthLevelSpells.Text,
					FifthLevelSpellList = strFifthLevelSpellList.Text,
					SixthLevelSpells = strSixthLevelSpells.Text,
					SixthLevelSpellList = strSixthLevelSpellList.Text,
					SeventhLevelSpells = strSeventhLevelSpells.Text,
					SeventhLevelSpellList = strSeventhLevelSpellList.Text,
					EighthLevelSpells = strEigthLevelSpells.Text,
					EighthLevelSpellList = strEigthLevelSpellList.Text,
					NinthLevelSpells = strNinthLevelSpells.Text,
					NinthLevelSpellList = strNinthLevelSpellList.Text,
					MarkedSpells = strMarkedSpells.Text,
					Description = strDescription.Text,
					NonID = strNonID.Text,
					NPCImage = strNPCImage.Text,
				};

			((App)Application.Current).NpcModel = npcModel;
			
		}

		private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex(@"[^0-9-]+"); ;
			e.Handled = regex.IsMatch(e.Text);
		}

		private void LoadNPCOption_Click(object sender, RoutedEventArgs e)
		{
			// Create OpenFileDialog
			Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

			// Launch OpenFileDialog by calling ShowDialog method
			Nullable<bool> result = openFileDlg.ShowDialog();
			// Get the selected file name and display in a TextBox.
			// Load content of file in a TextBlock
			if (result == true)
			{
				npcController.Load(openFileDlg.FileName);
				NPCModel npcModel = npcController.GetNPCModel();

				//Refresh all the data on the UI
				DataContext = npcModel;
			}
		}

		private void PreviewNPC_Click(object sender, RoutedEventArgs e)
		{
			new PreviewNPC().Show();
		}
	}
}
