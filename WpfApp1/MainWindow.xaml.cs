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
        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ESManageCategories_Click(object sender, RoutedEventArgs e)
        {
            ESManageCategories win2 = new ESManageCategories();
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
        private void OnImageButtonClick (object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void OnImageButtonClick1 (object sender, RoutedEventArgs e)
        {
            ESAbout win2 = new ESAbout();
            win2.Show();
        }
        private void OnImageButtonClick2 (object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void OnImageButtonClick3(object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void OnImageButtonClick4 (object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void OnImageButtonClick5 (object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void OnImageButtonClick6 (object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
        private void OnImageButtonClick7 (object sender, RoutedEventArgs e)
        {
            NPCEngineer win2 = new NPCEngineer();
            win2.Show();
        }
    }


}
