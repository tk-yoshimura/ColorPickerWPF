using ColorPicker.Utils;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public partial class HSVColorPicker {
        enum OperationPlace { None, Circle, Triangle };
        OperationPlace operation_place = OperationPlace.None;

        private bool CheckHitCircleArea(Point pt) {
            if (!IsValidSize) {
                return false;
            }

            (double norm, _) = CircleCoord(pt);

            bool inside = norm >= inner_ring && norm <= outer_ring;

            return inside;
        }

        private (double norm, double phase) CircleCoord(Point pt) {
            int size = PixelSize;
            double radius = size * 0.5;

            double dx = pt.X - radius, dy = pt.Y - radius;
            double norm = double.Hypot(dx, dy) / radius;
            double phase = (double.Atan2Pi(-dx, dy) + 1.0) * 3.0;

            return (norm, phase);
        }

        private bool CheckHitTriangleArea(Point pt) {
            if (!IsValidSize) {
                return false;
            }

            (double u, double v) = TriangleCoord(pt);

            bool inside = u >= 0 && v >= 0 && u + v <= 1;

            return inside;
        }

        private (double u, double v) TriangleCoord(Point pt) {
            int size = PixelSize;
            double radius = size * 0.5;

            double sqrt3 = double.Sqrt(3);

            double side = radius * triangle_vertex * sqrt3;
            double side_inv = 1 / side;
            double x0 = radius - side * 0.5;
            double y0 = radius + radius * triangle_vertex * 0.5;
            double sy = 2d / sqrt3;

            double dy = (y0 - pt.Y) * sy, v = dy * side_inv;
            double dx = pt.X - dy * 0.5 - x0, u = dx * side_inv;

            return (u, v);
        }

        private static (double sat, double val) TriangleCoordToSV((double u, double v) coord) {
            return TriangleCoordToSV(coord.u, coord.v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (double sat, double val) TriangleCoordToSV(double u, double v) {
            double val = double.Min(1d, u + v);
            double sat = double.Min(1d, (val > 0d) ? (v / val) : 0d);

            return (sat, val);
        }

        private void AcceptOperation(Point pt) {
            if (operation_place == OperationPlace.Circle) {
                (_, double phase) = CircleCoord(pt);

                if (double.IsNaN(phase)) {
                    return;
                }

                SetSelectedColor(new ColorSpace.HSV(phase, SelectedColor.S, SelectedColor.V), user_operation: true);
            }
            else if (operation_place == OperationPlace.Triangle) {
                (double sat, double val) = TriangleCoordToSV(TriangleCoord(pt));
                sat = double.Clamp(sat, 0, 1);
                val = double.Clamp(val, 0, 1);

                if (double.IsNaN(sat) || double.IsNaN(val)) {
                    return;
                }

                SetSelectedColor(new ColorSpace.HSV(SelectedColor.H, sat, val), user_operation: true);
            }
        }

        private void Grid_ColorPicker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Point pt = e.GetDPIScaledPosition(Grid_ColorPicker);

            if (CheckHitCircleArea(pt)) {
                operation_place = OperationPlace.Circle;
            }
            else if (CheckHitTriangleArea(pt)) {
                operation_place = OperationPlace.Triangle;
            }
            else {
                operation_place = OperationPlace.None;
            }

            if (operation_place != OperationPlace.None) {
                AcceptOperation(pt);
            }

            UIElement el = (UIElement)sender;

            if (!el.IsMouseCaptured) {
                el.CaptureMouse();
            }
        }

        private void Grid_ColorPicker_MouseMove(object sender, MouseEventArgs e) {

            if (e.LeftButton != MouseButtonState.Pressed) {
                operation_place = OperationPlace.None;

                UIElement el = (UIElement)sender;

                if (el.IsMouseCaptured) {
                    el.ReleaseMouseCapture();
                }
            }
            else if (operation_place != OperationPlace.None) {
                Point pt = e.GetDPIScaledPosition(Grid_ColorPicker);
                AcceptOperation(pt);
            }
        }

        private void Grid_ColorPicker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {

            if (operation_place != OperationPlace.None) {
                Point pt = e.GetDPIScaledPosition(Grid_ColorPicker);
                AcceptOperation(pt);
            }

            operation_place = OperationPlace.None;

            UIElement el = (UIElement)sender;

            if (el.IsMouseCaptured) {
                el.ReleaseMouseCapture();
            }
        }
    }
}
