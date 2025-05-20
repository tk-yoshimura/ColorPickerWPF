using ColorPicker.ColorSpace;
using System.Windows.Media;

namespace ColorPickerColorSpaceTest {
    [TestClass]
    public class RGBATest {
        [TestMethod]
        public void CreateTest() {
            RGBA rgb1 = new(-1.5, 2, 3, 4);
            Assert.AreEqual(-1.5, rgb1.R);
            Assert.AreEqual(2, rgb1.G);
            Assert.AreEqual(3, rgb1.B);
            Assert.AreEqual(4, rgb1.A);

            RGBA rgb2 = rgb1.Normalize;
            Assert.AreEqual(0, rgb2.R);
            Assert.AreEqual(1, rgb2.G);
            Assert.AreEqual(1, rgb2.B);
            Assert.AreEqual(1, rgb2.A);

            RGBA rgb3 = new() {
                R = 0.1,
                G = 0.2,
                B = 0.3,
                A = 0.4
            };

            Assert.AreEqual(0.1, rgb3.R);
            Assert.AreEqual(0.2, rgb3.G);
            Assert.AreEqual(0.3, rgb3.B);
            Assert.AreEqual(0.4, rgb3.A);

            (RGB cr1, double alpha1) = rgb3;

            Assert.AreEqual(0.1, cr1.R);
            Assert.AreEqual(0.2, cr1.G);
            Assert.AreEqual(0.3, cr1.B);
            Assert.AreEqual(0.4, alpha1);

            RGBA rgb4 = (cr1, alpha1);

            Assert.AreEqual(0.1, rgb4.R);
            Assert.AreEqual(0.2, rgb4.G);
            Assert.AreEqual(0.3, rgb4.B);
            Assert.AreEqual(0.4, rgb4.A);
        }

        [TestMethod]
        public void CastTest() {
            RGBA rgb1 = new(new RGB(0.1, 0.2, 0.3), 0.4);

            Color cr1 = (Color)rgb1;

            Assert.AreEqual(26, cr1.R);
            Assert.AreEqual(51, cr1.G);
            Assert.AreEqual(77, cr1.B);
            Assert.AreEqual(102, cr1.A);

            RGBA rgb2 = (RGBA)cr1;

            Assert.AreEqual(26 / 255d, rgb2.R);
            Assert.AreEqual(51 / 255d, rgb2.G);
            Assert.AreEqual(77 / 255d, rgb2.B);
            Assert.AreEqual(102 / 255d, rgb2.A);

            RGBA rgb3 = new(new RGB(-0.1, -0.2, 1.3), 1.4);

            Color cr2 = (Color)rgb3;

            Assert.AreEqual(0, cr2.R);
            Assert.AreEqual(0, cr2.G);
            Assert.AreEqual(255, cr2.B);
            Assert.AreEqual(255, cr2.A);
        }
    }
}
