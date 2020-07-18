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
using Microsoft.Win32;
using System.Text;

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
		private bool _isViewDiceFunctionWindowOpen = false;
        #endregion

        #region Methods
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
			Regex regex = new Regex(@"[^0-9-]+"); ;
			e.Handled = regex.IsMatch(e.Text);
		}
		private void PositiveNumberValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex(@"[^0-9-]+");
			e.Handled = regex.IsMatch(e.Text) || e.Text.Contains("-");
		}
		#endregion
		public BaseStatsUC()
        {
            InitializeComponent();
            npcController = new NPCController();
			DataContext = npcController.GetNPCModel();
        }
		public void Refresh()
		{
			DataContext = npcController.GetNPCModel();
		}

		#region BaseStatChange
		private void StrengthScore_TextChanged(object sender, RoutedEventArgs e)
		{
			int num;
			if (int.TryParse(strAttrStr.Text, out num))
			{
				int answer = -5 + (num / 2);
				StringBuilder stringBuilder = new StringBuilder();
				if (num < 10)
				{
					strModStr.Content = answer;
				}
				else
				{
					strModStr.Content = stringBuilder.Append("+" + answer);
				}
			}
		}
		
		private void DexterityScore_TextChanged(object sender, RoutedEventArgs e)
		{
			int num;
			if (int.TryParse(strAttrDex.Text, out num))
			{
				int answer = -5 + (num / 2);
				StringBuilder stringBuilder = new StringBuilder();
				if (num < 10)
				{
					strModDex.Content = answer;
				}
				else
				{
					strModDex.Content = stringBuilder.Append("+" + answer);
				}
			}
		}
		private void ConstitutionScore_TextChanged(object sender, RoutedEventArgs e)
		{
			int num;
			if (int.TryParse(strAttrCon.Text, out num))
			{
				int answer = -5 + (num / 2);
				StringBuilder stringBuilder = new StringBuilder();
				if (num < 10)
				{
					strModCon.Content = answer;
				}
				else
				{
					strModCon.Content = stringBuilder.Append("+" + answer);
				}
			}
		}
		private void IntelligenceScore_TextChanged(object sender, RoutedEventArgs e)
		{
			int num;
			if (int.TryParse(strAttrInt.Text, out num))
			{
				int answer = -5 + (num / 2);
				StringBuilder stringBuilder = new StringBuilder();
				if (num < 10)
				{
					strModInt.Content = answer;
				}
				else
				{
					strModInt.Content = stringBuilder.Append("+" + answer);
				}
			}
		}
		private void WisdomScore_TextChanged(object sender, RoutedEventArgs e)
		{
			uint num;
			if (uint.TryParse(strAttrWis.Text, out num))
			{
				int answer = (int)(-5 + (num / 2));
				StringBuilder stringBuilder = new StringBuilder();
				if (num < 10)
				{
					strModWis.Content = answer;
				}
				else
				{
					strModWis.Content = stringBuilder.Append("+" + answer);
				}
			}
		}
		private void CharismaScore_TextChanged(object sender, RoutedEventArgs e)
		{
			uint num;
			if (uint.TryParse(strAttrCha.Text, out num))
			{
				int answer = (int)(-5 + (num / 2));
				StringBuilder stringBuilder = new StringBuilder();
				if (num < 10)
				{
					strModCha.Content = answer;
				}
				else
				{
					strModCha.Content = stringBuilder.Append("+" + answer);
				}
			}
		}
		#endregion

		private void DiceRoller_Click(object sender, RoutedEventArgs e)
		{
			if (!_isViewDiceFunctionWindowOpen)
			{
				_isViewDiceFunctionWindowOpen = true;
				DiceFunction diceFunction = new DiceFunction();
				diceFunction.Closing += DiceFunction_Closing;
				diceFunction.Show();
			}
		}

		private void DiceFunction_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_isViewDiceFunctionWindowOpen = false;
		}

		private void strNPCToken_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
			if (openFileDialog.ShowDialog() == true)
				strNPCToken.Text = openFileDialog.FileName;
		}
    }
}
