using FantasyModuleParser.NPC.Controllers;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace FantasyModuleParser.NPC.UserControls.NPCTabs
{
    /// <summary>
    /// Interaction logic for SpellcastingUC.xaml
    /// </summary>
    public partial class SpellcastingUC : UserControl
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
        private void PositiveNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text) || e.Text.Contains("-");
        }
        #endregion
        public SpellcastingUC()
        {
            InitializeComponent();
            npcController = new NPCController();
            //var npcModel = ((App)Application.Current).NpcModelObject;
            DataContext = npcController.GetNPCModel();
        }

        public void Refresh()
        {
            var npcModel = npcController.GetNPCModel();
            DataContext = npcController.GetNPCModel();
        }
    }
}
