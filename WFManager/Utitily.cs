using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WFManager {
    class Utility {
        public static TimeSpan ParseTimeSpan(string timeString) {
            var time = Regex.Replace(timeString, "[^0-9:]", "").Split(':');
            return new TimeSpan(int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2]));
        }

        public static DateTime AssemblyDate {
            get {
                Version v = Assembly.GetEntryAssembly().GetName().Version;
                return new DateTime(2000, 1, 1).AddDays(v.Build).AddSeconds(v.Revision * 2);
            }
        }
    }
}
