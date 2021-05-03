using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FantasyModuleParser.Tables.Views
{
    /// <summary>
    /// Interaction logic for ChangeColumnHeaderView.xaml
    /// </summary>
    public partial class ChangeColumnHeaderView : Window
    {
        public String ColumnHeaderText
        {
            get
            {
                if (ColumnHeaderTextBox == null) return String.Empty;
                return ColumnHeaderTextBox.Text;
            }
        }
        public ChangeColumnHeaderView(String currentColumnHeaderText)
        {
            InitializeComponent();

            ColumnHeaderTextBox.Text = currentColumnHeaderText;
        }
        private void OnSave(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            MoveBottomRightEdgeOfWindowToMousePosition();
        }

        private void MoveBottomRightEdgeOfWindowToMousePosition()
        {
            var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
            var mouse = transform.Transform(GetMousePosition());
            Left = mouse.X - ActualWidth;
            Top = mouse.Y - ActualHeight;
        }

        public System.Windows.Point GetMousePosition()
        {
            System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
            return new System.Windows.Point(point.X, point.Y);
        }
    }
}
