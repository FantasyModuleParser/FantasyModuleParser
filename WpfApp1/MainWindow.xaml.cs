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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        #region MenuOptions
        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ESManageCategories_Click(object sender, RoutedEventArgs e)
        {
            ESManageCategories win2 = new ESManageCategories();
            win2.Show();
        }
        private void ESSettings_Click(object sender, RoutedEventArgs e)
        {
            ESSettings win2 = new ESSettings();
            win2.Show();
        }
        private void ESManageProject_Click(object sender, RoutedEventArgs e)
        {
            ESProjectManagement win2 = new ESProjectManagement();
            win2.Show();
        }
        private void ESAbout_Click(object sender, RoutedEventArgs e)
        {
            ESAbout win2 = new ESAbout();
            win2.Show();
        }
        private void ESSupporters_Click(object sender, RoutedEventArgs e)
        {
            ESSupporters win2 = new ESSupporters();
            win2.Show();
        }
        #endregion
        #region ImageClicks
        private void NPCEngineer_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void ProjectEngineer_Click(object sender, RoutedEventArgs e)
        {
            ESProjectManagement win2 = new ESProjectManagement();
            win2.Show();
        }
        private void SpellEngineer_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void ArtifactEngineer_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void TableEngineer_Click(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        #endregion



        private void EquipmentEngineer_Click (object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void ParcelEngineer_Click (object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void RefManEngineer_Click (object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void CreateModule_Click (object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
    }


}
