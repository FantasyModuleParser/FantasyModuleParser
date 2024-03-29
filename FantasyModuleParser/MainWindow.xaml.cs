﻿using FantasyModuleParser.Equipment.ViewModels;
using FantasyModuleParser.Exporters;
using FantasyModuleParser.Main;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.Main.Views;
using FantasyModuleParser.NPC.UserControls;
using FantasyModuleParser.NPC.UserControls.Options;
using FantasyModuleParser.Spells;
using FantasyModuleParser.Spells.UserControls;
using FantasyModuleParser.Spells.ViewModels;
using FantasyModuleParser.Tables.ViewModels;
using log4net;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private bool isViewStatBlockVisible = false;
        private readonly ModuleService moduleService;

        // TODO: Darkpool: reverting readonly status due to an anti-pattern, where the Save action
        //  should update moduleModel, but in reality it **should** have the moduleModel object
        //  itself call an update... maybe?  Need to discuss w/ Battlemarch
        private ModuleModel moduleModel;
        private SettingsModel settingsModel;
        private readonly SettingsService settingsService;
        private readonly SpellStatBlockUC spellStatBlockUC;
        private SpellViewModel spellViewModel;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MainWindow()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
            moduleService = new ModuleService();
            settingsService = new SettingsService();
            settingsModel = settingsService.Load();
            spellStatBlockUC = new SpellStatBlockUC();
            if(settingsModel.DefaultGUISelection != null && 
                settingsModel.DefaultGUISelection.Equals("NPCOption", StringComparison.CurrentCulture))
            {
                ShowNPCUserControl();
                optionNPC.IsSelected = true;
            }
            if(settingsModel.LastProject != null && settingsModel.LoadLastProject)
            {
                string lastModuleFilePath = Path.Combine(settingsModel.ProjectFolderLocation, settingsModel.LastProject + ".fmp");
                if(File.Exists(lastModuleFilePath)) 
                {
                    moduleService.Load(Path.Combine(settingsModel.ProjectFolderLocation, settingsModel.LastProject + ".fmp"));
                    RefreshModuleModelOnDataContext(false);
                }
            }
        }

        private void Directory_Click(object sender, RoutedEventArgs e)
        {
            var menuitem = (MenuItem)sender;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            settingsModel = settingsService.Load();
            switch (menuitem.Name)
            {
                case "AppData":
                    openFileDialog.InitialDirectory = CheckAndCreateDirectory(settingsModel.MainFolderLocation);
                    break;
                case "Projects":
                    openFileDialog.InitialDirectory = CheckAndCreateDirectory(settingsModel.ProjectFolderLocation);
                    break;
                case "Artifacts":
                    openFileDialog.InitialDirectory = CheckAndCreateDirectory(settingsModel.ArtifactFolderLocation);
                    break;
                case "Equipment":
                    openFileDialog.InitialDirectory = CheckAndCreateDirectory(settingsModel.EquipmentFolderLocation);
                    break;
                case "NPCs":
                    openFileDialog.InitialDirectory = CheckAndCreateDirectory(settingsModel.NPCFolderLocation);
                    break;
                case "Parcels":
                    openFileDialog.InitialDirectory = CheckAndCreateDirectory(settingsModel.ParcelFolderLocation);
                    break;
                case "Spells":
                    openFileDialog.InitialDirectory = CheckAndCreateDirectory(settingsModel.SpellFolderLocation);
                    break;
                case "Tables":
                    openFileDialog.InitialDirectory = CheckAndCreateDirectory(settingsModel.TableFolderLocation);
                    break;
                case "FGModules":
                    openFileDialog.InitialDirectory = CheckAndCreateDirectory(settingsModel.FGModuleFolderLocation);
                    break;
            }
            openFileDialog.ShowDialog();
        }
        private static string CheckAndCreateDirectory(string folderLocation)
        {
            if (!Directory.Exists(folderLocation))
            {
                log.Debug("Folder location is non-existant;  Creating folder :: " + folderLocation);
                Directory.CreateDirectory(folderLocation);
            }
            return folderLocation;
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
                    projectManagement.SaveAndCloseAction += ProjectManagement_OnSaveAndCloseAction;
                    projectManagement.Show();
                    break;
                case "ProjectManagement":
                    projectManagement = new ProjectManagement();
                    projectManagement.OnCloseWindowAction += ProjectManagement_OnCloseWindowAction;
                    projectManagement.SaveAndCloseAction += ProjectManagement_OnSaveAndCloseAction;
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
            RefreshModuleModelOnDataContext(false);
        }

        private void ProjectManagement_OnSaveAndCloseAction(object sender, EventArgs e)
        {
            RefreshModuleModelOnDataContext(true);
        }

        private void RefreshModuleModelOnDataContext(bool forceRefresh)
        {
            moduleModel = moduleService.GetModuleModel();
            npcOptionUserControl.Refresh(forceRefresh);
            (spellOptionUserControl.DataContext as SpellViewModel).Refresh();
            (tableOptionUserControl.DataContext as TableOptionViewModel).Refresh();
            (equipmentUserControl.DataContext as EquipmentOptionControlViewModel).Refresh();
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
            MessageBox.Show("Module Created Successfully");
        }

        private void CreateCampaign_Click(object sender, RoutedEventArgs e)
        {
            // TODO:  This is a prime example of how Dependency Injection would be 
            // amazing!  each exporter uses the interface IExporter, so the DI system
            // could be updated based on the user's selection from somewhere else

            // For now, just infer that IExporter = new FantasyGroundsExporter;

            ICampaign exporter = new FantasyGroundsCampaign();

            // DI would be used here to get an singleton instance of ModuleService (acting almost as a factory...)
            ModuleService moduleService = new ModuleService();

            //moduleService.GetModuleModel() refers to the static instantiation of ModuleModel, which is modified
            // throughout the application

            exporter.CreateCampaign(moduleService.GetModuleModel());
            MessageBox.Show("Campaign Created Successfully");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            log.Info("=====  Fantasy Module Parser closed  ======");
            log.Info("=============  Logging Ended  =============");
            System.Windows.Application.Current.Shutdown();
        }
        private void ShowNPCUserControl()
        {
            HideMainUserControls();
            stackNPC.Visibility = Visibility.Visible;
        }
        private void ShowSpellUserControl()
        {
            HideMainUserControls();
            stackSpells.Visibility = Visibility.Visible;
            spellViewModel = spellOptionUserControl.DataContext as SpellViewModel;
            spellStatBlockUC.DataContext = spellViewModel;
        }

        private void ShowTableUserControl()
        {
            HideMainUserControls();
            stackTable.Visibility = Visibility.Visible;
        }

        private void ShowEquipmentUserControl()
        {
            HideMainUserControls();
            stackEquipment.Visibility = Visibility.Visible;
        }

        private void HideMainUserControls()
        {
            stackNPC.Visibility = Visibility.Hidden;
            stackMain.Visibility = Visibility.Hidden;
            stackSpells.Visibility = Visibility.Hidden;
            stackTable.Visibility = Visibility.Hidden;
            stackEquipment.Visibility = Visibility.Hidden;
        }
        private void listBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            if (optionNPC.IsSelected)
            {
                ShowNPCUserControl();
            }
            if (optionSpells.IsSelected)
            {
                ShowSpellUserControl();
            }
            if (optionTable.IsSelected)
            {
                ShowTableUserControl();
            }
            if (optionEquipment.IsSelected)
            {
                ShowEquipmentUserControl();
            }
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

                    if (isViewStatBlockVisible)     { ViewStatBlockPanel.Children.Add(new ViewNPCStatBlockUC()); }
                    break;
                case nameof(SpellOptionControl):
                    // TODO:  Create the Stat Block for Spells and add it here (uncomment the break when doing so)
                    this.Width += 450 * (isViewStatBlockVisible ? 1 : -1);

                    if (isViewStatBlockVisible)     { ViewStatBlockPanel.Children.Add(spellStatBlockUC); }
                    break;
                //break;
                default:
                    // Reset the width to the default of 810
                    this.Width = 810;
                    break;
            }
        }

        private void event_UpdateViewStatBlockPanel(object sender, EventArgs eventArgs)
        {
            SpellViewModel spellViewModel = spellStatBlockUC.DataContext as SpellViewModel;
            spellViewModel.Refresh();
        }
    }
}
