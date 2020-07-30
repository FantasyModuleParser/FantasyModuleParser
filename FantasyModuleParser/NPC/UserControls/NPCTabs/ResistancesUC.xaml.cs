using FantasyModuleParser.NPC.ViewModels;
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
        }

		public void Refresh()
		{
			(DataContext as ResistanceUserControlViewModel).Refresh();
		}

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
	}
}
