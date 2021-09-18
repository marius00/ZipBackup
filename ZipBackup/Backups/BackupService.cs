using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Input;
using Ionic.Zip;
using log4net;
using ZipBackup.Services;
using ZipBackup.Utils;


namespace ZipBackup.Backups {
    public class BackupService {
        static readonly ILog Logger = LogManager.GetLogger(typeof(BackupService));
        private readonly AppSettings _appSettings;
        public event EventHandler OnError;

        public BackupService(AppSettings appSettings) {
            _appSettings = appSettings;
        }

        /// <summary>
        /// Attempts to perform a backup for every single source entry, and store at each destination folder.
        /// </summary>
        public void Backup() {
            var sources = _appSettings.BackupSources.ToList();
            if (sources.Count == 0)
                return;

            var destinations = _appSettings.BackupDestinations.ToList();
            if (destinations.Count == 0) {
                Logger.Warn($"Attempting to perform backup of {sources.Count} sources, but no destinations are configured. Aborting.");
                return;
            }

            // Include our own config as well
            if (sources.All(src => src.Folder != GlobalPaths.CoreFolder)) {
                _appSettings.AddBackupSource(new BackupSourceEntry {
                    Folder = GlobalPaths.CoreFolder,
                    Name = "ZipBackup",
                    InclusionMask = ".json",
                    Recursive = false,
                    ExclusionMask = ""
                });

                sources = _appSettings.BackupSources.ToList();
            }



            var plaintextPassword = _appSettings.ZipPasswordPlaintext; // CPU heavy to read
            if (!string.IsNullOrEmpty(plaintextPassword) && _appSettings.CpuHash != EncryptionUtil.GetDeterministicHashCode(EncryptionUtil.GetCpuSerial())) {
                Logger.Error("The CPU serial does not match the expected value. The zip encryption password must be reset before continuing");
                Logger.Error($"Got {_appSettings.CpuHash} expected {EncryptionUtil.GetDeterministicHashCode(EncryptionUtil.GetCpuSerial())}");
                // TODO: This could trigger an action which redirects to password config
                OnError?.Invoke(this, new BackupErrorEventArg {
                    Component = "Core",
                    Content = "ZipBackup disabled\nThe CPU serial hash does not match the current setup.\nPlease update the ZIP password under misc settings."
                });

                return;
            }

            // Backup each source folder
            foreach (var source in sources) {
                if (CanBackup(source)) {
                    var tempFileName = Path.GetTempFileName();
                    try {
                        if (Backup(source, tempFileName, plaintextPassword)) {
                            foreach (var dest in destinations) {
                                File.Copy(tempFileName, Path.Combine(dest.Folder, Format(source.Name)), true); // TODO: IOException
                            }

                            // Success: No more errors.
                            source.Errors.Clear();
                            source.NextUpdate = DateTime.UtcNow.AddHours(_appSettings.BackupIntervalHours).ToTimestamp();
                        }
                        else {
                            // Retry in 45 minutes
                            source.NextUpdate = DateTime.UtcNow.AddMinutes(45).ToTimestamp();
                            if (source.Errors.Count >= _appSettings.ErrorThreshold) {
                                OnError?.Invoke(this, new BackupErrorEventArg {
                                    Component = source.Name,
                                    Content = $"Error backing up {source.Name}\n{string.Join("\n", source.Errors)}"
                                });
                            }
                        }

                    }
                    finally {
                        File.Delete(tempFileName);
                    }
                }
            }

            _appSettings.BackupSourcesHasMutated();
        }

        /// <summary>
        /// Checks if the source has been backed up on the past X hours, as defined in the AppSettings
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private bool CanBackup(BackupSourceEntry entry) {
            var nextUpdateTime = DateTimeEpochExtension.FromTimestamp(entry.NextUpdate);
            return DateTime.UtcNow > nextUpdateTime;
        }

