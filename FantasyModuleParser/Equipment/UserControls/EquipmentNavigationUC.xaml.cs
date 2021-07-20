using FantasyModuleParser.Main.Models;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.Equipment.UserControls
{
    /// <summary>
    /// Interaction logic for EquipmentNavigationUC.xaml
    /// </summary>
    public partial class EquipmentNavigationUC : UserControl
    {
        private ModuleModel _moduleModel;
        public ModuleModel SelectedModuleModel
        {
            get => this._moduleModel;
            set => this._moduleModel = value;
        }

        public string PreviousItemLabel { get; set; }
        public string NextItemLabel { get; set; }
        public EquipmentNavigationUC()
        {
            InitializeComponent();
            ModuleNavigationUC.DataContext = this;
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            OnPrevEquipmentAction();
        }

        public event EventHandler PrevEquipmentAction;
        protected virtual void OnPrevEquipmentAction()
        {
            if (PrevEquipmentAction != null)
                PrevEquipmentAction(this, EventArgs.Empty);
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            OnNextEquipmentAction();
        }

        public event EventHandler NextEquipmentAction;
        protected virtual void OnNextEquipmentAction()
        {
            if (NextEquipmentAction != null)
                NextEquipmentAction(this, EventArgs.Empty);
        }

        private void TableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        //public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(HeaderedComboBox));


        #region Custom Exposed Dependencies
        public static readonly DependencyProperty ModuleCategoryItemSourceProperty =
            DependencyProperty.Register( "ModuleCategoryItemSource", typeof(IEnumerable), typeof(EquipmentNavigationUC));

        public IEnumerable ModuleCategoryItemSource
        {
            get { return (IEnumerable)GetValue(ModuleCategoryItemSourceProperty); }
            set { SetValue(ModuleCategoryItemSourceProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemModelProperty =
            DependencyProperty.Register("SelectedItemModel", typeof(ModelBase), typeof(EquipmentNavigationUC));

        public ModelBase SelectedItemModel
        {
            get { return (ModelBase)GetValue(SelectedItemModelProperty); }
            set { SetValue(SelectedItemModelProperty, value); }
        }
        #endregion

        private void FGCategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO:  To make this item more generic, need to pass in some variable so that the correct
            // 'ModelBase' object is passed through (e.g. EquipmentModel)

            // For now, default to passing an 'EquipmentModel' object through, as it is the PoC piece.

            TableComboBox.ItemsSource = (FGCategoryComboBox.SelectedItem as CategoryModel).EquipmentModels;
        }
    }
}
