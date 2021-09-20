using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Equipment.Enums;
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

        // Typically this number ranges from 0 -> 3;
        public int NumberOfSkillsToChoose;
        public ObservableCollection<SkillAttributeEnum> SkillAttributeOptions;

        // Re-Use the ArmorEnum from the Equipment Module portion
        public ObservableCollection<ArmorEnum> ArmorProficiencies;
        // Data to capture unique rules (e.g. Druids cannot use armor or shields made of metal)
        public string UniqueArmorProficencies;

        public ObservableCollection<ClassFeature> ClassFeatures;

        public string ClassSpecializationDescription;
        public ObservableCollection<ClassSpecialization> ClassSpecializations;

        // The following attributes are catch-alls, in that there's no hardcoded way to generate
        // the required data, such as custom weapons / tools in a campaign setting:

        public string ToolProficiencies;
        public string WeaponProficiencies;

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
