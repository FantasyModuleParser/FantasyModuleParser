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
		public NPCController npcController { get; set; }
		#endregion
		#region ViewModel
		private NPCOptionControlViewModel npcOptionControlViewModel;
        #endregion
        #region Variables
		private bool _isViewStatblockWindowOpen = false;
		private int categoryIndex = 0;
		private SettingsService settingsService;
		private SettingsModel settingsModel;
		#endregion

		public event EventHandler OnViewStatBlock;
		private static readonly ILog log = LogManager.GetLogger(typeof(NPCOptionControl));
		public NPCOptionControl()
		{
			InitializeComponent();
			npcController = new NPCController();
			npcOptionControlViewModel = new NPCOptionControlViewModel();
			settingsService = new SettingsService();
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
			string saveDirectory = settingsService.Load().NPCFolderLocation;
			string savePath = Path.Combine(saveDirectory, npcModel.NPCName + ".json");

			if (string.IsNullOrEmpty(npcModel.NPCType))
            {
				log.Warn("NPC Type is missing from " + npcModel.NPCName);
				throw new InvalidDataException("NPC Type is missing from " + npcModel.NPCName);
			}		
			if (string.IsNullOrEmpty(npcModel.Size))
            {
				log.Warn("Size is missing from " + npcModel.NPCName);
				throw new InvalidDataException("Size is missing from " + npcModel.NPCName);
			}				
			if (string.IsNullOrEmpty(npcModel.AC))
            {
				log.Warn("AC is missing from " + npcModel.NPCName);
				throw new InvalidDataException("AC is missing from " + npcModel.NPCName);
			}
			if (string.IsNullOrEmpty(npcModel.Alignment))
            {
				log.Warn("Alignment is missing from " + npcModel.NPCName);
				throw new InvalidDataException("Alignment is missing from " + npcModel.NPCName);
			}
			if (string.IsNullOrEmpty(npcModel.ChallengeRating))
            {
				log.Warn("Challenge Rating is missing from " + npcModel.NPCName);
				throw new InvalidDataException("Challenge Rating is missing from " + npcModel.NPCName);
			}				
			if (!string.IsNullOrEmpty(npcModel.HP))
            {
				log.Warn("Hit Points are missing from " + npcModel.NPCName);
				throw new InvalidDataException("Hit Points are missing from " + npcModel.NPCName);
			}			
			if (!string.IsNullOrEmpty(npcModel.LanguageOptions))
            {
				log.Warn("Language Option (usually No special conditions) is missing from " + npcModel.NPCName);
				throw new InvalidDataException("Language Option (usually No special conditions) is missing from " + npcModel.NPCName);
			}				
			if (npcModel.Telepathy == true && string.IsNullOrEmpty(npcModel.TelepathyRange))
            {
				log.Warn("Telepathy Range is missing from " + npcModel.NPCName);
				throw new InvalidDataException("Telepathy Range is missing from " + npcModel.NPCName);
			}
			if (npcModel.InnateSpellcastingSection == true && !string.IsNullOrEmpty(npcModel.InnateSpellcastingAbility))
            {
				log.Warn("Innate Spellcasting Ability is missing from " + npcModel.NPCName);
				throw new InvalidDataException("Innate Spellcasting Ability is missing from " + npcModel.NPCName);
			}				
			if (npcModel.SpellcastingSection == true)
            {
				if (string.IsNullOrEmpty(npcModel.SCSpellcastingAbility))
                {
					log.Warn("Spellcasting Ability is missing from " + npcModel.NPCName);
					throw new InvalidDataException("Spellcasting Ability is missing from " + npcModel.NPCName);
				}
				if (string.IsNullOrEmpty(npcModel.CantripSpellList) && string.IsNullOrEmpty(npcModel.CantripSpells))
                {
					log.Warn("Number of Cantrip slots is missing from " + npcModel.NPCName);
					throw new InvalidDataException("Number of Cantrip slots is missing from " + npcModel.NPCName);
				}
				if (string.IsNullOrEmpty(npcModel.FirstLevelSpellList) && string.IsNullOrEmpty(npcModel.FirstLevelSpells))
				{
					log.Warn("Number of First Level Spell slots is missing from " + npcModel.NPCName);
					throw new InvalidDataException("Number of First Level Spell slots is missing from " + npcModel.NPCName);
				}				
				if (string.IsNullOrEmpty(npcModel.SecondLevelSpellList) && string.IsNullOrEmpty(npcModel.SecondLevelSpells))
                {
					log.Warn("Number of Second Level Spell slots is missing from " + npcModel.NPCName);
					throw new InvalidDataException("Number of Second Level Spell slots is missing from " + npcModel.NPCName);
				}					
				if (string.IsNullOrEmpty(npcModel.ThirdLevelSpellList) && string.IsNullOrEmpty(npcModel.ThirdLevelSpells))
                {
					log.Warn("Number of Third Level Spell slots is missing from " + npcModel.NPCName);
					throw new InvalidDataException("Number of Third Level Spell slots is missing from " + npcModel.NPCName);
				}					
				if (string.IsNullOrEmpty(npcModel.FourthLevelSpellList) && string.IsNullOrEmpty(npcModel.FourthLevelSpells))
                {
					log.Warn("Number of Fourth Level Spell slots is missing from " + npcModel.NPCName);
					throw new InvalidDataException("Number of Fourth Level Spell slots is missing from " + npcModel.NPCName);
				}					
				if (string.IsNullOrEmpty(npcModel.FifthLevelSpellList) && string.IsNullOrEmpty(npcModel.FifthLevelSpells))
                {
					log.Warn("Number of Fifth Level Spell slots is missing from " + npcModel.NPCName);
					throw new InvalidDataException("Number of Fifth Level Spell slots is missing from " + npcModel.NPCName);
				}					
				if (string.IsNullOrEmpty(npcModel.SixthLevelSpellList) && string.IsNullOrEmpty(npcModel.SixthLevelSpells))
                {
					log.Warn("Number of Sixth Level Spell slots is missing from " + npcModel.NPCName);
					throw new InvalidDataException("Number of Sixth Level Spell slots is missing from " + npcModel.NPCName);
				}					
				if (string.IsNullOrEmpty(npcModel.SeventhLevelSpellList) && string.IsNullOrEmpty(npcModel.SeventhLevelSpells))
                {
					log.Warn("Number of Seventh Level Spell slots is missing from " + npcModel.NPCName);
					throw new InvalidDataException("Number of Seventh Level Spell slots is missing from " + npcModel.NPCName);
				}					
				if (string.IsNullOrEmpty(npcModel.EighthLevelSpellList) && string.IsNullOrEmpty(npcModel.EighthLevelSpells))
                {
					log.Warn("Number of Eighth Level Spell slots is missing from " + npcModel.NPCName);
					throw new InvalidDataException("Number of Eighth Level Spell slots is missing from " + npcModel.NPCName);
				}					
				if (string.IsNullOrEmpty(npcModel.NinthLevelSpellList) && string.IsNullOrEmpty(npcModel.NinthLevelSpells))
                {
					log.Warn("Number of Ninth Level Spell slots is missing from " + npcModel.NPCName);
					throw new InvalidDataException("Number of Ninth Level Spell slots is missing from " + npcModel.NPCName);
				}					
			}
			
			((App)Application.Current).NpcModel = npcModel;
			if (Directory.Exists(saveDirectory))
            {
				npcController.Save(savePath, npcModel);
				log.Info("NPC " + npcModel.NPCName + " has successfully been saved to " + savePath);
				MessageBox.Show("NPC " + npcModel.NPCName + " Saved Successfully");
			}
			else
            {
				Directory.CreateDirectory(saveDirectory);
				npcController.Save(savePath, npcModel);
				log.Info("NPC " + npcModel.NPCName + " has successfully been saved to " + savePath);
				MessageBox.Show("NPC " + npcModel.NPCName + " Saved Successfully");
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
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

			openFileDialog.InitialDirectory = settingsService.Load().NPCFolderLocation;
			openFileDialog.Filter = "Image files (*.json;*.npc)|*.json;*.npc|All files (*.*)|*.*";

			// Launch OpenFileDialog by calling ShowDialog method
			Nullable<bool> result = openFileDialog.ShowDialog();
			// Get the selected file name and display in a TextBox.
			// Load content of file in a TextBlock
			if (result == true)
			{
				npcController.Load(openFileDialog.FileName);
				NPCModel npcModel = npcController.GetNPCModel();

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
				moduleService.AddNPCToCategory(npcController.GetNPCModel(), (FGCategoryComboBox.SelectedItem as CategoryModel).Name);
				Refresh();
				log.Info("NPC " + npcModel.NPCName + " has successfully been added to project");
				MessageBox.Show("NPC " + npcModel.NPCName + " has been added to the project");
			}
			catch (Exception exception)
			{
				MessageBox.Show("Error detected while adding NPC to Project :: " + exception.Message);
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
