using ColorPicker.ColorSpace;
using System.Windows.Media;

namespace ColorPickerColorSpaceTest {
    [TestClass]
    public class PaletteTest {
        [TestMethod]
        public void OstwaldTest() {
            RGB[] colors = [
                (RGB)Color.FromRgb(0xFD, 0x1B, 0x1B),
                (RGB)Color.FromRgb(0xFE, 0x41, 0x18),
                (RGB)Color.FromRgb(0xFF, 0x59, 0x0B),
                (RGB)Color.FromRgb(0xFF, 0x7F, 0x00),
                (RGB)Color.FromRgb(0xFF, 0xCC, 0x00),
                (RGB)Color.FromRgb(0xFF, 0xE6, 0x00),
                (RGB)Color.FromRgb(0xCC, 0xE7, 0x00),
                (RGB)Color.FromRgb(0x99, 0xCF, 0x15),
                (RGB)Color.FromRgb(0x66, 0xB8, 0x2B),
                (RGB)Color.FromRgb(0x33, 0xA2, 0x3D),
                (RGB)Color.FromRgb(0x00, 0x8F, 0x62),
                (RGB)Color.FromRgb(0x00, 0x86, 0x78),
                (RGB)Color.FromRgb(0x00, 0x7A, 0x87),
                (RGB)Color.FromRgb(0x05, 0x5D, 0x87),
                (RGB)Color.FromRgb(0x09, 0x3F, 0x86),
                (RGB)Color.FromRgb(0x0F, 0x21, 0x8B),
                (RGB)Color.FromRgb(0x1D, 0x1A, 0x88),
                (RGB)Color.FromRgb(0x28, 0x12, 0x85),
                (RGB)Color.FromRgb(0x34, 0x0C, 0x81),
                (RGB)Color.FromRgb(0x56, 0x00, 0x7D),
                (RGB)Color.FromRgb(0x77, 0x00, 0x71),
                (RGB)Color.FromRgb(0xAF, 0x00, 0x65),
                (RGB)Color.FromRgb(0xD4, 0x00, 0x45),
                (RGB)Color.FromRgb(0xEE, 0x00, 0x26),
            ];

            foreach (double t in new[] { 0, 0.25, 0.45, 0.6 }) {
                foreach (RGB color in colors) {
                    Color cr = (Color)((1 - t) * color + t * new RGB(1d, 1d, 1d));

                    Console.WriteLine($"Color.FromRgb(0x{cr.R:X2}, 0x{cr.G:X2}, 0x{cr.B:X2}),");
                }
            }

            foreach (double t in new[] { 0.25, 0.45, 0.6 }) {
                foreach (RGB color in colors) {
                    Color cr = (Color)((1 - t) * color);

                    Console.WriteLine($"Color.FromRgb(0x{cr.R:X2}, 0x{cr.G:X2}, 0x{cr.B:X2}),");
                }
            }

            for (int i = 0; i <= 23; i++) {
                double t = i / 23d;

                Color cr = (Color)new RGB(t, t, t);

                Console.WriteLine($"Color.FromRgb(0x{cr.R:X2}, 0x{cr.G:X2}, 0x{cr.B:X2}),");
            }
        }
    }
}
