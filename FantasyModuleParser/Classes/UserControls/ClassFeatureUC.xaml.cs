using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.NPC.Commands;

namespace FantasyModuleParser.Classes.UserControls
{
    /// <summary>
    /// Interaction logic for ClassFeatureUC.xaml
    /// </summary>
    public partial class ClassFeatureUC : UserControl, INotifyPropertyChanged
    {
        public ClassFeatureUC()
        {
            _selectedClassFeature = new ClassFeature();
            IsFeatureSelected = false;
            InitializeComponent();
            ClassFeatureLayout.DataContext = this;
        }

        public static readonly DependencyProperty ClassModelProperty =
            DependencyProperty.Register("ClassModelValue", typeof(ClassModel), typeof(ClassFeatureUC));


        public ClassModel ClassModelValue
        {
            get { return (ClassModel)GetValue(ClassModelProperty); }
            set { SetValue(ClassModelProperty, value); }
        }

        public bool IsFeatureSelected { get; set; }

        private ClassFeature _selectedClassFeature;
        public ClassFeature SelectedClassFeature
        {
            get { return this._selectedClassFeature; }
            set
            {
                this._selectedClassFeature = value;
                RaisePropertyChanged(nameof(SelectedClassFeature));
            }
        }

        private ICommand _addClassFeatureCommand;
        public ICommand AddClassFeatureCommand
        {
            get
            {
                if (_addClassFeatureCommand == null)
                {
                    _addClassFeatureCommand = new ActionCommand(param => OnAddClassFeatureAction(),
                        param => !String.IsNullOrWhiteSpace(SelectedClassFeature?.Name) );
                }
                return _addClassFeatureCommand;
            }
        }
        
        protected virtual void OnAddClassFeatureAction()
        {
            if (ClassModelValue.ClassFeatures == null)
            {
                ClassModelValue.ClassFeatures = new ObservableCollection<ClassFeature>();
            }
            ClassModelValue.ClassFeatures.Add(SelectedClassFeature);
            RaisePropertyChanged(nameof(ClassModelValue));
            RaisePropertyChanged(nameof(ClassModelValue.ClassFeatures));
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

        private void ClearClassFeatureButton_OnClick(object sender, RoutedEventArgs e)
        {
            _selectedClassFeature = new ClassFeature();
            RaisePropertyChanged(nameof(SelectedClassFeature));
        }
    }
}
