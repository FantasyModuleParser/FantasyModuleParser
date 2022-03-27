using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;

namespace FantasyModuleParser.NPC.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private ModuleService _moduleService;
        protected ViewModelBase()
        {
            _moduleService = new ModuleService();
        }

        #region Tab Footer Common Items
        private ModuleModel _moduleModel;
        private CategoryModel _categoryModel;
        public ObservableCollection<CategoryModel> ModuleCategoriesSource
        {
            get { return _moduleModel?.Categories; }
            private set { _moduleModel.Categories = value; }
        }

        public CategoryModel SelectedCategoryModel
        {
            get { return _categoryModel; }
            set { Set(ref _categoryModel, value); }
        }
        #endregion

        public void Refresh()
        {
            _moduleModel = _moduleService.GetModuleModel();
            SelectedCategoryModel = _moduleModel?.Categories.Count > 0 ? _moduleModel?.Categories[0] : null;
            RaisePropertyChanged(nameof(ModuleCategoriesSource));
        }

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
