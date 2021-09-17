using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ionic.Zip;
using log4net;
using ZipBackup.Services;
using ZipBackup.Utils;

namespace ZipBackup.Backups {
    class BackupService {
        private readonly string _localAppdata = System.Environment.GetEnvironmentVariable("LocalAppData");
        private readonly string _userProfile = System.Environment.GetEnvironmentVariable("UserProfile");
        static readonly ILog Logger = LogManager.GetLogger(typeof(BackupService));
        private readonly SettingsService _settingsService;

        public BackupService(SettingsService settingsService) {
            _settingsService = settingsService;
        }

        public void Backup() {
            var sources = _settingsService.BackupSources.ToList();
            if (sources.Count == 0)
                return;

            var destinations = _settingsService.BackupDestinations.ToList();
            if (destinations.Count == 0) {
                // TODO: Error?
                Logger.Warn($"Attempting to perform backup of {sources.Count} sources, but no destinations are configured. Aborting.");
                return;
            }

            // Include our own config as well
            sources.Add(new BackupSourceEntry {
                Folder = GlobalPaths.CoreFolder,
                Name = "ZipBackup",
                InclusionMask = ".json",
                Recursive = false
            });

            foreach (var source in sources) {
                var tempFileName = Path.GetTempFileName();
                try {
                    Backup(source, tempFileName);
                    foreach (var dest in destinations) {
                        File.Copy(tempFileName, Path.Combine(dest.Folder, Format(source.Name)), true); // TODO: IOException
                    }
                }
                finally {
                    File.Delete(tempFileName);
                }
            }
        }

        private string Format(string name) {
            var suffix = "";
            if (!string.IsNullOrEmpty(_settingsService.FilenamePattern))
                suffix = DateTime.Now.ToString(_settingsService.FilenamePattern);

            return name.Replace(" ", "_") + suffix + ".zip";
        }

        public void Backup(BackupSourceEntry source, string outputFilename) {
            if (!Directory.Exists(source.Folder)) {
                Logger.Warn($"The requested directory {source.Folder} does not exist");
                // TODO: Log, notify?
                return;
            }

            if (File.Exists(outputFilename)) {
                Logger.Info($"The output filename {outputFilename} already exists, overwriting");
                File.Delete(outputFilename);
            }

            List<string> files = new List<string>();
            var depth = source.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filename in Directory.GetFiles(source.Folder, "*.*", depth)) {
                // We don't really want to run exclusions on the input folder, only it's contents.
                var cleanFilename = filename.Substring(source.Folder.Length);
                if (IsIncluded(source, cleanFilename)) {
                    files.Add(filename);
                }
            }

            if (files.Count == 0) {
                if (!string.IsNullOrEmpty(source.InclusionMask))
                    Logger.Info($"Could not find any files in {source.Folder} matching inclusion pattern {source.InclusionMask}");
                else if (!string.IsNullOrEmpty(source.ExclusionMask))
                    Logger.Info($"Could not find any files in {source.Folder} after applying exclusion pattern {source.ExclusionMask}");
                else
                    Logger.Info($"Could not find any files in {source.Folder}");

                // TODO: Err
                return;
            }

            using (ZipFile zip = new ZipFile()) {
                if (!string.IsNullOrEmpty(_settingsService.ZipPassword)) {
                    zip.Password = _settingsService.ZipPassword;
                    zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                }

                foreach (var file in files) {
                    try {
                        zip.AddFile(file, Sanitize(file, source.Folder)); // TODO: Filename becomes folder i think..
                    }
                    catch (IOException) {
                        FileInfo fileInfo = new FileInfo(file);
                        if (fileInfo.Length < 100 * 1024 * 1024) {
                            // Probably a lock on it. CreateEntryFromFile requires a lock, so lets just copy it.
                            var tempFileName = Path.GetTempFileName();
                            File.Copy(file, tempFileName, true);
                            try {
                                zip.AddFile(tempFileName, Sanitize(file, source.Folder)); // TODO: Filename becomes folder i think..
                            }
                            finally {
                                File.Delete(tempFileName);
                            }
                        }
                        else {
                            // Rather large file, not copying > 100 MB
                            throw;
                        }
                    }

                    zip.Save(outputFilename);
                }
            }

            Logger.Debug($"Successfully zipped {files.Count} files to {outputFilename}");
        }

        public bool IsIncluded(BackupSourceEntry source, string filename) {
            if (!string.IsNullOrEmpty(source.InclusionMask)) {
                return Regex.IsMatch(filename, source.InclusionMask, RegexOptions.IgnoreCase);
            } else if (!string.IsNullOrEmpty(source.ExclusionMask)) {
                return !Regex.IsMatch(filename, source.ExclusionMask, RegexOptions.IgnoreCase);
            }

            return true;
        }

        private string Sanitize(string filePath, string prefix) {
            return Path.GetDirectoryName(filePath)
                .Replace(_localAppdata, "%LocalAppData%")
                .Replace(_userProfile, "%UserProfile%")
                .Replace(prefix, new DirectoryInfo(prefix).Name);
        }
    }
}
