using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using log4net.Repository.Hierarchy;
using ZipBackup.Backups;
using ZipBackup.Services;
using ZipBackup.Settings;
using ZipBackup.UI.Dialogs;
using ZipBackup.Utils;

namespace ZipBackup.UI {
    public partial class SourceConfig : Form {
        static readonly ILog Logger = LogManager.GetLogger(typeof(SourceConfig));
        private readonly AppSettings _appSettings;
        private readonly BackupService _backupService;
        private readonly NotificationService _notificationService;
        private System.Windows.Forms.Timer _refreshListviewTimer;
        private bool _hasModiifedList = false;

        public SourceConfig(AppSettings appSettings, BackupService backupService, NotificationService notificationService) {
            _appSettings = appSettings;
            _backupService = backupService;
            _notificationService = notificationService;
            InitializeComponent();
            TopLevel = false;
        }

        private void SourceConfig_Load(object sender, EventArgs e) {
            Dock = DockStyle.Fill;
            UpdateListview();

            listView1.SelectedIndexChanged += ListView1_SelectedIndexChanged;
            ListView1_SelectedIndexChanged(null, null);
            listView1.MouseDoubleClick += btnEdit_Click;

            contextMenuStrip1.Opening += (s, e) => e.Cancel = false;


            _refreshListviewTimer = new System.Windows.Forms.Timer {Interval = 5000};
            _refreshListviewTimer.Tick += _refreshListviewTimer_Tick;
            _refreshListviewTimer.Start();

            // Skip refreshes if the list is unmodified
            _appSettings.OnMutate += (_, ___) => {
                _hasModiifedList = true;
            };
        }

        private void _refreshListviewTimer_Tick(object sender, EventArgs e) {
            if (Visible && _hasModiifedList) {
                UpdateListview();
                _hasModiifedList = false;
            }
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e) {
            var allowEditDelete = listView1.SelectedItems.Count != 0;
            btnEdit.Enabled = allowEditDelete;
            btnDelete.Enabled = allowEditDelete;
        }

        private void UpdateListview() {
            var entry = GetSelectedItem();

            listView1.BeginUpdate();
            listView1.Items.Clear();

            var sources = _appSettings.BackupSources ?? new List<BackupSourceEntry>();
            foreach (var source in sources.ToList()) {
                var srcEntry = ToListViewItem(source);
                if (entry != null && entry.Folder == source.Folder)
                    srcEntry.Selected = true;

                listView1.Items.Add(srcEntry);
            }

            listView1.EndUpdate();


            var allowEditDelete = listView1.SelectedItems.Count != 0;
            btnEdit.Enabled = allowEditDelete;
            btnDelete.Enabled = allowEditDelete;
        }

        private ListViewItem ToListViewItem(BackupSourceEntry entry) {
            var lvi = new ListViewItem(entry.Name);
            lvi.SubItems.Add(entry.Folder);
            lvi.SubItems.Add(entry.InclusionMask);
            lvi.SubItems.Add(entry.ExclusionMask);
            lvi.SubItems.Add(entry.NextUpdate > 0 ? DateTimeEpochExtension.FromTimestamp(entry.NextUpdate).ToString("yyyy-MM-dd HH:mm") : "Now..");
            lvi.Tag = entry;

            return lvi;
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            var srcEntryForm = new SourceEntryForm();
            if (srcEntryForm.ShowDialog() == DialogResult.OK) {
                _appSettings.AddBackupSource(srcEntryForm.Entry);
                UpdateListview();
            }
        }

        private BackupSourceEntry GetSelectedItem() {
            foreach (ListViewItem lvi in listView1.SelectedItems) {
                var entry = (BackupSourceEntry) lvi.Tag;
                return entry;
            }

            return null;
        }

        private void btnEdit_Click(object sender, EventArgs e) {
            if (listView1.SelectedItems.Count == 0)
                MessageBox.Show("Error - Nothing selected");
            else {
                var entry = GetSelectedItem();
                if (entry != null) {
                    var srcEntryForm = new SourceEntryForm(entry);
                    if (srcEntryForm.ShowDialog() == DialogResult.OK) {
                        _appSettings.RemoveBackupSource(entry);
                        _appSettings.AddBackupSource(srcEntryForm.Entry);
                        UpdateListview();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            if (listView1.SelectedItems.Count == 0)
                MessageBox.Show("Error - Nothing selected");
            else {
                var entry = GetSelectedItem();
                if (entry != null && MessageBox.Show("Are you sure you wish to delete this entry?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes) {
                    _appSettings.RemoveBackupSource(entry);
                    UpdateListview();
                }
            }
        }

        private void btnSuggestions_Click(object sender, EventArgs e) {
            var sourceSuggestionDialog = new SourceSuggestions(_appSettings);
            if (sourceSuggestionDialog.ShowDialog() == DialogResult.OK) {
                _appSettings.AddBackupSource(new BackupSourceEntry {
                    Name = sourceSuggestionDialog.ChosenSuggestion.Name,
                    Folder = sourceSuggestionDialog.ChosenSuggestion.Path
                });

                UpdateListview();
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e) {
            btnEdit_Click(sender, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            btnDelete_Click(sender, e);
        }

        private void backupnowToolStripMenuItem_Click(object sender, EventArgs e) {
            if (listView1.SelectedItems.Count == 0)
                MessageBox.Show("Error - Nothing selected");
            else {
                foreach (ListViewItem lvi in listView1.SelectedItems) {
                    var entry = (BackupSourceEntry) lvi.Tag;
                    _notificationService.Add(Guid.NewGuid().ToString(), $"Starting backup of {entry.Name}...\nThis may take a while..");
                    ThreadPool.QueueUserWorkItem(m => _backupService.PerformBackup(entry));
                }
            }
        }
    }
}