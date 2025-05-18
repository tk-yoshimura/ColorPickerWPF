using ColorPicker.ColorSpace;
using System.Windows;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class SaturationNumericBox : NumericBox {
        public SaturationNumericBox() : base() {
            ResolutionMode = NumericBoxResolutionMode.Percent;

            ValueChanged += NumericBox_ValueChanged;
        }

        #region SelectedColor
        protected static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(HSV),
                typeof(SaturationNumericBox),
                new FrameworkPropertyMetadata(
                    new HSV(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnColorChanged
                )
            );

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is SaturationNumericBox ctrl) {
                ctrl.SelectedColor = (HSV)e.NewValue;
            }
        }

        public HSV SelectedColor {
            get => (HSV)GetValue(SelectedColorProperty);
            set {
                SetValue(SelectedColorProperty, value);
                UpdateValue();
            }
        }

        protected void UpdateValue() {
            ValueChanged -= NumericBox_ValueChanged;
            Value = (int)(SelectedColor.S * MaxValue + 0.5);
            ValueChanged += NumericBox_ValueChanged;
        }
        #endregion

        #region ResolutionMode
        protected static readonly DependencyProperty ResolutionModeProperty =
            DependencyProperty.Register(
                nameof(ResolutionMode),
                typeof(NumericBoxResolutionMode),
                typeof(SaturationNumericBox),
                new FrameworkPropertyMetadata(
                    NumericBoxResolutionMode.Percent,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnResolutionModeChanged
                )
            );

        private static void OnResolutionModeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is SaturationNumericBox ctrl) {
                ctrl.ResolutionMode = (NumericBoxResolutionMode)e.NewValue;
            }
        }

        public NumericBoxResolutionMode ResolutionMode {
            get => (NumericBoxResolutionMode)GetValue(ResolutionModeProperty);
            set {
                SetValue(ResolutionModeProperty, value);

                MaxValue = (int)value;

                UpdateValue();
            }
        }
        #endregion

        private void NumericBox_ValueChanged(object sender, EventArgs e) {
            SelectedColor = new(SelectedColor.H, Value / (double)MaxValue, SelectedColor.V);
        }
    }
}
