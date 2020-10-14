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
using FantasyModuleParser.Main.Models;
using Microsoft.Win32;

namespace FantasyModuleParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private bool isViewStatBlockVisible = false;
        private SettingsModel settingsModel;
        private SettingsService settingsService;
        

        public MainWindow()
        {
            InitializeComponent();
            settingsService = new SettingsService();
        }

        private void Directory_Click(object sender, RoutedEventArgs e)
        {
            var menuitem = (MenuItem)sender;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            settingsModel = settingsService.Load();
            switch (menuitem.Name)
            {
                case "AppData":
                    if (!Directory.Exists(settingsModel.MainFolderLocation))
                    {
                        Directory.CreateDirectory(settingsModel.MainFolderLocation);
                    }
                    openFileDialog.InitialDirectory = settingsModel.MainFolderLocation;
                    break;
                case "Projects":
                    if (!Directory.Exists(settingsModel.ProjectFolderLocation))
                    {
                        Directory.CreateDirectory(settingsModel.ProjectFolderLocation);
                    }
                    openFileDialog.InitialDirectory = settingsModel.ProjectFolderLocation;
                    break;
                case "Artifacts":
                    if (!Directory.Exists(settingsModel.ArtifactFolderLocation))
                    {
                        Directory.CreateDirectory(settingsModel.ArtifactFolderLocation);
                    }
                    openFileDialog.InitialDirectory = settingsModel.ArtifactFolderLocation;
                    break;
                case "Equipment":
                    if (!Directory.Exists(settingsModel.EquipmentFolderLocation))
                    {
                        Directory.CreateDirectory(settingsModel.EquipmentFolderLocation);
                    }
                    openFileDialog.InitialDirectory = settingsModel.EquipmentFolderLocation;
                    break;
                case "NPCs":
                    if (!Directory.Exists(settingsModel.NPCFolderLocation))
                    {
                        Directory.CreateDirectory(settingsModel.NPCFolderLocation);
                    }
                    openFileDialog.InitialDirectory = settingsModel.NPCFolderLocation;
                    break;
                case "Parcels":
                    if (!Directory.Exists(settingsModel.ParcelFolderLocation))
                    {
                        Directory.CreateDirectory(settingsModel.ParcelFolderLocation);
                    }
                    openFileDialog.InitialDirectory = settingsModel.ParcelFolderLocation;
                    break;
                case "Spells":
                    if (!Directory.Exists(settingsModel.SpellFolderLocation))
                    {
                        Directory.CreateDirectory(settingsModel.SpellFolderLocation);
                    }
                    openFileDialog.InitialDirectory = settingsModel.SpellFolderLocation;
                    break;
                case "Tables":
                    if (!Directory.Exists(settingsModel.TableFolderLocation))
                    {
                        Directory.CreateDirectory(settingsModel.TableFolderLocation);
                    }
                    openFileDialog.InitialDirectory = settingsModel.TableFolderLocation;
                    break;
                case "FGModules":
                    if (!Directory.Exists(settingsModel.FGModuleFolderLocation))
                    {
                        Directory.CreateDirectory(settingsModel.FGModuleFolderLocation);
                    }
                    openFileDialog.InitialDirectory = settingsModel.FGModuleFolderLocation;
                    break;
            }
            openFileDialog.ShowDialog();
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
