using System.ComponentModel;
using System.Text.RegularExpressions;
using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FantasyModuleParser.Classes.UserControls.ClassProficiency
{
    /// <summary>
    /// Interaction logic for StartingToolsUC.xaml
    /// </summary>
    public partial class StartingToolsUC : UserControl
    {
        public StartingToolsUC()
        {
            InitializeComponent();
        }

        private void StartingToolsListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                foreach (SelectableItem enumerationMember in e.AddedItems)
                {
                    ClassStartingToolEnum choice;
                    if (ClassStartingToolEnum.TryParse(enumerationMember.Name, out choice))
                    {
                        (DataContext as StartingToolsViewModel).AddStartingToolOption(choice);
                    }
                }
            }

            if (e.RemovedItems.Count > 0)
            {
                foreach (SelectableItem enumerationMember in e.RemovedItems)
                {
                    ClassStartingToolEnum choice;
                    if (ClassStartingToolEnum.TryParse(enumerationMember.Name, out choice))
                    {
                        (DataContext as StartingToolsViewModel).RemoveStartingToolOption(choice);
                    }
                }
            }
        }
        
        private void PositiveNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text) || e.Text.Contains("-");
        }
    }
}
