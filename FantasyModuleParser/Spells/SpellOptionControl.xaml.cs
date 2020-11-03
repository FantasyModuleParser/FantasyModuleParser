using FantasyModuleParser.Extensions;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Spells.Models;
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
        public event EventHandler OnViewStatUpdate;
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
            if (!IsSpellModelNull())
            {
                if (DurationTime != null)
                    DurationTime.IsEnabled = (DataContext as SpellViewModel).SpellModel.DurationType == Enums.DurationType.Time;
                if (DurationUnit != null)
                    DurationUnit.IsEnabled = (DataContext as SpellViewModel).SpellModel.DurationType == Enums.DurationType.Time;
            }
        }

        private void Casting_Changed(object sender, EventArgs e)
        {
            SpellModel spellModel = (DataContext as SpellViewModel).SpellModel;
            if (spellModel == null)
                return;
            if (ReactionDescription != null && !IsSpellModelNull())
            {
                ReactionDescription.IsEnabled = spellModel.CastingType == Enums.CastingType.Reaction;
                CastingDescriptionLabel.IsEnabled = spellModel.CastingType == Enums.CastingType.Reaction;
            }
            switch (spellModel.CastingType)
            {
                case Enums.CastingType.None:
                case Enums.CastingType.Reaction:
                case Enums.CastingType.Action:
                case Enums.CastingType.BonusAction:
                    CastingTimeTB.IsEnabled = false;
                    spellModel.CastingTime = 1;
                    break;
                default:
                    CastingTimeTB.IsEnabled = true;
                    break;
            }
            if (spellModel.CastingType != Enums.CastingType.None)
                CastingDisplayValue.Text = spellModel.CastingTime + " "
                + spellModel.CastingType.GetDescription()
                + (spellModel.CastingTime > 1 ? "s" : "")
                + (String.IsNullOrWhiteSpace(spellModel.ReactionDescription) ? "" : ", " + spellModel.ReactionDescription);
            else
                CastingDisplayValue.Text = "";
            //if(OnViewStatUpdate != null)
            //    OnViewStatUpdate.Invoke(this, EventArgs.Empty);
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
            ComboBox comboBox = (sender as ComboBox);
            (DataContext as SpellViewModel).SelectedCategoryModel = comboBox.SelectedItem as CategoryModel;
        }
        private void LoadSpellButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (DataContext as SpellViewModel).LoadSpell();
        }
        private void AddToProjectButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (FGCategoryComboBox == null || FGCategoryComboBox.SelectedItem == null)
                (DataContext as SpellViewModel).AddSpellToModule(null);
            else
                (DataContext as SpellViewModel).AddSpellToModule((FGCategoryComboBox.SelectedItem as CategoryModel).Name);
        }
        private void RangeComboBox_SelectionChanged(object sender, EventArgs e)
        {
            SpellModel spellModel = (DataContext as SpellViewModel).SpellModel;
            if (spellModel == null)
                return;
            if (RangeValueTB == null)
                return;

            if (RangeValueTB.IsEnabled = (spellModel.RangeType == Enums.RangeType.Ranged))
                RangeDisplayValue.Text = spellModel.Range + " feet";
            else
                RangeDisplayValue.Text = spellModel.RangeType == Enums.RangeType.None ? "" : spellModel.RangeType.GetDescription();
            RangeDistanceLabel.IsEnabled = RangeValueTB.IsEnabled;
        }
        private void SpellComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (sender as ComboBox);
            (DataContext as SpellViewModel).SpellModel = comboBox.SelectedItem as SpellModel;
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SpellViewModel spellViewModel = DataContext as SpellViewModel;
            CategoryModel categoryModel = FGCategoryComboBox.SelectedItem as CategoryModel;

            // Ignore any button actions if no Spells exist for the selected Category
            if (categoryModel.SpellModels.Count <= 0)
                return;

            int categoryIndex = SpellComboBox.SelectedIndex;
            switch ((sender as Button).Name)
            {
                case nameof(PrevSpellBtn):

                    if (categoryModel != null)
                    {
                        if (categoryIndex <= 0)
                            categoryIndex = categoryModel.SpellModels.Count - 1;
                        else
                            categoryIndex--;

                        // By updating the selected Item here, it will invoke CategorySelectedNPCComboBox_SelectionChanged event 
                        // because the CategorySelectedNPCComboBox selected item has changed
                        SpellComboBox.SelectedItem = categoryModel.SpellModels[categoryIndex];
                    }
                    break;
                case nameof(NextSpellBtn):
                    if (categoryModel != null)
                    {
                        if (categoryIndex == categoryModel.SpellModels.Count - 1)
                            categoryIndex = 0;
                        else
                            categoryIndex++;

                        // By updating the selected Item here, it will invoke CategorySelectedNPCComboBox_SelectionChanged event 
                        // because the CategorySelectedNPCComboBox selected item has changed
                        SpellComboBox.SelectedItem = categoryModel.SpellModels[categoryIndex];
                    }
                    break;
            }
        }
        private void NewSpellButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (DataContext as SpellViewModel).SpellModel = new SpellModel();
        }
        private void SelectCasters_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CastByWindow castByWindow = new CastByWindow();
            castByWindow.Closed += CastByWindow_Closed;
            castByWindow.ShowDialog();
        }
        private void CastByWindow_Closed(object sender, EventArgs e)
        {
            if (sender is CastByWindow)
            {
                CastByWindow castByWindow = (sender as CastByWindow);

                // Now you have access to castByWindow.SpellCharacterClass.SelectedItems
            }
        }
    }
}
