﻿using FantasyModuleParser.NPC.Controllers;
using FantasyModuleParser.NPC.Models.Action;
using FantasyModuleParser.NPC.ViewModel;
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

namespace FantasyModuleParser.NPC.UserControls.Action
{
    /// <summary>
    /// Interaction logic for OverviewControl.xaml
    /// </summary>
    public partial class OverviewControl : UserControl
    {

        public ActionViewModel actionViewModel { get; set; }

        public Visibility ShowEditButton { get; set; }

        public OverviewControl()
        {
            InitializeComponent();
            ShowEditButton = Visibility.Visible;
        }

        private void btn_Up_Click(object sender, RoutedEventArgs e)
        {
            OnRaiseActionInList();
        }

        private void btn_Down_Click(object sender, RoutedEventArgs e)
        {
            OnLowerActionInList();
        }

        private void btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            OnEditAction();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            OnRemoveAction();
        }

        private ActionViewModel GetParentActionViewModel()
        {
            if (Parent is Actions)
            {
                var _parentDataContext = ((Actions)Parent).DataContext;

                // Validate and make sure the parent DataContext is ActionViewModel
                if (_parentDataContext is ActionViewModel)
                {
                    ActionViewModel _actionViewModel = (ActionViewModel)_parentDataContext;
                    return _actionViewModel;
                }
            }
            return null;
        }

        #region Button Events
        public event EventHandler RemoveAction;
        protected virtual void OnRemoveAction()
        {
            if (RemoveAction != null) RemoveAction(this, EventArgs.Empty);
        }

        public event EventHandler EditAction;
        protected virtual void OnEditAction()
        {
            if (EditAction != null) EditAction(this, EventArgs.Empty);
        }

        public event EventHandler RaiseActionInList;
        protected virtual void OnRaiseActionInList()
        {
            if (RaiseActionInList != null) RaiseActionInList(this, EventArgs.Empty);
        }
        public event EventHandler LowerActionInList;
        protected virtual void OnLowerActionInList()
        {
            if (LowerActionInList != null) LowerActionInList(this, EventArgs.Empty);
        }
        #endregion
    }
}
