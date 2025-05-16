using System.Windows;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class AlphaNumericBox : NumericBox {
        public AlphaNumericBox() : base() {
            MaxValue = 255;

            ValueChanged += NumericBox_ValueChanged;
        }

        public static readonly DependencyProperty SelectedAlphaProperty =
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

                ValueChanged -= NumericBox_ValueChanged;
                Value = (int)(SelectedAlpha * 255 + 0.5);
                ValueChanged += NumericBox_ValueChanged;
            }
        }

        private void NumericBox_ValueChanged(object sender, EventArgs e) {
            SelectedAlpha = Value / 255d;
        }
    }
}
