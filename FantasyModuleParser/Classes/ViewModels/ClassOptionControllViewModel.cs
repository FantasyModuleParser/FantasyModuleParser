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
        // private ModuleModel _moduleModel;
        // private CategoryModel _categoryModel;

        private ClassMenuOptionEnum _classMenuOptionEnum = ClassMenuOptionEnum.ClassFeatures;

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

        public string StartingEquipment
        {
            get { return _classModel.StartingEquipment; }
            set { Set(ref _classModel.StartingEquipment, value); }
        }



        private ModelBase _footerSelectedModel;
        public ModelBase SelectedFooterItemModel
        {
            get { return _footerSelectedModel; }
            set
            {
                if (value is ClassModel)
                {
                    ParentClassModel = (value as ClassModel).ShallowCopy();
                    raiseAllUIProperties();
                }
                Set(ref _footerSelectedModel, value);
            }
        }

        private ClassFeature _selectedClassFeature;
        public ClassFeature SelectedClassFeature
        {
            get { return this._selectedClassFeature; }
            set { Set(ref _selectedClassFeature, value); RaisePropertyChanged(nameof(SelectedClassFeatureDescription)); }
        }

        public ClassOptionControllViewModel() : base()
        {
            NewClassModel();
            SelectedFooterItemModel = ParentClassModel;
            SelectedClassFeature = new ClassFeature();
            this._moduleService = new ModuleService();
        }

        public void Save()
        {

        }
        private void raiseAllUIProperties()
        {
            RaisePropertyChanged(nameof(ParentClassModel));
            // RaisePropertyChanged(nameof(HasSpellSlots));
            RaisePropertyChanged(nameof(StartingEquipment));
            RaisePropertyChanged(nameof(Proficiency));
            RaisePropertyChanged(nameof(MultiProficiency));
            RaisePropertyChanged(nameof(ClassMenuOptionEnum));
            RaisePropertyChanged(nameof(ParentClassModel.ClassFeatures));
            RaisePropertyChanged(nameof(SelectedClassFeature));

            
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
        }

        #region Class Features
        private ICommand _addClassFeatureCommand;
        public ICommand AddClassFeatureCommand
        {
            get
            {
                if (_addClassFeatureCommand == null)
                {
                    _addClassFeatureCommand = new ActionCommand(param => OnAddClassFeatureAction(),
                        //param => !String.IsNullOrWhiteSpace(SelectedClassFeature?.Name));
                        param => OnAddClassFeatureEnableAction(param as String));
                }
                return _addClassFeatureCommand;
            }
        }

        protected virtual void OnAddClassFeatureAction()
        {
            if(ParentClassModel.ClassFeatures == null)
            {
                ParentClassModel.ClassFeatures = new ObservableCollection<ClassFeature>();
            }
            ParentClassModel.ClassFeatures.Add(SelectedClassFeature.ShallowCopy());
            RaisePropertyChanged(nameof(ParentClassModel));
        }

        protected virtual bool OnAddClassFeatureEnableAction(string param)
        {
            return !String.IsNullOrWhiteSpace(param);
        }

        private ICommand _removeClassFeatureCommand;
        public ICommand RemoveClassFeatureCommand
        {
            get
            {
                if (_removeClassFeatureCommand == null)
                {
                    _removeClassFeatureCommand = new ActionCommand(
                        param => OnRemoveClassFeature(param as ClassFeature));
                }
                return _removeClassFeatureCommand;
            }
        }

        protected virtual void OnRemoveClassFeature(ClassFeature classFeature)
        {
            if (classFeature == null)
                return;

            ParentClassModel.ClassFeatures.Remove(classFeature);
        }

        private ICommand _clearSelectedFeatureCommand;
        public ICommand ClearSelectedClassFeatureCommand
        {
            get
            {
                if (_clearSelectedFeatureCommand == null)
                {
                    _clearSelectedFeatureCommand = new ActionCommand(
                        param => OnClearSelectedClassFeatureCommand());
                }
                return _clearSelectedFeatureCommand;
            }
        }

        protected virtual void OnClearSelectedClassFeatureCommand()
        {
            SelectedClassFeature = new ClassFeature();
        }

        public string SelectedClassFeatureDescription
        {
            get { return SelectedClassFeature?.Name + " - Description"; }
        }
        #endregion
    }
}
