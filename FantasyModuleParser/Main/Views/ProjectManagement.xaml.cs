using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.ViewModels;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace FantasyModuleParser.Main
{
    /// <summary>
    /// Interaction logic for ProjectManagement.xaml
    /// </summary>
    public partial class ProjectManagement : Window
    {
        private SettingsService settingsService;
        public ProjectManagement()
        {
            InitializeComponent();

            // Enable it so the popup window can close on the Escape key
            PreviewKeyDown += (sender, eventArgs) => { if (eventArgs.Key == Key.Escape) Close(); };

            settingsService = new SettingsService();
        }
        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OpenThumbnailFilePath(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.InitialDirectory = (DataContext as ProjectManagementViewModel).SettingsModel.MainFolderLocation;
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                string sSelectedPath = openFileDialog.FileName;
                ProjectManagementViewModel viewModel = DataContext as ProjectManagementViewModel;
                viewModel.ModuleModel.ThumbnailPath = sSelectedPath;
                ModuleThumbnameFilename.Text = sSelectedPath;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ProjectManagementViewModel viewModel = DataContext as ProjectManagementViewModel;
            OnCloseWindowEvent(EventArgs.Empty);
            viewModel.UpdateModule();
        }

        private void SaveModule_Click(object sender, RoutedEventArgs e)
        {
            ProjectManagementViewModel viewModel = DataContext as ProjectManagementViewModel;       
            //if (viewModel.ModuleModel.SaveFilePath == null || viewModel.ModuleModel.SaveFilePath.Length <= 0)
            //    SaveToModule_Click(sender, e);
            //else
            viewModel.SaveModule(settingsService.Load().ProjectFolderLocation, viewModel.ModuleModel);

            viewModel.SettingsModel.LastProject = viewModel.ModuleModel.ModFilename;
            settingsService.Save(viewModel.SettingsModel);

        }
        private void SaveModuleAndClose_Click(object sender, RoutedEventArgs e)
        {
            if (validateRequiredFields()) { 
                SaveModule_Click(sender, e);
                OnSaveAndCloseAction(EventArgs.Empty);
                ESExit_Click(sender, e);
            } else
            {
                RequiredTextBox_TextChanged(sender, null);
            }
        }

        private void SaveToModule_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select which folder to save the Project Module data to";

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string sSelectedPath = folderBrowserDialog.SelectedPath;
                ProjectManagementViewModel viewModel = DataContext as ProjectManagementViewModel;
                viewModel.SaveModule(sSelectedPath, viewModel.ModuleModel);
            }
        }

        private void LoadModule_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.InitialDirectory = (DataContext as ProjectManagementViewModel).SettingsModel.ProjectFolderLocation;
            openFileDlg.Filter = "Project files (*.fmp)|*.fmp|All files (*.*)|*.*";
            openFileDlg.RestoreDirectory = true;

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                ProjectManagementViewModel viewModel = DataContext as ProjectManagementViewModel;
                viewModel.LoadModule(openFileDlg.FileName);
                DataContext = viewModel;
            }
        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ProjectManagementViewModel).NewModuleSetup();

            // For some reason, the fields are not updated automatically.  
            // TODO:  This is the workaround for now.
            ModuleName.Text = "";
            ModuleCategory.Text = "";
            ModuleAuthor.Text = "";
            ModuleModFilename.Text = "";
            ModuleThumbnameFilename.Text = "";
            ModulePathTB.Text = "";
        }

        public event EventHandler OnCloseWindowAction;
        protected virtual void OnCloseWindowEvent(EventArgs e)
        {
            EventHandler handler = OnCloseWindowAction;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler SaveAndCloseAction;
        protected virtual void OnSaveAndCloseAction(EventArgs e)
        {
            EventHandler handler = SaveAndCloseAction;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void ModuleModFilename_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            (DataContext as ProjectManagementViewModel).UpdateFullModulePath();
            RequiredTextBox_TextChanged(sender, e);
        }

        private void RequiredTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (validateRequiredFields())
            {
                RequiredLabel.Visibility = Visibility.Hidden;
            }
            else
            {
                RequiredLabel.Visibility = Visibility.Visible;
            }
        }

        private bool validateRequiredFields()
        {
            if (ModuleName == null || ModuleModFilename == null)
                return false;
            return !String.IsNullOrEmpty(ModuleName.Text) && !String.IsNullOrEmpty(ModuleModFilename.Text);
        }
    }
}
