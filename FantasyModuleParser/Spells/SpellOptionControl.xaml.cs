using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Spells.ViewModels;
using System;
using System.Windows.Controls;

namespace FantasyModuleParser.Spells
{
    /// <summary>
    /// Interaction logic for Spell_Option.xaml
    /// </summary>
    public partial class SpellOptionControl : UserControl
    {
        public event EventHandler OnViewStatBlock;
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
            if(!IsSpellModelNull())
            { 
                if(DurationTime != null)
                    DurationTime.IsEnabled = (DataContext as SpellViewModel).SpellModel.DurationType == Enums.DurationType.Time;
                if (DurationUnit != null)
                    DurationUnit.IsEnabled = (DataContext as SpellViewModel).SpellModel.DurationType == Enums.DurationType.Time;
            }
        }

        private void CastingType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReactionDescription != null && !IsSpellModelNull())
                ReactionDescription.IsEnabled = (DataContext as SpellViewModel).SpellModel.CastingType == Enums.CastingType.Reaction;
        }

        private bool IsSpellModelNull() { return (DataContext as SpellViewModel).SpellModel == null; }

        private void ViewStatBlock_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnViewStatBlock.Invoke(this, EventArgs.Empty);
        }

        private void SpellOptionControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            (DataContext as SpellViewModel).Refresh();
        }

        private void CategoryCB_SelectionChange(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LoadSpellButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (DataContext as SpellViewModel).LoadSpell();
        }

        private void AddToProjectButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(FGCategoryComboBox == null || FGCategoryComboBox.SelectedItem == null)
                (DataContext as SpellViewModel).AddSpellToModule(null);
            else
                (DataContext as SpellViewModel).AddSpellToModule((FGCategoryComboBox.SelectedItem as CategoryModel).Name);
        }
    }
}
