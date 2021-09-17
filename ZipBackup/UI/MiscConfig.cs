using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZipBackup.Services;

namespace ZipBackup.UI {
    public partial class MiscConfig : Form {
        private readonly SettingsService _settingsService;
        public MiscConfig(SettingsService settingsService) {
            _settingsService = settingsService;
            InitializeComponent();
            TopLevel = false;
        }

        private void tbFilePattern_TextChanged(object sender, EventArgs e) {
            var result = "";
            try {
                result = DateTime.Now.ToString(tbFilePattern.Text);
                _settingsService.FilenamePattern = tbFilePattern.Text;
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
            tbFilePattern.Text = _settingsService.FilenamePattern;
        }


        private void btnSavePassword_Click(object sender, EventArgs e) {
            _settingsService.SetZipPassword(maskedTextBox1.Text);
        }
    }
}
