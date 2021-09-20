using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.NPC.ViewModel;
using System.ComponentModel;

namespace FantasyModuleParser.Classes.ViewModels
{
    public class ClassOptionControllViewModel : ViewModelBase
    {
        private ClassMenuOptionEnum _classMenuOptionEnum;

        [DefaultValue(ClassMenuOptionEnum.Proficiencies)]
        public ClassMenuOptionEnum ClassMenuOptionEnum
        {
            get { return _classMenuOptionEnum; }
            set { Set(ref _classMenuOptionEnum, value); }
        }
    }
}
