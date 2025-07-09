using ColorPicker.ColorSpace;

namespace ColorPickerColorSpaceTest {
    [TestClass]
    public class OstwaldHueTest {
        [TestMethod]
        public void ConvertTest() {
            for (double h = 0; h < 6; h += 1d / 32) {
                double u = OstwaldHue.Value(h);
                double v = OstwaldHue.InvertValue(u);

                Console.WriteLine(h);
                Console.WriteLine(u);
                Console.WriteLine(v);

                Assert.AreEqual(h, v, 1e-8);
            }
        }

        [TestMethod]
        public void CyclicTest() {
            for (double h = 0; h < 6; h += 1d / 32) {
                double u = OstwaldHue.Value(h - 6);
                double v = OstwaldHue.InvertValue(u);

                Console.WriteLine(h);
                Console.WriteLine(u);
                Console.WriteLine(v);

                Assert.AreEqual(h, v, 1e-8);
            }

            for (double h = 0; h < 6; h += 1d / 32) {
                double u = OstwaldHue.Value(h + 6);
                double v = OstwaldHue.InvertValue(u);

                Console.WriteLine(h);
                Console.WriteLine(u);
                Console.WriteLine(v);

                Assert.AreEqual(h, v, 1e-8);
            }

            for (double h = 0; h < 6; h += 1d / 32) {
                double u = OstwaldHue.Value(h);
                double v = OstwaldHue.InvertValue(u - 6);

                Console.WriteLine(h);
                Console.WriteLine(u);
                Console.WriteLine(v);

                Assert.AreEqual(h, v, 1e-8);
            }

            for (double h = 0; h < 6; h += 1d / 32) {
                double u = OstwaldHue.Value(h);
                double v = OstwaldHue.InvertValue(u + 6);

                Console.WriteLine(h);
                Console.WriteLine(u);
                Console.WriteLine(v);

                Assert.AreEqual(h, v, 1e-8);
            }
        }
    }
}
