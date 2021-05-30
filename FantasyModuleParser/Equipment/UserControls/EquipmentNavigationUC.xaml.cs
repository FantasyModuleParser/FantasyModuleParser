using FantasyModuleParser.Main.Models;
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
    /// Interaction logic for EquipmentNavigationUC.xaml
    /// </summary>
    public partial class EquipmentNavigationUC : UserControl
    {
        private ModuleModel _moduleModel;
        public ModuleModel SelectedModuleModel
        {
            get => this._moduleModel;
            set => this._moduleModel = value;
        }

        public string PreviousItemLabel { get; set; }
        public string NextItemLabel { get; set; }
        public EquipmentNavigationUC()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
