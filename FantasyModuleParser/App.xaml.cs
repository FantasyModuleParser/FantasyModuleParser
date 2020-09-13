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
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = string.Format("An unhandled exception occurred: {0}\r=========================\r\r Restarting Fantasy Module Parser maybe required.", e.Exception.Message);
            
            // This is purely for quick debugging errors that a user can copy & Paste.
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            
            e.Handled = true;
        }
    }
}
