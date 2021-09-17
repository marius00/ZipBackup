using System;
using System.Windows.Forms;
using ZipBackup.Services;
using ZipBackup.UI;

namespace ZipBackup {
    public partial class Form1 : Form {
        private readonly SettingsService _settingsService;
        public Form1(SettingsService settingsService) {
            _settingsService = settingsService;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            ExceptionHandler.EnableLogUnhandledOnThread();
            tabControl.Dock = DockStyle.Fill;

            var srcConfig = new SourceConfig(_settingsService);
            tabPage1.Controls.Add(srcConfig);
            srcConfig.Show();

            var dstConfig = new DestinationConfig();
            tabPage2.Controls.Add(dstConfig);
            dstConfig.Show();

            var miscConfig = new MiscConfig(_settingsService);
            tabPage3.Controls.Add(miscConfig);
            miscConfig.Show();
        }
    }
}
