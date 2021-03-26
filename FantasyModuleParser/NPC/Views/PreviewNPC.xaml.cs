using FantasyModuleParser.NPC.Models.Skills;
using FantasyModuleParser.NPC.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace FantasyModuleParser.NPC.Views
{
    /// <summary>
    /// Interaction logic for PreviewNPC.xaml
    /// </summary>
    
    public partial class PreviewNPC : Window
    {

        #region Controllers
        private PreviewNPCViewModel viewModel;
        #endregion
        public PreviewNPC()
        {
            InitializeComponent();

            // Enable it so the popup window can close on the Escape key
            PreviewKeyDown += (sender, eventArgs) => { if (eventArgs.Key == Key.Escape) Close(); };

            viewModel = new PreviewNPCViewModel();
            viewModel.NpcModel.PropertyChanged += RefreshDataContext;

            setupPropertyChangedEventForLanguages(viewModel.NpcModel.StandardLanguages);
            setupPropertyChangedEventForLanguages(viewModel.NpcModel.ExoticLanguages);
            setupPropertyChangedEventForLanguages(viewModel.NpcModel.MonstrousLanguages);
            // Custom method for UserLanguages, as the user can add / remove languages at will

            if(viewModel.NpcModel.UserLanguages == null)
                viewModel.NpcModel.UserLanguages = new ObservableCollection<LanguageModel>();
                
            viewModel.NpcModel.UserLanguages.CollectionChanged += (sender, e) =>
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
        
        public void WindowClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

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
