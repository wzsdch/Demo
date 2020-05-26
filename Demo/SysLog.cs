using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Demo
{
    public class SysLog
    {
        public static string LogName = "";
        private static object sync_log = new object();
        public static void WriteLog(string log)
        {
            lock (sync_log)
            {
                string path = Application.StartupPath + "\\log\\" + LogName;
                string datestr = DateTime.Now.ToString("yyyy-MM-dd");
                string logfilename = datestr + ".log";   // "sys.log";
                string fullname = Path.Combine(path, logfilename);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                StreamWriter f = new StreamWriter(fullname, true);
                string str = DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToString("HH:mm:ss.ffff") + "        " + "info:         " + log;
                f.WriteLine(str);
                f.Close();
            }
        }
        public static void WriteError(string log)
        {
            lock (sync_log)
            {
                string path = Application.StartupPath + "\\log\\" + LogName;
                string datestr = DateTime.Now.ToString("yyyy-MM-dd");
                string logfilename = datestr + ".log";   // "sys.log";
                string fullname = Path.Combine(path, logfilename);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                StreamWriter f = new StreamWriter(fullname, true);
                string str = DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToString("HH:mm:ss.ffff") + "        " + "error:        " + log;
                f.WriteLine(str);
                f.Close();
            }
        }
    }
}
