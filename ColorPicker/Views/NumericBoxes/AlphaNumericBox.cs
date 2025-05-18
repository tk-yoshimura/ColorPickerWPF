using System.Windows;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class AlphaNumericBox : NumericBox {
        public AlphaNumericBox() : base() {
            ResolutionMode = NumericBoxResolutionMode.Byte;

            ValueChanged += NumericBox_ValueChanged;
        }

        #region SelectedAlpha
        protected static readonly DependencyProperty SelectedAlphaProperty =
            DependencyProperty.Register(
                nameof(SelectedAlpha),
                typeof(double),
                typeof(AlphaNumericBox),
                new FrameworkPropertyMetadata(
                    0d,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnAlphaChanged
                )
            );

        private static void OnAlphaChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is AlphaNumericBox ctrl) {
                ctrl.SelectedAlpha = (double)e.NewValue;
            }
        }

        public double SelectedAlpha {
            get => (double)GetValue(SelectedAlphaProperty);
            set {
                SetValue(SelectedAlphaProperty, value);
                UpdateValue();
            }
        }

        protected void UpdateValue() {
            ValueChanged -= NumericBox_ValueChanged;
            Value = (int)(SelectedAlpha * MaxValue + 0.5);
            ValueChanged += NumericBox_ValueChanged;
        }
        #endregion

        #region ResolutionMode
        protected static readonly DependencyProperty ResolutionModeProperty =
            DependencyProperty.Register(
                nameof(ResolutionMode),
                typeof(NumericBoxResolutionMode),
                typeof(AlphaNumericBox),
                new FrameworkPropertyMetadata(
                    NumericBoxResolutionMode.Byte,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnResolutionModeChanged
                )
            );

        private static void OnResolutionModeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is AlphaNumericBox ctrl) {
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
            SelectedAlpha = Value / (double)MaxValue;
        }
    }
}
