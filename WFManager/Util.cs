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
            int prefix = 0;

            var match = Regex.Match(idString, "(kp|e)[0-9]+");
            if (!match.Success) {
                prefix = 10000;
                match = Regex.Match(idString, "f_(m_)?symbol[0-9]+");
                if (!match.Success)
                    throw new ArgumentException("String does not contain valid product id");
            }

             return prefix + int.Parse(Regex.Match(match.Value, "[0-9]+").Value);
        }



        public static DateTime AssemblyDate {
            get {
                Version v = Assembly.GetEntryAssembly().GetName().Version;
                return new DateTime(2000, 1, 1).AddDays(v.Build).AddSeconds(v.Revision * 2);
            }
        }
    }
}
