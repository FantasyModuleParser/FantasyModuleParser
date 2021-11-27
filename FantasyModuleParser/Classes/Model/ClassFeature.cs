using FantasyModuleParser.NPC.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FantasyModuleParser.Classes.Model
{
    public class ClassFeature : ViewModelBase
    {
        private string _name;
        public string Name { get { return this._name; } set { Set(ref _name, value); } }
        private int _level;
        public int Level
        {
            get { return this._level; }
            set { Set(ref _level, value); }
        }
        private string _description;  
        public string Description
        {
            get { return this._description; }
            set { Set(ref _description, value); }
        }

        public void AddToClassSpecialization(ClassModel classModel, ClassSpecialization classSpecialization)
        {
            // 1. Validate that the classSpecialization is not null;
            if (classSpecialization == null)
                throw new NullReferenceException();

            // 2. Check all Class Specializations in ClassModel so that the feature does not exist anywhere else
            _removeClassFeatureFromExistingClassSpecializations(classModel);

            // 3. If the ClassFeatures for the selected ClassSpecialization is null, then set it here
            if (classSpecialization.ClassFeatures == null)
                classSpecialization.ClassFeatures = new ObservableCollection<ClassFeature>();

            // 4. Add this ClassFeature to the selected ClassSpecialization object
            classSpecialization.ClassFeatures.Add(this);
            
        }

        public void RemoveFromClassSpecialization(ClassModel classModel)
        {
            _removeClassFeatureFromExistingClassSpecializations(classModel);
        }

        public ClassFeature ShallowCopy()
        {
            return (ClassFeature)this.MemberwiseClone();
        }

        private void _removeClassFeatureFromExistingClassSpecializations(ClassModel classModel)
        {
            foreach (ClassSpecialization internalClassSpec in classModel.ClassSpecializations)
            {
                if (internalClassSpec.ClassFeatures == null || internalClassSpec.ClassFeatures.Count == 0)
                    continue;

                ClassFeature checkClassFeature = internalClassSpec.ClassFeatures.FirstOrDefault(item =>
                    item.Name.Equals(this.Name, StringComparison.Ordinal));

                // If a ClassFeature object is found, then remove it from the discovered ClassSpecialization
                // so that it can be 'transferred' to the new Class Specialization
                if (checkClassFeature != null)
                {
                    internalClassSpec.ClassFeatures.Remove(checkClassFeature);
                }
            }
        }
    }
}
