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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FantasyModuleParser.Equipment.UserControls
{
    /// <summary>
    /// Interaction logic for EquipmentFooterUC.xaml
    /// </summary>
    public partial class EquipmentFooterUC : UserControl
    {
        public EquipmentFooterUC()
        {
            InitializeComponent();
        }

        private void NewEquipment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadEquipment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveEquipment_Click(object sender, RoutedEventArgs e)
        {
            OnSaveEquipmentAction();
        }

        public event EventHandler SaveEquipmentAction;
        protected virtual void OnSaveEquipmentAction()
        {
            if (SaveEquipmentAction != null)
                SaveEquipmentAction(this, EventArgs.Empty);
        }

        private void AddToProject_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
