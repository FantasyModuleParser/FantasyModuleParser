using FantasyModuleParser.Main;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.UserControls.NPCTabs;
using FantasyModuleParser.NPC.ViewModels;
using FantasyModuleParser.NPC.Views;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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
		private int categoryIndex = 0;
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
			importTextWindow.IsVisibleChanged += ImportTextWindow_IsVisibleChanged;
			importTextWindow.Show();
		}

		private void ImportTextWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
			(DataContext as NPCOptionControlViewModel).Refresh();
			RefreshUserControls();
		}

        private void FGListOptions_Click(object sender, RoutedEventArgs e)
		{
			new FGListOptions().Show();
		}
		#endregion

		private void SaveNPCToFile(object sender, RoutedEventArgs e)
		{

			NPCModel npcModel = npcController.GetNPCModel();
			string saveDirectory = Path.Combine(installPath, installFolder);
			string savePath = Path.Combine(saveDirectory, npcModel.NPCName + ".json");

			((App)Application.Current).NpcModel = npcModel;
			if (Directory.Exists(saveDirectory))
            {
				npcController.Save(savePath, npcModel);
				MessageBox.Show("NPC Saved Successfully");
			}
			else
            {
				Directory.CreateDirectory(saveDirectory);
				npcController.Save(savePath, npcModel);
				MessageBox.Show("NPC Saved Successfully");
			}
		}
		private void NewNPC_Click(object sender, RoutedEventArgs e)
		{
			npcController.ClearNPCModel();
			//DataContext = npcController.GetNPCModel();
			(DataContext as NPCOptionControlViewModel).Refresh();
			RefreshUserControls();
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
				//DataContext = npcModel;
				(DataContext as NPCOptionControlViewModel).Refresh();

				// TODO:  Get the active tab the user is on
				// As the assumption here is the User is on the Base Stats tab while loading
				// a NPC File
				RefreshUserControls();
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
			NPCModel npcModel = new NPCModel();
			try 
			{
				moduleService.AddNPCToCategory(npcController.GetNPCModel(), (FGCategoryComboBox.SelectedItem as CategoryModel).Name);
				Refresh();
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
			if(FGCategoryComboBox.SelectedItem == null)
			{
				FGCategoryComboBox.ItemsSource = npcOptionControlViewModel.ModuleModel.Categories;
				FGCategoryComboBox.SelectedIndex = 0;
			} 
			else
				CategorySelectedNPCComboBox.ItemsSource = (FGCategoryComboBox.SelectedItem as CategoryModel).NPCModels;
			
			DataContext = npcOptionControlViewModel;
		}

        private void PrevNPCInCategory_Button_Click(object sender, RoutedEventArgs e)
        {
			CategoryModel categoryModel = FGCategoryComboBox.SelectedItem as CategoryModel;
			if (categoryModel != null)
			{
				if (categoryIndex == 0)
					categoryIndex = categoryModel.NPCModels.Count - 1;
				else
					categoryIndex--;

				// By updating the selected Item here, it will invoke CategorySelectedNPCComboBox_SelectionChanged event 
				// because the CategorySelectedNPCComboBox selected item has changed
				CategorySelectedNPCComboBox.SelectedItem = categoryModel.NPCModels[categoryIndex];
			}
			RefreshUserControls();

		}

        private void NextNPCInCategory_Button_Click(object sender, RoutedEventArgs e)
        {
			CategoryModel categoryModel = FGCategoryComboBox.SelectedItem as CategoryModel;
			if (categoryModel != null)
			{
				if (categoryIndex == categoryModel.NPCModels.Count - 1)
					categoryIndex = 0;
				else
					categoryIndex++;

				// By updating the selected Item here, it will invoke CategorySelectedNPCComboBox_SelectionChanged event 
				// because the CategorySelectedNPCComboBox selected item has changed
				CategorySelectedNPCComboBox.SelectedItem = categoryModel.NPCModels[categoryIndex];
			}
			RefreshUserControls();
		}

		//TODO:  As a gut feeling, I *think* we do not need to force a refresh of the relevant UserControls
		// I do not have any concrete way of addressing this (and may even be an anti-pattern for MVVM)
		private void RefreshUserControls()
        {
			npcOptionControlViewModel.Refresh();
			BaseStatsUserControl.Refresh();
			SkillsUserControl.Refresh();
			SpellcastingUserControl.Refresh();
			TraitsUserControl.Refresh();
			InnateCastingUserControl.Refresh();
			ResistanceUserControl.Refresh();
			ActionOverviewUserControl.Refresh();
			ImageUserControl.Refresh();
			DescriptionUserControl.Refresh();
		}

        private void CategorySelectedNPCComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			NPCModel selectedNPCModel = CategorySelectedNPCComboBox.SelectedItem as NPCModel;
			npcController.UpdateNPCModel(selectedNPCModel);
			RefreshUserControls();
		}

		private void FGCategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			categoryIndex = 0;
			Refresh();
		}
	}
}
