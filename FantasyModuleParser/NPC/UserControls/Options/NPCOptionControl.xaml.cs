using FantasyModuleParser.Main;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action.Enums;
using FantasyModuleParser.NPC.Views;
using FantasyModuleParser.NPC.UserControls.NPCTabs;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static FantasyModuleParser.Extensions.EnumerationExtension;
using FantasyModuleParser.NPC.ViewModels;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;

namespace FantasyModuleParser.NPC.UserControls.Options
{
    /// <summary>
    /// Interaction logic for NPCOptionControl.xaml
    /// </summary>
    public partial class NPCOptionControl : UserControl
    {
		#region Controllers
		public NPCController npcController { get; set; }
		#endregion
		#region ViewModel
		private NPCOptionControlViewModel npcOptionControlViewModel;
        #endregion
        #region Variables
        string installPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		string installFolder = "FMP/NPC";
		private bool _isViewStatblockWindowOpen = false;
		#endregion
		public NPCOptionControl()
		{
			InitializeComponent();
			npcController = new NPCController();
			npcOptionControlViewModel = new NPCOptionControlViewModel();
			DataContext = npcOptionControlViewModel;
		}
		private void openfolder(string strPath, string strFolder)
		{
			var fPath = Path.Combine(strPath, strFolder);
			Directory.CreateDirectory(fPath);
			System.Diagnostics.Process.Start(fPath);
		}
		private void Menu_Click(object sender, RoutedEventArgs e)
		{
			var menuitem = (MenuItem)sender;
			switch (menuitem.Name)
			{
				case "About":
                    new About().ShowDialog();
					break;
				case "ManageCategories":
					new FMPConfigurationView().ShowDialog();
					break;
				case "ManageLanguages":
					new FMPConfigurationView().ShowDialog();
					break;
				case "ManageProject":
					new ProjectManagement().ShowDialog();
					break;
				case "ProjectManagement":
					new ProjectManagement().ShowDialog();
					break;
				case "Settings":
					new Settings().ShowDialog();
					break;
				case "Supporters":
					new Supporters().ShowDialog();
					break;
			}
		}
		#region MenuOptions
		
		#endregion
		#region Actions
		private void ImportText_Click(object sender, RoutedEventArgs e)
		{
			ImportText importTextWindow = new ImportText();
            importTextWindow.Closing += ImportTextWindow_Closing;
			new ImportText().Show();
		}

        private void ImportTextWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
			DataContext = npcController.GetNPCModel();
			BaseStatsUserControl.Refresh();
			SkillsUserControl.Refresh();
			SpellcastingUserControl.Refresh();
			TraitsUserControl.Refresh();
			InnateCastingUserControl.Refresh();
		}

        private void FGListOptions_Click(object sender, RoutedEventArgs e)
		{
			new FGListOptions().Show();
		}
		#endregion

		private void SaveNPCToFile(object sender, RoutedEventArgs e)
		{

			NPCModel npcModel = npcController.GetNPCModel();
			string savePath = Path.Combine(installPath, installFolder, npcModel.NPCName + ".json");

			((App)Application.Current).NpcModel = npcModel;
			npcController.Save(savePath, npcModel);

		}
		private void NewNPC_Click(object sender, RoutedEventArgs e)
		{
			npcController.ClearNPCModel();
			DataContext = npcController.GetNPCModel();
			BaseStatsUserControl.Refresh();
			SkillsUserControl.Refresh();
			SpellcastingUserControl.Refresh();
			TraitsUserControl.Refresh();
			InnateCastingUserControl.Refresh();
		}
		private void LoadNPCOption_Click(object sender, RoutedEventArgs e)
		{
			// Create OpenFileDialog
			Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

			// Launch OpenFileDialog by calling ShowDialog method
			Nullable<bool> result = openFileDlg.ShowDialog();
			// Get the selected file name and display in a TextBox.
			// Load content of file in a TextBlock
			if (result == true)
			{
				npcController.Load(openFileDlg.FileName);
				NPCModel npcModel = npcController.GetNPCModel();

				//Refresh all the data on the UI
				DataContext = npcModel;

				// TODO:  Get the active tab the user is on
				// As the assumption here is the User is on the Base Stats tab while loading
				// a NPC File
				BaseStatsUserControl.Refresh();
				SkillsUserControl.Refresh();
				SpellcastingUserControl.Refresh();
				TraitsUserControl.Refresh();
				InnateCastingUserControl.Refresh();
			}
		}

		private void PreviewNPC_Click(object sender, RoutedEventArgs e)
		{
			if (!_isViewStatblockWindowOpen)
			{
				_isViewStatblockWindowOpen = true;
				PreviewNPC previewNPC = new PreviewNPC();
				previewNPC.Closing += PreviewNPC_Closing;
				previewNPC.Show();
			}
		}

		private void PreviewNPC_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_isViewStatblockWindowOpen = false;
		}

		private void NPCOptionControl_Loaded(object sender, RoutedEventArgs e)
		{
			npcOptionControlViewModel.Refresh();
			DataContext = npcOptionControlViewModel;
		}

		private void AddToProjectButton_Click(object sender, RoutedEventArgs e)
		{
			if(FGCategoryComboBox.Items.Count == 0)
			{
				MessageBox.Show("No Module Project loaded!\nPlease create / load a Module through Options -> Manage Project");
				return;
			}

			ModuleService moduleService = new ModuleService();
			try { 
				moduleService.AddNPCToCategory(npcController.GetNPCModel(), (FGCategoryComboBox.SelectedItem as CategoryModel).Name);
				MessageBox.Show("NPC has been added to the project");
			}
			catch (Exception exception)
			{
				MessageBox.Show("Error detected while adding NPC to button :: " + exception.Message);
			}
		}

		public void Refresh()
		{
			npcOptionControlViewModel.Refresh();
			FGCategoryComboBox.ItemsSource = npcOptionControlViewModel.ModuleModel.Categories;
			FGCategoryComboBox.SelectedIndex = FGCategoryComboBox.Items.Count - 1;
			DataContext = npcOptionControlViewModel;
		}
	}
}
