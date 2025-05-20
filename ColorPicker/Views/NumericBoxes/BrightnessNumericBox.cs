using ColorPicker.ColorSpace;
using System.Windows;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class BrightnessNumericBox : NumericBox {
        public BrightnessNumericBox() : base() {
            ResolutionMode = NumericBoxResolutionMode.Percent;

            ValueChanged += NumericBox_ValueChanged;
        }

        #region SelectedColor
        protected static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(HSV),
                typeof(BrightnessNumericBox),
                new FrameworkPropertyMetadata(
                    new HSV(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnColorChanged
                )
            );

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is BrightnessNumericBox ctrl) {
                ctrl.SetSelectedColor((HSV)e.NewValue, internal_only: true);
            }
        }

        public HSV SelectedColor {
            get => (HSV)GetValue(SelectedColorProperty);
            set {
                SetSelectedColor(value);
            }
        }

        private HSV current_color = new();
        private void SetSelectedColor(HSV value, bool internal_only = false) {
            if (current_color.V != value.V) {
                current_color = value;
                UpdateValue(value.V);

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
                typeof(BrightnessNumericBox),
                new FrameworkPropertyMetadata(
                    NumericBoxResolutionMode.Percent,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnResolutionModeChanged
                )
            );

        private static void OnResolutionModeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is BrightnessNumericBox ctrl) {
                ctrl.ResolutionMode = (NumericBoxResolutionMode)e.NewValue;
            }
        }

        public NumericBoxResolutionMode ResolutionMode {
            get => (NumericBoxResolutionMode)GetValue(ResolutionModeProperty);
            set {
                SetValue(ResolutionModeProperty, value);

                MaxValue = (int)value;

                UpdateValue(SelectedColor.V);
            }
        }
        #endregion

        private void NumericBox_ValueChanged(object sender, EventArgs e) {
            SelectedColor = new(SelectedColor.H, SelectedColor.S, Value / (double)MaxValue);
        }
    }
}
