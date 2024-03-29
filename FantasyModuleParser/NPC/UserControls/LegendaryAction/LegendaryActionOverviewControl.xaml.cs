﻿using FantasyModuleParser.NPC.Models.Action;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FantasyModuleParser.NPC.UserControls.LegendaryAction
{
    /// <summary>
    /// Interaction logic for LegendaryActionOverviewControl.xaml
    /// </summary>
    public partial class LegendaryActionOverviewControl : UserControl
    {
        public LegendaryActionModel LegendaryActionModel { get; set; } = new LegendaryActionModel();
        public LegendaryActionOverviewControl()
        {
            InitializeComponent();
            //DataContext = LegendaryActionModel;
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
    }
}
