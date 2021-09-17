using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ZipBackup.Backups;
using ZipBackup.Services;
using ZipBackup.Utils;

namespace ZipBackup {
    static class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            var settingsService = SettingsReader.Load(GlobalPaths.SettingsFile).Settings;
            ExceptionHandler.EnableLogUnhandledOnThread();
            if (string.IsNullOrEmpty(settingsService.UUID)) {
                settingsService.UUID = Guid.NewGuid().ToString();
            }


            var bs = new BackupService(settingsService);

            bs.Backup(new BackupSourceEntry {
                Folder = @"F:\Dev\IAGrim\IAGrim\UI",
                InclusionMask = ".resx",
                ExclusionMask = ".resx|.designer.cs"

            }, @"f:\temp\backup.zip");

            UsageStatisticsReporter.UrlStats = "https://webstats.evilsoft.net/zipbackup";
            UsageStatisticsReporter.Uuid = settingsService.UUID;

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(settingsService));
        }
    }
}
