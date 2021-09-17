using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace ZipBackup.Services {
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
