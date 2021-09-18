using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ionic.Zip;
using log4net;
using ZipBackup.Services;
using ZipBackup.Utils;

namespace ZipBackup.Backups {
    class BackupService {
        static readonly ILog Logger = LogManager.GetLogger(typeof(BackupService));
        private readonly AppSettings _appSettings;

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
            sources.Add(new BackupSourceEntry {
                Folder = GlobalPaths.CoreFolder,
                Name = "ZipBackup",
                InclusionMask = ".json",
                Recursive = false
            });



            var plaintextPassword = _appSettings.ZipPasswordPlaintext; // CPU heavy to read
            if (!string.IsNullOrEmpty(plaintextPassword) && _appSettings.CpuHash != EncryptionUtil.GetCpuSerial().GetHashCode()) {
                Logger.Error("The CPU serial does not match the expected value. The zip encryption password must be reset before continuing");
                // TODO: This could trigger an action which redirects to password config
                ToastUtil.Show("ZipBackup disabled", "The CPU serial hash does not match the current setup.", "Please update the ZIP password under misc settings.");
                return;
            }


            foreach (var source in sources) {
                var tempFileName = Path.GetTempFileName();
                try {
                    Backup(source, tempFileName, plaintextPassword);
                    foreach (var dest in destinations) {
                        File.Copy(tempFileName, Path.Combine(dest.Folder, Format(source.Name)), true); // TODO: IOException
                    }
                }
                finally {
                    File.Delete(tempFileName);
                }
            }
        }

        /// <summary>
        /// Performs a backup on a single source/directory
        /// </summary>
        /// <param name="source"></param>
        /// <param name="outputFilename"></param>
        /// <param name="plaintextPassword"></param>
        public void Backup(BackupSourceEntry source, string outputFilename, string plaintextPassword) {
            string folder = EnvPathConverterUtil.FromEnvironmentalPath(source.Folder);
            if (!Directory.Exists(folder)) {
                Logger.Warn($"The requested directory {folder} does not exist");
                // TODO: Log, notify? -- add to an error queue? Prevents duplicates and can be shown afterwards
                return;
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
                if (!string.IsNullOrEmpty(source.InclusionMask))
                    Logger.Info($"Could not find any files in {folder} matching inclusion pattern {source.InclusionMask}");
                else if (!string.IsNullOrEmpty(source.ExclusionMask))
                    Logger.Info($"Could not find any files in {folder} after applying exclusion pattern {source.ExclusionMask}");
                else
                    Logger.Info($"Could not find any files in {folder}");

                // TODO: Err
                return;
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
                    catch (IOException) {
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
                            // Rather large file, not copying > 100 MB
                            throw;
                        }
                    }

                    zip.Save(outputFilename);
                }
            }

            Logger.Debug($"Successfully zipped {files.Count} files to {outputFilename}");
        }

        /// <summary>
        /// Checks a given filename against the inclusion and exclusion masks.
        /// If an inclusion mask is defined, the exclusion mask is ignored.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool IsIncluded(BackupSourceEntry source, string filename) {
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
