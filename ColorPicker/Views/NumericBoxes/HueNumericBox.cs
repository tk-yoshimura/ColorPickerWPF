using ColorPicker.ColorSpace;
using System.Windows;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class HueNumericBox : NumericBox {
        public HueNumericBox() : base() {
            MaxValue = 360;

            ValueChanged += NumericBox_ValueChanged;
        }

        #region SelectedColor
        protected static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(HSV),
                typeof(HueNumericBox),
                new FrameworkPropertyMetadata(
                    new HSV(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnColorChanged
                )
            );

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (obj is HueNumericBox ctrl) {
                ctrl.SelectedColor = (HSV)e.NewValue;
            }
        }

        private HSV prev_color = new();
        public HSV SelectedColor {
            get => (HSV)GetValue(SelectedColorProperty);
            set {
                if (prev_color.H != value.H) {
                    prev_color = value;
                    UpdateValue(value.H);
                    SetValue(SelectedColorProperty, value);
                }
                else if (prev_color != value) {
                    prev_color = value;
                    SetValue(SelectedColorProperty, value);
                }
            }
        }

        protected void UpdateValue(double val) {
            base.UpdateValue((int)(val * 60d + 0.5));
        }
        #endregion

        private void NumericBox_ValueChanged(object sender, EventArgs e) {
            SelectedColor = new(Value / double.BitIncrement(60d), SelectedColor.S, SelectedColor.V);
        }
    }
}
