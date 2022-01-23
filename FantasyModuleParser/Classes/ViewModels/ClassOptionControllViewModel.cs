using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC.Commands;
using FantasyModuleParser.NPC.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace FantasyModuleParser.Classes.ViewModels
{
    public class ClassOptionControllViewModel : ViewModelBase
    {
        private ClassModel _classModel;
        private ModuleService _moduleService;

        private bool _isAddToProjectActionUsed = false;
        // private ModuleModel _moduleModel;
        // private CategoryModel _categoryModel;

        // Define child ViewModels
        private ClassSpecializationViewModel _classSpecializationViewModel;
        public ClassSpecializationViewModel ClassSpecializationViewModel
        {
            get { return _classSpecializationViewModel; }
            set { SetProperty(ref _classSpecializationViewModel, value); }
        }
        private ClassProficiencyViewModel _classProficiencyViewModel;
        public ClassProficiencyViewModel ClassProficiencyViewModel
        {
            get { return _classProficiencyViewModel; }
            set { SetProperty(ref _classProficiencyViewModel, value); }
        }

        private ClassProficiencyViewModel _classMultiClassProficiencyViewModel;
        public ClassProficiencyViewModel ClassMultiClassProficiencyViewModel
        {
            get { return _classMultiClassProficiencyViewModel; }
            set { SetProperty(ref _classMultiClassProficiencyViewModel, value); }
        }
        private ClassFeatureViewModel _classFeatureViewModel;
        public ClassFeatureViewModel ClassFeatureViewModel
        {
            get { return _classFeatureViewModel; }
            set { SetProperty(ref _classFeatureViewModel, value); }
        }

        private ClassHeaderViewModel _classHeaderViewModel;
        public ClassHeaderViewModel ClassHeaderViewModel
        {
            get { return _classHeaderViewModel; }
            set { SetProperty(ref _classHeaderViewModel, value); }
        }

        private ClassMenuOptionEnum _classMenuOptionEnum = ClassMenuOptionEnum.ClassSpecialization;

        public ClassModel ParentClassModel
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

        public ClassMenuOptionEnum ClassMenuOptionEnum
        {
            get { return _classMenuOptionEnum; }
            set { Set(ref _classMenuOptionEnum, value); }
        }

        #region Proficiency Bonus
        // public ObservableCollection<ProficiencyBonusModel> ProfBonusValues
        // {
        //     get { return this._classModel.ProfBonusValues; }
        // }
        #endregion

        #region Spell Slots 
        // public bool HasSpellSlots
        // {
        //     get { return _classModel.HasSpellSlots; }
        //     set { Set(ref _classModel.HasSpellSlots, value); }
        // }
        public ObservableCollection<SpellSlotModel> SpellSlotValues
        {
            get { return this._classModel.SpellSlotValues; }
        }
        #endregion
        private ModelBase _footerSelectedModel;
        public ModelBase SelectedFooterItemModel
        {
            get { return _footerSelectedModel; }
            set
            {
                if (value is ClassModel && !_isAddToProjectActionUsed)
                {
                    ParentClassModel = (value as ClassModel).ShallowCopy();
                    _resetAllViewModel(ParentClassModel);

                    raiseAllUIProperties();
                }
                _isAddToProjectActionUsed = false;
                Set(ref _footerSelectedModel, value);
            }
        }

        public ClassOptionControllViewModel() : base()
        {
            NewClassModel();
            SelectedFooterItemModel = ParentClassModel;
            this._moduleService = new ModuleService();
            _resetAllViewModel(ParentClassModel);
        }

        private void _resetAllViewModel(ClassModel ParentClassModel)
        {
            ClassSpecializationViewModel = new ClassSpecializationViewModel(ParentClassModel);
            ClassHeaderViewModel = new ClassHeaderViewModel(ParentClassModel);
            ClassProficiencyViewModel = new ClassProficiencyViewModel(ParentClassModel.Proficiency, false);
            ClassMultiClassProficiencyViewModel = new ClassProficiencyViewModel(ParentClassModel.MultiProficiency, true);
            ClassFeatureViewModel = new ClassFeatureViewModel(ParentClassModel);
        }

        public void Save()
        {
            ParentClassModel.Save();
        }
        private void raiseAllUIProperties()
        {
            RaisePropertyChanged(nameof(ParentClassModel));
            // RaisePropertyChanged(nameof(HasSpellSlots));
            RaisePropertyChanged(nameof(Proficiency));
            RaisePropertyChanged(nameof(MultiProficiency));
            RaisePropertyChanged(nameof(ClassMenuOptionEnum));
            RaisePropertyChanged(nameof(ParentClassModel.ClassFeatures));
            RaisePropertyChanged(nameof(ParentClassModel.Description));
            _resetAllViewModel(ParentClassModel);
        }

        public void NewClassModel()
        {
            var tempClassModel = new ClassModel();
            tempClassModel.PrePopulateProficiencyBonusValues();
            tempClassModel.PrePopulateSpellSlotValues();
            tempClassModel.PrePopulateStartingEquipment();
            ParentClassModel = tempClassModel;
            raiseAllUIProperties();
        }

        public void SaveClassModel()
        {
            ParentClassModel.Save();
        }

        public void LoadClassModel(string @filePath)
        {
            ParentClassModel = ParentClassModel.Load(filePath);
            raiseAllUIProperties();
        }

        public void AddClassToCategory()
        {
             _moduleService.AddClassToCategory(ParentClassModel, SelectedCategoryModel.Name);
            RaisePropertyChanged(nameof(ModuleCategoriesSource));
            RaisePropertyChanged(nameof(SelectedCategoryModel));
            _isAddToProjectActionUsed = true;
        }
    }
}
