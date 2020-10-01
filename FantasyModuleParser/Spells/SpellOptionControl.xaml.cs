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
            SpellViewModel spellViewModel = DataContext as SpellViewModel;

            ReactionDescription.IsEnabled = spellViewModel.SpellModel.CastingType == Enums.CastingType.Reaction;
            DurationTime.IsEnabled = spellViewModel.SpellModel.DurationType == Enums.DurationType.Time;
            DurationUnit.IsEnabled = spellViewModel.SpellModel.DurationType == Enums.DurationType.Time;
        }

        private void SaveSpellButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (DataContext as SpellViewModel).Save();
        }

        private void DurationSelectionEnabled_ComboboxChanged(object sender, SelectionChangedEventArgs e)
        {
            if(DurationTime != null)
                DurationTime.IsEnabled = (DataContext as SpellViewModel).SpellModel.DurationType == Enums.DurationType.Time;
            if (DurationUnit != null)
                DurationUnit.IsEnabled = (DataContext as SpellViewModel).SpellModel.DurationType == Enums.DurationType.Time;
        }

        private void CastingType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReactionDescription != null)
                ReactionDescription.IsEnabled = (DataContext as SpellViewModel).SpellModel.CastingType == Enums.CastingType.Reaction;
        }
    }
}
