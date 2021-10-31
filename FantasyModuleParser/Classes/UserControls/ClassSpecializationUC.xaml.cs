using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.NPC.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FantasyModuleParser.Classes.UserControls
{
    /// <summary>
    /// Interaction logic for ClassSpecializationUC.xaml
    /// </summary>
    public partial class ClassSpecializationUC : UserControl, INotifyPropertyChanged
    {
        public ClassSpecializationUC()
        {
            _selectedClassSpecialization = new ClassSpecialization();
            IsClassSpecialSelected = false;
            InitializeComponent();
            ClassSpecializationLayout.DataContext = this;
        }

        public static readonly DependencyProperty ClassModelProperty =
            DependencyProperty.Register("ClassModelValue", typeof(ClassModel), typeof(ClassSpecializationUC));


        public ClassModel ClassModelValue
        {
            get { return (ClassModel)GetValue(ClassModelProperty); }
            set { SetValue(ClassModelProperty, value); }
        }

        public bool IsClassSpecialSelected { get; set; }

        public string TabDescriptionLabel
        {
            get { return _selectedClassSpecialization?.Name + " - Description"; }
        }

        private ClassSpecialization _selectedClassSpecialization;

        public ClassSpecialization SelectedClassSpecialization
        {
            get { return this._selectedClassSpecialization; }
            set
            {
                this._selectedClassSpecialization = value;
                RaisePropertyChanged(nameof(SelectedClassSpecialization));
            }
        }

        private void OnClassSpecializationListBox_SelectionChangedEvent(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                _selectedClassSpecialization = e.AddedItems[0] as ClassSpecialization;
                IsClassSpecialSelected = true;
            }
            else
            {
                _selectedClassSpecialization = new ClassSpecialization();
                IsClassSpecialSelected = false;
            }
            RaisePropertyChanged(nameof(SelectedClassSpecialization));
            RaisePropertyChanged(nameof(TabDescriptionLabel));
            RaisePropertyChanged(nameof(IsClassSpecialSelected));
        }

        private ICommand _addClassSpecializationCommand;
        public ICommand AddClassSpecializationCommand
        {
            get
            {
                if (_addClassSpecializationCommand == null)
                {
                    _addClassSpecializationCommand = new ActionCommand(param => OnAddClassSpecializationAction(),
                        param => !String.IsNullOrWhiteSpace(SelectedClassSpecialization?.Name));
                }
                return _addClassSpecializationCommand;
            }
        }

        protected virtual void OnAddClassSpecializationAction()
        {
            if (ClassModelValue.ClassSpecializations == null)
            {
                ClassModelValue.ClassSpecializations = new ObservableCollection<ClassSpecialization>();
            }
            ClassModelValue.ClassSpecializations.Add(SelectedClassSpecialization.ShallowCopy());
            RaisePropertyChanged(nameof(ClassModelValue));
            RaisePropertyChanged(nameof(ClassModelValue.ClassSpecializations));
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
