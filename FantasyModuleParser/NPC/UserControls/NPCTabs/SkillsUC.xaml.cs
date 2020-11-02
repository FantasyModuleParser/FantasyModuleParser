using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Skills;
using System.Collections.ObjectModel;
using System.Linq;
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
            //DataContext = npcController.GetNPCModel();

            DataContext = npcModel;
        }

        public void Refresh()
        {
            var npcModel = npcController.GetNPCModel();
            npcModel = initializeLanguageSelection(npcModel);
            //DataContext = npcController.GetNPCModel();
            DataContext = npcModel;
        }

        private NPCModel initializeLanguageSelection(NPCModel npcModel)
        {
            npcModel.StandardLanguages = initSpecificLanguageSet(languageController.GenerateStandardLanguages(), npcModel.StandardLanguages);
            npcModel.MonstrousLanguages = initSpecificLanguageSet(languageController.GenerateMonsterLanguages(), npcModel.MonstrousLanguages);
            npcModel.ExoticLanguages = initSpecificLanguageSet(languageController.GenerateExoticLanguages(), npcModel.ExoticLanguages);
            npcModel.UserLanguages = initSpecificLanguageSet(languageController.GenerateUserLanguages(), npcModel.UserLanguages);

            return npcModel;
        }

        // Merges an existing set of NPC language models w/ the default set (whether hardcoded or defined by users)
        private ObservableCollection<LanguageModel> initSpecificLanguageSet(ObservableCollection<LanguageModel> defaultLanguages, ObservableCollection<LanguageModel> npcModelLanguages)
        {
            if (npcModelLanguages == null || npcModelLanguages.Count == 0)
            {
                npcModelLanguages = defaultLanguages;
            }
            else
            {
                foreach (LanguageModel userLanguage in defaultLanguages)
                {
                    if (npcModelLanguages.FirstOrDefault(
                            lang => lang.Language.ToLower().Equals(userLanguage.Language.ToLower())) == null)
                    {
                        npcModelLanguages.Add(userLanguage);
                    }
                }
            }

            return npcModelLanguages;
        }
    }
}
