using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ZipBackup.Backups;
using ZipBackup.Services;
using ZipBackup.UI;
using ZipBackup.Utils;

namespace ZipBackup {
    public partial class Form1 : Form {
        private readonly AppSettings _appSettings;
        private readonly BackgroundBackupService _backupBackgroundService;
        private readonly BackupService _backupService;
        private System.Windows.Forms.Timer _toastTimer;

        public Form1(AppSettings appSettings, BackupService backupService) {
            _appSettings = appSettings;
            _backupService = backupService;
            _backupBackgroundService = new BackgroundBackupService(backupService, appSettings);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            ExceptionHandler.EnableLogUnhandledOnThread();
            tabControl.Dock = DockStyle.Fill;

            var srcConfig = new SourceConfig(_appSettings);
            tabPage1.Controls.Add(srcConfig);
            srcConfig.Show();

            var dstConfig = new DestinationConfig(_appSettings);
            tabPage2.Controls.Add(dstConfig);
            dstConfig.Show();

            var miscConfig = new MiscConfig(_appSettings);
            tabPage3.Controls.Add(miscConfig);
            miscConfig.Show();

#if !DEBUG
            ThreadPool.QueueUserWorkItem(m => UsageStatisticsReporter.ReportUsage());
#endif

            // Backup thread
            _backupBackgroundService.Start();
            this.FormClosing += (_, __) => _backupBackgroundService.Dispose();

            // Toast thread (UI thread)
            _toastTimer = new System.Windows.Forms.Timer { Interval = 5000 };
            _toastTimer.Tick += _toastTimer_Tick;
            _toastTimer.Start();
            this.FormClosing += (_, __) => _toastTimer.Stop();
        }

        private void _toastTimer_Tick(object sender, EventArgs e) {
            var error = _backupService.Errors.FirstOrDefault();
            if (!string.IsNullOrEmpty(error)) {
                _backupService.Errors.Remove(error);
                var content = error.Split("\n");
                ToastUtil.Show(content);
            }
        }
    }
}
