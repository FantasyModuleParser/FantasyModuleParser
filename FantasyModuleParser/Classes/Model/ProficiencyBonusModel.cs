
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FantasyModuleParser.Classes.Model
{
    public class ProficiencyBonusModel : INotifyPropertyChanged
    {
        private int _level;
        public int Level
        {
            get { return this._level; }
            set { this._level = value; RaiseProperChanged(); }
        }

        private int _bonus;
        public int Bonus
        {
            get { return this._bonus; }
            set { this._bonus = value; RaiseProperChanged(); }
        }


        public ProficiencyBonusModel(int level)
        {
            Level = level;
        }

        public ProficiencyBonusModel(int level, int bonus) : this(level)
        {
            Bonus = bonus;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaiseProperChanged([CallerMemberName] string caller = "")
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
