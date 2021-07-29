using FantasyModuleParser.Equipment.Models;
using FantasyModuleParser.Equipment.ViewModels;
using FantasyModuleParser.Main.Services;
using log4net;
using System;
using System.Windows.Controls;

namespace FantasyModuleParser.Equipment
{
    /// <summary>
    /// Interaction logic for EquipmentOptionControl.xaml
    /// </summary>
    public partial class EquipmentOptionControl : UserControl
    {
        private SettingsService settingsService;
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public EquipmentOptionControl()
        {
            InitializeComponent();
            settingsService = new SettingsService();
        }

        private void PrimaryEquipmentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void EquipmentFooterUC_SaveEquipmentAction(object sender, System.EventArgs e)
        {
            EquipmentOptionControlViewModel viewModel = DataContext as EquipmentOptionControlViewModel;
            if (viewModel != null)
            {
                viewModel.SaveEquipmentModel();
                log.Info("Equipment " + viewModel.Name + " has successfully been saved");
            }
        }

        private void EquipmentFooterUC_PrevEquipmentAction(object sender, System.EventArgs e)
        {
            WeaponUserControl.Refresh();
        }

        private void EquipmentFooterUC_NextEquipmentAction(object sender, System.EventArgs e)
        {
            WeaponUserControl.Refresh();
        }

        private void EquipmentFooterUC_LoadEquipmentAction(object sender, System.EventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                InitialDirectory = settingsService.Load().EquipmentFolderLocation,
                Filter = "Equipment files (*.json)|*.json|All files (*.*)|*.*"
            };

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDialog.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                EquipmentOptionControlViewModel viewModel = DataContext as EquipmentOptionControlViewModel;
                if (viewModel != null)
                {
                    viewModel.LoadEquipmentModel(openFileDialog.FileName);

                    log.Info("Equipment " + viewModel.Name + " has successfully been loaded");

                }
                else
                    log.Debug("DataContext is not what it is expected to be :: " + DataContext.GetType());

            }
        }

        private void EquipmentFooterUC_AddToProjectAction(object sender, EventArgs e)
        {
            EquipmentOptionControlViewModel viewModel = DataContext as EquipmentOptionControlViewModel;
            if (viewModel != null)
            {
                viewModel.AddEquipmentToCategory();
                log.Info("Equipment " + viewModel.Name + " has successfully been added to Category " + viewModel.SelectedCategoryModel.Name);
            }
        }

        private void EquipmentFooterUC_SelectedItemModelChangeAction(object sender, EventArgs e)
        {

        }

        private void EquipmentFooterUC_NewItemAction(object sender, EventArgs e)
        {
            EquipmentOptionControlViewModel viewModel = DataContext as EquipmentOptionControlViewModel;
            if (viewModel != null)
            {
                viewModel.NewEquipmentModel();
                log.Info("Equipment " + viewModel.Name + " has been reset (New Item Action Invoked)");
            }
        }

        private void ArmorListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ArmorDetailUserControl != null)
                ArmorDetailUserControl.Refresh();
        }
    }
}
