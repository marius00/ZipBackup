﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using ZipBackup.Services;
using ZipBackup.Settings;
using ZipBackup.Utils;

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
            Dock = DockStyle.Fill;
            tbFilePattern.Text = _appSettings.FilenamePattern;
            tbInterval.KeyPress += tbInterval_KeyPress;
            tbInterval.Text = _appSettings.BackupIntervalHours.ToString();

            tbDefaultExclusion.Text = _appSettings.DefaultExclusionPattern;
            tbErrorThreshold.KeyPress += tbInterval_KeyPress;
            tbErrorThreshold.Text = _appSettings.ErrorThreshold.ToString();
            cbStartOnSystemBoot.Checked = StartupRegistrationService.IsInstalled("ZipBackup");
            cbStartMinimized.Checked = _appSettings.StartMinimized;

            passwordInput1.Enter += PasswordInput_GotFocus;
            passwordInput2.Enter += PasswordInput_GotFocus;

            if (string.IsNullOrEmpty(_appSettings.ZipPassword)) {
                passwordInput1.Text = "";
                passwordInput2.Text = "";
            }
        }

        private void PasswordInput_GotFocus(object sender, EventArgs e) {
            if (sender is MaskedTextBox tb) {
                tb.BeginInvoke(new Action(tb.SelectAll));
            }
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

        private void tbErrorThreshold_TextChanged(object sender, EventArgs e) {
            var value = int.TryParse(tbErrorThreshold.Text, out var val) ? val : 0;
            if (value is > 0 and < 1000) {
                _appSettings.ErrorThreshold = value;
                Logger.Info($"Updated error threshold to {value} errors");
            }

        }

        private void cbStartOnSystemBoot_CheckedChanged(object sender, EventArgs e) {
            if (sender is CheckBox cb) {
                if (cb.Checked) {
                    StartupRegistrationService.Install("ZipBackup");
                } else {
                    StartupRegistrationService.Uninstall("ZipBackup");
                }
            }
        }

        private void cbStartMinimized_CheckedChanged(object sender, EventArgs e) {
            if (sender is CheckBox cb) {
                _appSettings.StartMinimized = cb.Checked;
            }
        }

        private void linkFilenamePatternHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            ProcessStartInfo psi = new ProcessStartInfo {
                FileName = "https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings",
                UseShellExecute = true
            };

            Process.Start(psi);
        }

        private void tbDefaultExclusion_TextChanged(object sender, EventArgs e) {
            var tb = (TextBox)sender;
            
            if (!string.IsNullOrEmpty(tb.Text) && !RegexUtils.IsValidRegex(tb.Text)) {
                errorProvider1.SetError(tb, "Invalid regex");
            } else {
                _appSettings.DefaultExclusionPattern = tb.Text;
                errorProvider1.Clear();
            }
        }
    }
}
