using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.NPC.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using FantasyModuleParser.Main.Services;

namespace FantasyModuleParser.Classes.ViewModels
{
    public class ClassOptionControllViewModel : ViewModelBase
    {
        private ClassModel _classModel;
        private ModuleService _moduleService;
        private ClassMenuOptionEnum _classMenuOptionEnum;

        public ClassModel ClassModelValue
        {
            get { return _classModel; }
            set { Set(ref _classModel, value); }
        }

        public ProficiencyModel Proficiency
        {
            get { return _classModel.Proficiency; }
            set { Set(ref _classModel.Proficiency, value); }
        }
        public ProficiencyModel MultiProficiency
        {
            get { return _classModel.MultiProficiency; }
            set { Set(ref _classModel.MultiProficiency, value); }
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



        public ClassOptionControllViewModel() : base()
        {
            this._classModel = new ClassModel();
            this._moduleService = new ModuleService();

        }

        public void Save()
        {

        }
        private void raiseAllUIProperties()
        {
            RaisePropertyChanged(nameof(ClassModelValue));
            RaisePropertyChanged(nameof(HasSpellSlots));
            RaisePropertyChanged(nameof(StartingEquipment));
            RaisePropertyChanged(nameof(Proficiency));
            RaisePropertyChanged(nameof(MultiProficiency));
            RaisePropertyChanged(nameof(ClassMenuOptionEnum));
        }

        public void NewClassModel()
        {
            ClassModelValue = new ClassModel();
            raiseAllUIProperties();
        }

        public void SaveClassModel()
        {
            ClassModelValue.Save();
        }

        public void LoadClassModel(string @filePath)
        {
            ClassModelValue = ClassModelValue.Load(filePath);

            raiseAllUIProperties();
        }

        public void AddClassToCategory()
        {
            _moduleService.AddClassToCategory(ClassModelValue, SelectedCategoryModel.Name);
            RaisePropertyChanged(nameof(ModuleCategoriesSource));
            RaisePropertyChanged(nameof(SelectedCategoryModel));
        }
    }
}
