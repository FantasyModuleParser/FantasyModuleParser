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
using FantasyModuleParser;
using FantasyModuleParser.Exporters;
using FantasyModuleParser.Main;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.UserControls.Options;

namespace FantasyModuleParser
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
            OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP");
        }
        private void Projects_Click(object sender, RoutedEventArgs e)
        {
                OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP/Projects");
        }
        private void Artifacts_Click(object sender, RoutedEventArgs e)
        {
                OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP/Artifacts");
        }
        private void Equipment_Click(object sender, RoutedEventArgs e)
        {
            OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP/Equipment");
        }
        private void NPC_Click(object sender, RoutedEventArgs e)
        {
            OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP/NPC");
        }
        private void Parcel_Click(object sender, RoutedEventArgs e)
        {
            OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP/Parcel");
        }
        private void Spell_Click(object sender, RoutedEventArgs e)
        {
            OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP/Spell");
        }
        private void Table_Click(object sender, RoutedEventArgs e)
        {
            OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FMP/Table");
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
                case "ManageCategories":
                    new FMPConfigurationView().Show();
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
                case "Supporters":
                    new Supporters().Show();
                    break;
            }
        }

        private void listBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            if (optionNPC.IsSelected == true)
            {
                stackNPC.Visibility = Visibility.Visible;
                stackMain.Visibility = Visibility.Hidden;
            }
        }

        private void CreateModule_Click(object sender, RoutedEventArgs e)
        {
            // TODO:  This is a prime example of how Dependency Injection would be 
            // amazing!  each exporter uses the interface IExporter, so the DI system
            // could be updated based on the user's selection from somewhere else

            // For now, just infer that IExporter = new FantasyGroundsExporter;

            IExporter exporter = new FantasyGroundsExporter();

            // DI would be used here to get an singleton instance of ModuleService (acting almost as a factory...)
            ModuleService moduleService = new ModuleService();

            //moduleService.GetModuleModel() refers to the static instantiation of ModuleModel, which is modified
            // throughout the application

            exporter.CreateModule(moduleService.GetModuleModel());
        }
    }
}
