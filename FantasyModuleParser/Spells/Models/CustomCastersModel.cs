using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FantasyModuleParser.Spells.Models
{
    public class CustomCastersModel : INotifyPropertyChanged
    {
        private bool _selected;
        public bool Selected { get { return _selected; } set { Set(ref _selected, value); } }
        public string Caster { get; set; }

        public CustomCastersModel()
        {
        }

        public CustomCastersModel(string caster)
        {
            Caster = caster;
            Selected = false;
        }

        public CustomCastersModel(bool selected, string caster)
        {
            Selected = selected;
            Caster = caster;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool Set<T>(ref T backingField, T value, [CallerMemberName] string propertyname = null)
        {
            // Check if the value and backing field are actualy different
            if (EqualityComparer<T>.Default.Equals(backingField, value))
            {
                return false;
            }

            // Setting the backing field and the RaisePropertyChanged
            backingField = value;
            OnPropertyChanged(propertyname);
            return true;
        }
    }
}
