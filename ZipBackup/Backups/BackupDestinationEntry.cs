using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipBackup.Backups {
    public class BackupDestinationEntry {
        public string Name { get; set; }
        public string Folder { get; set; }

        public override bool Equals(object? obj) {
            if (obj is BackupDestinationEntry entry) {
                return Folder.Equals(entry.Folder);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return Folder.GetHashCode(StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
