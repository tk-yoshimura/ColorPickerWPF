using System.Diagnostics;

// Copyright (c) T.Yoshimura 2025
// https://github.com/tk-yoshimura

namespace ColorPicker {
    public class GrayscaleSlider : GraphicalSlider {
        protected override void RenderSlider(int width, int height, byte[] buf, object parameter = null) {
            if (height < 1) {
                return;
            }

            double scale = 255d / (width - 1);

            unsafe {
                fixed (byte* c = buf) {
                    for (int x = 0, i = 0; x < width; x++, i += 4) {
                        byte v = (byte)double.Clamp(x * scale, 0, 255);

                        c[i] = v;
                        c[i + 1] = v;
                        c[i + 2] = v;
                        c[i + 3] = 255;

                        Debug.Assert(i + 3 < buf.Length);
                    }

                    for (int y = 1; y < height; y++) {
                        Array.Copy(buf, 0, buf, y * width * 4, width * 4);
                    }
                }
            }
        }
    }
}
