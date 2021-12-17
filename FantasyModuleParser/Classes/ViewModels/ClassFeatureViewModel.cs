using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.NPC.Commands;
using FantasyModuleParser.NPC.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FantasyModuleParser.Classes.ViewModels
{
    public class ClassFeatureViewModel : ViewModelBase
    {
        public ClassModel classModel { get; set; }
        public ClassFeatureViewModel() : base ()
        {
            classModel = new ClassModel();
        }

        public ClassFeatureViewModel(ClassModel classModel) : base()
        {
            this.classModel = classModel;
        }

        private ClassFeature _selectedClassFeature;
        public ClassFeature SelectedClassFeature
        {
            get { return this._selectedClassFeature; }
            set { Set(ref _selectedClassFeature, value); RaisePropertyChanged(nameof(SelectedClassFeatureDescription)); }
        }
        private ICommand _addClassFeatureCommand;
        public ICommand AddClassFeatureCommand
        {
            get
            {
                if (_addClassFeatureCommand == null)
                {
                    _addClassFeatureCommand = new ActionCommand(param => OnAddClassFeatureAction(),
                        //param => !String.IsNullOrWhiteSpace(SelectedClassFeature?.Name));
                        param => OnAddClassFeatureEnableAction(param as String));
                }
                return _addClassFeatureCommand;
            }
        }

        protected virtual void OnAddClassFeatureAction()
        {
            if (classModel.ClassFeatures == null)
            {
                classModel.ClassFeatures = new ObservableCollection<ClassFeature>();
            }
            classModel.ClassFeatures.Add(SelectedClassFeature.ShallowCopy());
            RaisePropertyChanged(nameof(classModel));
        }

        protected virtual bool OnAddClassFeatureEnableAction(string param)
        {
            return !String.IsNullOrWhiteSpace(param);
        }

        private ICommand _removeClassFeatureCommand;
        public ICommand RemoveClassFeatureCommand
        {
            get
            {
                if (_removeClassFeatureCommand == null)
                {
                    _removeClassFeatureCommand = new ActionCommand(
                        param => OnRemoveClassFeature(param as ClassFeature));
                }
                return _removeClassFeatureCommand;
            }
        }

        protected virtual void OnRemoveClassFeature(ClassFeature classFeature)
        {
            if (classFeature == null)
                return;

            classModel.ClassFeatures.Remove(classFeature);
        }

        private ICommand _clearSelectedFeatureCommand;
        public ICommand ClearSelectedClassFeatureCommand
        {
            get
            {
                if (_clearSelectedFeatureCommand == null)
                {
                    _clearSelectedFeatureCommand = new ActionCommand(
                        param => OnClearSelectedClassFeatureCommand());
                }
                return _clearSelectedFeatureCommand;
            }
        }

        protected virtual void OnClearSelectedClassFeatureCommand()
        {
            SelectedClassFeature = new ClassFeature();
        }

        private ICommand _assignClassSpecializationCommand;
        public ICommand AssignClassSpecializationCommand
        {
            get
            {
                if (_assignClassSpecializationCommand == null)
                {
                    _assignClassSpecializationCommand = new ActionCommand(
                        param => OnAssignClassSpecializationCommand(param as ClassSpecialization));
                }
                return _assignClassSpecializationCommand;
            }
        }

        protected virtual void OnAssignClassSpecializationCommand(ClassSpecialization classSpecialization)
        {
            if (classSpecialization == null)
                return;

            if (classSpecialization.ClassFeatures == null)
                classSpecialization.ClassFeatures = new ObservableCollection<ClassFeature>();

            if (!classSpecialization.ClassFeatures.Contains(SelectedClassFeature))
            {
                classSpecialization.ClassFeatures.Add(SelectedClassFeature);
            }
        }

        public string SelectedClassFeatureDescription
        {
            get { return SelectedClassFeature?.Name + " - Description"; }
        }
    }
}
