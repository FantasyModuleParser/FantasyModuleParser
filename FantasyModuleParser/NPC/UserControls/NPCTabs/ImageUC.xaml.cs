using FantasyModuleParser.NPC.Controllers;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using UserControl = System.Windows.Controls.UserControl;

namespace FantasyModuleParser.NPC.UserControls.NPCTabs
{
    /// <summary>
    /// Interaction logic for ImageUC.xaml
    /// </summary>
    public partial class ImageUC : UserControl
    {
        #region Controllers
        public NPCController npcController { get; set; }
        #endregion
        public ImageUC()
        {
            InitializeComponent();
            npcController = new NPCController();
            DataContext = npcController.GetNPCModel();
        }
        public void Refresh()
        {
            DataContext = npcController.GetNPCModel();

            NPCModel npcModel = (DataContext as NPCModel);
            if(!string.IsNullOrEmpty(npcModel.NPCImage))
            {
                _displayBitmapImage(npcModel);
            }
        }
        private void strNPCImage_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Image files (*.jpg)|*.jpg|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog.FileName;
                (DataContext as NPCModel).NPCImage = selectedFileName;
                _displayBitmapImage(DataContext as NPCModel);
                strNPCImage.Text = selectedFileName;
                if (strNPCImage.Text.StartsWith("file:///"))
                {
                    strNPCImage.Text = selectedFileName.Remove(0, 8);
                    (DataContext as NPCModel).NPCImage = selectedFileName;
                }
            }
        }

        private void _displayBitmapImage(NPCModel npcModel)
        {
            if (File.Exists(npcModel.NPCImage)) { 
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(npcModel.NPCImage);
                bitmap.EndInit();
                ImageBox.Source = bitmap;
                strNPCImage.ClearValue(TextBox.BorderBrushProperty);
                strNPCImage.ClearValue(TextBox.BorderThicknessProperty);
            } 
            else
            {
                // This likely breaks MVVM, as I think the ViewModel should not be aware of any specific UI components in the View
                // and that the View UI compoenents should be interacted through Bindings... but what do I know!
                strNPCImage.BorderBrush = System.Windows.Media.Brushes.Red;
                strNPCImage.BorderThickness = new Thickness(2);
                Keyboard.ClearFocus();
            }
        }

        private void clearImage_Click(object sender, RoutedEventArgs e)
        {
            strNPCImage.Text = "";
        }
    }
}