        /// <summary>
        /// Performs a backup on a single source/directory
        /// </summary>
        /// <param name="source"></param>
        /// <param name="outputFilename"></param>
        /// <param name="plaintextPassword"></param>
        public bool Backup(BackupSourceEntry source, string outputFilename, string plaintextPassword) {
            string folder = EnvPathConverterUtil.FromEnvironmentalPath(source.Folder);
            if (!Directory.Exists(folder)) {
                Logger.Warn($"The requested directory {folder} does not exist");
                // TODO: Log, notify? -- add to an error queue? Prevents duplicates and can be shown afterwards
                return false;
            }


            if (File.Exists(outputFilename)) {
                Logger.Info($"The output filename {outputFilename} already exists, overwriting");
                File.Delete(outputFilename);
            }

            List<string> files = new List<string>();
            var depth = source.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filename in Directory.GetFiles(folder, "*.*", depth)) {
                // We don't really want to run exclusions on the input folder, only it's contents.
                var cleanFilename = filename.Substring(folder.Length);
                if (IsIncluded(source, cleanFilename)) {
                    files.Add(filename);
                }
            }

            if (files.Count == 0) {
                if (!string.IsNullOrEmpty(source.InclusionMask)) {
                    Logger.Info($"Could not find any files in {folder} matching inclusion pattern {source.InclusionMask}");
                }
                else if (!string.IsNullOrEmpty(source.ExclusionMask)) {
                    Logger.Info($"Could not find any files in {folder} after applying exclusion pattern {source.ExclusionMask}");
                }
                else {
                    Logger.Info($"Could not find any files in {folder}");
                }

                source.Errors.Add($"No files found\nCould not find any files to backup for {source.Name}.");
                return false;
            }

  

            using (ZipFile zip = new ZipFile()) {
                if (!string.IsNullOrEmpty(plaintextPassword)) {
                    zip.Password = plaintextPassword; ;
                    zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                }

                foreach (var file in files) {
                    try {
                        zip.AddFile(file, Sanitize(file, folder));
                    }
                    catch (IOException ex) {
                        FileInfo fileInfo = new FileInfo(file);
                        if (fileInfo.Length < 100 * 1024 * 1024) {
                            // Probably a lock on it. CreateEntryFromFile requires a lock, so lets just copy it.
                            var tempFileName = Path.GetTempFileName();
                            File.Copy(file, tempFileName, true);
                            try {
                                zip.AddFile(tempFileName, Sanitize(file, folder));
                            }
                            finally {
                                File.Delete(tempFileName);
                            }
                        }
                        else {
                            source.Errors.Add($"Unknown error backing up {source.Name}\n{ex.Message}");
                            // Rather large file, not copying > 100 MB
                        }
                    }

                    zip.Save(outputFilename);
                }
            }

            Logger.Debug($"Successfully zipped {files.Count} files to {outputFilename}");
            return true;
        }

        /// <summary>
        /// Checks a given filename against the inclusion and exclusion masks.
        /// If an inclusion mask is defined, the exclusion mask is ignored.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        private bool IsIncluded(BackupSourceEntry source, string filename) {
            if (!string.IsNullOrEmpty(source.InclusionMask)) {
                return Regex.IsMatch(filename, source.InclusionMask, RegexOptions.IgnoreCase);
            } else if (!string.IsNullOrEmpty(source.ExclusionMask)) {
                return !Regex.IsMatch(filename, source.ExclusionMask, RegexOptions.IgnoreCase);
            }

            return true;
        }

        /// <summary>
        /// Sanitizes the file path stored inside the zip file.
        /// Attempts to replace appdata with %appdata% and failing all, stores the root folder only.
        /// This prevents having zip files with paths such as c:\my\folder\is\very\long\with\one\file.txt
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private string Sanitize(string filePath, string prefix) {
            return EnvPathConverterUtil.ToEnvironmentalPath(Path.GetDirectoryName(filePath))
                .Replace(prefix, new DirectoryInfo(prefix).Name);
        }

        /// <summary>
        /// Format filename according to date pattern (ignored if empty)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string Format(string name) {
            var suffix = "";
            if (!string.IsNullOrEmpty(_appSettings.FilenamePattern))
                suffix = DateTime.Now.ToString(_appSettings.FilenamePattern);

            return name.Replace(" ", "_") + suffix + ".zip";
        }
    }
}
