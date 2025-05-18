using ColorPicker.ColorSpace;
using System.Diagnostics;
using System.Windows;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class GreenSlider : GraphicalSlider {
        public event EventHandler<RGBColorChangedEventArgs> RGBColorChanged;

        #region SelectedColor
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(RGB),
                typeof(GreenSlider),
                new FrameworkPropertyMetadata(
                    new RGB(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnColorChanged
                )
            );

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is GreenSlider ctrl) {
                ctrl.SelectedColor = (RGB)e.NewValue;
            }
        }

        public RGB SelectedColor {
            get => (RGB)GetValue(SelectedColorProperty);
            set {
                SetSelectedColor(value, user_operation: false);
            }
        }

        protected override void SetValue(double value, bool user_operation) {
            base.SetValue(value, user_operation);
            SetSelectedColor(new RGB(SelectedColor.R, Value, SelectedColor.B), user_operation);
        }

        RGB prev_color = new();
        protected void SetSelectedColor(RGB color, bool user_operation) {
            if (prev_color.R != color.R || prev_color.B != color.B) {
                prev_color = color;

                SetValue(SelectedColorProperty, color);
                RenderTrack();
            }
            else {
                prev_color = color;

                SetValue(SelectedColorProperty, color);
            }

            base.SetValue(SelectedColor.G, user_operation);

            RGBColorChanged?.Invoke(this, new RGBColorChangedEventArgs(SelectedColor, user_operation));
        }
        #endregion

        protected override void RenderSlider(int width, int height, byte[] buf) {
            if (height < 1) {
                return;
            }

            byte r = (byte)double.Clamp(SelectedColor.R * 255 + 0.5, 0, 255);
            byte b = (byte)double.Clamp(SelectedColor.B * 255 + 0.5, 0, 255);

            double scale = 255d / (width - 1);

            unsafe {
                fixed (byte* c = buf) {
                    int i = 0;
                    for (int x = 0; x < width; x++, i += 4) {
                        byte g = (byte)double.Clamp(x * scale, 0, 255);

                        c[i] = b;
                        c[i + 1] = g;
                        c[i + 2] = r;
                        c[i + 3] = 255;

                        Debug.Assert(i + 3 < buf.Length);
                    }

                    for (int x, y = 1, j; y < height; y++) {
                        for (x = 0, j = 0; x < width; x++, i += 4, j += 4) {
                            c[i] = c[j];
                            c[i + 1] = c[j + 1];
                            c[i + 2] = c[j + 2];
                            c[i + 3] = c[j + 3];

                            Debug.Assert(i + 3 < buf.Length);
                            Debug.Assert(j + 3 < buf.Length);
                        }
                    }
                }
            }
        }
    }
}
