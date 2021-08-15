using FantasyModuleParser.Main;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.ViewModels;
using FantasyModuleParser.NPC.Views;
using log4net;
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
		public NPCController NpcController { get; set; }
		#endregion
		#region ViewModel
		private readonly NPCOptionControlViewModel npcOptionControlViewModel;
        #endregion
        #region Variables
		private bool _isViewStatblockWindowOpen = false;
		private int categoryIndex = 0;
		private readonly SettingsService settingsService;
		private SettingsModel settingsModel;
		#endregion

		public event EventHandler OnViewStatBlock;
		private static readonly ILog log = LogManager.GetLogger(typeof(NPCOptionControl));
		public NPCOptionControl()
		{
			InitializeComponent();
			NpcController = new NPCController();
			npcOptionControlViewModel = new NPCOptionControlViewModel();
			settingsService = new SettingsService();
			DataContext = npcOptionControlViewModel;
		}

		// private void openfolder(string strPath, string strFolder)
		// {
		// 	var fPath = Path.Combine(strPath, strFolder);
		// 	Directory.CreateDirectory(fPath);
		// 	System.Diagnostics.Process.Start(fPath);
		// }

		private void SaveNPCToFile(object sender, RoutedEventArgs e)
		{
			NPCModel npcModel = NpcController.GetNPCModel();
			string saveDirectory = settingsService.Load().NPCFolderLocation;
			string savePath = Path.Combine(saveDirectory, npcModel.NPCName + ".json");

			string warningMessageDoNotSave = npcModel.OkToSaveToFile(sender, e);

			if (!string.IsNullOrEmpty(warningMessageDoNotSave))
			{
				MessageBox.Show(warningMessageDoNotSave);
				return;
			}
			((App)Application.Current).NpcModel = npcModel;
			if (Directory.Exists(saveDirectory))
			{
				NpcController.Save(savePath, npcModel);
				log.Info("NPC " + npcModel.NPCName + " has successfully been saved to " + savePath);
				MessageBox.Show("NPC " + npcModel.NPCName + " Saved Successfully");
			}
			else
			{
				Directory.CreateDirectory(saveDirectory);
				NpcController.Save(savePath, npcModel);
				log.Info("NPC " + npcModel.NPCName + " has successfully been saved to " + savePath);
				MessageBox.Show("NPC " + npcModel.NPCName + " Saved Successfully");
			}
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

		private void NewNPC_Click(object sender, RoutedEventArgs e)
		{
			NpcController.ClearNPCModel();
			//DataContext = npcController.GetNPCModel();
			(DataContext as NPCOptionControlViewModel).Refresh();
			RefreshUserControls();
		}
		private void LoadNPCOption_Click(object sender, RoutedEventArgs e)
		{
			// Create OpenFileDialog
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
			{
				InitialDirectory = settingsService.Load().NPCFolderLocation,
				Filter = "NPC files (*.json;*.npc)|*.json;*.npc|All files (*.*)|*.*"
			};

			// Launch OpenFileDialog by calling ShowDialog method
			Nullable<bool> result = openFileDialog.ShowDialog();
			// Get the selected file name and display in a TextBox.
			// Load content of file in a TextBlock
			if (result == true)
			{
				NpcController.Load(openFileDialog.FileName);
				NPCModel npcModel = NpcController.GetNPCModel();

				//Refresh all the data on the UI
				//DataContext = npcModel;
				(DataContext as NPCOptionControlViewModel).Refresh();

				// TODO:  Get the active tab the user is on
				// As the assumption here is the User is on the Base Stats tab while loading
				// a NPC File
				RefreshUserControls();
				log.Info("NPC " + npcModel.NPCName + " has successfully been loaded");
			}
		}

		private void PreviewNPC_Click(object sender, RoutedEventArgs e)
		{ 
			settingsModel = settingsService.Load();

			if (!settingsModel.PersistentWindow)
            {
				if (!_isViewStatblockWindowOpen)
				{
					_isViewStatblockWindowOpen = true;
					PreviewNPC previewNPC = new PreviewNPC();
					previewNPC.Closing += PreviewNPC_Closing;
					previewNPC.Show();
				}
			}
			else
            {
				OnViewStatBlock.Invoke(this, EventArgs.Empty);
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
				log.Warn("No Module Project loaded!\nPlease create / load a Module through Options -> Manage Project");
				MessageBox.Show("No Module Project loaded!\nPlease create / load a Module through Options -> Manage Project");
				return;
			}

			ModuleService moduleService = new ModuleService();
			NPCModel npcModel = new NPCModel();
			try 
			{
				moduleService.AddNPCToCategory(NpcController.GetNPCModel(), (FGCategoryComboBox.SelectedItem as CategoryModel).Name);
				Refresh();
			}
			catch (Exception exception)
			{
				MessageBox.Show("Error detected while adding NPC to Project :: " + exception.Message);
			}
		}

		public void Refresh()
		{
			Refresh(false);
		}

		public void Refresh(bool flush)
        {
			npcOptionControlViewModel.Refresh();
			if (FGCategoryComboBox.SelectedItem == null || flush)
			{
				FGCategoryComboBox.ItemsSource = npcOptionControlViewModel.ModuleModel.Categories;
				FGCategoryComboBox.SelectedIndex = 0;

				// Clear out CategorySelectedNPCComboBox

				CategorySelectedNPCComboBox.SelectedIndex = 0;

				// Darkpool 07-12-2021:  Bugfix -- When loading a project file, the behavior is to
				// default to the first selected NPC in the first category.
				//
				// Before, the first NPC was populated
				// in the combobox CategorySelectedNPCComboBox, but the backing NPCModel in NPCController was 
				// overridden with a new instance of NPCModel.
				var selectedNPCValue = CategorySelectedNPCComboBox.SelectedValue;
				if(selectedNPCValue is NPCModel)
                {
					NpcController.UpdateNPCModel(selectedNPCValue as NPCModel);
				}
				else
                {
					NpcController.UpdateNPCModel(new NPCModel());
				}
				RefreshUserControls();
			}
			else
			{
				CategorySelectedNPCComboBox.ItemsSource = (FGCategoryComboBox.SelectedItem as CategoryModel).NPCModels;
			}				
			
			DataContext = npcOptionControlViewModel;
		}

        private void PrevNPCInCategory_Button_Click(object sender, RoutedEventArgs e)
        {
			CategoryModel categoryModel = FGCategoryComboBox.SelectedItem as CategoryModel;
			if (categoryModel != null && categoryModel.NPCModels.Count > 0)
			{
				if (categoryIndex == 0) { categoryIndex = categoryModel.NPCModels.Count - 1; }
				else { categoryIndex--; }
					
				// By updating the selected Item here, it will invoke CategorySelectedNPCComboBox_SelectionChanged event 
				// because the CategorySelectedNPCComboBox selected item has changed
				CategorySelectedNPCComboBox.SelectedItem = categoryModel.NPCModels[categoryIndex];
			}
			RefreshUserControls();
		}

        private void NextNPCInCategory_Button_Click(object sender, RoutedEventArgs e)
        {
			CategoryModel categoryModel = FGCategoryComboBox.SelectedItem as CategoryModel;
			if (categoryModel != null && categoryModel.NPCModels.Count > 0)
			{
				if (categoryIndex == categoryModel.NPCModels.Count - 1) { categoryIndex = 0; }
				else { categoryIndex++; }

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
			NpcController.UpdateNPCModel(selectedNPCModel);
			RefreshUserControls();
		}

		private void FGCategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			categoryIndex = 0;
			Refresh();
		}
	}
}
