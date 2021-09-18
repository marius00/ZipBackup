using System;
using System.Threading;
using System.Windows.Forms;
using ZipBackup.Services;
using ZipBackup.UI;

namespace ZipBackup {
    public partial class Form1 : Form {
        private readonly AppSettings _appSettings;
        public Form1(AppSettings appSettings) {
            _appSettings = appSettings;
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
        }
    }
}
