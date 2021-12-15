using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.Classes.UserControls
{
    /// <summary>
    /// Interaction logic for ClassSpecializationUC.xaml
    /// </summary>
    public partial class ClassSpecializationUC : UserControl
    {
        public ClassSpecializationUC()
        {
            InitializeComponent();
        }

        private void OnClassSpecializationListBox_SelectionChangedEvent(object sender, SelectionChangedEventArgs e)
        {
            //if (e.AddedItems.Count > 0)
            //{
            //    _selectedClassSpecialization = e.AddedItems[0] as ClassSpecialization;
            //    IsClassSpecialSelected = true;
            //}
            //else
            //{
            //    _selectedClassSpecialization = new ClassSpecialization();
            //    IsClassSpecialSelected = false;
            //}
            //RaisePropertyChanged(nameof(SelectedClassSpecialization));
            //RaisePropertyChanged(nameof(TabDescriptionLabel));
            //RaisePropertyChanged(nameof(IsClassSpecialSelected));
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
                return;


            //if (button.DataContext is ClassSpecialization selectedSpecialization)
            //    ClassModelValue.ClassSpecializations.Remove(selectedSpecialization);

            //RaisePropertyChanged(nameof(ClassModelValue));
            //RaisePropertyChanged(nameof(ClassModelValue.ClassSpecializations));
        }

        private void ClearClassFeatureButton_OnClick(object sender, RoutedEventArgs e)
        {
            //_selectedClassSpecialization = new ClassSpecialization();
            //RaisePropertyChanged(nameof(SelectedClassSpecialization));
        }
    }
}
