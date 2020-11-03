using FantasyModuleParser.Main.ViewModels;
using FantasyModuleParser.NPC.Commands;
using FantasyModuleParser.Spells.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FantasyModuleParser.Main.UserControls
{
    /// <summary>
    /// Interaction logic for CustomCastersUC.xaml
    /// </summary>
    public partial class CustomCastersUC : UserControl
    {
        private CustomCastersUCViewModel customCastersUCViewModel;
        public CustomCastersUC()
        {
            InitializeComponent();
            customCastersUCViewModel = new CustomCastersUCViewModel();
            DataContext = customCastersUCViewModel;
            OnRemoveCustomCasterCommand = new ActionCommand(x => customCastersUCViewModel.RemoveCustomCaster(x.ToString()));
        }
        public ICommand OnRemoveCustomCasterCommand { get; set; }

        private void AddNewCustomCasterButton_Click(object sender, RoutedEventArgs e)
        {
            customCastersUCViewModel.AddCustomCaster(NewCustomCastersText.Text);
        }

        private void RemoveCustomCasterButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (sender as Button);
            CustomCastersModel customCastersModel = button.DataContext as CustomCastersModel;
            customCastersUCViewModel.RemoveCustomCaster(customCastersModel);
        }
    }
}
