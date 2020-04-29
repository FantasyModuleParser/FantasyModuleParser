using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EngineeringSuite.NPC.Controller;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace EngineeringSuite.NPC
{
    /// <summary>
    /// NPC Engineer back-end C# coding
    /// Function openfolder controls creating all the required folders if they don't exist already
    /// </summary>
public partial class NPCEngineer : Window
    {

		#region Controllers
        public NPCController npcController { get; set; }
        #endregion

        #region Variables
        string installPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string installFolder = "Engineer Suite/NPC";
        #endregion

        public NPCEngineer()
        {
            InitializeComponent();
            npcController = new NPCController();
        }
        private void openfolder(string strPath, string strFolder)
        {
            var fPath = Path.Combine(strPath, strFolder);
            System.IO.Directory.CreateDirectory(fPath);
            System.Diagnostics.Process.Start(fPath);
        }
        private void AppData_Click(object sender, RoutedEventArgs e)
        {
            openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite");
        }
        private void Projects_Click(object sender, RoutedEventArgs e)
        {
            openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/Projects");
        }
        private void Artifacts_Click(object sender, RoutedEventArgs e)
        {
            openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/Artifacts");
        }
        private void Equipment_Click(object sender, RoutedEventArgs e)
        {
            openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/Equipment");
        }
        private void NPC_Click(object sender, RoutedEventArgs e)
        {
            openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/NPC");
        }
        private void Parcel_Click(object sender, RoutedEventArgs e)
        {
            openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/Parcel");
        }
        private void Spell_Click(object sender, RoutedEventArgs e)
        {
            openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/Spell");
        }
        private void Table_Click(object sender, RoutedEventArgs e)
        {
            openfolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/Table");
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
                    new ESAbout().Show();
                    break;
                case "Exit":
                    this.Close();
                    break;
                case "ManageCategories":
                    new ESManageCategories().Show();
                    break;
                case "ManageProject":
                    new ESProjectManagement().Show();
                    break;
                case "ProjectManagement":
                    new ESProjectManagement().Show();
                    break;
                case "Settings":
                    new ESSettings().Show();
                    break;
                case "RefManEngineer":
                    new NPCEngineer().Show();
                    break;
                case "SpellEngineer":
                    new NPCEngineer().Show();
                    break;
                case "Supporters":
                    new ESSupporters().Show();
                    break;
                case "TableEngineer":
                    new NPCEngineer().Show();
                    break;
            }
        }
        #region MenuOptions
        private void ESEditDeleteNPC_Click(object sender, RoutedEventArgs e)
        {
            ESEditDeleteNPC win2 = new ESEditDeleteNPC();
            win2.Show();
        }
        #endregion
        #region ActionTabs
        private void NPCELairActions_Click(object sender, RoutedEventArgs e)
        {
            NPCELairActions win2 = new NPCELairActions();
            win2.Show();
        }
        private void NPCEActions_Click(object sender, RoutedEventArgs e)
        {
            NPCEActions win3 = new NPCEActions();
            win3.Show();
        }
        private void NPCEReactions_Click(object sender, RoutedEventArgs e)
        {
            NPCEReactions win3 = new NPCEReactions();
            win3.Show();
        }
        private void NPCELegAction_Click(object sender, RoutedEventArgs e)
        {
            NPCELegActions win4 = new NPCELegActions();
            win4.Show();
        }
        #endregion
        #region NPCE_Actions
        private void ImportText_Click(object sender, RoutedEventArgs e)
        {
            NPCEImportText win2 = new NPCEImportText();
            win2.Show();
        }
        private void NPCEFGListOptions_Click(object sender, RoutedEventArgs e)
        {
            NPCEFGListOptions win2 = new NPCEFGListOptions();
            win2.Show();
        }
        #endregion
        #region NPCE_Up
        private void Up2_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up3_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up4_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up5_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up6_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up7_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up8_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up9_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Up10_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        #endregion
        #region NPCE_Down
        private void Down1_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down2_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down3_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down4_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down5_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down6_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down7_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down8_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Down9_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        #endregion
        #region NPCE_Edit
        private void Edit1_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit2_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit3_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit4_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit5_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit6_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit7_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit8_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit9_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Edit10_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        #endregion
        #region NPCE_Cancel
        private void Cancel1_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel2_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel3_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel4_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel5_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel6_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel7_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel8_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel9_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void Cancel10_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        #endregion
        #region BaseStatChange
        private void StrengthScore_TextChanged(object sender, RoutedEventArgs e)
        {
            int num;
            if (int.TryParse(strAttrStr.Text, out num))
            {
                strModStr1.Content = -5 + (num / 2);
                if (num < 10)
                {
                    strModStr.Content = "";
                }
                else
                {
                    strModStr.Content = "+";
                }
            }
        }
        private void DexterityScore_TextChanged(object sender, RoutedEventArgs e)
        {
            int num;
            if (int.TryParse(strAttrDex.Text, out num))
            {
                strModDex1.Content = -5 + (num / 2);
                if (num < 10)
                {
                    strModDex.Content = "";
                }
                else
                {
                    strModDex.Content = "+";
                }
            }
        }
        private void ConstitutionScore_TextChanged(object sender, RoutedEventArgs e)
        {
            int num;
            if (int.TryParse(strAttrCon.Text, out num))
            {
                strModCon1.Content = -5 + (num / 2);
                if (num < 10)
                {
                    strModCon.Content = "";
                }
                else
                {
                    strModCon.Content = "+";
                }
            }
        }
        private void IntelligenceScore_TextChanged(object sender, RoutedEventArgs e)
        {
            int num;
            if (int.TryParse(strAttrInt.Text, out num))
            {
                strModInt1.Content = -5 + (num / 2);
                if (num < 10)
                {
                    strModInt.Content = "";
                }
                else
                {
                    strModInt.Content = "+";
                }
            }
        }
        private void WisdomScore_TextChanged(object sender, RoutedEventArgs e)
        {
            int num;
            if (int.TryParse(strAttrWis.Text, out num))
            {
                strModWis1.Content = -5 + (num / 2);
                if (num < 10)
                {
                    strModWis.Content = "";
                }
                else
                {
                    strModWis.Content = "+";
                }
            }
        }
        private void CharismaScore_TextChanged(object sender, RoutedEventArgs e)
        {
            int num;
            if (int.TryParse(strAttrCha.Text, out num))
            {
                strModCha1.Content = -5 + (num / 2);
                if (num < 10)
                {
                    strModCha.Content = "";
                }
                else
                {
                    strModCha.Content = "+";
                }
            }
        }
        #endregion

        private void CreateNPCFile(object sender, RoutedEventArgs e)
        {
	        string npcName = NPC_name.Text;
            string savePath = Path.Combine(installPath, installFolder, npcName + ".json");

            NPCModel npcModel = new NPCModel
            {
	            NPCName = NPC_name.Text,
	            Size = strSize.Text,
	            NPCType = strType.Text,
	            Tag = strTag.Text,
	            Alignment = strAlignment.Text,
	            AC = strAC.Text,
	            HP = strHP.Text,
                NPCGender = strGender.Text,
                Unique = chkUnique.IsChecked.Value,
                NPCNamed = chkNamed.IsChecked.Value,
                Speed = int.Parse(intSpeed.Text),
                Burrow = int.Parse(intBurrow.Text),
                Climb = int.Parse(intClimb.Text),
                Fly = int.Parse(intFly.Text),
                Swim = int.Parse(intSwim.Text),
                AttributeStr = int.Parse(strAttrStr.Text),
                AttributeDex = int.Parse(strAttrDex.Text),
                AttributeCon = int.Parse(strAttrCon.Text),
                AttributeInt = int.Parse(strAttrInt.Text),
                AttributeWis = int.Parse(strAttrWis.Text),
                AttributeCha = int.Parse(strAttrCha.Text),
                SavingThrowStr = int.Parse(strSaveStr.Text),
                SavingThrowDex = int.Parse(strSaveDex.Text),
                SavingThrowCon = int.Parse(strSaveCon.Text),
                SavingThrowInt = int.Parse(strSaveInt.Text),
                SavingThrowWis = int.Parse(strSaveWis.Text),
                SavingThrowCha = int.Parse(strSaveCha.Text),
                Blindsight = int.Parse(strBlindsight.Text),
                BlindBeyond = chkBlindBeyond.IsChecked.Value,
				Darkvision = int.Parse(strDarkvision.Text),
                Tremorsense = int.Parse(strTremorsense.Text),
                Truesight = int.Parse(strTruesight.Text),
                PassivePerception = int.Parse(strPassivePerception.Text),
                ChallengeRating = int.Parse(strChallenge.Text),
                XP = int.Parse(strExperience.Text),
                NPCToken = strNPCToken.Text,
                DamageResistance = listDamageResistance.SelectedItems.Cast<ListBoxItem>().Where(a => a.IsSelected).Select(a => (string)a.Content).ToList(),
                DamageVulnerability = listDamageVulnerability.SelectedItems.Cast<ListBoxItem>().Where(a => a.IsSelected).Select(a => (string)a.Content).ToList(),
                DamageImmunity = listDamageImmunity.SelectedItems.Cast<ListBoxItem>().Where(a => a.IsSelected).Select(a => (string)a.Content).ToList(),
                ConditionImmunity = listConditionImmunity.SelectedItems.Cast<ListBoxItem>().Where(a => a.IsSelected).Select(a => (string)a.Content).ToList(),
                SpecialWeaponResistance = listWeaponResistances.SelectedItems.Cast<ListBoxItem>().Where(a => a.IsSelected).Select(a => (string)a.Content).ToList(),
                SpecialWeaponImmunity = listWeaponImmunity.SelectedItems.Cast<ListBoxItem>().Where(a => a.IsSelected).Select(a => (string)a.Content).ToList(),
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
                TraitsDesc11 = strTraitDesc11.Text
            };

            npcController.Save(savePath, npcModel);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9-]+"); ;
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
