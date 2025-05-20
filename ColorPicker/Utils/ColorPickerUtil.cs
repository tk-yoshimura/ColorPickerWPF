using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ColorPicker.Utils {
    internal static class ColorPickerUtil {
        public static (double dpiX, double dpiY) GetVisualDPI(Visual visual) {
            (double scaleX, double scaleY) = GetVisualScalingFactor(visual);

            const double dpi_base = 96d;

            return (scaleX * dpi_base, scaleY * dpi_base);
        }

        public static (double scaleX, double scaleY) GetVisualScalingFactor(Visual visual) {
            PresentationSource source = PresentationSource.FromVisual(visual);

            if (source == null || source.CompositionTarget == null) {
                return (1d, 1d);
            }

            return (
                source.CompositionTarget.TransformToDevice.M11,
                source.CompositionTarget.TransformToDevice.M22
            );
        }

        public static ScaleTransform GetVisualScalingTransform(Visual visual) {
            PresentationSource source = PresentationSource.FromVisual(visual);

            if (source == null || source.CompositionTarget == null) {
                return new ScaleTransform(1d, 1d);
            }

            return new ScaleTransform(
                1d / source.CompositionTarget.TransformToDevice.M11,
                1d / source.CompositionTarget.TransformToDevice.M22
            );
        }

        public static (double pixelWidth, double pixelHeight) GetPixelSize(FrameworkElement element) {
            (double scaleX, double scaleY) = GetVisualScalingFactor(element);

            double pixelWidth = element.ActualWidth * scaleX;
            double pixelHeight = element.ActualHeight * scaleY;

            return (pixelWidth, pixelHeight);
        }

        public static Point ScaleDPI(FrameworkElement element, Point pt) {
            (double scaleX, double scaleY) = GetVisualScalingFactor(element);

            double x = pt.X * scaleX;
            double y = pt.Y * scaleY;

            return new Point(x, y);
        }

        public static Point GetDPIScaledPosition(this MouseEventArgs e, FrameworkElement element) {
            return ScaleDPI(element, e.GetPosition(element));
        }

        public static double GetPixelMinSize(FrameworkElement element) {
            (double pixelWidth, double pixelHeight) = GetPixelSize(element);

            return double.Min(pixelWidth, pixelHeight);
        }
    }
}
