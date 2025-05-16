using System.Diagnostics;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class GrayscaleSlider : GraphicalSlider {
        protected override void RenderSlider(int width, int height, byte[] buf) {
            if (height < 1) {
                return;
            }

            double scale = 255d / (width - 1);

            unsafe {
                fixed (byte* c = buf) {
                    int i = 0;
                    for (int x = 0; x < width; x++, i += 4) {
                        byte v = (byte)double.Clamp(x * scale, 0, 255);

                        c[i] = v;
                        c[i + 1] = v;
                        c[i + 2] = v;
                        c[i + 3] = 255;

                        Debug.Assert(i + 3 < buf.Length);
                    }

                    for (int x, y = 1, j; y < height; y++) {
                        for (x = 0, j = 0; x < width; x++, i += 4, j += 4) {
                            c[i] = c[j];
                            c[i + 1] = c[j + 1];
                            c[i + 2] = c[j + 2];
                            c[i + 3] = c[j + 3];

                            Debug.Assert(i + 3 < buf.Length);
                            Debug.Assert(j + 3 < buf.Length);
                        }
                    }
                }
            }
        }
    }
}
