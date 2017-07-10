using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFManager {
    class Logger {
        private static StreamWriter infoLogFile;
        private static StreamWriter errorLogFile;

        private static bool msgLocked = false;

        private static Label infoLabel;


        public static void Initialize(Label label) {
            infoLabel = label;
        }


        public static void Info(string s, params object[] args) {
            lock ("InfoLock") {
                // Open file if not opened
                if (infoLogFile == null || infoLogFile.BaseStream == null) {
                    string dir = WF.storagePath;
                    string path = dir + "\\InfoLog.txt";
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    infoLogFile = new StreamWriter(path, true);
                }

                // Write log
                infoLogFile.WriteLine(DateTime.Now.ToString("\n[dd/MM/yyyy HH:mm:ss]") + "\t" + s, args);
                infoLogFile.Flush();
            }
        }

        public static void Error(string s, params object[] args) {
            lock ("ErrorLock") {
                // Open file if not opened
                if (errorLogFile == null || errorLogFile.BaseStream == null) {
                    string dir = WF.storagePath;
                    string path = dir + "\\ErrorLog.txt";
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    errorLogFile = new StreamWriter(path, true);
                }

                // Write log
                errorLogFile.WriteLine(DateTime.Now.ToString("\n[dd/MM/yyyy HH:mm:ss]") + "\t" + s, args);
                errorLogFile.Flush();

                alert(s);
            }
        }

        private static void alert(string msg) {
            if (!msgLocked) {
                msgLocked = true;
                new Thread((m) => _alert(m)).Start(msg);
            }
        }
        private static void _alert(object msg) {
            MessageBox.Show((string)msg);
            msgLocked = false;
        }

        
        public static void Label(string s, params object[] args) {
            infoLabel.Text = string.Format(s, args);
        }
    }
}
