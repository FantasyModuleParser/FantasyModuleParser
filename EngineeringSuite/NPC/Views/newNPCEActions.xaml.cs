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
    public partial class newNPCEActions : Window
    {
        public newNPCEActions()
        {
            InitializeComponent();
        }

        private void action_Checked(object sender, RoutedEventArgs e)
        {
	        throw new NotImplementedException();
        }
    }
   
}
