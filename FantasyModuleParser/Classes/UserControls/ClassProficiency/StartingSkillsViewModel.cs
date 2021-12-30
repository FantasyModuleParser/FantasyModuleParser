using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.NPC.ViewModel;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using static FantasyModuleParser.Extensions.EnumerationExtension;
using System.ComponentModel;
using System.Collections.Generic;

namespace FantasyModuleParser.Classes.UserControls.ClassProficiency
{
    public class StartingSkillsViewModel : ClassProficiencyViewModelBase
    {
        public StartingSkillsViewModel()
        {
        }

        public StartingSkillsViewModel(ProficiencyModel proficiencyModel) : base(proficiencyModel)
        {
            if (proficiencyModel == null)
                return;

            if (proficiencyModel.SkillAttributeOptions == null)
                proficiencyModel.SkillAttributeOptions = new HashSet<SkillAttributeEnum>();

            var skillAttributeOptions = (SkillAttributeEnum[])Enum.GetValues(typeof(SkillAttributeEnum));
            foreach (var skillAttributeOption in skillAttributeOptions)
            {
                SelectableItem item = new SelectableItem()
                {
                    Name = skillAttributeOption.ToString(),
                    Description = GetDescription(typeof(SkillAttributeEnum), skillAttributeOption),
                    IsSelected = false
                };

                if(proficiencyModel.SkillAttributeOptions.Contains(skillAttributeOption))
                {
                    item.IsSelected = true;
                }

                SkillsList.Add(item);
            }
        }

        private string GetDescription(Type EnumType, object enumValue)
        {
            DescriptionAttribute descriptionAttribute = EnumType
                .GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            return descriptionAttribute != null ? descriptionAttribute.Description : enumValue.ToString();
        }

        public ObservableCollection<SelectableItem> SkillsList { get; set; } = new ObservableCollection<SelectableItem>();
        public int NumberOfSkillsToChoose
        {
            get { return proficiencyModel != null ? proficiencyModel.NumberOfSkillsToChoose : 0; }
            set
            {
                proficiencyModel.NumberOfSkillsToChoose = value;
                RaisePropertyChanged(nameof(NumberOfSkillsToChoose));
            }
        }

        public void AddSkillAttributeOption(SkillAttributeEnum skillAttributeEnum)
        {
            proficiencyModel.SkillAttributeOptions.Add(skillAttributeEnum);
        }

        public void RemoveSkillAttributeOption(SkillAttributeEnum skillAttributeEnum)
        {
            proficiencyModel.SkillAttributeOptions.Remove(skillAttributeEnum);
        }
    }
}
