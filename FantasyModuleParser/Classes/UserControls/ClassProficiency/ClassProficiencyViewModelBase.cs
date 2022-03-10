using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.NPC.ViewModel;

namespace FantasyModuleParser.Classes.UserControls.ClassProficiency
{
    public class ClassProficiencyViewModelBase : ViewModelBase
    {
        public ProficiencyModel proficiencyModel { get; set; }

        public ClassProficiencyViewModelBase()
        {
        }

        public ClassProficiencyViewModelBase(ProficiencyModel proficiencyModel)
        {
            this.proficiencyModel = proficiencyModel;
        }
    }
}
