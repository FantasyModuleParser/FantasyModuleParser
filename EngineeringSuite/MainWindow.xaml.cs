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
using EngineeringSuite;
using EngineeringSuite.NPC;

namespace EngineeringSuite
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
        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            var menuitem = (MenuItem)sender;
            switch (menuitem.Name)
            {
                case "About":
                    new ESAbout().Show();
                    break;
                case "Exit":
                    this.Close();
                    break;
                case "ManageCategories":
                    new ESManageCategories().Show();
                    break;
                case "ManageProject":
                    new ESProjectManagement().Show();
                    break;
                case "ProjectManagement":
                    new ESProjectManagement().Show();
                    break;
                case "Settings":
                    new ESSettings().Show();
                    break;
                case "RefManEngineer":
                    new NPCEngineer().Show();
                    break;
                case "SpellEngineer":
                    new NPCEngineer().Show();
                    break;
                case "Supporters":
                    new ESSupporters().Show();
                    break;
                case "TableEngineer":
                    new NPCEngineer().Show();
                    break;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            switch (button.Name)
            {
                case "ArtifactEngineer":
                    new NPCEngineer().Show();
                    break;
                case "EquipmentEngineer":
                    new NPCEngineer().Show();
                    break;
                case "NPCEngineer":
                    new NPCEngineer().Show();
                    break;
                case "ParcelEngineer":
                    new ESProjectManagement().Show();
                    break;
                case "ProjectManagement":
                    new ESProjectManagement().Show();
                    break;
                case "RefManEngineer":
                    new NPCEngineer().Show();
                    break;
                case "SpellEngineer":
                    new NPCEngineer().Show();
                    break;
                case "TableEngineer":
                    new NPCEngineer().Show();
                    break;
            }
        }
    }
}
