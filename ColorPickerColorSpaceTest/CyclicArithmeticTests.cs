using ColorPicker.Utils;

namespace ColorPickerColorSpaceTest {
    [TestClass()]
    public class CyclicArithmeticTest {
        [TestMethod()]
        public void RemainderTest() {
            for (int i = -8; i <= 8; i++) {
                for (double h = 0; h < 6; h += 1d / 32) {
                    double u = CyclicArithmetic.Remainder(h + i * 6, 6);
                    Assert.AreEqual(h, u);
                }
            }
        }
    }
}