using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using ZipBackup.Services;

namespace ZipBackup.UI {
    public partial class MiscConfig : Form {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MiscConfig));
        private readonly AppSettings _appSettings;
        public MiscConfig(AppSettings appSettings) {
            _appSettings = appSettings;
            InitializeComponent();
            TopLevel = false;
        }

        private void tbFilePattern_TextChanged(object sender, EventArgs e) {
            var result = "";
            try {
                result = DateTime.Now.ToString(tbFilePattern.Text);
                _appSettings.FilenamePattern = tbFilePattern.Text;
                Logger.Info($"Updated filenavm pattern to {tbFilePattern.Text}");
            }
            catch (Exception ex) {
                result = "Error: " + ex.Message;
            }
            lbPatternPreview.Text = $"Preview: name{result}";
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) {
        }

        private void groupBox1_Enter(object sender, EventArgs e) {

        }

        private void MiscConfig_Load(object sender, EventArgs e) {
            tbFilePattern.Text = _appSettings.FilenamePattern;
            tbInterval.KeyPress += tbInterval_KeyPress;
            tbInterval.Text = _appSettings.BackupIntervalHours.ToString();
        }


        private void btnSavePassword_Click(object sender, EventArgs e) {
            if (passwordInput1.Text != passwordInput2.Text) {
                errorProvider1.SetError(passwordInput1, "The passwords does not match");
            }
            else {
                errorProvider1.Clear();
                _appSettings.SetZipPassword(passwordInput1.Text);
                Logger.Info("Updated zip password");
            }
        }


        private void tbInterval_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == '\b');
        }

        private void tbInterval_TextChanged(object sender, EventArgs e) {
            var value = int.TryParse(tbInterval.Text, out var val) ? val : 0;
            if (value is > 0 and < 1000) {
                _appSettings.BackupIntervalHours = value;
                Logger.Info($"Updated backup interval to {value} hours");
            }
        }
    }
}
