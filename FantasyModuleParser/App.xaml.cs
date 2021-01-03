using FantasyModuleParser.Main.Models;
using FantasyModuleParser.Main.Services;
using FantasyModuleParser.NPC;
using FantasyModuleParser.NPC.Controllers;
using log4net;
using log4net.Appender;
#region Log4Net Info
// using log4net;
// using log4net.Appender;
// using log4net.Core;
#endregion
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;

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
            string errorMessage = string.Format(CultureInfo.InvariantCulture, "An unhandled exception occurred: {0}\r=========================\r\r Restarting Fantasy Module Parser may be required.\n\nIf still unresolved, please let developers know in the Discord server.", e.Exception.Message);
            
            // This is purely for quick debugging errors that a user can copy & Paste.
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            #region Log4Net Info
            log.Error("Unexpected Error :: " + e.Exception.Message);
            #endregion
            e.Handled = true;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        protected override void OnStartup(StartupEventArgs e) 
        {
            #region Log4Net Info
            //log4net.Config.XmlConfigurator.Configure(Log4NetXmlSetup());
            Log4NetXmlSetup();
            RollingFileAppender fileAppender = LogManager.GetRepository()
                .GetAppenders().First(appender => appender is RollingFileAppender) as RollingFileAppender;
            #endregion
            SettingsService settingService = new SettingsService();
            SettingsModel settingsModel = settingService.Load();
            #region Log4Net Info
            string logFolderPath = settingsModel.LogFolderLocation;

            if (String.IsNullOrEmpty(logFolderPath))
            {
                logFolderPath = Path.Combine(new SettingsService().Load().MainFolderLocation, "logs");
            }
            //TODO: Create option to change Log folder location
             fileAppender.File = Path.Combine(logFolderPath, "fantasyModuleParser.log");

            //Update the logging level to what's defined in the config
             settingService.ChangeLogLevel(settingsModel);

            log.Info("        =============  Started Logging  =============        ");
            base.OnStartup(e);
            #endregion
        }

        private static void Log4NetXmlSetup()
        {
            const string xmlData = @" <log4net>
  <root>
    <level value=""DEBUG"" />
    <appender-ref ref= ""console"" />
    <appender-ref ref= ""file"" />
  </root >
  <appender name = ""console"" type = ""log4net.Appender.ConsoleAppender"" >
       <layout type = ""log4net.Layout.PatternLayout"" >
          <conversionPattern value = ""%date %level %logger - %message%newline"" />
         </layout >
       </appender >
       <appender name = ""file"" type = ""log4net.Appender.RollingFileAppender"" >
            <file type = ""log4net.Util.PatternString""
    value = ""%envFolderPath{MainFolderLocation}\\logs\\fantasyModuleParser.log"" />
    <appendToFile value = ""true"" />
     <rollingStyle value = ""Size"" />
      <maxSizeRollBackups value = ""5"" />
       <maximumFileSize value = ""2MB"" />
        <staticLogFileName value = ""true"" />
         <layout type = ""log4net.Layout.PatternLayout"" >
            <conversionPattern value = ""%date [%thread] %level %logger - %message%newline"" />
           </layout >
                </appender >
       </log4net > ";

            //return new MemoryStream(ASCIIEncoding.Default.GetBytes(xmlData));

            XmlDocument doc = new XmlDocument() { XmlResolver = null };
            System.IO.StringReader sreader = new System.IO.StringReader(xmlData);
            XmlReader reader = XmlReader.Create(sreader, new XmlReaderSettings() { XmlResolver = null });
            doc.Load(reader);
            reader.Dispose();
            log4net.Config.XmlConfigurator.Configure(doc.DocumentElement);
        }
    }
}
