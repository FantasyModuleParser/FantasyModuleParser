using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.Extensions;
using Google.Protobuf.WellKnownTypes;

namespace FantasyModuleParser.Classes.UserControls.ClassProficiency
{
    /// <summary>
    /// Interaction logic for StartingSkillsUC.xaml
    /// </summary>
    public partial class StartingSkillsUC : UserControl, INotifyPropertyChanged
    {
        public StartingSkillsUC()
        {
            InitializeComponent();
            StartingSkillsUCLayout.DataContext = this;
        }

        public static readonly DependencyProperty ProficiencyModelProperty =
            DependencyProperty.Register("ProficiencyModelValue", typeof(ProficiencyModel), typeof(StartingSkillsUC));

        public ProficiencyModel ProficiencyModelValue
        {
            get { return (ProficiencyModel)GetValue(ProficiencyModelProperty); }
            set { SetValue(ProficiencyModelProperty, value); }
        }

        private void SkillAttributeEnumListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                foreach (EnumerationExtension.EnumerationMember enumerationMember in e.AddedItems)
                {
                    SkillAttributeEnum choice;
                    if (SkillAttributeEnum.TryParse(enumerationMember.Value.ToString(), out choice))
                    {
                        ProficiencyModelValue.SkillAttributeOptions.Add(choice);
                    }
                }
            }

            if (e.RemovedItems.Count > 0)
            {
                foreach (EnumerationExtension.EnumerationMember enumerationMember in e.RemovedItems)
                {
                    SkillAttributeEnum choice;
                    if (SkillAttributeEnum.TryParse(enumerationMember.Value.ToString(), out choice))
                    {
                        ProficiencyModelValue.SkillAttributeOptions.Remove(choice);
                    }
                }
            }
        }

        public int NumberOfSkillsToChoose
        {
            get { return ProficiencyModelValue != null ? ProficiencyModelValue.NumberOfSkillsToChoose : 0;}
            set
            {
                ProficiencyModelValue.NumberOfSkillsToChoose = value;
                RaisePropertyChanged(nameof(NumberOfSkillsToChoose));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
