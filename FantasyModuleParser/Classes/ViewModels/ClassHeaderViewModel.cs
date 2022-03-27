using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.NPC.ViewModel;

namespace FantasyModuleParser.Classes.ViewModels
{
    public class ClassHeaderViewModel : ViewModelBase
    {
        private ClassModel _classModel;
        public ClassModel ClassModel
        {
            get { return _classModel; }
            set { Set(ref _classModel, value); }
        }

        public string ClassModelName
        {
            get { return _classModel.Name; }
            set {   _classModel.Name = value;
                    RaisePropertyChanged(nameof(ClassModel));
            }
        }
        public bool ClassModelIsLocked
        {
            get { return _classModel.IsLocked; }
            set
            {
                _classModel.IsLocked = value;
                RaisePropertyChanged(nameof(ClassModel));
            }
        }

        public ClassHitDieEnum HitDiePerLevel
        {
            get { return _classModel.HitPointDiePerLevel; }
            set
            {
                _classModel.HitPointDiePerLevel = value;
                RaisePropertyChanged(nameof(ClassModel));
            }
        }

        public ClassHeaderViewModel() : base()
        {
            ClassModel = new ClassModel();
        }

        public ClassHeaderViewModel(ClassModel classModel) : base()
        {
            ClassModel = classModel;
        }
    }
}
