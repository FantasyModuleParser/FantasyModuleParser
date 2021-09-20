using FantasyModuleParser.Classes.Enums;
using System;
using System.Collections.ObjectModel;

namespace FantasyModuleParser.Classes.Model
{
    public class ClassModel
    {
        public string Name;
        public string Description;
        public ClassHitDieEnum HitPointDiePerLevel;
        public ObservableCollection<SavingThrowAttributeEnum> SavingThrowAttributes;
        public ObservableCollection<ClassFeature> ClassFeatures;
        public ObservableCollection<ClassSpecialization> ClassSpecializations;

        public void RemoveClassFeature(ClassFeature classFeature)
        {
            throw new NotImplementedException();
        }
        public void RemoveClassSpecialization(ClassSpecialization classSpecialization)
        {
            throw new NotImplementedException();
        }
    }
}
