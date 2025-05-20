using ColorPicker.ColorSpace;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public partial class PreviewPanel {

        protected void RenderPanel(RGB color, double alpha) {
            if (!IsValidSize) {
                return;
            }

            byte[] buf = new byte[checked(PanelWidth * PanelHeight * 4)];

            RenderPanel(PanelWidth, PanelHeight, buf, color, alpha);

            PixelFormat pixel_format = PixelFormats.Pbgra32;
            int stride = checked(PanelWidth * pixel_format.BitsPerPixel + 7) / 8;
            (double dpi_x, double dpi_y) = Utils.ColorPickerUtil.GetVisualDPI(this);

            BitmapSource bitmap = BitmapSource.Create(PanelWidth, PanelHeight, dpi_x, dpi_y, pixel_format, null, buf, stride);

            bitmap.Freeze();

            ImagePanel.Source = bitmap;

            OnPropertyChanged(nameof(ImagePanel));

            Debug.WriteLine($"{nameof(PreviewPanel)} - {nameof(RenderPanel)}");
        }

        protected virtual void RenderPanel(int width, int height, byte[] buf, RGB color, double alpha) {
            const double light_color = 0.75, dark_color = 0.25;

            if (height < 1) {
                return;
            }

            int block_size = BlockSize;
            double scale = 1d / (width - 1);

            (double r, double g, double b) = color.Normalize;
            alpha = double.Clamp(alpha, 0d, 1d);
            double alpha_c = 1 - alpha;

            byte[] buf0 = new byte[4], buf1 = new byte[4], buf2 = new byte[4];

            double r1 = r * alpha + dark_color * alpha_c;
            double g1 = g * alpha + dark_color * alpha_c;
            double b1 = b * alpha + dark_color * alpha_c;

            double r2 = r * alpha + light_color * alpha_c;
            double g2 = g * alpha + light_color * alpha_c;
            double b2 = b * alpha + light_color * alpha_c;

            buf0[0] = (byte)(b * 255d + 0.5);
            buf0[1] = (byte)(g * 255d + 0.5);
            buf0[2] = (byte)(r * 255d + 0.5);
            buf0[3] = 255;

            buf1[0] = (byte)(b1 * 255d + 0.5);
            buf1[1] = (byte)(g1 * 255d + 0.5);
            buf1[2] = (byte)(r1 * 255d + 0.5);
            buf1[3] = 255;

            buf2[0] = (byte)(b2 * 255d + 0.5);
            buf2[1] = (byte)(g2 * 255d + 0.5);
            buf2[2] = (byte)(r2 * 255d + 0.5);
            buf2[3] = 255;

            unsafe {
                fixed (byte* c = buf, c0 = buf0, c1 = buf1, c2 = buf2) {
                    for (int x, y = 0, i = 0; y < height; y++) {
                        for (x = 0; x < width; x++, i += 4) {
                            if (x <= y) {
                                byte* d = (((x / block_size) & 1) ^ ((y / block_size) & 1)) == 0 ? c1 : c2;

                                if (x < y) {
                                    c[i] = d[0];
                                    c[i + 1] = d[1];
                                    c[i + 2] = d[2];
                                    c[i + 3] = d[3];
                                }
                                else {
                                    c[i] = (byte)(d[0] / 2 + c0[0] / 2);
                                    c[i + 1] = (byte)(d[1] / 2 + c0[1] / 2);
                                    c[i + 2] = (byte)(d[2] / 2 + c0[2] / 2);
                                    c[i + 3] = 255;
                                }
                            }
                            else {
                                c[i] = c0[0];
                                c[i + 1] = c0[1];
                                c[i + 2] = c0[2];
                                c[i + 3] = c0[3];
                            }

                            Debug.Assert(i + 3 < buf.Length);
                        }
                    }
                }
            }
        }
    }
}
