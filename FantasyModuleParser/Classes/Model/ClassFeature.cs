using System;

namespace FantasyModuleParser.Classes.Model
{
    public class ClassFeature
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }

        public void AddToClassSpecialization(ClassSpecialization classSpecialization)
        {
            throw new NotImplementedException();
        }

        public void RemoveFromClassSpecialization(ClassSpecialization classSpecialization)
        {
            throw new NotImplementedException();
        }

        public ClassFeature ShallowCopy()
        {
            return (ClassFeature)this.MemberwiseClone();
        }
    }
}
