using EngineeringSuite.NPC;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EngineeringSuite
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // NPCModel object intended to be shared across all windows
        private NPCModel _npcModel;
        public NPCModel NpcModelObject 
        {
            get
            {
                if (_npcModel == null)
                {
                    _npcModel = new NPCModel();
                } return _npcModel;
            }
            set
            {
                // Because this is initialized from the beginning, 'set' may not be needed.
                _npcModel = value;
            }
        }
    }
}
