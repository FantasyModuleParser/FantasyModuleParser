﻿using System;
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
using System.Windows.Shapes;
using EngineeringSuite;

namespace EngineeringSuite.NPC
{
    /// <summary>
    /// Interaction logic for LairActions.xaml
    /// </summary>
    public partial class LairActions : Window
    {
        public LairActions()
        {
            InitializeComponent();
        }
        private void action_Checked(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
