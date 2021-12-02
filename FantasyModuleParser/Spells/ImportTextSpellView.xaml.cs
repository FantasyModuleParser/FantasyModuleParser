using FantasyModuleParser.Importer.Spells;
using FantasyModuleParser.Spells.ViewModels;
using System.Windows;

namespace FantasyModuleParser.Spells
{
    /// <summary>
    /// Interaction logic for ImportTextSpellUC.xaml
    /// </summary>
    public partial class ImportTextSpellView : Window
    {

        public ImportTextSpellView()
        {
            InitializeComponent();
        }

        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void ImportTextAndReturnButton_Click(object sender, RoutedEventArgs e)
        {
            IImportSpell importSpell = new ImportSpellBase();
            switch (ImportSelector.SelectedIndex)
            {
                case 0:
                    (DataContext as SpellViewModel).SpellModel = new ImportSpellBase().ImportTextToSpellModel(ImportTextBox.Text);
                    break;
                case 1:
                    (DataContext as SpellViewModel).SpellModel = new ImportSpellBase().ImportDnDTextToSpellModel(ImportTextBox.Text);
                    break;
            }
            this.Visibility = Visibility.Hidden;
        }
    }
}
