﻿using System.Collections.ObjectModel;

namespace FantasyModuleParser.Classes.Model
{
    public class ClassSpecialization
    {
        public string Name;
        public string Description;

        public ObservableCollection<ClassFeature> ClassFeatures;
    }
}
