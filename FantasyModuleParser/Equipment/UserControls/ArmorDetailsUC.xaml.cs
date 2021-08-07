using FantasyModuleParser.Equipment.Enums;
using FantasyModuleParser.Equipment.UserControls.Models;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FantasyModuleParser.Equipment.UserControls
{
    /// <summary>
    /// Interaction logic for ArmorDetailsUC.xaml
    /// </summary>
    public partial class ArmorDetailsUC : UserControl
    {


        #region Selected Armor Enum Option
        public int selectedArmorEnum
        {
            get { 
                return (int)GetValue(ArmorTypeEnumProperty);
            }
            set { 
                SetValue(ArmorTypeEnumProperty, value); 
            }
        }
        #endregion Selected Armor Enum Option
        public ArmorDetailsUC()
        {
            InitializeComponent();
            ArmorUCLayout.DataContext = this;
        }

        public void Refresh()
        {
            if(ArmorDetailsUC_ArmorTypeEnum == ArmorEnum.Shield)
            {
                //IsStealthDisadvantagedCB.Visibility = Visibility.Hidden;
            }
        }

        private void PositiveNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text) || e.Text.Contains("-");
        }

        public static readonly DependencyProperty ArmorModelProperty =
            DependencyProperty.Register("ArmorModelValue", typeof(ArmorModel), typeof(ArmorDetailsUC));

        public ArmorModel ArmorModelValue
        {
            get { return (ArmorModel)GetValue(ArmorModelProperty); }
            set { SetValue(ArmorModelProperty, value); }
        }

        public static readonly DependencyProperty ArmorTypeEnumProperty =
                DependencyProperty.Register("ArmorDetailsUC_ArmorTypeEnum", typeof(ArmorEnum), typeof(ArmorDetailsUC));
                    //new PropertyMetadata(OnArmorModelPropertyChanged));

        public ArmorEnum ArmorDetailsUC_ArmorTypeEnum
        {
            get { return (ArmorEnum)GetValue(ArmorTypeEnumProperty); }
            set { SetValue(ArmorTypeEnumProperty, value); }
        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        //The purpose of this is that when a WeaponModel object is updated (based on the Parent binding), this 
        // will allow for fine-tuning of the UI without extensive bindings.  i.e. Add Bonus Damage CB, the list
        // of weapon attributes & properties

        // Works in combination with OnWeaponModelChanged
        //private static void OnArmorModelPropertyChanged(
        //    DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    ArmorDetailsUC c = sender as ArmorDetailsUC;
        //    if (c != null)
        //    {
        //        c.OnArmorModelChanged();
        //    }
        //}

        ////
        //protected virtual void OnArmorModelChanged()
        //{
        //    ArmorModel armorModel = ArmorModelValue;

        //}
    }
}
