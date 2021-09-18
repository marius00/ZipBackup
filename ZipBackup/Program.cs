using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using ZipBackup.Backups;
using ZipBackup.Services;
using ZipBackup.Utils;

namespace ZipBackup {
    static class Program {
        static Program() {
            var retrete = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(retrete, new FileInfo("Log4net.config"));

        }
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            DateTime buildDate = new DateTime(2000, 1, 1)
                .AddDays(version.Build)
                .AddSeconds(version.Revision * 2);

            ILog Logger = LogManager.GetLogger(typeof(Program));
            Logger.InfoFormat("Running version {0}.{1}.{2}.{3} from {4}", version.Major, version.Minor, version.Build, version.Revision, buildDate.ToString("dd/MM/yyyy"));
            

            var appSettings = SettingsReader.Load(GlobalPaths.SettingsFile).AppSettings;
            ExceptionHandler.EnableLogUnhandledOnThread();
            if (string.IsNullOrEmpty(appSettings.UUID)) {
                appSettings.UUID = Guid.NewGuid().ToString();
            }

            
            var bs = new BackupService(appSettings);

            bs.Backup(new BackupSourceEntry {
                Folder = @"%appdata%\..\local\evilsoft\MIRCopy",
                InclusionMask = ".resx",
                ExclusionMask = ".resx|.designer.cs"

            }, @"f:\temp\backup.zip", string.Empty);

            UsageStatisticsReporter.UrlStats = "https://webstats.evilsoft.net/zipbackup";
            UsageStatisticsReporter.Uuid = appSettings.UUID;

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(appSettings));
        }
    }
}
