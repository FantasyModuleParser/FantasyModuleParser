using System.ComponentModel;
using System.Text.RegularExpressions;
using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FantasyModuleParser.Classes.UserControls.ClassProficiency
{
    /// <summary>
    /// Interaction logic for StartingToolsUC.xaml
    /// </summary>
    public partial class StartingToolsUC : UserControl, INotifyPropertyChanged
    {
        public StartingToolsUC()
        {
            InitializeComponent();
            StartingToolLayout.DataContext = this;
        }

        public static readonly DependencyProperty ProficiencyModelProperty =
            DependencyProperty.Register("ProficiencyModelValue", typeof(ProficiencyModel), typeof(StartingToolsUC));

        public ProficiencyModel ProficiencyModelValue
        {
            get { return (ProficiencyModel)GetValue(ProficiencyModelProperty); }
            set { SetValue(ProficiencyModelProperty, value); }
        }



        private void StartingToolsListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                foreach (EnumerationExtension.EnumerationMember enumerationMember in e.AddedItems)
                {
                    ClassStartingToolEnum choice;
                    if (ClassStartingToolEnum.TryParse(enumerationMember.Value.ToString(), out choice))
                    {
                        ProficiencyModelValue.ClassStartingToolOptions.Add(choice);
                    }
                }
            }

            if (e.RemovedItems.Count > 0)
            {
                foreach (EnumerationExtension.EnumerationMember enumerationMember in e.RemovedItems)
                {
                    ClassStartingToolEnum choice;
                    if (ClassStartingToolEnum.TryParse(enumerationMember.Value.ToString(), out choice))
                    {
                        ProficiencyModelValue.ClassStartingToolOptions.Remove(choice);
                    }
                }
            }
        }
        public int NumberOfGamingSets
        {
            get => ProficiencyModelValue != null ? ProficiencyModelValue.NumberOfGamingSets : 0; 
            set
            {
                ProficiencyModelValue.NumberOfGamingSets = value;
                RaisePropertyChanged(nameof(NumberOfGamingSets));
            }
        }
        public int NumberOfMusicalInstruments
        {
            get => ProficiencyModelValue != null ? ProficiencyModelValue.NumberOfMusicalInstruments : 0;
            set
            {
                ProficiencyModelValue.NumberOfMusicalInstruments = value;
                RaisePropertyChanged(nameof(NumberOfMusicalInstruments));
            }
        }
        private void PositiveNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text) || e.Text.Contains("-");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
