using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.NPC.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public static readonly DependencyProperty ProficiencyModelProperty =
            DependencyProperty.Register("ProficiencyModelValue", typeof(ProficiencyModel), typeof(ClassProficiencyUC));

        public ProficiencyModel ProficiencyModelValue
        {
            get { return (ProficiencyModel)GetValue(ProficiencyModelProperty); }
            set { SetValue(ProficiencyModelProperty, value); }
        }

        public static readonly DependencyProperty IsMultiProficiencyOptionProperty =
            DependencyProperty.Register("IsMultiProficiencyOptionValue", typeof(Visibility), typeof(ClassProficiencyUC), new PropertyMetadata(Visibility.Visible));

        public Visibility IsMultiProficiencyOptionValue
        {
            get { return (Visibility)GetValue(IsMultiProficiencyOptionProperty); }
            set { SetValue(IsMultiProficiencyOptionProperty, value); }
        }

        private void SavingThrow_Click(object sender, RoutedEventArgs e)
        {
            if(ProficiencyModelValue.SavingThrowAttributes == null)
            {
                ProficiencyModelValue.SavingThrowAttributes = new System.Collections.ObjectModel.ObservableCollection<Enums.SavingThrowAttributeEnum>();
            }
        }

        private ICommand _savingThrowSelectCommand;
        public ICommand SavingThrowSelectCommand
        {
            get
            {
                if (_savingThrowSelectCommand == null)
                {
                    _savingThrowSelectCommand = new ActionCommand(param => OnSavingThrowSelectAction(param),
                        param => IsSavingThrowCheckboxEnabled(param));
                }
                return _savingThrowSelectCommand;
            }
        }

        private bool IsSavingThrowCheckboxEnabled(object param)
        {
            if (param == null)
                return false;
            if (!(param is SavingThrowAttributeEnum))
                return false;

            SavingThrowAttributeEnum savingThrow = (SavingThrowAttributeEnum)param;
            if(ProficiencyModelValue?.SavingThrowAttributes?.Count >= 2)
            {
                return ProficiencyModelValue.SavingThrowAttributes.Contains(savingThrow);
            }

            return true;
        }

        protected virtual void OnSavingThrowSelectAction(object param)
        {
            SavingThrowAttributeEnum savingThrow = (SavingThrowAttributeEnum)param;

            if (ProficiencyModelValue.SavingThrowAttributes == null)
                ProficiencyModelValue.SavingThrowAttributes = new System.Collections.ObjectModel.ObservableCollection<SavingThrowAttributeEnum>();

            if (ProficiencyModelValue.SavingThrowAttributes.Contains(savingThrow))
                ProficiencyModelValue.SavingThrowAttributes.Remove(savingThrow);
            else
                ProficiencyModelValue.SavingThrowAttributes.Add(savingThrow);
        }
    }
}
