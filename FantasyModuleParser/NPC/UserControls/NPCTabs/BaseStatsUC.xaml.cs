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

		#region Methods
		private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9-]+"); ;
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion
        public BaseStatsUC()
        {
            InitializeComponent();
            npcController = new NPCController();
			// NPCModel npcModel = ((App)Application.Current).NpcModel;
			DataContext = npcController.GetNPCModel();
			npcController.LoadNpcModelAction += NpcController_LoadNpcModelAction;
        }

		private void NpcController_LoadNpcModelAction(object sender, EventArgs e)
		{
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
