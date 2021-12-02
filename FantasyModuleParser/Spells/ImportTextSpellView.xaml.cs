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
            IImportSpell importSpell = null;
            switch (ImportSelector.SelectedIndex)
            {
                case 0:
                    importSpell = new ImportSpellPDF();
                    //(DataContext as SpellViewModel).SpellModel = new ImportSpellBase().ImportTextToSpellModel(ImportTextBox.Text);
                    break;
                case 1:
                    importSpell = new ImportSpellDnDBeyond();
                    //(DataContext as SpellViewModel).SpellModel = new ImportSpellBase().ImportDnDTextToSpellModel(ImportTextBox.Text);
                    break;
            }

            (DataContext as SpellViewModel).SpellModel = importSpell.ImportTextToSpellModel(ImportTextBox.Text);
            this.Visibility = Visibility.Hidden;
        }
    }
}
