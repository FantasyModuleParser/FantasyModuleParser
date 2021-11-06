using FantasyModuleParser.Classes.Model;
using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.Classes.UserControls
{
    /// <summary>
    /// Interaction logic for ClassSpellSlotUC.xaml
    /// </summary>
    public partial class ClassSpellSlotUC : UserControl
    {
        public ClassSpellSlotUC()
        {
            InitializeComponent();
            SpellSlotLayout.DataContext = this;
        }

        public static readonly DependencyProperty ClassModelProperty =
            DependencyProperty.Register("ClassModelValue", typeof(ClassModel), typeof(ClassSpellSlotUC));


        public ClassModel ClassModelValue
        {
            get { return (ClassModel)GetValue(ClassModelProperty); }
            set { SetValue(ClassModelProperty, value); }
        }
    }
}
