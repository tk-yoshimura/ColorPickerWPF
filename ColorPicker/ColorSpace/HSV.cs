using System.Diagnostics;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker.ColorSpace {
    [DebuggerDisplay("{ToString(),nq}")]
    public struct HSV {
        private double h, s, v;

        public HSV(double h, double s, double v) {
            this.h = CyclicRemainder(double.IsNaN(h) ? 0d : h, 6d);
            this.s = double.Clamp(double.IsNaN(s) ? 0d : s, 0d, 1d);
            this.v = double.Clamp(double.IsNaN(v) ? 0d : v, 0d, 1d);
        }

        public double H {
            readonly get {
                return h;
            }
            set {
                h = CyclicRemainder(double.IsNaN(value) ? 0d : value, 6d);
            }
        }

        public double S {
            readonly get {
                return s;
            }
            set {
                s = double.Clamp(double.IsNaN(value) ? 0d : value, 0d, 1d);
            }
        }

        public double V {
            readonly get {
                return v;
            }
            set {
                v = double.Clamp(double.IsNaN(value) ? 0d : value, 0d, 1d);
            }
        }

        public RGB RGB {
            set {
                value = value.Normalize;

                double r = value.R, g = value.G, b = value.B;

                double max_c = double.Max(double.Max(r, g), b);
                double min_c = double.Min(double.Min(r, g), b);

                h = max_c - min_c;
                s = max_c > 0d ? h / max_c : 0d;
                v = max_c;

                if (h > 0) {
                    if (max_c == r) {
                        h = (g - b) / h + (g >= b ? 0d : 6d);
                    }
                    else if (max_c == g) {
                        h = (b - r) / h + 2d;
                    }
                    else {
                        h = (r - g) / h + 4d;
                    }
                }
            }

            readonly get {
                double r = v, g = v, b = v;

                if (s > 0d) {
                    double d = double.Floor(h);
                    double f = h - d;
                    int i = (int)d;

                    switch (i) {
                        case 0:
                            g *= 1d - s * (1d - f);
                            b *= 1d - s;
                            break;
                        case 1:
                            r *= 1d - s * f;
                            b *= 1d - s;
                            break;
                        case 2:
                            r *= 1d - s;
                            b *= 1d - s * (1d - f);
                            break;
                        case 3:
                            r *= 1d - s;
                            g *= 1d - s * f;
                            break;
                        case 4:
                            r *= 1d - s * (1d - f);
                            g *= 1d - s;
                            break;
                        default:
                            g *= 1d - s;
                            b *= 1d - s * f;
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
            if (double.IsNegative(value)) {
                value += cycle;
            }

            return value;
        }
    }
}
