using ColorPicker.ColorSpace;
using System.Windows;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class SaturationNumericBox : NumericBox {
        public SaturationNumericBox() : base() {
            MaxValue = 100;

            ValueChanged += NumericBox_ValueChanged;
        }

        public static readonly DependencyProperty SelectedColorProperty =
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

                ValueChanged -= NumericBox_ValueChanged;
                Value = (int)(SelectedColor.S * 100 + 0.5);
                ValueChanged += NumericBox_ValueChanged;
            }
        }

        private void NumericBox_ValueChanged(object sender, EventArgs e) {
            SelectedColor = new(SelectedColor.H, Value / 100d, SelectedColor.V);
        }
    }
}
