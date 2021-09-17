using System.IO;

namespace ZipBackup.Utils {
    public static class GlobalPaths {
        private static string LocalAppdata {
            get {
                string appdata = System.Environment.GetEnvironmentVariable("LocalAppData");
                if (string.IsNullOrEmpty(appdata))
                    return Path.Combine(System.Environment.GetEnvironmentVariable("AppData"), "..", "local");
                else
                    return appdata;
            }
        }

        public static string CoreFolder {
            get {
                string path = Path.Combine(LocalAppdata, "EvilSoft", "ZipBackup");
                Directory.CreateDirectory(path);

                return path;
            }
        }

        public static string SettingsFile => Path.Combine(CoreFolder, "settings.json");
    }
}
