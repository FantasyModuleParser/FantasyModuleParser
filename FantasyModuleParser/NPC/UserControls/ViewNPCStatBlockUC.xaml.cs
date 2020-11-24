using FantasyModuleParser.NPC.Models.Skills;
using FantasyModuleParser.NPC.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace FantasyModuleParser.NPC.UserControls
{
    /// <summary>
    /// Interaction logic for ViewNPCStatBlockUC.xaml
    /// </summary>
    public partial class ViewNPCStatBlockUC : UserControl
    {
        public ViewNPCStatBlockUC()
        {
            InitializeComponent();

            viewModel = new PreviewNPCViewModel();
            viewModel.NPCModel.PropertyChanged += RefreshDataContext;

            setupPropertyChangedEventForLanguages(viewModel.NPCModel.StandardLanguages);
            setupPropertyChangedEventForLanguages(viewModel.NPCModel.ExoticLanguages);
            setupPropertyChangedEventForLanguages(viewModel.NPCModel.MonstrousLanguages);
            // Custom method for UserLanguages, as the user can add / remove languages at will

            if (viewModel.NPCModel.UserLanguages == null)
                viewModel.NPCModel.UserLanguages = new ObservableCollection<LanguageModel>();

            viewModel.NPCModel.UserLanguages.CollectionChanged += (sender, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (LanguageModel newItem in e.NewItems)
                    {
                        //Add listener for each item on PropertyChanged event
                        newItem.PropertyChanged += this.RefreshDataContext;
                    }
                }

                if (e.OldItems != null)
                {
                    foreach (LanguageModel oldItem in e.OldItems)
                    {
                        oldItem.PropertyChanged -= this.RefreshDataContext;
                    }
                }
            };

            DataContext = viewModel;
        }

        #region Controllers
        private PreviewNPCViewModel viewModel;
        #endregion

        public void RefreshDataContext(object sender, PropertyChangedEventArgs e)
        {
            // This is resource intensive way of forcing a refresh 
            viewModel = new PreviewNPCViewModel();
            DataContext = viewModel;
        }

        private void setupPropertyChangedEventForLanguages(ObservableCollection<LanguageModel> languageModels)
        {
            foreach (LanguageModel languageModel in languageModels)
            {
                languageModel.PropertyChanged += RefreshDataContext;
            }
        }
    }
}
