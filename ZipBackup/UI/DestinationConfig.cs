using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZipBackup.Backups;
using ZipBackup.Services;
using ZipBackup.Utils;

namespace ZipBackup.UI {
    public partial class DestinationConfig : Form {
        private readonly AppSettings _appSettings;

        public DestinationConfig(AppSettings appSettings) {
            _appSettings = appSettings;
            InitializeComponent();
            TopLevel = false;
        }
        private void DestinationConfig_Load(object sender, EventArgs e) {
            Dock = DockStyle.Fill;
            UpdateListview();

            listView1.SelectedIndexChanged += ListView1_SelectedIndexChanged;
            ListView1_SelectedIndexChanged(null, null);

            contextMenuStrip1.Opening += (s, e) => e.Cancel = false;
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e) {
            var allowEditDelete = listView1.SelectedItems.Count != 0;
            btnEdit.Enabled = allowEditDelete;
            btnDelete.Enabled = allowEditDelete;
        }

        private void UpdateListview() {
            listView1.BeginUpdate();
            listView1.Items.Clear();

            var sources = _appSettings.BackupDestinations ?? new List<BackupDestinationEntry>();
            foreach (var source in sources.ToList()) {
                listView1.Items.Add(ToListViewItem(source));
            }
            listView1.EndUpdate();


            var allowEditDelete = listView1.SelectedItems.Count != 0;
            btnEdit.Enabled = allowEditDelete;
            btnDelete.Enabled = allowEditDelete;
        }

        private ListViewItem ToListViewItem(BackupDestinationEntry entry) {
            var lvi = new ListViewItem(entry.Name);
            lvi.SubItems.Add(entry.Folder);
            lvi.Tag = entry;

            return lvi;
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            var srcEntryForm = new DestinationEntryForm();
            if (srcEntryForm.ShowDialog() == DialogResult.OK) {
                _appSettings.AddBackupDestination(srcEntryForm.Entry);
                UpdateListview();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e) {

            if (listView1.SelectedItems.Count == 0)
                MessageBox.Show("Error - Nothing selected");
            else {
                foreach (ListViewItem lvi in listView1.SelectedItems) {
                    var entry = (BackupDestinationEntry)lvi.Tag;
                    var srcEntryForm = new DestinationEntryForm(entry);
                    if (srcEntryForm.ShowDialog() == DialogResult.OK) {
                        _appSettings.RemoveBackupDestination(entry);
                        _appSettings.AddBackupDestination(srcEntryForm.Entry);
                        UpdateListview();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            if (listView1.SelectedItems.Count == 0)
                MessageBox.Show("Error - Nothing selected");
            else {
                foreach (ListViewItem lvi in listView1.SelectedItems) {
                    if (MessageBox.Show("Are you sure you wish to delete this entry?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes) {
                        var entry = (BackupDestinationEntry)lvi.Tag;
                        _appSettings.RemoveBackupDestination(entry);
                        UpdateListview();
                    }
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e) {
            btnEdit_Click(sender, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            btnDelete_Click(sender, e);
        }
    }
}
