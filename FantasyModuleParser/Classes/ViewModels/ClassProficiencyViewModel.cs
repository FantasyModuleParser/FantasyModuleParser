using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.NPC.Commands;
using FantasyModuleParser.NPC.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace FantasyModuleParser.Classes.ViewModels
{
    public class ClassProficiencyViewModel : ViewModelBase
    {
        public ProficiencyModel proficiencyModel { get; set; }
        public bool IsMultiClassSelected { get; set; }
        public Visibility IsMultiProficiencyOptionValue { get; set; }

        public ClassProficiencyViewModel() :base()
        {
        }

        public ClassProficiencyViewModel(ProficiencyModel proficiencyModel, bool isMultiClassSelected) :base()
        {
            this.proficiencyModel = proficiencyModel;
            this.IsMultiClassSelected = isMultiClassSelected;
            if(this.IsMultiClassSelected)
                IsMultiProficiencyOptionValue = Visibility.Hidden;
            else
                IsMultiProficiencyOptionValue = Visibility.Visible;
        }

        public bool IsStrengthSavingThrowChecked
        {
            get { return validateIfSavingThrowAttributeChecked(SavingThrowAttributeEnum.Strength);}
            set { }
        }
        public bool IsDexeritySavingThrowChecked
        {
            get { return validateIfSavingThrowAttributeChecked(SavingThrowAttributeEnum.Dexerity); }
            set { }
        }
        public bool IsConstitutionSavingThrowChecked
        {
            get { return validateIfSavingThrowAttributeChecked(SavingThrowAttributeEnum.Constitution); }
            set { }
        }
        public bool IsIntelligenceSavingThrowChecked
        {
            get { return validateIfSavingThrowAttributeChecked(SavingThrowAttributeEnum.Intelligence); }
            set { }
        }
        public bool IsWisdomSavingThrowChecked
        {
            get { return validateIfSavingThrowAttributeChecked(SavingThrowAttributeEnum.Wisdom); }
            set { }
        }
        public bool IsCharismaSavingThrowChecked
        {
            get { return validateIfSavingThrowAttributeChecked(SavingThrowAttributeEnum.Charisma); }
            set { }
        }

        private bool validateIfSavingThrowAttributeChecked (SavingThrowAttributeEnum savingThrowAttributeEnum)
        {
            if (proficiencyModel == null) return false;
            if (proficiencyModel.SavingThrowAttributes == null || proficiencyModel.SavingThrowAttributes.Count <= 0) return false;
            return proficiencyModel.SavingThrowAttributes.Contains(savingThrowAttributeEnum);
        }

        private ICommand _savingThrowSelectCommand;
        public ICommand SavingThrowSelectCommand
        {
            get
            {
                if (_savingThrowSelectCommand == null)
                {
                    _savingThrowSelectCommand = new ActionCommand(param => OnSavingThrowSelectAction(param),
                        param => IsSavingThrowCheckboxEnabled(param));
                }
                return _savingThrowSelectCommand;
            }
        }

        private bool IsSavingThrowCheckboxEnabled(object param)
        {
            if (param == null)
                return false;
            if (!(param is SavingThrowAttributeEnum))
                return false;
            SavingThrowAttributeEnum savingThrow = (SavingThrowAttributeEnum)param;
            if (proficiencyModel?.SavingThrowAttributes?.Count >= 2)
            {
                return proficiencyModel.SavingThrowAttributes.Contains(savingThrow);
            }

            return true;
        }

        protected virtual void OnSavingThrowSelectAction(object param)
        {
            SavingThrowAttributeEnum savingThrow = (SavingThrowAttributeEnum)param;

            if (proficiencyModel.SavingThrowAttributes == null)
                proficiencyModel.SavingThrowAttributes = new System.Collections.ObjectModel.ObservableCollection<SavingThrowAttributeEnum>();

            if (proficiencyModel.SavingThrowAttributes.Contains(savingThrow))
                proficiencyModel.SavingThrowAttributes.Remove(savingThrow);
            else
                proficiencyModel.SavingThrowAttributes.Add(savingThrow);

            RaisePropertyChanged(nameof(IsStrengthSavingThrowChecked));
            RaisePropertyChanged(nameof(IsDexeritySavingThrowChecked));
            RaisePropertyChanged(nameof(IsConstitutionSavingThrowChecked));
            RaisePropertyChanged(nameof(IsIntelligenceSavingThrowChecked));
            RaisePropertyChanged(nameof(IsWisdomSavingThrowChecked));
            RaisePropertyChanged(nameof(IsCharismaSavingThrowChecked));
        }
    }
}
