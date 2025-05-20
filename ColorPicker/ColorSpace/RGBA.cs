// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

using System.Diagnostics;
using System.Windows.Media;

namespace ColorPicker.ColorSpace {
    [DebuggerDisplay("{ToString(),nq}")]
    public struct RGBA {
        public RGBA(double r, double g, double b, double a) {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        public RGBA(RGB rgb, double alpha) {
            this.R = rgb.R;
            this.G = rgb.G;
            this.B = rgb.B;
            this.A = alpha;
        }

        public double R { set; get; }

        public double G { set; get; }

        public double B { set; get; }

        public double A { set; get; }


        public static explicit operator Color(RGBA rgb) {
            Color color = Color.FromArgb(
                (byte)double.Clamp(rgb.A * 255d + 0.5d, 0d, 255d),
                (byte)double.Clamp(rgb.R * 255d + 0.5d, 0d, 255d),
                (byte)double.Clamp(rgb.G * 255d + 0.5d, 0d, 255d),
                (byte)double.Clamp(rgb.B * 255d + 0.5d, 0d, 255d)
            );

            return color;
        }

        public static explicit operator RGBA(Color color) {
            RGBA rgb = new(
                color.R / 255d,
                color.G / 255d,
                color.B / 255d,
                color.A / 255d
            );

            return rgb;
        }

        public static implicit operator RGBA((double r, double g, double b, double a) cr) {
            return new(cr.r, cr.g, cr.b, cr.a);
        }

        public static implicit operator (double r, double g, double b, double a)(RGBA cr) {
            return (cr.R, cr.G, cr.B, cr.A);
        }

        public static implicit operator RGBA((RGB rgb, double alpha) cr) {
            return new(cr.rgb.R, cr.rgb.G, cr.rgb.B, cr.alpha);
        }

        public static implicit operator (RGB rgb, double a)(RGBA cr) {
            return (new RGB(cr.R, cr.G, cr.B), cr.A);
        }

        public readonly void Deconstruct(out double r, out double g, out double b, out double a) {
            r = R;
            g = G;
            b = B;
            a = A;
        }

        public readonly void Deconstruct(out RGB rgb, out double alpha) {
            rgb = new RGB(R, G, B);
            alpha = A;
        }

        public static bool operator ==(RGBA cr1, RGBA cr2) {
            return cr1.R == cr2.R && cr1.G == cr2.G && cr1.B == cr2.B && cr1.A == cr2.A;
        }

        public static bool operator !=(RGBA cr1, RGBA cr2) {
            return !(cr1 == cr2);
        }

        public readonly RGBA Normalize =>
            new(
                double.Clamp(R, 0d, 1d),
                double.Clamp(G, 0d, 1d),
                double.Clamp(B, 0d, 1d),
                double.Clamp(A, 0d, 1d)
            );

        public override readonly bool Equals(object obj) {
            return (obj is RGBA cr) && (this == cr);
        }

        public override readonly int GetHashCode() {
            return R.GetHashCode() ^ G.GetHashCode() ^ B.GetHashCode() ^ A.GetHashCode();
        }

        public override readonly string ToString() {
            return $"r={R:0.000} g={G:0.000} b={B:0.000} a={A:0.000}";
        }
    }
}
