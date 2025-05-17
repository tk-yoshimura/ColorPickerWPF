using ColorPicker.Utils;
using System.Windows;
using System.Windows.Input;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public partial class GraphicalSlider {
        private double Coord(Point pt) {
            int side = TrackPixelWidth - 1;
            double side_inv = 1d / side;

            double x = (pt.X - TrackMarginWidth) * side_inv;

            return x;
        }

        private void AcceptOperation(Point pt) {
            double x = Coord(pt);
            x = double.Clamp(x, 0, 1);

            if (double.IsNaN(x)) {
                return;
            }

            SetValue(x, user_operation: true);
        }

        private void Grid_Slider_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Point pt = e.GetDPIScaledPosition(Grid_Slider);
            AcceptOperation(pt);

            UIElement el = (UIElement)sender;

            if (!el.IsMouseCaptured) {
                el.CaptureMouse();
            }
        }

        private void Grid_Slider_MouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton != MouseButtonState.Pressed) {
                UIElement el = (UIElement)sender;

                if (el.IsMouseCaptured) {
                    el.ReleaseMouseCapture();
                }

                return;
            }

            Point pt = e.GetDPIScaledPosition(Grid_Slider);
            AcceptOperation(pt);
        }

        private void Grid_Slider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            Point pt = e.GetDPIScaledPosition(Grid_Slider);
            AcceptOperation(pt);

            UIElement el = (UIElement)sender;

            if (el.IsMouseCaptured) {
                el.ReleaseMouseCapture();
            }
        }
    }
}
