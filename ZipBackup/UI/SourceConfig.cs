﻿using System;
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

namespace ZipBackup.UI {
    public partial class SourceConfig : Form {
        private readonly SettingsService _settingsService;

        public SourceConfig(SettingsService settingsService) {
            _settingsService = settingsService;
            InitializeComponent();
            TopLevel = false;
        }

        private void SourceConfig_Load(object sender, EventArgs e) {
            Dock = DockStyle.Fill;
            UpdateListview();

            listView1.SelectedIndexChanged += ListView1_SelectedIndexChanged;
            ListView1_SelectedIndexChanged(null, null);
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e) {
            var allowEditDelete = listView1.SelectedItems.Count != 0;
            btnEdit.Enabled = allowEditDelete;
            btnDelete.Enabled = allowEditDelete;
        }

        private void UpdateListview() {
            listView1.BeginUpdate();
            listView1.Items.Clear();

            var sources = _settingsService.BackupSources ?? new List<BackupSourceEntry>();
            foreach (var source in sources.ToList()) {
                listView1.Items.Add(ToListViewItem(source));
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
            lvi.Tag = entry;

            return lvi;
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            var srcEntryForm = new SourceEntryForm();
            if (srcEntryForm.ShowDialog() == DialogResult.OK) {
                _settingsService.AddBackupSource(srcEntryForm.Entry);
                UpdateListview();
            }

        }

        private void btnEdit_Click(object sender, EventArgs e) {
            if (listView1.SelectedItems.Count == 0)
                MessageBox.Show("Error - Nothing selected");
            else {
                foreach (ListViewItem lvi in listView1.SelectedItems) {
                    var entry = (BackupSourceEntry) lvi.Tag;
                    var srcEntryForm = new SourceEntryForm(entry);
                    if (srcEntryForm.ShowDialog() == DialogResult.OK) {
                        _settingsService.RemoveBackupSource(entry);
                        _settingsService.AddBackupSource(srcEntryForm.Entry);
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
                        var entry = (BackupSourceEntry)lvi.Tag;
                        _settingsService.RemoveBackupSource(entry);
                        UpdateListview();
                    }
                }
            }
        }
    }
}
