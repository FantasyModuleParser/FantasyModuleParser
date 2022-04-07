using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FantasyModuleParser.NPC.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        protected void RaisePropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool Set<T>(ref T backingField, T value, [CallerMemberName] string propertyname = null)
        {
            // Check if the value and backing field are actualy different
            if (EqualityComparer<T>.Default.Equals(backingField, value)) { return false; }

            // Setting the backing field and the RaisePropertyChanged
            backingField = value;
            //OnPropertyChanged(propertyname);
            RaisePropertyChanged(propertyname);
            return true;
        }
    }
}
