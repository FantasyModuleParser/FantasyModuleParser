using FantasyModuleParser.Main.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.Main.UserControls
{
    /// <summary>
    /// Interaction logic for CategoriesUC.xaml
    /// </summary>
    public partial class CategoriesUC : UserControl
    {
        private CategoriesUCViewModel viewModel;
        public CategoriesUC()
        {
            InitializeComponent();
            viewModel = new CategoriesUCViewModel();
            DataContext = viewModel;
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AddCategoryToModule(NewCategoryValue.Text);
        }
    }
}
