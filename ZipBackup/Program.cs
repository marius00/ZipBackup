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
        /// <summary>
        /// Configure log4net
        /// </summary>
        static Program() {
            var loggerRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(loggerRepository, new FileInfo("Log4net.config"));

        }
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            var appSettings = SettingsReader.Load(GlobalPaths.SettingsFile).AppSettings;
            ExceptionHandler.EnableLogUnhandledOnThread();
            if (string.IsNullOrEmpty(appSettings.UUID)) {
                appSettings.UUID = Guid.NewGuid().ToString();
            }
            
            var bs = new BackupService(appSettings);

            bs.Backup(new BackupSourceEntry {
                Folder = @"%appdata%\..\local\evilsoft\MIRCopy",
                //InclusionMask = ".resx",
                ExclusionMask = ".resx|.designer.cs"

            }, @"f:\temp\backup.zip", string.Empty);

            UsageStatisticsReporter.UrlStats = "https://webstats.evilsoft.net/zipbackup";
            UsageStatisticsReporter.Uuid = appSettings.UUID;
            ToastUtil.Show("ZipBackup disabled", "The CPU serial hash does not match the current setup.", "Please update the ZIP password under misc settings.");
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(appSettings));
        }
    }
}
