using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Data;
using System.Collections;
using FantasyModuleParser.Classes.Comparators;

namespace FantasyModuleParser.Classes.Model
{
    public class ClassModel : ModelBase
    {
        public string Description { get; set; }
        public ClassHitDieEnum HitPointDiePerLevel { get; set; }
        public bool IsLocked { get; set; }

        public ObservableCollection<ProficiencyBonusModel> ProfBonusValues { get; set; }

        public bool HasSpellSlots { get; set; }
        public ObservableCollection<SpellSlotModel> SpellSlotValues { get; set; }

        public ProficiencyModel Proficiency;
        public ProficiencyModel MultiProficiency;

        public ObservableCollection<ClassFeature> ClassFeatures { get; set; }
        private ListCollectionView _listCollectionViewClassFeatures;

        public string ClassSpecializationDescription { get; set; }
        public ObservableCollection<ClassSpecialization> ClassSpecializations { get; set; }

        // The following attributes are catch-alls, in that there's no hardcoded way to generate
        // the required data, such as custom weapons / tools in a campaign setting:

        public string ToolProficiencies { get; set; }
        public string StartingEquipment { get; set; }
        public string ImageFilePath { get; set; }

        public ClassModel()
        {
            // PrePopulateProficiencyBonusValues();
            // PrePopulateSpellSlotValues();
            // PrePopulateStartingEquipment();
            Proficiency = new ProficiencyModel();
            MultiProficiency = new ProficiencyModel();
            IntializeClassFeatures();
        }

        public void PrePopulateProficiencyBonusValues()
        {
            this.ProfBonusValues = new ObservableCollection<ProficiencyBonusModel>();
            for (int i = 0; i < 20; i++)
            {
                this.ProfBonusValues.Add(new ProficiencyBonusModel(i + 1, 0));
            }
        }

        public void PrePopulateSpellSlotValues()
        {
            this.SpellSlotValues = new ObservableCollection<SpellSlotModel>();
            for (int i = 0; i < 20; i++)
            {
                this.SpellSlotValues.Add(new SpellSlotModel(i + 1));
            }
        }

        public void PrePopulateStartingEquipment()
        {
            this.StartingEquipment = "You start with the following equipment, in addition to the equipment granted by your background:\n\n* Item 1\n\n* Item 2";
        }

        public void UpdateClassFeature(ClassFeature classFeature)
        {
            if (classFeature == null)
                return;

            ClassFeature foundClassFeature = ClassFeatures.Where(x => x.Id == classFeature.Id)
                .FirstOrDefault();

            if (foundClassFeature != null)
            {
                ClassFeatures.Remove(foundClassFeature);
            }

            ClassFeatures.Add(classFeature);
        }

        public void RemoveClassFeature(ClassFeature classFeature)
        {
            if (classFeature == null)
                return;

            ClassFeatures.Remove(classFeature);

            if(ClassSpecializations != null)    
                foreach (ClassSpecialization classSpecialization in ClassSpecializations)
                {
                    if(classSpecialization.ClassFeatures != null && classSpecialization.ClassFeatures.Count > 0)
                    {
                        classSpecialization.ClassFeatures.Remove(classFeature);
                    }
                }
        }
        public void RemoveClassSpecialization(ClassSpecialization classSpecialization)
        {
            ClassSpecializations.Remove(classSpecialization);
        }


        public void Save()
        {
            SettingsService settingsService = new SettingsService();
            SettingsModel settingsModel = settingsService.Load();
            string folderPath = settingsModel.ClassFolderLocation;

            Save(folderPath);
        }
        public void Save(string folderPath)
        {
            Directory.CreateDirectory(folderPath);
            using (StreamWriter file = File.CreateText(Path.Combine(folderPath, this.Name + ".json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, this);
            }
        }

        public ClassModel Load(string @filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(@filePath);
                return JsonConvert.DeserializeObject<ClassModel>(jsonData);
            }
            return null;
        }

        public ClassModel ShallowCopy()
        {
            return (ClassModel)this.MemberwiseClone();
        }

        public void IntializeClassFeatures()
        {
            ClassFeatures = new ObservableCollection<ClassFeature>();
            _listCollectionViewClassFeatures = CollectionViewSource.GetDefaultView(ClassFeatures) as ListCollectionView;

            if(_listCollectionViewClassFeatures != null)
            {
                _listCollectionViewClassFeatures.IsLiveSorting = true;
                _listCollectionViewClassFeatures.CustomSort = new ClassFeatureByLevel();
            }
        }
    }
}
