using System.Windows;
using System.Windows.Media;

namespace ColorPicker.Utils {
    internal static class ColorPickerUtil {
        public static (double dpiX, double dpiY) GetVisualDPI(this Visual visual) {
            const double dpi_base = 96;

            PresentationSource source = PresentationSource.FromVisual(visual);

            if (source == null || source.CompositionTarget == null) {
                return (dpi_base, dpi_base);
            }

            return (
                dpi_base * source.CompositionTarget.TransformToDevice.M11,
                dpi_base * source.CompositionTarget.TransformToDevice.M22
            );
        }
    }
}
