using ColorPicker.ColorSpace;
using System.Diagnostics;
using System.Windows;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class GreenSlider : GraphicalSlider {

        public event EventHandler<RGBColorChangedEventArgs> RGBColorChanged;

        #region SelectedColor
        protected static readonly DependencyProperty SelectedColorProperty =
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
            SetSelectedColor(new RGB(SelectedColor.R, value, SelectedColor.B), user_operation, internal_only);
            base.SetValue(value, user_operation, internal_only);
        }

        RGB current_color = new();
        protected void SetSelectedColor(RGB color, bool user_operation, bool internal_only = false) {
            if (current_color.R != color.R || current_color.B != color.B) {
                if (current_color.G != color.G) {
                    current_color = color;
                    base.SetValue(color.G, user_operation, internal_only);
                }
                else {
                    current_color = color;
                }

                RenderTrack(color);

                if (!internal_only) {
                    SetValue(SelectedColorProperty, color);

                    RGBColorChanged?.Invoke(this, new RGBColorChangedEventArgs(color, user_operation));
                }
            }
            else if (current_color.G != color.G) {
                current_color = color;

                base.SetValue(color.G, user_operation, internal_only);

                if (!internal_only) {
                    SetValue(SelectedColorProperty, color);

                    RGBColorChanged?.Invoke(this, new RGBColorChangedEventArgs(color, user_operation));
                }
            }
        }
        #endregion

        #region Render
        protected override void RenderSlider(int width, int height, byte[] buf, object parameter) {
            if (height < 1) {
                return;
            }

            parameter ??= SelectedColor;

            RGB color = (RGB)parameter;

            byte r = (byte)double.Clamp(color.R * 255 + 0.5, 0, 255);
            byte b = (byte)double.Clamp(color.B * 255 + 0.5, 0, 255);

            double scale = 255d / (width - 1);

            unsafe {
                fixed (byte* c = buf) {
                    for (int x = 0, i = 0; x < width; x++, i += 4) {
                        byte g = (byte)double.Clamp(x * scale, 0, 255);

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
        #endregion
    }
}
