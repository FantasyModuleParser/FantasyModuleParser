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
		#region MenuOptions
		private void EditDeleteNPC_Click(object sender, RoutedEventArgs e)
		{
			EditDeleteNPC win2 = new EditDeleteNPC();
			win2.Show();
		}
		#endregion
		#region Actions
		private void ImportText_Click(object sender, RoutedEventArgs e)
		{
			new ImportText().Show();
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
		private void AddToProject_Click(object sender, RoutedEventArgs e)
		{

		}

		private void PreviewNPC_Click(object sender, RoutedEventArgs e)
		{
			new PreviewNPC().Show();
		}

		private void NPCOptionControl_Loaded(object sender, RoutedEventArgs e)
		{
			npcOptionControlViewModel.Refresh();
			DataContext = npcOptionControlViewModel;
		}

		// For some reason I don't know, the ItemsSource binding does not work in XAML for getting the
		// updated list of Categories based on the loaded module.  This is the alternate way for the UI
		// to be refreshed (FGCategoryComboBox_MouseDownClick && FGCategoryComboBox_PreviewKeyDown)
		private void FGCategoryComboBox_MouseDownClick(object sender, MouseButtonEventArgs e)
		{
			npcOptionControlViewModel.Refresh();
			FGCategoryComboBox.ItemsSource = npcOptionControlViewModel.ModuleModel.Categories;
			FGCategoryComboBox.SelectedIndex = FGCategoryComboBox.Items.Count - 1;
		}

		private void FGCategoryComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			npcOptionControlViewModel.Refresh();
			FGCategoryComboBox.ItemsSource = npcOptionControlViewModel.ModuleModel.Categories;
			FGCategoryComboBox.SelectedIndex = FGCategoryComboBox.Items.Count - 1;
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
			moduleService.AddNPCToCategory(npcController.GetNPCModel(), 
				(FGCategoryComboBox.SelectedItem as CategoryModel).Name);
			}
			catch (Exception exception)
			{
				MessageBox.Show("Error detected while adding NPC to button :: " + exception.Message);
			}
		}
	}
}
