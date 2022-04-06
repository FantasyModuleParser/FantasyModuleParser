using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Classes.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.Classes.UserControls.ClassProficiency
{
    public class StartingToolsViewModel : ClassProficiencyViewModelBase
    {
        public StartingToolsViewModel()
        {
        }

        public StartingToolsViewModel(ProficiencyModel proficiencyModel) : base(proficiencyModel)
        {
            if (proficiencyModel.ClassStartingToolOptions == null)
                proficiencyModel.ClassStartingToolOptions = new HashSet<ClassStartingToolEnum>();

            var classStartingToolOptions = (ClassStartingToolEnum[])Enum.GetValues(typeof(ClassStartingToolEnum));
            foreach (var classStartingToolOption in classStartingToolOptions)
            {
                SelectableItem item = new SelectableItem()
                {
                    Name = classStartingToolOption.ToString(),
                    Description = GetDescription(typeof(ClassStartingToolEnum), classStartingToolOption),
                    IsSelected = false
                };

                if (proficiencyModel.ClassStartingToolOptions.Contains(classStartingToolOption))
                {
                    item.IsSelected = true;
                }

                StartingToolsList.Add(item);
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

        public ObservableCollection<SelectableItem> StartingToolsList { get; set; } = new ObservableCollection<SelectableItem>();

        public int NumberOfGamingSets
        {
            get => proficiencyModel != null ? proficiencyModel.NumberOfGamingSets : 0;
            set
            {
                proficiencyModel.NumberOfGamingSets = value;
                RaisePropertyChanged(nameof(NumberOfGamingSets));
            }
        }
        public int NumberOfMusicalInstruments
        {
            get => proficiencyModel != null ? proficiencyModel.NumberOfMusicalInstruments : 0;
            set
            {
                proficiencyModel.NumberOfMusicalInstruments = value;
                RaisePropertyChanged(nameof(NumberOfMusicalInstruments));
            }
        }

        public void AddStartingToolOption(ClassStartingToolEnum classStartingToolEnum)
        {
            proficiencyModel.ClassStartingToolOptions.Add(classStartingToolEnum);
        }
        public void RemoveStartingToolOption(ClassStartingToolEnum classStartingToolEnum)
        {
            proficiencyModel.ClassStartingToolOptions.Remove(classStartingToolEnum);
        }
    }
}
