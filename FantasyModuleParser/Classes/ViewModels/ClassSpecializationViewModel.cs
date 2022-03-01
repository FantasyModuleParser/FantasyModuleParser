using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.Classes.Windows.ClassSpecialization;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Commands;
using FantasyModuleParser.NPC.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace FantasyModuleParser.Classes.ViewModels
{
    public class ClassSpecializationViewModel : ViewModelBase
    {
        private Boolean _isNewClassSpecialization;
        private ClassModel _classModelValue;

        public ClassSpecializationViewModel() : base()
        {
            _classModelValue = new ClassModel();
            _isNewClassSpecialization = false;
        }

        public ClassSpecializationViewModel(ClassModel classModel): base()
        {
            this.ClassModelValue = classModel;
            SelectedClassSpecialization = new ClassSpecialization();
            _isNewClassSpecialization = false;
            RaisePropertyChanged(nameof(this.ClassModelValue));
            RaisePropertyChanged(nameof(this.ClassModelValue.ClassSpecializations));

        }

        public ClassModel ClassModelValue
        {
            get { return _classModelValue; }
            set { _classModelValue = value; }
        }
        public string SelectedClassSpecializationName
        {
            get { return SelectedClassSpecialization?.Name; }
            set {
                if (SelectedClassSpecialization == null) SelectedClassSpecialization = new ClassSpecialization();
                SelectedClassSpecialization.Name = value;
                RaisePropertyChanged(nameof(ClassModelValue));
                RaisePropertyChanged(nameof(ClassModelValue.ClassSpecializations));
                RaisePropertyChanged(nameof(SelectedClassSpecialization));
            }
        }
        public string SelectedClassSpecializationDescription
        {
            get { return SelectedClassSpecialization?.Description; }
            set {
                if (SelectedClassSpecialization == null) SelectedClassSpecialization = new ClassSpecialization();
                SelectedClassSpecialization.Description = value;
                RaisePropertyChanged(nameof(ClassModelValue));
                RaisePropertyChanged(nameof(ClassModelValue.ClassSpecializations));
                RaisePropertyChanged(nameof(SelectedClassSpecializationDescription));
            }
        }

        public bool IsClassSpecialSelected { get { return SelectedClassSpecialization != null; }
            private set { }
        }

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
                RaisePropertyChanged(nameof(ClassModelValue));
                RaisePropertyChanged(nameof(ClassModelValue.ClassSpecializations));
                RaisePropertyChanged(nameof(SelectedClassSpecialization));
                RaisePropertyChanged(nameof(SelectedClassSpecialization.ClassFeatures));
                RaisePropertyChanged(nameof(TabDescriptionLabel));
                //RaisePropertyChanged(nameof(IsClassSpecialSelected));
                RaisePropertyChanged(nameof(SelectedClassSpecializationName));
                RaisePropertyChanged(nameof(SelectedClassSpecializationDescription));
            }
        }

        public string AddUpdateButtonContent
        {
            get
            {
                if ((ClassModelValue.ClassSpecializations != null 
                    && ClassModelValue.ClassSpecializations.Contains(SelectedClassSpecialization)))
                    return "Updating Specialization";
                return "Add Specialization";
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
            //RaisePropertyChanged(nameof(SelectedClassSpecialization));
            //RaisePropertyChanged(nameof(TabDescriptionLabel));
            //RaisePropertyChanged(nameof(IsClassSpecialSelected));
        }

        private ICommand _addClassSpecializationCommand;
        public ICommand AddClassSpecializationCommand
        {
            get
            {
                if (_addClassSpecializationCommand == null)
                {
                    _addClassSpecializationCommand = new ActionCommand(param => OnAddClassSpecializationAction(),
                        param => !String.IsNullOrWhiteSpace(SelectedClassSpecialization?.Name)
                            && !(ClassModelValue.ClassSpecializations != null 
                                && ClassModelValue.ClassSpecializations.Contains(SelectedClassSpecialization)));
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

        private ICommand _editClassSpecializationCommand;
        public ICommand EditClassSpecializationCommand
        {
            get
            {
                if (_editClassSpecializationCommand == null)
                {
                    _editClassSpecializationCommand = new ActionCommand(param =>
                        OnShowCreateClassSpecializationWindowCommand(param as ClassSpecialization));
                }
                return _editClassSpecializationCommand;
            }
        }

        private ICommand _removeClassSpecializationCommand;
        public ICommand RemoveClassSpecializationCommand
        {
            get
            {
                if (_removeClassSpecializationCommand == null)
                {
                    _removeClassSpecializationCommand = new ActionCommand(param =>
                        OnRemoveClassSpecializationAction(param as ClassSpecialization));
                }
                return _removeClassSpecializationCommand;
            }
        }

        protected virtual void OnRemoveClassSpecializationAction(ClassSpecialization classSpecialization)
        {
            ClassModelValue.RemoveClassSpecialization(classSpecialization);
            RaisePropertyChanged(nameof(ClassModelValue.ClassSpecializations));
        }

        private ICommand _removeClassFeatureCommand;
        public ICommand RemoveClassFeatureCommand
        {
            get
            {
                if (_removeClassFeatureCommand == null)
                {
                    _removeClassFeatureCommand = new ActionCommand(param =>
                        OnRemoveClassFeatureAction(param as ClassFeature));
                }
                return _removeClassFeatureCommand;
            }
        }

        protected virtual void OnRemoveClassFeatureAction(ClassFeature classFeature)
        {
            if(SelectedClassSpecialization != null)
            {
                SelectedClassSpecialization.ClassFeatures.Remove(classFeature);
                classFeature.ClassSpecialization = null;
            }
            RaisePropertyChanged(nameof(ClassModelValue.ClassSpecializations));
        }


        private ICommand _updateClassSpecializationCommand;
        public ICommand UpdateClassSpecializationCommand
        {
            get
            {
                if (_updateClassSpecializationCommand == null)
                {
                    _updateClassSpecializationCommand = new ActionCommand(
                        param => OnUpdateClassSpecializationCommand());
                }
                return _updateClassSpecializationCommand;
            }
        }

        protected virtual void OnUpdateClassSpecializationCommand()
        {
            // If the _isNewClassFeature flag is set to true, then add it to the list of Class Features
            // Otherwise, do nothing (as MVVM auto-magically updates 
            if (this._isNewClassSpecialization)
            {
                this.ClassModelValue.ClassSpecializations.Add(this.SelectedClassSpecialization);
                this._isNewClassSpecialization= false;
            } 
            else
            {
                this.ClassModelValue.UpdateClassSpecializationNameAndDescription(this.SelectedClassSpecialization);
            }
            RaisePropertyChanged(nameof(ClassModelValue));
            RaisePropertyChanged(nameof(ClassModelValue.ClassSpecializations));
            RaisePropertyChanged(nameof(SelectedClassSpecialization));
            RaisePropertyChanged(nameof(SelectedClassSpecializationDescription));
        }

        private ICommand _showCreateClassSpecializationWindowCommand;
        public ICommand ShowCreateClassSpecializationWindowCommand
        {
            get
            {
                if (_showCreateClassSpecializationWindowCommand == null)
                {
                    _showCreateClassSpecializationWindowCommand = new ActionCommand(param => OnShowCreateClassSpecializationWindowCommand(null));
                }
                return _showCreateClassSpecializationWindowCommand;
            }
        }
        protected virtual void OnShowCreateClassSpecializationWindowCommand(ClassSpecialization classSpecialization)
        {
            ClassSpecializationWindow classFeatureWindow = new ClassSpecializationWindow();
            if (classSpecialization == null)
            {
                this.SelectedClassSpecialization = new ClassSpecialization();
                this._isNewClassSpecialization = true;
            }
            else
            {
                // Create a shallow copy of the selected CF to edit as to not mess with the existing item!
                // This is an ass backwards way of handling this without a DB structure, but in order to sync
                // up the edited ClassFeature, then assign a random ID value to classFeature BEFORE writing a shallow
                // copy.  Then, at the UpdateCFAction method, sync up to the corresponding ClassFeature with the matching
                // ID value
                classSpecialization.Id = CommonMethod.LongRandom(0, (1024 * 1024 * 1024));
                this.SelectedClassSpecialization = classSpecialization.ShallowCopy();
                this._isNewClassSpecialization = false;
            }
            classFeatureWindow.DataContext = this;
            classFeatureWindow.Show();
            RaisePropertyChanged(nameof(ClassModelValue.ClassSpecializations));

        }


        //** ================ DELETE EVERYTHING BELOW HERE ==================== **//

        private ClassMenuOptionEnum _classMenuOptionEnum = ClassMenuOptionEnum.ClassSpecialization;
        public ClassMenuOptionEnum ClassMenuOptionEnum
        {
            get { return _classMenuOptionEnum; }
            set { Set(ref _classMenuOptionEnum, value); }
        }
    }
}
