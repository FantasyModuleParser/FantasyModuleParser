using FantasyModuleParser.Spells.ViewModels;
using System.Windows.Controls;

namespace FantasyModuleParser.Spells
{
    /// <summary>
    /// Interaction logic for Spell_Option.xaml
    /// </summary>
    public partial class SpellOptionControl : UserControl
    {
        public SpellOptionControl()
        {
            InitializeComponent();
        }

        private void SaveSpellButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (DataContext as SpellViewModel).Save();
        }
    }
}
