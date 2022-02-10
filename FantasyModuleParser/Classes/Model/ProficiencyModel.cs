using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Equipment.Enums;
using System.Linq;
using FantasyModuleParser.Extensions;

namespace FantasyModuleParser.Classes.Model
{
    public class ProficiencyModel
    {
        public ObservableCollection<SavingThrowAttributeEnum> SavingThrowAttributes;
        // Typically this number ranges from 0 -> 3;
        public int NumberOfSkillsToChoose { get; set; }
        public HashSet<SkillAttributeEnum> SkillAttributeOptions;

        public HashSet<ClassStartingToolEnum> ClassStartingToolOptions;
        public int NumberOfGamingSets { get; set; }
        public int NumberOfMusicalInstruments { get; set; }

        // Re-Use the ArmorEnum from the Equipment Module portion
        public ObservableCollection<ArmorEnum> ArmorProficiencies;
        // Data to capture unique rules (e.g. Druids cannot use armor or shields made of metal)
        public string UniqueArmorProficencies { get; set; }


        public string WeaponProficiencies { get; set; }

        public ProficiencyModel()
        {
            SavingThrowAttributes = new ObservableCollection<SavingThrowAttributeEnum>();
            SkillAttributeOptions = new HashSet<SkillAttributeEnum>();
            ClassStartingToolOptions = new HashSet<ClassStartingToolEnum>();
            ArmorProficiencies = new ObservableCollection<ArmorEnum>();
        }

        public string GetArmorProficienciesForExporter()
        {
            string delimiter = ", ";
            StringBuilder stringBuilder = new StringBuilder();

            foreach(ArmorEnum armor in ArmorProficiencies)
            {
                stringBuilder.Append(armor.GetDescription()).Append(delimiter);
            }
            return stringBuilder.ToString(0, stringBuilder.Length - delimiter.Length); 
        }

        public string GetToolProficienciesForExporter()
        {
            string delimiter = ", ";
            StringBuilder stringBuilder = new StringBuilder();

            foreach (ClassStartingToolEnum tool in ClassStartingToolOptions)
            {
                stringBuilder.Append(tool.GetDescription()).Append(delimiter);
            }
            return stringBuilder.ToString(0, stringBuilder.Length - delimiter.Length);
        }

        public void GetSkillProficienciesForExporter(int switchval)
		{
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Choose ");
            stringBuilder.Append(NumberOfSkills(switchval));
            stringBuilder.Append("from ");
            stringBuilder.Append(ListOfSkills());
		}

        public string NumberOfSkills(int switchval)
        {
            StringBuilder stringBuilder = new StringBuilder();
            return switchval switch
            {
                0 => NoSkills(),
                1 => stringBuilder.Append("one ").ToString(),
                2 => stringBuilder.Append("two ").ToString(),
                3 => stringBuilder.Append("three ").ToString(),
                4 => stringBuilder.Append("four ").ToString(),
                5 => stringBuilder.Append("five ").ToString(),
                _ => NoSkills(),
            };
        }

        public string ListOfSkills()
		{
            string delimiter = ", ";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (SkillAttributeEnum skills in SkillAttributeOptions)
            {
                stringBuilder.Append(skills.GetDescription()).Append(delimiter);
            }
            return stringBuilder.ToString(0, stringBuilder.Length - delimiter.Length);
        }

        public string NoSkills()
		{
            return "";
		}
    }
}
