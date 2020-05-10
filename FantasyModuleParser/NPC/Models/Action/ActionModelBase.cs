using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyModuleParser.NPC.Models.Action
{
    public class ActionModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
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

    }
}
