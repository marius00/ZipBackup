using System;
using log4net;

namespace ZipBackup.Utils {
    class ExceptionHandler {
        static readonly ILog Logger = LogManager.GetLogger(typeof(ExceptionHandler));

        public static void EnableLogUnhandledOnThread() {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
        }

        private static void MyHandler(object sender, UnhandledExceptionEventArgs args) {
            Exception e = (Exception)args.ExceptionObject;
            Logger.Fatal(e.Message);
            Logger.Fatal(e.StackTrace);
        }
    }
}
