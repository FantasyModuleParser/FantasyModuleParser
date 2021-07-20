using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FantasyModuleParser.Equipment.UserControls
{
    /// <summary>
    /// Interaction logic for EquipmentFooterUC.xaml
    /// </summary>
    public partial class EquipmentFooterUC : UserControl
    {
        public EquipmentFooterUC()
        {
            InitializeComponent();
            ModuleFooterUC.DataContext = this;
        }

        private void NewEquipment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadEquipment_Click(object sender, RoutedEventArgs e)
        {
            OnLoadEquipmentAction();
        }

        public event EventHandler LoadEquipmentAction;
        protected virtual void OnLoadEquipmentAction()
        {
            if (LoadEquipmentAction != null)
                LoadEquipmentAction(this, EventArgs.Empty);
        }

        private void SaveEquipment_Click(object sender, RoutedEventArgs e)
        {
            OnSaveEquipmentAction();
        }

        public event EventHandler SaveEquipmentAction;
        protected virtual void OnSaveEquipmentAction()
        {
            if (SaveEquipmentAction != null)
                SaveEquipmentAction(this, EventArgs.Empty);
        }

        private ICommand _addToProjectCommand;
        public ICommand AddToProjectCommand
        {
            get
            {
                if(_addToProjectCommand == null)
                {
                    _addToProjectCommand = new ActionCommand(param => OnAddToProjectAction(),
                        param => SelectedCategoryModel != null);
                }
                return _addToProjectCommand;
            }
        }

        public event EventHandler AddToProjectAction;
        protected virtual void OnAddToProjectAction()
        {
            if (AddToProjectAction != null)
                AddToProjectAction(this, EventArgs.Empty);
        }

        private void EquipmentNavigationUC_PrevEquipmentAction(object sender, EventArgs e)
        {
            OnPrevEquipmentAction();
        }

        public event EventHandler PrevEquipmentAction;
        protected virtual void OnPrevEquipmentAction()
        {
            if (PrevEquipmentAction != null)
                PrevEquipmentAction(this, EventArgs.Empty);
        }


        private void EquipmentNavigationUC_NextEquipmentAction(object sender, EventArgs e)
        {
            OnNextEquipmentAction();
        }

        public event EventHandler NextEquipmentAction;
        protected virtual void OnNextEquipmentAction()
        {
            if (NextEquipmentAction != null)
                NextEquipmentAction(this, EventArgs.Empty);
        }

        private void FGCategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO:  To make this item more generic, need to pass in some variable so that the correct
            // 'ModelBase' object is passed through (e.g. EquipmentModel)

            // For now, default to passing an 'EquipmentModel' object through, as it is the PoC piece.

            //TableComboBox.ItemsSource = (FGCategoryComboBox.SelectedItem as CategoryModel).EquipmentModels;
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            OnPrevEquipmentAction();
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            OnNextEquipmentAction();
        }

        private void TableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        #region Custom Exposed Dependencies
        public static readonly DependencyProperty ModuleCategoryItemSourceProperty =
            DependencyProperty.Register("ModuleCategoryItemSource", typeof(IEnumerable), typeof(EquipmentFooterUC));

        public IEnumerable ModuleCategoryItemSource
        {
            get { return (IEnumerable)GetValue(ModuleCategoryItemSourceProperty); }
            set { SetValue(ModuleCategoryItemSourceProperty, value); }
        }

        public static readonly DependencyProperty SelectedCategoryModelProperty =
            DependencyProperty.Register("SelectedCategoryModel", typeof(CategoryModel), typeof(EquipmentFooterUC));

        public CategoryModel SelectedCategoryModel
        {
            get { return (CategoryModel)GetValue(SelectedCategoryModelProperty); }
            set { SetValue(SelectedCategoryModelProperty, value); }
        }
        #endregion
    }
}
