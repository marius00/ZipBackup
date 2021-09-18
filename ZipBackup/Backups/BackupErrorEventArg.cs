using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipBackup.Backups {
    class BackupErrorEventArg : EventArgs {
        public string Component { get; set; }
        public string Content { get; set; }
    }
}
