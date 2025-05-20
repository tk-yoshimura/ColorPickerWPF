using ColorPicker.ColorSpace;
using System.Windows;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class BlueNumericBox : NumericBox {
        public BlueNumericBox() : base() {
            ResolutionMode = NumericBoxResolutionMode.Byte;

            ValueChanged += NumericBox_ValueChanged;
        }

        #region SelectedColor
        protected static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(RGB),
                typeof(BlueNumericBox),
                new FrameworkPropertyMetadata(
                    new RGB(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnColorChanged
                )
            );

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is BlueNumericBox ctrl) {
                ctrl.SetSelectedColor((RGB)e.NewValue, internal_only: true);
            }
        }

        public RGB SelectedColor {
            get => (RGB)GetValue(SelectedColorProperty);
            set {
                SetSelectedColor(value);
            }
        }

        private RGB current_color = new();
        private void SetSelectedColor(RGB value, bool internal_only = false) {
            if (current_color.B != value.B) {
                current_color = value;
                UpdateValue(value.B);

                if (!internal_only) {
                    SetValue(SelectedColorProperty, value);
                }
            }
            else if (current_color != value) {
                current_color = value;

                if (!internal_only) {
                    SetValue(SelectedColorProperty, value);
                }
            }
        }

        protected void UpdateValue(double val) {
            ValueChanged -= NumericBox_ValueChanged;
            SetValue((int)(val * MaxValue + 0.5), internal_only: true);
            ValueChanged += NumericBox_ValueChanged;
        }
        #endregion

        #region ResolutionMode
        protected static readonly DependencyProperty ResolutionModeProperty =
            DependencyProperty.Register(
                nameof(ResolutionMode),
                typeof(NumericBoxResolutionMode),
                typeof(BlueNumericBox),
                new FrameworkPropertyMetadata(
                    NumericBoxResolutionMode.Byte,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnResolutionModeChanged
                )
            );

        private static void OnResolutionModeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is BlueNumericBox ctrl) {
                ctrl.ResolutionMode = (NumericBoxResolutionMode)e.NewValue;
            }
        }

        public NumericBoxResolutionMode ResolutionMode {
            get => (NumericBoxResolutionMode)GetValue(ResolutionModeProperty);
            set {
                SetValue(ResolutionModeProperty, value);

                MaxValue = (int)value;

                UpdateValue(SelectedColor.B);
            }
        }
        #endregion

        private void NumericBox_ValueChanged(object sender, EventArgs e) {
            SelectedColor = new(SelectedColor.R, SelectedColor.G, Value / (double)MaxValue);
        }
    }
}
