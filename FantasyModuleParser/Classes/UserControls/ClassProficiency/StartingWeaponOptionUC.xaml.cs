using FantasyModuleParser.Classes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.Classes.UserControls.ClassProficiency
{
    /// <summary>
    /// Interaction logic for StartingWeaponOptionUC.xaml
    /// </summary>
    public partial class StartingWeaponOptionUC : UserControl
    {
        public StartingWeaponOptionUC()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ClassModelProperty =
            DependencyProperty.Register("ClassModelValue", typeof(ClassModel), typeof(StartingWeaponOptionUC));

        public ClassModel ClassModelValue
        {
            get { return (ClassModel)GetValue(ClassModelProperty); }
            set { SetValue(ClassModelProperty, value); }
        }
    }
}
