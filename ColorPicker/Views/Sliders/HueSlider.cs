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
        protected static readonly DependencyProperty SelectedColorProperty =
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
                ctrl.SetSelectedColor((HSV)e.NewValue, user_operation: false, internal_only: true);
            }
        }

        public HSV SelectedColor {
            get => (HSV)GetValue(SelectedColorProperty);
            set {
                SetSelectedColor(value, user_operation: false);
            }
        }

        protected override void SetValue(double value, bool user_operation, bool internal_only = false) {
            double hue = value * scale6_dec;

            if (HueConversionMode == HueConversionMode.OstwaldPerceptual) {
                hue = OstwaldHue.Value(hue);
            }

            SetSelectedColor(new HSV(hue, SelectedColor.S, SelectedColor.V), user_operation, internal_only);
            base.SetValue(value, user_operation, internal_only);
        }

        HSV current_color = new();
        protected void SetSelectedColor(HSV color, bool user_operation, bool internal_only = false) {
            if (current_color.H != color.H) {
                current_color = color;

                double hue = color.H;

                if (HueConversionMode == HueConversionMode.OstwaldPerceptual) {
                    hue = OstwaldHue.InvertValue(hue);
                }

                base.SetValue(hue / scale6_dec, user_operation);

                if (!internal_only) {
                    SetValue(SelectedColorProperty, color);

                    HSVColorChanged?.Invoke(this, new HSVColorChangedEventArgs(color, user_operation));
                }
            }
            else if (current_color != color) {
                current_color = color;

                if (!internal_only) {
                    SetValue(SelectedColorProperty, color);

                    HSVColorChanged?.Invoke(this, new HSVColorChangedEventArgs(color, user_operation));
                }
            }
        }
        #endregion

        #region HueConversionMode
        protected static readonly DependencyProperty HueConversionModeProperty =
            DependencyProperty.Register(
                nameof(HueConversionMode),
                typeof(HueConversionMode),
                typeof(HueSlider),
                new FrameworkPropertyMetadata(
                    HueConversionMode.OstwaldPerceptual,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnHueConversionModeChanged
                )
            );

        private static void OnHueConversionModeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is HueSlider ctrl) {
                ctrl.HueConversionMode = (HueConversionMode)e.NewValue;
            }
        }

        private HueConversionMode hue_conversion_mode = HueConversionMode.OstwaldPerceptual;
        public HueConversionMode HueConversionMode {
            get => hue_conversion_mode;
            set {
                if (hue_conversion_mode != value) {
                    hue_conversion_mode = value;

                    SetValue(HueConversionModeProperty, value);

                    double hue = SelectedColor.H;

                    if (HueConversionMode == HueConversionMode.OstwaldPerceptual) {
                        hue = OstwaldHue.InvertValue(hue);
                    }

                    base.SetValue(hue / scale6_dec, user_operation: false);

                    RenderAllImages();
                }
            }
        }
        #endregion

        #region Render
        protected override void RenderSlider(int width, int height, byte[] buf, object parameter = null) {
            if (height < 1) {
                return;
            }

            double scale = 6d / (width - 1);

            bool is_standard_hue = HueConversionMode == HueConversionMode.RGBStandard;

            unsafe {
                fixed (byte* c = buf) {
                    for (int x = 0, i = 0; x < width; x++, i += 4) {
                        double hue = double.Clamp(x * scale, 0, 6);

                        if (!is_standard_hue) {
                            hue = OstwaldHue.Value(hue);
                        }

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
                }
            }

            Parallel.For(1, height, new ParallelOptions() { MaxDegreeOfParallelism = 4 }, (y) => {
                Array.Copy(buf, 0, buf, y * width * 4, width * 4);
            });
        }
        #endregion
    }
}
