using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Repository.Hierarchy;
using ZipBackup.Backups;
using ZipBackup.Utils;

namespace ZipBackup.Services {
    class BackgroundBackupService : IDisposable {
        static readonly ILog Logger = LogManager.GetLogger(typeof(BackgroundBackupService));
        private Thread _t;
        private readonly BackupService _backupService;
        private readonly AppSettings _appSettings;
        private volatile bool _isActive = true;

        public BackgroundBackupService(BackupService backupService, AppSettings appSettings) {
            _backupService = backupService;
            _appSettings = appSettings;
        }


        public void Start() {
            if (_t != null) {
                throw new ArgumentException("Attempting to a second thread");
            }

            _t = new Thread(ThreadStart);
            _t.Start();
        }

        private void Run() {
            _backupService.Backup();

            // TODO: If there are more than X errors: Toast.
        }

        private void ThreadStart() {
            ExceptionHandler.EnableLogUnhandledOnThread();
            _t.Name = "BackgroundBackupService";
            _t.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            Logger.Info("Background service started");

            while (_isActive && (_t?.IsAlive ?? false)) {
                Run();

                try {
                    Thread.Sleep(100);
                } catch (ThreadInterruptedException) {
                    // Don't care
                }
            }

            Logger.Info("Background service terminated");
        }

        public void Dispose() {
            _isActive = false;
            _t = null;
        }
    }
}
