using ColorPicker.ColorSpace;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public partial class YCbCrColorPicker {
        private void RenderCbCr() {
            if (!IsValidSize) {
                return;
            }

            int size = PickerSize, side = size - 1;
            double radius = side * 0.5, side_inv = 1d / side;

            static byte clip(double c) => (byte)double.Clamp(c, 0, 255);

            byte[] buf = new byte[checked(size * size * 4)];

            double cr_y = SelectedColor.Y;

            unsafe {
                fixed (byte* c = buf) {
                    for (int x, y = 0, i = 0; y < size; y++) {
                        double cr = (y - radius) * side_inv;
                        for (x = 0; x < size; x++, i += 4) {
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
            }

            PixelFormat pixel_format = PixelFormats.Pbgra32;
            int stride = checked(size * pixel_format.BitsPerPixel + 7) / 8;
            (double dpi_x, double dpi_y) = Utils.ColorPickerUtil.GetVisualDPI(this);

            BitmapSource bitmap = BitmapSource.Create(size, size, dpi_x, dpi_y, pixel_format, null, buf, stride);

            bitmap.Freeze();

            Image_CbCr.Source = bitmap;

            OnPropertyChanged(nameof(Image_CbCr));
        }

        private void RenderPointer() {
            if (!IsValidSize) {
                return;
            }

            int side = PickerSize - 1;
            double radius = side * 0.5;

            (double dpi_x, double dpi_y) = Utils.ColorPickerUtil.GetVisualDPI(this);

            RenderTargetBitmap bitmap = new(Size, Size, dpi_x, dpi_y, PixelFormats.Pbgra32);

            (_, double cb, double cr) = SelectedColor;

            DrawingVisual visual = new();

            Pen pen_white = new(new SolidColorBrush(Colors.White), 1);
            Pen pen_black = new(new SolidColorBrush(Colors.Black), 1);

            double x = cb * side + radius + MarginWidth + 0.5;
            double y = cr * side + radius + MarginWidth + 0.5;

            Point tri_center = new(x, y);

            using (DrawingContext context = visual.RenderOpen()) {
                context.DrawEllipse(null, pen_black, tri_center, 3, 3);
                context.DrawEllipse(null, pen_white, tri_center, 4, 4);
            }

            bitmap.Render(visual);
            bitmap.Freeze();

            Image_Pointer.Source = bitmap;

            OnPropertyChanged(nameof(Image_Pointer));
        }
    }
}
