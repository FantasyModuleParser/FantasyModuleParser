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

namespace FantasyModuleParser.NPC.UserControls.NPCTabs
{
    /// <summary>
    /// Interaction logic for BaseStatsUC.xaml
    /// </summary>
    public partial class BaseStatsUC : UserControl
    {
        #region Controllers
        public NPCController npcController { get; set; }
		#endregion

		#region Variables
		string installPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		string installFolder = "FMP/NPC";
		#endregion

		#region Methods
		private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9-]+"); ;
            e.Handled = regex.IsMatch(e.Text);
        }

		private void StoreData(object sender, RoutedEventArgs e)
		{
			string npcName = NPC_name.Text;
			string savePath = Path.Combine(installPath, installFolder, npcName + ".json");
			string NPCName = NPC_name.Text;
			string Size = strSize.Text;
			string NPCType = strType.Text;
			string Tag = strTag.Text;
			string Alignment = strAlignment.Text;
			string AC = strAC.Text;
			string HP = strHP.Text;
			string NPCGender = strGender.Text;
			bool Unique = chkUnique.IsChecked.Value;
			bool NPCNamed = chkNamed.IsChecked.Value;
			int Speed = int.Parse(intSpeed.Text);
			int Burrow = int.Parse(intBurrow.Text);
			int Climb = int.Parse(intClimb.Text);
			int Fly = int.Parse(intFly.Text);
			int Swim = int.Parse(intSwim.Text);
			int AttributeStr = int.Parse(strAttrStr.Text);
			int AttributeDex = int.Parse(strAttrDex.Text);
			int AttributeCon = int.Parse(strAttrCon.Text);
			int AttributeInt = int.Parse(strAttrInt.Text);
			int AttributeWis = int.Parse(strAttrWis.Text);
			int AttributeCha = int.Parse(strAttrCha.Text);
			int SavingThrowStr = int.Parse(strSaveStr.Text);
			int SavingThrowDex = int.Parse(strSaveDex.Text);
			int SavingThrowCon = int.Parse(strSaveCon.Text);
			int SavingThrowInt = int.Parse(strSaveInt.Text);
			int SavingThrowWis = int.Parse(strSaveWis.Text);
			int SavingThrowCha = int.Parse(strSaveCha.Text);
			int Blindsight = int.Parse(strBlindsight.Text);
			bool BlindBeyond = chkBlindBeyond.IsChecked.Value;
			int Darkvision = int.Parse(strDarkvision.Text);
			int Tremorsense = int.Parse(strTremorsense.Text);
			int Truesight = int.Parse(strTruesight.Text);
			int PassivePerception = int.Parse(strPassivePerception.Text);
			int ChallengeRating = int.Parse(strChallenge.Text);
			int XP = int.Parse(strExperience.Text);
			string NPCToken = strNPCToken.Text;
		}
        #endregion
        public BaseStatsUC()
        {
            InitializeComponent();
            npcController = new NPCController();
			npcController.LoadNpcModelAction += NpcController_LoadNpcModelAction;
			DataContext = npcController.GetNPCModel();
        }

		private void NpcController_LoadNpcModelAction(object sender, EventArgs e)
		{
			DataContext = npcController.GetNPCModel();
		}

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
	}
}
