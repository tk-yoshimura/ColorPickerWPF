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
            if (current_color.H != value.H) {
                current_color = value;
                UpdateValue(value.H);

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
            SetValue((int)(val * 60d + 0.5), internal_only: true);
            ValueChanged += NumericBox_ValueChanged;
        }
        #endregion

        private void NumericBox_ValueChanged(object sender, EventArgs e) {
            SelectedColor = new(Value / double.BitIncrement(60d), SelectedColor.S, SelectedColor.V);
        }
    }
}
