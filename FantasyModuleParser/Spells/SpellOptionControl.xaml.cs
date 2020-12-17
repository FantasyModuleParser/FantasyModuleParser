using FantasyModuleParser.Extensions;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Spells.Models;
using FantasyModuleParser.Spells.ViewModels;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FantasyModuleParser.Spells
{
    /// <summary>
    /// Interaction logic for Spell_Option.xaml
    /// </summary>
    public partial class SpellOptionControl : UserControl
    {
        public event EventHandler OnViewStatBlock;
        private ImportTextSpellView importTextSpellView;
        public SpellOptionControl()
        {
            InitializeComponent();
            SpellViewModel spellViewModel = DataContext as SpellViewModel;
            ReactionDescription.IsEnabled = spellViewModel.SpellModel.CastingType == Enums.CastingType.Reaction;
            DurationTime.IsEnabled = spellViewModel.SpellModel.DurationType == Enums.DurationType.Time;
            DurationUnit.IsEnabled = spellViewModel.SpellModel.DurationType == Enums.DurationType.Time;
            importTextSpellView = new ImportTextSpellView();
            importTextSpellView.DataContext = this.DataContext;
        }
        private void SaveSpellButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (DataContext as SpellViewModel).Save();
        }
        private void DurationSelectionEnabled_ComboboxChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DurationTime != null && DurationUnit != null)
            {
                SpellModel spellModel = (DataContext as SpellViewModel).SpellModel;
                switch (spellModel.DurationType)
                {
                    case Enums.DurationType.Time:
                    case Enums.DurationType.Concentration:
                        DurationTime.IsEnabled = true;
                        DurationTime_TextChanged();
                        break;
                    case Enums.DurationType.Instantaneous:
                    case Enums.DurationType.UntilDispelled:
                    case Enums.DurationType.UntilDispelledOrTriggered:
                        spellModel.DurationText = spellModel.DurationType.GetDescription();
                        DurationText.Text = spellModel.DurationText;
                        DurationTime.IsEnabled = false;
                        break;
                    default:
                        DurationTime.IsEnabled = false;
                        break;
                }
                DurationUnit.IsEnabled = DurationTime.IsEnabled;
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

            spellModel.RangeDescription = RangeDisplayValue.Text;

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
            CastByWindow castByWindow = new CastByWindow((DataContext as SpellViewModel).SpellModel.CastBy);
            castByWindow.Closed += CastByWindow_Closed;
            castByWindow.ShowDialog();
        }
        private void CastByWindow_Closed(object sender, EventArgs e)
        {
            if (sender is CastByWindow)
            {
                StringBuilder stringBuilder = new StringBuilder();
                CastByWindow castByWindow = (sender as CastByWindow);

                // Now you have access to castByWindow.SpellCharacterClass.SelectedItems
                foreach(string className in castByWindow.SpellCharacterClass.SelectedItems)
                    stringBuilder.Append(className).Append(", ");
                foreach (string className in castByWindow.SpellDivineArchetypes.SelectedItems)
                    stringBuilder.Append(className).Append(", ");
                foreach (string className in castByWindow.SpellArcaneArchetypes.SelectedItems)
                    stringBuilder.Append(className).Append(", ");
                foreach (string className in castByWindow.SpellOtherArchetypes.SelectedItems)
                    stringBuilder.Append(className).Append(", ");
                foreach (string className in castByWindow.CustomCastersClass.SelectedItems)
                    stringBuilder.Append(className).Append(", ");


                // Remove the last comma character
                if (stringBuilder.Length > 2)
                    stringBuilder.Remove(stringBuilder.Length - 2, 2);

                SpellModel spellModel = (DataContext as SpellViewModel).SpellModel;
                spellModel.CastBy = stringBuilder.ToString();
                CastByTB.Text = spellModel.CastBy;
            }
        }

        private void OpenImportSpellView_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            importTextSpellView.ShowDialog();
        }

        private void DurationTime_TextChanged(object sender, RoutedEventArgs e)
        {
            DurationTime_TextChanged();
        }

        private void DurationTime_TextChanged()
        {
            int numDuration;
            if (int.TryParse(DurationTime.Text, out numDuration))
            {
                SpellModel spellModel = (DataContext as SpellViewModel).SpellModel;
                StringBuilder stringBuilder = new StringBuilder();
                if (spellModel.DurationType == Enums.DurationType.Time)
                {
                    stringBuilder.Append(numDuration + " " + spellModel.DurationUnit.GetDescription());
                    if (numDuration > 1)
                        stringBuilder.Append("s");
                }
                if (spellModel.DurationType == Enums.DurationType.Concentration)
                {
                    stringBuilder.Append("Concentration, up to " + numDuration + " " + spellModel.DurationUnit.GetDescription());
                    if (numDuration > 1)
                        stringBuilder.Append("s");
                }

                spellModel.DurationText = stringBuilder.ToString();
                DurationText.Text = spellModel.DurationText;
            }
        }
    }
}
