using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace FantasyModuleParser.NPC.Models.Skills
{
    public class LanguageModel : INotifyPropertyChanged
    {
        private bool _selected;
        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Selected { get { return _selected; } set { Set(ref _selected, value); } }
        public string Language { get; set; }

        public LanguageModel()
        {
        }
        public LanguageModel(string language)
        {
            Language = language;
            Selected = false;
        }
        public LanguageModel(bool selected, string language)
        {
            Selected = selected;
            Language = language;
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