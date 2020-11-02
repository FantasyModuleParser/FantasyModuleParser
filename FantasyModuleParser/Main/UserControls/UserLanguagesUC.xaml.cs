using FantasyModuleParser.Main.ViewModels;
using FantasyModuleParser.NPC.Commands;
using FantasyModuleParser.NPC.Models.Skills;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FantasyModuleParser.Main.UserControls
{
    /// <summary>
    /// Interaction logic for UserLanguagesUC.xaml
    /// </summary>
    public partial class UserLanguagesUC : UserControl
    {
        private UserLanguagesUCViewModel userLanguagesUCViewModel;
        public UserLanguagesUC()
        {
            InitializeComponent();
            userLanguagesUCViewModel = new UserLanguagesUCViewModel();
            DataContext = userLanguagesUCViewModel;
            OnRemoveCategoryCommand = new ActionCommand(x => userLanguagesUCViewModel.RemoveUserLanguage(x.ToString()));
        }

        public ICommand OnRemoveCategoryCommand { get; set; }

        private void AddNewCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            userLanguagesUCViewModel.AddUserLanguage(NewUserLanguageText.Text);
        }

        private void RemoveCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (sender as Button);
            LanguageModel userLanguageModel = button.DataContext as LanguageModel;
            userLanguagesUCViewModel.RemoveUserLanguage(userLanguageModel);
        }
    }
}
