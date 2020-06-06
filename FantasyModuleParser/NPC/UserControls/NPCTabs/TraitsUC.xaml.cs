using FantasyModuleParser.NPC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FantasyModuleParser.NPC.UserControls.NPCTabs
{
    /// <summary>
    /// Interaction logic for TraitsUC.xaml
    /// </summary>
    public partial class TraitsUC : UserControl
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
		public TraitsUC()
        {
            InitializeComponent();
            npcController = new NPCController();
            //var npcModel = ((App)Application.Current).NpcModelObject;
            DataContext = npcController.GetNPCModel();
        }
    }
}
