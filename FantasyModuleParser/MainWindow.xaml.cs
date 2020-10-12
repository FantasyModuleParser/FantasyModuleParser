using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
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
using FantasyModuleParser.Main.Views;
using FantasyModuleParser.NPC.UserControls;
using FantasyModuleParser.Spells;

namespace FantasyModuleParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private bool isViewStatBlockVisible = false;
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
            ProjectManagement projectManagement = null;
            var menuitem = (MenuItem)sender;
            switch (menuitem.Name)
            {
                case "About":
                    new About().ShowDialog();
                    break;
                case "ManageCategories":
                    new FMPConfigurationView().ShowDialog();
                    break;
                case "ManageProject":
                    projectManagement = new ProjectManagement();
                    projectManagement.OnCloseWindowAction += ProjectManagement_OnCloseWindowAction;
                    projectManagement.Show();
                    break;
                case "ProjectManagement":
                    projectManagement = new ProjectManagement();
                    projectManagement.OnCloseWindowAction += ProjectManagement_OnCloseWindowAction;
                    projectManagement.ShowDialog();
                    break;
                case "Settings":
                    new Settings().ShowDialog();
                    break;
                case "Supporters":
                    new Supporters().ShowDialog();
                    break;
                case "Changelog":
                    new Changelog().ShowDialog();
                    break;
                case "Exit":
                    Close();
                    break;
            }
        }

        private void ProjectManagement_OnCloseWindowAction(object sender, EventArgs e)
        {
            npcOptionUserControl.Refresh();
        }

        private void listBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            if (optionNPC.IsSelected == true)
            {
                stackNPC.Visibility = Visibility.Visible;
                stackMain.Visibility = Visibility.Hidden;
                stackSpells.Visibility = Visibility.Hidden;
            }
            if (optionSpells.IsSelected == true)
            {
                stackNPC.Visibility = Visibility.Hidden;
                stackMain.Visibility = Visibility.Hidden;
                stackSpells.Visibility = Visibility.Visible;
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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            System.Windows.Application.Current.Shutdown();
        }

        private void event_EnableViewStatBlockPanel(object sender, EventArgs e)
        {
            ViewStatBlockPanel.Children.Clear();
            isViewStatBlockVisible = !isViewStatBlockVisible;

            switch (sender.GetType().Name)
            {
                case nameof(NPCOptionControl):
                    // Shrink / Grow the main window based on the ViewStatBlock
                    this.Width += 450 * (isViewStatBlockVisible ? 1 : -1);

                    if (isViewStatBlockVisible)
                        ViewStatBlockPanel.Children.Add(new ViewNPCStatBlockUC());
                    break;
                case nameof(SpellOptionControl):
                    // TODO:  Create the Stat Block for Spells and add it here (uncomment the break when doing so)
                    //break;
                default:
                    // Reset the width to the default of 810
                    this.Width = 810;
                    break;
            }
            
        }
    }
}
