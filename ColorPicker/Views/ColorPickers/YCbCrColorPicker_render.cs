using ColorPicker.ColorSpace;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public partial class YCbCrColorPicker {
        private void RenderCbCr(YCbCr color) {
            if (!IsValidSize) {
                return;
            }

            int size = PickerPixelSize, side = size - 1;
            double radius = side * 0.5, side_inv = 1d / side;

            static byte clip(double c) => (byte)double.Clamp(c, 0, 255);

            byte[] buf = new byte[checked(size * size * 4)];

            double cr_y = color.Y;

            Parallel.For(0, size, new ParallelOptions() { MaxDegreeOfParallelism = 8 }, (y) => {
                unsafe {
                    fixed (byte* c = buf) {
                        double cr = (y - radius) * side_inv;
                        for (int x = 0, i = y * size * 4; x < size; x++, i += 4) {
                            double cb = (x - radius) * side_inv;

                            double r = cr_y + YCbCr.Consts.ycbcr_to_rgb_m31 * cr;
                            double g = cr_y + YCbCr.Consts.ycbcr_to_rgb_m22 * cb + YCbCr.Consts.ycbcr_to_rgb_m32 * cr;
                            double b = cr_y + YCbCr.Consts.ycbcr_to_rgb_m23 * cb;

                            c[i] = clip(b * 255 + 0.5);
                            c[i + 1] = clip(g * 255 + 0.5);
                            c[i + 2] = clip(r * 255 + 0.5);
                            c[i + 3] = 255;

                            Debug.Assert(i + 3 < buf.Length);
                        }
                    }
                }
            });

            PixelFormat pixel_format = PixelFormats.Pbgra32;
            int stride = checked(size * pixel_format.BitsPerPixel + 7) / 8;
            (double dpi_x, double dpi_y) = Utils.ColorPickerUtil.GetVisualDPI(this);

            BitmapSource bitmap = BitmapSource.Create(size, size, dpi_x, dpi_y, pixel_format, null, buf, stride);

            bitmap.Freeze();

            ImageCbCr.Source = bitmap;

            OnPropertyChanged(nameof(ImageCbCr));

            Debug.WriteLine($"{nameof(YCbCrColorPicker)} - {nameof(RenderCbCr)}");
        }

        private void RenderPointer(YCbCr color) {
            if (!IsValidSize) {
                return;
            }

            int side = PickerPixelSize - 1;
            double radius = side * 0.5;

            (double dpi_x, double dpi_y) = Utils.ColorPickerUtil.GetVisualDPI(this);

            RenderTargetBitmap bitmap = new(PixelSize, PixelSize, dpi_x, dpi_y, PixelFormats.Pbgra32);

            (_, double cb, double cr) = color;

            DrawingVisual visual = new();

            Pen pen_white = new(new SolidColorBrush(Colors.White), 1);
            Pen pen_black = new(new SolidColorBrush(Colors.Black), 1);

            (double sx, double sy) = Utils.ColorPickerUtil.GetVisualScalingFactor(this);

            double x = cb * side + radius + MarginWidth * sx + 0.5;
            double y = cr * side + radius + MarginWidth * sy + 0.5;

            Point tri_center = new(x, y);

            using (DrawingContext context = visual.RenderOpen()) {
                context.PushTransform(Utils.ColorPickerUtil.GetVisualScalingTransform(this));

                const double radius1 = 3, radius2 = 4;
                context.DrawEllipse(null, pen_black, tri_center, radius1 * sx, radius1 * sy);
                context.DrawEllipse(null, pen_white, tri_center, radius2 * sx, radius2 * sy);
            }

            bitmap.Render(visual);
            bitmap.Freeze();

            ImagePointer.Source = bitmap;

            OnPropertyChanged(nameof(ImagePointer));

            Debug.WriteLine($"{nameof(YCbCrColorPicker)} - {nameof(RenderPointer)}");
        }
    }
}
