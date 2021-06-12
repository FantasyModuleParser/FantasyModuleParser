using System.Windows.Controls;

namespace FantasyModuleParser.Equipment
{
    /// <summary>
    /// Interaction logic for EquipmentOptionControl.xaml
    /// </summary>
    public partial class EquipmentOptionControl : UserControl
    {
        public EquipmentOptionControl()
        {
            InitializeComponent();
        }

        private void PrimaryEquipmentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void EquipmentFooterUC_SaveEquipmentAction(object sender, System.EventArgs e)
        {

        }

        private void EquipmentFooterUC_PrevEquipmentAction(object sender, System.EventArgs e)
        {
            WeaponUserControl.Refresh();
        }

        private void EquipmentFooterUC_NextEquipmentAction(object sender, System.EventArgs e)
        {
            WeaponUserControl.Refresh();
        }
    }
}
