using FantasyModuleParser.Equipment.Models;
using FantasyModuleParser.Equipment.ViewModels;
using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            int selectedItemIdx = TableComboBox.SelectedIndex;
            if (AddToProjectAction != null)
                AddToProjectAction(this, EventArgs.Empty);

            TableComboBox.SelectedIndex = selectedItemIdx;
        }


        public event EventHandler SelectedItemModelChangeAction;
        protected virtual void OnSelectedItemModelChange()
        {
            SelectedItemModelChangeAction?.Invoke(this, EventArgs.Empty);
        }

        private void FGCategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO:  To make this item more generic, need to pass in some variable so that the correct
            // 'ModelBase' object is passed through (e.g. EquipmentModel)

            // For now, default to passing an 'EquipmentModel' object through, as it is the PoC piece.

            if (this.DataContext is EquipmentOptionControlViewModel)
            {
                EquipmentOptionControlViewModel viewModel = this.DataContext as EquipmentOptionControlViewModel;
                SelectedCategoryItemSource = viewModel.SelectedCategoryModel.EquipmentModels;
            }
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            int currentSelectedItemIdx = TableComboBox.SelectedIndex - 1;

            if (currentSelectedItemIdx < 0 || currentSelectedItemIdx == TableComboBox.Items.Count + 1)
                currentSelectedItemIdx = TableComboBox.Items.Count - 1;
            
            TableComboBox.SelectedIndex = currentSelectedItemIdx;

            OnSelectedItemModelChange();
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            int currentSelectedItemIdx = TableComboBox.SelectedIndex;

            if (currentSelectedItemIdx == TableComboBox.Items.Count - 1)
                TableComboBox.SelectedIndex = 0;
            else
                TableComboBox.SelectedIndex++;

            OnSelectedItemModelChange();
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
            set 
            { 
                SetValue(SelectedCategoryModelProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedCategoryItemSourceProperty =
            DependencyProperty.Register("SelectedCategoryItemSource", typeof(IEnumerable), typeof(EquipmentFooterUC));

        public IEnumerable SelectedCategoryItemSource
        {
            get { return (IEnumerable)GetValue(SelectedCategoryItemSourceProperty); }
            set { SetValue(SelectedCategoryItemSourceProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemModelProperty =
    DependencyProperty.Register("SelectedItemModel", typeof(ModelBase), typeof(EquipmentFooterUC));

        public ModelBase SelectedItemModel
        {
            get { return (ModelBase)GetValue(SelectedItemModelProperty); }
            set { SetValue(SelectedItemModelProperty, value); }
        }
        #endregion
    }
}
