namespace ColorPicker.Utils {
    public static class CyclicArithmetic {

        public static double Remainder(double value, double cycle) {
            value %= cycle;

            if (double.IsNegative(value)) {
                if (value == 0d) {
                    return 0d;
                }

                value += cycle;
            }

            return value;
        }
    }
}
