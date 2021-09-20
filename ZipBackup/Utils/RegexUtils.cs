using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZipBackup.Settings;

namespace ZipBackup.Utils {
    static class RegexUtils {
        public static bool IsValidRegex(string expression) {
            try {
                Regex.IsMatch("abcc", expression, RegexOptions.IgnoreCase);
                return true;
            } catch (RegexParseException) {
                return false;
            }
        }

        public static string ApplyAppSettings(string regex, AppSettings appSettings) {
            return regex.Replace("[default]", appSettings.DefaultExclusionPattern);
        }
    }
}
