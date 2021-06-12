using FantasyModuleParser.Equipment.ViewModels;
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
    /// Interaction logic for WeaponUC.xaml
    /// </summary>
    public partial class WeaponUC : UserControl
    {
        public WeaponUC()
        {
            InitializeComponent();
        }

        public void Refresh()
        {
            // Check local DataContext that it's an instance of EquipmentOptionControlViewModel
            // and refresh the two list boxes accordingly
            if(DataContext is EquipmentOptionControlViewModel)
            {
                EquipmentOptionControlViewModel viewModel = DataContext as EquipmentOptionControlViewModel;
            }
        }
    }
}
