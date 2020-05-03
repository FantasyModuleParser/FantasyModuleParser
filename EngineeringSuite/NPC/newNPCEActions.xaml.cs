using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EngineeringSuite.NPC.Controller;
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
}
