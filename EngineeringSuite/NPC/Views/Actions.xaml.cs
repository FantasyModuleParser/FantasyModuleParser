using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EngineeringSuite.NPC.Controllers;
using EngineeringSuite.NPC.ViewModel;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace EngineeringSuite.NPC
{
    /// <summary>
    /// Interaction logic for new_NPCActions.xaml
    /// </summary>
    public partial class Actions : Window
    {
        public Actions()
        {
            InitializeComponent();
        }

        private void action_Checked(object sender, RoutedEventArgs e)
        {
            if (multiAttack.IsChecked == true)
            {
                stackMulti.Visibility = Visibility.Visible;
                stackOther.Visibility = Visibility.Hidden;
                stackWeapon.Visibility = Visibility.Hidden;
            }
            if (weaponAttack.IsChecked == true)
            {
                stackMulti.Visibility = Visibility.Hidden;
                stackOther.Visibility = Visibility.Hidden;
                stackWeapon.Visibility = Visibility.Visible;
            }
            if (otherAttack.IsChecked == true)
            {
                stackMulti.Visibility = Visibility.Hidden;
                stackOther.Visibility = Visibility.Visible;
                stackWeapon.Visibility = Visibility.Hidden;
            }
        }
    }
   
}
