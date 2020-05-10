using EngineeringSuite.NPC;
using System.Windows;

namespace EngineeringSuite
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public NPCModel NpcModel { get; set; } = new NPCModel();
    }
}
