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

		#region Variables
		string installPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		string installFolder = "FMP/NPC";
		#endregion
		public NPCOptionControl()
		{
			InitializeComponent();
			npcController = new NPCController();
			//var npcModel = ((App)Application.Current).NpcModelObject;
			DataContext = npcController.GetNPCModel();
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
					new UserCreationManagement().Show();
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
	}
}
