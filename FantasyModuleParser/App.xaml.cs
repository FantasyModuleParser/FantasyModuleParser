using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using log4net;
using log4net.Appender;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace FantasyModuleParser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public NPCModel NpcModel { get; set; }
        private SettingsService settingService;

        public App()
        {
            ShutdownMode = ShutdownMode.OnLastWindowClose;
            NPCController npcController = new NPCController();
            NpcModel = npcController.InitializeNPCModel();
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = string.Format("An unhandled exception occurred: {0}\r=========================\r\r Restarting Fantasy Module Parser may be required.\n\nIf still unresolved, please let developers know in the Discord server.", e.Exception.Message);
            
            // This is purely for quick debugging errors that a user can copy & Paste.
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            
            e.Handled = true;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        protected override void OnStartup(StartupEventArgs e) 
        {
            log4net.Config.XmlConfigurator.Configure();
            RollingFileAppender fileAppender = LogManager.GetRepository()
                .GetAppenders().First(appender => appender is RollingFileAppender) as RollingFileAppender;

            //TODO:  Create option to change Log folder location
            fileAppender.File = Path.Combine(new SettingsService().Load().MainFolderLocation, "logs", "fantasyModuleParser.log");
            
            //TODO: Create option to change logging level

            log.Info("        =============  Started Logging  =============        ");
            base.OnStartup(e);
        }
    }
}
