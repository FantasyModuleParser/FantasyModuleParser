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
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is CategoriesUCViewModel)
                viewModel = (CategoriesUCViewModel)this.DataContext;

            viewModel.AddCategoryToModule(NewCategoryValue.Text);
        }

    }
}
