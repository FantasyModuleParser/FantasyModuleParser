using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite.NPC.Models.NPCAction
{
    public class ActionModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T newValue, string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }

        private int _actionId;
        private string _actionName, _actionDescription;
        public string ActionName
        {
            get { return _actionName; }
            set
            {
                _actionName = value;
                RaisePropertyChanged("ActionName");
            }
        }
        public string ActionDescription
        {
            get { return _actionDescription; }
            set
            {
                _actionDescription = value;
                RaisePropertyChanged("ActionDescription");
            }
        }

        public int ActionID
        {
            get { return _actionId; }
            set
            {
                _actionId = value;
                RaisePropertyChanged("ActionID");
            }
        }

        public ActionModelBase()
        {
            _actionId = 0;
        }

        public ActionModelBase(string actionName, string actionDescription)
        {
            ActionName = actionName;
            ActionDescription = actionDescription;
        }

        public ActionModelBase(int actionId, string actionName, string actionDescription)
        {
            _actionId = actionId;
            ActionName = actionName;
            ActionDescription = actionDescription;
        }


        // Intended to work with an IComparable function so that the collection is
        // sorted based on the _actionId value
        public void raisePriority()
        {
            this._actionId++;
        }

        public void lowerPriority()
        {
            this._actionId--;
        }


        #region Equals and HashCode
        // Only the ActionName will act as the identifier of this object
        public override bool Equals(object obj)
        {
            return obj is ActionModelBase @base &&
                   _actionName == @base._actionName;
        }

        public override int GetHashCode()
        {
            return 736796887 + EqualityComparer<string>.Default.GetHashCode(_actionName);
        }
        #endregion

    }
}
