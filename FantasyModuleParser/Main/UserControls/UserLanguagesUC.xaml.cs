using FantasyModuleParser.Commands;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.ViewModels;
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
            UserLanguageModel userLanguageModel = button.DataContext as UserLanguageModel;
            userLanguagesUCViewModel.RemoveUserLanguage(userLanguageModel);
        }
    }
}
