using System;
using System.IO;
using System.Windows.Forms;
using ZipBackup.Backups;
using ZipBackup.Utils;

namespace ZipBackup.UI {
    public partial class DestinationEntryForm : Form {
        public BackupDestinationEntry Entry;

        public DestinationEntryForm() {
            InitializeComponent();
            Entry = new BackupDestinationEntry();
        }

        public DestinationEntryForm(BackupDestinationEntry entry) {
            InitializeComponent();
            Entry = entry;
        }

        private void DestinationEntryForm_Load(object sender, EventArgs e) {
            tbName.Text = Entry.Name;
            tbPath.Text = Entry.Folder;
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }


        private void btnSave_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(tbPath.Text) || !Directory.Exists(EnvPathConverterUtil.FromEnvironmentalPath(tbPath.Text))) {
                errorProvider.SetError(btnBrowsePath, "Directory does not exist");
                return;
            }

            if (string.IsNullOrEmpty(tbName.Text)) {
                errorProvider.SetError(tbName, "No name defined");
                return;
            }
            Entry.Folder = tbPath.Text;
            Entry.Name = tbName.Text;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnBrowsePath_Click(object sender, EventArgs e) {
            using (var folderBrowserDialog = new FolderBrowserDialog()) {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                    DialogResult = DialogResult.None;
                    tbPath.Text = EnvPathConverterUtil.ToEnvironmentalPath(folderBrowserDialog.SelectedPath);
                }
            }
        }
    }
}
