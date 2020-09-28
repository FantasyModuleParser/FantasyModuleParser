﻿using FantasyModuleParser.Main.ViewModels;
using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

namespace FantasyModuleParser.Main.UserControls.Settings
{
    /// <summary>
    /// Interaction logic for SuiteSettingsUC.xaml
    /// </summary>
    public partial class SuiteSettingsUC : UserControl
    {
        public SuiteSettingsUC()
        {
            DataContext = new SettingsViewModel();

            InitializeComponent();
              
        }

        private void MainDefaultFolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SettingsViewModel viewModel = DataContext as SettingsViewModel;
            viewModel.ChangeDefaultMainFolder();
        }

        private void ProjectDefaultFolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SettingsViewModel viewModel = DataContext as SettingsViewModel;
            viewModel.ChangeDefaultProjectFolder(); // <-- Needs some input here
        }

        private void NPCDefaultFolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SettingsViewModel viewModel = DataContext as SettingsViewModel;
            viewModel.ChangeDefaultNPCFolder(); // <-- Needs some input here
        }
    }
}
