using ColorPicker.ColorSpace;
using System.Diagnostics;
using System.Windows;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class AlphaSlider : GraphicalSlider {

        #region SelectedColor
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(RGB),
                typeof(AlphaSlider),
                new FrameworkPropertyMetadata(
                    new RGB(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnColorChanged
                )
            );

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is AlphaSlider ctrl) {
                ctrl.SelectedColor = (RGB)e.NewValue;
            }
        }

        public RGB SelectedColor {
            get => (RGB)GetValue(SelectedColorProperty);
            set {
                SetSelectedColor(value);
            }
        }

        private RGB prev_color = new();
        protected void SetSelectedColor(RGB color) {
            if (prev_color.R != color.R || prev_color.G != color.G || prev_color.B != color.B) {
                prev_color = color;
                RenderTrack();
            }

            SetValue(SelectedColorProperty, color);
        }
        #endregion

        protected override void RenderSlider(int width, int height, byte[] buf) {
            const int block_size = 4;
            const double light_color = 0.75, dark_color = 0.25;

            if (height < 1) {
                return;
            }

            double scale = 1d / (width - 1);

            (double r, double g, double b) = SelectedColor.Normalize;

            byte[] buf1 = new byte[checked(width * 4)], buf2 = new byte[checked(width * 4)];

            unsafe {
                fixed (byte* c1 = buf1, c2 = buf2) {
                    int i = 0;
                    for (int x = 0; x < width; x++, i += 4) {
                        double alpha = double.Clamp(x * scale, 0, 1), alpha_c = 1 - alpha;

                        double r1 = r * alpha + dark_color * alpha_c;
                        double g1 = g * alpha + dark_color * alpha_c;
                        double b1 = b * alpha + dark_color * alpha_c;

                        double r2 = r * alpha + light_color * alpha_c;
                        double g2 = g * alpha + light_color * alpha_c;
                        double b2 = b * alpha + light_color * alpha_c;

                        c1[i] = (byte)(b1 * 255 + 0.5);
                        c1[i + 1] = (byte)(g1 * 255 + 0.5);
                        c1[i + 2] = (byte)(r1 * 255 + 0.5);
                        c1[i + 3] = 255;

                        c2[i] = (byte)(b2 * 255 + 0.5);
                        c2[i + 1] = (byte)(g2 * 255 + 0.5);
                        c2[i + 2] = (byte)(r2 * 255 + 0.5);
                        c2[i + 3] = 255;

                        Debug.Assert(i + 3 < buf1.Length);
                    }
                }

                fixed (byte* c = buf, c1 = buf1, c2 = buf2) {
                    for (int x, y = 0, i = 0, j; y < height; y++) {
                        for (x = 0, j = 0; x < width; x++, i += 4, j += 4) {
                            byte* d = (((x / block_size) & 1) ^ ((y / block_size) & 1)) == 0 ? c1 : c2;

                            c[i] = d[j];
                            c[i + 1] = d[j + 1];
                            c[i + 2] = d[j + 2];
                            c[i + 3] = d[j + 3];

                            Debug.Assert(i + 3 < buf.Length);
                            Debug.Assert(j + 3 < buf1.Length);
                        }
                    }
                }
            }
        }
    }
}
