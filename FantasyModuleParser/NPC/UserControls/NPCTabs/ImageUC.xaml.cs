using FantasyModuleParser.NPC.Controllers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            if(npcModel.NPCImage != null && npcModel.NPCImage.Trim().Length != 0)
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
                
            }
        }

        private void _displayBitmapImage(NPCModel npcModel)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(npcModel.NPCImage);
            bitmap.EndInit();
            ImageBox.Source = bitmap;
        }

        private void clearImage_Click(object sender, RoutedEventArgs e)
        {
            strNPCImage.Text = "";
        }
    }
}
