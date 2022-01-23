using System;
using FantasyModuleParser.Classes.ViewModels;
using System.Windows.Controls;
using FantasyModuleParser.Equipment.ViewModels;
using FantasyModuleParser.Main.Services;
using log4net;
using System.Windows;
using FantasyModuleParser.Classes.Model;

namespace FantasyModuleParser.Classes
{
	/// <summary>
	/// Interaction logic for ClassOptionControl.xaml
	/// </summary>
	public partial class ClassOptionControl : UserControl
    {
        private SettingsService settingsService;

        private static readonly ILog log = LogManager.GetLogger(typeof(ClassOptionControl));
        public ClassOptionControl()
		{
			InitializeComponent();
            settingsService = new SettingsService();
        }

        private void EquipmentFooterUC_SaveEquipmentAction(object sender, System.EventArgs e)
        {
			if(DataContext is ClassOptionControllViewModel)
            {
				((ClassOptionControllViewModel)DataContext).Save();
            }
        }

        private void EquipmentFooterUC_LoadEquipmentAction(object sender, System.EventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                InitialDirectory = settingsService.Load().ClassFolderLocation,
                Filter = "Class files (*.json)|*.json|All files (*.*)|*.*"
            };

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDialog.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                ClassOptionControllViewModel viewModel = DataContext as ClassOptionControllViewModel;
                if (viewModel != null)
                {
                    viewModel.LoadClassModel(openFileDialog.FileName);

                    log.Info("Class " + viewModel.ParentClassModel.Name + " has successfully been loaded");

                }
                else
                    log.Debug("DataContext is not what it is expected to be :: " + DataContext.GetType());

            }
        }

        private void AddToProjectAction(object sender, EventArgs e)
        {
            ClassOptionControllViewModel viewModel = DataContext as ClassOptionControllViewModel;
            if (viewModel != null)
            {
                viewModel.AddClassToCategory();
                log.Info("Class " + viewModel.ParentClassModel.Name + " has successfully been added to Category " + viewModel.SelectedCategoryModel.Name);
            }
        }

        private void SelectedItemModelChangeAction(object sender, EventArgs e)
        {
            //ClassFeatureUserControl.ClassFeatureListBox.Items.Refresh();
        }

        private void NewItemAction(object sender, EventArgs e)
        {
            ClassOptionControllViewModel viewModel = DataContext as ClassOptionControllViewModel;
            if (viewModel != null)
            {
                viewModel.NewClassModel();
                log.Info("Class " + viewModel.ParentClassModel.Name + " has been reset (New Item Action Invoked)");
            }
        }
    }
}
