using ColorPicker.ColorSpace;
using System.Windows.Media;

namespace ColorPickerColorSpaceTest {
    [TestClass]
    public class RGBTest {
        [TestMethod]
        public void CreateTest() {
            RGB rgb1 = new(-1.5, 2, 3);
            Assert.AreEqual(-1.5, rgb1.R);
            Assert.AreEqual(2, rgb1.G);
            Assert.AreEqual(3, rgb1.B);

            RGB rgb2 = rgb1.Normalize;
            Assert.AreEqual(0, rgb2.R);
            Assert.AreEqual(1, rgb2.G);
            Assert.AreEqual(1, rgb2.B);

            RGB rgb3 = new() {
                R = 0.1,
                G = 0.2,
                B = 0.3
            };

            Assert.AreEqual(0.1, rgb3.R);
            Assert.AreEqual(0.2, rgb3.G);
            Assert.AreEqual(0.3, rgb3.B);
        }


        [TestMethod]
        public void CastTest() {
            RGB rgb1 = new(0.1, 0.2, 0.3);

            Color cr1 = (Color)rgb1;

            Assert.AreEqual(26, cr1.R);
            Assert.AreEqual(51, cr1.G);
            Assert.AreEqual(77, cr1.B);
            Assert.AreEqual(255, cr1.A);

            RGB rgb2 = (RGB)cr1;

            Assert.AreEqual(26 / 255d, rgb2.R);
            Assert.AreEqual(51 / 255d, rgb2.G);
            Assert.AreEqual(77 / 255d, rgb2.B);

            RGB rgb3 = new(-0.1, -0.2, 1.3);

            Color cr2 = (Color)rgb3;

            Assert.AreEqual(0, cr2.R);
            Assert.AreEqual(0, cr2.G);
            Assert.AreEqual(255, cr2.B);
            Assert.AreEqual(255, cr2.A);
        }
    }
}
