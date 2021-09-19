using System;
using System.IO;
using System.Windows.Forms;
using log4net;
using Microsoft.Win32;

namespace ZipBackup.Utils {
    static class StartupRegistrationService {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(StartupRegistrationService));

        public static bool Uninstall(string name) {
            using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", RegistryKeyPermissionCheck.ReadWriteSubTree)) {
                if (registryKey == null) {
                    Logger.Warn(@"Could not find CurrentVersion\Run...");
                    return false;
                }

                try {
                    registryKey.DeleteValue(name);
                    return true;
                } catch (Exception) {
                    return false;
                }
            }
        }

        public static bool IsInstalled(string name) {
            using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run")) {
                if (registryKey == null) {
                    Logger.Warn(@"Could not find CurrentVersion\Run...");
                    return false;
                }

                var value = registryKey.GetValue(name);
                var fullPath = Path.Combine(Application.StartupPath, Application.ExecutablePath);
                return fullPath.Equals(value);
            }
        }
        
        public static bool Install(string name) {
            using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", RegistryKeyPermissionCheck.ReadWriteSubTree)) {
                if (registryKey == null) {
                    Logger.Warn(@"Could not find CurrentVersion\Run...");
                    return false;
                }

                var fullPath = Path.Combine(Application.StartupPath, Application.ExecutablePath);
                try {
                    registryKey.SetValue(name, fullPath);
                    return true;
                }
                catch (Exception ex) {
                    Logger.Warn(ex.Message, ex);
                    return false;
                }
            }
        }
    }
}
