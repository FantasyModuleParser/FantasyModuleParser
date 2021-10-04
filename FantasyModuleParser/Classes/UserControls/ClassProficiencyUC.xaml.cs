using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.NPC.Commands;
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

namespace FantasyModuleParser.Classes.UserControls
{
    /// <summary>
    /// Interaction logic for ClassProficiencyUC.xaml
    /// </summary>
    public partial class ClassProficiencyUC : UserControl
    {
        public ClassProficiencyUC()
        {
            InitializeComponent();
            ProficencyLayout.DataContext = this;
        }

        public static readonly DependencyProperty ClassModelProperty =
            DependencyProperty.Register("ClassModelValue", typeof(ClassModel), typeof(ClassProficiencyUC));

        public ClassModel ClassModelValue
        {
            get { return (ClassModel)GetValue(ClassModelProperty); }
            set { SetValue(ClassModelProperty, value); }
        }

        private void SavingThrow_Click(object sender, RoutedEventArgs e)
        {
            if(ClassModelValue.SavingThrowAttributes == null)
            {
                ClassModelValue.SavingThrowAttributes = new System.Collections.ObjectModel.ObservableCollection<Enums.SavingThrowAttributeEnum>();
            }


        }

        private ICommand _savingThrowSelectCommand;
        public ICommand SavingThrowSelectCommand
        {
            get
            {
                if (_savingThrowSelectCommand == null)
                {
                    _savingThrowSelectCommand = new ActionCommand(param => OnSavingThrowSelectAction(param));
                }
                return _savingThrowSelectCommand;
            }
        }

        public event EventHandler SavingThrowSelectAction;
        protected virtual void OnSavingThrowSelectAction(object param)
        {
            if (SavingThrowSelectAction != null)
                SavingThrowSelectAction(this, EventArgs.Empty);
        }
    }
}
