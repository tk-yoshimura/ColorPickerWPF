using ColorPicker.ColorSpace;
using System.Diagnostics;
using System.Windows;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class BrightnessSlider : GraphicalSlider {
        public event EventHandler<HSVColorChangedEventArgs> HSVColorChanged;

        #region SelectedColor
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(HSV),
                typeof(BrightnessSlider),
                new FrameworkPropertyMetadata(
                    new HSV(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnColorChanged
                )
            );

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is BrightnessSlider ctrl) {
                ctrl.SelectedColor = (HSV)e.NewValue;
            }
        }

        public HSV SelectedColor {
            get => (HSV)GetValue(SelectedColorProperty);
            set {
                SetSelectedColor(value, user_operation: false);
            }
        }

        protected override void SetValue(double value, bool user_operation) {
            base.SetValue(value, user_operation);
            SetSelectedColor(new HSV(SelectedColor.H, SelectedColor.S, Value), user_operation);
        }

        HSV prev_color = new();
        protected void SetSelectedColor(HSV color, bool user_operation) {
            if (prev_color.H != color.H || prev_color.S != color.S) {
                SetValue(SelectedColorProperty, color);
                RenderTrack();
            }
            else {
                SetValue(SelectedColorProperty, color);
            }

            base.SetValue(SelectedColor.V, user_operation);
            prev_color = color;

            HSVColorChanged?.Invoke(this, new HSVColorChangedEventArgs(SelectedColor, user_operation));
        }
        #endregion

        protected override void RenderSlider(int width, int height, byte[] buf) {
            if (height < 1) {
                return;
            }

            double scale = 1d / (width - 1);

            (double hue, double sat, _) = SelectedColor;

            unsafe {
                fixed (byte* c = buf) {
                    int i = 0;
                    for (int x = 0; x < width; x++, i += 4) {
                        double val = double.Clamp(x * scale, 0, 1);

                        (double r, double g, double b) = (RGB)new HSV(hue, sat, val);

                        c[i] = (byte)(b * 255 + 0.5);
                        c[i + 1] = (byte)(g * 255 + 0.5);
                        c[i + 2] = (byte)(r * 255 + 0.5);
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
