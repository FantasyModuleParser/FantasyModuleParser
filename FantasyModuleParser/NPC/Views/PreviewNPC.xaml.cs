using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

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
            viewModel = new PreviewNPCViewModel();
            viewModel.NPCModel.PropertyChanged += RefreshDataContext;
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
	}
}
