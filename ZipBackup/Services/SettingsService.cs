using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZipBackup.Backups;

namespace ZipBackup.Services {
    class SettingsService {
        public event EventHandler OnMutate;
        private List<BackupSourceEntry> _backupSources;
        private List<BackupDestinationEntry> _backupDestinations;
        private string _filenamePattern;
        private string _uuid;
        private string _zipPassword;
        
        public void AddBackupSource(BackupSourceEntry entry) {
            _backupSources ??= new List<BackupSourceEntry>();

            if (!_backupSources.Contains(entry)) {
                _backupSources.Add(entry);
            }

            OnMutate?.Invoke(null, null);
        }

        public void RemoveBackupSource(BackupSourceEntry entry) {
            _backupSources?.RemoveAll(src => src.Folder.Equals(entry.Folder, StringComparison.CurrentCultureIgnoreCase));
            OnMutate?.Invoke(null, null);
        }


        public List<BackupSourceEntry> BackupSources {
            get => _backupSources;
            set {
                _backupSources = value;
                OnMutate?.Invoke(null, null);
            }
        }


        public List<BackupDestinationEntry> BackupDestinations {
            get => _backupDestinations;
            set {
                _backupDestinations = value;
                OnMutate?.Invoke(null, null);
            }
        }

        // TODO: "field = value" as of C# 10, .NET 6 Preview 7 and up
        public string UUID {
            get => _uuid;
            set {
                _uuid = value;
                OnMutate?.Invoke(null, null);
            }
        }

        public string FilenamePattern {
            get => _filenamePattern;
            set {
                _filenamePattern = value;
                OnMutate?.Invoke(null, null);
            }
        }

        public string ZipPassword {
            get => _zipPassword;
            set {
                _zipPassword = value;
                OnMutate?.Invoke(null, null);
            }
        }

        
    }
}
