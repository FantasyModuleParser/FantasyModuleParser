using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.Equipment.UserControls
{
    /// <summary>
    /// Interaction logic for EquipmentHeaderUC.xaml
    /// </summary>
    public partial class EquipmentHeaderUC : UserControl
    {
        public static DependencyProperty NameTextProperty =
            DependencyProperty.Register
            (
                "NameText",
                typeof(string),
                typeof(EquipmentHeaderUC),
                new PropertyMetadata("")
            );

        public string NameText
        {
            get { return (string)GetValue(NameTextProperty); }
            set { SetValue(NameTextProperty, value); }
        }

        public static DependencyProperty NonIDTextProperty =
            DependencyProperty.Register
            (
                "NonIDText",
                typeof(string),
                typeof(EquipmentHeaderUC),
                new PropertyMetadata("")
            );

        public string NonIDText
        {
            get { return (string)GetValue(NonIDTextProperty); }
            set { SetValue(NonIDTextProperty, value); }
        }

        public static DependencyProperty NonIDDescriptionTextProperty =
            DependencyProperty.Register
            (
                "NonIDDescriptionText",
                typeof(string),
                typeof(EquipmentHeaderUC),
                new PropertyMetadata("")
            );

        public string NonIDDescriptionText
        {
            get { return (string)GetValue(NonIDDescriptionTextProperty); }
            set { SetValue(NonIDDescriptionTextProperty, value); }
        }

        public EquipmentHeaderUC()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }
    }
}
