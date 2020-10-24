using FantasyModuleParser.NPC.Controllers;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace FantasyModuleParser.NPC.UserControls.NPCTabs
{
    /// <summary>
    /// Interaction logic for SkillsUC.xaml
    /// </summary>
    public partial class SkillsUC : UserControl
    {
        #region Controllers
        public NPCController npcController { get; set; }
        public LanguageController languageController { get; set; }
        #endregion

        #region Methods
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9-]+"); ;
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion
        public SkillsUC()
        {
            InitializeComponent();
            npcController = new NPCController();
            languageController = new LanguageController();
            var npcModel = npcController.GetNPCModel();
            npcModel = initializeLanguageSelection(npcModel);
            DataContext = npcController.GetNPCModel();
        }

        public void Refresh()
        {
            var npcModel = npcController.GetNPCModel();
            npcModel = initializeLanguageSelection(npcModel);
            DataContext = npcController.GetNPCModel();
        }

        private NPCModel initializeLanguageSelection(NPCModel npcModel)
        {
            if (npcModel.StandardLanguages == null || npcModel.StandardLanguages.Count == 0)
            {
                npcModel.StandardLanguages = languageController.GenerateStandardLanguages();
            }
            if (npcModel.ExoticLanguages == null || npcModel.ExoticLanguages.Count == 0)
            {
                npcModel.ExoticLanguages = languageController.GenerateExoticLanguages();
            }
            if (npcModel.MonstrousLanguages == null || npcModel.MonstrousLanguages.Count == 0)
            {
                npcModel.MonstrousLanguages = languageController.GenerateMonsterLanguages();
            }
            if (npcModel.UserLanguages == null || npcModel.UserLanguages.Count == 0)
            {
                npcModel.UserLanguages = languageController.GenerateUserLanguages();
            }
            return npcModel;
        }
    }
}
