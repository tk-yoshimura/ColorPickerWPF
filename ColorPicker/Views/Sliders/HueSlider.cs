using ColorPicker.ColorSpace;
using System.Diagnostics;
using System.Windows;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class HueSlider : GraphicalSlider {
        private static readonly double scale6_dec = double.BitDecrement(6);

        public event EventHandler<HSVColorChangedEventArgs> HSVColorChanged;

        #region SelectedColor
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(HSV),
                typeof(HueSlider),
                new FrameworkPropertyMetadata(
                    new HSV(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnColorChanged
                )
            );

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is HueSlider ctrl) {
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
            SetSelectedColor(new HSV(Value * scale6_dec, SelectedColor.S, SelectedColor.V), user_operation);
        }

        protected void SetSelectedColor(HSV color, bool user_operation) {
            SetValue(SelectedColorProperty, color);

            base.SetValue(SelectedColor.H / scale6_dec, user_operation);

            HSVColorChanged?.Invoke(this, new HSVColorChangedEventArgs(SelectedColor, user_operation));
        }
        #endregion

        protected override void RenderSlider(int width, int height, byte[] buf, object parameter = null) {
            if (height < 1) {
                return;
            }

            double scale = 6d / (width - 1);

            unsafe {
                fixed (byte* c = buf) {
                    int i = 0;
                    for (int x = 0; x < width; x++, i += 4) {
                        double hue = double.Clamp(x * scale, 0, 6);

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
