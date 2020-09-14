﻿using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.Main.ViewModels;
using System;
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

namespace FantasyModuleParser.Main.UserControls
{
    /// <summary>
    /// Interaction logic for CategoriesUC.xaml
    /// </summary>
    public partial class CategoriesUC : UserControl
    {
        private CategoriesUCViewModel viewModel;
        public CategoriesUC()
        {
            InitializeComponent();
            viewModel = new CategoriesUCViewModel();
            DataContext = viewModel;
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AddCategoryToModule(NewCategoryValue.Text);
        }
    }
}