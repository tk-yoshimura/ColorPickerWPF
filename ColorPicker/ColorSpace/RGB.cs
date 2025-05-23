﻿// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

using System.Diagnostics;
using System.Windows.Media;

namespace ColorPicker.ColorSpace {
    [DebuggerDisplay("{ToString(),nq}")]
    public struct RGB {
        public RGB(double r, double g, double b) {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public double R { set; get; }

        public double G { set; get; }

        public double B { set; get; }

        public static explicit operator Color(RGB rgb) {
            Color color = Color.FromRgb(
                (byte)double.Clamp(rgb.R * 255d + 0.5, 0d, 255d),
                (byte)double.Clamp(rgb.G * 255d + 0.5, 0d, 255d),
                (byte)double.Clamp(rgb.B * 255d + 0.5, 0d, 255d)
            );

            return color;
        }

        public static explicit operator RGB(Color color) {
            RGB rgb = new(
                color.R / 255d,
                color.G / 255d,
                color.B / 255d
            );

            return rgb;
        }

        public static implicit operator HSV(RGB rgb) {
            HSV hsv = new() {
                RGB = rgb
            };

            return hsv;
        }

        public static implicit operator YCbCr(RGB rgb) {
            YCbCr ycbcr = new() {
                RGB = rgb
            };

            return ycbcr;
        }

        public static implicit operator RGB((double r, double g, double b) cr) {
            return new(cr.r, cr.g, cr.b);
        }

        public static implicit operator (double r, double g, double b)(RGB cr) {
            return (cr.R, cr.G, cr.B);
        }

        public readonly void Deconstruct(out double r, out double g, out double b) {
            r = R;
            g = G;
            b = B;
        }

        public static bool operator ==(RGB cr1, RGB cr2) {
            return cr1.R == cr2.R && cr1.G == cr2.G && cr1.B == cr2.B;
        }

        public static bool operator !=(RGB cr1, RGB cr2) {
            return !(cr1 == cr2);
        }

        public static RGB operator +(RGB cr) {
            return cr;
        }

        public static RGB operator -(RGB cr) {
            return new(-cr.R, -cr.G, -cr.B);
        }

        public static RGB operator +(RGB cr1, RGB cr2) {
            return new(cr1.R + cr2.R, cr1.G + cr2.G, cr1.B + cr2.B);
        }

        public static RGB operator -(RGB cr1, RGB cr2) {
            return new(cr1.R - cr2.R, cr1.G - cr2.G, cr1.B - cr2.B);
        }

        public static RGB operator *(RGB cr, double r) {
            return new(cr.R * r, cr.G * r, cr.B * r);
        }

        public static RGB operator *(double r, RGB cr) {
            return cr * r;
        }

        public static RGB operator /(RGB cr, double r) {
            return new(cr.R / r, cr.G / r, cr.B / r);
        }

        public readonly RGB Normalize =>
            new(
                double.Clamp(R, 0d, 1d),
                double.Clamp(G, 0d, 1d),
                double.Clamp(B, 0d, 1d)
            );

        public override readonly bool Equals(object obj) {
            return (obj is RGB cr) && (this == cr);
        }

        public override readonly int GetHashCode() {
            return R.GetHashCode() ^ G.GetHashCode() ^ B.GetHashCode();
        }

        public override readonly string ToString() {
            return $"r={R:0.000} g={G:0.000} b={B:0.000}";
        }
    }
}
