﻿#nullable enable
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipBackup.Backups {
    class BackupSourceEntry : IComparable {
        public string Name { get; set; }
        public string Folder { get; set; }

        public bool Recursive { get; set; } = true;

        /// <summary>
        /// Patterns to exclude. Regex.
        /// </summary>
        public string ExclusionMask { get; set; } = "node_modules|build|bin\\\\debug|bin\\\\release";

        /// <summary>
        /// Patterns to include. Regex.
        /// If this is set, only files matching this pattern will be set.
        /// </summary>
        public string InclusionMask { get; set; } = "";

        public CompressionLevel Compression { get; set; } = CompressionLevel.Fastest;

        public int CompareTo(object? obj) {
            if (obj is BackupSourceEntry entry) {
                return String.Compare(Folder, entry.Folder, StringComparison.InvariantCultureIgnoreCase);
            }

            return 0;
        }

        public override bool Equals(object? obj) {
            if (obj is BackupSourceEntry entry) {
                return Folder.Equals(entry.Folder);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return Folder.GetHashCode(StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
