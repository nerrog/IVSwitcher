using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace IVSwitcher
{
    class IVLogger
    {

        private static void OutLog(string loglevel,string logvalue,string file, int callline)
        {
            //ログ出力
            Encoding enc = Encoding.GetEncoding("utf-8");

            string logpath = AppDomain.CurrentDomain.BaseDirectory + "IV Switcher_log.txt";

            bool logappend;

            if (File.Exists(logpath))
            {
                logappend = true;
            }
            else
            {
                logappend = false;
            }

            StreamWriter writer = new StreamWriter(logpath, logappend, enc);

            DateTime dt = DateTime.Now;

            writer.WriteLine("["+loglevel+"]"+"["+ dt.ToString("yyyy/MM/dd HH:mm:ss")+"]"+"["+file+" "+callline+"]"+logvalue);

            writer.Close();
        }

        //各ソースから呼び出す用のメソッド

        public static void info(string logvalue, [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            OutLog("INFO",logvalue, Path.GetFileName(sourceFilePath), lineNumber);
        }

        public static void warn(string logvalue, [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            OutLog("WANING", logvalue, Path.GetFileName(sourceFilePath), lineNumber);
        }

        public static void error(string logvalue, [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            OutLog("ERROR", logvalue, Path.GetFileName(sourceFilePath), lineNumber);
        }

        public static void debug(string logvalue, [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            OutLog("DEBUG", logvalue, Path.GetFileName(sourceFilePath), lineNumber);
        }

        public static void logger_init()
        {
            //ログ初期化用
            string logpath = AppDomain.CurrentDomain.BaseDirectory + "IV Switcher_log.txt";

            if (File.Exists(logpath))
            {
                File.Delete(logpath);
            }
            var dt = File.GetLastWriteTimeUtc(Assembly.GetExecutingAssembly().Location);
            dt += new TimeSpan(9, 0, 0);

            OutLog("INFO", "IV Switcher Info:\n"+System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location), "IVLogger.cs", -1);
            OutLog("INFO", "Build Date:"+dt, "IVLogger.cs", -1);
            OutLog("INFO", "Runtime build:" + Environment.Version.ToString(), "IVLogger.cs", -1);

        }
    }
}
