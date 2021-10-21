﻿using System.Collections.ObjectModel;
using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Equipment.Enums;

namespace FantasyModuleParser.Classes.Model
{
    public class ProficiencyModel
    {
        public ObservableCollection<SavingThrowAttributeEnum> SavingThrowAttributes;
        // Typically this number ranges from 0 -> 3;
        public int NumberOfSkillsToChoose;
        public ObservableCollection<SkillAttributeEnum> SkillAttributeOptions;

        public ObservableCollection<ClassStartingToolEnum> ClassStartingToolOptions;
        public int NumberOfGamingSets;
        public int NumberOfMusicalInstruments;

        // Re-Use the ArmorEnum from the Equipment Module portion
        public ObservableCollection<ArmorEnum> ArmorProficiencies;
        // Data to capture unique rules (e.g. Druids cannot use armor or shields made of metal)
        public string UniqueArmorProficencies;


        public string WeaponProficiencies;

        public ProficiencyModel()
        {
            SavingThrowAttributes = new ObservableCollection<SavingThrowAttributeEnum>();
            SkillAttributeOptions = new ObservableCollection<SkillAttributeEnum>();
            ClassStartingToolOptions = new ObservableCollection<ClassStartingToolEnum>();
            ArmorProficiencies = new ObservableCollection<ArmorEnum>();
        }
    }
}
