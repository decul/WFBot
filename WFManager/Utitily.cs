using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WFManager {
    class Utility {
        public static TimeSpan ParseTimeSpan(string timeString) {
            var time = Regex.Replace(timeString, "[^0-9:]", "").Split(':');
            return new TimeSpan(int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2]));
        }
    }
}
