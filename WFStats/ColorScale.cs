﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFManager {
    class ColorScale {

        public static Color getColor(double value, double minValue, double maxValue) {
            double Delta = maxValue - minValue;
            if (Delta == 0.0)
                return Color.FromArgb(255, 255, 255);

            double delta = value - minValue;
            double h = delta / Delta * 0.6666 - 0.3333;
            if (h < 0.0)
                h += 1.0;
            return ColorFromHSL(h, 1.0, 0.7);
        }


        public static Color ColorFromHSL(double h, double s, double l) {
            double r = 0, g = 0, b = 0;
            if (l != 0) {
                if (s == 0)
                    r = g = b = l;
                else {
                    double temp2;
                    if (l < 0.5)
                        temp2 = l * (1.0 + s);
                    else
                        temp2 = l + s - (l * s);

                    double temp1 = 2.0 * l - temp2;

                    r = GetColorComponent(temp1, temp2, h + 1.0 / 3.0);
                    g = GetColorComponent(temp1, temp2, h);
                    b = GetColorComponent(temp1, temp2, h - 1.0 / 3.0);
                }
            }
            return Color.FromArgb((int)(255 * r), (int)(255 * g), (int)(255 * b));
        }

        private static double GetColorComponent(double temp1, double temp2, double temp3) {
            if (temp3 < 0.0)
                temp3 += 1.0;
            else if (temp3 > 1.0)
                temp3 -= 1.0;

            if (temp3 < 1.0 / 6.0)
                return temp1 + (temp2 - temp1) * 6.0 * temp3;
            else if (temp3 < 0.5)
                return temp2;
            else if (temp3 < 2.0 / 3.0)
                return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
            else
                return temp1;
        }
    }
}