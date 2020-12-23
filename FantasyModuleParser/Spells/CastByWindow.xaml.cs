using System.Windows;

namespace FantasyModuleParser.Spells
{
    /// <summary>
    /// Interaction logic for CastByWindow.xaml
    /// </summary>
    public partial class CastByWindow : Window
    {
        public CastByWindow()
        {
            InitializeComponent();
        }
        public CastByWindow(string classValues)
        {
            InitializeComponent();
            if(classValues != null)
            {
                foreach (string classValue in classValues.Split(new string[] { ", " }, System.StringSplitOptions.RemoveEmptyEntries))
                {
                    SpellCharacterClass.SelectedItems.Add(classValue);
                    SpellArcaneArchetypes.SelectedItems.Add(classValue);
                    SpellDivineArchetypes.SelectedItems.Add(classValue);
                    SpellOtherArchetypes.SelectedItems.Add(classValue);
                    CustomCastersClass.SelectedItems.Add(classValue);
                }
            }
        }
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
