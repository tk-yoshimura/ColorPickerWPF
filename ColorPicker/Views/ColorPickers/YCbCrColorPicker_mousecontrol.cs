using ColorPicker.Utils;
using System.Windows;
using System.Windows.Input;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public partial class YCbCrColorPicker {
        private (double cb, double cr) Coord(Point pt) {
            int side = PickerPixelSize - 1;
            double radius = side * 0.5, side_inv = 1d / side;

            double cb = (pt.X - MarginWidth - radius) * side_inv;
            double cr = (pt.Y - MarginWidth - radius) * side_inv;

            return (cb, cr);
        }

        private void AcceptOperation(Point pt) {
            (double cb, double cr) = Coord(pt);
            cb = double.Clamp(cb, -0.5, 0.5);
            cr = double.Clamp(cr, -0.5, 0.5);

            if (double.IsNaN(cb) || double.IsNaN(cr)) {
                return;
            }

            SetSelectedColor(new ColorSpace.YCbCr(SelectedColor.Y, cb, cr), user_operation: true);
        }

        private void Grid_ColorPicker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Point pt = e.GetDPIScaledPosition(Grid_ColorPicker);
            AcceptOperation(pt);

            UIElement el = (UIElement)sender;

            if (!el.IsMouseCaptured) {
                el.CaptureMouse();
            }
        }

        private void Grid_ColorPicker_MouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton != MouseButtonState.Pressed) {
                UIElement el = (UIElement)sender;

                if (el.IsMouseCaptured) {
                    el.ReleaseMouseCapture();
                }

                return;
            }

            Point pt = e.GetDPIScaledPosition(Grid_ColorPicker);
            AcceptOperation(pt);
        }

        private void Grid_ColorPicker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            Point pt = e.GetDPIScaledPosition(Grid_ColorPicker);
            AcceptOperation(pt);

            UIElement el = (UIElement)sender;

            if (el.IsMouseCaptured) {
                el.ReleaseMouseCapture();
            }
        }
    }
}
