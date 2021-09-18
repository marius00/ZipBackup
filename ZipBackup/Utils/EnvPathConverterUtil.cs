using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipBackup.Utils {
    public static class EnvPathConverterUtil {
        private static readonly string LocalAppdata;
        private static readonly string AppData;
        private static readonly string UserProfile;
        private static readonly string ProgramData;

        static EnvPathConverterUtil() {
            LocalAppdata = System.Environment.GetEnvironmentVariable("LocalAppData");
            AppData = System.Environment.GetEnvironmentVariable("AppData");
            UserProfile = System.Environment.GetEnvironmentVariable("UserProfile");
            ProgramData = System.Environment.GetEnvironmentVariable("ProgramData");
        }

        /// <summary>
        /// Replaces parts of the path with %LocalAppData% and %UserProfile% where possible
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToEnvironmentalPath(string path) {
            return path
                .Replace(LocalAppdata, "%LocalAppData%", StringComparison.InvariantCultureIgnoreCase)
                .Replace(AppData, "%AppData%", StringComparison.InvariantCultureIgnoreCase)
                .Replace(ProgramData, "%ProgramData%", StringComparison.InvariantCultureIgnoreCase)
                .Replace(UserProfile, "%UserProfile%", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Replaces environmentals such as %LocalAppData% with the full path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FromEnvironmentalPath(string path) {
            return path
                .Replace("%LocalAppData%", LocalAppdata, StringComparison.InvariantCultureIgnoreCase)
                .Replace("%AppData%", AppData, StringComparison.InvariantCultureIgnoreCase)
                .Replace("%ProgramData%", ProgramData, StringComparison.InvariantCultureIgnoreCase)
                .Replace("%UserProfile%", UserProfile, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}