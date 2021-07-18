using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using UserControl = System.Windows.Controls.UserControl;

namespace FantasyModuleParser.Equipment.UserControls
{
    /// <summary>
    /// Interaction logic for ImageUC.xaml
    /// </summary>
    public partial class GenericImageUserControl : UserControl
    {

        public GenericImageUserControl()
        {
            InitializeComponent();
            ImageUCLayoutRoot.DataContext = this;
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
               // FIXME Expose generated filepath so parent can read property
            }
        }

        private void _displayBitmapImage(string @imageFilePath)
        {
            if (File.Exists(imageFilePath)) { 
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imageFilePath);
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

        public static readonly DependencyProperty ImageFilePathProperty =
            DependencyProperty.Register
            (
                "ImageFilePath",
                typeof(string),
                typeof(GenericDescriptionUC),
                new PropertyMetadata("")
            );

        public string ImageFilePath
        {
            get { return (string)GetValue(ImageFilePathProperty); }
            set { SetValue(ImageFilePathProperty, value); }
        }

        public static readonly DependencyProperty LabelNameTextProperty =
    DependencyProperty.Register
    (
        "LabelNameText",
        typeof(string),
        typeof(GenericDescriptionUC),
        new PropertyMetadata("")
    );

        public string LabelNameText
        {
            get { return (string)GetValue(LabelNameTextProperty); }
            set { SetValue(LabelNameTextProperty, value); }
        }
    }
}
