using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ZipBackup.Backups;
using ZipBackup.Utils;

namespace ZipBackup.Services {
    public class AppSettings {
        public event EventHandler OnMutate;
        private List<BackupSourceEntry> _backupSources;
        private List<BackupDestinationEntry> _backupDestinations;
        private string _filenamePattern;
        private string _uuid;
        private string _zipPassword;
        private int _cpuHash;
        private int _backupIntervalHours;

        public void AddBackupSource(BackupSourceEntry entry) {
            _backupSources ??= new List<BackupSourceEntry>();

            if (!_backupSources.Contains(entry)) {
                _backupSources.Add(entry);
            }

            OnMutate?.Invoke(null, null);
        }

        public void BackupSourcesHasMutated() {
            OnMutate?.Invoke(null, null);
        }

        public void AddBackupDestination(BackupDestinationEntry entry) {
            _backupDestinations ??= new List<BackupDestinationEntry>();

            if (!_backupDestinations.Contains(entry)) {
                _backupDestinations.Add(entry);
            }

            OnMutate?.Invoke(null, null);
        }

        public void RemoveBackupSource(BackupSourceEntry entry) {
            _backupSources?.RemoveAll(src => src.Folder.Equals(entry.Folder, StringComparison.CurrentCultureIgnoreCase));
            OnMutate?.Invoke(null, null);
        }

        public void RemoveBackupDestination(BackupDestinationEntry entry) {
            _backupDestinations?.RemoveAll(src => src.Folder.Equals(entry.Folder, StringComparison.CurrentCultureIgnoreCase));
            OnMutate?.Invoke(null, null);
        }


        public List<BackupSourceEntry> BackupSources {
            get => _backupSources;
            set{
                _backupSources = value;
                OnMutate?.Invoke(null, null);
            }
        }


        public List<BackupDestinationEntry> BackupDestinations {
            get => _backupDestinations;
            set{
                _backupDestinations = value;
                OnMutate?.Invoke(null, null);
            }
        }

        // TODO: "field = value" as of C# 10, .NET 6 Preview 7 and up
        public string UUID {
            get => _uuid;
            set{
                _uuid = value;
                OnMutate?.Invoke(null, null);
            }
        }

        public int BackupIntervalHours {
            get => _backupIntervalHours;
            set{
                _backupIntervalHours = value;
                OnMutate?.Invoke(null, null);
            }
        }

        public int CpuHash {
            get => _cpuHash;
            set{
                _cpuHash = value;
                OnMutate?.Invoke(null, null);
            }
        }

        public string FilenamePattern {
            get => _filenamePattern;
            set{
                _filenamePattern = value;
                OnMutate?.Invoke(null, null);
            }
        }

        /// <summary>
        /// Password encrypted using the CPU serial
        /// </summary>
        public string ZipPassword {
            get => _zipPassword;
            set{
                _zipPassword = value;
                OnMutate?.Invoke(null, null);
            }
        }

        public void SetZipPassword(string password) {
            ZipPassword = EncryptionUtil.EncryptString(password, EncryptionUtil.GetCpuSerial());
            CpuHash = EncryptionUtil.GetDeterministicHashCode(EncryptionUtil.GetCpuSerial());
        }

        /// <summary>
        /// Password encrypted using the CPU serial
        /// </summary>
        [JsonIgnore]
        public string ZipPasswordPlaintext => EncryptionUtil.DecryptString(_zipPassword, EncryptionUtil.GetCpuSerial());
    }
}