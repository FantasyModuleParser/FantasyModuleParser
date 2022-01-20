using System.Collections.ObjectModel;

namespace FantasyModuleParser.Classes.Model
{
    public class ClassSpecialization
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }

        public ObservableCollection<ClassFeature> ClassFeatures { get; set; }

        public ClassSpecialization ShallowCopy()
        {
            return (ClassSpecialization)this.MemberwiseClone();
        }
    }
}
