using ColorPicker.ColorSpace;
using System.Windows;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class BrightnessNumericBox : NumericBox {
        public BrightnessNumericBox() : base() {
            MaxValue = 100;

            ValueChanged += NumericBox_ValueChanged;
        }

        #region SelectedColor
        public static readonly DependencyProperty SelectedColorProperty =
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
                ctrl.SelectedColor = (HSV)e.NewValue;
            }
        }

        public HSV SelectedColor {
            get => (HSV)GetValue(SelectedColorProperty);
            set {
                SetValue(SelectedColorProperty, value);

                ValueChanged -= NumericBox_ValueChanged;
                Value = (int)(SelectedColor.V * 100 + 0.5);
                ValueChanged += NumericBox_ValueChanged;
            }
        }
        #endregion

        private void NumericBox_ValueChanged(object sender, EventArgs e) {
            SelectedColor = new(SelectedColor.H, SelectedColor.S, Value / 100d);
        }
    }
}
