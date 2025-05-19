using ColorPicker.ColorSpace;
using System.Diagnostics;
using System.Windows;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class RedSlider : GraphicalSlider {
        public event EventHandler<RGBColorChangedEventArgs> RGBColorChanged;

        #region SelectedColor
        protected static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(RGB),
                typeof(RedSlider),
                new FrameworkPropertyMetadata(
                    new RGB(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnColorChanged
                )
            );

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is RedSlider ctrl) {
                ctrl.SetSelectedColor((RGB)e.NewValue, user_operation: false, internal_only: true);
            }
        }

        public RGB SelectedColor {
            get => (RGB)GetValue(SelectedColorProperty);
            set {
                SetSelectedColor(value, user_operation: false);
            }
        }

        protected override void SetValue(double value, bool user_operation, bool internal_only = false) {
            SetSelectedColor(new RGB(value, SelectedColor.G, SelectedColor.B), user_operation, internal_only);
            base.SetValue(value, user_operation, internal_only);
        }

        RGB prev_color = new();
        protected void SetSelectedColor(RGB color, bool user_operation, bool internal_only = false) {
            if (prev_color.G != color.G || prev_color.B != color.B) {
                if (prev_color.R != color.R) {
                    prev_color = color;
                    base.SetValue(color.R, user_operation);
                }
                else {
                    prev_color = color;
                }

                RenderTrack(color);

                if (!internal_only) {
                    SetValue(SelectedColorProperty, color);

                    RGBColorChanged?.Invoke(this, new RGBColorChangedEventArgs(SelectedColor, user_operation));
                }
            }
            else if (prev_color.R != color.R) {
                prev_color = color;

                base.SetValue(color.R, user_operation);

                if (!internal_only) {
                    SetValue(SelectedColorProperty, color);

                    RGBColorChanged?.Invoke(this, new RGBColorChangedEventArgs(SelectedColor, user_operation));
                }
            }
        }
        #endregion

        protected override void RenderSlider(int width, int height, byte[] buf, object parameter) {
            if (height < 1) {
                return;
            }

            parameter ??= SelectedColor;

            RGB color = (RGB)parameter;

            byte g = (byte)double.Clamp(color.G * 255 + 0.5, 0, 255);
            byte b = (byte)double.Clamp(color.B * 255 + 0.5, 0, 255);

            double scale = 255d / (width - 1);

            unsafe {
                fixed (byte* c = buf) {
                    for (int x = 0, i = 0; x < width; x++, i += 4) {
                        byte r = (byte)double.Clamp(x * scale, 0, 255);

                        c[i] = b;
                        c[i + 1] = g;
                        c[i + 2] = r;
                        c[i + 3] = 255;

                        Debug.Assert(i + 3 < buf.Length);
                    }
                }
            }

            Parallel.For(1, height, new ParallelOptions() { MaxDegreeOfParallelism = 4 }, (y) => {
                Array.Copy(buf, 0, buf, y * width * 4, width * 4);
            });
        }
    }
}
