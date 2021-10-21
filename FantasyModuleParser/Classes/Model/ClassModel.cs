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
        public bool IsLocked;

        public ObservableCollection<ProficiencyBonusModel> ProfBonusValues;

        public bool HasSpellSlots;
        public ObservableCollection<SpellSlotModel> SpellSlotValues;

        public ProficiencyModel Proficiency;
        public ProficiencyModel MultiProficiency;

        public ObservableCollection<ClassFeature> ClassFeatures;

        public string ClassSpecializationDescription;
        public ObservableCollection<ClassSpecialization> ClassSpecializations;

        // The following attributes are catch-alls, in that there's no hardcoded way to generate
        // the required data, such as custom weapons / tools in a campaign setting:

        public string ToolProficiencies;
        public string StartingEquipment;

        public ClassModel()
        {
            PrePopulateProficiencyBonusValues();
            PrePopulateSpellSlotValues();
            PrePopulateStartingEquipment();
            Proficiency = new ProficiencyModel();
            MultiProficiency = new ProficiencyModel();
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
