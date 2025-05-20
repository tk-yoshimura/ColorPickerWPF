using System.Diagnostics;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker.ColorSpace {
    [DebuggerDisplay("{ToString(),nq}")]
    public struct HSV {
        private double h, s, v;

        public HSV(double h, double s, double v) {
            this.h = CyclicRemainder(double.IsNaN(h) ? 0 : h, 6);
            this.s = double.Clamp(double.IsNaN(s) ? 0 : s, 0, 1);
            this.v = double.Clamp(double.IsNaN(v) ? 0 : v, 0, 1);
        }

        public double H {
            readonly get {
                return h;
            }
            set {
                h = CyclicRemainder(double.IsNaN(value) ? 0 : value, 6);
            }
        }

        public double S {
            readonly get {
                return s;
            }
            set {
                s = double.Clamp(double.IsNaN(value) ? 0 : value, 0, 1);
            }
        }

        public double V {
            readonly get {
                return v;
            }
            set {
                v = double.Clamp(double.IsNaN(value) ? 0 : value, 0, 1);
            }
        }

        public RGB RGB {
            set {
                value = value.Normalize;

                double r = value.R, g = value.G, b = value.B;

                double max_c = double.Max(double.Max(r, g), b);
                double min_c = double.Min(double.Min(r, g), b);

                h = max_c - min_c;
                s = max_c > 0 ? h / max_c : 0;
                v = max_c;

                if (h > 0) {
                    if (max_c == r) {
                        h = (g - b) / h + (g >= b ? 0.0 : 6.0);
                    }
                    else if (max_c == g) {
                        h = (b - r) / h + 2.0;
                    }
                    else {
                        h = (r - g) / h + 4.0;
                    }
                }
            }

            readonly get {
                double r = v, g = v, b = v;

                if (s > 0) {
                    double d = double.Floor(h);
                    double f = h - d;
                    int i = (int)d;

                    switch (i) {
                        case 0:
                            g *= 1 - s * (1 - f);
                            b *= 1 - s;
                            break;
                        case 1:
                            r *= 1 - s * f;
                            b *= 1 - s;
                            break;
                        case 2:
                            r *= 1 - s;
                            b *= 1 - s * (1 - f);
                            break;
                        case 3:
                            r *= 1 - s;
                            g *= 1 - s * f;
                            break;
                        case 4:
                            r *= 1 - s * (1 - f);
                            g *= 1 - s;
                            break;
                        default:
                            g *= 1 - s;
                            b *= 1 - s * f;
                            break;
                    }
                }

                return new RGB(r, g, b);
            }
        }

        public static implicit operator RGB(HSV hsv) {
            return hsv.RGB;
        }

        public static implicit operator HSV((double h, double s, double v) cr) {
            return new(cr.h, cr.s, cr.v);
        }

        public static implicit operator (double h, double s, double v)(HSV cr) {
            return (cr.H, cr.S, cr.V);
        }

        public readonly void Deconstruct(out double h, out double s, out double v) {
            h = H;
            s = S;
            v = V;
        }

        public static bool operator ==(HSV cr1, HSV cr2) {
            return cr1.H == cr2.H && cr1.S == cr2.S && cr1.V == cr2.V;
        }

        public static bool operator !=(HSV cr1, HSV cr2) {
            return !(cr1 == cr2);
        }

        public override readonly bool Equals(object obj) {
            return (obj is HSV cr) && (this == cr);
        }

        public override readonly int GetHashCode() {
            return H.GetHashCode() ^ S.GetHashCode() ^ V.GetHashCode();
        }

        public override readonly string ToString() {
            return $"h={H:0.000} s={S:0.000} v={V:0.000}";
        }

        private static double CyclicRemainder(double value, double cycle) {
            value %= cycle;
            if (value < 0) {
                value += cycle;
            }

            return value;
        }
    }
}
