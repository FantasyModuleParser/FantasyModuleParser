using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.NPC.Commands;
using System;
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
            if(ClassModelValue.SavingThrowAttributes?.Count >= 2)
            {
                return ClassModelValue.SavingThrowAttributes.Contains(savingThrow);
            }

            return true;
        }

        protected virtual void OnSavingThrowSelectAction(object param)
        {
            SavingThrowAttributeEnum savingThrow = (SavingThrowAttributeEnum)param;

            if (ClassModelValue.SavingThrowAttributes == null)
                ClassModelValue.SavingThrowAttributes = new System.Collections.ObjectModel.ObservableCollection<SavingThrowAttributeEnum>();

            if (ClassModelValue.SavingThrowAttributes.Contains(savingThrow))
                ClassModelValue.SavingThrowAttributes.Remove(savingThrow);
            else
                ClassModelValue.SavingThrowAttributes.Add(savingThrow);
        }
    }
}
