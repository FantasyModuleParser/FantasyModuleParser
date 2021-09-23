using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.NPC.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FantasyModuleParser.Classes.ViewModels
{
    public class ClassOptionControllViewModel : ViewModelBase
    {
        private ClassModel _classModel;
        private ClassMenuOptionEnum _classMenuOptionEnum;

        [DefaultValue(ClassMenuOptionEnum.Proficiencies)]
        public ClassMenuOptionEnum ClassMenuOptionEnum
        {
            get { return _classMenuOptionEnum; }
            set { Set(ref _classMenuOptionEnum, value); }
        }

        public ObservableCollection<ProficiencyBonusModel> ProfBonusValues
        {
            get { return this._classModel.ProfBonusValues; }
        }

        public ObservableCollection<SpellSlotModel> SpellSlotValues
        {
            get { return this._classModel.SpellSlotValues; }
        }

        public ClassOptionControllViewModel()
        {
            this._classModel = new ClassModel();
            this._classModel.PrePopulateProficiencyBonusValues();
            this._classModel.PrePopulateSpellSlotValues();
        }

        public void Save()
        {

        }
    }
}
