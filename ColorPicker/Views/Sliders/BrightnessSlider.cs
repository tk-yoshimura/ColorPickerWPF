﻿using ColorPicker.ColorSpace;
using System.Diagnostics;
using System.Windows;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class BrightnessSlider : GraphicalSlider {

        public event EventHandler<HSVColorChangedEventArgs> HSVColorChanged;

        #region SelectedColor
        protected static readonly DependencyProperty SelectedColorProperty =
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
            SetSelectedColor(new HSV(SelectedColor.H, SelectedColor.S, value), user_operation, internal_only);
            base.SetValue(value, user_operation, internal_only);
        }

        HSV current_color = new();
        protected void SetSelectedColor(HSV color, bool user_operation, bool internal_only = false) {
            if (current_color.H != color.H || current_color.S != color.S) {
                if (current_color.V != color.V) {
                    current_color = color;
                    base.SetValue(color.V, user_operation, internal_only);
                }
                else {
                    current_color = color;
                }

                RenderTrack(color);

                if (!internal_only) {
                    SetValue(SelectedColorProperty, color);

                    HSVColorChanged?.Invoke(this, new HSVColorChangedEventArgs(color, user_operation));
                }
            }
            else if (current_color.V != color.V) {
                current_color = color;

                base.SetValue(color.V, user_operation, internal_only);

                if (!internal_only) {
                    SetValue(SelectedColorProperty, color);

                    HSVColorChanged?.Invoke(this, new HSVColorChangedEventArgs(color, user_operation));
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

            HSV color = (HSV)parameter;

            double scale = 1d / (width - 1);

            (double hue, double sat, _) = color;

            unsafe {
                fixed (byte* c = buf) {
                    for (int x = 0, i = 0; x < width; x++, i += 4) {
                        double val = double.Clamp(x * scale, 0, 1);

                        (double r, double g, double b) = (RGB)new HSV(hue, sat, val);

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
