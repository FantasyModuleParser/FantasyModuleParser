using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FantasyModuleParser.Classes.Model
{
    public class SpellSlotModel : INotifyPropertyChanged
    {
        private int _level;
        public int Level
        {
            get { return _level; }
        }

        private int _cantrips;
        public int Cantrips
        {
            get { return _cantrips; }
            set { this._cantrips = value; RaisePropertyChanged(); }
        }

        private int _firstLevel;
        public int FirstLevel
        {
            get { return _firstLevel; }
            set { this._firstLevel = value; RaisePropertyChanged(); }
        }

        private int _secondLevel;
        public int SecondLevel
        {
            get { return _secondLevel; }
            set { this._secondLevel = value; RaisePropertyChanged(); }
        }

        private int _thirdLevel;
        public int ThirdLevel
        {
            get { return _thirdLevel; }
            set { this._thirdLevel = value; RaisePropertyChanged(); }
        }

        private int _fourthLevel;
        public int FourthLevel
        {
            get { return _fourthLevel; }
            set { this._fourthLevel = value; RaisePropertyChanged(); }
        }

        private int _fifthLevel;
        public int FifthLevel
        {
            get { return _fifthLevel; }
            set { this._fifthLevel = value; RaisePropertyChanged(); }
        }

        private int _sixthLevel;
        public int SixthLevel
        {
            get { return _sixthLevel; }
            set { this._sixthLevel = value; RaisePropertyChanged(); }
        }

        private int _seventhLevel;
        public int SeventhLevel
        {
            get { return _seventhLevel; }
            set { this._seventhLevel = value; RaisePropertyChanged(); }
        }

        private int _eighthLevel;
        public int EighthLevel
        {
            get { return _eighthLevel; }
            set { this._eighthLevel = value; RaisePropertyChanged(); }
        }

        private int _ninthLevel;
        public int NinthLevel
        {
            get { return _ninthLevel; }
            set { this._ninthLevel = value; RaisePropertyChanged(); }
        }

        public SpellSlotModel(int level)
        {
            _level = level;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
