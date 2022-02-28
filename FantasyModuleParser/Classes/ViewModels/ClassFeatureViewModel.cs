using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.NPC.Commands;
using FantasyModuleParser.NPC.ViewModel;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Collections.Generic;
using FantasyModuleParser.Classes.Windows.ClassFeature;
using FantasyModuleParser.NPC;

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

        private Boolean _isNewClassFeature = false;

        private ClassFeature _selectedClassFeature;
        public ClassFeature SelectedClassFeature
        {
            get { return this._selectedClassFeature; }
            set { 
                if(value != null)
                    Set(ref _selectedClassFeature, value);
                else
                    Set(ref _selectedClassFeature, new ClassFeature());

                RaisePropertyChanged(nameof(this.classModel.ClassFeatures));
                RaisePropertyChanged(nameof(SelectedClassFeatureName));
                RaisePropertyChanged(nameof(SelectedClassFeatureDescription));
                RaisePropertyChanged(nameof(GetClassSpecializationNameForSelectedClassFeature));
                RaisePropertyChanged(nameof(AddUpdateButtonContent));
            }
        }

        public string AddUpdateButtonContent
        {
            get { if ((classModel.ClassFeatures != null && classModel.ClassFeatures.Contains(SelectedClassFeature)))
                    return "Updating Feature";
                return "Add Feature";
            }
        }
        private ICommand _showCreateClassFeatureWindowCommand;
        public ICommand ShowCreateClassFeatureWindowCommand
        {
            get
            {
                if (_showCreateClassFeatureWindowCommand == null)
                {
                    _showCreateClassFeatureWindowCommand = new ActionCommand(param => OnShowCreateClassFeatureWindowCommand(null));
                }
                return _showCreateClassFeatureWindowCommand;
            }
        }
        protected virtual void OnShowCreateClassFeatureWindowCommand(ClassFeature classFeature)
        {
            ClassFeatureWindow classFeatureWindow = new ClassFeatureWindow();
            if (classFeature == null) 
            {                 
                this.SelectedClassFeature = new ClassFeature();
                this._isNewClassFeature = true;
            }
            else 
            {
                // Create a shallow copy of the selected CF to edit as to not mess with the existing item!
                // This is an ass backwards way of handling this without a DB structure, but in order to sync
                // up the edited ClassFeature, then assign a random ID value to classFeature BEFORE writing a shallow
                // copy.  Then, at the UpdateCFAction method, sync up to the corresponding ClassFeature with the matching
                // ID value
                classFeature.Id = CommonMethod.LongRandom(0, (1024 * 1024 * 1024));
                this.SelectedClassFeature = classFeature.ShallowCopy();
                this._isNewClassFeature = false;
            }
            classFeatureWindow.DataContext = this;
            classFeatureWindow.Show();

        }


        private ICommand _updateClassFeatureCommand;
        public ICommand UpdateClassFeatureCommand
        {
            get
            {
                if (_updateClassFeatureCommand == null)
                {
                    _updateClassFeatureCommand = new ActionCommand(
                        param => OnUpdateClassFeatureCommand());
                }
                return _updateClassFeatureCommand;
            }
        }

        protected virtual void OnUpdateClassFeatureCommand()
        {
            // If the _isNewClassFeature flag is set to true, then add it to the list of Class Features
            // Otherwise, do nothing (as MVVM auto-magically updates 
            if (this._isNewClassFeature)
            {
                classModel.ClassFeatures.Add(this.SelectedClassFeature);
                this._isNewClassFeature = false;
            } 
            else
            {
                this.classModel.UpdateClassFeature(this.SelectedClassFeature);
            }
            RaisePropertyChanged(nameof(classModel));
            RaisePropertyChanged(nameof(classModel.ClassFeatures));

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
            RaisePropertyChanged(nameof(classModel));
            RaisePropertyChanged(nameof(classModel.ClassFeatures));
        }

        private ICommand _editClassFeatureCommand;
        public ICommand EditClassFeatureCommand
        {
            get
            {
                if (_editClassFeatureCommand == null)
                {
                    _editClassFeatureCommand = new ActionCommand(
                        param => OnShowCreateClassFeatureWindowCommand(param as ClassFeature));
                }
                return _editClassFeatureCommand;
            }
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
                RaisePropertyChanged(nameof(AddUpdateButtonContent));
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
                    .Where(_ => _ != null && _.Name.Equals(SelectedClassFeature.Name, StringComparison.OrdinalIgnoreCase)).ToList();

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

                if (classSpecialization.ClassFeatures?
                    .FirstOrDefault(
                        _ => _ != null && _.Name.Equals(SelectedClassFeature.Name, StringComparison.OrdinalIgnoreCase)) != null)
                    return true;
            }
            return false;
        }

        public string GetClassSpecializationNameForSelectedClassFeature
        {
            get { 
                foreach (ClassSpecialization classSpecialization in this.classModel.ClassSpecializations)
                {
                    if (classSpecialization.ClassFeatures == null)
                        classSpecialization.ClassFeatures = new ObservableCollection<ClassFeature>();

                    if (classSpecialization.ClassFeatures
                        .FirstOrDefault(
                            _ => _ != null &&_.Name.Equals(SelectedClassFeature.Name, StringComparison.OrdinalIgnoreCase)) != null)
                        return String.Format("Un-Assign from {0}", classSpecialization.Name);
                }
                return "";
            }
            private set { }
        }
    }
}
