using FantasyModuleParser.Classes.Model;
using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.Classes.UserControls
{
    /// <summary>
    /// Interaction logic for ClassProficiencyUC.xaml
    /// </summary>
    public partial class ClassProficiencyBonusUC : UserControl
    {
        public ClassProficiencyBonusUC()
        {
            InitializeComponent();
            ProfBonusLayout.DataContext = this;
        }

        public static readonly DependencyProperty ClassModelProperty =
            DependencyProperty.Register("ClassModelValue", typeof(ClassModel), typeof(ClassProficiencyBonusUC));


        public ClassModel ClassModelValue
        {
            get { return (ClassModel)GetValue(ClassModelProperty); }
            set { SetValue(ClassModelProperty, value); }
        }
    }
}
