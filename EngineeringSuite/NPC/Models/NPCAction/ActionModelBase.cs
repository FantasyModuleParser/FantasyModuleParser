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

        public ActionModelBase()
        {
        }

        public ActionModelBase(string actionName, string actionDescription)
        {
            ActionName = actionName;
            ActionDescription = actionDescription;
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
