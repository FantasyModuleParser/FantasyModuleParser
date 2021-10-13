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

        public ClassModel ClassModelValue
        {
            get { return _classModel; }
            set { Set(ref _classModel, value); }
        }

        [DefaultValue(ClassMenuOptionEnum.Proficiencies)]
        public ClassMenuOptionEnum ClassMenuOptionEnum
        {
            get { return _classMenuOptionEnum; }
            set { Set(ref _classMenuOptionEnum, value); }
        }

        #region Proficiency Bonus
        public ObservableCollection<ProficiencyBonusModel> ProfBonusValues
        {
            get { return this._classModel.ProfBonusValues; }
        }
        #endregion

        #region Spell Slots 
        public bool HasSpellSlots
        {
            get { return _classModel.HasSpellSlots; }
            set { Set(ref _classModel.HasSpellSlots, value); }
        }
        public ObservableCollection<SpellSlotModel> SpellSlotValues
        {
            get { return this._classModel.SpellSlotValues; }
        }
        #endregion

        public string StartingEquipment
        {
            get { return _classModel.StartingEquipment; }
            set { Set(ref _classModel.StartingEquipment, value); }
        }



        public ClassOptionControllViewModel()
        {
            this._classModel = new ClassModel();
            this._classModel.PrePopulateProficiencyBonusValues();
            this._classModel.PrePopulateSpellSlotValues();
            this._classModel.PrePopulateStartingEquipment();
        }

        public void Save()
        {

        }
    }
}
