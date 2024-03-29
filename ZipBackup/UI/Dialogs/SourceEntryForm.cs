﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ZipBackup.Backups;
using ZipBackup.Utils;

namespace ZipBackup.UI.Dialogs {
    public partial class SourceEntryForm : Form {
        public BackupSourceEntry Entry;

        public SourceEntryForm() {
            InitializeComponent();
            Entry = new BackupSourceEntry();
        }

        public SourceEntryForm(BackupSourceEntry entry) {
            InitializeComponent();
            Entry = entry;
        }

        private void SourceEntryForm_Load(object sender, EventArgs e) {
            cbIncludeSubfodlers.Checked = Entry.Recursive;
            tbExclusionFilter.Text = Entry.ExclusionMask;
            tbInclusionFilter.Text = Entry.InclusionMask;
            tbName.Text = Entry.Name;
            tbPath.Text = Entry.Folder;
            tbExclusionFilter.Enabled = tbInclusionFilter.Text.Length == 0;

            tbInclusionFilter.KeyPress += InclusionFilter_KeyPress;
            tbExclusionFilter.KeyPress += ExclusionFilter_KeyPress;
            tbName.KeyPress += TbName_KeyPress;
            cbCompression.SelectedIndex = (int)Entry.Compression;
            tbPath.OnPaste += TbPath_OnPaste;
        }

        private void TbPath_OnPaste(object sender, EventArgs e) {
            var arg = (TextBoxWithPaste.PasteEventArg) e;
            tbPath.Text = EnvPathConverterUtil.ToEnvironmentalPath(arg.Clipboard);

            UpdateNameIfMissing(arg.Clipboard);
        }

        private void UpdateNameIfMissing(string path) {
            if (string.IsNullOrEmpty(tbName.Text)) {
                // Not using Path or Directory here, as we have no idea if the pasted path is valid.
                tbName.Text = path.Substring(1 + path.LastIndexOf(@"\", StringComparison.Ordinal));
            }
        }

        private void TbName_KeyPress(object sender, KeyPressEventArgs e) {
            // Invalid characters for a filename
            List<char> prohibitedChars = new List<char>() {
                '>',
                '<',
                ':',
                '"',
                '/',
                '\\',
                '|',
                '?',
                '*'
            };
            if (prohibitedChars.Any(c => c == e.KeyChar)) {
                errorProvider.SetError(tbName, $"The letter '{e.KeyChar}' is prohibited");
                e.Handled = true;
                return;
            }

            errorProvider.Clear();
            var result = (tbName.Text + e.KeyChar).ToUpperInvariant();
            ValidateName(result);
        }

        private bool ValidateName(string name) {
            // Prohibited filenames
            List<string> prohibitedNames = new List<string>() {
                "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8",
                "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
            };

            if (prohibitedNames.Any(prohibited => prohibited.Equals(name) || name.StartsWith(prohibited + "."))) {
                errorProvider.SetError(tbName, $"The name '{name}' is prohibited");
                return false;
            }
            errorProvider.Clear();

            return true;
        }

        /// <summary>
        /// Verifies that the regex on the inclusion filter is at least half safe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InclusionFilter_KeyPress(object sender, KeyPressEventArgs e) {
            var pattern = tbInclusionFilter.Text + e.KeyChar;
            if (!RegexUtils.IsValidRegex(pattern)) {
                errorProvider.SetError(tbInclusionFilter, "Invalid regex");
            }
            else {
                errorProvider.Clear();
            }

            e.Handled = false;
        }


        /// <summary>
        /// Verifies that the regex on the exclusion filter is at least half safe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExclusionFilter_KeyPress(object sender, KeyPressEventArgs e) {
            var pattern = tbExclusionFilter.Text + e.KeyChar;

            if (!RegexUtils.IsValidRegex(pattern)) {
                errorProvider.SetError(tbExclusionFilter, "Invalid regex");
            }
            else {
                errorProvider.Clear();
            }

            e.Handled = false;
        }

        /// <summary>
        /// Disable the exclusion filter if inclusion is set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbInclusionFilter_TextChanged(object sender, EventArgs e) {
            tbExclusionFilter.Enabled = tbInclusionFilter.Text.Length == 0;
        }

        private void tbExclusionFilter_TextChanged(object sender, EventArgs e) {

        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        

        private void btnSave_Click(object sender, EventArgs e) {
            if (!RegexUtils.IsValidRegex(tbInclusionFilter.Text)) {
                errorProvider.SetError(tbInclusionFilter, "Invalid regex");
                return;
            }

            if (tbInclusionFilter.Text.Length == 0) {
                if (!RegexUtils.IsValidRegex(tbExclusionFilter.Text)) {
                    errorProvider.SetError(tbExclusionFilter, "Invalid regex");
                    return;
                }
            }

            if (string.IsNullOrEmpty(tbPath.Text) || !Directory.Exists(EnvPathConverterUtil.FromEnvironmentalPath(tbPath.Text))) {
                errorProvider.SetError(btnBrowsePath, "Directory does not exist");
                return;
            }

            if (string.IsNullOrEmpty(tbName.Text)) {
                errorProvider.SetError(tbName, "No name defined");
                return;
            }

            if (!ValidateName(tbName.Text)) {
                return;
            }

            Entry.Folder = tbPath.Text;
            Entry.Name = tbName.Text;
            Entry.InclusionMask = tbInclusionFilter.Text;
            Entry.ExclusionMask = string.IsNullOrEmpty(tbInclusionFilter.Text) ? tbExclusionFilter.Text : string.Empty;
            Entry.Compression = (CompressionLevel)cbCompression.SelectedIndex;
            Entry.Recursive = cbIncludeSubfodlers.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnBrowsePath_Click(object sender, EventArgs e) {
            using (var folderBrowserDialog = new FolderBrowserDialog()) {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                    DialogResult = DialogResult.None;
                    tbPath.Text = EnvPathConverterUtil.ToEnvironmentalPath(folderBrowserDialog.SelectedPath);

                    UpdateNameIfMissing(folderBrowserDialog.SelectedPath);
                }
            }
        }
    }
}
