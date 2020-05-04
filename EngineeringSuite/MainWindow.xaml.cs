using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using EngineeringSuite.Main;
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
        private void OpenFolder(string strPath, string strFolder)
        {
            var fPath = System.IO.Path.Combine(strPath, strFolder);
            System.IO.Directory.CreateDirectory(fPath);
            System.Diagnostics.Process.Start(fPath);
        }
        private void AppData_Click(object sender, RoutedEventArgs e)
        {
            OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite");
        }
        private void Projects_Click(object sender, RoutedEventArgs e)
        {
                OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/Projects");
        }
        private void Artifacts_Click(object sender, RoutedEventArgs e)
        {
                OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/Artifacts");
        }
        private void Equipment_Click(object sender, RoutedEventArgs e)
        {
            OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/Equipment");
        }
        private void NPC_Click(object sender, RoutedEventArgs e)
        {
            OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/NPC");
        }
        private void Parcel_Click(object sender, RoutedEventArgs e)
        {
            OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/Parcel");
        }
        private void Spell_Click(object sender, RoutedEventArgs e)
        {
            OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/Spell");
        }
        private void Table_Click(object sender, RoutedEventArgs e)
        {
            OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Engineer Suite/Table");
        }
        private void FG_Click(object sender, RoutedEventArgs e)
        {
            OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Fantasy Grounds");
        }
        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            var menuitem = (MenuItem)sender;
            switch (menuitem.Name)
            {
                case "About":
                    new About().Show();
                    break;
                case "Exit":
                    this.Close();
                    break;
                case "ManageCategories":
                    new ManageCategories().Show();
                    break;
                case "ManageProject":
                    new ProjectManagement().Show();
                    break;
                case "ProjectManagement":
                    new ProjectManagement().Show();
                    break;
                case "Settings":
                    new Settings().Show();
                    break;
                case "RefManEngineer":
                    new Engineer().Show();
                    break;
                case "SpellEngineer":
                    new Engineer().Show();
                    break;
                case "Supporters":
                    new Supporters().Show();
                    break;
                case "TableEngineer":
                    new Engineer().Show();
                    break;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            switch (button.Name)
            {
                case "ArtifactEngineer":
                    new Engineer().Show();
                    break;
                case "EquipmentEngineer":
                    new Engineer().Show();
                    break;
                case "Engineer":
                    new Engineer().Show();
                    break;
                case "ParcelEngineer":
                    new ProjectManagement().Show();
                    break;
                case "ProjectManagement":
                    new ProjectManagement().Show();
                    break;
                case "RefManEngineer":
                    new Engineer().Show();
                    break;
                case "SpellEngineer":
                    new Engineer().Show();
                    break;
                case "TableEngineer":
                    new Engineer().Show();
                    break;
            }
        }
    }
}
