using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Classes.Model;
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
        private ClassModel _classModelValue;

        public ClassSpecializationViewModel() : base()
        {
            _classModelValue = new ClassModel();
        }

        public ClassSpecializationViewModel(ClassModel classModel): base()
        {
            this.ClassModelValue = classModel;
            SelectedClassSpecialization = new ClassSpecialization();
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
            }
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
