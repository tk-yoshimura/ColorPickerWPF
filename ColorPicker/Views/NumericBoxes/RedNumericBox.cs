using ColorPicker.ColorSpace;
using System.Windows;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class RedNumericBox : NumericBox {
        public RedNumericBox() : base() {
            MaxValue = 255;

            ValueChanged += NumericBox_ValueChanged;
        }

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(RGB),
                typeof(RedNumericBox),
                new FrameworkPropertyMetadata(
                    new RGB(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnColorChanged
                )
            );

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is RedNumericBox ctrl) {
                ctrl.SelectedColor = (RGB)e.NewValue;
            }
        }

        public RGB SelectedColor {
            get => (RGB)GetValue(SelectedColorProperty);
            set {
                SetValue(SelectedColorProperty, value);

                ValueChanged -= NumericBox_ValueChanged;
                Value = (int)(SelectedColor.R * 255 + 0.5);
                ValueChanged += NumericBox_ValueChanged;
            }
        }

        private void NumericBox_ValueChanged(object sender, EventArgs e) {
            SelectedColor = new(Value / 255d, SelectedColor.G, SelectedColor.B);
        }
    }
}
