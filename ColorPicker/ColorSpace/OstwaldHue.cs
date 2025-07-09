using System.Collections.ObjectModel;

namespace ColorPicker.ColorSpace {
    public static class OstwaldHue {
        static readonly ReadOnlyCollection<double> table = new([
            0.000, 0.173, 0.325, 0.502, 0.804, 0.902, 1.110, 1.292,
            1.577, 2.087, 2.686, 2.889, 3.092, 3.329, 3.580, 3.861,
            4.043, 4.194, 4.341, 4.680, 5.065, 5.430, 5.672, 5.843,
            6.000,
        ]);

        public static double Value(double h) {
            h = Utils.CyclicArithmetic.Remainder(double.IsFinite(h) ? h : 0d, 6d);

            double h4 = double.ScaleB(h, 2);
            int index = (int)double.Floor(h4);
            double f = h4 - index;

            double u = (1d - f) * table[index] + f * table[index + 1];

            return u;
        }

        public static double InvertValue(double h) {
            h = Utils.CyclicArithmetic.Remainder(double.IsFinite(h) ? h : 0d, 6d);

            int index;
            for (index = 0; index < table.Count - 1; index++) {
                if (h < table[index + 1]) {
                    break;
                }
            }

            if (index >= table.Count - 1) {
                return 6d;
            }

            double a = table[index], b = table[index + 1];
            double f = (h - a) / (b - a);

            double v = double.ScaleB(index + f, -2);

            return v;
        }
    }
}
