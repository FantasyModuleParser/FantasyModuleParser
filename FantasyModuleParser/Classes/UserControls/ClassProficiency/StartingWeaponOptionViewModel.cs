using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.NPC.ViewModel;

namespace FantasyModuleParser.Classes.UserControls.ClassProficiency
{
    public class StartingWeaponOptionViewModel : ClassProficiencyViewModelBase
    {
        public StartingWeaponOptionViewModel()
        {
        }

        public StartingWeaponOptionViewModel(ProficiencyModel proficiencyModel) : base(proficiencyModel)
        {
        }

        public string WeaponProficiencies
        {
            get => proficiencyModel != null ? proficiencyModel.WeaponProficiencies : string.Empty;
            set
            {
                proficiencyModel.WeaponProficiencies = value;
                RaisePropertyChanged(nameof(WeaponProficiencies));
            }
        }
    }
}
