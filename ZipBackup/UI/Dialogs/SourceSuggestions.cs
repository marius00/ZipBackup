using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ZipBackup.Backups;
using ZipBackup.Settings;
using ZipBackup.Utils;

namespace ZipBackup.UI.Dialogs {
    public partial class SourceSuggestions : Form {
        private readonly AppSettings _appSettings;
        public Suggestion ChosenSuggestion { get; private set; }

        public SourceSuggestions(AppSettings appSettings) {
            _appSettings = appSettings;
            InitializeComponent();
        }

        private void SourceSuggestions_Load(object sender, EventArgs e) {
            listView1.MouseDoubleClick += ListView1_MouseDoubleClick;
            UpdateListview();
        }

        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e) {
            ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);
            ListViewItem item = info.Item;

            if (item != null) {
                ChosenSuggestion = item.Tag as Suggestion;
                DialogResult = DialogResult.OK;
                Close();
            } else {
                this.listView1.SelectedItems.Clear();
            }
        }

        private void UpdateListview() {
            listView1.BeginUpdate();
            listView1.Items.Clear();


            // TODO: Not really the UIs job to keep this .. but..
            var suggestions = new List<Suggestion> {
                new Suggestion { Path = @"%appdata%\VanDyke\Config", Name = "SecureCRT Appdata" },
                new Suggestion { Path = @"%userprofile%\Application Data\VanDyke\Config", Name = "SecureCRT ApplicationData" },
                new Suggestion { Path = @"%AppData%\mIRC", Name = "mIRC" },
                new Suggestion { Path = @"%Appdata%\StardewValley", Name = "Stardew Valley" },
                new Suggestion { Path = @"%appdata%\..\LocalLow\Redbeet Interactive\Raft\User", Name = "Raft" },
                new Suggestion { Path = @"%userprofile%\Documents\Dyson Sphere Program", Name = "Dyson Sphere Program" },
                new Suggestion { Path = @"%userprofile%\Documents\Klei", Name = "Oxygen not included" },
                new Suggestion { Path = @"%appdata%\Postman\Partitions", Name = "Postman" },
                new Suggestion { Path = @"%LocalAppData%\Logitech\Logitech Gaming Software", Name = "Logitech Gaming Software" },
                new Suggestion { Path = @"%appdata%\Mozilla\Firefox", Name = "Firefox" },
                new Suggestion { Path = @"%programfiles%\OpenVPN\config", Name = "OpenVPN certs" },
                new Suggestion { Path = @"%LocalAppData%\PersistentWindows", Name = "PersistentWindows" },
                new Suggestion { Path = @"%programdata%\FlashFXP\5", Name = "FlashFXP 5" },
                new Suggestion { Path = @"%appdata%\..\LocalLow\Noble Muffins\Thief Simulator", Name = "Thief Simulator" },
                new Suggestion { Path = @"%appdata%\..\LocalLow\IronGate\Valheim", Name = "Valheim" },
                new Suggestion { Path = @"%userprofile%\Saved Games", Name = "Elex & other retarded games savegames" },
            };

            var existing = _appSettings.BackupSources.Select(s => s.Folder).ToList();
            foreach (var suggestion in suggestions) {
                if (existing.Contains(suggestion.Path))
                    continue;

                listView1.Items.Add(ToListViewItem(suggestion));

            }


            listView1.EndUpdate();
        }


        private ListViewItem ToListViewItem(Suggestion suggestion) {
            var defaultValues = new BackupSourceEntry();
            var lvi = new ListViewItem(suggestion.Name);
            lvi.SubItems.Add(suggestion.Path);
            lvi.SubItems.Add(defaultValues.InclusionMask);
            lvi.SubItems.Add(defaultValues.ExclusionMask);
            lvi.Tag = suggestion;
            if (!Directory.Exists(EnvPathConverterUtil.FromEnvironmentalPath(suggestion.Path)))
                lvi.ForeColor = Color.Red;

            return lvi;
        }

        public class Suggestion {
            public string Path { get; set; }
            public string Name { get; set; }
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            if (listView1.SelectedItems.Count == 0) {
                MessageBox.Show("Error - Nothing selected");
            }
            else {
                foreach (ListViewItem lvi in listView1.SelectedItems) {
                    ChosenSuggestion = (Suggestion) lvi.Tag;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
