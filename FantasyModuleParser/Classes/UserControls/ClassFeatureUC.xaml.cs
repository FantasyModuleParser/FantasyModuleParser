using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
            SelectedClassSpecialization = new ClassSpecialization();
        }

        public static readonly DependencyProperty ClassModelProperty =
            DependencyProperty.Register("ClassModelValue", typeof(ClassModel), typeof(ClassFeatureUC));


        public ClassModel ClassModelValue
        {
            get { return (ClassModel)GetValue(ClassModelProperty); }
            set { SetValue(ClassModelProperty, value); }
        }

        public bool IsFeatureSelected { get; set; }

        public ClassSpecialization SelectedClassSpecialization { get; set; }

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

        public string SelectedClassFeatureDescription
        {
            get { return SelectedClassFeature.Name + " - Description"; }
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
            ClassModelValue.ClassFeatures.Add(SelectedClassFeature.ShallowCopy());
            RaisePropertyChanged(nameof(ClassModelValue));
            RaisePropertyChanged(nameof(ClassModelValue.ClassFeatures));
        }

        private ICommand _assignClassSpecializationCommand;
        public ICommand AssignClassSpecializationCommand
        {
            get
            {
                if (_assignClassSpecializationCommand == null)
                {
                    _assignClassSpecializationCommand = new ActionCommand(param => OnAssignClassSpecializationAction(param),
                        param => ClassModelValue.ClassSpecializations.Count > 0);
                }
                return _assignClassSpecializationCommand;
            }
        }

        protected virtual void OnAssignClassSpecializationAction(object param)
        {
            ClassSpecialization classSpecialization = param as ClassSpecialization;
            
            if(classSpecialization != null)
                SelectedClassFeature.AddToClassSpecialization(ClassModelValue, classSpecialization);
        }

        private ICommand _unAssignClassSpecializationCommand;
        public ICommand UnAssignClassSpecializationCommand
        {
            get
            {
                if (_unAssignClassSpecializationCommand == null)
                {
                    _unAssignClassSpecializationCommand = new ActionCommand(param => OnUnAssignClassSpecializationAction(param),
                        param => _isSelectedClassFeaturePartOfAClassSpecialization(SelectedClassFeature));
                }
                return _unAssignClassSpecializationCommand;
            }
        }

        protected virtual void OnUnAssignClassSpecializationAction(object param)
        {
            if(SelectedClassFeature != null)
                SelectedClassFeature.RemoveFromClassSpecialization(ClassModelValue);
        }

        private bool _isSelectedClassFeaturePartOfAClassSpecialization(ClassFeature classFeature)
        {
            if (ClassModelValue.ClassSpecializations == null)
                return false;

            foreach (ClassSpecialization internalClassSpec in ClassModelValue.ClassSpecializations)
            {
                if (internalClassSpec.ClassFeatures == null || internalClassSpec.ClassFeatures.Count == 0)
                    continue;

                ClassFeature checkClassFeature = internalClassSpec.ClassFeatures.FirstOrDefault(item =>
                    item.Name.Equals(SelectedClassFeature.Name, StringComparison.Ordinal));

                // If a ClassFeature object is found, then remove it from the discovered ClassSpecialization
                // so that it can be 'transferred' to the new Class Specialization
                if (checkClassFeature != null)
                {
                    return true;
                }
            }

            return false;
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

        private void FeatureListBoxOnSelectorChangeOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                _selectedClassFeature = e.AddedItems[0] as ClassFeature;
                IsFeatureSelected = true;
            }
            else
            {
                _selectedClassFeature = new ClassFeature();
                IsFeatureSelected = false;
            }
            RaisePropertyChanged(nameof(SelectedClassFeature));
            RaisePropertyChanged(nameof(SelectedClassFeatureDescription));
            RaisePropertyChanged(nameof(IsFeatureSelected));
        }

        private void FrameworkElement_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            // In a twist, at the Opening of the context menu, we want to get the Specialization name for a 
            // selected class feature if one exists.

            ListBox listBox = sender as ListBox;

            if (listBox.SelectedValue is ClassFeature localClassFeature)
            {
                if (_isSelectedClassFeaturePartOfAClassSpecialization(localClassFeature))
                {
                    foreach (ClassSpecialization intClassSpecialization in ClassModelValue.ClassSpecializations)
                    {
                        if (intClassSpecialization.ClassFeatures.FirstOrDefault(
                            item => item.Name.Equals(localClassFeature.Name,StringComparison.Ordinal)) != null)
                        {
                            FeaturnSpecialNameMenuItem.Header = intClassSpecialization.Name;
                            FeaturnSpecialNameMenuItem.Visibility = Visibility.Visible;
                            return;
                        }
                    }

                    
                }
            }

            // If the break point is hit, then that means no class features are assigned 
            // to a class specialization
            FeaturnSpecialNameMenuItem.Visibility = Visibility.Collapsed;
        }
    }
}
