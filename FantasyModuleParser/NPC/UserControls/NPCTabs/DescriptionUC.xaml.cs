﻿using FantasyModuleParser.NPC.Controllers;
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

namespace FantasyModuleParser.NPC.UserControls.NPCTabs
{
    /// <summary>
    /// Interaction logic for DescriptionUC.xaml
    /// </summary>
    public partial class DescriptionUC : UserControl
    {
        #region Controllers
        public NPCController npcController { get; set; }
        #endregion
        public DescriptionUC()
        {
            InitializeComponent();
            npcController = new NPCController();
            //var npcModel = ((App)Application.Current).NpcModelObject;
            DataContext = npcController.GetNPCModel();
        }
    }
}
