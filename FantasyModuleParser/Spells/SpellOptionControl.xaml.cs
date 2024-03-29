﻿using FantasyModuleParser.Extensions;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC.Views;
using FantasyModuleParser.Spells.Models;
using FantasyModuleParser.Spells.ViewModels;
using log4net;
using Markdig;
using Markdig.Wpf;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xaml;
using XamlReader = System.Windows.Markup.XamlReader;

namespace FantasyModuleParser.Spells
{
    /// <summary>
    /// Interaction logic for Spell_Option.xaml
    /// </summary>
    public partial class SpellOptionControl : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SpellOptionControl));
        private bool _isMarkdownHelpWindowOpen = false;
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
            PreviewKeyDown += SpellOptionControl_PreviewKeyDown;
            log.Debug("Spell Option UC Initialized");
        }
        private void SpellOptionControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.B)
                    btn_bold_Click(sender, e);
                if (e.Key == Key.I)
                    btn_italics_Click(sender, e);
                if (e.Key == Key.U)
                    btn_underline_Click(sender, e);
            }

            // If the return key is pressed AND the 
        }
        private void SaveSpellButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SpellModel spellModel = (DataContext as SpellViewModel).SpellModel;
            log.Info("Saving spell " + spellModel.SpellName + " has started...");

            if (spellModel == null)
            {
                log.Error(nameof(SpellModel) + " data object is null");
                throw new InvalidDataException(nameof(SpellModel) + " data object is null");
            }
            if (spellModel.SpellName == null || spellModel.SpellName.Length == 0)
            {
                log.Error("Spell name is empty!");
                throw new InvalidDataException("Spell name is empty!");
            }
            if (string.IsNullOrEmpty(spellModel.CastBy))
            {
                log.Error("Spell " + spellModel.SpellName + " has a null value for CastBy");
            }
            (DataContext as SpellViewModel).Save();
        }
        private void DurationSelectionEnabled_ComboboxChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DurationTime != null && DurationUnit != null)
            {
                SpellModel spellModel = (DataContext as SpellViewModel).SpellModel;
                // Darkpool : BugFix #34
                // When loading a new module, the Spell tab needs to reload, and it is not guaranteed
                // that a spellModel object exists... Hence a null check
                if(spellModel != null)
                {
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
        private void ComponentDescription_Changed(object sender, EventArgs e)
        {
            SpellModel spellModel = (DataContext as SpellViewModel).SpellModel;
            spellModel.ComponentDescription = SpellViewModel.GenerateComponentDescription(spellModel);

            // Due to some wierd ass quirk, this window needs to bind spellModel.ComponentDescription to an 
            // TextBox on the .xaml in order for the View Stat Block page to update correctly.  My guess is
            // I am missing an event trigger (or invoking) somewhere...
            HiddenComponentDescriptionTB.Text = spellModel.ComponentDescription;
        }
        private void SelfComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SpellModel spellModel = (DataContext as SpellViewModel).SpellModel;
            StringBuilder stringBuilder = new StringBuilder();
            if (spellModel == null)
                return;
            if (RangeValueTB == null)
                return;

            if (RangeTypeCB.SelectedValue.Equals(Enums.RangeType.None) || RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Sight) || RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Special) || RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Touch) || RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Unlimited))
                RangeDisplayValue.Text = spellModel.RangeType.ToString();
            else if (RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Ranged))
                RangeDisplayValue.Text = spellModel.Range + " " + spellModel.Unit.GetDescription();
            else
            {
                if (spellModel.SelfType.Equals(Enums.SelfType.Cone) || spellModel.SelfType.Equals(Enums.SelfType.Cube) || spellModel.SelfType.Equals(Enums.SelfType.Line) || spellModel.SelfType.Equals(Enums.SelfType.Radius))
                    RangeDisplayValue.Text = spellModel.RangeType + " (" + spellModel.Range + "-" + spellModel.Unit.GetDescription() + " " + spellModel.SelfType.GetDescription() + ")";
                else if (spellModel.SelfType.Equals(Enums.SelfType.Sphere))
                    RangeDisplayValue.Text = spellModel.RangeType + " (" + spellModel.Range + "-" + spellModel.Unit.GetDescription() + "-radius " + spellModel.SelfType.GetDescription() + ")";
                else
                    RangeDisplayValue.Text = spellModel.RangeType.ToString();
            }
        }
        private void DistanceTextBox_SelectionChanged(object sender, TextChangedEventArgs e)
        {
            SpellModel spellModel = (DataContext as SpellViewModel).SpellModel;
            StringBuilder stringBuilder = new StringBuilder();
            if (spellModel == null)
                return;
            if (RangeValueTB == null)
                return;

            if (RangeTypeCB.SelectedValue.Equals(Enums.RangeType.None) || RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Sight) || RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Special) || RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Touch) || RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Unlimited))
                RangeDisplayValue.Text = spellModel.RangeType.ToString();
            else if (RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Ranged))
                RangeDisplayValue.Text = spellModel.Range + " " + spellModel.Unit.GetDescription();
            else
            {
                if (spellModel.SelfType.Equals(Enums.SelfType.Cone) || spellModel.SelfType.Equals(Enums.SelfType.Cube) || spellModel.SelfType.Equals(Enums.SelfType.Line) || spellModel.SelfType.Equals(Enums.SelfType.Radius))
                    RangeDisplayValue.Text = spellModel.RangeType + " (" + spellModel.Range + "-" + spellModel.Unit.GetDescription() + " " + spellModel.SelfType.GetDescription() + ")";
                else if (spellModel.SelfType.Equals(Enums.SelfType.Sphere))
                    RangeDisplayValue.Text = spellModel.RangeType + " (" + spellModel.Range + "-" + spellModel.Unit.GetDescription() + "-radius " + spellModel.SelfType.GetDescription() + ")";
                else
                    RangeDisplayValue.Text = spellModel.RangeType.ToString();
            }
        }
        private void UnitValueCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SpellModel spellModel = (DataContext as SpellViewModel).SpellModel;
            StringBuilder stringBuilder = new StringBuilder();
            if (spellModel == null)
                return;
            if (RangeValueTB == null)
                return;

            if (RangeTypeCB.SelectedValue.Equals(Enums.RangeType.None) || RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Sight) || RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Special) || RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Touch) || RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Unlimited))
                RangeDisplayValue.Text = spellModel.RangeType.ToString();
            else if (RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Ranged))
                RangeDisplayValue.Text = spellModel.Range + " " + spellModel.Unit.GetDescription();
            else
            {
                if (spellModel.SelfType.Equals(Enums.SelfType.Cone) || spellModel.SelfType.Equals(Enums.SelfType.Cube) || spellModel.SelfType.Equals(Enums.SelfType.Line) || spellModel.SelfType.Equals(Enums.SelfType.Radius))
                    RangeDisplayValue.Text = spellModel.RangeType + " (" + spellModel.Range + "-" + spellModel.Unit.GetDescription() + " " + spellModel.SelfType.GetDescription() + ")";
                else if (spellModel.SelfType.Equals(Enums.SelfType.Sphere))
                    RangeDisplayValue.Text = spellModel.RangeType + " (" + spellModel.Range + "-" + spellModel.Unit.GetDescription() + "-radius " + spellModel.SelfType.GetDescription() + ")";
                else
                    RangeDisplayValue.Text = spellModel.RangeType.ToString();
            }
        }
        private void RangeTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SpellModel spellModel = (DataContext as SpellViewModel).SpellModel;
            StringBuilder stringBuilder = new StringBuilder();
            if (spellModel == null)
                return;
            if (RangeValueTB == null)
                return;

            if (RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Ranged))
            {
                RangeDistanceLabel.IsEnabled = true;
                RangeValueTB.IsEnabled = true;
                UnitLabel.IsEnabled = true;
                UnitValueCB.IsEnabled = true;
                ShapeLabel.IsEnabled = false;
                SelfTypeCB.IsEnabled = false;
            }
            else if (RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Self))
            {
                RangeDistanceLabel.IsEnabled = true;
                RangeValueTB.IsEnabled = true;
                UnitLabel.IsEnabled = true;
                UnitValueCB.IsEnabled = true;
                ShapeLabel.IsEnabled = true;
                SelfTypeCB.IsEnabled = true;
            }
            else
            {
                RangeDistanceLabel.IsEnabled = false;
                RangeValueTB.IsEnabled = false;
                UnitLabel.IsEnabled = false;
                UnitValueCB.IsEnabled = false;
                ShapeLabel.IsEnabled = false;
                SelfTypeCB.IsEnabled = false;
            }

            if (RangeTypeCB.SelectedValue.Equals(Enums.RangeType.None) || RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Sight) || RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Special) || RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Touch) || RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Unlimited))
                RangeDisplayValue.Text = spellModel.RangeType.ToString();
            else if (RangeTypeCB.SelectedValue.Equals(Enums.RangeType.Ranged))
                RangeDisplayValue.Text = spellModel.Range + " " + spellModel.Unit.GetDescription();
            else
            {
                if (spellModel.SelfType.Equals(Enums.SelfType.Cone) || spellModel.SelfType.Equals(Enums.SelfType.Cube) || spellModel.SelfType.Equals(Enums.SelfType.Line) || spellModel.SelfType.Equals(Enums.SelfType.Radius))
                    RangeDisplayValue.Text = spellModel.RangeType + " (" + spellModel.Range + "-" + spellModel.Unit.GetDescription() + " " + spellModel.SelfType.GetDescription() + ")";
                else if (spellModel.SelfType.Equals(Enums.SelfType.Sphere))
                    RangeDisplayValue.Text = spellModel.RangeType + " (" + spellModel.Range + "-" + spellModel.Unit.GetDescription() + "-radius " + spellModel.SelfType.GetDescription() + ")";
                else
                    RangeDisplayValue.Text = spellModel.RangeType.ToString();
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var markdown = MarkdownTextBox.Text;
            var xaml = Markdig.Wpf.Markdown.ToXaml(markdown, BuildPipeline());
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xaml)))
            {
                using (var reader = new XamlXmlReader(stream, new MyXamlSchemaContext()))
                {
                    if (XamlReader.Load(reader) is FlowDocument document)
                    {
                        MarkdownViewer.Document = document;
                    }
                }
            }
        }
        private void MarkdownHelp_Click(object sender, RoutedEventArgs e)
        {
            if (!_isMarkdownHelpWindowOpen)
            {
                _isMarkdownHelpWindowOpen = true;
                MarkdownHelp markdownHelp = new MarkdownHelp();
                markdownHelp.Closing += MarkdownHelp_Closing;
                markdownHelp.Show();
            }
        }
        private void MarkdownHelp_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _isMarkdownHelpWindowOpen = false;
        }
        private static MarkdownPipeline BuildPipeline()
        {
            return new MarkdownPipelineBuilder()
                .UseSupportedExtensions()
                .Build();
        }
        class MyXamlSchemaContext : XamlSchemaContext
        {
            public override bool TryGetCompatibleXamlNamespace(string xamlNamespace, out string compatibleNamespace)
            {
                if (xamlNamespace.Equals("clr-namespace:Markdig.Wpf", StringComparison.Ordinal))
                {
                    compatibleNamespace = $"clr-namespace:Markdig.Wpf;assembly={Assembly.GetAssembly(typeof(Markdig.Wpf.Styles)).FullName}";
                    return true;
                }
                return base.TryGetCompatibleXamlNamespace(xamlNamespace, out compatibleNamespace);
            }
        }

        private void btn_bold_Click(object sender, RoutedEventArgs e)
        {
            applySelectedTextMarkdownMod("**");
        }

        private void btn_italics_Click(object sender, RoutedEventArgs e)
        {
            applySelectedTextMarkdownMod("*");
        }

        private void btn_underline_Click(object sender, RoutedEventArgs e)
        {
            applySelectedTextMarkdownMod("++");
        }

        private void btn_header_Click(object sender, RoutedEventArgs e)
        {
            applySelectedTextMarkdownMod("# ", " #");
        }

        private void btn_text_Click(object sender, RoutedEventArgs e)
        {
            // I think this is supposed to reset all mods?
        }

        private void btn_chatbox_Click(object sender, RoutedEventArgs e)
        {
            applySelectedTextMarkdownMod("`");
        }

        private void applySelectedTextMarkdownMod(String markdownMod)
        {
            applySelectedTextMarkdownMod(markdownMod, markdownMod);
        }
        private void applySelectedTextMarkdownMod(String markdownMod, String markdownModSuffix)
        {
            String selectedText = MarkdownTextBox.SelectedText;
            if (!String.IsNullOrEmpty(selectedText))
            {
                MarkdownTextBox.SelectedText = markdownMod + selectedText + markdownModSuffix;
            }
        }

        private void btn_bullet_Click(object sender, RoutedEventArgs e)
        {
            applySelectedTextMarkdownMod("* ", "");
        }

        private void btn_ClearText_Click(object sender, RoutedEventArgs e)
        {
            MarkdownTextBox.Text = "";
        }
    }
}
