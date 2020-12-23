using FantasyModuleParser.NPC.Controllers;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FantasyModuleParser.NPC.UserControls.NPCTabs
{
    /// <summary>
    /// Interaction logic for InnateCastingUC.xaml
    /// </summary>
    public partial class InnateCastingUC : UserControl
    {
        #region Methods
        private void InnateSpellcasting_Click(object sender, RoutedEventArgs e)
        {
            strComponentText.Text = "requiring no material components";
        }
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

        private NPCController npcController;
        public InnateCastingUC()
        {
            InitializeComponent();
            npcController = new NPCController();
            DataContext = npcController.GetNPCModel();
        }

        public void Refresh()
        {
            DataContext = npcController.GetNPCModel();
            //(DataContext as InnateSpellcastingViewModel).Refresh()k;
        }
    }
}
