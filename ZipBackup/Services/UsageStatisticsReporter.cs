using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZipBackup.Utils;
using Timer = System.Timers.Timer;

namespace ZipBackup.Services {
    class UsageStatisticsReporterThread : IDisposable {
        private Timer _timer;

        public UsageStatisticsReporterThread() {
            var reportUsageStatistics = new Stopwatch();
            reportUsageStatistics.Start();


            _timer = new Timer();
            _timer.Start();
            _timer.Elapsed += (a1, a2) => {
                if (Thread.CurrentThread.Name == null) {
                    Thread.CurrentThread.Name = "ReportUsageThread";
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                }

                if (reportUsageStatistics.Elapsed.Hours > 12) {
                    ReportUsage();
                    reportUsageStatistics.Restart();
                }
            };

            int min = 1000 * 60;
            int hour = 60 * min;
            _timer.Interval = 12 * hour;
            _timer.AutoReset = true;
            _timer.Start();
        }


        /// <summary>
        /// Report usage once every 12 hours, in case the user runs it 'for ever'
        /// </summary>
        private void ReportUsage() {
            ThreadPool.QueueUserWorkItem(m => UsageStatisticsReporter.ReportUsage());
        }

        public void Dispose() {
            _timer?.Stop();
            _timer?.Dispose();
            _timer = null;
        }
    }
}