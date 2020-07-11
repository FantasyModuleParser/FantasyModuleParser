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
using System.Windows.Shapes;

namespace FantasyModuleParser.Main
{
    /// <summary>
    /// Interaction logic for FMPConfigurationView.xaml
    /// </summary>
    public partial class FMPConfigurationView : Window
    {
        private FMPConfigurationViewModel configurationViewModel;
        public FMPConfigurationView()
        {
            InitializeComponent();

            // Enable it so the popup window can close on the Escape key
            this.PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };

            configurationViewModel = new FMPConfigurationViewModel();
            DataContext = configurationViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            configurationViewModel.Refresh();
            DataContext = configurationViewModel;
        }
    }
}
