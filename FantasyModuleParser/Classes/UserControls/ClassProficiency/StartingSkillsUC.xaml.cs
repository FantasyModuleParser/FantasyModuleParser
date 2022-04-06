using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FantasyModuleParser.Classes.Enums;
using FantasyModuleParser.Classes.Model;
using FantasyModuleParser.Extensions;

namespace FantasyModuleParser.Classes.UserControls.ClassProficiency
{
    /// <summary>
    /// Interaction logic for StartingSkillsUC.xaml
    /// </summary>
    public partial class StartingSkillsUC : UserControl
    {
        public StartingSkillsUC()
        {
            InitializeComponent();
        }

        private void SkillAttributeEnumListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                foreach (SelectableItem enumerationMember in e.AddedItems)
                {
                    SkillAttributeEnum choice;
                    if (SkillAttributeEnum.TryParse(enumerationMember.Name, out choice))
                    {
                        (DataContext as StartingSkillsViewModel).AddSkillAttributeOption(choice);
                    }
                }
            }

            if (e.RemovedItems.Count > 0)
            {
                foreach (SelectableItem enumerationMember in e.RemovedItems)
                {
                    SkillAttributeEnum choice;
                    if (SkillAttributeEnum.TryParse(enumerationMember.Name, out choice))
                    {
                        (DataContext as StartingSkillsViewModel).RemoveSkillAttributeOption(choice);
                    }
                }
            }
        }
    }
}
