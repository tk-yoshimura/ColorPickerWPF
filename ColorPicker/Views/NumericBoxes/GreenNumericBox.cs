using ColorPicker.ColorSpace;
using System.Windows;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class GreenNumericBox : NumericBox {
        public GreenNumericBox() : base() {
            MaxValue = 255;

            ValueChanged += NumericBox_ValueChanged;
        }

        #region SelectedColor
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(RGB),
                typeof(GreenNumericBox),
                new FrameworkPropertyMetadata(
                    new RGB(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnColorChanged
                )
            );

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is GreenNumericBox ctrl) {
                ctrl.SelectedColor = (RGB)e.NewValue;
            }
        }

        public RGB SelectedColor {
            get => (RGB)GetValue(SelectedColorProperty);
            set {
                SetValue(SelectedColorProperty, value);

                ValueChanged -= NumericBox_ValueChanged;
                Value = (int)(SelectedColor.G * 255 + 0.5);
                ValueChanged += NumericBox_ValueChanged;
            }
        }
        #endregion

        private void NumericBox_ValueChanged(object sender, EventArgs e) {
            SelectedColor = new(SelectedColor.R, Value / 255d, SelectedColor.B);
        }
    }
}
