using ColorPicker.Utils;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public partial class GraphicalSlider {
        protected void RenderTrack() {
            if (!IsValidSize) {
                return;
            }

            byte[] buf = new byte[checked(TrackWidth * TrackHeight * 4)];

            RenderSlider(TrackWidth, TrackHeight, buf);

            PixelFormat pixel_format = PixelFormats.Pbgra32;
            int stride = checked(TrackWidth * pixel_format.BitsPerPixel + 7) / 8;
            (double dpi_x, double dpi_y) = Utils.ColorPickerUtil.GetVisualDPI(this);

            BitmapSource bitmap = BitmapSource.Create(TrackWidth, TrackHeight, dpi_x, dpi_y, pixel_format, null, buf, stride);

            bitmap.Freeze();

            Image_Track.Source = bitmap;

            OnPropertyChanged(nameof(Image_Track));
        }

        protected virtual void RenderSlider(int width, int height, byte[] buf) {
            unsafe {
                fixed (byte* c = buf) {
                    for (int x, y = 0, i = 0; y < height; y++) {
                        for (x = 0; x < width; x++, i += 4) {

                            c[i] = 128;
                            c[i + 1] = 128;
                            c[i + 2] = 128;
                            c[i + 3] = 255;

                            Debug.Assert(i + 3 < buf.Length);
                        }
                    }
                }
            }
        }

        protected void RenderPointer() {
            if (!IsValidSize) {
                return;
            }

            int side = TrackWidth - 1;

            (double dpi_x, double dpi_y) = Utils.ColorPickerUtil.GetVisualDPI(this);

            RenderTargetBitmap bitmap = new(checked((int)ActualWidth), checked((int)ActualHeight), dpi_x, dpi_y, PixelFormats.Pbgra32);

            DrawingVisual visual = new();

            Pen pen_white = new(new SolidColorBrush(Colors.White), 1);
            Brush brush_black = new SolidColorBrush(Colors.Black);

            double x = Value * side + TrackMarginWidth + 0.5;
            double y = TrackHeight + TrackMarginWidth;

            using (DrawingContext context = visual.RenderOpen()) {
                context.DrawPolygon(
                    brush_black, pen_white,
                    [new(x, y - ThumbSize.Height - 1),
                     new(x - ThumbSize.Width * 0.5, y - 1),
                     new(x + ThumbSize.Width * 0.5, y - 1)
                    ],
                    fill_rule: FillRule.Nonzero
                );
            }

            bitmap.Render(visual);
            bitmap.Freeze();

            Image_Pointer.Source = bitmap;

            OnPropertyChanged(nameof(Image_Pointer));
        }
    }
}
