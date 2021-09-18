using System;
using System.IO;
using Microsoft.Toolkit.Uwp.Notifications;

namespace ZipBackup.Utils {
    static class ToastUtil {
        public static void Show(params string[] content) {
            var builder = new ToastContentBuilder();

            foreach (var body in content) {
                builder.AddText(body);
            }

            // Only add the logo if we cant find it. If the working directory has changed for any reason, this will fail.
            var logo = Path.Combine(Directory.GetCurrentDirectory(), "backup.png");
            if (File.Exists(logo)) {
                builder.AddAppLogoOverride(new Uri($"file://{logo}"), ToastGenericAppLogoCrop.Circle);
            }

            builder.Show();
                
        }
    }
}
