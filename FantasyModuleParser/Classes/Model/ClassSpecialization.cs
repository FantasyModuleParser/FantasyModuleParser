﻿using FantasyModuleParser.NPC.ViewModel;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace FantasyModuleParser.Classes.Model
{
    public class ClassSpecialization : ViewModelBase
    {
        private long _id;
        [JsonIgnore]
        public long Id
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }
        private string _name;
        public string Name { get { return this._name; } set { Set(ref _name, value); } }
        private string _description;
        public string Description
        {
            get { return this._description; }
            set { Set(ref _description, value); }
        }
        public int Level { get; set; }

        public ObservableCollection<ClassFeature> ClassFeatures { get; set; }

        public ClassSpecialization ShallowCopy()
        {
            return (ClassSpecialization)this.MemberwiseClone();
        }
    }
}
