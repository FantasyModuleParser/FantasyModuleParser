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
        private ProjectManagementViewModel projectManagementViewModel;
        public ProjectManagement()
        {
            InitializeComponent();

            // Enable it so the popup window can close on the Escape key
            this.PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };

            projectManagementViewModel = new ProjectManagementViewModel();
            DataContext = projectManagementViewModel;
        }
        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OpenModuleFilePath(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Custom Description";

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string sSelectedPath = folderBrowserDialog.SelectedPath;
                ProjectManagementViewModel viewModel = DataContext as ProjectManagementViewModel;
                viewModel.ModuleModel.ModulePath = sSelectedPath;
                ModulePathTB.Text = sSelectedPath;
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
            if (viewModel.ModuleModel.SaveFilePath == null || viewModel.ModuleModel.SaveFilePath.Length <= 0)
                SaveToModule_Click(sender, e);
            else
                viewModel.SaveModule(viewModel.ModuleModel.SaveFilePath, viewModel.ModuleModel);
        }
        private void SaveModuleAndClose_Click(object sender, RoutedEventArgs e)
        {
            SaveModule_Click(sender, e);
            ESExit_Click(sender, e);
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

        public event EventHandler OnCloseWindowAction;
        protected virtual void OnCloseWindowEvent(EventArgs e)
        {
            EventHandler handler = OnCloseWindowAction;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
