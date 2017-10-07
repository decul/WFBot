using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WFManager {
    class Util {
        public static TimeSpan ParseTimeSpan(string timeString) {
            var time = Regex.Match(timeString, "[0-9]+:[0-9:]+").Value.Split(':');
            return new TimeSpan(int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2]));
        }


        public static double ParsePrice(string priceString) {
            string price = Regex.Match(priceString, "[0-9,]+").Value.Replace(',', '.');
            return double.Parse(price, NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        public static int ParseProductId(string idString) {
            var match = Regex.Match(idString, "kp[0-9]+");
            if (match.Success)
                return int.Parse(match.Value.Substring(2));

            match = Regex.Match(idString, "f_m_symbol[0-9]+");
            if (match.Success)
                return 10000 + int.Parse(match.Value.Substring(10));

            throw new ArgumentException("String does not contain valid product id");
        }



        public static DateTime AssemblyDate {
            get {
                Version v = Assembly.GetEntryAssembly().GetName().Version;
                return new DateTime(2000, 1, 1).AddDays(v.Build).AddSeconds(v.Revision * 2);
            }
        }
    }
}
