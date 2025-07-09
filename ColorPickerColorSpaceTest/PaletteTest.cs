using ColorPicker.ColorSpace;
using System.Windows.Media;

namespace ColorPickerColorSpaceTest {
    [TestClass]
    public class PaletteTest {
        [TestMethod]
        public void OstwaldTest() {
            RGB[] colors = [
                (RGB)Color.FromRgb(0xFE, 0x00, 0x00),
                (RGB)Color.FromRgb(0xFF, 0x2C, 0x00),
                (RGB)Color.FromRgb(0xFF, 0x53, 0x00),
                (RGB)Color.FromRgb(0xFF, 0x80, 0x00),
                (RGB)Color.FromRgb(0xFF, 0xCD, 0x00),
                (RGB)Color.FromRgb(0xFF, 0xE6, 0x00),
                (RGB)Color.FromRgb(0xDA, 0xF5, 0x00),
                (RGB)Color.FromRgb(0xA7, 0xEC, 0x00),
                (RGB)Color.FromRgb(0x60, 0xE3, 0x00),
                (RGB)Color.FromRgb(0x00, 0xDA, 0x13),
                (RGB)Color.FromRgb(0x00, 0xD2, 0x90),
                (RGB)Color.FromRgb(0x00, 0xCF, 0xB8),
                (RGB)Color.FromRgb(0x00, 0xBC, 0xCF),
                (RGB)Color.FromRgb(0x00, 0x8B, 0xCF),
                (RGB)Color.FromRgb(0x00, 0x57, 0xCF),
                (RGB)Color.FromRgb(0x00, 0x1D, 0xD1),
                (RGB)Color.FromRgb(0x09, 0x00, 0xCF),
                (RGB)Color.FromRgb(0x28, 0x00, 0xCE),
                (RGB)Color.FromRgb(0x46, 0x00, 0xCD),
                (RGB)Color.FromRgb(0x8A, 0x00, 0xCB),
                (RGB)Color.FromRgb(0xC9, 0x00, 0xBC),
                (RGB)Color.FromRgb(0xDF, 0x00, 0x7F),
                (RGB)Color.FromRgb(0xEE, 0x00, 0x4E),
                (RGB)Color.FromRgb(0xF8, 0x00, 0x27),
            ];

            foreach (RGB color in colors) {
                HSV hsv = color;

                Console.WriteLine($"{hsv.H:0.000}");
            }

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
