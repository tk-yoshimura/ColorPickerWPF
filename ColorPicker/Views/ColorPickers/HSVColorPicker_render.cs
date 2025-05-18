using ColorPicker.ColorSpace;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public partial class HSVColorPicker {
        const double outer_ring = 0.995;
        const double inner_ring = 0.85;
        const double triangle_vertex = 0.81;

        private void RenderRing() {
            if (!IsValidSize) {
                return;
            }

            int size = PixelSize;
            double radius = size * 0.5;

            double outer_thr_sq = radius * radius * outer_ring * outer_ring;
            double inner_thr_sq = radius * radius * inner_ring * inner_ring;
            double norm_rate = 0.25 / double.Sqrt(inner_thr_sq);

            byte[] buf = new byte[checked(size * size * 4)];

            unsafe {
                fixed (byte* c = buf) {
                    for (int x, y = 0, i = 0; y < size; y++) {
                        double dy = y - radius;
                        for (x = 0; x < size; x++, i += 4) {
                            double dx = x - radius;
                            double norm_sq = dx * dx + dy * dy;

                            if (norm_sq > outer_thr_sq || inner_thr_sq > norm_sq) {
                                continue;
                            }

                            double alpha = double.Min(1, norm_rate * double.Min(outer_thr_sq - norm_sq, norm_sq - inner_thr_sq));

                            if (alpha <= 0.01) {
                                continue;
                            }

                            double hue = (double.Atan2Pi(-dx, dy) + 1d) * 3d;

                            double r = 0d, g = 0d, b = 0d;

                            if (hue < 1d) {
                                r = 1d;
                                g = hue - 0d;
                            }
                            else if (hue < 2d) {
                                r = 2d - hue;
                                g = 1;
                            }
                            else if (hue < 3d) {
                                g = 1d;
                                b = hue - 2d;
                            }
                            else if (hue < 4d) {
                                g = 4d - hue;
                                b = 1;
                            }
                            else if (hue < 5d) {
                                r = hue - 4d;
                                b = 1d;
                            }
                            else {
                                r = 1d;
                                b = 6d - hue;
                            }

                            c[i] = (byte)(b * 255 + 0.5);
                            c[i + 1] = (byte)(g * 255 + 0.5);
                            c[i + 2] = (byte)(r * 255 + 0.5);
                            c[i + 3] = (byte)(alpha * 255);

                            Debug.Assert(i + 3 < buf.Length);
                        }
                    }
                }
            }

            PixelFormat pixel_format = PixelFormats.Pbgra32;
            int stride = checked(size * pixel_format.BitsPerPixel + 7) / 8;
            (double dpi_x, double dpi_y) = Utils.ColorPickerUtil.GetVisualDPI(this);

            BitmapSource bitmap = BitmapSource.Create(size, size, dpi_x, dpi_y, pixel_format, null, buf, stride);

            bitmap.Freeze();

            Image_Ring.Source = bitmap;

            OnPropertyChanged(nameof(Image_Ring));

            Debug.WriteLine($"{nameof(HSVColorPicker)} - {nameof(RenderRing)}");
        }

        private void RenderTriangle(HSV color) {
            if (!IsValidSize) {
                return;
            }

            const double bias = 2d;

            int size = PixelSize;
            double radius = size * 0.5;

            double sqrt3 = double.Sqrt(3), sqrt3_inv = 1 / sqrt3;

            double side = radius * triangle_vertex * sqrt3;
            double side_inv = 1 / side;
            double x0 = radius - side * 0.5;
            double y0 = radius + radius * triangle_vertex * 0.5;
            double t = 2d / sqrt3;

            byte[] buf = new byte[checked(size * size * 4)];

            static double threshold(double s, double l, double bias) {
                return double.Min(double.Max(s + bias, 0), double.Max(l - s + bias, 0));
            }

            double d = double.Floor(color.H), f = color.H - d;
            int cr_i = (int)d;

            unsafe {
                double r, g, b;
                double* cv, cs, cf;

                switch (cr_i) {
                    case 0:
                        cv = &r;
                        cf = &g;
                        cs = &b;
                        f = (1 - f);
                        break;
                    case 1:
                        cf = &r;
                        cv = &g;
                        cs = &b;
                        break;
                    case 2:
                        cs = &r;
                        cv = &g;
                        cf = &b;
                        f = (1 - f);
                        break;
                    case 3:
                        cs = &r;
                        cf = &g;
                        cv = &b;
                        break;
                    case 4:
                        cf = &r;
                        cs = &g;
                        cv = &b;
                        f = (1 - f);
                        break;
                    default:
                        cv = &r;
                        cs = &g;
                        cf = &b;
                        break;
                }

                fixed (byte* c = buf) {
                    for (int x, y = 0, i = 0; y < size; y++) {
                        double dy = (y0 - y) * t, v = double.Clamp(dy * side_inv, 0, 1);
                        double va = threshold(dy, side, bias);

                        if (va < 0.01) {
                            i += size * 4;
                            continue;
                        }

                        int sx = int.Clamp((int)double.Floor(x0 - bias + dy * 0.5), 0, size - 1);
                        i += sx * 4;

                        for (x = sx; x < size; x++, i += 4) {
                            double dx = x - dy * 0.5 - x0, u = double.Clamp(dx * side_inv, 0, 1);
                            double ua = threshold(dx, side - dy, bias);

                            double a = double.Min(va, ua);
                            if (a <= 0.01) {
                                continue;
                            }

                            double alpha = double.Min(1, a);

                            (double sat, double val) = TriangleCoordToSV(u, v);

                            r = g = b = val;
                            *cs *= 1 - sat;
                            *cf *= 1 - sat * f;

                            c[i] = (byte)(b * 255 + 0.5);
                            c[i + 1] = (byte)(g * 255 + 0.5);
                            c[i + 2] = (byte)(r * 255 + 0.5);
                            c[i + 3] = (byte)(alpha * 255);

                            Debug.Assert(i + 3 < buf.Length);

                            if (u + v >= 1.05) {
                                i += (size - x) * 4;
                                break;
                            }
                        }
                    }
                }
            }

            PixelFormat pixel_format = PixelFormats.Pbgra32;
            int stride = checked(size * pixel_format.BitsPerPixel + 7) / 8;
            (double dpi_x, double dpi_y) = Utils.ColorPickerUtil.GetVisualDPI(this);

            BitmapSource bitmap = BitmapSource.Create(size, size, dpi_x, dpi_y, pixel_format, null, buf, stride);

            bitmap.Freeze();

            Image_Triangle.Source = bitmap;

            OnPropertyChanged(nameof(Image_Triangle));

            Debug.WriteLine($"{nameof(HSVColorPicker)} - {nameof(RenderTriangle)}");
        }

        private void RenderPointer(HSV color) {
            if (!IsValidSize) {
                return;
            }

            int size = PixelSize;
            double radius = size * 0.5;

            (double dpi_x, double dpi_y) = Utils.ColorPickerUtil.GetVisualDPI(this);

            RenderTargetBitmap bitmap = new(size, size, dpi_x, dpi_y, PixelFormats.Pbgra32);

            (double h, double s, double v) = color;

            double ring_radius = radius * (outer_ring + inner_ring) / 2;

            DrawingVisual visual = new();

            Pen pen_white = new(new SolidColorBrush(Colors.White), 1);
            Pen pen_black = new(new SolidColorBrush(Colors.Black), 1);

            double sqrt3 = double.Sqrt(3);

            double side = radius * triangle_vertex * sqrt3;
            double x0 = radius - side * 0.5;
            double y0 = radius + radius * triangle_vertex * 0.5;
            double t = 2d / sqrt3;

            double dy = s * v, dx = v - dy * 0.5;

            (double sx, double sy) = Utils.ColorPickerUtil.GetVisualScalingFactor(this);

            double x = x0 + dx * side + 0.5;
            double y = y0 - (dy * side / t) + 0.5;

            Point ring_center = new(
                radius + double.SinPi(h / 3.0) * ring_radius + 0.5,
                radius - double.CosPi(h / 3.0) * ring_radius + 0.5
            );
            Point tri_center = new(x, y);

            using (DrawingContext context = visual.RenderOpen()) {
                context.PushTransform(Utils.ColorPickerUtil.GetVisualScalingTransform(this));

                const double radius1 = 3, radius2 = 4;

                context.DrawEllipse(null, pen_black, ring_center, radius1 * sx, radius1 * sy);
                context.DrawEllipse(null, pen_white, ring_center, radius2 * sx, radius2 * sy);

                context.DrawEllipse(null, pen_black, tri_center, radius1 * sx, radius1 * sy);
                context.DrawEllipse(null, pen_white, tri_center, radius2 * sx, radius2 * sy);
            }

            bitmap.Render(visual);
            bitmap.Freeze();

            Image_Pointer.Source = bitmap;

            OnPropertyChanged(nameof(Image_Pointer));

            Debug.WriteLine($"{nameof(HSVColorPicker)} - {nameof(RenderPointer)}");
        }
    }
}
