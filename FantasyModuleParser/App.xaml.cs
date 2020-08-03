using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using System.Windows;

namespace FantasyModuleParser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public NPCModel NpcModel { get; set; }

        public App()
        {
            ShutdownMode = ShutdownMode.OnLastWindowClose;
            NPCController npcController = new NPCController();
            NpcModel = npcController.InitializeNPCModel();
        }
    }
}
