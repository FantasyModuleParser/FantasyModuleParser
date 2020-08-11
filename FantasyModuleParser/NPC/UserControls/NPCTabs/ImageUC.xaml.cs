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
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                ImageBox.Source = bitmap;
                strNPCImage.Text = selectedFileName;
            }
        }

        private void clearImage_Click(object sender, RoutedEventArgs e)
        {
            strNPCImage.Text = "";
        }
    }
}
