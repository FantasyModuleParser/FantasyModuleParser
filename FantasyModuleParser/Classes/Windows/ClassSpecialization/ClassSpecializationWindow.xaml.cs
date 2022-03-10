using System.Windows;

namespace FantasyModuleParser.Classes.Windows.ClassSpecialization
{
    /// <summary>
    /// Interaction logic for ClassSpecializationWindow.xaml
    /// </summary>
    public partial class ClassSpecializationWindow : Window
    {
        public ClassSpecializationWindow()
        {
            InitializeComponent();
        }
        private void Button_CloseWindowAction(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
