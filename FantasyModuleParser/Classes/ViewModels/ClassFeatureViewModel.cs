using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.NPC.Commands;
using FantasyModuleParser.NPC.ViewModel;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Collections.Generic;

namespace FantasyModuleParser.Classes.ViewModels
{
    public class ClassFeatureViewModel : ViewModelBase
    {
        public ClassModel classModel { get; set; }
        public ClassFeatureViewModel() : base ()
        {
            classModel = new ClassModel();
            SelectedClassFeature = new ClassFeature();
        }

        public ClassFeatureViewModel(ClassModel classModel) : base()
        {
            this.classModel = classModel;
            SelectedClassFeature = new ClassFeature();
        }

        public ObservableCollection<ClassFeature> ClassFeatures
        {
            get { return classModel.ClassFeatures; }
            set
            {
                classModel.ClassFeatures = value;
                RaisePropertyChanged(nameof(ClassFeatures));
            }
        }

        private ClassFeature _selectedClassFeature;
        public ClassFeature SelectedClassFeature
        {
            get { return this._selectedClassFeature; }
            set { Set(ref _selectedClassFeature, value);

                // Re-sort the Class Features based on the Level value (Ascending)
                if(this.classModel != null && this.classModel.ClassFeatures != null)
                { 
                    this.classModel.ClassFeatures = 
                        new ObservableCollection<ClassFeature>( this.classModel.ClassFeatures.OrderBy(_ => _.Level).ToList());
                }
                RaisePropertyChanged(nameof(this.classModel.ClassFeatures));
                RaisePropertyChanged(nameof(SelectedClassFeatureName));
                RaisePropertyChanged(nameof(SelectedClassFeatureDescription));
                RaisePropertyChanged(nameof(GetClassSpecializationNameForSelectedClassFeature));
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
            classModel.ClassFeatures = new ObservableCollection<ClassFeature>(classModel.ClassFeatures.OrderBy(_ => _.Level).ToList());
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
            classModel.RemoveClassFeature(classFeature);
            RaisePropertyChanged(nameof(classModel.ClassFeatures));
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
                        param => OnAssignClassSpecializationCommand(param as ClassSpecialization), 
                        param => !IsSelectedClassFeatureAssignedToSpecialization());
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

        public string SelectedClassFeatureName
        {
            get { return SelectedClassFeature?.Name; }
            set { SelectedClassFeature.Name = value;
            RaisePropertyChanged(nameof(SelectedClassFeatureName));
            }
        }

        private ICommand _unassignClassSpecializationCommand;
        public ICommand UnassignClassSpecializationCommand
        {
            get
            {
                if (_unassignClassSpecializationCommand == null)
                {
                    _unassignClassSpecializationCommand = new ActionCommand(param => OnUnassignClassSpecializationAction(),
                        //param => !String.IsNullOrWhiteSpace(SelectedClassFeature?.Name));
                        param => IsSelectedClassFeatureAssignedToSpecialization());
                }
                return _unassignClassSpecializationCommand;
            }
        }

        protected virtual void OnUnassignClassSpecializationAction()
        {
            foreach (ClassSpecialization classSpecialization in this.classModel.ClassSpecializations)
            {
                List<ClassFeature> classFeatureList = classSpecialization.ClassFeatures
                    .Where(_ => _.Name.Equals(SelectedClassFeature.Name, StringComparison.OrdinalIgnoreCase)).ToList();

                foreach(ClassFeature removeClassFeature in classFeatureList)
                {
                    classSpecialization.ClassFeatures.Remove(removeClassFeature);
                }
            }
        }

        protected virtual bool IsSelectedClassFeatureAssignedToSpecialization()
        {
            foreach(ClassSpecialization classSpecialization in this.classModel.ClassSpecializations)
            {
                if (classSpecialization.ClassFeatures
                    .FirstOrDefault(_ => _.Name.Equals(SelectedClassFeature.Name, StringComparison.OrdinalIgnoreCase)) != null)
                    return true;
            }
            return false;
        }

        public string GetClassSpecializationNameForSelectedClassFeature
        {
            get { 
                foreach (ClassSpecialization classSpecialization in this.classModel.ClassSpecializations)
                {
                    if (classSpecialization.ClassFeatures
                        .FirstOrDefault(_ => _.Name.Equals(SelectedClassFeature.Name, StringComparison.OrdinalIgnoreCase)) != null)
                        return String.Format("Un-Assign from {0}", classSpecialization.Name);
                }
                return "";
            }
            private set { }
        }
    }
}
